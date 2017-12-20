using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Domain
{
    [Serializable]
    public class cookieRepository
    {
        public virtual long IdFolder {get;set;}
        public virtual FolderType Type {get;set;}
        public virtual OrderBy ItemsOrderBy { get; set; }
        public virtual Boolean Ascending { get; set; }
        public virtual String IdentifierPath { get; set; }
        public virtual RepositoryIdentifier Repository { get; set; }

        public cookieRepository()
        {
            Repository = new RepositoryIdentifier();
            IdentifierPath = "";
        }

        public override String ToString()
        {
            return ToString("-");
        }

        public String ToString(String separator)
        {
            return IdFolder.ToString() + separator + Type.ToString() + separator + ItemsOrderBy.ToString() + separator + Ascending.ToString()
                + separator + (String.IsNullOrEmpty(IdentifierPath) ? "": IdentifierPath.ToString()) + separator + Repository.ToString(separator);
        }

        public static cookieRepository CreateFromString(RepositoryIdentifier identifier, String separator, String value)
        {
            long idFolder = 0;
            Boolean ascending = true;
            cookieRepository item = new cookieRepository();
            item.Repository = identifier;
            item.Type = FolderType.standard;
            item.ItemsOrderBy = OrderBy.name;
            if (!String.IsNullOrEmpty(value)){
                try{
                    List<String> values = value.Split(separator.ToCharArray()).ToList();
                    long.TryParse(values[0], out idFolder);
                    item.Type = lm.Comol.Core.DomainModel.Helpers.EnumParser<FolderType>.GetByString(values[1], FolderType.standard);
                    item.ItemsOrderBy = lm.Comol.Core.DomainModel.Helpers.EnumParser<OrderBy>.GetByString(values[2], OrderBy.name);
                    Boolean.TryParse(values[3], out ascending);
                    item.IdentifierPath = values[4];
                    item.IdFolder = idFolder;
                    item.Ascending = ascending;
                    return item;
                }
                catch(Exception ex){
                    return null;
                }
            }
            return null;
        }

    }
}