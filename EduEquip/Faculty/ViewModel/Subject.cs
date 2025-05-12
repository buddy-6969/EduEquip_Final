using System;

namespace EduEquip.Models
{
    public class CourseSubject
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Units { get; set; }
        public string FacultyId { get; set; }
    }
}