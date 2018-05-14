using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs.Reports
{
    public class ReportDTO
    {
        public List<ReportItemDTO> Reports { get; set; }
        [JsonProperty("sumOfIncomes")]
        public double SumOfIncomes { get; set; }
        [JsonProperty("category")]
        public string CategoryName { get; set; }
        [JsonProperty("totalNumberOfSoldOffers")]
        public int TotalNumberOfSoldOffers { get; set; }

        public ReportDTO ()
        { }
        public ReportDTO(List<ReportItemDTO> reports, double sumOfIncomes, string categoryName, int totalNumberOfSoldOffers)
        {
            Reports = reports;
            SumOfIncomes = sumOfIncomes;
            CategoryName = categoryName;
            TotalNumberOfSoldOffers = totalNumberOfSoldOffers;
        }

        public bool ShouldSerializeCategoryName()
        {
            
            return (this.SerializeSensitiveInfo);
        }
        public bool ShouldSerializeTotalNumberOfSoldOffers()
        {
            
            return (this.SerializeSensitiveInfo);
        }

        [JsonIgnore]
        public bool SerializeSensitiveInfo { get; set; }
        //ovaj property odredjuje sta ce se serijalizovati prilikom izvrsavanja
    }
}