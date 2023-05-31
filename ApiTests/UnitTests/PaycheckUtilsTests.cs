using Api.Models;
using Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.UnitTests
{
    public class PaycheckUtilsTests
    {
        /**
         * Test the scenario when:
         * 1) Employee has income < 80K
         * 2) Employee has multiple dependent with all <50 yrs old.
         */
        [Fact]
        public void CorrectPaycheckAmountEmployeeWithDependents()
        {
            Employee employee = new Employee()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 72365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
            };

            Dependent dependent1 = new Dependent()
            {
                Id = 1,
                FirstName = "Spouse",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1998, 3, 3),
                EmployeeId = employee.Id,
                Employee = employee
            };

            Dependent dependent2 = new Dependent()
            {
                Id = 2,
                FirstName = "Child1",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2020, 6, 23),
                EmployeeId = employee.Id,
                Employee = employee
            };

            Dependent dependent3 = new Dependent()
            {
                Id = 3,
                FirstName = "Child2",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2021, 5, 18),
                EmployeeId = employee.Id,
                Employee = employee
            };

            employee
                .Dependents
                .Add(dependent1);

            employee
                .Dependents
                .Add(dependent2);

            employee
                .Dependents
                .Add(dependent3);

            double calculatedPaycheck = PaycheckUtils.CalculatePaycheck(employee);
            Assert.Equal(1490.97d, calculatedPaycheck);
        }

        /**
         * Test the scenario when:
         * 1) Employee has income > 80K
         * 2) Employee has no dependent.
         */
        [Fact]
        public void CorrectPaycheckAmountEmployeeHigherIncome()
        {
            Employee employee = new Employee()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 80000.10m,
                DateOfBirth = new DateTime(1999, 8, 10),
            };

            double calculatedPaycheck = PaycheckUtils.CalculatePaycheck(employee);
            Assert.Equal(2553.85d, calculatedPaycheck);
        }

        /**
         * Test the scenario when:
         * 1) Employee has income < 80K
         * 2) Employee has one dependent with age > 50 yrs.
         */
        [Fact]
        public void CorrectPaycheckAmountEmployeeOneSeniorDependent()
        {
            Employee employee = new Employee()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 79999.00m,
                DateOfBirth = new DateTime(1972, 8, 10),
            };

            Dependent seniorDependent = new Dependent()
            {
                Id = 3,
                FirstName = "Kelly",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1972, 5, 18),
                EmployeeId = employee.Id,
                Employee = employee
            };
            employee.Dependents.Add(seniorDependent);

            double calculatedPaycheck = PaycheckUtils.CalculatePaycheck(employee);
            Assert.Equal(2246.12d, calculatedPaycheck);
        }

        /**
         * Test the scenario when:
         * 1) Employee has income < 80K
         * 2) After all deductions, employee ends up with negative paycheck.
         */
        [Fact]
        public void CorrectPaycheckAmountEmployeeNegativeNetIncome()
        {
            Employee employee = new Employee()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 100.00m,
                DateOfBirth = new DateTime(1972, 8, 10),
            };

            Dependent seniorDependent = new Dependent()
            {
                Id = 3,
                FirstName = "Kelly",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1972, 5, 18),
                EmployeeId = employee.Id,
                Employee = employee
            };
            employee.Dependents.Add(seniorDependent);

            double calculatedPaycheck = PaycheckUtils.CalculatePaycheck(employee);
            Assert.Equal(-826.92d, calculatedPaycheck);
        }

        /**
        * Test the scenario when:
        * 1) Employee has income < 80K
        * 2) Employee has a dependent with age exactly equal to 49 years and 364 days.
        */
        [Fact]
        public void CorrectPaycheckAmountEmployeeWith49yrsDependent()
        {
            Employee employee = new Employee()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 79999.00m,
                DateOfBirth = new DateTime(1972, 8, 10),
            };

            Dependent seniorDependent = new Dependent()
            {
                Id = 3,
                FirstName = "Kelly",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = DateTime.Now.AddYears(-49).AddDays(-364),
                EmployeeId = employee.Id,
                Employee = employee
            };
            employee.Dependents.Add(seniorDependent);

            double calculatedPaycheck = PaycheckUtils.CalculatePaycheck(employee);
            Assert.Equal(2338.42d, calculatedPaycheck);
        }
    }
}
