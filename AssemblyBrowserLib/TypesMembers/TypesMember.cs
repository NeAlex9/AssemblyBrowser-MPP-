using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib
{
    public abstract class TypesMember
    {
        public string Name{ get; private set; }
        public string AccessModifier{ get; private set; }

        protected TypesMember(string name, string accessModifier)
        {
            this.Name = name;
            this.AccessModifier = accessModifier;
        }

        public abstract override string ToString();
    }
}
