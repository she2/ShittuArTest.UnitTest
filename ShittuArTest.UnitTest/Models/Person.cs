using ShittuArTest.UnitTest.Logic;

namespace ShittuArTest.UnitTest.Models
{
    public class Person : Operations<Person>
    {
        public Person(string firstName, string lastName, Address address)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;

            //Set the data so that it becomes available to the base class
            _data = this;
        }


        public string FirstName { get; }
        public string LastName { get; }
        public Address Address { get; }

        public override bool Equals(object obj)
        {
            if (obj is Person)
            {
                var current = obj as Person;
                return FirstName == current.FirstName & LastName == current.LastName & Address.Equals(current.Address);
            }
            return false;
        }
        public override int GetHashCode()
        {
            if (string.IsNullOrEmpty(FirstName) & string.IsNullOrEmpty(LastName) & Address == null)
                return 0;

            return FirstName.GetHashCode();
        }
    }
}