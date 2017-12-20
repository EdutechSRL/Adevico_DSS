    Telerik.Web.UI.Editor.CommandList["#commandname#"] = function(commandName, editor, args)
    {
       var elem = editor.getSelectedElement(); //returns the selected element.
       if (elem.tagName == "A")
       {
            editor.selectElement(elem);
            argument = elem;
            argument.innerText = editor.getSelection().getText();
       }
       else
       {
            //remove links if present from the current selection - because of JS error thrown in IE
            editor.fire("Unlink");
                
            //remove Unlink command from the undo/redo list
            var commandsManager = editor.get_commandsManager();
            var commandIndex = commandsManager.getCommandsToUndo().length - 1;
            commandsManager.removeCommandAt(commandIndex);

            var content = editor.getSelectionHtml();

            var link = editor.get_document().createElement("A");

            link.innerHTML = content;
            link.innerText = editor.getSelection().getText();
            argument = link;        
        }
       
       var myCallbackFunction = function(sender, args)
       {
           editor.pasteHtml(String.format("<a href={0} target='{1}' class='{2}'>{3}</a> ", args.href, args.target, args.className, args.name))
       }
       
       editor.showExternalDialog(
            '#dialogurl#',
            argument,
            #dialogwidth#,
            #dialogheight#,
            myCallbackFunction,
            null,
            '#dialoghtitle#',
            true,
            Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move,
            false,
            true);
    };