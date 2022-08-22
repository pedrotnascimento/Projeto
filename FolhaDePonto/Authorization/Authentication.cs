﻿using Application.DTO;

namespace Application.Authorization
{
    public class Authentication : IAuthentication
    {
        public UserDTO GetSignedInUser(string token)
        {
            var mockedUser = new UserDTO
            {
                Id = 1,
                Name = "Usuario Mockado"
            };
            return mockedUser;
        }
    }
}
