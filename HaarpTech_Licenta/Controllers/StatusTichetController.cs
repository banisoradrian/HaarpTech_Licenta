using HaarpTech_Licenta.Models;
using HaarpTech_Licenta.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HaarpTech_Licenta.Controllers
{
    [Authorize(Roles = "Admin,ProjectManager")]
    public class StatusTichetController : Controller
    {
        private readonly IStatusTichetRepository _statusTichetRepository;
        private readonly ILogger<StatusTichet> _logger;
        private readonly IAngajatRepository _angajatRepository;

        //public StatusTichetController(IStatusTichetRepository statusTichetRepository, ILogger<StatusTichet> logger)
        //{
        //    _statusTichetRepository = statusTichetRepository;
        //    _logger = logger;
        //}
        public StatusTichetController(IStatusTichetRepository statusTichetRepository, ILogger<StatusTichet> logger , IAngajatRepository angajatRepository)
        {
            _statusTichetRepository = statusTichetRepository;
            _logger = logger;
            _angajatRepository = angajatRepository;
        }

        [HttpGet("StatusTichet/Index")]
        public async Task<IActionResult> Index()
        {
            var cerintaProduseRepository = await _statusTichetRepository.GetAllAsync()
                                                             ?? Enumerable.Empty<StatusTichet>();
            return View(cerintaProduseRepository);
        }

        [HttpGet("StatusTichet/IndexFiltred")]
        public async Task<IActionResult> IndexFiltred(string id)
        {

            var cerintaProduseRepository = await _statusTichetRepository.GetAllByIdAsync(id)
                                                             ?? Enumerable.Empty<StatusTichet>();
            return View("Index", cerintaProduseRepository);
        }

        [HttpGet("StatusTichet/Create")]
        public IActionResult Create(string id)
        {
            var model = new StatusTichet
            {
                ID_CERINTA = id
            };
            return View(model);
        }


        [HttpPost("StatusTichet/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StatusTichet model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var success = await _statusTichetRepository.AddTichetAsync(model);
                    if (success)
                    {
                        return RedirectToAction("Index", "StatusTichet");
                    }

                    ModelState.AddModelError("", "Nu s-a putut salva cerința produsului respectiv.");
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

            var statusTichet = await _statusTichetRepository.GetByIdAsync(id);
            if (statusTichet == null)
            {
                return NotFound();
            }

            return View(statusTichet);
        }


        [HttpGet("StatusTichet/Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var statusTichet = await _statusTichetRepository.GetByIdAsync(id);
            if (statusTichet == null)
                return NotFound();
            return View(statusTichet);
        }

        [HttpPost("StatusTichet/Edit")]
        public async Task<IActionResult> Edit(StatusTichet statusTichet)
        {
            if (!ModelState.IsValid)
                return View(statusTichet);

            bool ok = await _statusTichetRepository.UpdateTichetAsync(statusTichet);
            if (!ok)
                ModelState.AddModelError("", "A apărut o eroare la actualizare.");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var statusTichet = await _statusTichetRepository.GetByIdAsync(id);
            if (statusTichet == null)
                return NotFound();
            return View(statusTichet);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            bool ok = await _statusTichetRepository.DeleteTichetAsync(id);
            if (!ok)
            {
                ModelState.AddModelError("", "Ștergerea a eșuat.");
                var cerereOferta = await _statusTichetRepository.GetByIdAsync(id);
                return View("Delete", cerereOferta);
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Assign(string id)   // id = tichetId
        {
            var tichet = await _statusTichetRepository.GetByIdAsync(id);
            if (tichet == null) return NotFound();

            var viewModel = new AssignTichetViewModel
            {
                ID_TICHET = id,
                Subiect = tichet.Subiect,
                Angajati = await _angajatRepository.GetAllAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(AssignTichetViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Angajati = await _angajatRepository.GetAllAsync();
                return View(model);
            }

            var success = await _statusTichetRepository.AssignToAngajatAsync(model.ID_TICHET, model.ID_ANGAJAT);
            if (!success)
            {
                ModelState.AddModelError("", "Nu s-a putut asigna tichetul.");
                model.Angajati = await _angajatRepository.GetAllAsync();
                return View(model);
            }

            return RedirectToAction("Details", new { id = model.ID_TICHET });
        }

    }

}
