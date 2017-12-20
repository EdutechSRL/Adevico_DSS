using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public interface IViewEditViewDetails : IViewEditViewDetailsPageBase
    {
        #region "Context"
            Boolean AllowSave{ set; }
            Boolean AllowShowItem { set; }
            Boolean AllowHideItem { set; }
        #endregion
        void SetTitle(ItemType itemType);
        void DisplayItemDetails(Boolean editMode, dtoDisplayRepositoryItem item, Int32 thumbnailWidth, Int32 thumbnailHeight,String allowedExtensionsForPreview);
        void UpdateItemDetails(dtoDisplayRepositoryItem item, Int32 thumbnailWidth, Int32 thumbnailHeight, String allowedExtensionsForPreview);

        void DisplayItemPermissions(Boolean editMode, dtoDisplayRepositoryItem item);

        #region "Messages"
            void DisplayUnknownItem();

        #endregion
        void DisplayAddedVersion(dtoCreatedItem addedVersion,String folderName);
        void SetUrlForView(String url);
        void SetUrlForEdit(String url);
        void InitializeDefaultTags(List<String> tags);
        void InitializeHeader();
    
    
    }
}