using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events_asp.Models
{
    public class EventTime
    {
        public int EventTimeId { get; set; }

        public DateTime DateTime_ { get; set; }
       // [Required]

        public string Name {  get; set; }
               
        [ForeignKey("EventDateId")]
        public EventDate EventDate { get; set; }
        public int EventDateId { get; set; }

    }
}
