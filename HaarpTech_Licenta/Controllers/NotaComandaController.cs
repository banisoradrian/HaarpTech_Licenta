using HaarpTech_Licenta.Models;
using HaarpTech_Licenta.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;


namespace HaarpTech_Licenta.Controllers
{
    public class NotaComandaController : Controller
    {
        private readonly INotaComandaRepository _notaComandaRepository;
        private readonly ILogger<NotaComanda> _logger;
        private readonly IHostEnvironment _hostEnv;

        public NotaComandaController(
            INotaComandaRepository notaComandaRepository,
            ILogger<NotaComanda> logger,
            IHostEnvironment hostEnv)
        {
            _notaComandaRepository = notaComandaRepository;
            _logger = logger;
            _hostEnv = hostEnv;
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpGet("NotaComanda/Index")]
        public async Task<IActionResult> Index()
        {
            var notaComandaRepository = await _notaComandaRepository.GetAllAsync()
                                                             ?? Enumerable.Empty<NotaComanda>();
            return View(notaComandaRepository);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar,Client")]
        [HttpGet("NotaComanda/IndexFiltred")]
        public async Task<IActionResult> IndexFiltred(string id)
        {

            var notaComandaRepository = await _notaComandaRepository.GetAllByIdAsync(id)
                                                             ?? Enumerable.Empty<NotaComanda>();
            return View("Index", notaComandaRepository);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpGet("NotaComanda/Create")]
        public IActionResult Create(string id)
        {
            var model = new NotaComanda
            {
                ID_CERERE = id
            };
            return View(model);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpPost("NotaComanda/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NotaComanda model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var success = await _notaComandaRepository.AddNotaComandaAsync(model);
                    if (success)
                    {
                        return RedirectToAction("Index", "NotaComanda");
                    }

                    ModelState.AddModelError("", "Nu s-a putut aduga nota de comandă.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la crearea cererei pentru userul : {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
                ModelState.AddModelError("", "A apărut o eroare. Încercați din nou.");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar,Client")]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var notaComanda = await _notaComandaRepository.GetByIdAsync(id);
            if (notaComanda == null)
            {
                return NotFound();
            }

            return View(notaComanda);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpGet("NotaComanda/Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var notaComanda = await _notaComandaRepository.GetByIdAsync(id);
            if (notaComanda == null)
                return NotFound();
            return View(notaComanda);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpPost("NotaComanda/Edit")]
        public async Task<IActionResult> Edit(NotaComanda notaComanda)
        {
            if (!ModelState.IsValid)
                return View(notaComanda);

            bool ok = await _notaComandaRepository.UpdateNotaComandaAsync(notaComanda);
            if (!ok)
                ModelState.AddModelError("", "A apărut o eroare la actualizare.");

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        public async Task<IActionResult> Delete(string id)
        {
            var notaComanda = await _notaComandaRepository.GetByIdAsync(id);

            if (notaComanda == null)
                return NotFound();

            return View(notaComanda);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            bool ok = await _notaComandaRepository.DeleteNotaComandaAsync(id);
            if (!ok)
            {
                ModelState.AddModelError("", "Ștergerea a eșuat.");
                var notaComanda = await _notaComandaRepository.GetByIdAsync(id);
                return View("Delete", notaComanda);
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet("NotaComanda/PrintPdf/{id}")]
        public async Task<IActionResult> PrintPdf(string id, bool refresh = false)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();


            var nota = await _notaComandaRepository.GetByIdAsync(id);
            if (nota == null)
                return NotFound();

            var contentRoot = _hostEnv.ContentRootPath;
            var folderPath = Path.Combine(contentRoot, "Documente_Chitante", "NoteDeComandaFiles");
            Directory.CreateDirectory(folderPath);


            var safeNumar = string.Concat(nota.NumarComanda
                .Where(c => !Path.GetInvalidFileNameChars().Contains(c)));
            var fileName = $"NotaComanda_{safeNumar}.pdf";
            var filePath = Path.Combine(folderPath, fileName);


            var viewPath = Path.Combine(contentRoot, "Views", "NotaComanda", "NotaDeComandaPdf.cshtml");
            var viewModified = System.IO.File.GetLastWriteTimeUtc(viewPath);
            var fileModified = System.IO.File.Exists(filePath)
                ? System.IO.File.GetLastWriteTimeUtc(filePath)
                : DateTime.MinValue;


            var dataModified = nota.DataEmiterii.ToUniversalTime();

            byte[] pdfBytes;

            if (refresh
                || !System.IO.File.Exists(filePath)
                || fileModified < viewModified
                || fileModified < dataModified)
            {
                var pdf = new ViewAsPdf("NotaDeComandaPdf", nota)
                {
                    FileName = fileName,
                    PageSize = Size.A4,
                    PageOrientation = Orientation.Portrait,
                    PageMargins = new Margins(10, 10, 10, 10)
                };

                pdfBytes = await pdf.BuildFile(ControllerContext);
                await System.IO.File.WriteAllBytesAsync(filePath, pdfBytes);
            }
            else
            {
                pdfBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            }

            return File(pdfBytes, "application/pdf", fileName);
        }

    }
}

