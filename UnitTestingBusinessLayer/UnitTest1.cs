using Hotel.Domain.Exceptions;
using Hotel.Domain.Managers;
using Hotel.Domain.Model;
using Hotel.Util;

namespace UnitTestingBusinessLayer
{
    public class UnitTest1
    {
        private CustomerManager _customerManager = new CustomerManager(
            RepositoryFactory.CustomerRepository
        );

        [Fact]
        public void ValidateCustomerData_ValidCustomer_ReturnsTrue()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "John Doe",
                Contact = new ContactInfo { Email = "john@example.com", Phone = "123456789" }
            };

            // Act
            var result = _customerManager.ValidateCustomerData(customer);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateCustomerData_EmptyName_ReturnsFalse()
        {
            // Arrange
            var customer = new Customer
            {
                Name = string.Empty,
                Contact = new ContactInfo { Email = "john@example.com", Phone = "123456789" }
            };

            // Act
            var result = _customerManager.ValidateCustomerData(customer);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateCustomerData_LongName_ReturnsFalse()
        {
            // Arrange
            var customer = new Customer
            {
                Name = new string('A', 501), // exceeding the 500-character limit
                Contact = new ContactInfo { Email = "john@example.com", Phone = "123456789" }
            };

            // Act
            var result = _customerManager.ValidateCustomerData(customer);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateCustomerData_InvalidEmail_ReturnsFalse()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "John Doe",
                Contact = new ContactInfo
                {
                    Email = "invalid-email", // missing @ character
                    Phone = "123456789"
                }
            };

            // Act
            var result = _customerManager.ValidateCustomerData(customer);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void UpdateCustomer_InvalidCustomerData_ThrowsException()
        {
            // Arrange
            var invalidCustomer = new Customer
            {
                // Set invalid data to trigger validation failure
                Name = string.Empty,
                Contact = new ContactInfo
                {
                    Email = "john@example.com",
                    Phone = "123" // Invalid phone number for testing
                }
            };

            // Act & Assert
            var exception = Assert.Throws<CustomerManagerException>(
                () => _customerManager.UpdateCustomer(invalidCustomer)
            );
            Assert.Equal("Error updating customer", exception.Message);
        }

        [Fact]
        public void UpdateCustomer_InvalidCustomerData_ThrowsCustomerManagerException()
        {
            // Arrange
            var invalidCustomer = new Customer
            {
                // Set invalid data to trigger validation failure
                Name = string.Empty,
                Contact = new ContactInfo
                {
                    Email = "john@example.com",
                    Phone = "123" // Invalid phone number for testing
                }
            };
            // Act & Assert
            var exception = Assert.Throws<CustomerManagerException>(
                () => _customerManager.UpdateCustomer(invalidCustomer)
            );

            // Assert the type of the thrown exception
            Assert.IsType<CustomerManagerException>(exception);
            Assert.Equal("Error updating customer", exception.Message);
        }

        [Fact]
        public void CreateCustomer_WithIdAndNameAndContact_ShouldInitializeProperties()
        {
            // Arrange
            int customerId = 1;
            string customerName = "John Doe";
            var contactInfo = new ContactInfo(
                "john@example.com",
                "123-456-7890",
                new Address("City", "Street", "12345", "1A")
            );

            // Act
            var customer = new Customer(customerId, customerName, contactInfo);

            // Assert
            Assert.Equal(customerId, customer.Id);
            Assert.Equal(customerName, customer.Name);
            Assert.Equal(contactInfo, customer.Contact);
        }

        [Fact]
        public void CreateCustomer_WithIdAndName_ShouldInitializeProperties()
        {
            // Arrange
            int customerId = 1;
            string customerName = "John Doe";

            // Act
            var customer = new Customer(customerId, customerName);

            // Assert
            Assert.Equal(customerId, customer.Id);
            Assert.Equal(customerName, customer.Name);
        }

        [Fact]
        public void CreateCustomer_DefaultConstructor_ShouldInitializeProperties()
        {
            // Act
            var customer = new Customer();

            // Assert
            Assert.Equal(0, customer.Id);
            Assert.Null(customer.Name);
            Assert.Null(customer.Contact);
            Assert.Empty(customer.Members);
        }

        [Fact]
        public void GetMembers_ShouldReturnReadOnlyList()
        {
            // Arrange
            var customer = new Customer();

            // Act
            var members = customer.GetMembers();

            // Assert
            Assert.NotNull(members);
            Assert.IsType<System.Collections.ObjectModel.ReadOnlyCollection<Member>>(members);
        }

        [Fact]
        public void AddMember_WhenNotAlreadyAdded_ShouldAddMemberToList()
        {
            // Arrange
            var customer = new Customer();
            var member = new Member(1, "Member1", new DateOnly(2000, 1, 1));

            // Act
            customer.AddMember(member);

            // Assert
            Assert.Contains(member, customer.Members);
        }

        [Fact]
        public void AddMember_WhenAlreadyAdded_ShouldThrowCustomerException()
        {
            // Arrange
            var customer = new Customer();
            var member = new Member(1, "Member1", new DateOnly(2000, 1, 1));

            // Act
            customer.AddMember(member);

            // Assert
            Assert.Throws<CustomerException>(() => customer.AddMember(member));
        }

        [Fact]
        public void RemoveMember_WhenMemberExists_ShouldRemoveMemberFromList()
        {
            // Arrange
            var customer = new Customer();
            var member = new Member(1, "Member1", new DateOnly(2000, 1, 1));
            customer.AddMember(member);

            // Act
            customer.RemoveMember(member);

            // Assert
            Assert.DoesNotContain(member, customer.Members);
        }

        [Fact]
        public void RemoveMember_WhenMemberDoesNotExist_ShouldThrowCustomerException()
        {
            // Arrange
            var customer = new Customer();
            var member = new Member(1, "Member1", new DateOnly(2000, 1, 1));

            // Act & Assert
            Assert.Throws<CustomerException>(() => customer.RemoveMember(member));
        }

        [Theory]
        [InlineData(
            1,
            "John Doe",
            "john@example.com",
            "123-456-7890",
            "City",
            "Street",
            "12345",
            "1A"
        )]
        [InlineData(
            2,
            "Jane Smith",
            "jane@example.com",
            "987-654-3210",
            "Town",
            "Avenue",
            "54321",
            "2B"
        )]
        public void CreateCustomer_WithIdNameAndContact_ShouldInitializeProperties(
            int customerId,
            string customerName,
            string email,
            string phone,
            string city,
            string street,
            string postalCode,
            string houseNumber
        )
        {
            // Arrange
            var contactInfo = new ContactInfo(
                email,
                phone,
                new Address(city, street, postalCode, houseNumber)
            );

            // Act
            var customer = new Customer(customerId, customerName, contactInfo);

            // Assert
            Assert.Equal(customerId, customer.Id);
            Assert.Equal(customerName, customer.Name);
            Assert.Equal(contactInfo, customer.Contact);
        }

        [Theory]
        [InlineData(
            "John Doe",
            "john@example.com",
            "123-456-7890",
            "City",
            "Street",
            "12345",
            "1A"
        )]
        [InlineData(
            "Jane Smith",
            "jane@example.com",
            "987-654-3210",
            "Town",
            "Avenue",
            "54321",
            "2B"
        )]
        public void CreateCustomer_WithNameAndContact_ShouldInitializeProperties(
            string customerName,
            string email,
            string phone,
            string city,
            string street,
            string postalCode,
            string houseNumber
        )
        {
            // Arrange
            var contactInfo = new ContactInfo(
                email,
                phone,
                new Address(city, street, postalCode, houseNumber)
            );

            // Act
            var customer = new Customer(customerName, contactInfo);

            // Assert
            Assert.Equal(customerName, customer.Name);
            Assert.Equal(contactInfo, customer.Contact);
        }
    }
}
