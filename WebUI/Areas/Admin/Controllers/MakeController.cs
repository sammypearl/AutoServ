using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Persistence;
using Persistence.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Executive")]
    public class MakeController : Controller
    {
        private readonly AutoServDbContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public MakeController(IUnitOfWork unitOfWork, AutoServDbContext db)
        {
            _db = db;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View(_unitOfWork.Make.GetAll());
        }

        //HTTP Get Method
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Make make)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Make.Add(make);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(make);

        }

        // Implement IUnitOfWork for Edit and and Delete
        //HTTP Get Method
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var make = _unitOfWork.Make.Get(id);
            if (make == null)
            {
                return NotFound();
            }

            return View(make);
        }


        [HttpPost]
        public IActionResult Edit(Make make)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Make.Update(make);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(make);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var make = _unitOfWork.Make.Get(id);
            if (make == null)
            {
                return NotFound();
            }
            _unitOfWork.Make.Remove(make);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
