using System.Collections;

namespace Api.Models;

/**
 * Confused whats the need of this class? Requirements says we can't 
 * have more than one spouse/domestic-partner per employee, but that
 * verification should be part of other APIs (like create employee, add dependent etc)
 * which are out of scope of this project. 
 * The next best thing I can think of is to apply this check in Employee Model class. 
 * We need to verify this at two locations:
 * 1) When setting the whole Dependents property of an employee
 * 2) When adding a single dependent to an employee.
 * The first verification is take care of in the Setter method of the Dependents property 
 * in Employee class. 
 * The second part is trickier and hence required this custom collection, where the Add method
 * is overriden to apply this check.
 */
public class DependentCollection : ICollection<Dependent>
    {
        private IList<Dependent> _backingList;

        public DependentCollection(){
            _backingList = new List<Dependent>();
        }

        public int Count => _backingList.Count;

        public bool IsReadOnly => _backingList.IsReadOnly;

        public void Add(Dependent dependent)
        {
            //can not add multiple partners to dependents
            if((dependent.Relationship == Relationship.Spouse ||
                dependent.Relationship == Relationship.DomesticPartner) &&
                Employee.GetPartnerCount(_backingList) > 0)
            {
                throw new InvalidOperationException("More than one Domestic partner or Spouse not allowed!");
            }
            else
            {
                _backingList.Add(dependent);
            }
        }

        public void Clear()
        {
            _backingList.Clear();
        }

        public bool Contains(Dependent item)
        {
            return _backingList.Contains(item);
        }

        public void CopyTo(Dependent[] array, int arrayIndex)
        {
            _backingList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Dependent> GetEnumerator()
        {
            return _backingList.GetEnumerator();
        }

        public bool Remove(Dependent item)
        {
            return _backingList.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _backingList.GetEnumerator();
        }
}

