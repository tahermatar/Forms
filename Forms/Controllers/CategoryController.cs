using Forms.Data;
using Forms.Models;
using Forms.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Forms.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var categories = _db.Categories.Where(x => !x.IsDelete).OrderByDescending(x => x.CreatedAt).ToList();
            //ViewBag.msg = "Welcome to our system";
            //ViewData["msg"] = "Welcome to our system";
            //TempData["msg"] = "Welcome to our system";
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UpdateCategoryViewModel input)
        {
            if (ModelState.IsValid)
            {
                var category = new Category();
                category.Name = input.Name;
                category.CreatedAt = DateTime.Now;
                // Because in defult is false I commited it
                //category.IsDelete = false;  

                _db.Categories.Add(category);
                _db.SaveChanges();

                TempData["msg"] = "Item Added Successfully !";
                return RedirectToAction("Index");
            }

            return View(input);
        }

        [HttpGet]
        public IActionResult Update(int Id)
        {
            var category = _db.Categories.SingleOrDefault(x => x.Id == Id && !x.IsDelete);
            if (category == null)
            {
                return NotFound();
            }
            var categoryVm = new UpdateCategoryViewModel();
            categoryVm.Id = Id;
            categoryVm.Name = category.Name;

            return View(categoryVm);
        }

        [HttpPost]
        public IActionResult Update(UpdateCategoryViewModel input)
        {
            if (ModelState.IsValid)
            {
                var category = _db.Categories.SingleOrDefault(x => x.Id == input.Id && !x.IsDelete);
                if (category == null)
                {
                    return NotFound();
                }
                category.Name = input.Name;
                _db.Categories.Update(category);
                _db.SaveChanges();

                TempData["msg"] = "s:Item Updated Successfully !";

                return RedirectToAction("Index");
            }

            return View(input);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var category = _db.Categories.SingleOrDefault(x => x.Id == Id && !x.IsDelete);
            if (category == null)
            {
                return NotFound();
            }

            category.IsDelete = true;
            _db.SaveChanges();

            TempData["msg"] = "s:Item Deleted Successfully !";

            return RedirectToAction("Index");
        }
    }
}
