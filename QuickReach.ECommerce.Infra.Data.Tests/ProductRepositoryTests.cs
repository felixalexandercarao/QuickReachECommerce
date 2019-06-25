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
namespace QuickReach.ECommerce.Infra.Data.Tests
{
    public class ProductRepositoryTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateDatabaseRecord()
        {
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                .UseInMemoryDatabase($"CategoryForTesting{Guid.NewGuid()}")
                .Options;
            var category = new Category
            {
                Name = "Electronics",
                Description = "Gadgets",
            };
            using (var context = new ECommerceDbContext(options))
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
            var product = new Product
            {
                Name="ZST Earphones",
                Description="Hybrid Earphones",
                IsActive=true,
                Price=500,
                ImageURL="picture.net",
                CategoryID=category.ID,          
            };
            using (var context = new ECommerceDbContext(options))
            {
                var sut = new ProductRepository(context);
                //Act
                sut.Create(product);
            }
            using (var context = new ECommerceDbContext(options))
            {
                //Assert
                var result = context.Products.Find(product.ID);
                Assert.NotNull(result);
                Assert.Equal(product.Name, result.Name);
                Assert.Equal(product.Description, result.Description);
                Assert.Equal(product.Price, result.Price);
                Assert.Equal(product.ImageURL, result.ImageURL);
            }
        }


        [Fact]
        public void Delete_WithValidData_ShouldWork()
        {
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                .UseInMemoryDatabase($"CategoryForTesting{Guid.NewGuid()}")
                .Options;
            var category = new Category
            {
                Name = "Electronics",
                Description = "Gadgets",
            };
            using (var context = new ECommerceDbContext(options))
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
            var product = new Product
            {
                Name = "ZST Earphones",
                Description = "Hybrid Earphones",
                IsActive = true,
                Price = 500,
                ImageURL = "picture.net",
                CategoryID = category.ID,
            };
            using (var context = new ECommerceDbContext(options))
            {
                //Arrange
                context.Products.Add(product);
                context.SaveChanges();
            }
            using (var context = new ECommerceDbContext(options))
            {
                var sut = new ProductRepository(context);
                //Act
                sut.Delete(product.ID);
            }
            using (var context = new ECommerceDbContext(options))
            {
                var result = context.Products.Find(product.ID);
                //Assert
                Assert.Null(result);
            }
        } 

        [Fact]
        public void Retrieve_WitValidInfo_ShouldWork()
        {
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                .UseInMemoryDatabase($"CategoryForTesting{Guid.NewGuid()}")
                .Options;
            var category = new Category
            {
                Name = "Electronics",
                Description = "Gadgets",
            };
            using (var context = new ECommerceDbContext(options))
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
            var expectedProduct = new Product
            {
                Name = "ZST Earphones",
                Description = "Hybrid Earphones",
                IsActive = true,
                Price = 500,
                ImageURL = "picture.net",
                CategoryID = category.ID,
            };
            using (var context = new ECommerceDbContext(options))
            {
                //Arrange
                context.Products.Add(expectedProduct);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new ProductRepository(context);
                //Act
                var result = sut.Retrieve(expectedProduct.ID);
                //Assert
                Assert.Equal(result.Name, expectedProduct.Name);
                Assert.Equal(result.Description, expectedProduct.Description);
                Assert.Equal(result.Price, expectedProduct.Price);
                Assert.Equal(result.ImageURL, expectedProduct.ImageURL);
                Assert.True(result.IsActive == expectedProduct.IsActive);
            }
        }

        [Theory]
        [InlineData(0,2,2)]
        [InlineData(4,3,3)]
        [InlineData(20,4,0)]
        [InlineData(15,6,5)]
        public void Retrieve_WithSkipAndCountShouldWork(int startCount,int perPageAmount,int expectedCount)
        {
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                .UseInMemoryDatabase($"CategoryForTesting{Guid.NewGuid()}")
                .Options;
            var category = new Category
            {
                Name = "Electronics",
                Description = "Gadgets",
            };
            using (var context = new ECommerceDbContext(options))
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
            //Arrange
            
            using (var context = new ECommerceDbContext(options))
            {
                //Arrange
                for (int i = 0; i < 20; i += 1)
                {
                    context.Products.Add(new Product
                    {
                        Name = string.Format("ZST Earphones ver {0}", i),
                        Description = string.Format("Hybrid ver {0}", i),
                        IsActive = true,
                        Price = 500 + (10 * i),
                        ImageURL = "picture.net",
                        CategoryID = category.ID,
                    });
                }
                context.SaveChanges();
            }
            using (var context = new ECommerceDbContext(options))
            {
                var sut = new ProductRepository(context);

                //Act
                var resultList = sut.Retrieve(startCount, perPageAmount);

                //Assert
                Assert.True(resultList.Count() == expectedCount);
            }
        }

        [Fact]
        public void Update_WithValidData_ShouldWork()
        {
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                 .UseInMemoryDatabase($"CategoryForTesting{Guid.NewGuid()}")
                 .Options;
            int expectedId = 0;
            var category = new Category
            {
                Name = "Electronics",
                Description = "Gadgets",
            };
            using (var context = new ECommerceDbContext(options))
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
            
            using (var context = new ECommerceDbContext(options))
            {

                //Arrange
                var product = new Product
                {
                    Name = "ZST Earphones",
                    Description = "Hybrid Earphones",
                    IsActive = true,
                    Price = 500,
                    ImageURL = "picture.net",
                    CategoryID = category.ID,
                };
                context.Products.Add(product);
                context.SaveChanges();
                expectedId = product.ID;
            }
            string expectedName = "ZSN Earphones";
            int expectedPrice = 750;
            string expectedImageUrl = "newpictre.jpg";
            using (var context = new ECommerceDbContext(options))
            {
                var result =context.Products.Find(expectedId);
                result.Name = expectedName;
                result.Price = expectedPrice;
                result.ImageURL = expectedImageUrl;

                var sut = new ProductRepository(context);
                //Act
                sut.Update(result.ID, result);
                //Assert
                var newResult = context.Products.Find(result.ID);
                Assert.Equal(newResult.Name, expectedName);
                Assert.Equal(newResult.ImageURL, expectedImageUrl);
                Assert.True(newResult.Price == expectedPrice);
            }
        } 

        [Fact]
        public void Retrieve_WithMissingData_ShouldNot_Work()
        {
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                .UseInMemoryDatabase($"CategoryForTesting{Guid.NewGuid()}")
                .Options;
            using (var context = new ECommerceDbContext(options))
            {
                var sut = new ProductRepository(context);
                //Act
                var result = sut.Retrieve(-1);

                //Assert
                Assert.Null(result);
            } 
        }
    }
}
