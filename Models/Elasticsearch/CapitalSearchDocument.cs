using KhalidAbuhakmeh.AspNetCore.Search.Models.Csv;
using Nest;

namespace KhalidAbuhakmeh.AspNetCore.Search.Models.Elasticsearch
{
    public class CapitalSearchDocument
    {
        public CapitalSearchDocument()
        {
        }

        public CapitalSearchDocument(CapitalCityRecord record)
        {
            Id = record.Id;
            Names = new[] {record.City, record.CityAscii, record.Country};
            Country = record.Country;
            // Elasticsearch supports GeoPoints as Arrays
            Location = new[] {record.Longitude, record.Latitude};
            Data = record;
        }
        
        
        public string Id { get; set; }
        
        // We want to index the many variations
        // of a capital city, so we store the strings
        // in an array.
        //
        // We also want to index and search differently
        [Text(Analyzer = "standard", SearchAnalyzer = "standard")]
        public string[] Names { get; set; }
        
        // we want to filter by country
        [Keyword]
        public string Country { get; set; }
        
        [Object(Enabled = false)]
        public CapitalCityRecord Data { get; set; }
        
        // store location
        [GeoPoint]
        public decimal[] Location { get; set; }
    }
}