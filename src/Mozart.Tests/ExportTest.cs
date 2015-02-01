using System;
using System.Linq;
using Mozart.Helpers;
using Mozart.Model;
using Mozart.Tests.Model;
using Xunit;

namespace Mozart.Tests
{    
    public class ExportTest
    {
        [Fact]
        public void CanGetPropertySingleton()
        {
            Compose<IZoo>.Exports.Clear();

            //Register/Act
            Compose<IZoo>.Add(typeof(BostonZoo));

            // Act
            var zoo = Compose<IZoo>.Singleton;

            //Assert
            Assert.True(zoo != null && zoo.Name == "Babaam");
        }

        [Fact]
        public void SingletonIsSameInstance()
        {
            Compose<IZoo>.Exports.Clear();

            //Register/Act            
            Compose<IZoo>.Add(typeof(BostonZoo));
            Compose<IZoo>.Add(typeof(DallasZoo));

            // Act
            var zoo1 = Compose<IZoo>.Get();
            var zoo2 = Compose<IZoo>.Get();
            //Assert
            Assert.True(zoo1.Equals(zoo2));
        }

        [Fact]
        public void CanGetRewrittenPropertySingleton()
        {
            Compose<IZoo>.Exports.Clear();

            //Register/Act
            Compose<IZoo>.Add(typeof(BostonZoo));
            Compose<IZoo>.Add(typeof(DallasZoo));

            // Act
            var zoo = Compose<IZoo>.Singleton;

            //Assert
            Assert.True(zoo != null && zoo.GetType() == typeof(DallasZoo));
        }

        [Fact]
        public void CanGetRewrittenSingleton()
        {
            Compose<IZoo>.Exports.Clear();

            //Register/Act
            Compose<IZoo>.Add(typeof(BostonZoo));
            Compose<IZoo>.Add(typeof(DallasZoo));

            // Act
            var zoo = Compose<IZoo>.Get();

            //Assert
            Assert.True(zoo != null && zoo.GetType() == typeof(DallasZoo));
        }

        [Fact]
        public void WillFailIfTypeIsMissing()
        {
            Compose<IAnimal>.Exports.Clear();
            //Register/Act
            Compose<IAnimal>.Add(typeof(Lion));
            Compose<IAnimal>.Add(typeof(Dog));            
            
            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => { Compose<IAnimal>.Get<Cat>(); });

            //Assert
            Assert.True(ex.Message == "Sequence contains no matching element");
        }

        [Fact]
        public void CanGetAllExports()
        {
            Compose<IAnimal>.Exports.Clear();
            //Register
            Compose<IAnimal>.Add(typeof(Lion));
            Compose<IAnimal>.Add(typeof(Dog));
            Compose<IAnimal>.Add(typeof(Cat));
            
            //Act
            var animals = Compose<IAnimal>.GetAll();

            // Assert
            Assert.True(animals.Count == 3);
        }

        [Fact]
        public void WillNotAddMultipleTypes()
        {
            Compose<IAnimal>.Exports.Clear();
            //Register
            Compose<IAnimal>.Add(typeof(Lion));
            Compose<IAnimal>.Add(typeof(Lion));

            //Act
            var animals = Compose<IAnimal>.GetAll();

            // Assert
            Assert.True(animals.Count == 1 && animals.First().GetType() == typeof(Lion));
        }

        [Fact]
        public void CanGetSpecificType()
        {
            Compose<IAnimal>.Exports.Clear();
            //Register
            Compose<IAnimal>.Add(typeof(Lion));
            Compose<IAnimal>.Add(typeof(Dog));
            Compose<IAnimal>.Add(typeof(Cat));

            //Act
            var animal = Compose<IAnimal>.Get<Lion>();

            // Assert
            Assert.True(animal.Says() == "Grrr");
        }

        [Fact]
        public void CanGetExportAttribute()
        {
            //Arrange
            var type = typeof (IAnimal);

            //Act
            var attribute = type.ExportAttribute();

            // Assert
            Assert.True(attribute.InstanceRule == InstanceRule.Multiple);
        }

    }
}
