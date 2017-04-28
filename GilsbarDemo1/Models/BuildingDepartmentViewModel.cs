using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GilsbarDemo1.Models
{
    public class BuildingDepartmentViewModel
    {

        [Key]
        public int Id { get; set; }

        public int BuildingId { get; set; }

        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string Building_Name { get; set; }

        public int DepartmentId { get; set; }

        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string Department_Name { get; set; }

    } // public class BuildingDepartmentViewModel
}
