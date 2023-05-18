using RemoteAlohWork.Core;

namespace RemoteAlohWork.Data.Entities
{
    public class District : EntityBase
    {
        public string DistrictName { get; set; }
        public int CityId { get; set; }

        public City City { get; set; }
    }
}
