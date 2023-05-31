using Api.Dtos.Employee;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests
{
    public class PaycheckIntegrationTests : IntegrationTest
    {
        /**
         * Test the scenario when:
         * 1) Employee has income < 80K
         * 2) Employee has no dependent
         */
        [Fact]
        public async Task WhenAskedForSingleEmployeePaycheck_ShouldReturnCorrectAmount()
        {
            var response = await HttpClient.GetAsync("/api/v1/employees/1/paycheck");

            await response.ShouldReturn(HttpStatusCode.OK, 2439.27d);
        }

        /**
         * Test the scenario when:
         * 1) Employee has income > 80K
         * 2) Employee has multiple dependents
         * 3) Employee has no dependent > 50 yrs age
         */
        [Fact]
        public async Task WhenAskedForEmployeeWithMultipleDependentsPaycheck_ShouldReturnCorrectAmount()
        {
            var response = await HttpClient.GetAsync("/api/v1/employees/2/paycheck");

            await response.ShouldReturn(HttpStatusCode.OK, 2189.15d);
        }

        /**
        * Test the scenario when:
        * 1) Employee id does not exists in Database
        */
        [Fact]
        public async Task WhenAskedForNonExixtentEmployeePaycheck_ShouldReturn404()
        {
            var response = await HttpClient.GetAsync("/api/v1/employees/10/paycheck");

            await response.ShouldReturn(HttpStatusCode.NotFound);
        }
    }
}
