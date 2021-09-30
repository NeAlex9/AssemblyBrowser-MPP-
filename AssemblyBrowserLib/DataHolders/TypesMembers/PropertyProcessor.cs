using System.Reflection;
using System.Text;

namespace AssemblyBrowserLib.DataHolders.TypesMembers
{
    public class PropertyProcessor : MemberProcessor
    {
        public PropertyInfo PropertyInfo{ get; private set; }

        protected override string GetAccessModifier()
        {
            var accessor = PropertyInfo.GetAccessors(true)[0];
            if (accessor.IsPrivate) return "private";
            if (accessor.IsPublic) return "public";
            if (accessor.IsAssembly) return "internal";
            if (accessor.IsFamilyAndAssembly) return "private protected";

            return "protected internal";
        }

        protected override Modifiers GetModifiers()
        {
            var accessor = PropertyInfo.GetAccessors(true)[0];
            Modifiers modifier = (Modifiers)0;
            if (accessor.IsAbstract) modifier |= Modifiers.Abstract;
            else if (accessor.IsVirtual) modifier |= Modifiers.Virtual;
            if (accessor.IsStatic) modifier |= Modifiers.Static;

            return modifier;
        }

        public override DataContainer GetData(MemberInfo data)
        {
            PropertyInfo = (PropertyInfo)data;
            return new PropertyData(PropertyInfo.Name, GetAccessModifier(),
                ConvertTypeNameToString(PropertyInfo.PropertyType), GetModifiers(), PropertyInfo.GetAccessors(true));
        }
    }

    public class PropertyData : DataContainer
    {
        public string PropertyType { get; private set; }

        public MethodInfo[] Accessors { get; private set; }

        public PropertyData(string name, string accessModifier, string propertyType, Modifiers modifiers, MethodInfo[] accessors) : base(
            name, accessModifier, modifiers)
        {
            this.PropertyType = propertyType;
            this.Accessors = accessors;
            this.ContainerDeclaration = ToString();
        }

        protected override string ConvertModifierToString()
        {
            string modifiers = string.Empty;
            if ((this.Modifiers & Modifiers.Sealed) != 0) modifiers = modifiers + "sealed ";
            if ((this.Modifiers & Modifiers.Abstract) != 0) modifiers = modifiers + "abstract ";
            if ((this.Modifiers & Modifiers.Virtual) != 0) modifiers = modifiers + "virtual ";
            if ((this.Modifiers & Modifiers.Static) != 0) modifiers = modifiers + "static ";
            return modifiers;
        }

        public sealed override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.Append(this.AccessModifier + " ");
            res.Append(ConvertModifierToString());
            res.Append(this.PropertyType + " ");
            res.Append(this.Name);
            res.Append(" { ");
            string acc = string.Empty;
            foreach (var accessor in this.Accessors)
            {
                if (accessor.IsSpecialName)
                {
                    
                    if (accessor.IsPrivate) acc += "private ";
                    acc += accessor.Name;
                    acc += ", ";
                }
            }

            if (acc.Length > 0) acc = acc.Remove(acc.Length - 2, 2);
            res.Append(acc);
            res.Append(" } ");
            return res.ToString();
        }

    }
}

