using Microsoft.AspNetCore.Mvc;
using AirAidans.DATA.EF.Models;
using Microsoft.AspNetCore.Identity;
using AirAidans.Models;
using Newtonsoft.Json;

namespace AirAidans.Controllers
{
    public class ShoppingCartController : Controller
    {

         
        // Fields
        private readonly AirAidansContext _context;
        private readonly UserManager<IdentityUser> _userManager;
       

        //CTOR
        public ShoppingCartController(
            //Dependency injection (DI)
            AirAidansContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //retrieve the contents of the session cart

            //get the string stored in the session
            var sessionCart = HttpContext.Session.GetString("cart");

            //Create the local cart:
            Dictionary<int, CartItemViewModel> shoppingCart = null;
            if (sessionCart == null || sessionCart.Count() == 0)
            {
                //if it is empty we will create an empty cart
                shoppingCart = new Dictionary<int, CartItemViewModel>();

                //notify the user with a viewbag message
                ViewBag.Message = "There are no items in your cart";
            }
            else
            {
                //If the cart has items 
                //reset the message
                ViewBag.Message = null;

                //deserialize the cart 
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int,
                    CartItemViewModel>>(sessionCart);
            }

            return View(shoppingCart);
        }

        public IActionResult AddToCart(int id)
        {
            //Create an empty shell for a LOCAL shopping cart object.
            //our shopping cart will be a dictionary 
            //Where the key is teh Product ID (int)
            // and the value is the cartitemviewmodel (QTY & PRODUCT)
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            #region Session Notes
            /*
             * Session is a tool available on the server-side that can store information for a user while they are actively using your site.
             * Typically the session lasts for 20 minutes (this can be adjusted in Program.cs).
             * Once the 20 minutes is up, the session variable is disposed.
             * 
             * Values that we can store in Session are limited to: string, int
             * - Because of this we have to get creative when trying to store complex objects (like CartItemViewModel).
             * To keep the info separated into properties we will convert the C# object to a JSON string.
             * */
            #endregion

            var sessionCart = HttpContext.Session.GetString("cart");

            //Check to see if the session object exists
            //If not, instantiate a new collection 
            if (String.IsNullOrEmpty(sessionCart))
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }
            else
            {
                //if the cart already exists:
                //Transfer the existing cart items from the session to the local cart 


                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }

            //add the newly selected product(s) to the cart 

            //Retrieve the product details from the DB
            var shoe = _context.Shoes.Find(id);

            //Instantiate the object to add to the cart: 
            var cartItem = new CartItemViewModel(1, shoe);

            if (shoppingCart.ContainsKey(shoe.ShoeId))
            {
                //If the product is already in the cart, add 1 to the QTY 
                shoppingCart[shoe.ShoeId].Qty++;
            }
            else
            {
                //if the product in not in the cart, add the product
                shoppingCart.Add(shoe.ShoeId, cartItem);
            }

            //Update the session 
            //stringify the local cart 
            string jsonLocalCart = JsonConvert.SerializeObject(shoppingCart);
            //store it in local session
            HttpContext.Session.SetString("cart", jsonLocalCart);


            return RedirectToAction("Index");


        }

        public IActionResult RemoveFromCart(int id)
        {
            var sessionCart = HttpContext.Session.GetString("cart");

            var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            shoppingCart.Remove(id);

            if (shoppingCart.Count == 0)
            {
                //if there are no more items in cart, remove from the session
                HttpContext.Session.Remove("cart");
            }
            else // update the session
            {
                var JsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", JsonCart);
            }

            return RedirectToAction("Index");

        }

        public IActionResult UpdateCart(int productId, int qty)
        {
            var sessionCart = HttpContext.Session.GetString("cart");
            var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            shoppingCart[productId].Qty = qty;


            var JsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", JsonCart);

            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> SubmitOrder()
        //{
        //    #region Planning out Order Submission
        //    //BIG PICTURE PLAN
        //    //Create Order object -> then save to the DB
        //    //  - OrderDate
        //    //  - UserId
        //    //  - ShipToName, ShipCity, ShipState, ShipZip --> this info needs to be pulled from the UserDetails record
        //    //Add the record to _context
        //    //Save DB changes

        //    //Create OrderProducts object for each item in the cart
        //    //  - ProductId -> available from cart
        //    //  - OrderId -> from Order object
        //    //  - Qty -> available from cart
        //    //  - ProductPrice -> available from cart
        //    //Add the record to _context
        //    //Save DB changes
        //    #endregion

        //    var userId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;

        //    var userDetails = _context.UserDetails.Find(userId);

        //    //var order = new Locker()
        //    //{
        //    //    LockerId = lockerId,
        //    //    UserId = userId,
        //    //    Shoe_Id = shoeId
        //    //};

        //    //_context.Orders.Add(order);

        //    //var sessionCart = HttpContext.Session.GetString("cart");
        //    //var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

        //    //foreach (var item in shoppingCart)
        //    //{
        //    //    OrderProduct orderProduct = new OrderProduct()
        //    //    {
        //    //        OrderId = order.OrderId,
        //    //        ProductId = item.Key,
        //    //        ProductPrice = item.Value.Product.ProductPrice,
        //    //        Quantity = (short)item.Value.Qty
        //    //    };

        //    //    order.OrderProducts.Add(orderProduct);
        //    //}

        //    _context.SaveChanges();
        //    return RedirectToAction("Index", "Orders");
        //}
    }
}

