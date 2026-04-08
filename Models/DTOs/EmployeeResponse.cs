namespace API_Endpoints_Salary_Calculator.Models.DTOs
{
    public class EmployeeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string EmploymentType { get; set; } = string.Empty;
        public decimal HourlyRate { get; set; }
        public SalaryCalculationDetails Calculation { get; set; } = null!;
    }

    public class SalaryCalculationDetails
    {
        public string Period { get; set; } = string.Empty;
        public decimal HourlyRate { get; set; }
        public int RegularHours { get; set; }
        public int OvertimeHours { get; set; }
        public decimal VariableAmount { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal OvertimePay { get; set; }
        public decimal GrossSalary { get; set; }
        public DeductionDetails Deductions { get; set; } = new DeductionDetails();
        public EmployerCostsDetails EmployerCosts { get; set; } = new EmployerCostsDetails();
        public decimal NetSalary { get; set; }
        public decimal CostToCompany { get; set; }
    }

    public class DeductionDetails
    {
        public decimal Uif { get; set; }  // Employee portion
        public decimal Paye { get; set; }
        public decimal Total { get; set; }
    }

    public class EmployerCostsDetails
    {
        public decimal UifEmployer { get; set; }  // Employer portion
        public decimal Sdl { get; set; }  // Skills Development Levy
        public decimal Total { get; set; }
    }
}
