using RemoteAlohWork.Core;

namespace RemoteAlohWork.Data.Entities
{
    public class Country : EntityBase
    {
        public string CountryName { get; set; }

        public ICollection<City> Cities { get; set; }
    }
}
