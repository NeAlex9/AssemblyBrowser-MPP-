using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib
{
    public class FieldData : TypesMember
    {
        public string FieldType { get; private set; }
        public bool IsStatic { get; private set; }
        public bool IsReadonly { get; private set; }

        public FieldData(string name, string accessModifier, string fieldType, bool isStatic, bool isReadonly) : base(
            name, accessModifier)
        {
            this.FieldType = fieldType;
            this.IsStatic = isStatic;
            this.IsReadonly = isReadonly;
        }

        public override string ToString()
        {
            var res = new StringBuilder();
            res.Append(this.AccessModifier);
            if (this.IsStatic) res.Append(" Static");
            if (this.IsReadonly) res.Append(" Readonly");
            res.Append(this.Name);
            return res.ToString();
        }
    }
}
