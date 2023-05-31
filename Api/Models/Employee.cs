namespace Api.Models;

public partial class Employee
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal Salary { get; set; }
    public DateTime DateOfBirth { get; set; }

    private DependentCollection _dependents = new DependentCollection();
    public DependentCollection Dependents {
        get => _dependents;
        set
        {
            //check that their is only one spouse or domestic partner
            
            if (GetPartnerCount(value) <=1)
            {
                _dependents = value;
            }
            else
            {
                throw new InvalidOperationException("More than one Domestic partner or Spouse not allowed!");
            }
        }
    }

    public static int GetPartnerCount(ICollection<Dependent> dependentList)
    {
        int partnerCount = dependentList.Where(dependent =>
                                         dependent.Relationship == Relationship.Spouse ||
                                         dependent.Relationship == Relationship.DomesticPartner)
                         .Count();
        return partnerCount;
    }
}
