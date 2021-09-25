using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib
{
    public class PropertyData : TypesMember
    {
        public string PropertyType { get; private set; }

        public Modifiers Modifiers { get; private set; }
        // getter setter

        public PropertyData(string name, string accessModifier, string propertyType, Modifiers modifiers) : base(
            name, accessModifier)
        {
            this.PropertyType = propertyType;
            this.Modifiers = modifiers;
        }

        private string GetModifier()
        {
            string modifiers = string.Empty;
            if ((this.Modifiers & Modifiers.Sealed) != 0) modifiers = modifiers + "sealed ";
            if ((this.Modifiers & Modifiers.Abstract) != 0) modifiers = modifiers + "abstract";
            if ((this.Modifiers & Modifiers.Virtual) != 0) modifiers = modifiers + "virtual";
            if ((this.Modifiers & Modifiers.Static) != 0) modifiers = modifiers + "static";
            return modifiers;
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.Append(this.AccessModifier);
            res.Append(GetModifier());
            res.Append(" " + this.Name);
            // var accessors = propertyInfo.GetAccessors(true);
            // foreach (var accessor in accessors)
            // {
            //     if (accessor.IsSpecialName)
            //     {
            //         result.Append(" { ");
            //         result.Append(accessor.Name);
            //         result.Append(" } ");
            //     }
            // }

            return res.ToString();
        }

    }
}
