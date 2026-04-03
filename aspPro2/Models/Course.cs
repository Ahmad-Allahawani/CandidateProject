namespace aspPro2.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public List<Enrollment>? Enrollments { get; set; }
    }
}
