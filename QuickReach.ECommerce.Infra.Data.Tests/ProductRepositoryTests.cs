using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repositories;
using System;
using System.Linq;
using Xunit;

namespace QuickReach.ECommerce.Infra.Data.Tests
{
    public class ProductRepositoryTests
    {
        [Fact]
        public void Create_WithValidData_ShouldWork()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);
            var categoryRepo = new CategoryRepository(context);
            var category = new Category
            {
                Name = "Electronics",
                Description = "Gadgets",
            };
            categoryRepo.Create(category);
            var product = new Product
            {
                Name="ZST Earphones",
                Description="Hybrid Earphones",
                IsActive=true,
                Price=500,
                ImageURL="picture.net",
                CategoryID=category.ID,
                Category=category             
            };
            
            
            //Act
            sut.Create(product);

            //Assert
            Assert.True(product.ID != 0);
            var result = sut.Retrieve(product.ID);
            Assert.NotNull(result);

            //Cleanup
            sut.Delete(product.ID);
            categoryRepo.Delete(category.ID);
        }

        #region Delete_WithValidData_ShouldWork
        [Fact]
        public void Delete_WithValidData_ShouldWork()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);
            var categoryRepo = new CategoryRepository(context);
            var category = new Category
            {
                Name = "Electronics",
                Description = "Gadgets",
            };
            categoryRepo.Create(category);
            var product = new Product
            {
                Name = "ZST Earphones",
                Description = "Hybrid Earphones",
                IsActive = true,
                Price = 500,
                ImageURL = "picture.net",
                CategoryID = category.ID,
                Category = category
            };

            sut.Create(product);
            var result = sut.Retrieve(product.ID);
            //Act
            Assert.NotNull(result);
            sut.Delete(result.ID);

            //Assert
            Assert.Null(sut.Retrieve(result.ID));

            //Cleanup
            categoryRepo.Delete(category.ID);
        } 
        #endregion

        #region Retrieve_WitValidInfo_ShouldWork
        [Fact]
        public void Retrieve_WitValidInfo_ShouldWork()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);
            var categoryRepo = new CategoryRepository(context);
            var category = new Category
            {
                Name = "Electronics",
                Description = "Gadgets",
            };
            categoryRepo.Create(category);
            var expectedProduct = new Product
            {
                Name = "ZST Earphones",
                Description = "Hybrid Earphones",
                IsActive = true,
                Price = 500,
                ImageURL = "picture.net",
                CategoryID = category.ID,
                Category = category
            };
            sut.Create(expectedProduct);
            //Act
            var result = sut.Retrieve(expectedProduct.ID);


            //Assert
            Assert.Equal(result.Name, expectedProduct.Name);
            Assert.Equal(result.Description, expectedProduct.Description);
            Assert.Equal(result.Price, expectedProduct.Price);
            Assert.Equal(result.ImageURL, expectedProduct.ImageURL);
            Assert.True(result.IsActive == expectedProduct.IsActive);

            //Cleanup
            sut.Delete(result.ID); //Pagdinelete category, deleted lahat ng roducts nya
            categoryRepo.Delete(category.ID);
        }
        #endregion

        #region Retrieve_WithSkipAndCount
        [Theory]
        [InlineData(4,2,2)]
        [InlineData(4,3,3)]
        [InlineData(4,4,4)]
        [InlineData(15,6,5)]
        public void Retrieve_WithSkipAndCountShouldWork(int startCount,int perPageAmount,int expectedCount)
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);
            var categoryRepo = new CategoryRepository(context);
            var category = new Category
            {
                Name = "Electronics",
                Description = "Gadgets",
            };
            categoryRepo.Create(category);
            for (int i = 0; i < 20; i += 1)
            {
                var product = new Product
                {
                    Name = string.Format("ZST Earphones ver {0}", i),
                    Description = string.Format("Hybrid ver {0}", i),
                    IsActive = true,
                    Price = 500 + (10 * i),
                    ImageURL = "picture.net",
                    CategoryID = category.ID,
                    Category = category
                };
                sut.Create(product);
            }

            //Act
            var resultList = sut.Retrieve(startCount, perPageAmount);

            //Assert
            Assert.True(resultList.Count() == expectedCount);

            //Cleanup
            var listToBeDeleted = sut.Retrieve(0, Int32.MaxValue);
            foreach(var product in listToBeDeleted)
            {
                sut.Delete(product.ID);
            }
            categoryRepo.Delete(category.ID);
        }
        #endregion

        #region UpdateWithValidDataWorks
        [Fact]
        public void Update_WithValidData_ShouldWork()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);
            var categoryRepo = new CategoryRepository(context);
            var category = new Category
            {
                Name = "Electronics",
                Description = "Gadgets",
            };
            categoryRepo.Create(category);
            var product = new Product
            {
                Name = "ZST Earphones",
                Description = "Hybrid Earphones",
                IsActive = true,
                Price = 500,
                ImageURL = "picture.net",
                CategoryID = category.ID,
                Category = category
            };
            sut.Create(product);
            var result = sut.Retrieve(product.ID);
            string expectedName = "ZSN Earphones";
            int expectedPrice = 750;
            string expectedImageUrl = "newpictre.jpg";
            result.Name = expectedName;
            result.Price = expectedPrice;
            result.ImageURL = expectedImageUrl;


            //Act
            sut.Update(result.ID, result);

            //Assert
            var newResult = sut.Retrieve(result.ID);
            Assert.Equal(newResult.Name,expectedName);
            Assert.Equal(newResult.ImageURL, expectedImageUrl);
            Assert.True(newResult.Price==expectedPrice);

            //Cleanup
            categoryRepo.Delete(category.ID);
        } 
        #endregion

        [Fact]
        public void Retrieve_WithMissingData_ShouldNot_Work()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);
            //Act
            var result = sut.Retrieve(-1);

            //Assert
            Assert.Null(result);
        }
    }
}
