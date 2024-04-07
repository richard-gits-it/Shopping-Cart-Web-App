using System.ComponentModel.DataAnnotations;

namespace multiple_tables.models
{
    public class Payment
    {
        [Required(ErrorMessage = "Name on card is required")]
        [RegularExpression(@"^[a-zA-Z ]{2,50}$", ErrorMessage = "Invalid name on card")]
        public string NameOnCard { get; set; }

        [Required(ErrorMessage = "Card Number is required")]
        [RegularExpression(@"^\d{4}(\s?\d{4}){3}$", ErrorMessage = "Invalid card number")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Expiry Date is required")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/(2[4-9]|3[0-9])$", ErrorMessage = "Invalid expiry date")]
        public string ExpiryDate { get; set; }

        [Required(ErrorMessage = "CVV is required")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "Invalid CVV")]
        public string CVV { get; set; }
    }
}
