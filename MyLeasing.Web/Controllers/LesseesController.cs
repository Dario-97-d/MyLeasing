using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLeasing.Web.Data.Entities;
using MyLeasing.Web.Data.Repository;
using MyLeasing.Web.Helpers;
using MyLeasing.Web.Models;

namespace MyLeasing.Web.Controllers
{
    public class LesseesController : Controller
    {
        private readonly ILesseeRepository _lesseeRepository;
        readonly IConverterHelper _converterHelper;
        readonly IImageHelper _imageHelper;
        readonly IUserHelper _userHelper;

        public LesseesController(ILesseeRepository lesseeRepository,
            IConverterHelper converterHelper, IImageHelper imageHelper, IUserHelper userHelper)
        {
            _lesseeRepository = lesseeRepository;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
            _userHelper = userHelper;
        }

        // GET: Lessees
        public IActionResult Index()
        {
            return View(_lesseeRepository.GetAll());
        }

        // GET: Lessees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lessee = await _lesseeRepository.GetByIdAsync(id.Value);
            if (lessee == null)
            {
                return NotFound();
            }

            return View(lessee);
        }

        // GET: Lessees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lessees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LesseeViewModel lesseeViewModel)
        {
            if (ModelState.IsValid)
            {
                var lessee = await PrepareForCreateOrEdit(lesseeViewModel);

                await _lesseeRepository.CreateAsync(lessee);

                return RedirectToAction(nameof(Index));
            }
            return View(lesseeViewModel);
        }

        // GET: Lessees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lessee = await _lesseeRepository.GetByIdAsync(id.Value);
            if (lessee == null)
            {
                return NotFound();
            }

            var lesseeViewModel = _converterHelper.ToLesseeViewModel(lessee);

            return View(lesseeViewModel);
        }

        // POST: Lessees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LesseeViewModel lesseeViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var photoUrl = lesseeViewModel.PhotoUrl;

                    var lessee = await PrepareForCreateOrEdit(lesseeViewModel);

                    if (string.IsNullOrEmpty(lessee.PhotoUrl))
                        lessee.PhotoUrl = photoUrl;

                    await _lesseeRepository.UpdateAsync(lessee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await LesseeExistsAsync(lesseeViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lesseeViewModel);
        }

        // GET: Lessees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lessee = await _lesseeRepository.GetByIdAsync(id.Value);
            if (lessee == null)
            {
                return NotFound();
            }

            return View(lessee);
        }

        // POST: Lessees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Lessee entity)
        {
            var lessee = await _lesseeRepository.GetByIdAsync(entity.Id);
            await _lesseeRepository.DeleteAsync(entity);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> LesseeExistsAsync(int id)
        {
            return await _lesseeRepository.ExistsAsync(id);
        }


        async Task<Lessee> PrepareForCreateOrEdit(LesseeViewModel lesseeViewModel)
        {
            var photoUrl = await SaveImageFileAsync(lesseeViewModel.PhotoFile);
            // TODO: Update user -> logged user
            var user = await _userHelper.GetUserByEmailAsync("dario@e.mail");

            return new Lessee(lesseeViewModel)
            {
                PhotoUrl = photoUrl,
                User = user
            };
        }

        async Task<string> SaveImageFileAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length < 1)
                return string.Empty;

            return await _imageHelper.UploadImageAsync(imageFile, "lessees");
        }
    }
}
