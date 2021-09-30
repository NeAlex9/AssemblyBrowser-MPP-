using System.Collections.Generic;

namespace AssemblyBrowserLib.DataHolders
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
