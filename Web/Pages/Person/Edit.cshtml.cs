using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Pages.Models;

namespace Web.Pages.Person
{
    public class EditModel : PageModel
    {
        public PersonModel Person { get; set; }
        public IActionResult OnGet(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            Person = new PersonModel();
            Person.Id = 1;
            Person.Name = "adf";
            Person.Age = 11;
            return Page();
        }

        public IActionResult OnPost()
        {
            if(string.IsNullOrEmpty(Person.Name))
            {
                return new ContentResult();
            }

            return Page();
        }
    }
}