using System.ComponentModel.DataAnnotations;

namespace Exam.ViewModels.Account
{
    public class RegisterVM
    {
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }
        [MinLength(3)]
        [MaxLength(30)]
        public string SurName { get; set; }
        [MinLength(3)]
        [MaxLength(30)]
        public string Username { get; set; }
        [MinLength(3)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
