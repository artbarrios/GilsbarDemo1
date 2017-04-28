using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GilsbarDemo1.Models
{
    public class JobTask
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string Description { get; set; }

        [Display(Name = "Image Filename")]
        [DataType(DataType.Text)]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string ImageFilename { get; set; }

        [JsonIgnore]
        public virtual List<Person> Persons { get; set; }

    } // public class JobTask
}
