using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.Lib.Entities
{
      
   public class Part
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Part_Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage ="Please enter the name of the part")]
        public string Name { get; set; }

         [DataType(DataType.MultilineText)]
         [Display(Name = "Description")]
        [Required(ErrorMessage = "Please enter the description of the part")]
        public string Description { get; set; }

         [Display(Name = "Type")]
        [Required(ErrorMessage = "Please enter the type of the part")]
        public string Type { get; set; }

        [Display(Name = "Bran_Car")]
        [Required(ErrorMessage = "Please enter the car brand of the part")]
        public string Bran_Car { get; set; }

        [Display(Name = "Price($)")]
        [Required(ErrorMessage = "Please enter the acceptable price for the part")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter the acceptable price for the part")]
        public decimal Price { get; set; }

        public byte[] ImageData { get; set; }
         
        public string ImageType { get; set; }
    }
}
