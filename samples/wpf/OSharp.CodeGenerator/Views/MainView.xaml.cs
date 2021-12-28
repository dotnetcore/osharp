using System.ComponentModel;
using System.Windows;

using MahApps.Metro.Controls.Dialogs;

using OSharp.CodeGenerator.Data;


namespace OSharp.CodeGenerator.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
            MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            Helper.Main = this;
        }

        /// <summary>引发 <see cref="E:System.Windows.Window.Closing" /> 事件。</summary>
        /// <param name="e">包含事件数据的 <see cref="T:System.ComponentModel.CancelEventArgs" />。</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            //MessageBoxResult result = MessageBox.Show("是否关闭窗口", "请选择", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if (result == MessageBoxResult.No)
            //{
            //    e.Cancel = true;
            //    return;
            //}
            
            base.OnClosing(e);
        }
    }
}
