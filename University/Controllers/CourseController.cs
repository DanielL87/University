using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using University.Models;

namespace University.Controllers
{
    public class CourseController : Controller
    {
         [HttpGet("/courses")]
        public ActionResult Index()
        {   
            List<Course> allCourses = Course.GetAll();
            return View(allCourses);
        }

        [HttpGet("/courses/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/courses")]
        public ActionResult Create(string courseName, string courseNumber)
        {
            Course newCourse = new Course(courseName, courseNumber);
            newCourse.Save();
            List<Course> allCourses = Course.GetAll();
            return View("Index", allCourses);
        }

        [HttpGet("/courses/show/{id}")]
        public ActionResult Show(int id)
        {
            Course newCourse = Course.FindById(id);
            List<Student> foundStudents = newCourse.GetStudents();
            List<Student> allStudents = Student.GetAll();
            Dictionary<string, object> model = new Dictionary<string, object> ();
            model.Add("course", newCourse);
            model.Add("students", foundStudents);
            model.Add("allStudents", allStudents);
            return View(model);
        }
        
        [HttpPost("/courses/{id}")]
        public ActionResult AddStudent(int id, int studentId)
        {
            Course newCourse = Course.FindById(id);
            Student foundStudent = Student.FindById(studentId);
            newCourse.AddStudent(foundStudent);
            return RedirectToAction("Show");
        }


    }
}