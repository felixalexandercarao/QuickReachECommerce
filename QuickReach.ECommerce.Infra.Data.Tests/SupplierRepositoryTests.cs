using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace QuickReach.ECommerce.Infra.Data.Tests
{
    public class SupplierRepositoryTests
    {
        [Fact]
        public void Create_WithValidInformation_Works()
        {
            //Arrage
            var context = new ECommerceDbContext();
            var sut = new SupplierRepository(context);
            Supplier supplier = new Supplier
            {
                Name="Yakult Inc.",
                Description="Lactobacillus Protectus",
                IsActive=false
            };
            //Act
            sut.Create(supplier);
            var result = sut.Retrieve(supplier.ID);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Name, supplier.Name);
            Assert.Equal(result.Description, supplier.Description);
            //Cleanup
            sut.Delete(result.ID);
        }

        [Fact]
        public void Delete_Works()
        {
            //Arrage
            var context = new ECommerceDbContext();
            var sut = new SupplierRepository(context);
            Supplier supplier = new Supplier
            {
                Name = "Yakult Inc.",
                Description = "Lactobacillus Protectus",
                IsActive = false
            };
            sut.Create(supplier);
            var result = sut.Retrieve(supplier.ID);
            Assert.NotNull(result);
            //Act
            sut.Delete(result.ID);
            //Assert
            var newResult = sut.Retrieve(result.ID);
            Assert.Null(newResult);
        }

        //[Fact]
        //public void Delete_NonExistent_()
        //{
        //    //Arrage
        //    var context = new ECommerceDbContext();
        //    var sut = new SupplierRepository(context);

        //    //Act
        //    sut.Delete(-1);
        //    //Assert
        //    Assert.
        //}

        [Fact]
        public void Retrieve_ValidData_ShouldWork()
        {
            var context = new ECommerceDbContext();
            var sut = new SupplierRepository(context);
            string expectedName = "Yakult Inc.";
            string expectedDescription = "Lactobacillus Protectus";
            bool expectedIsActive = false;
            Supplier supplier = new Supplier
            {
                Name = expectedName,
                Description = expectedDescription,
                IsActive = expectedIsActive
            };
            sut.Create(supplier);
            //Act
            var result = sut.Retrieve(supplier.ID);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Name, expectedName);
            Assert.Equal(result.Description, expectedDescription);
            Assert.Equal(result.IsActive, expectedIsActive);
            //Cleanup
            sut.Delete(result.ID);
        }

        [Fact]
        public void Retrieve_WithSkipAndCount_Work()
        {
            var context = new ECommerceDbContext();
            var sut = new SupplierRepository(context);
            for(int i = 0; i< 30; i += 1)
            {
                Supplier supplier = new Supplier
                {
                    Name = String.Format("Name {0}",i),
                    Description = String.Format("Description {0}", i),
                    IsActive = true
                };
                sut.Create(supplier);
            }
            var list = sut.Retrieve(3, 3);
            Assert.True(3== list.Count());//Pano machecheck kung na skip yung unang tatlo

            //Cleanup
            var listToBeDeleted = sut.Retrieve(0, Int32.MaxValue);
            foreach(Supplier supplier in listToBeDeleted)
            {
                sut.Delete(supplier.ID);
            }
        }

        [Fact]
        public void Retrieve_WithMissingData_ShouldReturnNull()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new SupplierRepository(context);
            //Act
            var result = sut.Retrieve(-1);
            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void Update_WithValidData_ShouldWork()
        {
            var context = new ECommerceDbContext();
            var sut = new SupplierRepository(context);
            string expectedName = "Yakult Inc.";
            string expectedDescription = "Lactobacillus Protectus";
            bool expectedIsActive = false;
            Supplier supplier = new Supplier
            {
                Name = "Jakult inc.",
                Description = "Lactobacillus Aysus",
                IsActive = true
            };
            sut.Create(supplier);
            var actual = sut.Retrieve(supplier.ID);
            //Assert
            actual.Name = expectedName;
            actual.Description = expectedDescription;
            actual.IsActive = expectedIsActive;
            sut.Update(actual.ID, actual);
            //Cleanup
            sut.Delete(actual.ID);
        }
    }
}
