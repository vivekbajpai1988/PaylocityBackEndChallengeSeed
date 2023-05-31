using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;

namespace Api.Utils
{
    public class DataUtils
    {
        public static GetEmployeeDto ConvertToEmployeeDto(Employee employee)
        {
            return new GetEmployeeDto()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth,
                Salary = employee.Salary,
                Dependents = employee.Dependents.Select(dependent => new GetDependentDto()
                {
                    Id = dependent.Id,
                    FirstName = dependent.FirstName,
                    LastName = dependent.LastName,
                    DateOfBirth = dependent.DateOfBirth,
                    Relationship = dependent.Relationship
                })
                .ToList()
            };
        }

        public static GetDependentDto ConvertToDependentDto(Dependent dependent)
        {
            return new GetDependentDto()
            {
                Id = dependent.Id,
                FirstName = dependent.FirstName,
                LastName = dependent.LastName,
                DateOfBirth = dependent.DateOfBirth,
                Relationship = dependent.Relationship
            };
        }

        public static ApiResponse<T> GetSuccessApiResponse<T>(T dto)
        {
            return new ApiResponse<T> { Success = true, Data = dto };
        }

        public static ApiResponse<T> GetErrorApiResponse<T>(string errorMesasge)
        {
            return new ApiResponse<T> { Success = false, Error = errorMesasge };
        }
    }
}
