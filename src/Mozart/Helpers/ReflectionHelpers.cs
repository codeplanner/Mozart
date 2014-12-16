using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mozart.Helpers
{
    public static class ReflectionHelpers
    {
        //public static IDictionary<Type,Type> GenericExportInstances = new Dictionary<Type, Type>(); 

        /// <summary>
        /// Find the first exportable Ctor (maybe this should use ImportingCtor attr ?)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ConstructorInfo GetExportableConstructor(this Type type)
        {
            var cc = type.GetConstructors()
                .FirstOrDefault(c =>c.CtorIsExportable());

            return cc;
        }

        /// <summary>
        /// Check that the Ctor contains interface parameters only
        /// </summary>
        /// <param name="ctorInfo"></param>
        /// <returns></returns>
        public static bool CtorIsExportable(this ConstructorInfo ctorInfo)
        {
            return ctorInfo.GetParameters()
                .All(p => p.ParameterType.IsInterface && p.ParameterType.IsExported()) && ctorInfo.GetParameters().Any();
        }

        public static object[] GetCtorParameters(this ConstructorInfo ctorInfo)
        {
            var parameters = new List<object>();
            foreach (var parameterInfo in ctorInfo.GetParameters())
            {
                var export = parameterInfo.ParameterType.MakeGenericExport();
                var method = export.GetMethod("Get", new Type[] { typeof(Type) });
                parameters.Add(method.Invoke(export, new object[] { parameterInfo.ParameterType }));
            }
            return parameters.ToArray();
        }

        public static bool IsExported(this Type type)
        {
            return Compose.ExportedInterfaceFactory.ContainsKey(type);
            //var sasas = (ss.Key == type);
            //var exported = Compose.ExportedInterfaceFactory.ContainsKey(type);
            //return exported;
            //Type export = type.MakeGenericExport();

            //var getMethod = export.GetMethod("Get", new Type[] { typeof(Type) });

            //return getMethod.Invoke(export, new object[] { type }) != null;
        }

        public static Type MakeGenericExport(this Type t)
        {
            //TODO: Only do this once and then save the constructed item in a list

            if(!t.IsInterface) throw new Exception("Something that makes sense should be written here");

            //if (GenericExportInstances.ContainsKey(t))
            //    return GenericExportInstances[t];

            Type export = typeof(Compose<>);
            
            Type[] typeArgs = { t };

            Type constructed = export.MakeGenericType(typeArgs);

            var getMethod = constructed.GetMethod("Init");

            getMethod.Invoke(constructed,null);

            //Activator.CreateInstance(constructed, new []{t});
            //GenericExportInstances.Add(t,constructed);

            return constructed;
        }

        //TODO: move to delegate to avoid reflection
        public static T GetInstance<T>()
        {
            var type = typeof (T);
            //var ctorInfo = type.GetExportableConstructor();
            //if (ctorInfo == null) return Activator.CreateInstance<T>();
            //var parameters = ctorInfo.GetCtorParameters();

            //var p = new List<object>();
            //foreach (var parameter in parameters)
            //{
            //    p.Add(parameter.GetType().GetIstance);
            //}
            
            return (T) type.GetInstance();// ?? Activator.CreateInstance<T>();// ctorInfo.Invoke(parameters);
        }

        public static object GetInstance(this Type type)
        {
            var ctorInfo = type.GetExportableConstructor();
            if (ctorInfo == null) return Activator.CreateInstance(type);
            var parameters = ctorInfo.GetCtorParameters();

            var p = new List<object>();
            foreach (var parameter in parameters)
            {
                p.Add(parameter.GetType().GetInstance());
            }

            return ctorInfo.Invoke(p.ToArray());
        }
    }
}
