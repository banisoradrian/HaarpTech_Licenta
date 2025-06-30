using Dapper;
using HaarpTech_Licenta.Models;
using HaarpTech_Licenta.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList;
using X.PagedList.Extensions;
namespace HaarpTech_Licenta.Controllers
{
    [Authorize]
    public class SedintaController : Controller
    {
        private readonly ISedintaRepository _sedintaRepository;
        private readonly ILogger<SedintaController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SedintaController(ISedintaRepository sedintaRepository , ILogger<SedintaController> logger , IHttpContextAccessor httpContextAccessor)
        {
            _sedintaRepository = sedintaRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("Sedinta/Index")]
        public async Task<IActionResult> Index()
        {
            var sedintaRepository = await _sedintaRepository.GetAllAsync();

            return View(sedintaRepository);
        }

        [HttpGet("Sedinta/IndexSedintaRaport1")]
        public async Task<IActionResult> IndexSedintaRaport(string id)
        {
            var sedintaRepository = await _sedintaRepository.GetAllRaportCerintaAsync(id);

            return View("Index", sedintaRepository);
        }


        [HttpGet("Sedinta/Create/{id?}")]
        public IActionResult Create(string id = null)
        {
            string userId = _httpContextAccessor.HttpContext!
                             .User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = string.IsNullOrEmpty(id)
                            ? new Sedinta()               // clasa goala
                            : new Sedinta 
                              {
                                ID_CERERE = id,
                                ID_USER = userId 
                              }; // clasa prepopulata
            return View("Create", model);
        }

        [HttpPost("Sedinta/Create/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sedinta model)
        {
            try
            {
                if (ModelState.IsValid)
                { 
                    var success = await _sedintaRepository.AddSedintaAsync(model);

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

        [Authorize(Roles = "Admin,Consultant")]
        [HttpGet("Sedinta/Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var sedinta = await _sedintaRepository.GetByIdAsync(id);
            if (sedinta == null)
                return NotFound();
            return View(sedinta);
        }

        [HttpPost("Sedinta/Edit")]
        public async Task<IActionResult> Edit(Sedinta s)
        {
            if (!ModelState.IsValid)
                return View(s);

            bool ok = await _sedintaRepository.UpdateSedintaAsync(s);
            if (!ok)
                ModelState.AddModelError("", "A apărut o eroare la actualizare.");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _sedintaRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Authorize(Roles = "Admin,Consultant")]
        public async Task<IActionResult> Delete(string id)
        {
            var sedinta = await _sedintaRepository.GetByIdAsync(id);
            if (sedinta == null) return NotFound();
            return View(sedinta);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            bool ok = await _sedintaRepository.DeleteSedintaAsync(id);
            if (!ok)
            {
                ModelState.AddModelError("", "Ștergerea a eșuat.");
                var sedinta = await _sedintaRepository.GetByIdAsync(id);
                return View("Delete", sedinta);
            }
            return RedirectToAction(nameof(Index));
        }  
    }
}
