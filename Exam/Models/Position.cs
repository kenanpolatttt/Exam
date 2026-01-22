using Exam.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class Position:BaseEntity
    {
        [MaxLength(30)]
        [MinLength(3)]
        [Required]
        public string Name { get; set; }
        public List<Developer> Developers { get; set; }
    }
}
