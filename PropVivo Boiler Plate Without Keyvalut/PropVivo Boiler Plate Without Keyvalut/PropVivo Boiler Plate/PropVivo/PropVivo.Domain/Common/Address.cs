namespace PropVivo.Domain.Common
{
    public class Address
    {
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? AddressId { get; set; }
        public string? City { get; set; }
        public string? CityId { get; set; }
        public string? Country { get; set; }
        public string? CountryId { get; set; }
        public string? County { get; set; }

        public string FullAddress
        {
            get
            {
                var address2 = String.IsNullOrWhiteSpace(this.Address2) ? String.Empty : $", {this.Address2}";
                var city = String.IsNullOrWhiteSpace(this.City) ? String.Empty : $", {this.City}";
                var state = String.IsNullOrWhiteSpace(this.State) ? String.Empty : $", {this.State}";
                var county = String.IsNullOrWhiteSpace(this.County) ? String.Empty : $", {this.County}";
                var country = String.IsNullOrWhiteSpace(this.Country) ? String.Empty : $", {this.Country}";
                var zip = String.IsNullOrWhiteSpace(this.ZipCode) ? String.Empty : $", {this.ZipCode}";

                return $"{this.Address1}{address2}{city}{state}{county}{country}{zip}";
            }
        }

        public string? State { get; set; }
        public string? StateId { get; set; }
        public string? UnitNumber { get; set; }
        public string? ZipCode { get; set; }
        public string? ZipCodeId { get; set; }
    }
}