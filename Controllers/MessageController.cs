using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace WebApiDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        public readonly Services.IChatService chatService;
        private readonly IHubContext<ChatHub> hubContext;

        public MessageController(Services.IChatService services, IHubContext<ChatHub> context) //конструктор
        {
            chatService = services;
            hubContext = context;
        }

        [HttpGet("messages")] //получение всех сообщений в чате по chat_id
        public IActionResult GetMessage(int chat_id)
        {
            var messages = chatService.GetAllMessagesByChatId(chat_id);
            if (messages.Count == 0)
            {
                return NotFound("No messages in this chat!");
            }
            return Ok(messages);
        }

        [HttpPost("addMessage")] //добавление нового сообщения 
        public IActionResult AddMessage(int chat_id, string content, int user_id)
        {
            Message newMessage = new Message()
            {
                id = chatService.GetAllMessages().Count + 1,
                datetime = DateTime.Now,
                chat_id = chat_id,
                content = content,
                user_id = user_id
            };
            // Проверяем, существует ли пользователь с указанным id
            if (chatService.GetUserById(newMessage.user_id) == null)
            {
                return NotFound("User not found");
            }

            // Проверяем, существует ли чат с указанным id
            if (chatService.GetChatById(newMessage.chat_id) == null)
            {
                return NotFound("Chat not found");
            }
            chatService.AddMessage(newMessage);
            hubContext.Clients.All.SendAsync("ReceiveMessage", newMessage);
            return Ok(newMessage);
        }

        [HttpGet("{message_id}")] //нахождения сообщения по его id
        public IActionResult GetMessageById(int message_id)
        {
            var message = chatService.GetMessageById(message_id);
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }

        [HttpDelete("{message_id}")]
        public ActionResult DeleteMessageById(int message_id)
        {
            var message = chatService.GetMessageById(message_id);
            if (message== null)
            {
                return NotFound("Message to remove not found");
            }
            chatService.RemoveMessage(message);
            return Ok("Message deleted successfully");
        }
    }
}

