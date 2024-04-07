using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace multiple_tables.models
{
    public class Orders
    {
        public int Id { get; set; }
        //make these 5 properties so it doesnt have to be validated
        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Ship Address is required")]
        public string ShipAddress { get; set; }

        [Required(ErrorMessage = "Ship City is required")]
        public string ShipCity { get; set; }

        [Required(ErrorMessage = "Province is required")]
        public string ShipProvince { get; set; }

        [Required(ErrorMessage = "Ship Postal Code is required")]
        [RegularExpression("^[ABCEGHJ-NPRSTVXY]\\d[ABCEGHJ-NPRSTV-Z][ -]?\\d[ABCEGHJ-NPRSTV-Z]\\d$", ErrorMessage = "Invalid Postal Code")]
        public string ShipPostalCode { get; set; }

        [Required(ErrorMessage = "Ship Country is required")]
        public string ShipCountry { get; set; }

        [Required(ErrorMessage = "Ship Phone is required")]
        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Invalid Phone Number")]
        public string ShipPhone { get; set; }

        [Required(ErrorMessage = "Ship Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string ShipEmail { get; set; }

        //initialize order details
        public Orders()
        {
            OrderDetails = new List<OrderDetails>();
        }

        //status of the order(pending, shipped, delivered) depending on the dates
        [NotMapped]
        public string Status
        {
            get
            {
                if (ShippedDate > DateTime.Now)
                {
                    return "Pending";
                }
                else if (ShippedDate < DateTime.Now)
                {
                    return "Shipped";
                }
                else
                {
                    return "Delivered";
                }
            }
        }

        //total price of the order, gst, shipping fee
        [NotMapped]
        public double TotalPrice
        {
            get
            {
                double totalPrice = 0;
                foreach (var orderDetail in OrderDetails)
                {
                    totalPrice += (double)orderDetail.UnitPrice * orderDetail.Quantity;
                }
                return totalPrice;
            }
        }
        [NotMapped]
        public double Gst
        {
            get
            {
                return TotalPrice * 0.05;
            }
        }
        [NotMapped]
        public double ShippingFee
        {
            get
            {
                return 5;
            }
        }
        [NotMapped]
        public double GrandTotal
        {
            get
            {
                return TotalPrice + Gst + ShippingFee;
            }
        }


    }
}
