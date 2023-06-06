using Domain.Common;

namespace Domain.Entities
{
    public class ShippingAddress : BaseEntity
    {
        public ShippingAddress(int id, string firstName, string lastName, string addressLine1, string addressLine2, string city, string state, string country)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            State = state;
            Country = country;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string AddressLine1 { get; private set; }
        public string AddressLine2 { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }

        public void UpdateAddress(string firstName, string lastName, string addressLine1, string city, string state, string country)
        {
            FirstName = firstName;
            LastName = lastName;
            AddressLine1 = addressLine1;
            City = city;
            State = state;
            Country = country;
        }
    }
}
