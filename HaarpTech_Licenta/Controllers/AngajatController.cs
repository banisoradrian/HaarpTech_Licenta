using HaarpTech_Licenta.Models;
using HaarpTech_Licenta.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HaarpTech_Licenta.Controllers
{
    [Authorize(Roles ="Admin")]
    [Authorize]
    public class AngajatController : Controller
    {
        private readonly IAngajatRepository _angajatRepository;
        private readonly ILogger<Angajat> _logger;

        public AngajatController(IAngajatRepository angajatRepository, ILogger<Angajat> logger)
        {
            _angajatRepository = angajatRepository;
            _logger = logger;
        }

        [HttpGet("Angajat/Index")]
        public async Task<IActionResult> Index()
        {
            var angajatRepository = await _angajatRepository.GetAllAsync()
                                                             ?? Enumerable.Empty<Angajat>();
            return View(angajatRepository);
        }


        [HttpGet("Angajat/Create")]
        public IActionResult Create(string id)
        {
            var model = new Angajat
            {
                ID_ANGAJAT = id
            };
            return View(model);
        }

        [HttpPost("Angajat/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Angajat model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var success = await _angajatRepository.AddAngajatAsync(model);
                    if (success)
                    {
                        return RedirectToAction("Index", "Angajat");
                    }

                    ModelState.AddModelError("", "Nu s-a putut salva angajatul respectiv.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la crearea angajatului");
                ModelState.AddModelError("", "A apărut o eroare. Încercați din nou.");
            }

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var angajat = await _angajatRepository.GetByIdAsync(id);
            if (angajat == null)
            {
                return NotFound();
            }

            return View(angajat);
        }


        [HttpGet("Angajat/Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var angajat = await _angajatRepository.GetByIdAsync(id);
            if (angajat == null)
                return NotFound();
            return View(angajat);
        }

        [HttpPost("Angajat/Edit")]
        public async Task<IActionResult> Edit(Angajat angajat)
        {
            if (!ModelState.IsValid)
                return View(angajat);

            bool ok = await _angajatRepository.UpdateAngajatAsync(angajat);
            if (!ok)
                ModelState.AddModelError("", "A apărut o eroare la actualizare.");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var angajat = await _angajatRepository.GetByIdAsync(id);
            if (angajat == null)
                return NotFound();
            return View(angajat);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            bool ok = await _angajatRepository.DeleteAngajatAsync(id);
            if (!ok)
            {
                ModelState.AddModelError("", "Ștergerea a eșuat.");
                var angajat = await _angajatRepository.GetByIdAsync(id);
                return View("Delete", angajat);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
