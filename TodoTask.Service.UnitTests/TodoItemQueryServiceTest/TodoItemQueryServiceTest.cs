using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection.Metadata;
using TodoTask.DataAccess.EF.Context;
using TodoTask.DataAccess.Entities;
using TodoTask.Model.Dtos;
using TodoTask.Service.UnitTests.EFTestDbAsync;
using static System.Reflection.Metadata.BlobBuilder;

namespace TodoTask.Service.UnitTests.TodoItemQueryServiceTest
{
    public class TodoItemQueryServiceTest
    {
        [Fact]
        public async void GetPendingTodoItems_WhenTodoItemsHasEmptyDueDate_ShouldBeInResultList()
        {
            //Arrange
            var emptyDueDateItem1 = new TodoItem { Id = 0, Title = "todo1", IsComplated = false };
            var emptyDueDateItem2 = new TodoItem { Id = 1, Title = "todo2", IsComplated = false };
            var data = new List<TodoItem>
            {
                emptyDueDateItem1,
                emptyDueDateItem2,
            };
            var mockContext = new Mock<TodoTaskDbContext>();
            mockContext.Setup(x => x.TodoItems).ReturnsDbSet(data);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var queryService = new TodoItemQueryService(mockContext.Object, dateTimeProvider.Object);

            //Act
            var result = await queryService.GetPendingTodoItems();

            //Assert
            Assert.Equal(2, result.Count);
            Assert.True(result.Exists(x => x.Id == emptyDueDateItem1.Id));
            Assert.True(result.Exists(x => x.Id == emptyDueDateItem2.Id));
        }

        [Fact]
        public async void GetPendingTodoItems_WhenTodoItemsHasPassedDueDate_ShouldNotBeInResultList()
        {
            //Arrange
            var dueItem1 = new TodoItem { Id = 0, Title = "todo1", DueDate = new DateTime(2022, 11, 3), IsComplated = false, };
            var dueItem2 = new TodoItem { Id = 1, Title = "todo2", DueDate = new DateTime(2022, 11, 4), IsComplated = false, };
            var pendingItem1 = new TodoItem { Id = 2, Title = "todo3", DueDate = new DateTime(2022, 11, 5), IsComplated = false, };
            var data = new List<TodoItem>
            {
                dueItem1, dueItem2, pendingItem1
            };
            var mockContext = new Mock<TodoTaskDbContext>();
            mockContext.Setup(x => x.TodoItems).ReturnsDbSet(data);
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.Setup(x => x.Now).Returns(new DateTime(2022, 11, 5));
            var queryService = new TodoItemQueryService(mockContext.Object, mockDateTimeProvider.Object);

            //Act
            var result = await queryService.GetPendingTodoItems();

            //Assert
            Assert.Equal(1, result.Count);
            Assert.True(result.Exists(x => x.Id == pendingItem1.Id));
        }

        public async void GetPendingTodoItems_WhenTodoItemsHasFutureDueDate_ShouldBeInResultList()
        {
            //Arrange
            var pendingItem1 = new TodoItem { Id = 1, Title = "todo1", DueDate = new DateTime(2022, 11, 6), IsComplated = false, };
            var pendingItem2 = new TodoItem { Id = 2, Title = "todo2", DueDate = new DateTime(2022, 11, 7), IsComplated = false, };
            
            var data = new List<TodoItem>
            {
                pendingItem1, pendingItem2
            };
            var mockContext = new Mock<TodoTaskDbContext>();
            mockContext.Setup(x => x.TodoItems).ReturnsDbSet(data);
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.Setup(x => x.Now).Returns(new DateTime(2022, 11, 5));
            var queryService = new TodoItemQueryService(mockContext.Object, mockDateTimeProvider.Object);

            //Act
            var result = await queryService.GetPendingTodoItems();

            //Assert
            Assert.Equal(2, result.Count);
            Assert.True(result.Exists(x => x.Id == pendingItem1.Id));
            Assert.True(result.Exists(x => x.Id == pendingItem2.Id));
        }

        [Fact]
        public async void GetPendingTodoItems_WhenTodoItemsIsComplated_ShouldNotBeInResultList()
        {
            //Arrange
            var isCompatedItem = new TodoItem { Id = 0, Title = "todo1", DueDate = null, IsComplated = true, };
            var data = new List<TodoItem>()
            {
                isCompatedItem
            };
            var mockContext = new Mock<TodoTaskDbContext>();
            mockContext.Setup(x => x.TodoItems).ReturnsDbSet(data);
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.Setup(x => x.Now).Returns(new DateTime(2022, 11, 5));
            var queryService = new TodoItemQueryService(mockContext.Object, mockDateTimeProvider.Object);

            //Act
            var result = await queryService.GetPendingTodoItems();

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async void GetOverdueTodoItems_WhenTodoItemsIsComplated_ShouldNotBeInResultList()
        {
            //Arrange
            var isCompatedItem = new TodoItem { Id = 0, Title = "todo1", DueDate = null, IsComplated = true, };

            var data = new List<TodoItem>()
            {
                isCompatedItem
            };
            var mockContext = new Mock<TodoTaskDbContext>();
            mockContext.Setup(x => x.TodoItems).ReturnsDbSet(data);
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.Setup(x => x.Now).Returns(new DateTime(2022, 11, 5));
            var queryService = new TodoItemQueryService(mockContext.Object, mockDateTimeProvider.Object);

            //Act
            var result = await queryService.GetOverdueTodoItems();

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async void GetOverdueTodoItems_WhenTodoItemsHasEmptyDueDate_ShouldNotBeInResultList()
        {
            //Arrange
            var emptyDueDateItem = new TodoItem { Id = 0, Title = "todo1", DueDate = null, IsComplated = true, };
            var data = new List<TodoItem>
            {
                emptyDueDateItem
            };
            var mockContext = new Mock<TodoTaskDbContext>();
            mockContext.Setup(x => x.TodoItems).ReturnsDbSet(data);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var queryService = new TodoItemQueryService(mockContext.Object, dateTimeProvider.Object);

            //Act
            var result = await queryService.GetOverdueTodoItems();

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async void GetOverdueTodoItems_WhenTodoItemsHasPassedDueDate_ShouldBeInResultList()
        {
            //Arrange
            var dueItem1 = new TodoItem { Id = 0, Title = "todo1", DueDate = new DateTime(2022, 11, 3), IsComplated = false, };
            var dueItem2 = new TodoItem { Id = 1, Title = "todo2", DueDate = new DateTime(2022, 11, 4), IsComplated = false, };
            var pendingItem1 = new TodoItem { Id = 2, Title = "todo3", DueDate = new DateTime(2022, 11, 5), IsComplated = false, };
            var data = new List<TodoItem>
            {
                dueItem1,
                dueItem2,
                pendingItem1
            };
            var mockContext = new Mock<TodoTaskDbContext>();
            mockContext.Setup(x => x.TodoItems).ReturnsDbSet(data);
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.Setup(x => x.Now).Returns(new DateTime(2022, 11, 5));
            var queryService = new TodoItemQueryService(mockContext.Object, mockDateTimeProvider.Object);

            //Act
            var result = await queryService.GetOverdueTodoItems();

            //Assert
            Assert.Equal(2, result.Count);
            Assert.True(result.Exists(x => x.Id == dueItem1.Id));
            Assert.True(result.Exists(x => x.Id == dueItem2.Id));
            Assert.False(result.Exists(x => x.Id == pendingItem1.Id));
        }

        public async void GetOverdueTodoItems_WhenTodoItemsHasFutureDueDate_ShouldNotBeInResultList()
        {
            //Arrange
            var pendingItem1 = new TodoItem { Id = 1, Title = "todo1", DueDate = new DateTime(2022, 11, 6), IsComplated = false, };
            var pendingItem2 = new TodoItem { Id = 2, Title = "todo2", DueDate = new DateTime(2022, 11, 7), IsComplated = false, };

            var data = new List<TodoItem>
            {
                pendingItem1, pendingItem2
            };
            var mockContext = new Mock<TodoTaskDbContext>();
            mockContext.Setup(x => x.TodoItems).ReturnsDbSet(data);
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.Setup(x => x.Now).Returns(new DateTime(2022, 11, 5));
            var queryService = new TodoItemQueryService(mockContext.Object, mockDateTimeProvider.Object);

            //Act
            var result = await queryService.GetOverdueTodoItems();

            //Assert
            Assert.Empty(result);
        }
    }
}