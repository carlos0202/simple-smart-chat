using System;
using System.ComponentModel.DataAnnotations;

namespace Simple.Smart.Chat.App.Models
{
    public class ChatMessage
    {
        public long Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public DateTime DateSent { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string UserId { get; set; }
        public virtual ChatRoomUser ChatRoomUser { get; set; }
    }
}