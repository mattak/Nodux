using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nodux.Core;

namespace Nodux.PluginEditor
{
    public static class TypeSelectionHolder
    {
        private static List<Type> _candidateTypeList = null;
        private static IDictionary<string, List<Type>> _typeListMap = new Dictionary<string, List<Type>>();
        private static IDictionary<string, List<string>> _typeNameListMap = new Dictionary<string, List<string>>();

        private static IDictionary<string, List<string>> _typeAssemblyQualifiedNameListMap =
            new Dictionary<string, List<string>>();

        public static IDictionary<string, List<Type>> TypeListMap => _typeListMap;

        public static IDictionary<string, List<string>> TypeAssemblyQualifiedNameListMap =>
            _typeAssemblyQualifiedNameListMap;

        public static IDictionary<string, List<string>> TypeNameListMap => _typeNameListMap;

        public static void CheckInitialize(string filterCategory)
        {
            if (_candidateTypeList == null)
            {
                _candidateTypeList = AppDomain.CurrentDomain.GetAssemblies().SelectMany(it => it.GetTypes())
                    .Where(it => it.GetCustomAttribute(typeof(TypeSelectionEnable)) != null)
                    .ToList();
            }

            if (filterCategory != null && _candidateTypeList != null && !_typeListMap.ContainsKey(filterCategory))
            {
                _typeListMap[filterCategory] = _candidateTypeList
                    .Select(it => new
                    {
                        Type = it,
                        Category = ((TypeSelectionEnable) it.GetCustomAttribute(typeof(TypeSelectionEnable))).Category
                    })
                    .Where(it => filterCategory == it.Category)
                    .Select(it => it.Type)
                    .ToList();

                _typeAssemblyQualifiedNameListMap[filterCategory] =
                    _typeListMap[filterCategory].Select(it => it.AssemblyQualifiedName).ToList();
                _typeNameListMap[filterCategory] =
                    _typeListMap[filterCategory].Select(it => it.FullName).ToList();
            }
        }

        public static IList<Type> GetTypeList(string filterCategory)
        {
            CheckInitialize(filterCategory);
            return _typeListMap[filterCategory];
        }

        public static IList<string> GetTypeNameList(string filterCategory)
        {
            CheckInitialize(filterCategory);
            return _typeNameListMap[filterCategory];
        }

        public static IList<string> GetTypeAssemblyQualifiedNameList(string filterCategory)
        {
            CheckInitialize(filterCategory);
            return _typeNameListMap[filterCategory];
        }
    }
}