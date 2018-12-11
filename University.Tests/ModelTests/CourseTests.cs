using Microsoft.VisualStudio.TestTools.UnitTesting;
using University.Models;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
 
namespace University.Tests
{
  [TestClass]
    public class CourseTest : IDisposable
    { 
        public void Dispose()
        {
            Course.ClearAll();
        }

        public CourseTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=university_test;";
        }

        [TestMethod]
        public void Saves_SavestoCourseTable_Method()
        {
            
            Course newCourse = new Course("Intro to Programming", "INT069");
            newCourse.Save();
            List<Course> resultList = Course.GetAll();
            Course result = resultList[0];
            string resultName = result.GetName();
            Assert.AreEqual("Intro to Programming", resultName);
        }

        [TestMethod]
        public void GetAll_ReturnsEmptyListFromDatabase_List()
        {
            List<Course> newList = new List<Course>{};
            List<Course> result = Course.GetAll();
            CollectionAssert.AreEqual(newList, result);
        }

        [TestMethod]
        public void Find_ReturnsCourseInDatabase_Course()
        {
            //Arrange
            Course testCourse = new Course("Intro to Programming", "INT069");
            testCourse.Save();

            //Act
            Course foundCourse = Course.FindById(testCourse.GetId());

            //Assert
            Assert.AreEqual(testCourse, foundCourse);
        }

        [TestMethod]
        public void AddStudent_AddsStudentsToCourse_StudentList()
        {
        //Arrange
        Course testCourse = new Course("Intro to Programming", "INT069");
        testCourse.Save();
        Student testStudent = new Student("jon", "2018-09-01");
        testStudent.Save();

        //Act
        testCourse.AddStudent(testStudent);

        List<Student> result = testCourse.GetStudents();
        List<Student> testList = new List<Student>{testStudent};

        //Assert
        CollectionAssert.AreEqual(testList, result);
        }



    }  
}