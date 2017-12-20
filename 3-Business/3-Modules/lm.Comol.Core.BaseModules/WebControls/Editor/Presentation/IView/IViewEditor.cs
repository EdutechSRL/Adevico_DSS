using System;
using System.Collections.Generic;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.BaseModules.Editor
{
    public interface IViewEditor : IViewBaseEditor
    {
        String OnLoadScripts { get; set; }
        String OnCommandcripts { get; set; }
        String HTML { get; set; }
        EditorType CurrentType { get; }

        void InitializeEditor(EditorInitializer settings);

        //String LoaderCssClass { get; set; }
        //void InitializeControl(String moduleCode, EditorType type);
        //void LoadEditor(EditorType type);
    }
}