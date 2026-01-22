using Exam.Models;
using System.ComponentModel.DataAnnotations;

namespace Exam.Areas.AdminPanel.ViewModels
{
    public class UpdateDeveloperVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public string Photo { get; set; }
        [Required]
        public int? PositionId { get; set; }
        public List<Position>? Positions { get; set; }
    }
}