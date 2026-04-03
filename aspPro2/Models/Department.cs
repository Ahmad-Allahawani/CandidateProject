namespace aspPro2.Models
{
    public class Department
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        public List<Course>? Courses { get; set; }
    }
}
