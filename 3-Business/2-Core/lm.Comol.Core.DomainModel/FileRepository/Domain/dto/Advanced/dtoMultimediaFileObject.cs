using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoMultimediaFileObject 
    {

        private String _extension;
        private String _displayName;
        private String _Path;
        public virtual long Id { get; set; }
        public virtual String Fullname { get; set; }
        public virtual String FileName { get; set; }
        public virtual String DisplayName { get { return _displayName; } }
        public virtual String Extension { get { return _extension; } }
        public virtual String Path { get { return _Path; } }
        public virtual List<String> Folders { get; set; }
        public virtual Boolean IsDefaultDocument { get; set; }

        public dtoMultimediaFileObject()
        {
            Folders = new List<string>();
        }
        public dtoMultimediaFileObject(liteMultimediaFileObject obj)
        {
            Id = obj.Id;
            IsDefaultDocument = obj.IsDefaultDocument;
            Folders = new List<string>();

            String name = obj.Fullname;
            if (!String.IsNullOrEmpty(name))
                name = name.Replace(obj.UniqueIdVersion.ToString() + "\\", "");
            Fullname = name;

             List<String> items = name.Split('\\').ToList();
             FileName = items.Last();
             _displayName = FileName;

            int index = _displayName.LastIndexOf(".");
            if (index > 0){
                if (index + 1 < _displayName.Length)
                {
                    _extension = _displayName.Substring(index + 1);
                }
                else
                {
                    _extension = "";
                }
                _displayName = _displayName.Remove(index);
            }
            else
                _extension = "";
            items.RemoveRange(items.Count-1,1);
            _Path = String.Join("\\", items);
            Folders = items;
        }

        public dtoMultimediaFileObject(MultimediaFileObject obj)
        {
            Id = obj.Id;
            IsDefaultDocument = obj.IsDefaultDocument;
            Folders = new List<string>();

            String name = obj.Fullname;
            if (!String.IsNullOrEmpty(name))
                name = name.Replace(obj.UniqueIdVersion.ToString() + "\\", "");
            Fullname = name;

             List<String> items = name.Split('\\').ToList();
             FileName = items.Last();
             _displayName = FileName;

            int index = _displayName.LastIndexOf(".");
            if (index > 0){
                if (index + 1 < _displayName.Length)
                {
                    _extension = _displayName.Substring(index + 1);
                }
                else
                {
                    _extension = "";
                }
                _displayName = _displayName.Remove(index);
            }
            else
                _extension = "";
            items.RemoveRange(items.Count-1,1);
            _Path = String.Join("\\", items);
            Folders = items;
        }
    }
}