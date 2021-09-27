using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib
{
    public class PropertyData : DataContainer
    {
        public string PropertyType { get; private set; }

        public MethodInfo[] Accessors { get; private set; }

        public PropertyData(string name, string accessModifier, string propertyType, Modifiers modifiers, MethodInfo[] accessors) : base(
            name, accessModifier, modifiers)
        {
            this.PropertyType = propertyType;
            this.Accessors = accessors;
            this.ContainerDeclaration = ToString();
        }

        protected override string ConvertModifierToString()
        {
            string modifiers = string.Empty;
            if ((this.Modifiers & Modifiers.Sealed) != 0) modifiers = modifiers + "sealed ";
            if ((this.Modifiers & Modifiers.Abstract) != 0) modifiers = modifiers + "abstract ";
            if ((this.Modifiers & Modifiers.Virtual) != 0) modifiers = modifiers + "virtual ";
            if ((this.Modifiers & Modifiers.Static) != 0) modifiers = modifiers + "static ";
            return modifiers;
        }

        public sealed override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.Append(this.AccessModifier + " ");
            res.Append(ConvertModifierToString());
            res.Append(this.PropertyType + " ");
            res.Append(this.Name);
            res.Append(" { ");
            string acc = string.Empty;
            foreach (var accessor in this.Accessors)
            {
                if (accessor.IsSpecialName)
                {
                    
                    if (accessor.IsPrivate) acc += "private ";
                    acc += accessor.Name;
                    acc += ", ";
                }
            }

            if (acc.Length > 0) acc = acc.Remove(acc.Length - 2, 2);
            res.Append(acc);
            res.Append(" } ");
            return res.ToString();
        }

    }
}
