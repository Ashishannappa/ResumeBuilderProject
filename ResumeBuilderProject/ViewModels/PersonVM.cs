using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResumeBuilderProject.ViewModels
{
    public class PersonVM
    {
        internal List<SelectListItem> ListNationality;

        public int IDPers { get; set; }

        
        public string FirstName { get; set; }
       

        public string LastName { get; set; }
       

        public Nullable<System.DateTime> DateOfBirth { get; set; }

      
        public string Nationality { get; set; }

  
      
        public string Address { get; set; }


        public string Tel { get; set; }

       
        public string Email { get; set; }
    }
}