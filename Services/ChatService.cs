using System;
using System.Collections.Generic;
using System.Text.Json;

namespace WebApiDemo.Services
{
    public class ChatService : IChatService
    {
        private List<User> users;
        private List<Message> messages;
        private List<Chat> chats;

        private string usersFilePath = "/Users/svtrev/Downloads/WebApiDemo/WebApiDemo/Json/users.json";
        private string chatsFilePath = "/Users/svtrev/Downloads/WebApiDemo/WebApiDemo/Json/chats.json";
        private string messagesFilePath = "/Users/svtrev/Downloads/WebApiDemo/WebApiDemo/Json/messages.json";

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
        }

        public User GetUserById(int id)
        {
            return users.Find(u => u.id == id);
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
        }
    }

}

