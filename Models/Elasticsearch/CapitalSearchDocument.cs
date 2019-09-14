using System;
using System.Linq;
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
            // we want to do some work in setting
            // up the values that will be analyzed
            // thinking about what the user might
            // type into our search input
            Names = new[]
                {
                    record.City,
                    record.CityAscii, 
                    record.Country,
                }
                .Union(record.CityAscii.Split(' '))
                .Union(record.Country.Split(' '))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            City = record.City;
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
        [Text(
            Analyzer = Indices.IndexAnalyzerName,
            SearchAnalyzer = Indices.SearchAnalyzerName
        )]
        public string[] Names { get; set; }
        
        // we want to filter by country
        [Keyword]
        public string Country { get; set; }
        
        [Keyword]
        public string City { get; set; }
        
        [Object(Enabled = false)]
        public CapitalCityRecord Data { get; set; }
        
        // store location
        [GeoPoint]
        public decimal[] Location { get; set; }
    }
}