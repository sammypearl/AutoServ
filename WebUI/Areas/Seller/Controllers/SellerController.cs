
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebUI.Areas.Seller.Controllers
{
    [Area("Seller")]
    public class SellerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly AutoServDbContext _db;
        private readonly ISellerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public VehicleViewModel VehicleVM { get; set; }
        [BindProperty]
        public SellerViewModel SellerVM { get; set; }

        public SellerController(ISellerRepository repository, AutoServDbContext db, 
            IWebHostEnvironment hostEnvironment, UserManager<ApplicationUser> userManager, 
            IApplicationUser userService, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _userManager = userManager;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _db = db;
            _hostEnvironment = hostEnvironment;
            _repository = repository;

            VehicleVM = new VehicleViewModel()
            {
                Makes = _db.Makes.ToList(),
                Models = _db.Models.ToList(),
                Vehicle = new Models.Vehicle()
            };

            SellerVM = new SellerViewModel()
            {
                Makes = _db.Makes.ToList(),
                Models = _db.Models.ToList(),
                Seller = new Models.Seller(),
                Vehicles = _db.Vehicles.ToList()
            };
        }

        public IActionResult Index()
        {
            return View(_unitOfWork.Seller.GetAll());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            SellerVM.Seller = _db.Sellers.SingleOrDefault(b => b.SellerId == id);

            //Filter the models associated to the selected make
            SellerVM.Vehicles = _db.Vehicles.Where(m => m.SellerId == SellerVM.Seller.SellerId);

            if (SellerVM.Seller == null)
            {
                return NotFound();
            }
            return View(SellerVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost(SellerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Models.Seller newSeller = _unitOfWork.Seller.Get(model.Seller.SellerId);

                newSeller.SellerId = model.Seller.SellerId;
                newSeller.Name = model.Seller.Name;
                newSeller.Title = model.Seller.Title;
                newSeller.SellerEmail = model.Seller.SellerEmail;
                newSeller.Age = model.Seller.Age;
                newSeller.Sex = model.Seller.Sex;
                newSeller.Company = model.Seller.Company;
                newSeller.Address = model.Seller.Address;
                newSeller.SellerPhone = model.Seller.SellerPhone;
                newSeller.City = model.Seller.City;
                newSeller.Country = model.Seller.Country;
                newSeller.PhoneNo2 = model.Seller.PhoneNo2;
                newSeller.PostalCode = model.Seller.PostalCode;

                if (model.ExistingPhotoPath != null)
                {
                    string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images",
                         model.ExistingPhotoPath);
                    System.IO.File.Delete(filePath);
                }
                newSeller.ProfileImagePath = ProcessUploadedFile(model);

                _unitOfWork.Seller.Update(newSeller);

                _unitOfWork.Save();
            }
      
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(SellerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                Models.Seller newSeller = new Models.Seller
                {
                    SellerId = model.Seller.SellerId,
                    Name = model.Seller.Name,
                    Title = model.Seller.Title,
                    SellerEmail = model.Seller.SellerEmail,
                    Age = model.Seller.Age,
                    Sex = model.Seller.Sex,
                    Company = model.Seller.Company,
                    Address = model.Seller.Address,
                    SellerPhone = model.Seller.SellerPhone,
                    City = model.Seller.City,
                    Country = model.Seller.Country,
                    PhoneNo2 = model.Seller.PhoneNo2,
                    PostalCode = model.Seller.PostalCode,
                    ProfileImagePath = uniqueFileName,
                };
                _db.Sellers.Add(newSeller);

                _db.SaveChanges();
            }
           
            TempData["Success"] = "Profile Added Successfully!, We Will give You a Call";
            if (User.IsInRole(Roles.Admin))
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        private string ProcessUploadedFile(SellerViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {

                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "-" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }

            }

            return uniqueFileName;
        }

       


        // For Future Implementation
        //public IActionResult Index(SellerViewModel model)
        //{
        //    var vehicleList = _repository
        //        .SellerVehiclesAsync(model.Seller.SellerId).ToList();
        //    return View(vehicleList);
        //}

        public IActionResult Deactivate(string userId)
        {
            var user = _userService.GetById(userId);
            _userService.Deactivate(user);
            return RedirectToAction("Index", "Profile");
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Details(int id)
        {
            SellerVM.Seller = _db.Sellers.SingleOrDefault(b => b.SellerId == id);

            if (SellerVM.Seller == null)
            {
                return NotFound();
            }
            return View(SellerVM);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var seller = _db.Sellers.Find(id);
            if (seller == null)
            {
                return NotFound();
            }
            _db.Sellers.Remove(seller);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
