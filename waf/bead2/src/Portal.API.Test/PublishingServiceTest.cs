using System;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Identity;
using Portal.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Portal.API.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using Portal.Persistence.DTO;

namespace Portal.API.Test
{
    public class PublishingServiceTest : IDisposable
    {
        private readonly Mock<UserManager<DbUser>> mockUserManager;
        private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
        private readonly PortalContext portalContext;

        private readonly PublishingService service;

        public PublishingServiceTest()
        {
            var userStoreMock = new Mock<IUserStore<DbUser>>();
            mockUserManager =
                new Mock<UserManager<DbUser>>(
                    userStoreMock.Object,
                    null, null, null, null, null, null, null, null
                );

            mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContextAccessor.Setup(obj => obj.HttpContext).Returns(mockHttpContext.Object);

            var options = new DbContextOptionsBuilder<PortalContext>()
                .UseInMemoryDatabase("PortalTest")
                .Options;

            portalContext = new PortalContext(options);
            portalContext.Database.EnsureCreated();

            DbInitializer.ApplyToContext(portalContext);

            mockUserManager
                .Setup(obj => obj.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(DbInitializer.Publishers.Instrument));

            service = new PublishingService(portalContext, mockHttpContextAccessor.Object, mockUserManager.Object);
        }

        public void Dispose()
        {
            portalContext.Database.EnsureDeleted();
            portalContext.Dispose();
        }

        [Fact]
        public async void GetItemsTest()
        {
            var items = await service.GetPreviews();
            Assert.Contains(items, item => item.Id == DbInitializer.Items.LP100.Id);
            Assert.Contains(items, item => item.Id == DbInitializer.Items.Amp.Id);
            Assert.DoesNotContain(items, item => item.Id == DbInitializer.Items.Desk.Id);
            Assert.DoesNotContain(items, item => item.Id == DbInitializer.Items.Farewell.Id);
        }

        [Fact]
        public async void PublishItemTest()
        {
            var item = new ItemDataDTO()
            {
                Category = "Other",
                Name = "",
                Description = "nonempty",
                InitLicit = 10,
                Expiration = DateTime.Now.AddDays(1),
                Image = null,
            };

            int countBefore = await portalContext.Items.CountAsync();

            var result = await service.InsertItem(item);

            int countAfter = await portalContext.Items.CountAsync();

            Assert.Null(result.Id);
            Assert.Equal(countBefore, countAfter);

            item.Name = "nonempty";

            result = await service.InsertItem(item);
            countAfter = await portalContext.Items.CountAsync();

            Assert.NotNull(result.Id);
            Assert.Equal(countBefore + 1, countAfter);
        }

        [Fact]
        public async void CloseItemTest()
        {
            bool result = await service.CloseItem(DbInitializer.Items.LP100.Id);
            Assert.True(result);
            result = await service.CloseItem(DbInitializer.Items.Amp.Id);
            Assert.False(result);
            result = await service.CloseItem(DbInitializer.Items.Farewell.Id);
            Assert.False(result);
        }

        [Fact]
        public async void GetItemTest()
        {
            var dto = await service.GetItem(DbInitializer.Items.Desk.Id);
            Assert.Null(dto);
            var item = DbInitializer.Items.LP100;
            dto = await service.GetItem(item.Id);
            Assert.NotNull(dto);
            Assert.Equal(dto.Id, item.Id);
            Assert.Equal(dto.Image, item.Image);
            Assert.Equal(dto.InitLicit, item.InitLicit);
            Assert.Equal(dto.Name, item.Name);
            Assert.Equal(dto.Bids.Length, item.Bids.Count);
            Assert.Equal(dto.Category, item.Category.Name);
            Assert.Equal(dto.Description, item.Description);
            Assert.Equal(dto.Expiration, item.Expiration);
        }
    }
}
