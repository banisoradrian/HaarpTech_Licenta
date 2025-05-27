using HaarpTech_Licenta.Models;
using HaarpTech_Licenta.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HaarpTech_Licenta.Controllers
{
    public class CerereOfertaController : Controller
    {
        private readonly ICerereOfertaRepository _cerereOfertaRepository;
        private readonly ILogger<CerereOfertaController> _logger;

        public CerereOfertaController(ICerereOfertaRepository cereriOfertaRepository, ILogger<CerereOfertaController> logger)
        {
            _cerereOfertaRepository = cereriOfertaRepository;
            _logger = logger;
        }

        [HttpGet("CerereOferta/Index")]
        public async Task<IActionResult> Index()
        {
            var cereriOfertaRepository = await _cerereOfertaRepository.GetAllAsync()
                            ?? Enumerable.Empty<CerereOferta>();
            return View(cereriOfertaRepository);
        }

        [HttpGet("CerereOferta/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("CerereOferta/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CerereOferta model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var success = await _cerereOfertaRepository.AddCerereOfertaAsync(model);

                    if (success)
                    {
                        return RedirectToAction("Index", "CerereOferta");
                    }

                    ModelState.AddModelError("", "Nu s-a putut salva ședința.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la crearea cererei pentru userul : {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
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

            var cereriOferta = await _cerereOfertaRepository.GetByIdAsync(id);
            if (cereriOferta == null)
            {
                return NotFound();
            }

            return View(cereriOferta);
        }


        [HttpGet("CerereOferta/Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var cereriOferta = await _cerereOfertaRepository.GetByIdAsync(id);
            if (cereriOferta == null)
                return NotFound();
            return View(cereriOferta);
        }

        [HttpPost("CerereOferta/Edit")]
        public async Task<IActionResult> Edit(CerereOferta cerereOferta)
        {
            if (!ModelState.IsValid)
                return View(cerereOferta);

            bool ok = await _cerereOfertaRepository.UpdateCerereOfertaAsync(cerereOferta);
            if (!ok)
                ModelState.AddModelError("", "A apărut o eroare la actualizare.");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var cerereOferta = await _cerereOfertaRepository.GetByIdAsync(id);
            if (cerereOferta == null) 
                return NotFound();
            return View(cerereOferta);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            bool ok = await _cerereOfertaRepository.DeleteCerereOfertaAsync(id);
            if (!ok)
            {
                ModelState.AddModelError("", "Ștergerea a eșuat.");
                var  cerereOferta = await _cerereOfertaRepository.GetByIdAsync(id);
                return View("Delete", cerereOferta);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
