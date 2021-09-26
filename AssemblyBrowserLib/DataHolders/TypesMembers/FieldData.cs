using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib
{
    public class FieldData : DataContainer
    {
        public string FieldType { get; private set; }

        public FieldData(string name, string accessModifier, string fieldType, Modifiers methodModifiers) : base(name, accessModifier, methodModifiers)
        {
            this.FieldType = fieldType;
        }

        protected override string ConvertModifierToString()
        {
            string modifiers = string.Empty;
            if ((this.Modifiers & Modifiers.Static) != 0) modifiers = modifiers + "static ";
            if ((this.Modifiers & Modifiers.Readonly) != 0) modifiers = modifiers + "readonly";
            return modifiers;
        }

        public override string ToString()
        {
            var res = new StringBuilder();
            res.Append(this.AccessModifier + " ");
            res.Append(ConvertModifierToString() + " ");
            res.Append(this.FieldType + " ");
            res.Append(this.Name);
            return res.ToString();
        }
    }
}
