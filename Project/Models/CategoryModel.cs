using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models
{
    [Table("categories")]
    public class CategoryModel
    {
        public int Id {get; set;}
        [Column("name")]
        public string CategoryName { get; set; }
        [Column("description")]
        public string CategoryDescription { get; set; }
        [JsonIgnore]
        public virtual ICollection<OfferModel> Offers { get; set; }
        public CategoryModel()
        { }
    }
}