using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mozart.Attributes;

namespace Mozart.Helpers
{
    public static class DiscoveryHelpers
    {
        /// <summary>
        /// Check it the type has an attribute of type T attached
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool HasAttributeOfType<T>(this Type t)
        {
            return t.GetCustomAttributes(typeof(T), false).Any();
        }

        public static T GetAttributeOfType<T>(this Type t)
        {
            return (T)t.GetCustomAttributes(typeof(T), false).SingleOrDefault();
        }

        /// <summary>
        /// Find all public interfaces in the assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> PublicInterfaces(this Assembly assembly)
        {
            return assembly.GetExportedTypes().Where(p => p.IsInterface);
        }

        /// <summary>
        /// Find all public, non-generic, non-abstract classes in the assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> PublicClasses(this Assembly assembly)
        {
            return assembly.GetExportedTypes().Where(p => p.IsClass && !p.IsAbstract && !p.IsGenericType);
        }

        /// <summary>
        /// Checks if the type has the ExportAttribute or if the type 
        /// implements an interface that has the ExportAttribute
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsExport(this Type type)
        {
            return type.HasAttributeOfType<ExportAttribute>() || type.ImplementsExportedInterface();
        }

        /// <summary>
        /// Check if the type implements an interface that has the ExportAttribute attached
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool ImplementsExportedInterface(this Type type)
        {
            var exportOnInterface = Compose.ExportedInterfaceFactory.FirstOrDefault(i => type.GetInterfaces().Contains(i.Key) && !type.IsAbstract);
            if (exportOnInterface.Key == null)
                return false;

            return exportOnInterface.Key.GetCustomAttributes(typeof(ExportAttribute), false).Any();
        }

        /// <summary>
        /// Checks if the type is decorated with the NoExport attribute
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNoExport(this Type type)
        {
            return type.GetCustomAttributes(typeof(NoExportAttribute), false).Any();
        }

        /// <summary>
        /// Find all public interfaces decorated with the ExportAttribute
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> ExportedInterfaces(this Assembly assembly)
        {
            return assembly.PublicInterfaces().Where(p => p.IsExport());
        }

        /// <summary>
        /// Find all classes that implements exported interfaces or has the ExportAttribute them selves 
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<Type, Type>> ExportedClasses(this Assembly assembly)
        {
            return assembly.PublicClasses().Where(p => p.IsExport() && !p.IsNoExport()).Select(p => new Tuple<Type, Type>(p, p.ExportedInterface()));
        }

        public static Type ExportedInterface(this Type type)
        {
            var export = type.GetAttributeOfType<ExportAttribute>();// .GetCustomAttributes(typeof(ExportAttribute), false).FirstOrDefault() as ExportAttribute;

            return export != null ? export.Type : Compose.ExportedInterfaceFactory.Single(i => type.GetInterfaces().Contains(i.Key) && !type.IsAbstract).Key;

            //return exportOnInterface.Key;            
        }

        public static ExportAttribute ExportAttribute(this Type type)
        {
            return type.GetCustomAttributes(typeof(ExportAttribute), false).FirstOrDefault() as ExportAttribute;
        }
    }
}
