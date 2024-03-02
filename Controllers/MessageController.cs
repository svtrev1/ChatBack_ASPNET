using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace WebApiDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        public MessageController(ILogger<MessageController> logger)
        {
            _logger = logger;
        }
        private static List<Message> messages = new List<Message>();
        private string messagesFilePath = "/Users/svtrev/Downloads/WebApiDemo/WebApiDemo/messages.json";
        public MessageController()
        {
            if (System.IO.File.Exists(messagesFilePath))
            {
                var messagesData = System.IO.File.ReadAllText(messagesFilePath);
                messages = JsonSerializer.Deserialize<List<Message>>(messagesData);
            }
        }

        [HttpGet("messages")]
        public IActionResult GetMessage(int chat_id)
        {
            var chatMessages = messages.FindAll(m => m.chat_id == chat_id);
            return Ok(chatMessages);
        }
        [HttpPost("message")]
        public IActionResult AddMessage(int chat_id, string content, int user_id)
        {
            Message newMessage = new Message()
            {
                id = messages.Count + 1,
                datetime = DateTime.Now,
                chat_id = chat_id,
                content = content,
                user_id = user_id
            };
            messages.Add(newMessage);
            return Ok(newMessage);
        }
        [HttpGet("{message_id}")]
        public IActionResult GetUserById(int message_id)
        {
            var message = messages.Find(m => m.id == message_id);
            if (message == null)
            {
                return NotFound();
            }
            //var user = 
            return Ok(message);
        }
    }
}

