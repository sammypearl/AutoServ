using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Models;
using Persistence;
using Persistence.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly AutoServDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public VehicleViewModel VehicleVM { get; set; }

        public HomeController(AutoServDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;

            VehicleVM = new VehicleViewModel()
            {
                Makes = _db.Makes.ToList(),
                Models = _db.Models.ToList(),
              //  Sellers = _db.Sellers.ToList(),
                Vehicle = new Models.Vehicle()
                
            };
        }
        [AllowAnonymous]
        public IActionResult Index(string searchString, string sortOrder, int pageNumber = 1, int pageSize = 3)
        {
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.CurrentFilter = searchString;
            ViewBag.PriceSortParam = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            int ExcludeRecords = (pageSize * pageNumber) - pageSize;

            var Vehicles = from b in _db.Vehicles.Include(m => m.Make).Include(m => m.Model)
                           select b;

            var VehicleCount = Vehicles.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                Vehicles = Vehicles.Where(b => b.Make.Name.Contains(searchString));
                VehicleCount = Vehicles.Count();
            }

            //Sorting Logic
            switch (sortOrder)
            {
                case "price_desc":
                    Vehicles = Vehicles.OrderByDescending(b => b.Price);
                    break;
                default:
                    Vehicles = Vehicles.OrderBy(b => b.Price);
                    break;
            }

            Vehicles = Vehicles
            .Skip(ExcludeRecords)
                .Take(pageSize);

            var result = new PagedResult<Vehicle>
            {
                Data = Vehicles.AsNoTracking().ToList(),
                TotalItems = VehicleCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };


            return View(result);
        }
        
        [AllowAnonymous]
        [HttpGet]
        public IActionResult View(int id)
        {
            VehicleVM.Vehicle = _db.Vehicles.SingleOrDefault(b => b.Id == id);

            if (VehicleVM.Vehicle == null)
            {
                return NotFound();
            }
            return View(VehicleVM);
        }

        public IActionResult AboutUs()
        {

            return View();
        }

        public IActionResult Pimping()
        {

            return View();
        }

        public IActionResult AutoDiagnosis()
        {

            return View();
        }

        public IActionResult VehicleTracking()
        {

            return View();
        }

    }
}
