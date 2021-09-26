using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyBrowserLib;

namespace AssemblyBrowser
{
    class Program
    {
        static void Main(string[] args)
        {
            var assemblyBrowser = new AssemblyBrowserLib.AssemblyBrowser();
            var x = assemblyBrowser.GetAssemblyData(@"B:\BSUIR\3 course\5 sem\СПП\lab\Tracer(lab1)\Tracer(lab1)\bin\Debug\NTracer.dll");
        }
    }

    // модификаторы доступа работают у методов  в свойствах полях классах 
    // модификаторы тоже работают но не у классов(вроде исправил)
    // методы свойства и поля в классах работают
    // !! проверить генерики и делегаты и события
}
