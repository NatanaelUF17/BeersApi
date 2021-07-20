using System;
using System.ComponentModel.DataAnnotations;

namespace BeersApi.Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}