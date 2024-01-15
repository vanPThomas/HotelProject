using Hotel.Domain.Exceptions;
using Hotel.Domain.Interfaces;
using Hotel.Domain.Model;
using System;
using System.Collections.Generic;

namespace Hotel.Domain.Managers
{
    public class RegistrationManager
    {
        private IRegistrationRepository _registrationRepository;

        public RegistrationManager(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        public IReadOnlyList<Registration> GetRegistrations(string filter)
        {
            try
            {
                return _registrationRepository.GetRegistrations(filter);
            }
            catch (Exception ex)
            {
                throw new RegistrationManagerException("GetRegistrations", ex);
            }
        }

        public void AddRegistration(Registration registration)
        {
            try
            {
                ValidateRegistrationData(registration);
                _registrationRepository.AddRegistration(registration);
            }
            catch (Exception ex)
            {
                throw new RegistrationManagerException("AddRegistration", ex);
            }
        }

        public void UpdateRegistration(Registration registration)
        {
            try
            {
                ValidateRegistrationData(registration);
                _registrationRepository.UpdateRegistration(registration);
            }
            catch (Exception ex)
            {
                throw new RegistrationManagerException("UpdateRegistration", ex);
            }
        }

        public void DeleteRegistration(int registrationId)
        {
            try
            {
                // Soft-delete logic or additional checks can be added here
                _registrationRepository.DeleteRegistration(registrationId);
            }
            catch (Exception ex)
            {
                throw new RegistrationManagerException("DeleteRegistration", ex);
            }
        }

        private void ValidateRegistrationData(Registration registration)
        {
            // Additional validation logic can be added here
            if (registration.Members.Count == 0)
            {
                throw new RegistrationManagerException(
                    "Registration must have at least one member."
                );
            }

            // Add more validation as needed
        }

        public void RemoveMemberFromRegistration(int registrationId, int memberId)
        {
            try
            {
                _registrationRepository.RemoveMemberFromRegistration(registrationId, memberId);
            }
            catch (Exception ex)
            {
                throw new RegistrationManagerException("RemoveMemberFromRegistration", ex);
            }
        }

        private void ValidateMemberData(Member member)
        {
            // Additional validation logic can be added here
            if (string.IsNullOrWhiteSpace(member.Name))
            {
                throw new RegistrationManagerException("Member name cannot be empty.");
            }

            // Add more validation as needed
        }
    }
}
