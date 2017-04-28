using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GilsbarDemo1.Models
{
    public class JobTaskPersonViewModel
    {

        [Key]
        public int Id { get; set; }

        public int JobTaskId { get; set; }

        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string JobTask_Name { get; set; }

        public int PersonId { get; set; }

        [Display(Name = "Firstname")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string Person_Firstname { get; set; }

    } // public class JobTaskPersonViewModel
}
