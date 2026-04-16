namespace API_Endpoints_Salary_Calculator.Models
{
    public class SalaryCalculation
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        public string Period { get; set; } = string.Empty;
        public decimal HourlyRate { get; set; }
        public int RegularHours { get; set; }
        public int OvertimeHours { get; set; }
        public decimal VariableAmount { get; set; }

        // Salary Components
        public decimal BasicSalary { get; set; }
        public decimal OvertimePay { get; set; }
        public decimal GrossSalary { get; set; }

        // Employee Deductions (deducted from employee salary)
        public decimal UIF { get; set; }  // Employee portion (1%)
        public decimal PAYE { get; set; }
        public decimal RetirementFund { get; set; } // Added retirement fund to employee deductions
        public decimal TotalDeductions { get; set; }
        public decimal NetSalary { get; set; }  // Employee take-home pay

        // Employer Costs (NOT deducted from employee, paid by employer to SARS)
        public decimal UIF_Employer { get; set; }  // Employer portion (1%)
        public decimal SDL { get; set; }  // Skills Development Levy (1%)
        public decimal TotalEmployerCosts { get; set; }

        // Total Cost to Company
        public decimal CostToCompany { get; set; }  // Gross + Employer contributions

        public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
    }
}
