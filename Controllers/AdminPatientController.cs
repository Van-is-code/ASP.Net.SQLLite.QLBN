using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Asp.Net_MvcWeb_Pj3.Aptech.Models;
using BCrypt.Net;
using Microsoft.Extensions.Logging;
using PagedList;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.Net_MvcWeb_Pj3.Aptech.Controllers
{
    public class AdminPatientController : Controller
    {
        private readonly ILogger<AdminPatientController> _logger;
        private readonly DataContext _context;

        public AdminPatientController(ILogger<AdminPatientController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;

            // Creates the database if not exists
            // Tạo cơ sở dữ liệu nếu nó chưa có
            // Đồng bộ csdl và lớp mô hình
            // Sync database structure with Model Classes
            _context.Database.EnsureCreated();
        }

        public IActionResult Index()
        {
            return RedirectToAction("PatientIndex");
            // return View();
        }

        [HttpGet]
        [Route("/admin/Patient/list")]
        public IActionResult PatientIndex()
        {
            var patients = _context.Patient.ToList();
            return View(patients);
        }

        [HttpGet]
        [Route("/admin/Patient/add")]
        public IActionResult PatientAdd()
        {
            ViewData["publisherList"] = _context.Publisher.ToList();
            return View();
        }

        [HttpPost]
        [Route("/admin/Patient/add")]
        public async Task<IActionResult> PatientAdd(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Patient.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction("PatientIndex");
            }
            ViewData["publisherList"] = _context.Publisher.ToList();
            return View(patient);
        }

        [HttpGet]
        [Route("/admin/Patient/edit/{id}")]
        public IActionResult PatientEdit(int id)
        {
            var patient = _context.Patient.Find(id);
            if (patient == null)
            {
                return NotFound();
            }
            ViewData["publisherList"] = _context.Publisher.ToList();
            return View(patient);
        }

        [HttpPost]
        [Route("/admin/Patient/edit/{id}")]
        public async Task<IActionResult> PatientEdit(int id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Patient.Update(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction("PatientIndex");
            }
            ViewData["publisherList"] = _context.Publisher.ToList();
            return View(patient);
        }

        [HttpGet]
        [Route("/admin/Patient/delete/{id}")]
        public IActionResult PatientDelete(int id)
        {
            var patient = _context.Patient.Find(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        [HttpPost, ActionName("PatientDelete")]
        [Route("/admin/Patient/delete/{id}")]
        public async Task<IActionResult> PatientDeleteConfirmed(int id)
        {
            var patient = _context.Patient.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patient.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction("PatientIndex");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
