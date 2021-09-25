using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib
{
    public class TypeData // struct, enum, class, interface
    {
        public string Type { get; private set; }
        public string Name { get; private set; }
        public string AccessModifier{ get; private set; }

        // abstract, sealed

        public List<TypesMember> Members{ get; private set; }

        public TypeData(string type, string name, string accessModifier, List<TypesMember> members)
        {
            this.Type = type;
            this.Name = name;
            this.AccessModifier = accessModifier;
            this.Members = members;
        }
    }
}
