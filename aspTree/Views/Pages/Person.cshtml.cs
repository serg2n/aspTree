using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using aspTree.Models;
using aspTree.Pages;



namespace aspTree.Pages
{
    public class PersonModel : PageModel
    {
        List<Person> people;
        public Person Person { get; set; }
        public PersonModel()
        {
            people = new List<Person>()
            {
                new Person{ Name="Tom", Age=23},
                new Person {Name = "Sam", Age=25},
                new Person {Name="Bob", Age=23},
                new Person{Name="Tom", Age=25}
            };
        }

        public IActionResult OnGet(string name)
        {
            Person = people.FirstOrDefault(p => p.Name == name);
            if (Person == null)
                return RedirectToPage("Error");

            return Page();
        }
    }
}