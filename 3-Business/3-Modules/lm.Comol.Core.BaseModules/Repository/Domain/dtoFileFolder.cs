using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Repository
{
    [Serializable]
    public class dtoFileFolder
    {
        public long Id {get;set;}
        public String Name {get;set;}
        public Boolean isVisible {get;set;}
        public List<dtoFileFolder> SubFolders {get;set;}
        public List<dtoGenericFile> Files {get;set;}
        public Boolean Selected {get;set;}
        public Boolean Selectable {get;set;}
        public Boolean HasFiles
        {
            get {
                return Files.Any() || SubFolders.Where(f => f.HasFiles).Any();
            }
        
        }

        public dtoFileFolder(){
            Selected = false;
            Files = new List<dtoGenericFile>();
            SubFolders = new List<dtoFileFolder>();
        }

        //Sub New(ByVal oId As Long, ByVal oName As String, ByVal oVisible As Boolean)
        //    Me._ID = oId
        //    Me._Name = oName
        //    Me._isVisible = oVisible
        //    Me._Selected = False
        //    Me._Files = New List(Of dtoGenericFile)
        //    Me._Folder = New List(Of dtoFileFolder)
        //End Sub
        //Sub New(ByVal oId As Long, ByVal oName As String, ByVal oVisible As Boolean, ByVal oSelected As Boolean)
        //    Me._ID = oId
        //    Me._Name = oName
        //    Me._isVisible = oVisible
        //    Me._Selected = oSelected
        //    Me._Files = New List(Of dtoGenericFile)
        //    Me._Folder = New List(Of dtoFileFolder)
        //End Sub
    }

    [Serializable]
    public class dtoGenericFile
    {
        public long Id {get;set;}
        public String Name {get;set;}
        public String Extension {get;set;}
        public String DisplayName {get{return Name+ Extension;}}
        public Boolean isVisible {get;set;}
        public dtoFileFolder Folder {get;set;}
        public Boolean Selected {get;set;}
        public Boolean Selectable {get;set;}

        public dtoGenericFile(){
            Selected = false;
            Selectable = true;
        }
    }
}


    //    Sub New()
    //        _Selected = False
    //        _Selectable = True
    //    End Sub
    //    Sub New(ByVal oId As Long, ByVal oName As String, ByVal oExtension As String, ByVal oVisible As Boolean)
    //        Me._ID = oId
    //        Me._Name = oName
    //        Me._Extension = oExtension
    //        Me._isVisible = oVisible
    //        _Selected = False
    //        _Selectable = True
    //    End Sub
    //    Sub New(ByVal oId As Long, ByVal oName As String, ByVal oExtension As String, ByVal oVisible As Boolean, ByVal oSelected As Boolean)
    //        Me._ID = oId
    //        Me._Name = oName
    //        Me._Extension = oExtension
    //        Me._isVisible = oVisible
    //        _Selected = oSelected
    //        _Selectable = True
    //    End Sub
    //    'Sub New(ByVal oId As Long, ByVal oName As String, ByVal oVisible As Boolean, ByVal oFolder As dtoFileFolder)
    //    '    Me._ID = oId
    //    '    Me._Name = oName
    //    '    Me._isVisible = oVisible
    //    '    Me._Folder = oFolder
    //    'End Sub

    //End Class
        
