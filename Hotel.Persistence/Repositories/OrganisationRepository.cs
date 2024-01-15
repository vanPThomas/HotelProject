using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Hotel.Domain.Exceptions;
using Hotel.Domain.Interfaces;
using Hotel.Domain.Model;

namespace Hotel.Persistence.Repositories
{
    public class OrganisationRepository : IOrganisationRepository
    {
        private readonly string _connectionString;

        public OrganisationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IReadOnlyList<Organisation> GetOrganisations(string filter)
        {
            List<Organisation> organisations = new List<Organisation>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Organisation WHERE Name LIKE @Filter";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Filter", $"%{filter}%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Organisation organisation = MapOrganisationFromReader(reader);
                            organisations.Add(organisation);
                        }
                    }
                }
            }

            return organisations;
        }

        public Organisation GetOrganisationById(int organisationId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Organisation WHERE Id = @OrganisationId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrganisationId", organisationId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Organisation organisation = new Organisation
                            {
                                Id = (int)reader["Id"],
                                Name = reader["Name"].ToString(),
                                ContactInfo = GetContactInfoById((int)reader["ContactId"])
                            };

                            return organisation;
                        }
                    }
                }
            }

            return null; // Return null if no organization is found with the given ID
        }

        private ContactInfo GetContactInfoById(int contactId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM ContactInfo WHERE Id = @ContactId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ContactId", contactId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ContactInfo contactInfo = new ContactInfo
                            {
                                Id = (int)reader["Id"],
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Address = GetAddressById((int)reader["AddressId"])
                            };

                            return contactInfo;
                        }
                    }
                }
            }

            return null;
        }

        private Address GetAddressById(int addressId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Address WHERE Id = @AddressId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AddressId", addressId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Address address = new Address
                            {
                                Id = (int)reader["Id"],
                                City = reader["City"].ToString(),
                                PostalCode = reader["PostalCode"].ToString(),
                                HouseNumber = reader["HouseNumber"].ToString(),
                                Street = reader["Street"].ToString()
                            };

                            return address;
                        }
                    }
                }
            }

            return null; // Return null if no address is found with the given ID
        }

        public void AddOrganisation(Organisation organisation)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Add ContactInfo first to get the generated Id
                    AddContactInfo(connection, transaction, organisation.ContactInfo);

                    // Retrieve the generated ContactInfo Id
                    int contactId = GetContactInfoId(
                        connection,
                        transaction,
                        organisation.ContactInfo
                    );

                    // Insert Organisation with the ContactInfo Id
                    string query =
                        "INSERT INTO Organisation (Name, ContactId) VALUES (@Name, @ContactId)";
                    using (SqlCommand command = new SqlCommand(query, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@Name", organisation.Name);
                        command.Parameters.AddWithValue("@ContactId", contactId);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Handle exception and log or throw as needed
                    transaction.Rollback();
                    throw new OrganisationManagerException("AddOrganisation", ex);
                }
            }
        }

        private Organisation MapOrganisationFromReader(SqlDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("Id"));
            string name = reader.GetString(reader.GetOrdinal("Name"));
            int contactId = reader.GetInt32(reader.GetOrdinal("ContactId"));

            // You may need to fetch contact information separately depending on your schema

            Organisation organisation = new Organisation
            {
                Id = id,
                Name = name,
                // Populate other properties as needed
            };

            return organisation;
        }

        private void AddAddress(
            SqlConnection connection,
            SqlTransaction transaction,
            Address address
        )
        {
            string addressQuery =
                "INSERT INTO Address (City, PostalCode, HouseNumber, Street) VALUES (@City, @PostalCode, @HouseNumber, @Street)";
            using (SqlCommand command = new SqlCommand(addressQuery, connection, transaction))
            {
                command.Parameters.AddWithValue("@City", address.City);
                command.Parameters.AddWithValue("@PostalCode", address.PostalCode);
                command.Parameters.AddWithValue("@HouseNumber", address.HouseNumber);
                command.Parameters.AddWithValue("@Street", address.Street);

                command.ExecuteNonQuery();
            }
        }

        private int GetContactInfoId(
            SqlConnection connection,
            SqlTransaction transaction,
            ContactInfo contactInfo
        )
        {
            string query = "SELECT Id FROM ContactInfo WHERE Email = @Email";
            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@Email", contactInfo.Email);

                // ExecuteScalar to get the generated Id
                return (int)command.ExecuteScalar();
            }
        }

        private int GetAddressId(
            SqlConnection connection,
            SqlTransaction transaction,
            Address address
        )
        {
            string query =
                "SELECT Id FROM Address WHERE City = @City AND PostalCode = @PostalCode AND HouseNumber = @HouseNumber AND Street = @Street";
            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@City", address.City);
                command.Parameters.AddWithValue("@PostalCode", address.PostalCode);
                command.Parameters.AddWithValue("@HouseNumber", address.HouseNumber);
                command.Parameters.AddWithValue("@Street", address.Street);

                // ExecuteScalar to get the generated Id
                return (int)command.ExecuteScalar();
            }
        }

        private void AddContactInfo(
            SqlConnection connection,
            SqlTransaction transaction,
            ContactInfo contactInfo
        )
        {
            string contactInfoQuery =
                "INSERT INTO ContactInfo (Email, Phone, AddressId) VALUES (@Email, @Phone, @AddressId)";
            using (SqlCommand command = new SqlCommand(contactInfoQuery, connection, transaction))
            {
                command.Parameters.AddWithValue("@Email", contactInfo.Email);
                command.Parameters.AddWithValue("@Phone", contactInfo.Phone);

                // Add the Address first to get the generated Id
                AddAddress(connection, transaction, contactInfo.Address);

                // Retrieve the generated Address Id
                int addressId = GetAddressId(connection, transaction, contactInfo.Address);
                command.Parameters.AddWithValue("@AddressId", addressId);

                command.ExecuteNonQuery();
            }
        }
    }
}
