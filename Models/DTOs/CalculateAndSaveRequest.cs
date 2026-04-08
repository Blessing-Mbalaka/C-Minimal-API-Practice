namespace API_Endpoints_Salary_Calculator.Models.DTOs
{
    public class CalculateAndSaveRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string EmploymentType { get; set; } = string.Empty;
        public decimal HourlyRate { get; set; }
        public int RegularHours { get; set; }
        public int OvertimeHours { get; set; }
        public decimal VariableAmount { get; set; }
        public string Period { get; set; } = string.Empty;
    }
}
