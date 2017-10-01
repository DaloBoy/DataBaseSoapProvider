using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DataBaseSoapProvider
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private const string ConnectionStr =
                "Server=tcp:skoledatabase.database.windows.net,1433;Initial Catalog=SkoleDB;Persist Security Info=False;User ID=DaloBoy;Password=Tobber1997;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //Connection string, for at kunne tilgå Databsen - Usikker på AccName og Pass

        public IList<Student> GetAllStudents()
        {
            const string selectAllStudents = "select * from Student order by Id";

            using (SqlConnection databaseConnection = new SqlConnection(ConnectionStr))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectAllStudents, databaseConnection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        List<Student> studentList = new List<Student>();
                        while (reader.Read())
                        {
                            Student student = ReadStudent(reader);
                            studentList.Add(student);
                        }
                        return studentList;
                    }
                }
            }
        }

        private static Student ReadStudent(IDataRecord reader)
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            int semester = reader.GetInt32(2);
            DateTime timeStamp = reader.GetDateTime(3);
            Student student = new Student
            {
                Id = id,
                Name = name,
            };
            return student;
        }

        public Student GetStudentById(int id)
        {
            const string selectStudent = "select * from student where id=@id";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionStr))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectStudent, databaseConnection))
                {
                    selectCommand.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        reader.Read(); // Advance cursor to first row
                        Student student = ReadStudent(reader);
                        return student;
                    }
                }
            }
        }

        public IList<Student> GetStudentsByName(string name)
        {
            string selectStr = "select * from student where name LIKE @name";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionStr))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectStr, databaseConnection))
                {
                    selectCommand.Parameters.AddWithValue("@name", "%" + name + "%");
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        IList<Student> studentList = new List<Student>();
                        while (reader.Read())
                        {
                            Student st = ReadStudent(reader);
                            studentList.Add(st);
                        }
                        return studentList;
                    }
                }
            }
        }

        public int AddStudent(string name)
        {
            const string insertStudent = "insert into student (name" +
                                         "d) values (@name)";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionStr))
            {
                databaseConnection.Open();
                using (SqlCommand insertCommand = new SqlCommand(insertStudent, databaseConnection))
                {
                    insertCommand.Parameters.AddWithValue("@name", name);
                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
        }
    }
}
