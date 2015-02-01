using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;

namespace Mozart.Helpers
{
    public class ActivationManager
    {
        private static bool _hasInited;
        private static List<Assembly> _assemblies;

        // For unit test purpose
        public static void Reset()
        {
            _hasInited = false;
            _assemblies = null;
        }

        public static void Run()
        {
            if (_hasInited) return;

            //RunPreStartMethods(/*designerMode: HostingEnvironment.InClientBuildManager*/);

            //RunPostStartMethods();

            _hasInited = true;
        }

        internal static void ResetAssemblies()
        {
            _assemblies = null;
        }

        internal static IEnumerable<Assembly> Assemblies
        {
            get
            {
                if (_assemblies == null)
                {
                    // Cache the list of relevant assemblies, since we need it for both Pre and Post
                    _assemblies = new List<Assembly>();

                    foreach (var assemblyFile in GetAssemblyFiles())
                    {
                        try
                        {
                            // Ignore assemblies we can't load. They could be native, etc...
                            _assemblies.Add(Assembly.LoadFrom(assemblyFile));
                        }
// ReSharper disable once EmptyGeneralCatchClause
                        catch{}
                        //catch (Win32Exception ex)
                        //{
                        //    //Composable.InvokeErrorActions(new CompositionException(ex,string.Format("Could not load assmebly {0}",assemblyFile)));
                        //}
                        //catch (ArgumentException ex)
                        //{
                        //    //Composable.InvokeErrorActions(new CompositionException(ex, string.Format("Could not load assmebly {0}", assemblyFile)));
                        //}
                        //catch (FileNotFoundException ex)
                        //{
                        //    //Composable.InvokeErrorActions(new CompositionException(ex, string.Format("Could not load assmebly {0}", assemblyFile)));
                        //}
                        //catch (PathTooLongException ex)
                        //{
                        //    //Composable.InvokeErrorActions(new CompositionException(ex, string.Format("Could not load assmebly {0}", assemblyFile)));
                        //}
                        //catch (BadImageFormatException ex)
                        //{
                        //    //Composable.InvokeErrorActions(new CompositionException(ex, string.Format("Could not load assmebly {0}", assemblyFile)));
                        //}
                        //catch (SecurityException ex)
                        //{
                        //    //Composable.InvokeErrorActions(new CompositionException(ex, string.Format("Could not load assmebly {0}", assemblyFile)));
                        //}
                    }
                }

                return _assemblies;
            }
        }

        public static void AddAssembly(Assembly assembly)
        {
            var current = _assemblies.FirstOrDefault(p => p.FullName == assembly.FullName);
            if (current != null)
            {
                _assemblies.Remove(current);
            }


            _assemblies.Add(assembly);
        }

        private static IEnumerable<string> GetAssemblyFiles()
        {
            // When running under ASP.NET, find assemblies in the bin folder.
            // Outside of ASP.NET, use whatever folder WebActivator itself is in    
            var filter = new List<string> {"*.dll", "*.exe"};
            var directory = GetAssemblyDirectory();
            return filter.SelectMany(f => Directory.GetFiles(directory, f));
        }

        public static string GetAssemblyDirectory()
        {
            return HostingEnvironment.IsHosted
                ? HttpRuntime.BinDirectory
                : Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            //Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        }

        //// Return all the App_Code assemblies
        //private static IEnumerable<Assembly> AppCodeAssemblies
        //{
        //    get
        //    {
        //        // Return an empty list if we;re not hosted or there aren't any
        //        if (!HostingEnvironment.IsHosted || !_hasInited || BuildManager.CodeAssemblies == null)
        //        {
        //            return Enumerable.Empty<Assembly>();
        //        }

        //        return BuildManager.CodeAssemblies.OfType<Assembly>();
        //    }
        //}

        //public static void RunPreStartMethods(/*bool designerMode = false*/)
        //{
        //    RunActivationMethods<Attributes.PreApplicationStartMethodAttribute>(/*designerMode*/);
        //}

        //public static void RunPostStartMethods()
        //{
        //    RunActivationMethods<PostApplicationStartMethodAttribute>();
        //}

        //public static void RunShutdownMethods()
        //{
        //    RunActivationMethods<ApplicationShutdownMethodAttribute>();
        //}

        //// Call the relevant activation method from all assemblies
        //private static void RunActivationMethods<T>(/*bool designerMode = false*/) where T : BaseActivationMethodAttribute
        //{
        //    foreach (var assembly in Assemblies.Concat(AppCodeAssemblies))
        //    {
        //        foreach (
        //            var activationAttrib in
        //                assembly.GetActivationAttributes<T>().OrderBy(att => att.Order))
        //        {
        //            activationAttrib.InvokeMethod();
        //        }
        //    }
        //}
    }
}