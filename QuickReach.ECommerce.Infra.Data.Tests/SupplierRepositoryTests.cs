using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using QuickReach.ECommerce.Infra.Data.Tests.Utilities;

namespace QuickReach.ECommerce.Infra.Data.Tests
{
    public class SupplierRepositoryTests
    {
        [Fact]
        public void Create_WithValidInformation_Works()
        {
            var options = ConnectionOptionHelper.SqLite();
            //Arrange
            Supplier supplier = new Supplier
            {
                Name = "Yakult Inc.",
                Description = "Lactobacillus Protectus",
                IsActive = false
            };
            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                var sut = new SupplierRepository(context);
                //Act
                sut.Create(supplier);
            }
            using (var context = new ECommerceDbContext(options))
            {
                var result = context.Suppliers.Find(supplier.ID);
                //Assert
                Assert.NotNull(result);
                Assert.Equal(result.Name, supplier.Name);
                Assert.Equal(result.Description, supplier.Description);
            }
        }

        [Fact]
        public void Delete_RemovesValidDataFromDatabase()
        {
            var options = ConnectionOptionHelper.SqLite();
            //Arrange
            Supplier supplier = new Supplier
            {
                Name = "Yakult Inc.",
                Description = "Lactobacillus Protectus",
                IsActive = false
            };
            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Suppliers.Add(supplier);
                context.SaveChanges();
            }
            using (var context = new ECommerceDbContext(options))
            {
                var sut = new SupplierRepository(context);
                //Act
                sut.Delete(supplier.ID);
                //Assert
                var result = sut.Retrieve(supplier.ID);
                Assert.Null(result);
            }
              
        }

        [Fact]
        public void Retrieve_ValidData_ShouldWork()
        {
            var options = ConnectionOptionHelper.SqLite();
            //Arrange
            string expectedName = "Yakult Inc.";
            string expectedDescription = "Lactobacillus Protectus";
            bool expectedIsActive = false;
            Supplier supplier = new Supplier
            {
                Name = expectedName,
                Description = expectedDescription,
                IsActive = expectedIsActive
            };
            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Suppliers.Add(supplier);
                context.SaveChanges();
            }
            using (var context = new ECommerceDbContext(options))
            {
                var sut = new SupplierRepository(context);
                //Act
                var result = sut.Retrieve(supplier.ID);
                //Assert
                Assert.NotNull(result);
                Assert.Equal(result.Name, expectedName);
                Assert.Equal(result.Description, expectedDescription);
                Assert.Equal(result.IsActive, expectedIsActive);
            }
        }

        [Theory]
        [InlineData(0, 5, 5)]
        [InlineData(0, 20, 20)]
        [InlineData(17, 5, 3)]
        [InlineData(15, 5, 5)]
        [InlineData(20, 5, 0)]
        public void Retrieve_WithSkipAndCount_Work(int startNumber, int skipAmount, int expectedResult)
        {
            var options = ConnectionOptionHelper.SqLite();
            //Arrange
            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                for (int i = 0; i < 20; i += 1)
                {
                    context.Suppliers.Add(new Supplier
                    {
                        Name = String.Format("Name {0}", i),
                        Description = String.Format("Description {0}", i),
                        IsActive = true
                    });
                }
                context.SaveChanges();
            }
            using (var context = new ECommerceDbContext(options))
            {
                var sut = new SupplierRepository(context);
                //Act
                var list = sut.Retrieve(startNumber, skipAmount);

                //Assert
                Assert.True(expectedResult == list.Count());
            }         
        }

        [Fact]
        public void Retrieve_WithMissingData_ShouldReturnNull()
        {
            var options = ConnectionOptionHelper.SqLite();
            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                //Arrange
                var sut = new SupplierRepository(context);
                //Act
                var result = sut.Retrieve(-1);
                //Assert
                Assert.Null(result);
            }          
        }

        [Fact]
        public void Update_WithValidData_ShouldWork()
        {
            var options = ConnectionOptionHelper.SqLite();
            //Arrange
            string expectedName = "Yakult Inc.";
            string expectedDescription = "Lactobacillus Protectus";
            bool expectedIsActive = false;

            int supplierID = 0;
            
            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                Supplier supplier = new Supplier
                {
                    Name = "Jakult inc.",
                    Description = "Lactobacillus Aysus",
                    IsActive = true
                };
                context.Suppliers.Add(supplier);
                context.SaveChanges();
                supplierID = supplier.ID;
            }
            using (var context = new ECommerceDbContext(options))
            {
                var actual = context.Suppliers.Find(supplierID);

                var sut = new SupplierRepository(context);
                //Act
                actual.Name = expectedName;
                actual.Description = expectedDescription;
                actual.IsActive = expectedIsActive;
                sut.Update(actual.ID, actual);

                var result = sut.Retrieve(actual.ID);

                //Assert
                Assert.Equal(expectedName, result.Name);
                Assert.Equal(expectedDescription, result.Description);
                Assert.True(expectedIsActive== result.IsActive);
            }

        }
    }
}
