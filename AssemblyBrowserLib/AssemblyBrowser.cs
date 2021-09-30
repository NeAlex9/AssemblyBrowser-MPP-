using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AssemblyBrowserLib.DataHolders;
using AssemblyBrowserLib.DataHolders.TypesMembers;

namespace AssemblyBrowserLib
{
    public class AssemblyBrowser
    {
        public TypeProcessor TypeProcessor { get; private set; }

        public Dictionary<string, List<TypeData>> NamespaceTypesDic { get; private set; }

        public AssemblyBrowser()
        {
            this.TypeProcessor = new TypeProcessor();
            this.NamespaceTypesDic = new Dictionary<string, List<TypeData>>();
        }

        public void ProcessExtensionMethods()
        {
            foreach (var keyValue in NamespaceTypesDic)
            {
                var types = keyValue.Value;
                for (int i = 0; i < types.Count; i++)
                {
                    var type = types[i];
                    if (!type.IsExtension) continue;

                    if (TryGetExtensionMethods(type, out List<MethodData> methods, out List<int> indexes))
                    {
                        type.Members.RemoveAll(elem => methods.Any(newElem => elem == newElem));
                        foreach (var method in methods)
                        {
                            var extensibleType = FindExtensibleType(method.Parameters.Values.First());
                            extensibleType?.Members.Add(method);
                        }

                    }
                }
            }
        }

        public bool TryGetExtensionMethods(TypeData type, out List<MethodData> methods, out List<int> indexes)
        {
            methods = new List<MethodData>();
            indexes = new List<int>();
            for (int j = 0; j < type.Members.Count; j++)
            {
                var member = type.Members[j];
                if (!(member is MethodData) || !((MethodData)member).IsExtension) continue;

                methods.Add((MethodData)member);
                indexes.Add(j);
            }

            if (methods.Count > 0) return true;
            else return false;

        }

        public TypeData FindExtensibleType(string extensibleType)
        {
            foreach (var keyValue in NamespaceTypesDic)
            {
                var types = keyValue.Value;
                foreach (var type in keyValue.Value)
                {
                    if (type.Name == extensibleType)
                    {
                        return type;
                    }
                }
            }

            return null;
        }

        public List<NamespaceData> GetAssemblyData(string path)
        {
            this.NamespaceTypesDic.Clear();
            var assembly = Assembly.LoadFrom(path);
            var assemblyTypes = assembly.GetTypes();
            foreach (var assemblyType in assemblyTypes)
            {
                var typeData = TypeProcessor.GetData(assemblyType);
                if (this.NamespaceTypesDic.TryGetValue(assemblyType?.Namespace ?? "No namespace", out List<TypeData> namespaceTypes))
                {
                    namespaceTypes.Add(typeData);
                }
                else
                {
                    this.NamespaceTypesDic.Add(assemblyType?.Namespace ?? "No namespace", new List<TypeData>() { typeData });
                }
            }

            ProcessExtensionMethods();
            var namespaces = new List<NamespaceData>();
            foreach (var pair in NamespaceTypesDic)
            {
                namespaces.Add(new NamespaceData(pair.Key, pair.Value));
            }

            return namespaces;
        }
    }
}