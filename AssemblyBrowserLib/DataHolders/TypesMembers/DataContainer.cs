using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib
{
    public abstract class DataContainer
    {
        public string Name{ get; private set; }
        public string AccessModifier{ get; private set; }
        public Modifiers Modifiers{ get; private set; }

        protected abstract string ConvertModifierToString();

        protected DataContainer(string name, string accessModifier, Modifiers modifiers)
        {
            this.Name = name;
            this.AccessModifier = accessModifier;
            this.Modifiers = modifiers;
        }

        public abstract override string ToString();
    }
}
