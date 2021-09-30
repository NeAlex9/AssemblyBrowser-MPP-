using System.Reflection;
using System.Text;

namespace AssemblyBrowserLib.DataHolders.TypesMembers
{
    public class FieldProcessor : MemberProcessor
    {
        public FieldInfo FieldInfo { get; private set; }

        protected override string GetAccessModifier()
        {
            if (FieldInfo.IsPrivate) return "private";
            if (FieldInfo.IsPublic) return "public";
            if (FieldInfo.IsAssembly) return "internal";
            if (FieldInfo.IsFamilyAndAssembly) return "private protected";
            return "protected internal";
        }

        protected override Modifiers GetModifiers()
        {
            Modifiers modifier = (Modifiers)0;
            if (FieldInfo.IsStatic) modifier |= Modifiers.Static;
            if (FieldInfo.IsInitOnly) modifier |= Modifiers.Readonly;

            return modifier;
        }

        public override DataContainer GetData(MemberInfo data)
        {
            this.FieldInfo = (FieldInfo)data;
            return new FieldData(FieldInfo.Name, GetAccessModifier(), ConvertTypeNameToString(FieldInfo.FieldType), GetModifiers());
        }
    }

    public class FieldData : DataContainer
    {
        public string FieldType { get; private set; }

        public FieldData(string name, string accessModifier, string fieldType, Modifiers methodModifiers) : base(name, accessModifier, methodModifiers)
        {
            this.FieldType = fieldType;
            this.ContainerDeclaration = this.ToString();
        }

        protected override string ConvertModifierToString()
        {
            string modifiers = string.Empty;
            if ((this.Modifiers & Modifiers.Static) != 0) modifiers = modifiers + "static ";
            if ((this.Modifiers & Modifiers.Readonly) != 0) modifiers = modifiers + "readonly";
            return modifiers;
        }

        public sealed override string ToString()
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
