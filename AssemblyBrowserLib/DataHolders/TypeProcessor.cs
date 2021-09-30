using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using AssemblyBrowserLib.DataHolders.TypesMembers;

namespace AssemblyBrowserLib.DataHolders
{
    public class TypeProcessor
    {
        private BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;
        public MemberProcessor FieldProcessor { get; private set; }
        public MemberProcessor MethodProcessor { get; private set; }
        public MemberProcessor PropertyProcessor { get; private set; }

        public TypeProcessor()
        {
            this.FieldProcessor = new FieldProcessor();
            this.MethodProcessor = new MethodProcessor();
            this.PropertyProcessor = new PropertyProcessor();
        }

        public Type TypeData { get; private set; }

        private string GetAccessModifier()
        {
            if (TypeData.IsNotPublic) return "internal";

            return "public";
        }

        private string GetTypeName()
        {
            if (TypeData.IsClass && TypeData.BaseType.Name == "MulticastDelegate") return "delegate";
            if (TypeData.IsClass) return "class";
            if (TypeData.IsInterface) return "interface";
            if (TypeData.IsEnum) return "enum";
            if (TypeData.IsValueType && !TypeData.IsPrimitive) return "struct";

            return null;
        }

        private Modifiers GetModifiers()
        {
            Modifiers modifier = (Modifiers)0;
            if (TypeData.IsAbstract && TypeData.IsSealed)
                return modifier |= Modifiers.Static;
            if (TypeData.IsAbstract) modifier |= Modifiers.Abstract;

            return modifier;
        }

        public TypeData GetData(Type type)
        {
            TypeData = type;
            var members = new List<DataContainer>();
            foreach (var method in TypeData.GetMethods(bindingFlags))
            {
                if (!method.IsSpecialName)
                {
                    members.Add(this.MethodProcessor.GetData(method));
                }
            }

            foreach (var constructor in TypeData.GetConstructors(bindingFlags)) members.Add(this.MethodProcessor.GetData(constructor));
            
            foreach (var field in TypeData.GetFields(bindingFlags)) members.Add(this.FieldProcessor.GetData(field));

            foreach (var property in TypeData.GetProperties(bindingFlags)) members.Add(this.PropertyProcessor.GetData(property));

            return new TypeData(GetTypeName(), TypeData.Name, GetAccessModifier(), members, GetModifiers(), TypeData.IsDefined(typeof(ExtensionAttribute)));
        }
    }

    public class TypeData : DataContainer
    {
        public List<DataContainer> Members{ get; private set; }

        public bool IsExtension { get; private set; }

        public string TypeName{ get; private set; }

        protected override string ConvertModifierToString()
        {
            if ((this.Modifiers & Modifiers.Abstract) != 0) return "abstract ";
            if ((this.Modifiers & Modifiers.Static) != 0) return "static ";

            return "";
        }

        public TypeData(string type, string name, string accessModifier, List<DataContainer> members, Modifiers modifiers, bool isExtension) : base(name, accessModifier, modifiers)
        {
            this.IsExtension = isExtension;
            this.TypeName = type;
            this.Members = members;
            this.ContainerDeclaration = ToString();
        }

        public sealed override string ToString()
        {
            string res = string.Empty;
            res += this.AccessModifier;
            res += " " + ConvertModifierToString();
            res += " " + this.TypeName;
            res += " " + this.Name;
            return res;
        }
    }
}
