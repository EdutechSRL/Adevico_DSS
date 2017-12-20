using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.ModuleLinks;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewDisplayUrlItem : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        DisplayActionMode Display { get; set; }
        String ContainerCSS { get; set; }
        Boolean EnableAnchor { get; set; }
        Boolean DisplayCreateInfo { get; set; }
        String GetRemovedUser { get; }
        String GetUnknownUser { get; }
        lm.Comol.Core.DomainModel.Helpers.IconSize IconSize { get; set; }
        String ItemIdentifier { get; set; }
        lm.Comol.Core.DomainModel.Repository.RepositoryItemType ItemType { get; }


        String GetBaseUrl();
        void InitializeControl(DisplayActionMode mode, String url, String name = "", DateTime? createdOn = null, Person createdBy = null, List<dtoPlaceHolder> placeHolders = null);
        void InitializeControlLite(DisplayActionMode mode, String url, String name = "", DateTime? createdOn = null, litePerson createdBy = null, List<dtoPlaceHolder> placeHolders = null);
                
        void DisplayRemovedObject();
        void DisplayItem(String name, DateTime? createdOn = null, String username = "", String usersurname = "");
        void DisplayItem(String name, String url, DateTime? createdOn = null, String username = "", String usersurname = "");
        void DisplayEmptyAction();
        void DisplayPlaceHolders(List<dtoPlaceHolder> items);
    }
}