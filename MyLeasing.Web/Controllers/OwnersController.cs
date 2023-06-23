using System;
using System.Net;
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
    public class OwnersController : Controller
    {
        // Repository
        readonly IOwnerRepository _ownerRepository;

        // Helpers

        readonly IBlobHelper _blobHelper;
        readonly IUserHelper _userHelper;

        public OwnersController(IOwnerRepository ownerRepository,
            IBlobHelper blobHelper, IUserHelper userHelper)
        {
            _ownerRepository = ownerRepository;

            _blobHelper = blobHelper;
            _userHelper = userHelper;
        }


        // GET: Owners
        public IActionResult Index()
        {
            return View(_ownerRepository.GetAll());
        }


        // GET: Owners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return OwnerNotFound();
            }

            var owner = await _ownerRepository.GetByIdAsync(id.Value);
            if (owner == null)
            {
                return OwnerNotFound();
            }

            return View(owner);
        }


        // GET: Owners/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }


        // POST: Owners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OwnerViewModel ownerViewModel)
        {
            if (ModelState.IsValid)
            {
                Guid guid = await SavePhotoFileAsync(ownerViewModel.PhotoFile);

                var owner = OwnerViewModel.ToOwner(ownerViewModel);
                owner.PhotoId = guid;

                owner.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                await _ownerRepository.CreateAsync(owner);

                return RedirectToAction(nameof(Index));
            }
            return View(ownerViewModel);
        }


        // GET: Owners/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return OwnerNotFound();
            }

            var owner = await _ownerRepository.GetByIdAsync(id.Value);
            if (owner == null)
            {
                return OwnerNotFound();
            }

            var ownerViewModel = Owner.ToOwnerViewModel(owner);

            return View(ownerViewModel);
        }

        
        // POST: Owners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OwnerViewModel ownerViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid guid = await SavePhotoFileAsync(ownerViewModel.PhotoFile);

                    var owner = OwnerViewModel.ToOwner(ownerViewModel);
                    owner.PhotoId = guid;

                    // TODO: Mudar para que o User seja o logado, quando o login estiver implementado
                    owner.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                    await _ownerRepository.UpdateAsync(owner);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _ownerRepository.ExistsAsync(ownerViewModel.Id))
                    {
                        return OwnerNotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ownerViewModel);
        }


        // GET: Owners/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return OwnerNotFound();
            }

            var owner = await _ownerRepository.GetByIdAsync(id.Value);
            if (owner == null)
            {
                return OwnerNotFound();
            }

            return View(owner);
        }


        // POST: Owners/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var owner = await _ownerRepository.GetByIdAsync(id);
            await _ownerRepository.DeleteAsync(owner);
            return RedirectToAction(nameof(Index));
        }


        #region Non-Action methods

        async Task<Guid> SavePhotoFileAsync(IFormFile photoFile)
        {
            if (photoFile == null || photoFile.Length < 1)
                return Guid.Empty;

            return await _blobHelper.UploadBlobAsync(photoFile, "owners");
        }

        ViewResult OwnerNotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View("ObjectNotFound", new ObjectNotFoundViewModel(nameof(Owner)));
        }

        #endregion
    }
}
