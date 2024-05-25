using System;
namespace WebApiDemo
{
    public class TokenData
    {
        public string Token { get; set; }
        public int user_id { get; set; }
        public DateTime createTime { get; set; }
        public DateTime lifeTime { get; set; }
    }
}

