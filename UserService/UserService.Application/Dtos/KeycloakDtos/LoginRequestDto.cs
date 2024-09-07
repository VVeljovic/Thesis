﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Dtos.KeycloakDtos
{
    public class LoginRequestDto
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public LoginRequestDto(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
