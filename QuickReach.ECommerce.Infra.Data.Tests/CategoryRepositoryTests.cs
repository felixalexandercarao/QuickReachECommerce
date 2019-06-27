using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using System;
using Xunit;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using QuickReach.ECommerce.Infra.Data.Repositories;
using QuickReach.ECommerce.Domain.Models;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Microsoft.Data.Sqlite;
using QuickReach.ECommerce.Infra.Data.Tests.Utilities;

namespace QuickReach.ECommerce.Infra.Data.Tests
{
    public class CategoryRpositoryTest
    {
        [Fact]
        public void Create_WithValidEntity_ShouldCreateDatabaseRecord()
        {
            //Arrange
            var options = ConnectionOptionHelper.SqLite();

            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var sut = new CategoryRepository(context);

                // Act
                sut.Create(category);
            }
            
            using(var context = new ECommerceDbContext(options))
            {
                var result = context.Categories.Find(category.ID);

                //Assert
                Assert.NotNull(result);
                Assert.Equal(category.Name, result.Name);
                Assert.Equal(category.Description, result.Description);
            }
        }
        [Fact]
        public void Retrieve_WithValidEntityID_ReturnAValidEntity()
        {
            var options = ConnectionOptionHelper.SqLite();
            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };
            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Categories.Add(category);
                context.SaveChanges();
            }
            using (var context = new ECommerceDbContext(options))
            {
                var sut = new CategoryRepository(context);
                //Act
                var result = sut.Retrieve(category.ID);
                //Assert
                Assert.NotNull(result);
                Assert.Equal(category.Name, result.Name);
                Assert.Equal(category.Description, result.Description);
            }
            
        }

        [Fact]
        public void Retrieve_WithNonExistingEntityIDReturnsNull()
        {
            var options = ConnectionOptionHelper.SqLite();
            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                // Arrange
                var sut = new CategoryRepository(context);

                // Act
                var actual = sut.Retrieve(-1);

                // Assert
                Assert.Null(actual);
            }
        }

        [Theory]
        [InlineData(0,5,5)]
        [InlineData(0, 20, 20)]
        [InlineData(17, 5, 3)]
        [InlineData(15, 5, 5)]
        [InlineData(20, 5, 0)]
        public void Retrieve_WithSkipAndCount_ReturnsTheCorrectPage(int startNumber, int skipAmount,int expectedResult)
        {
            //Arrange
            var options = ConnectionOptionHelper.SqLite();
            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                for (var i = 1; i <= 20; i += 1)
                {
                    context.Categories.Add(new Category
                    {
                        Name = string.Format("Category {0}", i),
                        Description = string.Format("Description {0}", i)
                    });
                }
                context.SaveChanges();
            }
            using (var context = new ECommerceDbContext(options))
            {
                var sut = new CategoryRepository(context);
                var list = sut.Retrieve(startNumber, skipAmount);
                Assert.True(list.Count() == expectedResult);
            }
        }

        [Fact]
        public void Delete_WithValidEntityIDDeletes()
        {
            // Arrange
            var options = ConnectionOptionHelper.SqLite();
            
            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };
            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Categories.Add(category);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new CategoryRepository(context);

                // Act
                sut.Delete(category.ID);

                // Assert
                category = context.Categories.Find(category.ID);
                Assert.Null(category);
            }
        }

        [Fact]
        public void Update_WorksWithValidData()
        {
            var options = ConnectionOptionHelper.SqLite();
            var expectedName = "Sandals";
            var expectedDescription = "Sandals Depatment";
            int expectedId = 0;
            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var category = new Category
                {
                    Name = "Shoes",
                    Description = "Shoes Department"
                };
                context.Categories.Add(category);
                context.SaveChanges();

                expectedId = category.ID;
            }
            using (var context = new ECommerceDbContext(options))
            {
                // Arrange
                var category = context.Categories.Find(expectedId);

                category.Name = expectedName;
                category.Description = expectedDescription;

                var sut = new CategoryRepository(context);

                // Act
                sut.Update(category.ID, category);

                // Assert
                var actual = context.Categories.Find(category.ID);

                Assert.Equal(expectedName, actual.Name);
                Assert.Equal(expectedDescription, actual.Description);
            }
        }

        //[Fact]
        //public void Delete_CategoryWithExistingProducts_ShouldThrowException()
        //{
        //    //Arrange
        //    var options = ConnectionOptionHelper.SqLite();
        //    var category = new Category
        //    {
        //        Name = "Electronics",
        //        Description = "Gadgets",
        //    };
        //    using (var context = new ECommerceDbContext(options))
        //    {
        //        context.Database.OpenConnection();
        //        context.Database.EnsureCreated();
        //        context.Categories.Add(category);
        //        context.SaveChanges();
        //    }
        //    var product = new Product
        //    {
        //        Name = "ZST Earphones",
        //        Description = "Hybrid Earphones",
        //        IsActive = true,
        //        Price = 500,
        //        ImageURL = "https://images-na.ssl-images-amazon.com/images/I/61aQ5xUpsdL._SX679_.jpg",
        //    };
        //    using (var context = new ECommerceDbContext(options))
        //    {
                
        //        context.Products.Add(product);
        //        context.SaveChanges();
        //    }

        //    //Act//Assert
        //    using (var context = new ECommerceDbContext(options))
        //    {
        //        var sut = new CategoryRepository(context);
        //        Assert.Throws<DbUpdateException>(()=>sut.Delete(category.ID));
        //    }
            
        //}

    }
}
