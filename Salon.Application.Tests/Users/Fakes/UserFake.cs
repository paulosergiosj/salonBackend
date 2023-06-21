using MongoDB.Bson;
using Salon.Domain.Models.Enums;
using Salon.Domain.Users.Contracts;
using System;
using BCryptNet = BCrypt.Net.BCrypt;
using UserEntity = Salon.Domain.Users.Entities.User;

namespace Salon.Application.Tests.Users.Fakes
{
    public static class UserFake
    {
        public static ObjectId IdFoo = ObjectId.Parse("63e5cdeebf4932a2d1e8ba81");
        public const string LOGIN_FOO = "Foo";
        public const string PASSWORD_FOO = "PASSWORD_Bar";
        public const string NAME_FOO = "Foo_Name";
        public static DateTime BIRTHDATE_FOO = new DateTime(1988, 10, 10).ToUniversalTime();
        public const string EMAIL_FOO = "Foo@gmail.com";

        public const string LOGIN_JAMES = "James";
        public const string PASSWORD_JAMES = "PASSWORD_James";
        public const string NAME_JAMES = "James Jhonson";
        public static DateTime BIRTHDATE_JAMES = new DateTime(1976, 01, 02).ToUniversalTime();
        public const string EMAIL_JAMES = "james.jhonsons@hotmail.com";

        public const string LOGIN_ROBERT = "Robert";
        public const string PASSWORD_ROBERT = "PASSWORD_ROBERT";
        public const string NAME_ROBERT = "Robert Jhonson";
        public static DateTime BIRTHDATE_ROBERT = new DateTime(1988, 10, 04).ToUniversalTime();
        public const string EMAIL_ROBERT = "Robert.Smith@outlook.com";

        public static UserEntity GetUserEntity(string login, string password, string name, string email, ObjectId? id = null)
        {
            var user = new UserEntity(login, password, name)
                .InformEmail(email);
            user.Id = id ?? ObjectId.Empty;

            return user;
        }

        public static UserEntity GetUserEntity()
            => GetUserEntity(LOGIN_FOO, BCryptNet.HashPassword(PASSWORD_FOO), NAME_FOO, EMAIL_FOO, IdFoo);

        public static UserEntity GetUserRobert()
            => GetUserEntity(LOGIN_ROBERT, BCryptNet.HashPassword(PASSWORD_ROBERT), NAME_ROBERT, EMAIL_ROBERT);

        public static UserCommand GetUserCommand()
        {
            return new UserCommand()
            {
                Login = LOGIN_JAMES,
                Password = PASSWORD_JAMES,
                Name = NAME_JAMES,
                Email = EMAIL_JAMES
            };
        }

        public static UserEntity GetUserJames()
            => GetUserEntity(LOGIN_JAMES, BCryptNet.HashPassword(PASSWORD_JAMES), NAME_JAMES, EMAIL_JAMES);
    }
}
