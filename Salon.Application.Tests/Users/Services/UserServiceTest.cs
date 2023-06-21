﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Salon.Application.Tests.DataLoad;
using Salon.Application.Tests.Extensions;
using Salon.Application.Tests.Modules;
using Salon.Application.Tests.Users.Fakes;
using Salon.Application.Users.Interfaces;
using Salon.Domain.Users.Contracts;
using Salon.Domain.Users.Entities;
using Salon.Domain.Users.Repositories;
using Salon.Infra.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Salon.Application.Tests.Users.Services
{
    [TestFixture]
    public class UserServiceTest
    {
        private IServiceProvider _serviceProvider;
        private IMongoDbContext _mongoDbContext;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection = new ModuleLoader(serviceCollection)
                .UseModule<StartupModule>()
                .UseModule<LoadDataModule>()
                .Load();

            _serviceProvider = serviceCollection.BuildServiceProvider();

            await new DataLoadBuilder(_serviceProvider).AddLoadAsync<User>();

            _mongoDbContext = _serviceProvider.GetRequiredService<IMongoDbContext>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _mongoDbContext.Dispose();
        }

        [Test]
        public async Task Should_Create_User_From_Command()
        {
            var command = UserFake.GetUserCommand();

            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IUserService>();
            var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            await service.CreateUser(command);

            var created = await repository.GetUserByLoginAsync(command.Login);
            var expected = UserFake.GetUserJames();

            created.Should().HaveEquivalentMembers(expected,
                o => o
                .Excluding(x => x.Id)
                .Excluding(x=> x.Password));

            BCryptNet.Verify(UserFake.PASSWORD_JAMES, created.Password).Should().BeTrue();
        }

        [Test]
        public async Task Should_Flag_User_As_Removed()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IUserService>();
            var repository = scope.ServiceProvider.GetRequiredService<IUserRemovedRepository>();

            var user = await repository.GetUserByLoginAsync(UserFake.LOGIN_ROBERT);

            user.Removed.Should().BeFalse();

            await service.DeleteUser(user.Id);

            var removedUser = await repository.GetUserByLoginAsync(UserFake.LOGIN_ROBERT);

            removedUser.Removed.Should().BeTrue();
        }

        [Test]
        public async Task Should_Get_All_Users()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IUserService>();
            var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var allFromRepository = await repository.GetAllAsync();

            var result = await service.GetAllUsers();

            var users = (List<UserResponse>)result.Value;
            users.Count().Should().Be(allFromRepository.Count());
        }

        [Test]
        public async Task Should_Validate_Password()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IUserService>();
            var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var command = UserFake.GetUserCommand();
            command.Password = "test";
            command.Login = "newLogin";

            var result = await service.CreateUser(command);

            result.Error.Should().BeTrue();
            (result.Value as List<string>).Single().Should().Be("Password must be bigger than 5 characters!");
        }

        [Test]
        public async Task Should_Validate_Login_Empty()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IUserService>();
            var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var command = UserFake.GetUserCommand();
            command.Login = string.Empty;

            var result = await service.CreateUser(command);

            result.Error.Should().BeTrue();
            (result.Value as List<string>).Single().Should().Be("Login can't be empty!");
        }

        [Test]
        public async Task Should_Validate_Login_Already_Exist()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IUserService>();
            var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var userInDb = (await repository.GetAllAsync()).First();
            var command = UserFake.GetUserCommand();
            command.Login = userInDb.Login;

            var result = await service.CreateUser(command);

            result.Error.Should().BeTrue();
            (result.Value as List<string>).Single().Should().Be("Login already exist!");
        }
    }
}
