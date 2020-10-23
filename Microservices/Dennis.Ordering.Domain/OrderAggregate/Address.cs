using Dennis.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dennis.Ordering.Domain
{
    public class Address : ValueObject
    {
        public Address() 
        {
        }

        public Address(string street, string city, string zipCode)
        {
            this.Street = street;
            this.City = city;
            this.ZipCode = zipCode;
        }

        public string Street { get; private set; }
        public string City { get; private set; }
        public string ZipCode { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Street;
            yield return this.City;
            yield return this.ZipCode;
        }
    }
}
