using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Asp.Net_MvcWeb_Pj3.Aptech.Models;
using BCrypt.Net;
using Microsoft.Extensions.Logging;
using PagedList;

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
        public IActionResult PatientList()
        {
            // Nếu danh tính chưa có
            // thì điều hướng sang trang User Login
            // if(HttpContext.Session.GetInt32("User_Id") == null)
            // {
            //     // todo: Lưu lại url ngay trước khi bị điều hướng sang trang đăng nhập
            //     // để sau khi Login xong, thì quay lại trang đó.
            //     return RedirectToAction("UserLogin");
            // }

            // Gửi các quyển sách sang bên View HTML
            // ViewData["PatientList"] = _context.Patient.ToList();
            ViewData["PatientList"] = _context.Patient.Include(Patient => Patient.Pub).ToList();

            return View();
        }

        [HttpGet]
        [Route("/admin/Patient/index")]
        public IActionResult PatientIndex()
        {
            ViewBag.CurrentSort = "sortTest";

            ViewBag.CurrentFilter = "filterTest";

            // string sortOrder, string currentFilter, string searchString, int? page

            // string page_str = HttpContext.Request.Query["page"].ToString();

            int? page = Convert.ToInt32(HttpContext.Request.Query["page"]);

            if(page < 1) 
                page = 1;

            int pageSize = 3; // số phần tử trên trang
            int pageNumber = (page ?? 1); // số thứ tự của trang hiện tại

            var pagedList = _context.Patient.Include(item => item.Pub).ToPagedList(pageNumber, pageSize);

            // return View(students.ToPagedList(pageNumber, pageSize));

            // Gửi các quyển sách sang bên View HTML
            // ViewData["ds"] = _context.Sach.Include(bn => bn.NXB).ToList();
            return View(pagedList);

        }

        [HttpGet]
        [Route("/admin/Patient/add")]
        public IActionResult PatientAdd()
        {
            ViewData["publisherList"] = _context.Publisher.ToList();
            return View();
            // return View(new Patient()); // có thực sự cần thiết ? Nhất là khi validate mình làm bằng JS6
        }

        /**
         * https://www.claudiobernasconi.ch/2023/06/23/how-to-hash-passwords-with-bcrypt-in-csharp/
         * https://jasonwatmore.com/post/2022/01/16/net-6-hash-and-verify-passwords-with-bcrypt
         */

        [HttpPost]
        [Route("/admin/Patient/add")]
        public IActionResult PatientAdd(Patient Patient)
        {

            // Debug gỡ lỗi trên màn hình Terminal để biết vì sao nó lỗi Status: lúc nào cũng là true
            WriteLine("Patient status: "+Patient.Status.ToString());
            WriteLine("Patient status input: "+HttpContext.Request.Form["Status"]);

            // Nếu người dùng nhấp chuột vào checkbox Status thì có nghĩa là sách mới
            if(this.Request.Form["Status"].Equals("1"))
                Patient.Status = true ;
                

            else // ngược lại thì là sách cũ
                Patient.Status = false;
            // Các trường thông tin khác của Model Object được ánh xạ rất tốt từ html form vào OBject property
            // một cách tự động, riêng có kiểu BOol là phải làm thủ công.

                    // if (ModelState.IsValid)
            // {
            //     if (employee.EmployeeId == 0)
            //         _context.Add(employee);
            //     else
            //         _context.Update(employee);
            //     await _context.SaveChangesAsync();
            //     return RedirectToAction(nameof(Index));
            // }
            // return View(employee);

            // Patient.CreatedAt = DateTime.Now;
            // Patient.UpdatedAt = DateTime.Now;

            // Lưu dữ liệu mới vào DB
            _context.Add(Patient);
            _context.SaveChanges();

            HttpContext.Session.SetString("SUCCESS_MSG", "New Patient successfully added !");
            // tham khảo cách làm session đúng trong Asp.Net Core MVC
            // https://stackoverflow.com/questions/46921275/access-session-variable-in-razor-view-net-core-2
            // Xóa session đúng cách: Context.Session.Remove("SUCCESS_MSG");
            // đừng viết là: Context.Session.SetString("SUCCESS_MSG", null);
            // nó sẽ báo lỗi: value cannot be null

            // return Redirect("/admin/Patient/index");
            return RedirectToAction("PatientIndex");
        }

        //http://localhost:5017/admin/Patient/edit?id=2
        [HttpGet]
        [Route("/admin/Patient/edit")]
        public IActionResult PatientEdit(int id)
        {
            ViewData["publisherList"] = _context.Publisher.ToList();
            // return View();

            return View(_context.Patient.Find(id));
        }

        [HttpPost]
        [Route("/admin/Patient/edit")]
        public IActionResult PatientEdit(Patient Patient)
        {
                    // Nếu người dùng nhấp chuột vào checkbox Status thì có nghĩa là sách mới
            if(this.Request.Form["Status"].Equals("1"))
                Patient.Status = true;   
            else // ngược lại thì là sách cũ
                Patient.Status = false;
            // Các trường thông tin khác của Model Object được ánh xạ rất tốt từ html form vào OBject property
            // một cách tự động, riêng có kiểu BOol là phải làm thủ công.

            // Cập nhật csdl
            _context.Patient.AsNoTracking();
            _context.Update(Patient);
            _context.SaveChanges();

            // Điều hướng
            return RedirectToAction("PatientIndex");
        }

        //http://localhost:5017/Patient/delete?id=2
        // Xác nhận có chắc muốn xóa
        [HttpGet]
        [Route("/admin/Patient/delete")]
        public IActionResult PatientDelete(int id)
        {
            return View(_context.Patient.Find(id));
        }

        [HttpPost]
        [Route("/admin/Patient/delete")]
        public IActionResult PatientDelete(Patient obj)
        {
            // _context.Sach.AsNoTracking();

            // Xóa bản ghi trong DB
            // var old = _context.Patient.Find(obj.Id); // không cần thiết !!!
            _context.Patient.Remove(obj);
            _context.SaveChanges();
            // Điều hướng
            return RedirectToAction("PatientIndex");
            // return RedirectToAction(nameof(PatientIndex));

            // Cách 2:
            // var old = _context.Patient.Find(Int32.Parse(this.Request.Form["Id"].ToString()));
            // _context.Patient.Remove(old);
            // _context.SaveChanges();
            // return Redirect("/admin/Patient/index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
