using Hotel.Domain.Model;
using System;
using System.Collections.Generic;

namespace Hotel.Domain.Interfaces
{
    public interface IRegistrationRepository
    {
        void AddRegistration(Registration registration);
        void DeleteRegistration(int registrationId);
        IReadOnlyList<Registration> GetRegistrations(string filter);
        void RemoveMemberFromRegistration(int registrationId, int memberId);
        void UpdateRegistration(Registration registration);

        // Additional methods
        Registration GetRegistrationById(int registrationId);
        IReadOnlyList<Member> GetMembersByRegistrationId(int registrationId);
        void AddMembersToRegistration(int registrationId, IEnumerable<Member> members);
        void RemoveAllMembersFromRegistration(int registrationId);
    }
}
