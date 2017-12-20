Telerik.Web.UI.Editor.CommandList["#commandname#"] = function (commandName, editor, args) {

    //var elem = editor.getSelectedElement(); //returns the selected element.
    var content = editor.getSelection().getText();

    content = content.replace("[latex]", "").replace("[/latex]", "");

    var myCallbackFunction = function (sender, args) {
        editor.pasteHtml(String.format("{0}{1}{2}", args.openTag, args.text, args.closeTag));
    }

    editor.showExternalDialog("#dialogurl#",
            content,
            #dialogwidth#,
            #dialogheight#,
            myCallbackFunction,
            null,
            "#dialoghtitle#",
            true,
            Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move,
            false,
            true);
};