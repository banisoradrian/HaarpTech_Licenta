using HaarpTech_Licenta.Repository;
using HaarpTech_Licenta.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HaarpTech_Licenta.Controllers
{
    
    public class ClientDataController : Controller
    {
        private readonly IClientDataRepository _clientRepo;
        private readonly ILogger<ClientData> _logger;

        public ClientDataController(IClientDataRepository clientRepo , ILogger<ClientData> logger)
        {
            _clientRepo = clientRepo;
            _logger = logger;
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar,Client")]
        [HttpGet("ClientData/Index")]
        public async Task<IActionResult> Index()
        {
            var list = await _clientRepo.GetAllAsync() ?? Enumerable.Empty<ClientData>();
            return View(list);
        }

        [HttpGet("ClientData/IndexFiltred")]
        public async Task<IActionResult> IndexFiltred(string id)
        {
            var list = await _clientRepo.GetAllByIdAsync(id) ?? Enumerable.Empty<ClientData>();
            return View("Index", list);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar,Client")]
        [HttpGet("ClientData/Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var entity = await _clientRepo.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar,Client")]
        [HttpGet("ClientData/Create")]
        public IActionResult Create(string id)
        {
            var model = new ClientData
            {
                ID_CONTRACT = id
            };
            return View(model);
        }
        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar,Client")]
        [HttpPost("ClientData/Create"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientData model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var success = await _clientRepo.AddClientAsync(model);
                if (success)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Nu s-a putut adăuga clientul.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la Create ClientData");
                ModelState.AddModelError("", "A apărut o eroare. Încercați din nou.");
            }

            return View(model);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpGet("ClientData/Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var entity = await _clientRepo.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpPost("ClientData/Edit"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientData model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var ok = await _clientRepo.UpdateClientAsync(model);
            if (!ok)
                ModelState.AddModelError("", "A apărut o eroare la actualizare.");

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpGet("ClientData/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var entity = await _clientRepo.GetByIdAsync(id);
            if (entity == null)
                return NotFound();
            ViewData["Title"] = "Ștergere Datele Clientului";
            return View(entity);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpPost("ClientData/Delete/{id}"), ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var ok = await _clientRepo.DeleteClientAsync(id);
                if (!ok)
                {
                    TempData["ErrorMessage"] = "Ștergerea a eșuat.";
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la Delete ClientData");
                TempData["ErrorMessage"] = "A apărut o eroare. Încercați din nou.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
