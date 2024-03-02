using Microsoft.AspNetCore.Mvc;
namespace WebApiDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        //private readonly ILogger<UserController> _logger;
        //public UserController(ILogger<UserController> logger)
        //{
        //    _logger = logger;
        //}
        private readonly Services.IChatService chatService;
        public UserController(Services.IChatService services)
        {
            chatService = services;
        }
        [HttpPost("addUser")] //добавления нового пользователя
        public IActionResult AddUser(string username, string password)
        {
            if (chatService.GetAllUsers().Count > 0) //если список пользователей не пустой
            {
                int lastUserId = chatService.GetAllUsers().Max(u => u.id);
                User newUser = new()
                {
                    id = lastUserId + 1,
                    name = username
                };
                chatService.AddUser(newUser);
                return Ok(newUser);
            }
            else //если список пользователей пустой
            {
                User newUser = new()
                {
                    id = 1,
                    name = username
                };
                chatService.AddUser(newUser);
                return Ok(newUser);
            }
        }

        [HttpGet("users")] //получение всех пользователей
        public IActionResult GetUsers()
        {
            var users = chatService.GetAllUsers();
            if (users.Count == 0)
            {
                return NotFound("No users found!");
            }
            return Ok(users);
        }

        [HttpGet("{user_id}")] //нахождение пользователя по его id
        public IActionResult GetUserById(int user_id)
        {
            var user = chatService.GetUserById(user_id);
            if (user == null)
            {
                return NotFound("User not found!");
            }
            return Ok(user);
        }

        [HttpDelete("{user_id}")] //удаление пользователя по его id
        public ActionResult DeleteUserById(int user_id)
        {

            var user = chatService.GetUserById(user_id);
            if (user == null)
            {
                return NotFound("User to remove not found!");
            }
            chatService.RemoveUser(user);
            return Ok("User deleted successfully");
        }
    }
}