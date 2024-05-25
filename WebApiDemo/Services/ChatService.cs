using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace WebApiDemo.Services
{
    public class ChatService : IChatService
    {
        private List<User> users;
        private List<Message> messages;
        private List<Chat> chats;
        private List<TokenData> tokens;

        private string usersFilePath = "/Users/svtrev/Downloads/WebApiDemo/WebApiDemo/Json/users.json";
        private string chatsFilePath = "/Users/svtrev/Downloads/WebApiDemo/WebApiDemo/Json/chats.json";
        private string messagesFilePath = "/Users/svtrev/Downloads/WebApiDemo/WebApiDemo/Json/messages.json";
        private string tokensFilePath = "/Users/svtrev/Downloads/WebApiDemo/WebApiDemo/Json/tokens.json";

        public ChatService()
        {
            if (File.Exists(usersFilePath))
            {
                var usersData = File.ReadAllText(usersFilePath);
                users = JsonSerializer.Deserialize<List<User>>(usersData);
            }
            else
            {
                users = new List<User>();
            }

            if (File.Exists(chatsFilePath))
            {
                var chatsData = File.ReadAllText(chatsFilePath);
                chats = JsonSerializer.Deserialize<List<Chat>>(chatsData);
            }
            else
            {
                chats = new List<Chat>();
            }

            if (File.Exists(messagesFilePath))
            {
                var messagesData = File.ReadAllText(messagesFilePath);
                messages = JsonSerializer.Deserialize<List<Message>>(messagesData);
            }
            else
            {
                messages = new List<Message>();
            }

            if (File.Exists(tokensFilePath))
            {
                var tokensData = File.ReadAllText(tokensFilePath);
                tokens = JsonSerializer.Deserialize<List<TokenData>>(tokensData);
            }
            else
            {
                tokens = new List<TokenData>();
            }
        }

        public User GetUserById(int id)
        {
            return users.Find(u => u.id == id);
        }
        public User GetUserByName(string username)
        {
            User temp = users.Find(u => u.name == username);
            if (temp == null)
            {
                return null;
            }
            return temp;
        }
        public Chat GetChatById(int id)
        {
            return chats.Find(c => c.id == id);
        }
        public Message GetMessageById(int id)
        {
            return messages.Find(m => m.id == id);
        }

        public void AddUser(User newUser)
        {
            users.Add(newUser);
            SaveData();
        }
        public void AddChat(Chat newChat)
        {
            chats.Add(newChat);
            SaveData();
        }
        public void AddMessage(Message newMessage)
        {
            messages.Add(newMessage);
            SaveData();
        }

        public List<User> GetAllUsers()
        {
            return users;
        }
        public List<Chat> GetAllChats()
        {
            return chats;
        }
        public List<Message> GetAllMessages()
        {
            return messages;
        }

        public void RemoveUser(User user)
        {
            users.Remove(user);
            SaveData();
        }
        public void RemoveChat(Chat chat)
        {
            chats.Remove(chat);
            SaveData();
        }
        public void RemoveMessage(Message message)
        {
            messages.Remove(message);
            SaveData();
        }

        //нестандартные

        public List<Message> GetAllMessagesByChatId(int chat_id)
        {
            return messages.FindAll(m => m.chat_id == chat_id);
        }



        private void SaveData()
        {
            var usersData = JsonSerializer.Serialize(users);
            File.WriteAllText(usersFilePath, usersData);

            var chatsData = JsonSerializer.Serialize(chats);
            File.WriteAllText(chatsFilePath, chatsData);

            var messagesData = JsonSerializer.Serialize(messages);
            File.WriteAllText(messagesFilePath, messagesData);

            var tokensData = JsonSerializer.Serialize(tokens);
            File.WriteAllText(tokensFilePath, tokensData);

            tokens.RemoveAll(t => t.lifeTime < DateTime.UtcNow);
        }

        //Auth

        public async Task<string> RegisterUser(User newUser)
        {
            if (users.Any(u => u.name == newUser.name))
            {
                return "Username already exist";
            }
            newUser.password = HashPassword(newUser.password);
            users.Add(newUser);
            SaveData();
            return "Register successfully!";
        }

        public async Task<int> LoginUser(User user)
        {
            user.password = HashPassword(user.password);
            User oldUser = users.FirstOrDefault(u => u.name == user.name);
            if (oldUser == null)
            {
                return -1;
            }
            if (oldUser.password != user.password)
            {
                return -2;
            }
            string token = Guid.NewGuid().ToString();
            TokenData newToken = new()
            {
                Token = token,
                user_id = oldUser.id,
                createTime = DateTime.UtcNow,
                lifeTime = DateTime.UtcNow.AddHours(1)
            };
            
            tokens.Add(newToken);
            SaveData();
            return newToken.user_id;
            //return "Auth successfully!";
        }

        public async Task LogoutUser(int user_id)
        {
            tokens.RemoveAll(t => t.user_id == user_id);
            SaveData();
        }

        private string HashPassword(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2")); // Convert byte to hexadecimal string
                }

                return builder.ToString();
            }
        }

        public void AddUserInChat(int chat_id, int user_id)
        {
            Chat chat = chats.Find(c => c.id == chat_id);
            if (chat.users_id == null)
            {
                chat.users_id = new List<int>();
            }
            chat.users_id.Add(user_id);
            int index = chats.FindIndex(c => c.id == chat_id);
            if (index != -1)
            {
                chats[index] = chat;
            }
            SaveData();
        }
        public List<Chat> GetUserChats(int user_id)
        {
            return chats.Where(chat => chat.users_id.Contains(user_id)).ToList();
        }

        public int GetChatIdByName(string chatname)
        {
            Chat temp = chats.Find(c => c.name == chatname);
            if (temp == null)
            {
                return -1;
            }
            return temp.id;
        }

        public string GetChatNameById(int chat_id)
        {
            Chat temp = chats.Find(c => c.id == chat_id);
            if (temp == null)
            {
                return null;
            }
            return temp.name;
        }
    }
}

