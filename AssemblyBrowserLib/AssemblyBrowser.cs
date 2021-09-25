using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib
{
    public class AssemblyBrowser
    {
        public Dictionary<string, List<TypeData>> NamespaceTypesDic { get; private set; }

        public AssemblyBrowser()
        {
            this.NamespaceTypesDic = new Dictionary<string, List<TypeData>>();
        }

        public AssemblyData GetAssemblyData(string path)
        {
            var assembly = Assembly.LoadFrom(path);
            var assemblyTypes = assembly.GetTypes();
            foreach (var assemblyType in assemblyTypes)
            {
                var typeData = GetTypeData(assemblyType);
                if (this.NamespaceTypesDic.TryGetValue(assemblyType.Namespace, out List<TypeData> namespaceTypes))
                {
                    namespaceTypes.Add(typeData);
                }
                else
                {
                    this.NamespaceTypesDic.Add(assemblyType.Namespace, new List<TypeData>() { typeData });
                }
            }

            var namespaces = new List<NamespaceData>();
            foreach (var pair in NamespaceTypesDic)
            {
                namespaces.Add(new NamespaceData(pair.Key, pair.Value));
            }

            return new AssemblyData(namespaces);
        }       

        private string GetTypeName(Type type)
        {
            if (type.IsClass) return "class";
            if (type.IsInterface) return "interface";
            if (type.IsEnum) return "enum";
            if (type.IsValueType && !type.IsPrimitive) return "struct";
            return null;
        }

        private string GetTypeAccessModifier(Type type)
        {
            if (type.IsNotPublic) return "internal";

            return "public";
        }

        private TypeData GetTypeData(Type type)
        {
            var members = new List<TypesMember>();
            foreach (var method in type.GetMethods()) members.Add(GetMethod(method));

            foreach (var field in type.GetFields()) members.Add(GetFiled(field));

            foreach (var property in type.GetProperties()) members.Add(GetProperty(property));

            return new TypeData(GetTypeName(type), type.Name, GetTypeAccessModifier(type), members);
        }

        private string GetAccessModifier(dynamic methodInf)
        {
            if (methodInf.IsPrivate) return "private";
            if (methodInf.IsPublic) return "public";
            if (methodInf.IsAssembly) return "internal";
            if (methodInf.IsFamilyAndAssembly) return "private protected";
            return "protected internal";
        }   //????????????

        private Modifiers GetMethodModifiers(MethodInfo methodInf)
        {
            Modifiers modifier = (Modifiers)0;
            if (methodInf.IsAbstract) modifier |= Modifiers.Abstract;
            if (methodInf.IsVirtual) modifier |= Modifiers.Virtual;
            if (methodInf.IsStatic) modifier |= Modifiers.Static;
            if (methodInf.IsFinal && (methodInf.IsVirtual || methodInf.IsVirtual)) modifier |= Modifiers.Sealed;

            return modifier;
        }

        private Dictionary<string, string> GetParameters(MethodInfo methodInf)
        {
            var parameters = new Dictionary<string, string>();
            foreach (ParameterInfo parameterInfo in methodInf.GetParameters())
            {
                parameters.Add(parameterInfo.Name, parameterInfo.ParameterType.Name);
            }

            return parameters;
        }

        private TypesMember GetMethod(MethodInfo methodInf)
        {
            return new MethodData(methodInf.Name, GetAccessModifier(methodInf),
                methodInf.ReturnType.Name, GetParameters(methodInf), GetMethodModifiers(methodInf));
        }

        private Modifiers GetFieldModifiers(FieldInfo fieldInfo)
        {
            Modifiers modifier = (Modifiers)0;
            if (fieldInfo.IsStatic) modifier |= Modifiers.Static;
            if (fieldInfo.IsInitOnly) modifier |= Modifiers.Readonly;

            return modifier;
        }

        private TypesMember GetFiled(FieldInfo fieldInfo)
        {
            return new FieldData(fieldInfo.Name, GetAccessModifier(fieldInfo), fieldInfo.FieldType.Name, GetFieldModifiers(fieldInfo));
        }

        private Modifiers GetPropertyModifiers(PropertyInfo propertyInfo)
        {
            var modifier = GetMethodModifiers(propertyInfo.GetAccessors(true)[0]);
            return modifier;
        }

        private TypesMember GetProperty(PropertyInfo propertyInfo)
        {
            return new PropertyData(propertyInfo.Name, GetAccessModifier(propertyInfo),
                propertyInfo.PropertyType.Name, GetPropertyModifiers(propertyInfo));
        }
    }
}
