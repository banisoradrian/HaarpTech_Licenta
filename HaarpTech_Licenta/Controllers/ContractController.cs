using HaarpTech_Licenta.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore.Options;
using Rotativa.AspNetCore;
using HaarpTech_Licenta.Models;

namespace HaarpTech_Licenta.Controllers
{
    public class ContractController : Controller
    {
        private readonly IContractRepository _contractRepository;
        private readonly ILogger<Contract> _logger;
        private readonly IHostEnvironment _hostEnv;

        public ContractController(IContractRepository contractRepository, ILogger<Contract> logger, IHostEnvironment hostEnv)
        {
            _contractRepository = contractRepository;
            _logger = logger;
            _hostEnv = hostEnv;
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpGet("Contract/Index")]
        public async Task<IActionResult> Index()
        {
            var list = await _contractRepository.GetAllAsync()
                       ?? Enumerable.Empty<Contract>();
            return View(list);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar,Client")]
        [HttpGet("Contract/IndexFiltred")]
        public async Task<IActionResult> IndexFiltred(string id)
        {
            var list = await _contractRepository.GetAllByIdAsync(id)
                       ?? Enumerable.Empty<Contract>();
            return View("Index", list);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar,Client")]
        [HttpGet("Contract/Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var entity = await _contractRepository.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpGet("Contract/Create")]
        public IActionResult Create(string id)
        {
            var m = new Contract
            {
                ID_CERERE = id
            };
            return View(m);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpPost("Contract/Create"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var succes = await _contractRepository.AddContractAsync(model);
                if (succes)
                    return RedirectToAction("Index", "Contract");

                ModelState.AddModelError("", "Nu s-a putut adăuga contractul.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la Create Contract");
                ModelState.AddModelError("", "A apărut o eroare. Încercați din nou.");
            }

            return View(model);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpGet("Contract/Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var contract = await _contractRepository.GetByIdAsync(id);

            if (contract == null)
                return NotFound();

            return View(contract);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpPost("Contract/Edit"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Contract model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var ok = await _contractRepository.UpdateContractAsync(model);
            if (!ok)
                ModelState.AddModelError("", "A apărut o eroare la actualizare.");

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpGet("Contract/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            if (contract == null)
                return NotFound();

            return View(contract);
        }


        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpPost("Contract/Delete/{id}"), ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var ok = await _contractRepository.DeleteContractAsync(id);
            if (!ok)
            {
                ModelState.AddModelError("", "Ștergerea a eșuat.");
                var entity = await _contractRepository.GetByIdAsync(id);
                return View("Delete", entity);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Contract/PrintPdf/{id}")]
        public async Task<IActionResult> PrintContractPdf(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var entity = await _contractRepository.GetByIdAsync(id);
            if (entity == null) 
                return NotFound();

            // root = "...HaarpTech_Licenta"
            var root = _hostEnv.ContentRootPath;
            // include subfolderul Documente_Chitante
            var folder = Path.Combine(root, "Documente_Chitante", "ContractFiles");
            Directory.CreateDirectory(folder);

            var fileName = $"Contract_{entity.NumarContract}.pdf";
            var filePath = Path.Combine(folder, fileName);

            // restul codului rămâne neschimbat...
            byte[] pdfBytes;
            var viewPath = Path.Combine(root, "Views", "Contract", "ContractPdf.cshtml");
            var viewMod = System.IO.File.GetLastWriteTimeUtc(viewPath);
            var fileMod = System.IO.File.Exists(filePath)
                           ? System.IO.File.GetLastWriteTimeUtc(filePath)
                           : DateTime.MinValue;

            if (!System.IO.File.Exists(filePath) || fileMod < viewMod)
            {
                var pdf = new ViewAsPdf("ContractPdf", entity)
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