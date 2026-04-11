namespace API_Endpoints_Salary_Calculator.Models
{

//Quick explanation of the code: So here its nothing fancy, we just defining fields used in the calculation. 





    /// <summary>
    /// Represents a single SARS tax bracket for PAYE calculation
    /// </summary>
    public class TaxBracket
    {
        /// <summary>
        /// Minimum taxable income for this bracket (inclusive)
        /// </summary>
        public decimal MinIncome { get; set; }

        /// <summary>
        /// Maximum taxable income for this bracket (inclusive, null for the highest bracket)
        /// </summary>
        public decimal? MaxIncome { get; set; }

        /// <summary>
        /// Base tax amount before applying the marginal rate
        /// </summary>
        public decimal BaseTax { get; set; }

        /// <summary>
        /// Tax rate (as decimal, e.g., 0.18 for 18%) to apply to income above MinIncome
        /// </summary>
        public decimal Rate { get; set; }


        //constructor and fields defined.
        public TaxBracket(decimal minIncome, decimal? maxIncome, decimal baseTax, decimal rate)
        {
            MinIncome = minIncome;
            MaxIncome = maxIncome;
            BaseTax = baseTax;
            Rate = rate;
        }
    }
}
