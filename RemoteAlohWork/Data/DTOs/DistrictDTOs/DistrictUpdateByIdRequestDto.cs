namespace RemoteAlohWork.Data.DTOs.DistrictDTOs
{
    public class DistrictUpdateByIdRequestDto
    {
        public string DistrictName { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
    }
}
