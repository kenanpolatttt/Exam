using System.Diagnostics;
using Exam.Data;
using Exam.Models;
using Exam.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            List<Developer> developers = await _context.Developers.Include(d => d.Positions).ToListAsync();

            HomeVM homeVM = new()
            {
                Developers = developers,
            };

            return View(homeVM);
        }

    
    }
}
