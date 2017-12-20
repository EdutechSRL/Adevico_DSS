using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewMultimediaFileSettings: lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        String PreloadedBackUrl {get;}
        long PreloadedIdLink{get;}
        long PreloadedIdFile{get;}
       
        String BackUrl { get; set; }
        long IdLink { get; set; }
        long IdFile { get; set; }
        Boolean AllowSetDefaultDocument { set; }
       

        void LoadFileNotExist();
        void LoadFileNoPermission();
        void LoadFileWithoutIndex(BaseCommunityFile file);
        void LoadInvalidFileType(BaseCommunityFile file);
        void LoadTree(BaseCommunityFile file,IList<String> paths, String selectedPath);
        void SendToSessionExpiredPage(int idCommunity, String languageCode);

        RepositoryItemPermission GetModuleLinkPermission(int IdCommunity, int IdUser, ModuleLink link);
        //long GetPermissionToLink(int IdUser, long IdLink, BaseCommunityFile file, int IdCommunity);
        //RepositoryItemPermission GetModuleLinkPermission(int IdCommunity, long IdLink,int IdUser, ModuleObject sourceObject,ModuleObject linkedObject );
    }
}