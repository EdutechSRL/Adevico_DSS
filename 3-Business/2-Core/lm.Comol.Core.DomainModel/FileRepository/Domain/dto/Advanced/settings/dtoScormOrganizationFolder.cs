using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain.ScormSettings
{
    [Serializable()]
    public class dtoScormOrganizationFolder : dtoScormSettingsBase
    {
        public virtual List<dtoScormOrganizationFolder> Children { get; set; }
        public virtual List<dtoScormActivitySettings> Activities { get; set; }
        public dtoScormOrganizationFolder()
        {
            Type = ScormSettingsType.f;
            Children = new List<dtoScormOrganizationFolder>();
            Activities = new List<dtoScormActivitySettings>();
        }
        public static dtoScormOrganizationFolder CreateFrom(ScormItemSettings source, IEnumerable<ScormItemSettings> items, dtoScormOrganizationSettings organization, dtoScormPackageSettings package)
        {
            dtoScormOrganizationFolder folder = new dtoScormOrganizationFolder();
            folder.Id = source.Id;
            folder.Name = source.Title;
            foreach (ScormItemSettings item in items.Where(i => i.IdParentItem == source.Id))
            {
                if (items.Any(i => i.IdParentItem == item.Id))
                {
                    folder.Children.Add(dtoScormOrganizationFolder.CreateFrom(item, items.Where(i => i.IdParentItem != 0), organization, package));
                }
                else
                    folder.Activities.Add(dtoScormActivitySettings.CreateFrom(item, organization, package));
            }
            folder.DataChildren = "";
            if (folder.Children.Any() || folder.Activities.Any())
                folder.DataChildren = String.Join(",", folder.Children.Select(i => i.DataId).ToList().Union(folder.Children.Select(i => i.DataChildren).ToList().Union(folder.Activities.Select(a => a.DataId).ToList())));

            return folder;
        }               
    }
}