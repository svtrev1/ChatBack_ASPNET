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
    }
}

