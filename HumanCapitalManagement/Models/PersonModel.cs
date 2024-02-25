namespace HumanCapitalManagement.Models
{
    public class PersonModel
    {
        public int Id { get; set; }
        public string? FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Salary { get; set; }
        public string Department { get; set; } = null!;
    }
}