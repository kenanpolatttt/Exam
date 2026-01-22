using Exam.Areas.AdminPanel.ViewModels;
using Exam.Data;
using Exam.Models;
using Exam.Utilities.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Exam.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "Admin")]
    public class DeveloperController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DeveloperController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetDeveloperVM> getDeveloperVMs = await _context.Developers.Include(m => m.Positions).Select(m => new GetDeveloperVM
            {
                Id = m.Id,
                Name = m.Name,
                Image = m.Image,
                PositionName = m.Positions.Name,

            }).ToListAsync();

            return View(getDeveloperVMs);
        }
        public async Task<IActionResult> Create()
        {
            CreateDeveloperVM createDeveloperVM = new()
            {
                Positions = await _context.Positions.ToListAsync(),
            };
            return View(createDeveloperVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDeveloperVM createDeveloperVM)
        {
            createDeveloperVM.Positions = await _context.Positions.ToListAsync();

            if (!ModelState.IsValid) return View(createDeveloperVM);

            Developer developer = new()
            {

                Name = createDeveloperVM.Name,
                Description = createDeveloperVM.Description,
                Image = await createDeveloperVM.Image.CreateFileAsync(_env.WebRootPath, "images"),
                PositionId = createDeveloperVM.PositionId.Value,

            };

            if (!createDeveloperVM.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError(nameof(createDeveloperVM.Image), "File type is incorrect!!!");
                return View(createDeveloperVM);
            }

            bool existProfession = await _context.Positions.AnyAsync(p => p.Id == createDeveloperVM.PositionId);
            if (!existProfession)
            {
                ModelState.AddModelError(nameof(createDeveloperVM.PositionId), "Role does not exists!!!");
                return View(createDeveloperVM);
            }

            await _context.Developers.AddAsync(developer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Developer developer = await _context.Developers.FirstOrDefaultAsync(m => m.Id == id);

            if (developer == null) return NotFound();

            UpdateDeveloperVM updateDeveloperVM = new()
            {
                Photo = developer.Image,
                Name = developer.Name,
                Description = developer.Description,
                PositionId = developer.PositionId,
                Positions = await _context.Positions.ToListAsync()
            };
            return View(updateDeveloperVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateDeveloperVM updateDeveloperVM, int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Developer existedDeveloper = await _context.Developers.FirstOrDefaultAsync(m => m.Id == id);

            if (existedDeveloper == null) return NotFound();

            updateDeveloperVM.Positions = await _context.Positions.ToListAsync();

            if (!ModelState.IsValid) return View(updateDeveloperVM);

            if (existedDeveloper.PositionId != updateDeveloperVM.PositionId)
            {
                bool existMember = await _context.Positions.AnyAsync(m => m.Id == updateDeveloperVM.PositionId);
                if (!existMember)
                {
                    ModelState.AddModelError(nameof(updateDeveloperVM.PositionId), "yooxdu!");
                    return View(updateDeveloperVM);
                }
            }

            if (existedDeveloper.Image is not null)
            {
                if (!updateDeveloperVM.Image.CheckFileType("image/"))
                {
                    ModelState.AddModelError(nameof(updateDeveloperVM.Image), "not support");
                    return View(updateDeveloperVM);
                }
                existedDeveloper.Image = await updateDeveloperVM.Image.CreateFileAsync(_env.WebRootPath, "images");


                existedDeveloper.Image.DeleteFile(_env.WebRootPath, "images");
            }

            existedDeveloper.Name = updateDeveloperVM.Name;
            existedDeveloper.Description = updateDeveloperVM.Description;
            existedDeveloper.PositionId = updateDeveloperVM.PositionId.Value;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Developer developer = await _context.Developers.FirstOrDefaultAsync(x => x.Id == id);

            if (developer == null) return NotFound();

            developer.Image.DeleteFile(_env.WebRootPath, "images");

            _context.Developers.Remove(developer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Developer developer = await _context.Developers.Include(p => p.Positions).FirstOrDefaultAsync(p => p.Id == id);

            if (developer == null) return NotFound();

            DetailDeveloperVM detailDeveloperVM = new()
            {
                Id = developer.Id,
                Name = developer.Name,
                Image = developer.Image,
                Description = developer.Description,
                PositionName = developer.Positions.Name,
                PositionId = developer.Positions.Id,

            };
            return View(detailDeveloperVM);
        }
    }
}