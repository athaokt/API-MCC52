using API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModel
{
    public class GetAllData
    {
        public Employee Employees{ get; set; }
        public Account Accounts { get; set; }
        public Profiling Profilings { get; set; }
        public Education Educations { get; set; }
        public University Universities { get; set; }
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Salary { get; set; }
        public string PhoneNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Degree { get; set; }
        public string GPA { get; set; }
        public string UniversityName { get; set; }
    }
    /*public enum Gender
    {
        Pria, Wanita
    }*/
}
