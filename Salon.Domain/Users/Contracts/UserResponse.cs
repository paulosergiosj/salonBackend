using Salon.Domain.Models.Enums;
using System;
using System.Collections.Generic;

namespace Salon.Domain.Users.Contracts
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public List<string> ContactNumbers { get; set; } = new List<string>();
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public Role Role { get; set; }
        public string Email { get; set; }
    }
}
