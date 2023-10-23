using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AirAidans.DATA.EF.Models;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using AirAidans.UI.MVC.Utilities;
using Microsoft.AspNetCore.Http;
using X.PagedList;

namespace AirAidans.Controllers
{
    public class ShoesController : Controller
    {
        #region Connection Objects

        private readonly AirAidansContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        public ShoesController(AirAidansContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        // GET: Shoes
        public async Task<IActionResult> Index()
        {
            var airAidansContext = _context.Shoes.Include(s => s.Category).Include(s => s.Supplier);
            return View(await airAidansContext.ToListAsync());


        }

        //Card View


        public async Task<IActionResult> ShoeCards(string searchTerm, int categoryId, int page = 1)
        {
            int pageSize = 6;

            var products = _context.Shoes .Include(s => s.Category).Include(s => s.Supplier).Include(s => s.Lockers)
                .ToList();

            #region Optional Category Filter

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewBag.Category = 0;

            if (categoryId != 0)
            {
                //If the user selected a Category...
                //(1) Filter the Products
                products = products.Where(p => p.CategoryId == categoryId).ToList();

                //(2) Repopulate the Dropdown with the chosen Category selected.
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", categoryId);

                //(3) Persist the Category in the ViewBag.
                ViewBag.Category = categoryId;
            }

            #endregion

            #region Optional Search Filter

            if (!String.IsNullOrEmpty(searchTerm))
            {
                //If we have a SearchTerm...
                products = products
                    .Where(s =>
                    s.Model.ToLower().Contains(searchTerm.ToLower())
                    || s.Supplier.SupplierName.ToLower().Contains(searchTerm.ToLower())
                    || s.Category.CategoryName.ToLower().Contains(searchTerm.ToLower())
                    || s.ShoeDescription.ToLower().Contains(searchTerm.ToLower()))
                    .ToList();
                ViewBag.NbrResults = products.Count;
                ViewBag.SearchTerm = searchTerm;
            }
            else
            {
                //If we don't have a SearchTerm...
                ViewBag.NbrResults = null;
                ViewBag.SearchTerm = null;
            }

            #endregion

            //return View(await products.ToListAsync());
            //return View(products);
            return View(products.ToPagedList(page, pageSize));

        }

        // GET: Shoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Shoes == null)
            {
                return NotFound();
            }

            var shoe = await _context.Shoes
                .Include(s => s.Category)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.ShoeId == id);
            if (shoe == null)
            {
                return NotFound();
            }

            return View(shoe);
        }

        // GET: Shoes/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName");
            return View();
        }

        // POST: Shoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create([Bind("ShoeId,Brand,Model,Size,Color,Sku,CategoryId,SupplierId,ShoeDescription,ShoeImage,Image,Price")] Shoe shoe)
        {

            if (ModelState.IsValid)
            {
                #region File Upload - CREATE
                //Check to see if a file was uploaded
                if (shoe.Image != null)
                {
                    //Check the file type 
                    //- retrieve the extension of the uploaded file
                    string ext = Path.GetExtension(shoe.Image.FileName);

                    //- Create a list of valid extensions to check against
                    string[] validExts = { ".jpeg", ".jpg", ".gif", ".png" };

                    //- verify the uploaded file has an extension matching one of the extensions in the list above
                    //- AND verify file size will work with our .NET app
                    if (validExts.Contains(ext.ToLower()) && shoe.Image.Length < 4_194_303)//underscores don't change the number, they just make it easier to read
                    {
                        //Generate a unique filename
                        shoe.ShoeImage = Guid.NewGuid() + ext;

                        //Save the file to the web server (here, saving to wwwroot/images)
                        //To access wwwroot, add a property to the controller for the _webHostEnvironment (see the top of this class for our example)
                        //Retrieve the path to wwwroot
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        //variable for the full image path --> this is where we will save the image
                        string FullImagePath = webRootPath + "/Assets/images/";

                        //Create a MemoryStream to read the image into the server memory
                        using (var memoryStream = new MemoryStream())
                        {
                            await shoe.Image.CopyToAsync(memoryStream);//transfer file from the request to server memory
                            using (var img = Image.FromStream(memoryStream))//add a using statement for the Image class (using System.Drawing)
                            {
                                //now, send the image to the ImageUtility for resizing and thumbnail creation
                                //items needed for the ImageUtility.ResizeImage()
                                //1) (int) maximum image size
                                //2) (int) maximum thumbnail image size
                                //3) (string) full path where the file will be saved
                                //4) (Image) an image
                                //5) (string) filename
                                int maxImageSize = 500;//in pixels
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(FullImagePath, shoe.ShoeImage, img, maxImageSize, maxThumbSize);
                                //myFile.Save("path/to/folder", "filename"); - how to save something that's NOT an image

                            }
                        }
                    }
                }
                else
                {
                    //If no image was uploaded, assign a default filename
                    //Will also need to download a default image and name it 'noimage.png' -> copy it to the /images folder
                    shoe.ShoeImage = "noimage.jpg";
                }

                #endregion

                _context.Add(shoe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", shoe.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", shoe.SupplierId);
            return View(shoe);
        }

        // GET: Shoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Shoes == null)
            {
                return NotFound();
            }

            var shoe = await _context.Shoes.FindAsync(id);
            if (shoe == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", shoe.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", shoe.SupplierId);
            return View(shoe);
        }

        // POST: Shoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ShoeId,Brand,Model,Size,Color,Sku,CategoryId,SupplierId,ShoeDescription,ShoeImage,Image,Price")] Shoe shoe)
        {
            if (id != shoe.ShoeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
                // The FK's are invalid causing changes to work. 
            {

                #region EDIT File Upload
                //retain old image file name so we can delete if a new file was uploaded
                string oldImageName = shoe.ShoeImage;

                //Check if the user uploaded a file
                if (shoe.Image != null)
                {
                    //get the file's extension
                    string ext = Path.GetExtension(shoe.Image.FileName);

                    //list valid extensions
                    string[] validExts = { ".jpeg", ".jpg", ".png", ".gif" };

                    //check the file's extension against the list of valid extensions
                    if (validExts.Contains(ext.ToLower()) && shoe.Image.Length < 4_194_303)
                    {
                        //generate a unique file name
                        shoe.ShoeImage = Guid.NewGuid() + ext;
                        //build our file path to save the image
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string fullPath = webRootPath + "/Assets/images/";

                        //Delete the old image
                        if (oldImageName != "noimage.png")
                        {
                            ImageUtility.Delete(fullPath, oldImageName);
                        }

                        //Save the new image to webroot
                        using (var memoryStream = new MemoryStream())
                        {
                            await shoe.Image.CopyToAsync(memoryStream);
                            using (var img = Image.FromStream(memoryStream))
                            {
                                int maxImageSize = 500;
                                int maxThumbSize = 100;
                                ImageUtility.ResizeImage(fullPath, shoe.ShoeImage, img, maxImageSize, maxThumbSize);
                            }
                        }

                    }
                }

              
                #endregion

                try
                {
                    _context.Update(shoe);
                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoeExists(shoe.ShoeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", shoe.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", shoe.SupplierId);
            return View(shoe);
        }

        // GET: Shoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Shoes == null)
            {
                return NotFound();
            }

            var shoe = await _context.Shoes
                .Include(s => s.Category)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.ShoeId == id);
            if (shoe == null)
            {
                return NotFound();
            }

            return View(shoe);
        }

        // POST: Shoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Shoes == null)
            {
                return Problem("Entity set 'AirAidansContext.Shoes'  is null.");
            }
            var shoe = await _context.Shoes.FindAsync(id);
            if (shoe != null)
            {
                _context.Shoes.Remove(shoe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoeExists(int id)
        {
          return (_context.Shoes?.Any(e => e.ShoeId == id)).GetValueOrDefault();
        }
    }
}
