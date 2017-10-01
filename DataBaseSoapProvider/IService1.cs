using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DataBaseSoapProvider
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        IList<Student> GetAllStudents();

        [OperationContract]
        Student GetStudentById(int id);

        [OperationContract]
        IList<Student> GetStudentsByName(string name);

        [OperationContract]
        int AddStudent(string name);
    }

    [DataContract]
    public class Student
    {
        [DataMember]
        public int Id;

        [DataMember]
        public string Name;
    }
}

