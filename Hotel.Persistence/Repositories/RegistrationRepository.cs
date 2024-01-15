using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Hotel.Domain.Interfaces;
using Hotel.Domain.Model;

namespace Hotel.Persistence.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly string _connectionString;

        public RegistrationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IReadOnlyList<Registration> GetRegistrations()
        {
            List<Registration> registrations = new List<Registration>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Registration";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Registration registration = MapRegistrationFromReader(reader);
                            registrations.Add(registration);
                        }
                    }
                }
            }

            return registrations;
        }

        public void AddRegistration(Registration registration)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Registration (Id) VALUES (@RegistrationId)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RegistrationId", registration.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteRegistration(int registrationId)
        {
            // Implement soft-delete logic or additional checks as needed
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Registration WHERE Id = @RegistrationId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RegistrationId", registrationId);

                    command.ExecuteNonQuery();
                }
            }
        }

        private Registration MapRegistrationFromReader(SqlDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("Id"));

            Registration registration = new Registration
            {
                Id = id,
                // Populate other properties as needed
            };

            return registration;
        }

        public void UpdateRegistration(Registration registration)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query =
                    "UPDATE Registration SET /* Update fields here */ WHERE Id = @RegistrationId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RegistrationId", registration.Id);
                    // Add parameters for updating fields

                    command.ExecuteNonQuery();
                }
            }
        }

        public Registration GetRegistrationById(int registrationId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Registration WHERE Id = @RegistrationId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RegistrationId", registrationId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapRegistrationFromReader(reader);
                        }
                    }
                }
            }

            return null; // or throw an exception if needed
        }

        public IReadOnlyList<Member> GetMembersByRegistrationId(int registrationId)
        {
            List<Member> members = new List<Member>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query =
                    "SELECT M.* FROM Member M INNER JOIN RegistrationMember RM ON M.Id = RM.MemberId WHERE RM.RegistrationId = @RegistrationId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RegistrationId", registrationId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Member member = MapMemberFromReader(reader);
                            members.Add(member);
                        }
                    }
                }
            }

            return members;
        }

        public void AddMembersToRegistration(int registrationId, IEnumerable<Member> members)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                foreach (Member member in members)
                {
                    string query =
                        "INSERT INTO RegistrationMember (RegistrationId, MemberId) VALUES (@RegistrationId, @MemberId)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RegistrationId", registrationId);
                        command.Parameters.AddWithValue("@MemberId", member.Id);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void RemoveAllMembersFromRegistration(int registrationId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query =
                    "DELETE FROM RegistrationMember WHERE RegistrationId = @RegistrationId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RegistrationId", registrationId);

                    command.ExecuteNonQuery();
                }
            }
        }
        public IReadOnlyList<Registration> GetRegistrations(string filter)
        {
            List<Registration> registrations = new List<Registration>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Registration WHERE Id LIKE @Filter";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Filter", $"%{filter}%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Registration registration = MapRegistrationFromReader(reader);
                            registrations.Add(registration);
                        }
                    }
                }
            }

            return registrations;
        }

        public void RemoveMemberFromRegistration(int registrationId, int memberId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "DELETE FROM RegistrationMember WHERE RegistrationId = @RegistrationId AND MemberId = @MemberId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RegistrationId", registrationId);
                    command.Parameters.AddWithValue("@MemberId", memberId);

                    command.ExecuteNonQuery();
                }
            }
        }

        private Member MapMemberFromReader(SqlDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("Id"));
            string name = reader.GetString(reader.GetOrdinal("Name"));
            DateOnly birthday = DateOnly.FromDateTime(
                reader.GetDateTime(reader.GetOrdinal("Birthday"))
            );

            return new Member(id, name, birthday);
        }
    }
}
