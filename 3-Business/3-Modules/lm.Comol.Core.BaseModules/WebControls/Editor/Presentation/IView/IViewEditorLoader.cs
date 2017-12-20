using System;
using System.Collections.Generic;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.BaseModules.Editor
{
    public interface IViewEditorLoader : IViewBaseEditor
    {
        String ItemsToLink { get; set; }
        String SmartTags { get; set; }
     
        ToolbarType Toolbar { get; set; }

        String HTML { get; set; }
        String LoaderCssClass { get; set; }
        EditorType CurrentType { get; set; }
        Boolean AllAvailableFontnames { get; set; }
        String ModuleCode { get; set; }
        Boolean AutoInitialize { get; set; }
     
        void InitializeControl();
        void InitializeControl(EditorType type);
        void InitializeControl(String moduleCode);
        void InitializeControl(String moduleCode,String section);
        void InitializeControl(String moduleCode, EditorType type);
        void InitializeControl(String moduleCode, String section, EditorType type);

        void LoadEditor(EditorType type, EditorInitializer settings);
    }
}