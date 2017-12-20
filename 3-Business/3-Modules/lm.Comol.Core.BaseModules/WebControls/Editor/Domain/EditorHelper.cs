using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Web.UI;

namespace lm.Comol.Core.BaseModules.Editor.TelerikHelper
{
    public class EditorHelper
    {
         
	/// <summary>
	/// Rimuove un bottone
	/// </summary>
	/// <param name="RDE">L'editor da cui eliminare i bottoni</param>
	/// <param name="ButtonName">Il nome del bottone. Usare ToolsName.ToString()</param>
	/// <param name="GroupName">
	/// ""           -->  Toglie dai gruppi senza nome
	/// "ALL"/null   -->  Toglie da TUTTI i gruppi
	/// "GruopName"  -->  Toglie dal gruppo specificato
	/// </param>
	/// <remarks></remarks>

	public static void RemoveButton(ref Telerik.Web.UI.RadEditor editor, string buttonName, string groupName = "")
	{
		//If Not ToolsLoaded Then
        editor.EnsureToolsFileLoaded();
		//ToolsLoaded = True
		//End If

        if ((string.IsNullOrEmpty(groupName)))
            groupName = GroupName._ALL.ToString();

        else if (groupName == GroupName._VOID.ToString())
            groupName = "";

		List<Telerik.Web.UI.EditorToolGroup> DelGroups = new List<Telerik.Web.UI.EditorToolGroup>();

        foreach (Telerik.Web.UI.EditorToolGroup @group in editor.Tools)
        {
            if ((@group != null) && (groupName == "_ALL" || @group.Tag == groupName))
            {
				Telerik.Web.UI.EditorTool tool = default(Telerik.Web.UI.EditorTool);
				try {
                    tool = @group.FindTool(buttonName);
				} catch (Exception ex) {
				}

				if ((tool != null)) {
					try {
						@group.Tools.Remove(tool);

					} catch (Exception ex) {
					}
				}

				if (@group.Tools.Count <= 0) {
					DelGroups.Add(@group);
				}
			}

		}

		if ((DelGroups != null) && DelGroups.Count > 0) {
			foreach (Telerik.Web.UI.EditorToolGroup @group in DelGroups) {
                if ((@group != null) && @group.Tag == groupName)
                {
					editor.Tools.Remove(@group);
				}
			}
		}
	}

	/// <summary>
	/// Rimuove un gruppo di bottoni
	/// </summary>
	/// <param name="RDE">L'editor da cui eliminare i bottoni</param>
	/// <param name="GroupName">
	/// ""           -->  Toglie dai gruppi senza nome
	/// "_ALL"/null   -->  Toglie TUTTI i gruppi!!!
	/// "GruopName"  -->  Toglie dal gruppo specificato
	/// </param>
	/// <remarks>    
	/// </remarks>

    public static void RemoveGroup(ref Telerik.Web.UI.RadEditor editor, string groupName)
	{
        if (string.IsNullOrEmpty(groupName))
            groupName = "";
        else if (groupName == GroupName._VOID.ToString())
            groupName = "";

		//If Not ToolsLoaded Then
        editor.EnsureToolsFileLoaded();
		//ToolsLoaded = True
		//End If

		List<Telerik.Web.UI.EditorToolGroup> DelGroups = new List<Telerik.Web.UI.EditorToolGroup>();

        foreach (Telerik.Web.UI.EditorToolGroup @group in editor.Tools)
        {
            if ((@group != null) && (@group.Tag == groupName | groupName == GroupName._ALL.ToString()))
				DelGroups.Add(@group);
		}

		if ((DelGroups != null) && DelGroups.Count > 0) {
			foreach (Telerik.Web.UI.EditorToolGroup @group in DelGroups) {
                if ((@group != null) && @group.Tag == groupName)
                    editor.Tools.Remove(@group);
			}
		}

	}

	/// <summary>
	/// Aggiunge un bottone a specifici gruppi
	/// </summary>
	/// <param name="RDE">L'editor a cui aggiungere il bottone</param>
	/// <param name="ButtonName">Il nome del Tool (usare ToolsName.ToString())</param>
	/// <param name="GroupName"></param>
	/// <remarks>
	/// Se il gruppo non esite, ne verrà creato uno nuovo!
	/// </remarks>

    public static void AddButton(ref Telerik.Web.UI.RadEditor editor, string buttonName, string groupName)
	{
        editor.EnsureToolsFileLoaded();
		Boolean found = false;
        if (!string.IsNullOrEmpty(groupName))
        {
            foreach (Telerik.Web.UI.EditorToolGroup @group in editor.Tools)
            {
                if ((@group != null) && @group.Tag == groupName)
                {
					found = true;
                    Telerik.Web.UI.EditorTool tool = null;
					try {
                        tool = @group.FindTool(buttonName);
					} catch (Exception ex) {
					}

					if ((tool == null)) {
						tool = new Telerik.Web.UI.EditorTool();
                        tool.Name = buttonName;
						@group.Tools.Add(tool);
					}
				}
			}
		}

		if (!found) {
			Telerik.Web.UI.EditorToolGroup NewGroup = new Telerik.Web.UI.EditorToolGroup();
			editor.Tools.Add(NewGroup);
			Telerik.Web.UI.EditorTool Tool = new Telerik.Web.UI.EditorTool();
            Tool.Name = buttonName;
			NewGroup.Tools.Add(Tool);
		}
	}

	/// <summary>
	/// Rimuove il bottone
	/// </summary>
	/// <param name="RDE">L'editor da cui togliere il bottone</param>
	/// <param name="ButtonName">Il nome del bottone</param>
	/// <param name="GroupName">Il gruppo da cui eleminare il bottone</param>
	/// <remarks></remarks>

	public static void RemoveButton(ref Telerik.Web.UI.RadEditor editor, ToolsName buttonName, GroupName groupName)
	{
		//If Not ToolsLoaded Then
        editor.EnsureToolsFileLoaded();
		//ToolsLoaded = True
		//End If

		List<Telerik.Web.UI.EditorToolGroup> DelGroups = new List<Telerik.Web.UI.EditorToolGroup>();

        foreach (Telerik.Web.UI.EditorToolGroup @group in editor.Tools)
        {

            if ((@group != null) && ((groupName == GroupName._ALL) || @group.Tag == groupName.ToString() || (string.IsNullOrEmpty(@group.Tag) && groupName == GroupName._VOID)))
            {
                Telerik.Web.UI.EditorTool tool = null;
				try {
                    tool = @group.FindTool(buttonName.ToString());
				} catch (Exception ex) {
				}

				if ((tool != null)) {
					try {
						@group.Tools.Remove(tool);

					} catch (Exception ex) {
					}
				}

				if (@group.Tools.Count <= 0)
					DelGroups.Add(@group);
			}
		}

		if ((DelGroups != null) && DelGroups.Count > 0) {
			foreach (Telerik.Web.UI.EditorToolGroup @group in DelGroups) {
				if ((@group != null))
                    editor.Tools.Remove(@group);
			}
		}
	}

	/// <summary>
	/// Rimuove un gruppo di bottoni dall'editor
	/// </summary>
	/// <param name="RDE">L'editor da cui togliere i bottoni</param>
	/// <param name="GroupName">Il nome del gruppo da togliere</param>
	/// <remarks></remarks>

    public static void RemoveGroup(ref Telerik.Web.UI.RadEditor editor, GroupName groupName)
	{
		//If Not ToolsLoaded Then
        editor.EnsureToolsFileLoaded();
		//ToolsLoaded = True
		//End If

		List<Telerik.Web.UI.EditorToolGroup> DelGroups = new List<Telerik.Web.UI.EditorToolGroup>();

        foreach (Telerik.Web.UI.EditorToolGroup @group in editor.Tools)
        {
            if ((@group != null) && @group.Tag == groupName.ToString())
				DelGroups.Add(@group);
		}

		if ((DelGroups != null) && DelGroups.Count > 0) {
			foreach (Telerik.Web.UI.EditorToolGroup @group in DelGroups) {
				if ((@group != null))
                    editor.Tools.Remove(@group);
			}
		}
	}

	/// <summary>
	/// Aggiunge un bottone
	/// </summary>
	/// <param name="RDE">L'editor a cui aggiungere il bottone</param>
	/// <param name="ButtonName">Il nome del bottone</param>
	/// <param name="GroupName">Il nome del gruppo a cui aggiungere il bottone</param>
	/// <remarks>Se il gruppo non c'è, verra ricreato</remarks>

    public static void AddButton(ref Telerik.Web.UI.RadEditor editor, ToolsName buttonName, GroupName groupName)
	{
		//If Not ToolsLoaded Then
        editor.EnsureToolsFileLoaded();
		//ToolsLoaded = True
		//End If

		bool found = false;

        foreach (Telerik.Web.UI.EditorToolGroup @group in editor.Tools)
        {

            if ((@group != null) && ((groupName == GroupName._ALL) || @group.Tag == groupName.ToString() || (string.IsNullOrEmpty(@group.Tag) && groupName == GroupName._VOID)))
            {
				found = true;
                Telerik.Web.UI.EditorTool tool = null;
				try {
                    tool = @group.FindTool(buttonName.ToString());
				} catch (Exception ex) {
				}

				if ((tool == null)) {
					tool = new Telerik.Web.UI.EditorTool();
                    tool.Name = buttonName.ToString();
					//tool.ShortCut = "CTRL+B"
					@group.Tools.Add(tool);
				}
			}
		}

		if (!found) {
            Telerik.Web.UI.EditorToolGroup NewGroup = new Telerik.Web.UI.EditorToolGroup();
			editor.Tools.Add(NewGroup);
            Telerik.Web.UI.EditorTool Tool = new Telerik.Web.UI.EditorTool();
            Tool.Name = buttonName.ToString();
			NewGroup.Tools.Add(Tool);
		}
	}

    public static Telerik.Web.UI.EditorTool GetToolButton(ref Telerik.Web.UI.RadEditor editor, ToolsName buttonName)
    {
        Telerik.Web.UI.EditorTool fButton = null;
        //If Not ToolsLoaded Then
        editor.EnsureToolsFileLoaded();
        //ToolsLoaded = True
        //End If

        foreach (Telerik.Web.UI.EditorToolGroup @group in editor.Tools)
        {
            if (@group != null)
            {
                if (@group.Contains(buttonName.ToString()))
                    return @group.FindTool(buttonName.ToString());
            }
        }
        return fButton;
    }

    }

    [Serializable]
    public enum GroupName{
        _ALL,
        _VOID,
        StandardTools,
        GrpTable,
        FormattingDef,
        FormattingFonts,
        FormattingParagraph,
        InsertAdvanced,
        InsertStandard,
        About
    }

    [Serializable]
    public enum  ToolsName{
        AboutDialog,
        AbsolutePosition,
        AjaxSpellCheck,
        ApplyClass,
        BackColor,
        Bold,
        ConvertToLower,
        ConvertToUpper,
        Copy,
        Cut,
        DocumentManager,
        FindAndReplace,
        FlashManager,
        FontName,
        FontSize,
        ForeColor,
        FormatBlock,
        FormatCodeBlock,
        FormatStripper,
        Help,
        ImageManager,
        ImageMapDialog,
        Indent,
        InsertCustomLink,
        InsertDate,
        InsertFormElement,
        InsertHorizontalRule,
        InsertImage,
        InsertLink,
        InsertOrderedList,
        InsertParagraph,
        InsertSnippet,
        InsertSymbol,
        InsertTable,
        InsertTime,
        InsertUnorderedList,
        Italic,
        JustifyCenter,
        JustifyFull,
        JustifyLeft,
        JustifyNone,
        JustifyRight,
        LinkManager,
        MediaManager,
        ModuleManager,
        Outdent,
        Paste,
        PasteAsHtml,
        PasteFromWord,
        PasteFromWordNoFontsNoSizes,
        PasteHtml,
        PastePlainText,
        PasteStrip,
        Print,
        RealFontSize,
        Redo,
        StrikeThrough,
        StyleBuilder,
        Subscript,
        Superscript,
        TemplateManager,
        ToggleDocking,
        ToggleScreenMode,
        ToggleTableBorder,
        TrackChangesDialog,
        Underline,
        Undo,
        Unlink,
        XhtmlValidator,
        Zoom,
        InsertRowAbove,
        InsertRowBelow,
        DeleteRow,
        InsertColumnLeft,
        InsertColumnRight,
        DeleteColumn,
        MergeColumns,
        MergeRows,
        SplitCell,
        SplitCellHorizontal,
        DeleteCell,
        SetCellProperties,
        SetTableProperties,
        InsertEmoticons,
        InsertYoutube,
        InsertLatex,
        InsertWiki,
        InsertFaq,
        InsertGlossary,
        InsertRepositoryItem
    }
}


//'ConvertToUpper
//'ConvertToLower
//'RealFontSize
//'ToggleScreenMode
//'ToggleTableBorder
//'Zoom
//'ModuleManager
//'ToggleDocking
//'FindAndReplace
//'Print
//'AjaxSpellCheck
//'Cut
//'Copy
//'Paste
//'PasteStrip
//'PasteFromWord
//'PasteFromWordNoFontsNoSizes
//'PastePlainText
//'PasteHtml
//'PasteAsHtml
//'Undo
//'Redo
//'FormatStripper
//'Help
//'AboutDialog
//'XhtmlValidator
//'TrackChangesDialog
//'StyleBuilder
//'ImageManager
//'InsertImage
//'ImageMapDialog
//'AbsolutePosition
//'InsertTable
//'ToggleTableBorder
//'InsertSnippet
//'InsertFormElement
//'InsertDate
//'InsertTime
//'FlashManager
//'MediaManager
//'DocumentManager
//'LinkManager
//'InsertLink
//'Unlink
//'InsertSymbol
//'InsertCustomLink
//'TemplateManager
//'InsertParagraph
//'FormatBlock
//'Indent
//'Outdent
//'JustifyLeft
//'JustifyCenter
//'JustifyRight
//'JustifyFull
//'JustifyNone
//'InsertUnorderedList
//'InsertOrderedList
//'InsertHorizontalRule
//'Bold
//'Italic
//'Underline
//'StrikeThrough
//'Superscript
//'Subscript
//'FontName
//'FontSize
//'ForeColor
//'BackColor
//'FormatCodeBlock
//'ApplyClass
//'InsertCustomLink