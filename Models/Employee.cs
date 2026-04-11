namespace API_Endpoints_Salary_Calculator.Models
{
    public class Employee
    {
        public int Id { get; set; }
        //Just covering our asses with string.Empty so we can reduce null reference errors. 
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string EmploymentType { get; set; } = string.Empty;
        public decimal HourlyRate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public ICollection<SalaryCalculation> SalaryCalculations { get; set; } = new List<SalaryCalculation>();
    }
}
