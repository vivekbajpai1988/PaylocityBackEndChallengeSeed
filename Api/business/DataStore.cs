using Api.Dtos.Dependent;
using Api.Exceptions;
using Api.Models;

namespace Api.business
{
    public class DataStore : IDataStore
    {
        //In memory store to keep our employees. In real world, we can replace
        // this store with a relational Database.
        private readonly Dictionary<int,Employee> _employees;
        private readonly Dictionary<int, Dependent> _dependents;

        public DataStore()
        {
            _employees = new Dictionary<int,Employee>();
            _dependents = new Dictionary<int, Dependent>();

            //Employee 1
            Employee employee1 = new Employee()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            };
            _employees.Add(employee1.Id, employee1);

            //Employee 2
            Employee employee2 = new Employee()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
            };

            Dependent dependent1 = new Dependent()
            {
                Id = 1,
                FirstName = "Spouse",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1998, 3, 3),
                EmployeeId = employee2.Id,
                Employee = employee2
            };

            Dependent dependent2 = new Dependent()
            {
                Id = 2,
                FirstName = "Child1",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2020, 6, 23),
                EmployeeId = employee2.Id,
                Employee = employee2
            };

            Dependent dependent3 = new Dependent()
            {
                Id = 3,
                FirstName = "Child2",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2021, 5, 18),
                EmployeeId = employee2.Id,
                Employee = employee2
            };

            _dependents.Add(dependent1.Id, dependent1);
            _dependents.Add(dependent2.Id, dependent2);
            _dependents.Add(dependent3.Id, dependent3);

            employee2
                .Dependents
                .Add(dependent1);

            employee2
                .Dependents
                .Add(dependent2);

            employee2
                .Dependents
                .Add(dependent3);

            _employees.Add(employee2.Id, employee2);

            //Employee 3
            Employee employee3 = new Employee()
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
            };

            Dependent dependent4 = new Dependent()
            {
                Id = 4,
                FirstName = "DP",
                LastName = "Jordan",
                Relationship = Relationship.DomesticPartner,
                DateOfBirth = new DateTime(1974, 1, 2),
                EmployeeId = employee3.Id,
                Employee = employee3
            };

            _dependents.Add(dependent4.Id, dependent4);

            employee3
                .Dependents
                .Add(dependent4);

            _employees.Add(employee3.Id, employee3);
        }

        public Task<List<Employee>> GetAllEmployeesAsync()
        {
            // The async call stops here as this is in memory operation,
            // but in real world scenario, we will make async calls to DB.
            return Task.FromResult(_employees.Values.ToList());
        }

        public Task<Employee> GetEmployeeAsync(int id) 
        {
            // The async call stops here as this is in memory operation,
            // but in real world scenario, we will make async calls to DB.
            if (_employees.ContainsKey(id))
            {
                return Task.FromResult(_employees [id]);
            }
            throw new EntityNotFoundException(id);
        }

        public Task<List<Dependent>> GetAllDependentsAsync()
        {
            return Task.FromResult(_dependents.Values.ToList());
        }

        public Task<Dependent> GetDependentAsync(int id)
        {
            if (_dependents.ContainsKey(id))
            {
                return Task.FromResult(_dependents[id]);
            }
            throw new EntityNotFoundException(id);
        }
    }
}
