using System;
using System.Collections.Generic;
using UserManager.Data;

namespace UserManager.Tests
{
    public static class TestData
    {
        public static readonly IEnumerable<User> Users = new[]
        {
            new User()
            {
                Id = 1,
                FirstName = "Dmytro",
                LastName = "Surname",
                BirthDate = new DateTime(2000, 10, 10),
                Email = "somemail@gmail.com",
                PhoneNumber = "+3807777777",
                Info = "Personal info..."
            },
            new User()
            {
                Id = 2,
                FirstName = "Eugene",
                LastName = "Lapin",
                BirthDate = new DateTime(2000, 10, 10),
                Email = "somemail1@gmail.com",
                PhoneNumber = "+3807777771",
                Info = "Personal info..."
            },
            new User()
            {
                Id = 3,
                FirstName = "Petro",
                LastName = "Karpenko",
                BirthDate = new DateTime(2000, 10, 10),
                Email = "somemail2@gmail.com",
                PhoneNumber = "+3807777772",
                Info = "Personal info..."
            }
        };
    }
}