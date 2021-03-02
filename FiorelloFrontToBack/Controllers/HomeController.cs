using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiorelloFrontToBack.DAL;
using FiorelloFrontToBack.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloFrontToBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Sliders = _context.Sliders.ToList(),
                SliderContent = _context.SliderContents.FirstOrDefault(),
                Categories = _context.Categories.Where(c => c.HasDeleted == false).ToList(),
                Products = _context.Products.Include(p => p.Category).Where(p => p.HasDeleted == false).Take(8).ToList(),
                About = _context.Abouts.FirstOrDefault(),
                AboutInfos = _context.AboutInfo.ToList(),
                Experts = _context.Experts.Include(e => e.Profession).ToList(),
                Blogs = _context.Blogs.ToList()
            };
            return View(homeVM);
        }
        public async Task<IActionResult> Page(int? page)
        {
            ViewBag.PageCount = decimal.Ceiling((decimal)_context.Categories.Where(x => x.HasDeleted == false).Count() / 5);
            if (page == null)
            {
                return View(_context.Categories.Where(x => x.HasDeleted == false).Take(5).ToListAsync());
            }
            return View(_context.Categories.Where(x=>x.HasDeleted==false).Skip(((int)page-1)*5).Take(5).ToListAsync());
        }
    }
}
