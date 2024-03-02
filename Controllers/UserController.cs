//using System;
//using Microsoft.AspNetCore.Mvc;
//namespace WebApiDemo.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class UserController : ControllerBase
//    {
//        private readonly ILogger<UserController> _logger;
//        public UserController(ILogger<UserController> logger)
//        {
//            _logger = logger;
//        }
//        private static List<User> users = new List<User>();

//        [HttpGet("users")]
//        public IActionResult GetUsers()
//        {
//            return Ok(users);
//        }

//        [HttpPost("addUser")]
//        public IActionResult AddUser(string username)
//        {
//            User newUser = new()
//            {
//                id = users.Count + 1,
//                name = username
//            };
//            users.Add(newUser);
//            _logger.LogInformation($"Создан новый пользователь: {newUser.name}");
//            return Ok(newUser);
//        }

//        [HttpGet("{user_id}")]
//        public IActionResult GetUserById(int user_id)
//        {
//            var user = users.Find(u => u.id == user_id);
//            if (user == null)
//            {
//                return NotFound();
//            }
//            return Ok(user);
//        }
//    }

//}




using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
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
        private static List<User> users = new List<User>();
        private string usersFilePath = "/Users/svtrev/Downloads/WebApiDemo/WebApiDemo/users.json";

        public UserController()
        {
            if (System.IO.File.Exists(usersFilePath))
            {
                var userData = System.IO.File.ReadAllText(usersFilePath);
                users = JsonSerializer.Deserialize<List<User>>(userData);
            }
        }
        [HttpPost("addUser")]
        public IActionResult AddUser(string username)
        {
            if (users.Count > 0)
            {
                int lastUserId = users.Max(u => u.id);
                User newUser = new()
                {
                    id = lastUserId + 1,
                    name = username
                };
                users.Add(newUser);
                SaveUsersToJsonFile();
                return Ok(newUser);
            }
            else
            {
                User newUser = new()
                {
                    id = 1,
                    name = username
                };
                users.Add(newUser);
                SaveUsersToJsonFile();
                return Ok(newUser);
            }
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            return Ok(users);
        }

        [HttpGet("{user_id}")]
        public IActionResult GetUserById(int user_id)
        {
            var user = users.Find(u => u.id == user_id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpDelete("{user_id}")]
        public ActionResult DeleteUserById(int user_id)
        {
            var userToRemove = users.Find(u => u.id == user_id);
            if (userToRemove == null)
            {
                return NotFound("User not found");
            }
            users.Remove(userToRemove);
            SaveUsersToJsonFile();
            return Ok("User deleted successfully");
        }



        private void SaveUsersToJsonFile()
        {
            var usersData = JsonSerializer.Serialize(users);
            System.IO.File.WriteAllText(usersFilePath, usersData);
        }
    }
}