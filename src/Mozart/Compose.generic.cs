using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mozart.Attributes;
using Mozart.Helpers;
using Mozart.Model;

namespace Mozart
{
    /// <summary>
    /// For speeding up invokes in the server
    /// </summary>
    public interface IDelegateInfo
    {
        /// <summary>
        /// Delegate to invoke for an method
        /// </summary>
        Delegate Delegate { get; set; }
        /// <summary>
        /// Parameters for the method
        /// </summary>
        ParameterInfo[] ParameterInfo { get; set; }
    }

    /// <summary>
    /// For speeding up invokes in the server
    /// </summary>
    public class DelegateInfo : IDelegateInfo
    {
        /// <summary>
        /// Delegate to invoke for an method
        /// </summary>
        public Delegate Delegate { get; set; }
        /// <summary>
        /// Parameters for the method
        /// </summary>
        public ParameterInfo[] ParameterInfo { get; set; }
    }

    /// <summary>
    /// Generic instance where T needs to be an interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Compose<T>
    {
        /// <summary>
        /// Contains all exported concrete types for the interface T
        /// </summary>
        public static IList<Type> Exports { get; private set; }

        /// <summary>
        /// If a singleton export, this will hold the singleton object
        /// </summary>
        public static T Singleton { get; private set; }
        private static bool SingletonIsSet { get; set; }

        /// <summary>
        /// The export attribute for the interface
        /// </summary>
        public static ExportAttribute ExportAttribute { get; private set; }

        /// <summary>
        /// Ctor, make sure that T is an interface.
        ///  - Find the export for T
        ///  - Add the generic type for this instance to Compose repository
        /// </summary>
        static Compose()
        {
            if (!typeof(T).IsInterface)
                throw new Exception("Repository type T can only be an interface");

            ExportAttribute = typeof(T).ExportAttribute();

            if (!Compose.ExportedInterfaceFactory.ContainsKey(typeof(T)))
                Compose.ExportedInterfaceFactory.Add(typeof(T), typeof(T).MakeGenericExport());

            Exports = new List<Type>();
        }

        public static void Init() { }

        /// <summary>
        /// Add a type to the interface T,
        /// the type has to implement the interface T
        /// </summary>
        /// <param name="type"></param>
        public static void Add(Type type)
        {
            if (!CanAddItem(type)) return;

            if (ExportAttribute.InstanceRule == InstanceRule.Singleton)
            {
                AddOrUpdateSingleton(type);
                return;
            }

            Exports.Add(type);
        }

        private static void AddOrUpdateSingleton(Type type)
        {
            if (!SingletonIsSet)
            {
                Singleton = (T)Activator.CreateInstance(type);
                SingletonIsSet = true;
                return;
            }

            if (ExportAttribute.Rewritable)
            {
                Singleton = (T)Activator.CreateInstance(type);
            }

        }

        private static bool CanAddItem(Type type)
        {
            return !Exports.Contains(type) && !type.IsInterface && typeof(T).IsAssignableFrom(type);
        }


        /// <summary>
        /// Expects there to be only one export of the interface T
        /// If several exists the first export will be used
        /// 
        /// To get a specific type use the the overloads of Get
        /// </summary>
        /// <returns></returns>
        public static T Get()
        {
            if (ExportAttribute.InstanceRule == InstanceRule.Multiple)
                Activator.CreateInstance(Exports.First());
            //throw new Exception("Cant get singleton from type marked as multiple");

            return Singleton;
        }

        public static T Get<TA>()
        {
            return Get(typeof(TA));
        }

        public static T Get(Type t)
        {
            if (ExportAttribute.InstanceRule == InstanceRule.Singleton)
                return Get();

            if (t.IsInterface)
                return (T)Exports.First().GetInstance(); //(T)Activator.CreateInstance(Exports.First());
            return (T)Activator.CreateInstance(Exports.First(p => p == t));
        }

        public static IList<T> GetAll()
        {
            return Exports.Select(export => (T)Activator.CreateInstance(export)).ToList();
        }
    }
}