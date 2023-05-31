using Api.Models;

namespace Api.Utils
{
    public class PaycheckUtils
    {
        private static readonly int PaychecksPerYear = 26;
        private static readonly double PerPaycheckBenefitsCost = 1000.00;
        private static readonly double ExtraBenefitsCostIncomeThreshold = 80000.00;
        private static readonly double ExtraBenefitsCostPercentage = 0.02;
        private static readonly double DependentPerMonthCost = 600.00;
        private static readonly double SeniorDependentPerMonthExtraCost = 200.00;
        private static readonly double SeniorDependentAgeThreshold = 50.00;

        /**
         * Calculates the correct paycheck amount for the given employee 
         */
        public static double CalculatePaycheck(Employee employee)
        {
            //Note: Using double instead of decimal for all calculations, as precision is not much important here.

            double perPaycheckBaseIncome = (double)(employee.Salary / PaychecksPerYear);

            double perPaycheckAfterBenefits = perPaycheckBaseIncome - GetTotalBenefitsCostPerPaycheck((double)employee.Salary);

            double perPaycheckAfterDependents = perPaycheckAfterBenefits - GetTotalDependentsCostPerPaycheck(employee);

            //Rounding to two decimal places to get income to closest cent value
            return Math.Round(perPaycheckAfterDependents, 2);
        }

        private static double GetTotalDependentsCostPerPaycheck(Employee employee)
        {
            double totalDependentMonthlyCost = employee.Dependents.Select(dependent =>
            {
                int dependentAge = GetAge(dependent.DateOfBirth);

                double seniorExtraCost = dependentAge >= SeniorDependentAgeThreshold ? SeniorDependentPerMonthExtraCost : 0;
                return DependentPerMonthCost + seniorExtraCost;
            })
                .Sum();

            double perPaycheckDependentCost = totalDependentMonthlyCost * 12 / PaychecksPerYear;
            return perPaycheckDependentCost;
        }

        private static double GetTotalBenefitsCostPerPaycheck(double salary)
        {
            double totalBenefitsDeductions = PerPaycheckBenefitsCost * 12 / PaychecksPerYear;

            if(salary > ExtraBenefitsCostIncomeThreshold)
            {
                totalBenefitsDeductions += salary * ExtraBenefitsCostPercentage / PaychecksPerYear;
            }
            return totalBenefitsDeductions;

        }

        private static int GetAge(DateTime date)
        {
            int YearsPassed = DateTime.Now.Year - date.Year;

            //Ensure we calculate age correctly for people with 49 years of age, but will turn 50 something this year/month
            if (DateTime.Now.Month < date.Month || (DateTime.Now.Month == date.Month && DateTime.Now.Day < date.Day))
            {
                YearsPassed--;
            }
            return YearsPassed;
        }
    }
}
