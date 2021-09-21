using System;
using System.Collections.Generic;
using System.Text;

namespace Erm.Test
{
    [Db("ProSol")]
    [Table("StudentDetail")]
    class Student
    {
        [IDField]
        public int StudentDetailID { get; set; }
        public string FirstForename { get; set; }
        public string Surname { get; set; }
        public string AcademicYearID { get; set; }
        [DbQueryField("ProSolution.dbo.GetAgeOn31Aug", "DateOfBirth", "AcademicYearID")]
        public int Age { get; set; }
    }
}
