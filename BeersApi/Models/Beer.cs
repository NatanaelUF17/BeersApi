using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeersApi.Models
{
    public class Beer : BaseModel
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } 
        public float Price { get; set; }
        [Required]
        public int BeerTypeId { get; set; }
        [ForeignKey("BeerTypeId")]
        public BeerType BeerType { get; set; }
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Country Country { get; set; }
        [MaxLength(2048)]
        public string ImageUrl { get; set; } 
    }
}