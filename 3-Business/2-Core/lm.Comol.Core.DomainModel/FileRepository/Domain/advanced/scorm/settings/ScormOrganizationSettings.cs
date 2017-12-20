using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain.ScormSettings
{
    [Serializable()]
    public class ScormOrganizationSettings
    {
        public virtual long Id { get; set; }
        public virtual long IdPackage { get; set; }
        public virtual IList<ScormItemSettings> Items { get; set; }

        #region "SCORM details"
            public virtual Boolean IsDefault { get; set; }
            public virtual Boolean IsVisible { get; set; }
            public virtual String SchemaVersion { get; set; }
            public virtual String StructureType { get; set; }
            public virtual String Title { get; set; }
            //Scorm Organization Id
            public virtual String OrganizationId { get; set; }
        #endregion

        public ScormOrganizationSettings()
        {
            IsVisible = true;
            StructureType = "";
            IsDefault = true;
            Items = new List<ScormItemSettings>();
            Title = "";
        }

        public ScormOrganizationSettings(String identifier, String title, Boolean isVisible, String structuretype, Boolean isDefault)
        {
            OrganizationId = identifier;
            IsVisible = isVisible;
            StructureType = structuretype;
            Items = new List<ScormItemSettings>();
            Title = title;
            IsDefault = isDefault;
        }

        public virtual ScormOrganizationSettings Copy(long idPackage)
        {
            ScormOrganizationSettings item = new ScormOrganizationSettings() { IdPackage = idPackage };
            item.IsDefault = IsDefault;
            item.IsVisible = IsVisible;
            item.SchemaVersion = SchemaVersion;
            item.StructureType = StructureType;
            item.Title = Title;
            item.OrganizationId = OrganizationId;
            return item;
        }
    }
}