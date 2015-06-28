using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cpts483.Models.ViewModels
{
    public class RosterViewModel
    {
        public Course ActiveCourse { get; set; }
        public List<Course> AllCourses { get; set; }
        public List<Student> AllStudents { get; set; }
        public List<Student> EnrolledStudents { get; set; }

        public RosterViewModel()
        {
            //prevent null reference exceptions by creating dummy lists
            AllCourses = new List<Course>();
            AllStudents = new List<Student>();
            EnrolledStudents = new List<Student>();
        }

    }
}