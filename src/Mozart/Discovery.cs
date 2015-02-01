using System;
using System.Collections.Generic;
using System.Reflection;
using Mozart.Helpers;

namespace Mozart
{
    public static class Discovery
    {
        /// <summary>
        /// Return a list of all exported interfaces in the assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> InterfaceExports(Assembly assembly)
        {
            return assembly.ExportedInterfaces();
        }

        /// <summary>
        /// Returns enumerable tuples with [Type,Interface] for each exported class in the assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<Type,Type>> ClassExports(Assembly assembly)
        {
            return assembly.ExportedClasses();            
        }
    }
}