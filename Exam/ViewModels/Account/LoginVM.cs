using System.ComponentModel.DataAnnotations;

namespace Exam.ViewModels.Account
{
    public class LoginVM
    {

        [MinLength(3)]
        [MaxLength(30)]
        public string UsernameorEmail { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsPersistant { get; set; }
    }
}
