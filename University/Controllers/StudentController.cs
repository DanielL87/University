using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using University.Models;

namespace University.Controllers
{
    public class StudentController : Controller
    {
        [HttpGet("/students")]
        public ActionResult Index()
        {   
            List<Student> allStudents = Student.GetAll();
            return View(allStudents);
        }

        [HttpGet("/students/new")]
        public ActionResult New()
        {
            return View();
        }  

        [HttpPost("/students")]
        public ActionResult Create(string studentName, string enrollmentDate)
        {
            Student newStudent = new Student(studentName, enrollmentDate);
            newStudent.Save();
            List<Student> allStudents = Student.GetAll();
            return View("Index", allStudents);
        }

        [HttpGet("/students/show/{id}")]
        public ActionResult Show(int id)
        {
            Student newStudent = Student.FindById(id);
            List<Course> foundCourses = newStudent.GetCourse();
            List<Course> allCourses = Course.GetAll();
            Dictionary<string, object> myDic = new Dictionary<string, object> ();
            myDic.Add("student", newStudent);
            myDic.Add("courses", foundCourses);
            myDic.Add("allCourses", allCourses);
            return View(myDic);
        }

        [HttpPost("/students/{id}")]
        public ActionResult AddCourse(int id, int courseId)
        {
            Student newStudent = Student.FindById(id);
            Course foundCourse = Course.FindById(courseId);
            newStudent.AddCourse(foundCourse);
            return RedirectToAction("Show");   
        }

    }
}   