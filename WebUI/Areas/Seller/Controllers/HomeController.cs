using Microsoft.AspNetCore.Mvc;
using Persistence.Repository.IRepository;
using Persistence.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Seller.Controllers
{
    [Area("Seller")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISellerRepository _repository;
        public HomeController(IUnitOfWork unitOfWork, ISellerRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CheckProfile(string sellerEmail, string name)
        {
            //Seller seller = _unitOfWork.Seller.Where(x => x.ContactName == name).FirstOrDefault();
            //if (customer != null)
            //{
            //    ViewBag.Message = "Exist";
            //    return View();
            //}
            //else
            //{
            //    return View(customer);
            //}

            return View();
        }
    }
}
