using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib
{
    public class MethodData : TypesMember
    {
        public string ReturnType{ get; private set; }

        public Dictionary<string, string> Parameters { get; private set; } 

        public bool IsStatic{ get; private set; }
        public bool IsSealed {get; private set; }

        // Virtual abstract override

        public MethodData(string name, string accessModifier,  string returnType, Dictionary<string, string> parameters, bool isStatic, bool isSealed) : base(name, accessModifier)
        {
            this.ReturnType = returnType;
            this.Parameters = parameters;
            this.IsStatic = isStatic;
            this.IsSealed = isSealed;
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            if (this.IsStatic) res.Append(" static");
            if (this.IsSealed) res.Append(" sealed");
            res.Append(" " + this.ReturnType);

            return res.ToString();
        }
    }
}
