using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GilsbarDemo1.Models
{
    public class JobTaskPersons
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Job  Task")]
        public int JobTaskId { get; set; }
        [ForeignKey("JobTaskId")]
        [JsonIgnore]
        public virtual JobTask JobTask { get; set; }

        [Required]
        [Display(Name = "Person")]
        public int PersonId { get; set; }
        [ForeignKey("PersonId")]
        [JsonIgnore]
        public virtual Person Person { get; set; }

    } // public class JobTaskPersons
}
