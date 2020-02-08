using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Smart.Chat.App.Models
{
    public class ChatRoomUser : IdentityUser
    {
        public ChatRoomUser()
        {
            ChatMessages = new List<ChatMessage>();
        }

        [Required]
        [Display(Name = "Public Name")]
        public string DisplayName { get; set; }
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
    }
}