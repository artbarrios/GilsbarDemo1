using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GilsbarDemo1.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Firstname")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string Firstname { get; set; }

        [Required]
        [Display(Name = "Lastname")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string Lastname { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string Email { get; set; }

        [Display(Name = "Home Phone")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string HomePhone { get; set; }

        [Display(Name = "Cell Phone")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string CellPhone { get; set; }

        [Display(Name = "Work Phone")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string WorkPhone { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Manager")]
        public int ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        [JsonIgnore]
        public virtual Manager Manager { get; set; }

        [Display(Name = "Flowchart Diagram Data")]
        [DataType(DataType.Text)]
        [StringLength(4000, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string JobFlowchartDiagramData { get; set; }

        [Display(Name = "Flowchart Diagram Data")]
        [DataType(DataType.Text)]
        [StringLength(4000, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string JobTaskFlowchartDiagramData { get; set; }

        [JsonIgnore]
        public virtual List<JobTask> JobTasks { get; set; }

    } // public class Person
}
