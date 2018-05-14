using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs.Reports
{
    public class ReportItemDTO
    {
        [JsonProperty("date")]
        public DateTime ReportDate { get; set; }
        [JsonProperty("income")]
        public double Income { get; set; }
        [JsonProperty("numberOfOffers")]
        public int NumberOfOffers { get; set; }

        public ReportItemDTO()
        { }

        public ReportItemDTO(DateTime reportDate, double income, int numberOfOffers)
        {
            ReportDate = reportDate;
            Income = income;
            NumberOfOffers = numberOfOffers;
        }

        public bool ShouldSerializeNumberOfOffers()
        {
            // NumberOfOffers će biti serijalizovano ako je SerializeSensitiveInfo == true
            return (this.SerializeSensitiveInfo);
        }
      
        [JsonIgnore]
        public bool SerializeSensitiveInfo { get; set; }
        //ovaj property odredjuje sta ce se serijalizovati prilikom izvrsavanja
    }
}