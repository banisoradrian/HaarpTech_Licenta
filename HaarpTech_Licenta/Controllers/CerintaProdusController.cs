using HaarpTech_Licenta.Models;
using HaarpTech_Licenta.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HaarpTech_Licenta.Controllers
{
    [Authorize(Roles =("Admin,ProjectManager,Client"))]
    public class CerintaProdusController : Controller
    {
        private readonly ICerintaProdusRepository _cerintaProdusRepository;
        private readonly ILogger<CerereOfertaController> _logger;
        private readonly IHostEnvironment _hostEnv;


        public CerintaProdusController(ICerintaProdusRepository cerintaProdusRepository, ILogger<CerereOfertaController> logger , IHostEnvironment hostEnv)
        {
            _cerintaProdusRepository = cerintaProdusRepository;
            _logger = logger;
            _hostEnv = hostEnv;
        }

        [HttpGet("CerintaProdus/Index")]
        public async Task<IActionResult> Index()
        {
            var cerintaProduseRepository = await _cerintaProdusRepository.GetAllAsync()
                                                             ?? Enumerable.Empty<CerintaProdus>();
            return View(cerintaProduseRepository);
        }

        [HttpGet("CerintaProdus/IndexFiltred")]
        public async Task<IActionResult> IndexFiltred(string id)
        {
            
            var cerintaProduseRepository = await _cerintaProdusRepository.GetAllByIdAsync(id)
                                                             ?? Enumerable.Empty<CerintaProdus>();
            return View("Index" , cerintaProduseRepository);
        }

        [HttpGet("CerintaProdus/Create")]
        public IActionResult Create(string id)
        {
            var model = new CerintaProdus
            {
                ID_CERERE = id
            };
            return View(model);
        }

        [HttpPost("CerintaProdus/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CerintaProdus model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var success = await _cerintaProdusRepository.AddCerintaProdus(model);
                    if (success)
                    {
                        return RedirectToAction("Index", "CerintaProdus");
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

            var cerintaProdus= await _cerintaProdusRepository.GetByIdAsync(id);
            if (cerintaProdus == null)
            {
                return NotFound();
            }

            return View(cerintaProdus);
        }


        [HttpGet("CerintaProdus/Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var cerintaProdus= await _cerintaProdusRepository.GetByIdAsync(id);
            if (cerintaProdus == null)
                return NotFound();
            return View(cerintaProdus);
        }

        [HttpPost("CerintaProdus/Edit")]
        public async Task<IActionResult> Edit(CerintaProdus cerintaProdus)
        {
            if (!ModelState.IsValid)
                return View(cerintaProdus);

            bool ok = await _cerintaProdusRepository.UpdateCerintaProdusAsync(cerintaProdus);
            if (!ok)
                ModelState.AddModelError("", "A apărut o eroare la actualizare.");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var cerintaProdus = await _cerintaProdusRepository.GetByIdAsync(id);
            if (cerintaProdus == null)
                return NotFound();
            return View(cerintaProdus);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            bool ok = await _cerintaProdusRepository.DeleteCerintaProdusAsync(id);
            if (!ok)
            {
                ModelState.AddModelError("", "Ștergerea a eșuat.");
                var cerereOferta = await _cerintaProdusRepository.GetByIdAsync(id);
                return View("Delete", cerereOferta);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("CerintaProdus/CerinteProdusPdf")]
        public async Task<IActionResult> CerinteProdusPdf()
        {
            
            var all = await _cerintaProdusRepository.GetAllAsync()
                      ?? Enumerable.Empty<CerintaProdus>();

            var rezolvate = all.Where(c => c.StatusCerinta == "Rezolvată").ToList();

            var contentRoot = _hostEnv.ContentRootPath;
            var folderPath = Path.Combine(contentRoot, "Documente_Chitante", "Documente_Cerinte");
            Directory.CreateDirectory(folderPath);
            DateTime dataActuala = DateTime.Now;

            var fileName = $"Cerinte_Produs_Rezolvate{dataActuala.Year}_{dataActuala.Month}_{dataActuala.Day}_{dataActuala.Hour}.pdf";
            var filePath = Path.Combine(folderPath, fileName);

            byte[] pdfBytes;


            var viewPath = Path.Combine(contentRoot, "Views", "CerintaProdus", "RaportCerintaProdusPdf.cshtml");
            var viewModified = System.IO.File.GetLastWriteTimeUtc(viewPath);
            var fileModified = System.IO.File.Exists(filePath)
                ? System.IO.File.GetLastWriteTimeUtc(filePath)
                : DateTime.MinValue;

            if (!System.IO.File.Exists(filePath) || fileModified < viewModified)
            {
                var pdfResult = new ViewAsPdf("RaportCerintaProdusPdf", rezolvate)
                {
                    FileName = fileName,
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = new Rotativa.AspNetCore.Options.Margins(15, 15, 15, 15)
                };

                pdfBytes = await pdfResult.BuildFile(ControllerContext);
                await System.IO.File.WriteAllBytesAsync(filePath, pdfBytes);
            }
            else
            {
                pdfBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            }

            // 7) Trimite PDF-ul pentru download
            return File(pdfBytes, "application/pdf", fileName);
        }

    }
}
