using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AssemblyBrowserLib.DataHolders.TypesMembers
{
    public class MethodProcessor : MemberProcessor
    {
        public MethodBase MethodInfo { get; private set; }

        protected override string GetAccessModifier()
        {
            if (MethodInfo.IsPrivate) return "private";
            if (MethodInfo.IsPublic) return "public";
            if (MethodInfo.IsAssembly) return "internal";
            if (MethodInfo.IsFamilyAndAssembly) return "private protected";

            return "protected internal";
        }

        protected override Modifiers GetModifiers()
        {
            Modifiers modifier = (Modifiers)0;
            if (MethodInfo.IsAbstract) modifier |= Modifiers.Abstract;
            else if (MethodInfo.IsVirtual) modifier |= Modifiers.Virtual;
            if (MethodInfo.IsStatic) modifier |= Modifiers.Static;

            return modifier;
        }

        private Dictionary<string, string> GetParameters()
        {
            var parameters = new Dictionary<string, string>();
            try
            {
                foreach (ParameterInfo parameterInfo in this.MethodInfo.GetParameters())
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

        public override DataContainer GetData(MemberInfo data)
        {
            this.MethodInfo = (MethodBase)data;
            string returnType = string.Empty;
            var isExtension = false;
            if (data is MethodInfo)
            {
                var method = ((MethodInfo) MethodInfo);
                returnType = ConvertTypeNameToString(method.ReturnType);
                isExtension = (method.GetBaseDefinition().DeclaringType == method.DeclaringType) &&
                                  MethodInfo.IsDefined(typeof(ExtensionAttribute));
            }

            return new MethodData(MethodInfo.Name, GetAccessModifier(),
                returnType, GetParameters(), GetModifiers(), isExtension);
        }
    }

    public class MethodData : DataContainer
    {
        public string ReturnType { get; private set; }

        public bool IsExtension{ get; private set; }

        public Dictionary<string, string> Parameters { get; private set; }

        public MethodData(string name, string accessModifier, string returnType, Dictionary<string, string> parameters, Modifiers methodModifiers, bool isExtension) : base(name, accessModifier, methodModifiers)
        {
            this.IsExtension = isExtension;
            this.ReturnType = returnType;
            this.Parameters = parameters;
            this.ContainerDeclaration = this.ToString();
        }

        protected override string ConvertModifierToString()
        {
            string modifiers = string.Empty;
            if ((this.Modifiers & Modifiers.Sealed) != 0) modifiers = modifiers + "sealed ";
            if ((this.Modifiers & Modifiers.Abstract) != 0) modifiers = modifiers + "abstract";
            if ((this.Modifiers & Modifiers.Virtual) != 0) modifiers = modifiers + "virtual";
            if ((this.Modifiers & Modifiers.Static) != 0) modifiers = modifiers + "static";
            return modifiers;
        }

        public sealed override string ToString()
        {
            string res = string.Empty;
            res += this.AccessModifier;
            res += " " + ConvertModifierToString();
            res += " " + this.ReturnType;
            res += " " + this.Name;
            res += "(";
            foreach (var pair in this.Parameters)
            {
                res += pair.Value + " " + pair.Key + ", ";
            }

            if (this.Parameters.Count > 0) res = res.Remove(res.Length - 2, 2);
            res += ")";
            if (this.IsExtension) res += "(extension method)";

            return res;
        }
    }
}
