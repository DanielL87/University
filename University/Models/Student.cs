using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using University;

namespace University.Models
{
    public class Student
    {
        private int _id;
        private string _name;
        private string _date;

    public Student(string name, string date, int id = 0)
    {
     _name = name;
     _date = date;
     _id = id;    
    }

    public string GetName()
    {
        return _name;
    }

    public string GetDate()
    {
      return _date;  
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
            cmd.CommandText = @"INSERT INTO students (name, date) VALUES (@name , @date);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);

            MySqlParameter date = new MySqlParameter();
            date.ParameterName = "@date";
            date.Value = this._date;
            cmd.Parameters.Add(date);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

    public static List<Student> GetAll()
        {
        List<Student> allStudents = new List<Student> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM students;";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
            int id = rdr.GetInt32(0);
            string name = rdr.GetString(1);
            // DateTime date = (DateTime)rdr.GetDateTime(2);
            string date = rdr.GetDateTime(2).ToString();

            Student newStudent = new Student(name, date, id);
            allStudents.Add(newStudent);
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return allStudents;
        }

        public List<Course> GetCourse()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT courses.* FROM students
                JOIN students_courses ON (students.id = students_courses.student_id)
                JOIN courses ON (students_courses.course_id = courses.id)
                WHERE students.id = @studentId;";

            MySqlParameter studentIdParameter = new MySqlParameter();
            studentIdParameter.ParameterName = "@studentId";
            studentIdParameter.Value = _id;
            cmd.Parameters.Add(studentIdParameter);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Course> courses = new List<Course>{};
            while(rdr.Read())
            {
            int id = rdr.GetInt32(0);
            string courseName = rdr.GetString(1);
            string courseNumber = rdr.GetString(2);
           

            Course newCourse = new Course(courseName, courseNumber);
            courses.Add(newCourse);
            }
            conn.Close();
            if (conn != null)
            {
            conn.Dispose();
            }
            return courses;
        }

    public static Student FindById(int id)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM students WHERE id = (@searchId);";
        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = id;
        cmd.Parameters.Add(searchId);
        var rdr = cmd.ExecuteReader() as MySqlDataReader;

        int StudentId = 0;
        string StudentName = "";
        string StudentDate = "";

        while(rdr.Read())
        {
            StudentId = rdr.GetInt32(0);
            StudentName = rdr.GetString(1);
            StudentDate = rdr.GetDateTime(2).ToString();

        }

        Student newStudent = new Student(StudentName, StudentDate , StudentId);
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return newStudent;
    }

    public void AddCourse(Course newCourse)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO students_courses (student_id, course_id) VALUES (@studentId, @courseId);";
            MySqlParameter student_id = new MySqlParameter();
            student_id.ParameterName = "@studentId";
            student_id.Value = _id;
            cmd.Parameters.Add(student_id);

            MySqlParameter course_id = new MySqlParameter();
            course_id.ParameterName = "@courseId";
            course_id.Value = newCourse.GetId();
            cmd.Parameters.Add(course_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }




            //Clears and Overrides

    public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM students;";
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

    public override bool Equals(System.Object otherStudent)
        {
            if (!(otherStudent is Student))
            {
                return false;
            }
            else
            {
                Student newStudent = (Student) otherStudent;
                bool descriptionEquality = (this.GetName() == newStudent.GetName());
                return (descriptionEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }       

    }
}
