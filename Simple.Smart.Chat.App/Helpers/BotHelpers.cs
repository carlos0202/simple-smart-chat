using Simple.Smart.Chat.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Simple.Smart.Chat.App.Helpers
{
    public static class BotHelpers
    {
        private static readonly string commandPattern = @"(\/(?i)stock(?-i)={1})(\S+\b)$";
        public static bool IsCommand(this ChatMessage chatMessage)
        {

            return Regex.Match(chatMessage.Message, commandPattern).Success;
        }

        public static string GetCommand(this ChatMessage chatMessage)
        {
            var match = Regex.Match(chatMessage.Message, commandPattern);
            if (match.Success)
            {
                return match.Groups[2].Value;
            } else
            {
                return null;
            }
        }
    }
}
