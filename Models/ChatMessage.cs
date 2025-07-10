using System;
using System.ComponentModel.DataAnnotations;

namespace NestFinder.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }

        public string Message { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;
    }
}
