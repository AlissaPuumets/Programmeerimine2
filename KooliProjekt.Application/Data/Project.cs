using System;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    // 28.11
    // Pärib Entity klassist
    public class Project : Entity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        [MaxLength(50)]
        public string Status { get; set; }
        [Required]
        public decimal Budget { get; set; }
    }
}