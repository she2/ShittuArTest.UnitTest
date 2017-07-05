namespace ShittuArTest.UnitTest.Models
{
    public class Address
    {
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }
        public Address(string street, string city, string state, string zipCode)
        {
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public override bool Equals(object obj)
        {
            if (obj is Address)
            {
                var current = obj as Address;
                return Street == current.Street & City == current.City & State == current.State & ZipCode == current.ZipCode;
            }
            return false;
        }

        public override int GetHashCode()
        {
            if (string.IsNullOrEmpty(Street) & string.IsNullOrEmpty(City)
                & string.IsNullOrEmpty(State) & string.IsNullOrEmpty(ZipCode))
                return 0;

            return Street.GetHashCode();
        }

    }
}
