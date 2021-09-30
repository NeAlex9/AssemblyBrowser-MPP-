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
        private BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;
        public FieldProcessor FieldProcessor{ get; private set; }
        public Dictionary<string, List<TypeData>> NamespaceTypesDic { get; private set; }

        public AssemblyBrowser()
        {
            this.FieldProcessor = new FieldProcessor();
            this.NamespaceTypesDic = new Dictionary<string, List<TypeData>>();
        }

        public List<NamespaceData> GetAssemblyData(string path)
        {
            this.NamespaceTypesDic.Clear();
            var assembly = Assembly.LoadFrom(path);
            var assemblyTypes = assembly.GetTypes();
            foreach (var assemblyType in assemblyTypes)
            {
                var typeData = GetTypeData(assemblyType);
                if (this.NamespaceTypesDic.TryGetValue(assemblyType?.Namespace ?? "No namespace", out List<TypeData> namespaceTypes))
                {
                    namespaceTypes.Add(typeData);
                }
                else
                {
                    this.NamespaceTypesDic.Add(assemblyType?.Namespace ?? "No namespace", new List<TypeData>() { typeData });
                }
            }

            var namespaces = new List<NamespaceData>();
            foreach (var pair in NamespaceTypesDic)
            {
                namespaces.Add(new NamespaceData(pair.Key, pair.Value));
            }

            return namespaces;
        }

        private Modifiers GetTypesModifier(Type type)
        {
            Modifiers modifier = (Modifiers)0;
            if (type.IsAbstract && type.IsSealed)
                return modifier |= Modifiers.Static;
            if (type.IsAbstract) modifier |= Modifiers.Abstract;

            return modifier;
        }

        private string GetTypeName(Type type)
        {
            //if (type.IsDe)
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

        private string ConvertTypeNameToString(Type type)
        {
            if (type.IsGenericType)
            {
                var nestedTypes = type.GetGenericArguments();
                var res = type.Name.Remove(type.Name.Length - 2, 2) + "<";
                var stringRepresentation = string.Empty;
                foreach (var nestedType in nestedTypes)
                {
                    stringRepresentation += ConvertTypeNameToString(nestedType) + ", ";
                }

                stringRepresentation = stringRepresentation.Length > 0 ? stringRepresentation.Remove(stringRepresentation.Length - 2, 2) : stringRepresentation;
                return res + stringRepresentation + ">";
            }
            else
            {
                return type.Name;
            }
        }   ///!

        private TypeData GetTypeData(Type type)
        {
            var members = new List<DataContainer>();
            foreach (var method in type.GetMethods(bindingFlags))
            {
                if (!method.IsSpecialName)
                {
                    members.Add(GetMethod(method));
                }
            }

            foreach (var constructor in type.GetConstructors(bindingFlags))
            {
                members.Add(GetConstructor(constructor));
            }

            foreach (var field in type.GetFields(bindingFlags)) members.Add(GetFiled(field));

            foreach (var property in type.GetProperties(bindingFlags)) members.Add(GetProperty(property));

            return new TypeData(GetTypeName(type), type.Name, GetTypeAccessModifier(type), members, GetTypesModifier(type));
        }

        private string GetAccessModifier(dynamic inf)
        {
            try
            {
                if (inf.IsPrivate) return "private";
                if (inf.IsPublic) return "public";
                if (inf.IsAssembly) return "internal";
                if (inf.IsFamilyAndAssembly) return "private protected";
                return "protected internal";
            }
            catch (Exception e)
            {
                return "";
            }
        } 

        private Modifiers GetMethodModifiers(MethodInfo methodInf)
        {
            Modifiers modifier = (Modifiers)0;
            if (methodInf.IsAbstract) modifier |= Modifiers.Abstract;
            else if (methodInf.IsVirtual) modifier |= Modifiers.Virtual;
            if (methodInf.IsStatic) modifier |= Modifiers.Static;
            //if (methodInf.IsFinal && (methodInf.IsVirtual || methodInf.IsAbstract)) modifier |= Modifiers.Sealed;

            return modifier;
        }

        private Dictionary<string, string> GetParameters(dynamic methodInf)
        {
            var parameters = new Dictionary<string, string>();
            try
            {
                foreach (ParameterInfo parameterInfo in methodInf.GetParameters())
                {
                    parameters.Add(parameterInfo.Name, ConvertTypeNameToString(parameterInfo.ParameterType));
                }

                return parameters;
            }
            catch (Exception e)
            {
                return new Dictionary<string, string>();
            }
        }

        private DataContainer GetMethod(MethodInfo methodInf)
        {
            return new MethodData(methodInf.Name, GetAccessModifier(methodInf),
                methodInf.ReturnType.Name, GetParameters(methodInf), GetMethodModifiers(methodInf));
        }

        private DataContainer GetConstructor(ConstructorInfo constructor)
        {
            return new MethodData(constructor.Name, GetAccessModifier(constructor),
                "", GetParameters(constructor), 0);
        }

        private Modifiers GetFieldModifiers(FieldInfo fieldInfo) 
        {
            Modifiers modifier = (Modifiers)0;
            if (fieldInfo.IsStatic) modifier |= Modifiers.Static;
            if (fieldInfo.IsInitOnly) modifier |= Modifiers.Readonly;

            return modifier;
        }   ///!

        private DataContainer GetFiled(FieldInfo fieldInfo)
        {
            return new FieldData(fieldInfo.Name, GetAccessModifier(fieldInfo), ConvertTypeNameToString(fieldInfo.FieldType), GetFieldModifiers(fieldInfo));
        }   ///!

        private Modifiers GetPropertyModifiers(PropertyInfo propertyInfo)
        {
            var modifier = GetMethodModifiers(propertyInfo.GetAccessors(true)[0]);
            return modifier;
        }

        private DataContainer GetProperty(PropertyInfo propertyInfo)
        {
            return new PropertyData(propertyInfo.Name, GetAccessModifier(propertyInfo.GetAccessors(true)[0]),
                ConvertTypeNameToString(propertyInfo.PropertyType), GetPropertyModifiers(propertyInfo), propertyInfo.GetAccessors(true));
        }
    }
}