using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Hont.PostProcessing
{
    public static class HontPostProcessingUtility
    {
        public static bool IsSubClassOf(Type type, Type baseType)
        {
            var b = type.BaseType;
            while (b != null)
            {
                if (b.Equals(baseType))
                {
                    return true;
                }
                b = b.BaseType;
            }
            return false;
        }

        public static List<T> InstancesFromBaseClass<T>()
        {
            var result = new List<T>();

            var type = typeof(T);

            var assembly = Assembly.GetAssembly(type);

            var types = assembly.GetExportedTypes();

            var filterTypes = Array.FindAll(types, m => IsSubClassOf(m, type) && !m.IsGenericType && !m.IsAbstract);
            result.AddRange(Array.ConvertAll(filterTypes, m => (T)Activator.CreateInstance(m)));

            return result;
        }

        public static Type[] GetChildrenClasses<T>()
        {
            var type = typeof(T);

            var assembly = Assembly.GetAssembly(type);

            var types = assembly.GetExportedTypes();

            var filterTypes = Array.FindAll(types, m => IsSubClassOf(m, type) && !m.IsGenericType && !m.IsAbstract);
            return filterTypes;
        }

        //public
    }
}
