using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib.DataHolders.TypesMembers
{
    public abstract class MemberProcessor
    {
        protected MemberProcessor() { }

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
}
