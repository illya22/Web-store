using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Store.Lib.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage ="Enter your name!!!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter shipping address!!!")]
        public string Line1 { get; set; }
        [Required(ErrorMessage = "Enter Post oficce!!!")]
        public string Line2 { get; set; }
        [Required(ErrorMessage = "Enter Postcode!!!")]
        public string Line3 { get; set; }

        [Required(ErrorMessage = "Enter your city!!!")]
        public string City { get; set; }

        [Required(ErrorMessage = "Enter your Country!!!")]
        public string Country { get; set; }

        public bool Сash_on_delivery { get; set; }
        public bool Payment { get; set; }
    }
}
