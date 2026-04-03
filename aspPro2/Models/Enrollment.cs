using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace aspPro2.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }

        [ValidateNever]
        public User User { get; set; }
        [ValidateNever]
        public Course Course { get; set; }

        public DateTime EnrollmentDate { get; set; }
    }
}
