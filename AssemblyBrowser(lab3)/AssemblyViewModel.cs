using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AssemblyBrowserLib;

namespace AssemblyBrowser_lab3_
{
    public class AssemblyViewModel : INotifyPropertyChanged
    {
        public List<NamespaceData> NamespacesData{ get; private set; }

        public AssemblyBrowser AssemblyBrowser{ get; }

        public AssemblyViewModel()
        {
            this.AssemblyBrowser = new AssemblyBrowser();
            this.NamespacesData = this.AssemblyBrowser.GetAssemblyData(@"B:\BSUIR\3 course\5 sem\СПП\lab\Tracer(lab1)\Tracer(lab1)\bin\Debug\NTracer.dll");

        }

        public void LoadAssembly(string path)
        {
            this.NamespacesData = this.AssemblyBrowser.GetAssemblyData(path);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
