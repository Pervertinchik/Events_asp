
using System.ComponentModel.DataAnnotations.Schema;

namespace Events_asp.Models
{
    public class EventDate
    {
        public int EventDateId { get; set; }
        
        public DateTime DateTime_ { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public int UserId { get; set; }


        public List<EventTime> eventTimes { get; set; }

    }
}
