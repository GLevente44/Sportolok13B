namespace Sportolok13B.Models.DTOs
{
    public class Eredmeny
    {
        public int Id { get; set; }
        public string? Competition { get; set; }
        public string? Description { get; set; }
        public DateTime ResultTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int SportoloId { get; set; }
    }
}
