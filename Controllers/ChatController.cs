using Microsoft.AspNetCore.Mvc;
namespace WebApiDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly Services.IChatService chatService;
        public ChatController(Services.IChatService services) //конструктор контроллера
        {
            chatService = services;
        }

        [HttpPost("addChat")] //добавления нового чата
        public IActionResult AddChat(string chatname)
        {
            if (chatService.GetAllChats().Count > 0) //если список чатов не пустой
            {
                int lastChatId = chatService.GetAllChats().Max(u => u.id);
                Chat newChat = new()
                {
                    id = lastChatId + 1,
                    name = chatname
                };
                chatService.AddChat(newChat);
                return Ok(newChat);
            }
            else //если список пользователей пустой
            {
                Chat newChat = new()
                {
                    id = 1,
                    name = chatname
                };
                chatService.AddChat(newChat);
                return Ok(newChat);
            }
        }

        [HttpGet("chats")] //получение списка всех чатов
        public IActionResult GetUChats()
        {
            var chats = chatService.GetAllChats();
            if (chats.Count == 0)
            {
                return NotFound("No chats found!");
            }
            return Ok(chats);
        }

        [HttpGet("{chat_id}")] //нахождение чата по его id
        public IActionResult GetChatById(int chat_id)
        {
            var chat = chatService.GetChatById(chat_id);
            if (chat == null)
            {
                return NotFound("Chat not found!");
            }
            return Ok(chat);
        }

        [HttpDelete("{chat_id}")] //удаление пользователя по его id
        public ActionResult DeleteChatById(int chat_id)
        {
            var chat = chatService.GetChatById(chat_id);
            if (chat == null)
            {
                return NotFound("Chat to remove not found!");
            }
            chatService.RemoveChat(chat);
            return Ok("Chat deleted successfully");
        }
    }
}
