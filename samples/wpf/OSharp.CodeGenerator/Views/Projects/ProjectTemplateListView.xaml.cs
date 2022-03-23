using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OSharp.Wpf.Stylet;


namespace OSharp.CodeGenerator.Views.Projects
{
    /// <summary>
    /// ProjectTemplateListView.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectTemplateListView : UserControl
    {
        public ProjectTemplateListView()
        {
            InitializeComponent();
        }

        private void Locked_CheckAll(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                var list = IoC.Get<ProjectTemplateListViewModel>();
                foreach (var item in list.ProjectTemplates)
                {
                    item.IsLocked = checkBox.IsChecked ?? false;
                }
            }
        }
    }
}
