using System.Linq;
using System.Reflection;
using Mozart.Tests.Model;
using Xunit;

namespace Mozart.Tests
{

    public class DiscoveryTest
    {
        [Fact]
        public void CanFindDiscoverExportedInterfaces()
        {                    
            // Act
            var exports = Discovery.InterfaceExports(Assembly.GetAssembly(typeof (Dog)));

            //Assert
            Assert.True(exports.Contains(typeof(IAnimal)));        
        }

        [Fact]
        public void CanFindDiscoverExportedClasses()
        {            
            // Act
            var exports = Discovery.ClassExports(Assembly.GetAssembly(typeof(Dog)));

            //Assert
            Assert.True(exports.Any(p => p.Item1 == typeof(Cat)));
        }

        [Fact]
        public void WillNotDiscoverClassWithTheNoExportAttribute()
        {            
            // Act
            var exports = Discovery.ClassExports(Assembly.GetAssembly(typeof(Dog)));

            //Assert
            Assert.False(exports.Any(p => p.Item1 == typeof(Hippo)));
        }
    }
}
