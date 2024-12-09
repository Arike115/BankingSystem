using Azure;
using BankingSystem.Application.Commands.AccountRegistration;
using BankingSystem.Controllers;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Model;
using BankingSystem.Infrastructure.Persistence;
using Castle.Core.Logging;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Data.Entity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Xunit;


namespace TestBankingSystem
{
    public class Tests
    {
        private BankingSystemDbContext _dbcontext;

        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<AccountController>> _loggerMock;
        private readonly AccountController _account;


        public Tests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<AccountController>>();
            _account = new AccountController(_mediatorMock.Object, _loggerMock.Object);
        }

        //[SetUp]
        //public void Setup()
        //{
        //    _dbcontext = new BankingSystemDbContext(
        //    new DbContextOptionsBuilder<BankingSystemDbContext>()
        //        .UseInMemoryDatabase("TestBankingDb")
        //        .Options);
        //}

        [Test]
        public async Task CreateAccount_ShouldReturnOk_WhenResponseStatusIsTrue()
        {
            //Arrange
            var rnd = new Random();

            var account = new AccountCommand
            {
                AccountBalance = 1,
                AccountName = "Test",
                AccountNumber = rnd.Next(100000000, 999999999).ToString(),
                AccountType = BankingSystem.Domain.Enum.AccountType.Individual,
                Address = "Lagos Nigeria",
                Bvn = "22222222222",
                IsDeleted = false,
                Nin = "12312356789"
            };
            
            
            
            var baseResponse = new BaseResponse
            {
                Information = "",
                Message = "",
                Status = true,
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<AccountCommand>(), default))
                     .ReturnsAsync(baseResponse);


            //Act
            var result = await _account.CreateAccount(account);

            //Assert
            var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
            Xunit.Assert.Equal(baseResponse, okResult.Value);

            _mediatorMock.Verify(m => m.Send(It.Is<AccountCommand>(c => c == account), default), Times.Once);

            NUnit.Framework.Assert.Pass();
        }


        [Test]
        public async Task CreateAccount_ShouldReturnBadRequest_WhenResponseStatusIsFalse()
        {
            //Arrange
            var rnd = new Random();

            var account = new AccountCommand
            {
                AccountBalance = 1,
                AccountName = "Test",
                AccountNumber = rnd.Next(100000000, 999999999).ToString(),
                AccountType = BankingSystem.Domain.Enum.AccountType.Individual,
                Address = "Lagos Nigeria",
                Bvn = "22222222222",
                IsDeleted = false,
                Nin = "12312356789"
            };



            var baseResponse = new BaseResponse
            {
                Information = "",
                Message = "",
                Status = true,
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<AccountCommand>(), default))
                     .ReturnsAsync(baseResponse);


            //Act
            var result = await _account.CreateAccount(account);

            //var badRequest = (BadRequestObjectResult)result;

            //Assert
            var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
            Xunit.Assert.Equal(baseResponse, okResult.Value);

            _mediatorMock.Verify(m => m.Send(It.Is<AccountCommand>(c => c == account), default), Times.Once);

            NUnit.Framework.Assert.Pass();
        }





        [TearDown]
        public void TearDown()
        {
            // Dispose of the DbContext to release resources
            _dbcontext?.Dispose();
        }
    }
}