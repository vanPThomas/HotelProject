using Hotel.Domain.Model;

namespace Hotel.Domain.Interfaces
{
    public interface IOrganisationRepository
    {
        void AddOrganisation(Organisation organisation);
        IReadOnlyList<Organisation> GetOrganisations(string filter);
        Organisation GetOrganisationById(int organisationId);
    }
}
