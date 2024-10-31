using Microsoft.AspNetCore.Mvc;
using Lab1_1.Controllers;
using Lab1_1.Data.Model;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Lab1_1.Contracts;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Lab1_1.Test;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Lab1_1.Share.DTOs;
using System.Security.Cryptography.X509Certificates;

namespace Lab1_1.Test.Tests
{
    public class ControllerTests
    {
        public Mock<IRepository<N018Dictionary>> _mock = MockRepo.SetupMock();
        [Fact]
        public void GetAll_ReturnsListOfDictionaries()
        {
            // Arrange
            _mock.Setup(repo => repo.GetAll()).Returns(GetTestDicts());
            var controller = new DictionaryController(logger: null!, _mock.Object);

            // Act
            var result = controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<IQueryable<N018Dictionary>>(okResult.Value);

            // Assert that resulting value is expected dictionary
            value.Should().NotBeEmpty();
            value.ElementAt(0).Name.Should().Be("Test");
            value.ElementAt(0).BeginDate.Should().Be(DateTime.MinValue);
        }
        private IQueryable<N018Dictionary> GetTestDicts()
        {
            var dicts = new List<N018Dictionary>
            {
                new N018Dictionary {Id = 1, 
                BeginDate = DateTime.MinValue,
                EndDate = DateTime.MaxValue,
                Name = "Test",
                Comments = "Test2"}
            };
            return dicts.AsQueryable<N018Dictionary>();
        }

        [Fact]
        public void Get_ImpossibleId_NotFound()
        {
            // Setup
            int TestId = 7;
            _mock.Setup(repo => repo.GetByKey(TestId)).Returns(null as N018Dictionary);
            var controller = new DictionaryController(logger: null!, _mock.Object);

            // Action
            var result = controller.Get(TestId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Get_RealId_OkWithValue()
        {
            // Setup
            _mock.Setup(repo => repo.GetByKey(1)).Returns(GetTestDicts().First());
            var controller = new DictionaryController(logger: null!, _mock.Object);

            // Action
            var result = controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<N018Dictionary>(okResult.Value);

            value.Id.Should().Be(1);
        }

        [Fact]
        public void Post_GoodDictDTO_Ok()
        {
            // Setup
            _mock.Setup(repo => repo.Add(It.IsAny<N018Dictionary>()));
            var controller = new DictionaryController(logger: null!, _mock.Object);

            DictionaryPostDTO postData = new DictionaryPostDTO() { Code = 1, Name = "AAAA"};

            // Action
            var result = controller.Post(postData);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Post_BadDictPostDTO_Ok()
        {
            // Setup
            _mock.Setup(repo => repo.Add(It.IsAny<N018Dictionary>())).Throws(new Exception());
            var controller = new DictionaryController(logger: null!, _mock.Object);
            var postData = new DictionaryPostDTO();

            // Action
            var result = controller.Post(postData);

            // Assert
            var okResult = Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Put_GoodDictDTO_Ok()
        {
            // Setup
            _mock.Setup(repo => repo.Edit(It.IsAny<N018Dictionary>()));
            _mock.Setup(repo => repo.GetByKey(3)).Returns(new N018Dictionary {Id = 3});
            var controller = new DictionaryController(logger: null!, _mock.Object);
            var putData = new DictionaryDTO() { Id = 3};

            // Action
            var result = controller.Put(putData);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<int>(okResult.Value);
            value.Should().Be(3);
        }

        [Fact]
        public void Put_MissingId_NotFound()
        {
            // Setup
            _mock.Setup(repo => repo.GetByKey(7)).Returns<N018Dictionary>(null!);
            var controller = new DictionaryController(logger: null!, _mock.Object);
            var putData = new DictionaryDTO() { Id = 7 };

            // Action
            var result = controller.Put(putData);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Delete_ImpossibleId_NotFound()
        {
            // Setup
            var controller = new DictionaryController(logger: null!, _mock.Object);

            // Action
            var result = controller.Delete(7);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Delete_GoodId_Ok()
        {
            // Setup
            _mock.Setup(repo => repo.GetByKey(1)).Returns(GetTestDicts().First());
            var controller = new DictionaryController(logger: null!, _mock.Object);

            // Action
            var result = controller.Delete(1);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void PostFromFile_NoFile_BadRequest()
        {
            // Setup
            var controller = new DictionaryController(logger: null!, _mock.Object);

            // Action
            var result = controller.PostFromFile(null!);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}