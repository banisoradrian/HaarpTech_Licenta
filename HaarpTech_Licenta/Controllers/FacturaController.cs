using HaarpTech_Licenta.Repository;
using HaarpTech_Licenta.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Rotativa.AspNetCore;

namespace HaarpTech_Licenta.Controllers
{
    
    public class FacturaController : Controller
    {
        private readonly IFacturaRepository _facturaRepo;
        private readonly ILogger<FacturaController> _logger;
        private readonly IWebHostEnvironment _hostEnv;


        public FacturaController(IFacturaRepository facturaRepo, ILogger<FacturaController> logger, IWebHostEnvironment hostEnv)
        {
            _facturaRepo = facturaRepo;
            _logger = logger;
            _hostEnv = hostEnv;
        }

        [HttpGet("Factura/Index")]
        public async Task<IActionResult> Index()
        {
            var list = await _facturaRepo.GetAllAsync() ?? Enumerable.Empty<Factura>();
            return View(list);
        }

        public async Task<IActionResult> IndexFiltred(string id)
        {
            var facturi = await _facturaRepo.GetAllByIdAsync(id)
                                                             ?? Enumerable.Empty<Factura>();
            return View("Index", _facturaRepo);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar,Client")]
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var vm = await _facturaRepo.GetFacturaWithElementsByIdAsync(id);
            if (vm?.Factura == null)
                return NotFound();

            return View(vm);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpGet("Factura/Create")]
        public IActionResult Create(string id)
        {
            var model = new FacturaCompletaViewModel
            {
                Factura = new Factura
                {
                    ID_CLIENT = id // populăm câmpul ID_CLIENT
                },
                ElementeFactura = new List<ElementeFactura>
                {
                    new ElementeFactura() // Inițializăm cel puțin o linie
                }
            };

            return View(model);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFactura(FacturaCompletaViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Create", model);

            model.ElementeFactura ??= new List<ElementeFactura>();

            var facturaId = Guid.NewGuid().ToString();
            model.Factura.ID_FACTURA = facturaId;

            // Calcule TVA automat pe fiecare element și total
            foreach (var element in model.ElementeFactura)
            {
                element.ID_ELEMENT_FACTURA = Guid.NewGuid().ToString();
                element.ID_FACTURA = facturaId;
                element.ValoareCuTva = Math.Round(element.ValoareFaraTva * 0.19m, 2);
            }

            model.Factura.TotalFaraTva = model.ElementeFactura.Sum(e => e.ValoareFaraTva);
            model.Factura.TotalCuTva = model.ElementeFactura.Sum(e => e.ValoareCuTva);
            model.Factura.TotalFactura = model.Factura.TotalFaraTva + model.Factura.TotalCuTva;

            try
            {
                var successFactura = await _facturaRepo.AddFacturaAsync(model.Factura);
                if (!successFactura)
                {
                    ModelState.AddModelError("", "Nu s-a putut salva factura.");
                    return View("Create", model);
                }

                foreach (var element in model.ElementeFactura)
                {
                    var successElement = await _facturaRepo.AddElementFacturaAsync(element);
                    if (!successElement)
                    {
                        ModelState.AddModelError("", "Nu s-a putut salva unul dintre produsele facturii.");
                        return View("Create", model);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la salvarea facturii.");
                ModelState.AddModelError("", "A apărut o eroare. Încercați din nou.");
                return View("Create", model);
            }
        }



        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpGet("Factura/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var factura = await _facturaRepo.GetByIdAsync(id);
            if (factura == null)
                return NotFound();

            ViewData["Title"] = "Ștergere Factură";
            return View(factura);
        }

        [Authorize(Roles = "Admin,Consultant,DepartamentFinanciar")]
        [HttpPost("Factura/Delete/{id}"), ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var success = await _facturaRepo.DeleteFacturaAsync(id);
                if (!success)
                {
                    TempData["ErrorMessage"] = "Ștergerea facturii a eșuat.";
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la Delete Factura");
                TempData["ErrorMessage"] = "A apărut o eroare. Încercați din nou.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet("Factura/PrintFacturaPdf/{id}")]
        public async Task<IActionResult> PrintFacturaPdf(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            
            var vm = await _facturaRepo.GetFacturaWithElementsAndClientDataByIdAsync(id);
            if (vm?.Factura == null)
                return NotFound();

            
            vm.ElementeFactura ??= new List<ElementeFactura>();
            vm.Client ??= new ClientData();

            // Configurare fisier și cale
            var contentRoot = _hostEnv.ContentRootPath;
            var folderPath = Path.Combine(contentRoot, "Documente_Chitante", "FacturiFiles");
            Directory.CreateDirectory(folderPath);

            var fileName = $"Factura_{vm.Factura.SeriaFactura}-{vm.Factura.NumarFactura}.pdf";
            var filePath = Path.Combine(folderPath, fileName);

            byte[] pdfBytes;
            var viewPath = Path.Combine(contentRoot, "Views", "Factura", "FacturaCompletaPdf.cshtml");
            var viewModified = System.IO.File.GetLastWriteTimeUtc(viewPath);
            var fileModified = System.IO.File.Exists(filePath)
                ? System.IO.File.GetLastWriteTimeUtc(filePath)
                : DateTime.MinValue;

            if (!System.IO.File.Exists(filePath) || fileModified < viewModified)
            {
                var pdfResult = new ViewAsPdf("FacturaCompletaPdf", vm)
                {
                    FileName = fileName,
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = new Rotativa.AspNetCore.Options.Margins(20, 15, 15, 15)
                };
                pdfBytes = await pdfResult.BuildFile(ControllerContext);
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
