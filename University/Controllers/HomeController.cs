using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using University.Models;

namespace University.Controllers
{
    public class HomeController : Controller
    {
         [HttpGet("/")]
        public ActionResult Index()
        {   
            return View();
        }
    }
}
