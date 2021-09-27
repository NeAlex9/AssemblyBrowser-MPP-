using System;
using System.Collections.Generic;
using System.Reflection;
using AssemblyBrowserLib;
using NUnit;
using NUnit.Framework;

namespace AssemblyBrowser.Test
{
    [TestFixture]
    public class UnitTest
    {
        private AssemblyBrowserLib.AssemblyBrowser _assemblyBrowser;

        private readonly string Path = @"B:\Visual Studio programs\example\example\bin\Debug\example.dll";

        public UnitTest()
        {
            _assemblyBrowser = new AssemblyBrowserLib.AssemblyBrowser();
        }

        private FieldData GetField(string path, int index)
        {
            var data = _assemblyBrowser.GetAssemblyData(
                path);
            return (FieldData)data[0].Types[0].Members[index];
        }

        private PropertyData GetProperty(string path, int index)
        {
            var data = _assemblyBrowser.GetAssemblyData(
                path);
            return (PropertyData)data[0].Types[0].Members[index];
        }

        private MethodData GetMethod(string path, int index)
        {
            var data = _assemblyBrowser.GetAssemblyData(
                path);
            return (MethodData)data[0].Types[0].Members[index];
        }

        private TypeData GetType(string path, int index)
        {
            var data = _assemblyBrowser.GetAssemblyData(
                path);
            return (TypeData)data[0].Types[index];
        }

        private NamespaceData GetNamespace(string path, int index)
        {
            var data = _assemblyBrowser.GetAssemblyData(
                path);
            return (NamespaceData)data[index];
        }

        [Test]
        public void GetAssemblyData_FirstFieldName_ReturnCorrectName()
        {
            var field = GetField(Path, 10);
            string name = field.Name;
            Assert.That("lst", Is.EqualTo(name));
        }

        [Test]
        public void GetAssemblyData_FirstFieldType_ReturnCorrectType()
        {
            var field = GetField(Path, 10);
            var type = field.FieldType;
            Assert.That("List<Int32>", Is.EqualTo(type));
        }

        [Test]
        public void GetAssemblyData_FirstFieldAccessModifier_ReturnCorrectResult()
        {
            var field = GetField(Path, 10);
            var accessModifier = field.AccessModifier;
            Assert.That("public", Is.EqualTo(accessModifier));
        }

        [Test]
        public void GetAssemblyData_FirstPropertyName_ReturnCorrectResult()
        {
            var property = GetProperty(Path, 12);
            var name = property.Name;
            Assert.That("Name", Is.EqualTo(name));
        }

        [Test]
        public void GetAssemblyData_FirstPropertyType_ReturnCorrectResult()
        {
            var property = GetProperty(Path, 12);
            var name = property.PropertyType;
            Assert.That("List<Int32>", Is.EqualTo(name));
        }

        [Test]
        public void GetAssemblyData_FirstPropertyModifier_ReturnCorrectResult()
        {
            var property = GetProperty(Path, 12);
            var access = property.AccessModifier;
            Assert.That("public", Is.EqualTo(access));
        }

        [Test]
        public void GetAssemblyData_FirstMethodName_ReturnCorrectResult()
        {
            var method = GetMethod(Path, 0);
            var name = method.Name;
            Assert.That("Method", Is.EqualTo(name));
        }

        [Test]
        public void GetAssemblyData_FirstMethodModifier_ReturnCorrectResult()
        {
            var method = GetMethod(Path, 0);
            var access = method.AccessModifier;
            Assert.That("private", Is.EqualTo(access));
        }

        [Test]
        public void GetAssemblyData_FirstMethodReturnValue_ReturnCorrectResult()
        {
            var method = GetMethod(Path, 0);
            var returnValue = method.ReturnType;
            Assert.That("Int32", Is.EqualTo(returnValue));
        }

        [Test]
        public void GetAssemblyData_FirstMethodParameters_ReturnCorrectResult()
        {
            var method = GetMethod(Path, 0);
            var parameters = method.Parameters;
            var actual = new Dictionary<string, string>();
            actual.Add("a", "Int32");
            actual.Add("b", "Int32[]");
            Assert.That(actual, Is.EqualTo(parameters));
        }

        [Test]
        public void GetAssemblyData_FirstTypeName_ReturnCorrectResult()
        {
            var type = GetType(Path, 0);
            var name = type.Name;
            Assert.That("NoNamespaceClass1", Is.EqualTo(name));
        }

        [Test]
        public void GetAssemblyData_FirstTypeAccessor_ReturnCorrectResult()
        {
            var type = GetType(Path, 0);
            var accessor = type.AccessModifier;
            Assert.That("public", Is.EqualTo(accessor));
        }

        [Test]
        public void GetAssemblyData_FirstTypesType_ReturnCorrectResult()
        {
            var type = GetType(Path, 0);
            var typeName = type.TypeName;
            Assert.That("class", Is.EqualTo(typeName));
        }

        [Test]
        public void GetAssemblyData_FirstNamespace_ReturnCorrectResult()
        {
            var namespaceData = GetNamespace(Path, 0);
            var name = namespaceData.Name;
            Assert.That("No namespace", Is.EqualTo(name));
        }

        [Test]
        public void GetAssemblyData_CountOfNamespaced_ReturnCorrectResult()
        {
            var data = _assemblyBrowser.GetAssemblyData(Path);
            var count = data.Count;
            Assert.That(2, Is.EqualTo(count));
        }

        [Test]
        public void GetAssemblyData_CountOfTypesInN1_ReturnCorrectResult()
        {
            var namespaceData = GetNamespace(Path, 0);
            var count = namespaceData.Types.Count;
            Assert.That(1, Is.EqualTo(count));
        }

        [Test]
        public void GetAssemblyData_CountOfMembersInЕ1_ReturnCorrectResult()
        {
            var type = GetType(Path, 0);
            var count = type.Members.Count;
            Assert.That(13, Is.EqualTo(count));
        }
    }
}
