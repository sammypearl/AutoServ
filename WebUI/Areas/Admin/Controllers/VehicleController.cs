using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Models;
using Persistence;
using Persistence.Helpers;
using Persistence.Repository.IRepository;
using Persistence.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Executive)]
    public class VehicleController : Controller
    {
        private readonly AutoServDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVehicleRepository _repository;

        [BindProperty]
        public VehicleViewModel VehicleVM { get; set; }

        public VehicleController(AutoServDbContext db, 
            IWebHostEnvironment hostingEnvironment,
            IUnitOfWork unitOfWork, IVehicleRepository repository)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            _unitOfWork = unitOfWork;
            _repository = repository;

            VehicleVM = new VehicleViewModel()
            {
                Makes = _db.Makes.ToList(),
                Models = _db.Models.ToList(),
                Sellers = _db.Sellers.ToList(),
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


        [HttpGet]
        public IActionResult Edit(int id)
        {
            VehicleVM.Vehicle = _db.Vehicles.SingleOrDefault(b => b.Id == id);

            //Filter the models associated to the selected make
            VehicleVM.Models = _db.Models.Where(m => m.MakeID == VehicleVM.Vehicle.MakeID);

            if (VehicleVM.Vehicle == null)
            {
                return NotFound();
            }
            return View(VehicleVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost(VehicleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Vehicle vehicle = _repository.Get(model.Vehicle.Id);

                VehicleVM.Makes = _db.Makes.ToList();
                VehicleVM.Models = _db.Models.ToList();


                return View(VehicleVM);
            }
            _db.Vehicles.Update(VehicleVM.Vehicle);
       
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View(VehicleVM);
        }


        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(VehicleViewModel VehicleVM)
        {
       
            if (!ModelState.IsValid)
            {
           
                 VehicleVM.Makes = _db.Makes.ToList();
                 VehicleVM.Models = _db.Models.ToList();
                 return View(VehicleVM);
            };
            _db.Vehicles.Add(VehicleVM.Vehicle);
             UploadImageIfAvailable();
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        private void UploadImageIfAvailable()
        {
            //Get VehicleID we have saved in database            
            var VehicleId = VehicleVM.Vehicle.Id;

            //Get wwrootPath to save the file on server
            string wwrootPath = _hostingEnvironment.WebRootPath;

            //Get the Uploaded files
            var files = HttpContext.Request.Form.Files;

            //Get the reference of DBSet for the Vehicle we have saved in our database
            var SavedVehicle = _db.Vehicles.Find(VehicleId);


            //Upload the file on server and save the path in database if user have submitted file
            if (files.Count != 0)
            {
                //Extract the extension of submitted file
                var Extension = Path.GetExtension(files[0].FileName);

                //Create the relative image path to be saved in database table 
                var RelativeImagePath = Image.VehicleImagePath + VehicleId + Extension;

                //Create absolute image path to upload the physical file on server
                var AbsImagePath = Path.Combine(wwrootPath, RelativeImagePath);


                //Upload the file on server using Absolute Path
                using (var filestream = new FileStream(AbsImagePath, FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }

                //Set the path in database
               // SavedVehicle.ImagePath = RelativeImagePath;
            }
        }


        [HttpPost]
            public IActionResult Delete(int id)
            {
                Vehicle Vehicle = _db.Vehicles.Find(id);
                if (Vehicle == null)
                {
                    return NotFound();
                }
                _db.Vehicles.Remove(Vehicle);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

    }
}
