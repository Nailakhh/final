using Dewi.Areas.Admin.ViewModels;
using Dewi.DAL;
using Dewi.Helpers.Extentions;
using Dewi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dewi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MemberController : Controller
    {
        AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public MemberController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var member = await _context.Members.ToListAsync();
            return View(member);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateVM createVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createVM);
            }
            Member member = new Member()
            {
                Name = createVM.Name,
                Position = createVM.Position,
                ImgUrl = createVM.File.FileCreating(_environment.WebRootPath, "upload/member")
            };


            if (!createVM.File.ContentType.Contains("image"))
            {
                ModelState.AddModelError("file", "dogru format secin");
                return View(createVM);
            }
            if (createVM.File.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("file", "dogru olcu secin");
                return View(createVM);
            }
            if (createVM.File == null)
            {
                ModelState.AddModelError("file", "dogru bos olmaz ");
                return View(createVM);
            }
            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Delete(int? id)
        {

            var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == id);
            if (member == null)
            {
                return BadRequest();
            }
            if (id == null)
            {
                return BadRequest();
            }
            if (member.ImgUrl != null)
            {
                member.ImgUrl.DeletingFile(_environment.WebRootPath, "upload/member");
            }
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == id);
            if (member == null)
            {
                return BadRequest();
            }

            UpdateVM updateVM = new UpdateVM()
            {
                Name = member.Name,
                Position = member.Position,
                CurentImgUrl = member.ImgUrl,

            };
            return View(updateVM);

        }

        [HttpPost]

        public async Task<IActionResult> Update(int id, UpdateVM updateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(updateVM);
            }
            var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == id);

            if (member == null)
            {
                return BadRequest();
            }
            if (updateVM.File != null)
            {
                if (!updateVM.File.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("file", "dogru format secin");
                    return View(updateVM);
                }
                if (updateVM.File.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("file", "dogru olcu secin");
                    return View(updateVM);
                }
                if (!string.IsNullOrEmpty(member.ImgUrl))
                {
                    member.ImgUrl.DeletingFile(_environment.WebRootPath, "upload/member");
                }
                member.ImgUrl = updateVM.File.FileCreating(_environment.WebRootPath, "upload/member");
            }

            member.Name=updateVM.Name;
            member.Position=updateVM.Position;

            _context.Members.Update(member);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }


    }
}
