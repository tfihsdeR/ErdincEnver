using RemoteAlohWork.Core;

namespace RemoteAlohWork.Data.Entities
{
    public class City : EntityBase
    {
        public string CityName { get; set; }
        public int CountryId { get; set; }

        public Country Country { get; set; }
        public ICollection<District> Districts { get; set; }
    }
}
