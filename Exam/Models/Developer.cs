using Exam.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class Developer:BaseEntity
    {
        [MaxLength(30)]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        public string Image { get; set; }
        public string Description { get; set; }
        public int PositionId { get; set; }
        public Position Positions { get; set; }

    }
}
