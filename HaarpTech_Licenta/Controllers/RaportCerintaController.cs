using HaarpTech_Licenta.Models;
using HaarpTech_Licenta.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HaarpTech_Licenta.Controllers
{
    public class RaportCerintaController : Controller
    {
        private readonly IRaportCerintaRepository _raportCerintaRepository;
        private readonly ILogger<RaportCerintaController> _logger;

        public RaportCerintaController(IRaportCerintaRepository raportCerintaRepository, ILogger<RaportCerintaController> logger)
        {
            _raportCerintaRepository = raportCerintaRepository;
            _logger = logger;
        }

        [HttpGet("raportcerinta/index")]
        public async Task<IActionResult> Index()
        {
            var raportCerinta = await _raportCerintaRepository.GetAllAsync()
                            ?? Enumerable.Empty<RaportCerinta>();
            return View(raportCerinta);
        }


        [HttpGet("raportcerinta/create")]
        public IActionResult Create(string id)
        {
            var model = new RaportCerinta
            {
                ID_SEDINTA = id
            };
            return View(model);
        }


        [HttpPost("raportcerinta/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RaportCerinta model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var success = await _raportCerintaRepository.AddRaportCerintaAsync(model);

                    if (success)
                    {
                        return RedirectToAction("Index", "Sedinta");
                    }

                    ModelState.AddModelError("", "Nu s-a putut salva ședința.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la crearea ședinței pentru userul : {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
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

            var user = await _raportCerintaRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpGet("raportcerinta/edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var sedinta = await _raportCerintaRepository.GetByIdAsync(id);
            if (sedinta == null)
                return NotFound();
            return View(sedinta);
        }

        [HttpPost("raportcerinta/edit")]
        public async Task<IActionResult> Edit(RaportCerinta raportCerinta)
        {
            if (!ModelState.IsValid)
                return View(raportCerinta);

            bool ok = await _raportCerintaRepository.UpdateRaportCerintaAsync(raportCerinta);
            if (!ok)
                ModelState.AddModelError("", "A apărut o eroare la actualizare.");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var sedinta = await _raportCerintaRepository.GetByIdAsync(id);
            if (sedinta == null) return NotFound();
            return View(sedinta);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            bool ok = await _raportCerintaRepository.DeleteRaportCerintaAsync(id);
            if (!ok)
            {
                ModelState.AddModelError("", "Ștergerea a eșuat.");
                var sedinta = await _raportCerintaRepository.GetByIdAsync(id);
                return View("Delete", sedinta);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
