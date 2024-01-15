using Hotel.Domain.Exceptions;
using Hotel.Domain.Interfaces;
using Hotel.Domain.Model;
using System;
using System.Collections.Generic;

namespace Hotel.Domain.Managers
{
    public class OrganisationManager
    {
        private IOrganisationRepository _organisationRepository;

        public OrganisationManager(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        public IReadOnlyList<Organisation> GetOrganisations(string filter)
        {
            try
            {
                return _organisationRepository.GetOrganisations(filter);
            }
            catch (Exception ex)
            {
                throw new OrganisationManagerException("GetOrganisations", ex);
            }
        }

        public void AddOrganisation(Organisation organisation)
        {
            try
            {
                ValidateOrganisationData(organisation);
                _organisationRepository.AddOrganisation(organisation);
            }
            catch (Exception ex)
            {
                throw new OrganisationManagerException("AddOrganisation", ex);
            }
        }

        public Organisation GetOrganisationById(int organisationId)
        {
            try
            {
                return _organisationRepository.GetOrganisationById(organisationId);
            }
            catch (Exception ex)
            {
                throw new OrganisationManagerException("GetOrganisationById", ex);
            }
        }

        public void ValidateOrganisationData(Organisation organisation)
        {
            if (string.IsNullOrWhiteSpace(organisation.Name))
            {
                throw new OrganisationManagerException("Organisation name cannot be empty.");
            }

            if (
                string.IsNullOrWhiteSpace(organisation.ContactInfo?.Email)
                || !organisation.ContactInfo.Email.Contains("@")
            )
            {
                throw new OrganisationManagerException("Invalid email address.");
            }

            if (string.IsNullOrWhiteSpace(organisation.ContactInfo?.Phone))
            {
                throw new OrganisationManagerException("Phone number is required.");
            }

            if (
                organisation.ContactInfo?.Address == null
                || string.IsNullOrWhiteSpace(organisation.ContactInfo.Address.City)
                || string.IsNullOrWhiteSpace(organisation.ContactInfo.Address.PostalCode)
                || string.IsNullOrWhiteSpace(organisation.ContactInfo.Address.Street)
                || string.IsNullOrWhiteSpace(organisation.ContactInfo.Address.HouseNumber)
            )
            {
                throw new OrganisationManagerException("Address information is incomplete.");
            }
        }
    }
}
