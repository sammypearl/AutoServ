using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Persistence.Helpers;
using Persistence.Repository.IRepository;
using Persistence.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Model.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Executive)]
    public class ModelController : Controller
    {
        private readonly AutoServDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        [BindProperty]
        public ModelViewModel ModelVM { get; set; }

        public ModelController(IUnitOfWork unitOfWork, AutoServDbContext db, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _mapper = mapper;
            ModelVM = new ModelViewModel()
            {
                Makes = _db.Makes.ToList(),
                Model = new Models.Model()
            };
        }
        public IActionResult Index()
        {
            var model = _unitOfWork.Model.GetAllIncluding(m => m.Make);
            return View(model.ToList());
        }

        public IActionResult Create()
        {
            return View(ModelVM);
        }
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost()
        {
            if (!ModelState.IsValid)
            {
                return View(ModelVM);
            }
            _unitOfWork.Model.Add(ModelVM.Model);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost()
        {
            if (!ModelState.IsValid)
            {
                return View(ModelVM);
            }

            _unitOfWork.Model.Update(ModelVM.Model);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        //HTTP Get Method
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ModelVM.Model = _unitOfWork.Model.GetAllIncluding(m => m.Make).SingleOrDefault(m => m.Id == id);
            if (ModelVM.Model == null)
            {
                return NotFound();
            }

            return View(ModelVM);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {

            Models.Model model = _unitOfWork.Model.Get(id);
            if (model == null)
            {
                return NotFound();
            }
            _unitOfWork.Model.Remove(model);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [HttpGet("api/models/{MakeID}")]
        public IEnumerable<ModelResources> Models(int MakeID)
        {
            var models = _unitOfWork.Model.GetAll();
            var modelResources = models
                .Select(m => new ModelResources
                {
                    Id = m.Id,
                    Name = m.Name,
                    MakeID = m.MakeID
                }).ToList()
                .Where(m => m.MakeID == MakeID);
            return modelResources;
        }

        [AllowAnonymous]
        [HttpGet("api/models")]
        public IEnumerable<ModelResources> Models()
        {
            var models = _db.Models.ToList();
            return _mapper.Map<List<Models.Model>, List<ModelResources>>(models);

            var modelResources = models
                .Select(m => new ModelResources
                {
                    Id = m.Id,
                    Name = m.Name
                }).ToList();
        }
    }
}
