using System;
using System.Reflection;
using Mozart.Helpers;
using Mozart.Tests.Model;
using Xunit;

namespace Mozart.Tests
{
    public class ReflectionTest
    {
        [Fact]
        public void CanMakeGenericExport()
        {
            Compose<IAnimal>.Exports.Clear();
            //Register            
            Compose<IAnimal>.Add(typeof(Lion));
            Compose<IAnimal>.Add(typeof(Dog));
            Compose<IAnimal>.Add(typeof(Cat));

            //Act
            var genericExport = typeof (IAnimal).MakeGenericExport();

            Assert.True(genericExport != null);

            var getMethod = genericExport.GetMethod("Get", new Type[] { typeof(Type) });

            // Assert
            Assert.True(getMethod.Invoke(genericExport,new object[]{typeof (Dog)}) != null);
        }

        [Fact]
        public void CanFindExportableCtor()
        {
            //Arrange
            Compose<IAnimal>.Exports.Clear();
            Compose<IManager>.Exports.Clear();
            Compose<IZoo>.Exports.Clear();

            Compose<IAnimal>.Add(typeof(Lion));
            Compose<IAnimal>.Add(typeof(Dog));
            Compose<IAnimal>.Add(typeof(Cat));

            Compose<IZoo>.Add(typeof(DallasZoo));

            Compose<IManager>.Add(typeof(MrJones));


            //Act
            var ctorInfo = typeof (Cat).GetExportableConstructor();
            
            // Assert
            Assert.True(ctorInfo != null);
        }

        [Fact]
        public void CanPopulateExportableCtor()
        {
            //Arrange
            Compose<IAnimal>.Exports.Clear();
            Compose<IManager>.Exports.Clear();
            Compose<IZoo>.Exports.Clear();
            Compose<ICar>.Exports.Clear();

            Compose<IAnimal>.Add(typeof(Lion));
            Compose<IAnimal>.Add(typeof(Dog));
            Compose<IAnimal>.Add(typeof(Cat));
            Compose<IZoo>.Add(typeof(DallasZoo));
            Compose<IManager>.Add(typeof(MrJones));
            Compose<ICar>.Add(typeof(Car));

            //Act
            var ctorInfo = typeof(Cat).GetExportableConstructor();
            var parameters = ctorInfo.GetCtorParameters();

            var obj =  (Cat)ctorInfo.Invoke(parameters);

            // Assert
            Assert.True(obj != null && obj.Manager != null && obj.Zoo != null);
        }

        [Fact]
        public void CanGetInstanceWithExportableCtor()
        {
            //Arrange
            Compose<IAnimal>.Exports.Clear();
            Compose<IManager>.Exports.Clear();
            Compose<IZoo>.Exports.Clear();
            Compose<ICar>.Exports.Clear();
            Compose<IAnimal>.Add(typeof(Lion));
            Compose<IAnimal>.Add(typeof(Dog));
            Compose<IAnimal>.Add(typeof(Cat));
            Compose<ICar>.Add(typeof(Car));
            Compose<IZoo>.Add(typeof(DallasZoo));
            Compose<IManager>.Add(typeof(MrJones));

            //Act            
            var obj = ReflectionHelpers.GetInstance<Cat>();

            // Assert
            Assert.True(obj != null && obj.Manager != null && obj.Manager.Car != null && obj.Zoo != null);
        }
    }
}
