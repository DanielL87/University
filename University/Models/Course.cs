using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using University;

namespace University.Models
{
    public class Course
    {
        private int _id;
        private string _name;
        private string _courseNumber;

        public Course(string name, string courseNumber, int id = 0)
        {
            _name = name;
            _courseNumber = courseNumber;
            _id = id;
        }

        public string GetName()
        {
            return _name;
        }

        public string GetCourseNumber()
        {
            return _courseNumber;
        }

        public int GetId()
        {
            return _id;
        }

    public void Save()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO courses (name, courseNumber) VALUES (@name , @courseNumber);";

        MySqlParameter name = new MySqlParameter();
        name.ParameterName = "@name";
        name.Value = this._name;
        cmd.Parameters.Add(name);

        MySqlParameter courseNumber = new MySqlParameter();
        courseNumber.ParameterName = "@courseNumber";
        courseNumber.Value = this._courseNumber;
        cmd.Parameters.Add(courseNumber);

        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }

    public static List<Course> GetAll()
    {
        List<Course> allCourses = new List<Course> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM courses;";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
            int id = rdr.GetInt32(0);
            string name = rdr.GetString(1);
            string courseNumber = rdr.GetString(2);
            
        

            Course newCourse = new Course(name, courseNumber, id);
            allCourses.Add(newCourse);
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return allCourses;
    }

     public static Course FindById(int id)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM courses WHERE id = (@searchId);";
        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = id;
        cmd.Parameters.Add(searchId);
        var rdr = cmd.ExecuteReader() as MySqlDataReader;

        int CourseId = 0;
        string CourseName = "";
        string CourseNumber = "";

        while(rdr.Read())
        {
            CourseId = rdr.GetInt32(0);
            CourseName = rdr.GetString(1);
            CourseNumber = rdr.GetString(2);

        }

        Course newCourse = new Course(CourseName, CourseNumber , CourseId);
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return newCourse;
    }   

    public void AddStudent(Student newStudent)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students_courses (student_id, course_id) VALUES (@studentId, @courseId);";
      MySqlParameter student_id = new MySqlParameter();
      student_id.ParameterName = "@studentId";
      student_id.Value = newStudent.GetId();
      cmd.Parameters.Add(student_id);

      MySqlParameter course_id = new MySqlParameter();
      course_id.ParameterName = "@courseId";
      course_id.Value = _id;
      cmd.Parameters.Add(course_id);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

     public List<Student> GetStudents()
            {
                MySqlConnection conn = DB.Connection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"SELECT students.* FROM courses
                    JOIN students_courses ON (courses.id = students_courses.course_id)
                    JOIN students ON (students_courses.student_id = students.id)
                    WHERE courses.id = @courseId;";

                MySqlParameter courseIdParameter = new MySqlParameter();
                courseIdParameter.ParameterName = "@courseId";
                courseIdParameter.Value = _id;
                cmd.Parameters.Add(courseIdParameter);
                MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
                List<Student> Students = new List<Student>{};
                while(rdr.Read())
                {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                string date = rdr.GetDateTime(2).ToString();

                Student newStudent = new Student(name, date);
                Students.Add(newStudent);
                }
                conn.Close();
                if (conn != null)
                {
                conn.Dispose();
                }
                return Students;
            }
    
//Clears and Overrides
    public static void ClearAll()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM courses;";
        cmd.ExecuteNonQuery();

        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
    public override bool Equals(System.Object otherCourse)
    {
        if (!(otherCourse is Course))
        {
            return false;
        }
        else
        {
            Course newCourse = (Course) otherCourse;
            bool descriptionEquality = (this.GetName() == newCourse.GetName());
            return (descriptionEquality);
        }
    }

    public override int GetHashCode()
    {
        return this.GetName().GetHashCode();
    }

    }
}