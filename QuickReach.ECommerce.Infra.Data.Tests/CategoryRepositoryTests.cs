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
    public class CategoryRpositoryTest
    {
        [Fact]
        public void Create_WithValidEntity_ShouldCreateDatabaseRecord()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);


            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };

            //Act
            sut.Create(category);

            //Assert
            Assert.True(category.ID != 0);
            var entity = sut.Retrieve(category.ID);
            Assert.NotNull(entity);

            //Cleanup
            sut.Delete(category.ID);
        }
        [Fact]
        public void Retrieve_WithValidEntityID_ReturnAValidEntity()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);
            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };
            sut.Create(category);

            //Act
            var actual = sut.Retrieve(category.ID);

            //Assert
            Assert.NotNull(actual);

            //Cleanup
            sut.Delete(actual.ID);
        }

        [Fact]
        public void Retrieve_WithNonExistingEntityIDReturnsNull()
        {



            //Arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);

            //Act
            var actual = sut.Retrieve(-1);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public void Retrieve_WithSkipAndCount_ReturnsTheCorrectPage()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);
            for(var i = 1; i<= 20; i += 1)
            {
                sut.Create(new Category
                {
                    Name = string.Format("Category {0}", i),
                    Description = string.Format("Description {0}", i)
                });
            }

            //Act
            var list = sut.Retrieve(5, 5);

            //Assert
            Assert.True(list.Count() == 5);

            //Cleanup
            list = sut.Retrieve(0, Int32.MaxValue);
            foreach(Category category in list)
            {
                sut.Delete(category.ID);
            }
        }

        [Fact]
        public void Delete_WithValidEntityIDDeletes()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);
            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };
            sut.Create(category);

            //Act
            var actual = sut.Retrieve(category.ID);
            Assert.NotNull(actual);
            sut.Delete(actual.ID);

            //Assert
            Assert.Null(sut.Retrieve(actual.ID));
        }

        [Fact]
        public void Update_WorksWithValidData()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);
            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };
            sut.Create(category);
            var actual = sut.Retrieve(category.ID);
            var expectedName= "Sandals";
            var expectedDescription= "Sandals Depatment";

            actual.Name = expectedName;
            actual.Description = expectedDescription;            

            //Act

            sut.Update(actual.ID, actual);
            

            //Assert
            var result = sut.Retrieve(category.ID);
            Assert.Equal(result.Name,expectedName);
            Assert.Equal(result.Description, expectedDescription);

            //Cleanup
            var list = sut.Retrieve(0, Int32.MaxValue);
            foreach (Category caTegory in list)
            {
                sut.Delete(caTegory.ID);
            }
        }

    }
}
