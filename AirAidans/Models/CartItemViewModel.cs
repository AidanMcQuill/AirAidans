using AirAidans.DATA.EF.Models;

namespace AirAidans.UI.MVC.Models
{
    public class CartItemViewModel
    {
        //Cart items
        public int Qty { get; set; }

        public Shoe Shoe { get; set; }

        //The above us an example of a concept called containment
        //We are using a complex datatype as a field/prop in a class
        //A complex datatype is any class with multiple props: datetime, product
        // a primitive datatype is any class with a single value: bool, int, short

        //CTRL + . => Generate constructor ... => Select props => OK
        public CartItemViewModel(int qty, Shoe shoe)
        {
            Qty = qty;
            Shoe = shoe;
        }
    }
}

