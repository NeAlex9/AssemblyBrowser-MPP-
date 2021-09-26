using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib
{
    public class TypeData : DataContainer
    {
        public List<DataContainer> Members{ get; private set; }

        protected override string ConvertModifierToString()
        {
            if ((this.Modifiers & (Modifiers.Sealed | Modifiers.Abstract)) != 0) return "sealed ";
            if ((this.Modifiers & Modifiers.Abstract) != 0) return "abstract ";
            if ((this.Modifiers & Modifiers.Sealed) != 0) return "static ";
            return "";
        }

        public TypeData(string type, string name, string accessModifier, List<DataContainer> members, Modifiers modifiers) : base(name, accessModifier, modifiers)
        {
            this.Members = members;
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
