using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib
{
    public class MethodData : DataContainer
    {
        public string ReturnType { get; private set; }

        public Dictionary<string, string> Parameters { get; private set; }

        public MethodData(string name, string accessModifier, string returnType, Dictionary<string, string> parameters, Modifiers methodModifiers) : base(name, accessModifier, methodModifiers)
        {
            this.ReturnType = returnType;
            this.Parameters = parameters;
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

        public override string ToString()
        {
            string res = string.Empty;
            res += this.AccessModifier;
            res += " " + ConvertModifierToString();
            res += " " + this.ReturnType;
            res += " " + this.Name;
            res += "(";
            foreach (var pair in this.Parameters)
            {
                res += pair.Key + " " + pair.Value + ", ";
            }

            if (this.Parameters.Count > 0) res.Remove(res.Length - 2, 2);
            res += ")";
            return res;
        }
    }

    
}
