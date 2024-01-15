using Hotel.Domain.Interfaces;
using Hotel.Domain.Model;
using Hotel.Persistence.Exceptions;
using System.Data.SqlClient;

namespace Hotel.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddCustomer(Customer customer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert into Address table
                        string addressQuery =
                            "INSERT INTO Address (City, postalCode, Street, HouseNumber) VALUES (@City, @Zip, @Street, @HouseNumber); SELECT SCOPE_IDENTITY();";

                        using (
                            SqlCommand addressCommand = new SqlCommand(
                                addressQuery,
                                connection,
                                transaction
                            )
                        )
                        {
                            addressCommand.Parameters.AddWithValue(
                                "@City",
                                customer.Contact.Address.City
                            );
                            addressCommand.Parameters.AddWithValue(
                                "@Zip",
                                customer.Contact.Address.PostalCode
                            );
                            addressCommand.Parameters.AddWithValue(
                                "@Street",
                                customer.Contact.Address.Street
                            );
                            addressCommand.Parameters.AddWithValue(
                                "@HouseNumber",
                                customer.Contact.Address.HouseNumber
                            );

                            int addressId = Convert.ToInt32(addressCommand.ExecuteScalar());

                            // Insert into Contact table
                            string contactQuery =
                                "INSERT INTO ContactInfo (Email, Phone, AddressId) VALUES (@Email, @Phone, @AddressId); SELECT SCOPE_IDENTITY();";

                            using (
                                SqlCommand contactCommand = new SqlCommand(
                                    contactQuery,
                                    connection,
                                    transaction
                                )
                            )
                            {
                                contactCommand.Parameters.AddWithValue(
                                    "@Email",
                                    customer.Contact.Email
                                );
                                contactCommand.Parameters.AddWithValue(
                                    "@Phone",
                                    customer.Contact.Phone
                                );
                                contactCommand.Parameters.AddWithValue("@AddressId", addressId);

                                int contactId = Convert.ToInt32(contactCommand.ExecuteScalar());

                                // Insert into Customer table
                                string customerQuery =
                                    "INSERT INTO Customer (Name, ContactId) VALUES (@Name, @ContactId); SELECT SCOPE_IDENTITY();";

                                using (
                                    SqlCommand customerCommand = new SqlCommand(
                                        customerQuery,
                                        connection,
                                        transaction
                                    )
                                )
                                {
                                    customerCommand.Parameters.AddWithValue("@Name", customer.Name);
                                    customerCommand.Parameters.AddWithValue(
                                        "@ContactId",
                                        contactId
                                    );

                                    int customerId = Convert.ToInt32(
                                        customerCommand.ExecuteScalar()
                                    );

                                    // Insert into Member table
                                    string memberQuery =
                                        "INSERT INTO Member (Name, Birthday, CustomerId) VALUES (@Name, @Birthday, @CustomerId);";

                                    foreach (var member in customer.Members)
                                    {
                                        using (
                                            SqlCommand memberCommand = new SqlCommand(
                                                memberQuery,
                                                connection,
                                                transaction
                                            )
                                        )
                                        {
                                            memberCommand.Parameters.AddWithValue(
                                                "@Name",
                                                member.Name
                                            );
                                            memberCommand.Parameters.AddWithValue(
                                                "@Birthday",
                                                new DateTime(
                                                    member.Birthday.Year,
                                                    member.Birthday.Month,
                                                    member.Birthday.Day
                                                )
                                            );
                                            memberCommand.Parameters.AddWithValue(
                                                "@CustomerId",
                                                customerId
                                            );

                                            memberCommand.ExecuteNonQuery();
                                        }
                                    }

                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Update Address table
                        string addressQuery =
                            @"
                    UPDATE Address 
                    SET City = @City, PostalCode = @Zip, Street = @Street, HouseNumber = @HouseNumber
                    WHERE Id = @AddressId";

                        using (
                            SqlCommand addressCommand = new SqlCommand(
                                addressQuery,
                                connection,
                                transaction
                            )
                        )
                        {
                            addressCommand.Parameters.AddWithValue(
                                "@City",
                                customer.Contact.Address.City
                            );
                            addressCommand.Parameters.AddWithValue(
                                "@Zip",
                                customer.Contact.Address.PostalCode
                            );
                            addressCommand.Parameters.AddWithValue(
                                "@Street",
                                customer.Contact.Address.Street
                            );
                            addressCommand.Parameters.AddWithValue(
                                "@HouseNumber",
                                customer.Contact.Address.HouseNumber
                            );
                            addressCommand.Parameters.AddWithValue(
                                "@AddressId",
                                customer.Contact.Address.Id
                            );

                            addressCommand.ExecuteNonQuery();
                        }

                        // Update ContactInfo table
                        string contactQuery =
                            @"
                    UPDATE ContactInfo 
                    SET Email = @Email, Phone = @Phone
                    WHERE Id = @ContactId";

                        using (
                            SqlCommand contactCommand = new SqlCommand(
                                contactQuery,
                                connection,
                                transaction
                            )
                        )
                        {
                            contactCommand.Parameters.AddWithValue(
                                "@Email",
                                customer.Contact.Email
                            );
                            contactCommand.Parameters.AddWithValue(
                                "@Phone",
                                customer.Contact.Phone
                            );
                            contactCommand.Parameters.AddWithValue(
                                "@ContactId",
                                customer.Contact.Id
                            );

                            contactCommand.ExecuteNonQuery();
                        }

                        // Update Customer table
                        string customerQuery =
                            @"
                    UPDATE Customer 
                    SET Name = @Name
                    WHERE Id = @CustomerId";

                        using (
                            SqlCommand customerCommand = new SqlCommand(
                                customerQuery,
                                connection,
                                transaction
                            )
                        )
                        {
                            customerCommand.Parameters.AddWithValue("@Name", customer.Name);
                            customerCommand.Parameters.AddWithValue("@CustomerId", customer.Id);

                            customerCommand.ExecuteNonQuery();
                        }

                        // Delete existing members
                        string deleteMembersQuery =
                            "DELETE FROM Member WHERE CustomerId = @CustomerId";
                        using (
                            SqlCommand deleteMembersCommand = new SqlCommand(
                                deleteMembersQuery,
                                connection,
                                transaction
                            )
                        )
                        {
                            deleteMembersCommand.Parameters.AddWithValue(
                                "@CustomerId",
                                customer.Id
                            );
                            deleteMembersCommand.ExecuteNonQuery();
                        }

                        // Insert new members
                        string insertMemberQuery =
                            "INSERT INTO Member (Name, Birthday, CustomerId) VALUES (@Name, @Birthday, @CustomerId)";
                        foreach (var member in customer.Members)
                        {
                            using (
                                SqlCommand insertMemberCommand = new SqlCommand(
                                    insertMemberQuery,
                                    connection,
                                    transaction
                                )
                            )
                            {
                                insertMemberCommand.Parameters.AddWithValue("@Name", member.Name);
                                insertMemberCommand.Parameters.AddWithValue(
                                    "@Birthday",
                                    new DateTime(
                                        member.Birthday.Year,
                                        member.Birthday.Month,
                                        member.Birthday.Day
                                    )
                                );
                                insertMemberCommand.Parameters.AddWithValue(
                                    "@CustomerId",
                                    customer.Id
                                );

                                insertMemberCommand.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void DeleteCustomer(int customerId)
        {
            // Implement soft-delete logic or additional checks as needed
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "UPDATE Customer SET SoftDeleted = 1 WHERE Id = @CustomerId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public Customer GetCustomerById(int customerId)
        {
            return MapCustomerFromReader(customerId);
        }

        private Customer MapCustomerFromReader(int customerId)
        {
            string query = "SELECT * FROM Customer WHERE Id = @CustomerId";

            int id;
            string name;
            int contactId;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            id = reader.GetInt32(reader.GetOrdinal("Id"));
                            name = reader.GetString(reader.GetOrdinal("Name"));
                            contactId = reader.GetInt32(reader.GetOrdinal("ContactId"));
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                connection.Close();
            }
            // Fetch contact information
            ContactInfo contact = GetContactInfoById(contactId);

            // Fetch members
            List<Member> members = GetMembersByCustomerId(id);

            Customer customer = new Customer(id, name, contact);
            customer.Members = members;

            return customer;
        }

        private ContactInfo GetContactInfoById(int contactId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string contactQuery = "SELECT * FROM ContactInfo WHERE Id = @ContactId";
                using (SqlCommand contactCommand = new SqlCommand(contactQuery, connection))
                {
                    contactCommand.Parameters.AddWithValue("@ContactId", contactId);

                    using (SqlDataReader contactReader = contactCommand.ExecuteReader())
                    {
                        if (contactReader.Read())
                        {
                            int id = contactReader.GetInt32(contactReader.GetOrdinal("Id"));
                            string email = contactReader.GetString(
                                contactReader.GetOrdinal("Email")
                            );
                            string phone = contactReader.GetString(
                                contactReader.GetOrdinal("Phone")
                            );
                            int addressId = contactReader.GetInt32(
                                contactReader.GetOrdinal("AddressId")
                            );

                            // Close the reader here
                            contactReader.Close();

                            // Fetch address
                            Address address = GetAddressById(addressId, connection);

                            ContactInfo contact = new ContactInfo(id, email, phone, address);
                            return contact;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        private Address GetAddressById(int addressId, SqlConnection connection)
        {
            string addressQuery = "SELECT * FROM Address WHERE Id = @AddressId";
            using (SqlCommand addressCommand = new SqlCommand(addressQuery, connection))
            {
                addressCommand.Parameters.AddWithValue("@AddressId", addressId);

                using (SqlDataReader addressReader = addressCommand.ExecuteReader())
                {
                    if (addressReader.Read())
                    {
                        string city = addressReader.GetString(addressReader.GetOrdinal("City"));
                        string postalCode = addressReader.GetString(
                            addressReader.GetOrdinal("PostalCode")
                        );
                        string street = addressReader.GetString(addressReader.GetOrdinal("Street"));
                        string houseNumber = addressReader.GetString(
                            addressReader.GetOrdinal("HouseNumber")
                        );

                        Address address = new Address(
                            addressId,
                            city,
                            street,
                            postalCode,
                            houseNumber
                        );
                        return address;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        private List<Member> GetMembersByCustomerId(int customerId)
        {
            List<Member> members = new List<Member>();

            string memberQuery = "SELECT * FROM Member WHERE CustomerId = @CustomerId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand memberCommand = new SqlCommand(memberQuery, connection))
            {
                connection.Open();
                memberCommand.Parameters.AddWithValue("@CustomerId", customerId);

                using (SqlDataReader memberReader = memberCommand.ExecuteReader())
                {
                    while (memberReader.Read())
                    {
                        int id = memberReader.GetInt32(memberReader.GetOrdinal("Id"));
                        string memberName = memberReader.GetString(memberReader.GetOrdinal("Name"));
                        DateTime birthdayDateTime = memberReader.GetDateTime(
                            memberReader.GetOrdinal("Birthday")
                        );
                        DateOnly birthday = DateOnly.FromDateTime(birthdayDateTime);

                        Member member = new Member(id, memberName, birthday);
                        members.Add(member);
                    }
                }
            }

            return members;
        }
    }
}
