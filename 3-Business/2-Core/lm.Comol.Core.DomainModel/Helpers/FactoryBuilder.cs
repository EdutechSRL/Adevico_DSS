using System.Reflection;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel.Helpers
{
	public class FactoryBuilder
	{
        ///// <summary>
        /////    Create the requested Factory
        ///// </summary>
        ///// <typeparam name="t">type of</typeparam>
        ///// <param name="classname">class name</param>
        ///// <returns>new instance of t</returns>
        ///// <remarks></remarks>
        //public static T BuildFactory<T>(string classname)
        //{
        //    T obj = (T)CreateObject<T>(classname);
        //    return obj;
        //}

        ///// <summary>
        /////    Generic BuildFactory
        ///// </summary>
        ///// <param name="classname">class full name</param>
        ///// <returns>new instance of t</returns>
        ///// <remarks></remarks>
        //public static object BuildFactory(string classname)
        //{
        //    Assembly asembly = Assembly.GetExecutingAssembly();
        //    object obj = CreateObject(asembly, classname);
        //    return obj;
        //}

        //public static object BuildFactory(Assembly assembly, string classname)
        //{
        //    return CreateObject(assembly, classname);
        //}

        ///// <summary>
        /////    Obtains a list of Type that implements the given interface
        ///// </summary>
        ///// <typeparam name="t">interface</typeparam>
        ///// <returns>list of type</returns>
        ///// <remarks></remarks>
        //public static IList<Type> iFactoryImplementations<T>()
        //{
        //    return (from item in Assembly.GetAssembly(typeof(T)).GetTypes() where IsImplemented(item, typeof(T)) select item).ToList<Type>();
        //}

        //#region "Create Object"

        ///// <summary>
        /////    Create an instance of a class, from assembly and class fullname
        ///// </summary>    
        ///// <param name="assemb">assembly</param>
        ///// <param name="fullname">class fullname</param>
        ///// <returns>a new object of requested class</returns>
        ///// <remarks></remarks>
        //private static object CreateObject(Assembly assemb, string fullname)
        //{
        //    return assemb.CreateInstance(fullname);
        //}

        ///// <summary>
        /////    Create an instance of a class, extracting the assembly from the type
        ///// </summary>
        ///// <typeparam name="t">type of</typeparam>
        ///// <param name="fullname">class fullname</param>
        ///// <returns>new instance of class requested</returns>
        ///// <remarks></remarks>
        //private static object CreateObject<T>(string fullname)
        //{
        //    Assembly assemb = Assembly.GetAssembly(typeof(T));
        //    return CreateObject(assemb, fullname);
        //}

        ///// <summary>
        /////    Create an instance of a class, extracting the assembly from dll or assembly name
        ///// </summary>    
        ///// <param name="DLLorAssemblyname">path of dll, or assembly name</param>
        ///// <param name="fullname">class fullname</param>
        ///// <param name="isDLL">true: DLL , false: assembly name</param>
        ///// <returns>new instance of class requested</returns>
        ///// <remarks></remarks>

        //private static object CreateObject(string DLLorAssemblyname, string fullname, bool isDLL = false)
        //{
        //    Assembly assemb = default(Assembly);
        //    if (isDLL) {
        //        assemb = Assembly.LoadFile(DLLorAssemblyname);
        //    } else {
        //        assemb = Assembly.Load(DLLorAssemblyname);
        //    }
        //    return CreateObject(assemb, fullname);
        //}
        //#endregion

        //#region "General utilities"
        ///// <summary>
        /////    Check if a type implements an interface
        ///// </summary>
        ///// <param name="objectType">type to check</param>
        ///// <param name="interfaceType">interface that must be implemented</param>
        ///// <returns>check implementation</returns>
        ///// <remarks></remarks>
        
        //private static Boolean IsImplemented(Type objectType, Type interfaceType)
        //{
        //    return (from thisInterface in objectType.GetInterfaces() where thisInterface == interfaceType select thisInterface).Count() > 0;
        //}

        //#endregion
        public static T BuildFactory<T>(String classname)
        {
            T obj = (T)CreateObject<T>(classname);
            return obj;
        }
        public static object BuildFactory(Assembly assemb, String fullname)
        {
            object obj = CreateObject(assemb, fullname);
            return obj;
        }

        private static object CreateObject(Assembly assemb, String fullname)
        {
            return assemb.CreateInstance(fullname);
        }

        private static object CreateObject<T>(String fullname)
        {
            Assembly assemb = Assembly.GetAssembly(typeof(T));
            return CreateObject(assemb, fullname);
        }

        private static object CreateObject(String DLLorAssemblyname, String fullname, Boolean isDLL = false)
        {
            Assembly assemb;
            if (isDLL)
            {
                assemb = Assembly.LoadFile(DLLorAssemblyname);
            }
            else
            {
                assemb = Assembly.Load(DLLorAssemblyname);
            }
            return CreateObject(assemb, fullname);
        }

        public static IList<Type> iFactoryImplementations<T>()
        {

            return (from item in Assembly.GetAssembly(typeof(T)).GetTypes() where IsImplemented(item, typeof(T)) select item).ToList<Type>();
        }

        private static Boolean IsImplemented(Type objectType, Type interfaceType)
        {
            return (from thisInterface in objectType.GetInterfaces() where thisInterface == interfaceType select thisInterface).Count() > 0;
        }


	}
}