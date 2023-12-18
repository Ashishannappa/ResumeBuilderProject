using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResumeBuilderProject.ViewModels
{
    public class WorkExperience
    {
       
        public int IDExp { get; set; }
        public string Company { get; set; }

       
        public string Title { get; set; }

       
        public string Country { get; set; }

       
        public Nullable<System.DateTime> FromYear { get; set; }

      
        public Nullable<System.DateTime> ToYear { get; set; }

      
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}