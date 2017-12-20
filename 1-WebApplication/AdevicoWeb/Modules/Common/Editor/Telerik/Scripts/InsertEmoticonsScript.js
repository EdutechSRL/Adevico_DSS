Telerik.Web.UI.Editor.CommandList["#commandname#"] = function (commandName, editor, args) {
    var myCallbackFunction = function (sender, args) {
        editor.pasteHtml(String.format("<img src='{0}' border='0' align='middle' alt='emoticon' /> ", args.image));
    }

    editor.showExternalDialog(
            "#dialogurl#",
            {},
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