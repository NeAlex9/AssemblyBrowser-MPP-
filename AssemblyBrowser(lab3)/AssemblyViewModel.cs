using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AssemblyBrowserLib;
using Microsoft.Win32;

namespace AssemblyBrowser_lab3_
{
    public class AssemblyViewModel : INotifyPropertyChanged
    { 
        public List<NamespaceData> NamespacesData{ get; private set; }

        private BrowsCommand _openCommand;
        public BrowsCommand OpenCommand
        {
            get
            {
                return _openCommand ??
                       (_openCommand = new BrowsCommand(obj =>
                       {
                           try
                           {
                               OpenFileDialog openFileDialog = new OpenFileDialog();
                               if (openFileDialog.ShowDialog() == true)
                               {
                                   var list = this.AssemblyBrowser.GetAssemblyData(openFileDialog.FileName);
                                   this.NamespacesData = list;
                                   OnPropertyChanged(nameof(NamespacesData));
                               }
                           }
                           catch (Exception e)
                           {
                               MessageBox.Show("failed to load assembly");
                           }
                       }) );
            }
        }

        public AssemblyBrowser AssemblyBrowser{ get; }

        public AssemblyViewModel()
        {
            this.AssemblyBrowser = new AssemblyBrowser();
            this.NamespacesData = new List<NamespaceData>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
