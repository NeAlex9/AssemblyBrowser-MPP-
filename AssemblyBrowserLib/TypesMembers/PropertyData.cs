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
        public bool IsStatic { get; private set; }
        public bool IsSealed { get; private set; }

        // getter setter
        // Virtual abstract override

        public PropertyData(string name, string accessModifier, string propertyType, bool isStatic, bool isReadonly) : base(
            name, accessModifier)
        {
            this.PropertyType = propertyType;
            this.IsStatic = isStatic;
            this.IsSealed = IsSealed;
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.Append(this.AccessModifier);
            if (this.IsStatic) res.Append(" static");
            if (this.IsSealed) res.Append(" sealed");
            res.Append(" " + this.Name);
            return res.ToString();
        }

    }
}
