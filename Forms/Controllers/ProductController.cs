using Forms.Data;
using Forms.Models;
using Forms.Services;
using Forms.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Forms.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private IFileService _fileService;
        private IEmailService _emailService;
        public ProductController(ApplicationDbContext db, IFileService fileService, IEmailService emailService)
        {
            _db = db;
            _fileService = fileService;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _db.Products.Include(x => x.Category).Where(x => !x.IsDelete).OrderByDescending(x => x.CreatedAt).ToListAsync();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["categoryList"] = new SelectList(_db.Categories.Where(x => !x.IsDelete).ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel input)
        {
            if(ModelState.IsValid)
            {
                var nameIsExist = _db.Products.Any(x => x.Name == input.Name && !x.IsDelete);
                if(nameIsExist)
                {
                    TempData["msg"] = "e:Product Name Is Already Exist!";
                    ViewData["categoryList"] = new SelectList(_db.Categories.Where(x => !x.IsDelete).ToList(), "Id", "Name");
                    return View(input);
                }
                var product = new Product();
                product.Name = input.Name;
                product.Description = input.Description;
                product.CategoryId = input.CategoryId;
                product.Price = input.Price;
                if(input.ImageUrl != null)
                {
                    product.ImageUrl = await _fileService.SaveFile(input.ImageUrl, "Images");
                }
                product.CreatedAt = DateTime.Now;
                
                _db.Products.Add(product);
                _db.SaveChanges();

                // send email to admins
                var admins = _db.Admins.Where(x => !x.IsDelete).ToList();

                foreach(var admin in admins)
                {
                   await _emailService.Send(admin.Email, "New Product !", $"New Product With Name: {product.Name}");
                }

                TempData["msg"] = "s:Product was added successfuly !";

                return RedirectToAction("Index");
            }
            ViewData["categoryList"] = new SelectList(_db.Categories.Where(x => !x.IsDelete).ToList(), "Id", "Name");
            return View(input);
        }
    }
}
