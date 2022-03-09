// -----------------------------------------------------------------------
//  <copyright file="LoadFromEntitiesViewModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-02-13 2:42</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using AsmResolver.DotNet;

using OSharp.Wpf.Stylet;

using Screen = Stylet.Screen;


namespace OSharp.CodeGenerator.Views.LoadFromEntities
{
    [Singleton]
    public class LoadFromEntitiesViewModel : Screen
    {
        private readonly IDictionary<string, TypeDefinition[]> _assemblyTypesDict = new Dictionary<string, TypeDefinition[]>();

        public bool IsShow { get; set; }

        public async void LoadDll()
        {
            FileDialog dialog = new OpenFileDialog()
            {
                Title = "打开包含实体类的dll文件",
                CheckFileExists = true,
                Multiselect = false,
                Filter = "DLL文件|*.dll"
            };
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }

            string file = dialog.FileName;
            await LoadDll(file);
        }

        private Task LoadDll(string file)
        {
            ModuleDefinition moduleDef = ModuleDefinition.FromFile(file);
            TypeDefinition[] typeDefs = moduleDef.GetAllTypes().Where(m => m.FullName.Contains(".Entities.")).ToArray();
            _assemblyTypesDict[file] = typeDefs;

            return Task.FromResult(0);
        }
    }
}
