using Microsoft.VisualStudio.TestTools.UnitTesting;
using University.Models;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
 
namespace University.Tests
{
  [TestClass]
    public class StudentTest : IDisposable
    { 
        public void Dispose()
        {
            Student.ClearAll();
        }

        public StudentTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=university_test;";
        }

        [TestMethod]
        public void Saves_SavestoStudentTable_Method()
        {
            
            Student newStudent = new Student("jon", "2018-09-01");
            newStudent.Save();
            List<Student> resultList = Student.GetAll();
            Student result = resultList[0];
            string resultName = result.GetName();
            Assert.AreEqual("jon", resultName);
        }

        [TestMethod]
        public void GetAll_ReturnsEmptyListFromDatabase_List()
        {
            List<Student> newList = new List<Student> {};
            List<Student> result = Student.GetAll();
            CollectionAssert.AreEqual(newList, result);
        }

        [TestMethod]
        public void Find_ReturnsStudentInDatabase_Student()
        {
            //Arrange
            Student testStudent = new Student("jon", "2018-09-01");
            testStudent.Save();

            //Act
            Student foundStudent = Student.FindById(testStudent.GetId());

            //Assert
            Assert.AreEqual(testStudent, foundStudent);
        }

        [TestMethod]
        public void AddCourse_AddsCourseToStudent_CourseList()
        {
            //Arrange
            Student testStudent = new Student("jon", "2018-09-01");
            testStudent.Save();
            Course testCourse = new Course("Intro to Programming", "INT069");
            testCourse.Save();

            //Act
            testStudent.AddCourse(testCourse);

            List<Course> result = testStudent.GetCourse();
            List<Course> testList = new List<Course>{testCourse};

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

    }  
}
