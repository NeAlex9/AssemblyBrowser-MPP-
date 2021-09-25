using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib
{
    public class NamespaceData
    {
        public string Name { get; private set; }
        public List<TypeData> Types { get; private set; }

        public NamespaceData(string name, List<TypeData> types)
        {
            this.Name = name;
            this.Types = types;
        }
    }
}
