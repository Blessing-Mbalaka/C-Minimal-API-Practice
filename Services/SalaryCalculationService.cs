using API_Endpoints_Salary_Calculator.Models;

namespace API_Endpoints_Salary_Calculator.Services
{
    /// <summary>
    /// Service for calculating South African salary deductions including PAYE and UIF
    /// </summary>
    public class SalaryCalculationService
    {
        // SARS 2024/2025 Tax Year Brackets (1 March 2024 - 28 February 2025)
        private readonly List<TaxBracket> _taxBrackets2024_2025 = new()
        {
            // R1 - R237,100: 18% of taxable income
            new TaxBracket(0, 237100, 0, 0.18m),

            // R237,101 - R370,500: R42,678 + 26% of taxable income above R237,100
            new TaxBracket(237101, 370500, 42678, 0.26m),

            // R370,501 - R512,800: R77,362 + 31% of taxable income above R370,500
            new TaxBracket(370501, 512800, 77362, 0.31m),

            // R512,801 - R673,000: R121,475 + 36% of taxable income above R512,800
            new TaxBracket(512801, 673000, 121475, 0.36m),

            // R673,001 - R857,900: R179,147 + 39% of taxable income above R673,000
            new TaxBracket(673001, 857900, 179147, 0.39m),

            // R857,901 - R1,817,000: R251,258 + 41% of taxable income above R857,900
            new TaxBracket(857901, 1817000, 251258, 0.41m),

            // R1,817,001 and above: R644,489 + 45% of taxable income above R1,817,000
            new TaxBracket(1817001, null, 644489, 0.45m)
        };

        /// <summary>
        /// Calculates annual PAYE (Pay As You Earn) tax based on SARS 2024/2025 tax brackets
        /// </summary>
        /// <param name="annualTaxableIncome">Annual taxable income in Rands</param>
        /// <returns>Annual PAYE tax amount</returns>
        public decimal CalculatePAYE(decimal annualTaxableIncome)
        {
            // If income is zero or negative, no tax is due
            if (annualTaxableIncome <= 0)
                return 0;

            // Find the applicable tax bracket for this income level
            TaxBracket? applicableBracket = null;

            foreach (var bracket in _taxBrackets2024_2025)
            {
                // Check if income falls within this bracket's range
                if (annualTaxableIncome >= bracket.MinIncome)
                {
                    // If there's no max (highest bracket) or income is below max
                    if (bracket.MaxIncome == null || annualTaxableIncome <= bracket.MaxIncome)
                    {
                        applicableBracket = bracket;
                        break;
                    }
                }
            }

            // If no bracket found (shouldn't happen), return 0
            if (applicableBracket == null)
                return 0;

            // Calculate tax: Base Tax + (Income above bracket minimum × Marginal Rate)
            decimal taxableAmount = annualTaxableIncome - applicableBracket.MinIncome;
            decimal tax = applicableBracket.BaseTax + (taxableAmount * applicableBracket.Rate);

            return Math.Max(0, tax); // Ensure tax is never negative
        }

        /// <summary>
        /// Calculates monthly PAYE tax from monthly gross income
        /// Note: Annualizes the income first, then divides the annual tax by 12
        /// </summary>
        /// <param name="monthlyGrossIncome">Monthly gross income in Rands</param>
        /// <returns>Monthly PAYE deduction</returns>
        public decimal CalculateMonthlyPAYE(decimal monthlyGrossIncome)
        {
            // Convert monthly income to annual for tax bracket calculation
            decimal annualIncome = monthlyGrossIncome * 12;

            // Calculate annual tax based on SARS brackets
            decimal annualTax = CalculatePAYE(annualIncome);

            // Divide by 12 to get monthly deduction
            return annualTax / 12;
        }

        /// <summary>
        /// Calculates UIF (Unemployment Insurance Fund) contribution
        /// UIF is 1% of gross salary, capped at R177.12 per month (based on max income of R17,712)
        /// NOTE: Both employee AND employer pay 1% each (total 2% to SARS)
        /// </summary>
        /// <param name="grossSalary">Monthly gross salary</param>
        /// <returns>UIF contribution amount (employee or employer portion)</returns>
        public decimal CalculateUIF(decimal grossSalary)
        {
            // UIF contribution rate (1% of gross salary)
            const decimal UIF_RATE = 0.01m;

            // Maximum UIF contribution per month (R177.12 as of 2024)
            // This is based on the UIF wage ceiling of R17,712
            const decimal UIF_CAP = 177.12m;

            // Calculate 1% of gross salary
            decimal uif = grossSalary * UIF_RATE;

            // Apply the cap - return the lesser of calculated UIF or the cap
            return Math.Min(uif, UIF_CAP);
        }

        /// <summary>
        /// Calculates SDL (Skills Development Levy) - Employer contribution only
        /// SDL is 1% of total payroll, paid by employer to SARS (NOT deducted from employee)
        /// </summary>
        /// <param name="grossSalary">Monthly gross salary</param>
        /// <returns>SDL contribution amount</returns>
        public decimal CalculateSDL(decimal grossSalary)
        {
            // SDL rate is 1% of gross payroll
            const decimal SDL_RATE = 0.01m;

            return grossSalary * SDL_RATE;
        }

        /// <summary>
        /// Gets the current tax year being used for calculations
        /// </summary>
        /// <returns>Tax year description</returns>
        public string GetCurrentTaxYear()
        {
            return "2024/2025 (1 March 2024 - 28 February 2025)";
        }

        /// <summary>
        /// Gets all tax brackets for reference/display purposes
        /// </summary>
        /// <returns>List of current tax brackets</returns>
        public IReadOnlyList<TaxBracket> GetTaxBrackets()
        {
            return _taxBrackets2024_2025.AsReadOnly();
        }
    }
}
