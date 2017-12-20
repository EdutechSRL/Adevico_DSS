function LatexPopup(el,addressPopUp){
    st=el.getAttribute("title");
    window.open(addressPopUp + '?'+st,'','location=no,scrollbars=yes,toolbar=no,menubar=no,width=640,height=360,resizable=yes');
    return false;
}

function selectAllText(el)
      {
        el.select();
        //copy_clip(el.value);
      }
      
      function copy_text()
      {
        el=document.getElementById("<%= txblatex.clientID %>");
        copy_clip(el.value);
        return false;
      }
                
      function copy_clip(meintext)
        {
            if (window.clipboardData)
            {

                window.clipboardData.setData("Text", meintext);

            }
                else if (window.netscape)
                {


                    netscape.security.PrivilegeManager.enablePrivilege('UniversalXPConnect');


                    var clip = Components.classes['...@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);
                    if (!clip) return;


                    var trans = Components.classes['...@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);
                    if (!trans) return;
                        trans.addDataFlavor('text/unicode');


                    var str = new Object();
                    var len = new Object();


                    var str = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);


                    var copytext=meintext;


                    str.data=copytext;


                    trans.setTransferData("text/unicode",str,copytext.length*2);


                    var clipid=Components.interfaces.nsIClipboard;


                    if (!clip) return false;


                    clip.setData(trans,null,clipid.kGlobalClipboard);


                }
            return false;


        }