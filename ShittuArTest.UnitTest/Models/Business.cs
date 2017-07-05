using ShittuArTest.UnitTest.Logic;

namespace ShittuArTest.UnitTest.Models
{
    public class Business : Operations<Business>
    {
        public Business(string name, Address address)
        {
            Name = name;
            Address = address;

            //Set the data so that it becomes available to the base class
            _data = this;
        }
        public string Name { get; }
        public Address Address { get; }

        public override bool Equals(object obj)
        {
            if (obj is Business)
            {
                var current = obj as Business;
                return Name == current.Name & Address.Equals(current.Address);
            }
            return false;
        }

        public override int GetHashCode()
        {
            if (string.IsNullOrEmpty(Name) & Address == null)
                return 0;

            return Name.GetHashCode();
        }
    }
}
