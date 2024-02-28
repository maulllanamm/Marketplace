﻿using Marketplace.Responses.Base;

namespace Marketplace.Responses
{
    public class AuthViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid CustomerId { get; set; }
        public int RoleId { get; set; }
        public string Token { get; set; }
    }


    public class LoginViewModal
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}