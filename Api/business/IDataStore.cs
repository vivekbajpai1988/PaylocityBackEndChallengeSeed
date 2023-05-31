using Api.Models;

namespace Api.business
{
    public interface IDataStore
    {
        public Task<Employee> GetEmployeeAsync(int id);

        public Task<List<Employee>> GetAllEmployeesAsync();

        public Task<Dependent> GetDependentAsync(int id);
        public Task<List<Dependent>> GetAllDependentsAsync();
    }
}
