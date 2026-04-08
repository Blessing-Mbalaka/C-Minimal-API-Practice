using API_Endpoints_Salary_Calculator.Data;
using API_Endpoints_Salary_Calculator.Models;
using API_Endpoints_Salary_Calculator.Models.DTOs;
using API_Endpoints_Salary_Calculator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Endpoints_Salary_Calculator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly SalaryCalculationService _salaryService;

        public EmployeeController(ApplicationDbContext context, SalaryCalculationService salaryService)
        {
            _context = context;
            _salaryService = salaryService;
        }

        [HttpPost("calculate-and-save")]
        public async Task<ActionResult<EmployeeResponse>> CalculateAndSave([FromBody] CalculateAndSaveRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new { error = "Name and Email are required" });
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == request.Email);

            if (employee == null)
            {
                employee = new Employee
                {
                    Name = request.Name,
                    Surname = request.Surname,
                    Email = request.Email,
                    JobTitle = request.JobTitle,
                    EmploymentType = request.EmploymentType,
                    HourlyRate = request.HourlyRate
                };
                _context.Employees.Add(employee);
            }
            else
            {
                employee.Name = request.Name;
                employee.Surname = request.Surname;
                employee.JobTitle = request.JobTitle;
                employee.EmploymentType = request.EmploymentType;
                employee.HourlyRate = request.HourlyRate;
            }

            // SALARY CALCULATION FLOW:
            // 1. Calculate Basic Salary (Hourly Rate × Regular Hours)
            var basicSalary = request.HourlyRate * request.RegularHours;

            // 2. Calculate Overtime Pay (Hourly Rate × 1.5 × Overtime Hours)
            // South African Labour Law: Overtime is typically paid at 1.5x the hourly rate
            var overtimePay = request.HourlyRate * 1.5m * request.OvertimeHours;

            // 3. Calculate Gross Salary (Basic + Overtime + Variable/Bonus)
            var grossSalary = basicSalary + overtimePay + request.VariableAmount;

            // EMPLOYEE DEDUCTIONS (deducted from employee's gross salary):
            // 4. Calculate UIF Employee Portion - 1% of gross, capped at R177.12
            var uifEmployee = _salaryService.CalculateUIF(grossSalary);

            // 5. Calculate PAYE (Pay As You Earn Tax) based on SARS 2024/2025 tax brackets
            // This annualizes the income, calculates annual tax, then divides by 12
            var paye = _salaryService.CalculateMonthlyPAYE(grossSalary);

            // 6. Calculate Total Employee Deductions
            var totalDeductions = uifEmployee + paye;

            // 7. Calculate Net Salary (Take-home pay after deductions)
            var netSalary = grossSalary - totalDeductions;

            // EMPLOYER COSTS (paid by employer to SARS, NOT deducted from employee):
            // 8. Calculate UIF Employer Portion - 1% of gross, capped at R177.12
            var uifEmployer = _salaryService.CalculateUIF(grossSalary);

            // 9. Calculate SDL (Skills Development Levy) - 1% of gross payroll
            var sdl = _salaryService.CalculateSDL(grossSalary);

            // 10. Calculate Total Employer Costs
            var totalEmployerCosts = uifEmployer + sdl;

            // 11. Calculate Total Cost to Company (Gross Salary + Employer Contributions)
            var costToCompany = grossSalary + totalEmployerCosts;

            // Store the salary calculation in the database
            var calculation = new SalaryCalculation
            {
                Employee = employee,
                Period = request.Period,
                HourlyRate = request.HourlyRate,
                RegularHours = request.RegularHours,
                OvertimeHours = request.OvertimeHours,
                VariableAmount = request.VariableAmount,
                BasicSalary = basicSalary,
                OvertimePay = overtimePay,
                GrossSalary = grossSalary,
                UIF = uifEmployee,
                PAYE = paye,
                TotalDeductions = totalDeductions,
                NetSalary = netSalary,
                UIF_Employer = uifEmployer,
                SDL = sdl,
                TotalEmployerCosts = totalEmployerCosts,
                CostToCompany = costToCompany
            };

            _context.SalaryCalculations.Add(calculation);
            await _context.SaveChangesAsync();

            var response = new EmployeeResponse
            {
                Id = employee.Id,
                Name = employee.Name,
                Surname = employee.Surname,
                Email = employee.Email,
                JobTitle = employee.JobTitle,
                EmploymentType = employee.EmploymentType,
                HourlyRate = employee.HourlyRate,
                Calculation = new SalaryCalculationDetails
                {
                    Period = calculation.Period,
                    HourlyRate = calculation.HourlyRate,
                    RegularHours = calculation.RegularHours,
                    OvertimeHours = calculation.OvertimeHours,
                    VariableAmount = calculation.VariableAmount,
                    BasicSalary = calculation.BasicSalary,
                    OvertimePay = calculation.OvertimePay,
                    GrossSalary = calculation.GrossSalary,
                    Deductions = new DeductionDetails
                    {
                        Uif = calculation.UIF,
                        Paye = calculation.PAYE,
                        Total = calculation.TotalDeductions
                    },
                    NetSalary = calculation.NetSalary
                }
            };

            return Ok(response);
        }

        /// <summary>
        /// Get all employees from the database
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(employees);
        }

        /// <summary>
        /// Get current SARS tax brackets and tax year information
        /// Useful for displaying tax information to users
        /// </summary>
        [HttpGet("tax-brackets")]
        public ActionResult GetTaxBrackets()
        {
            var brackets = _salaryService.GetTaxBrackets();
            var taxYear = _salaryService.GetCurrentTaxYear();

            return Ok(new
            {
                taxYear = taxYear,
                brackets = brackets.Select(b => new
                {
                    minIncome = b.MinIncome,
                    maxIncome = b.MaxIncome,
                    baseTax = b.BaseTax,
                    rate = b.Rate,
                    ratePercentage = $"{b.Rate * 100}%",
                    description = b.MaxIncome.HasValue
                        ? $"R{b.MinIncome:N0} - R{b.MaxIncome:N0}: R{b.BaseTax:N2} + {b.Rate * 100}% of income above R{b.MinIncome:N0}"
                        : $"R{b.MinIncome:N0} and above: R{b.BaseTax:N2} + {b.Rate * 100}% of income above R{b.MinIncome:N0}"
                })
            });
        }
    }
}
