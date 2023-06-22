using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudentsCRUD.Models;
using System.Diagnostics;

namespace StudentsCRUD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Students = await GetStudents();

            return View();
        }

        [HttpGet]
        public async Task<string> GetStudentsJson()
        {
            var students = await GetStudents();
            return JsonConvert.SerializeObject(students);
        }

        [HttpGet]
        public async Task<List<Student>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        [HttpPost]
        public async Task<RedirectToActionResult> AddStudent(Student model)
        {
            try
            {
                await _context.AddAsync(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<RedirectToActionResult> UpdateStudent(int id, Student student)
        {
            Student? _student = await _context.Students.FirstOrDefaultAsync(x => x.Id == id);

            if (_student != null)
            {
                _student.Name = student.Name;
                _student.Surname = student.Surname;
                _student.Age = student.Age;
                _student.Course = student.Course;
                _context.Students.Update(_student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<RedirectToActionResult> DeleteStudent(int id)
        {
            Student? _student = await _context.Students.FirstOrDefaultAsync(x => x.Id == id);

            if (_student != null)
            {
                _context.Remove(_student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}