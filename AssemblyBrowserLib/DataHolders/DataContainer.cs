using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib
{
    public abstract class DataProcessor
    {
        protected DataProcessor() { }

        protected abstract string GetAccessModifier();

        protected abstract Modifiers GetModifiers();

        protected string ConvertTypeNameToString(Type type)
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
        }

        public abstract DataContainer GetData(MemberInfo data);
    }

    public abstract class DataContainer
    {
        public string Name{ get; private set; }
        public string AccessModifier{ get; private set; }
        public Modifiers Modifiers{ get; private set; }
        public string ContainerDeclaration{ get; protected set; }

        protected abstract string ConvertModifierToString();

        protected DataContainer(string name, string accessModifier, Modifiers modifiers)
        {
            this.Name = name;
            this.AccessModifier = accessModifier;
            this.Modifiers = modifiers;
        }

        public abstract override string ToString();
    }
}
