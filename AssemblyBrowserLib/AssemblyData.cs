using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib
{
    public class AssemblyData
    {
        public List<NamespaceData> Namespaces{ get; private set; }

        public AssemblyData(List<NamespaceData> namespaces)
        {
            this.Namespaces = namespaces;
        }
    }
}
