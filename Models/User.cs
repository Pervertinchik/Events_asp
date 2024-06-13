using System.ComponentModel.DataAnnotations;

namespace Events_asp.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public List<EventDate> eventDates { get; set; }
    }
}
