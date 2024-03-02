using System;
namespace WebApiDemo
{
    public class Message
    {
        public int id { get; set; }
        public string content { get; set; }
        public DateTime datetime { get; set; }
        public int user_id { get; set; }
        public int chat_id { get; set; }
    }
}

