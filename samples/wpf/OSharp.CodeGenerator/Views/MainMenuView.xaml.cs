// -----------------------------------------------------------------------
//  <copyright file="MainMenuView.xaml.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-05-06 2:03</last-date>
// -----------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using MahApps.Metro.Controls;


namespace OSharp.CodeGenerator.Views
{
    /// <summary>
    /// MainMenuView.xaml 的交互逻辑
    /// </summary>
    public partial class MainMenuView
    {
        public MainMenuView()
        {
            InitializeComponent();
        }

        private void MenuButton_OnInitialized(object sender, EventArgs e)
        {
            if (!(sender is Button))
            {
                return;
            }

            Button btn = (Button)sender;
            btn.ContextMenu = null;
        }

        private void MenuButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button))
            {
                return;
            }
            Button btn = (Button)sender;
            string name = btn.Name.Replace("Btn", "Menu");
            if (btn.FindName(name) is ContextMenu menu)
            {
                menu.PlacementTarget = btn;
                menu.IsOpen = true;
            }
        }
    }
}
