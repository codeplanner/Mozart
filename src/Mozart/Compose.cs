using System;
using System.Collections.Generic;
using Mozart.Helpers;

namespace Mozart
{
    public static class Compose
    {
        /// <summary>
        /// To access the static generic instance with ease
        /// </summary>
        public static IDictionary<Type,Type> ExportedInterfaceFactory = new Dictionary<Type, Type>(); 

        /// <summary>
        /// To access the ctor with exported types with ease
        /// TODO: NOT YET IMPLEMENTED
        /// </summary>
        public static IDictionary<Type,IDelegateInfo> ExportedInterfaceCtorFactory = new Dictionary<Type, IDelegateInfo>();

        static Compose()
        {
            _Compose();
        }

        private static void _Compose()
        {
            // 1: Find exported interfaces
            foreach (var ass in ActivationManager.Assemblies)
            {
                // Create generic static instances
                foreach (var export in Discovery.InterfaceExports(ass))
                {
                    export.MakeGenericExport();
                }

                // Add all exports to the generic static instances
                foreach (var export in Discovery.ClassExports(ass))
                {
                    // ignore if no interface matched the class.... 
                    if (export.Item2 == null) continue;

                    var generic = ExportedInterfaceFactory[export.Item2];
                    var m = generic.GetMethod("Add", new[] { typeof(Type) });
                    m.Invoke(generic, new object[] { export.Item1 });
                }
            }
        }

        public static void ReCompose()
        {
            ExportedInterfaceFactory.Clear();
            _Compose();
        }

        /// <summary>
        /// Does nothing.. remove later when others are exposed
        /// </summary>
        public static void Init() { }              
    }
}