using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Mvc;
using Models;
using Persistence;

namespace NetCoreNCapas.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _db;
        
        //constructor
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {           
            Repository<Cutomers> repo = new Repository<Cutomers>(_db);
            
            Cutomers cust = new Cutomers {
                Name = "Erik",
                FirstName = "Rodriguez",
                LastName = "Gallegos",
                Address = ".",
                PhoneNumber = "123456789"
            };

            //Add 
            repo.Add(cust);

            //edit
            cust.Name = "Antonio";
            repo.Edit(cust);

            //delete
            repo.Delete(cust);

            //get 
            List<Cutomers> list = repo.GetAll().ToList();

            //get by id
            var model = repo.GetById(1);

            //get by predicate
            var model2 = repo.GetByPredicate(x => x.Name == "Antonio").ToList();

            //get list form sp
            SqlParameter[] parametros =
            {
                new SqlParameter("@name", "erik")                
            };

            var model3 = repo.FunctionToList("SpGetCustomers", parametros).ToList();






            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
