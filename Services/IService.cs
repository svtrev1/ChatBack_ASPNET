using System;
using System.Collections.Generic;
namespace WebApiDemo.Services
{ 
    public interface IChatService
    {
        //нахождение по id
        User GetUserById(int id);
        Chat GetChatById(int id);
        Message GetMessageById(int id);
        //нахождение по имени id пользователя
        //вывод всего списка
        List<User> GetAllUsers();
        List<Chat> GetAllChats();
        List<Message> GetAllMessages();
        //добавление нового
        void AddUser(User newUser);
        void AddMessage(Message newMessage);
        void AddChat(Chat newChat);
        //удаление по id
        void RemoveUser(User user);
        void RemoveMessage(Message message);
        void RemoveChat(Chat chat);
        //вывод
        List<Message> GetAllMessagesByChatId(int chat_id);
        //аутентификция
        Task<string> RegisterUser(User user);
        Task<int> LoginUser(User user);
        Task LogoutUser(int user_id);
        //добавление пользователя в чат
        void AddUserInChat(int chat_id, int user_id);
        List<Chat> GetUserChats(int user_id);
        int GetChatIdByName(string chatname);
    }
}

