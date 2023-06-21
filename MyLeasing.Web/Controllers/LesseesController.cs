using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        // Repository
        readonly ILesseeRepository _lesseeRepository;

        // Helpers

        readonly IBlobHelper _blobHelper;
        readonly IConverterHelper _converterHelper;
        readonly IUserHelper _userHelper;

        public LesseesController(ILesseeRepository lesseeRepository,
            IBlobHelper blobHelper, IConverterHelper converterHelper, IUserHelper userHelper)
        {
            _lesseeRepository = lesseeRepository;

            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
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
                return NotFound();

            var lessee = await _lesseeRepository.GetByIdAsync(id.Value);

            if (lessee == null)
                return NotFound();

            return View(lessee);
        }


        // GET: Lessees/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }


        // POST: Lessees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
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
        [Authorize]
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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LesseeViewModel lesseeViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var photoUrl = lesseeViewModel.PhotoId;

                    var lessee = await PrepareForCreateOrEdit(lesseeViewModel);

                    if (lessee.PhotoId == Guid.Empty)
                        lessee.PhotoId = photoUrl;

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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Lessee entity)
        {
            var lessee = await _lesseeRepository.GetByIdAsync(entity.Id);
            await _lesseeRepository.DeleteAsync(entity);
            return RedirectToAction(nameof(Index));
        }


        #region Non-Action methods

        private async Task<bool> LesseeExistsAsync(int id)
        {
            return await _lesseeRepository.ExistsAsync(id);
        }

        async Task<Lessee> PrepareForCreateOrEdit(LesseeViewModel lesseeViewModel)
        {
            Guid photoId = await SavePhotoFileAsync(lesseeViewModel.PhotoFile);

            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            return new Lessee(lesseeViewModel)
            {
                PhotoId = photoId,
                User = user
            };
        }

        async Task<Guid> SavePhotoFileAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length < 1)
                return Guid.Empty;

            return await _blobHelper.UploadBlobAsync(imageFile, "lessees");
        }

        #endregion
    }
}
