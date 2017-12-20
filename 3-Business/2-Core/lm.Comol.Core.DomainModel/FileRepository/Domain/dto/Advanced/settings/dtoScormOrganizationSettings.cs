using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain.ScormSettings
{
    [Serializable()]
    public class dtoScormOrganizationSettings : dtoScormSettingsBase
    {
        public virtual List<dtoScormOrganizationFolder> Children { get; set; }
        public virtual List<dtoScormActivitySettings> Activities { get; set; }
        public dtoScormOrganizationSettings()
        {
            Type = ScormSettingsType.o;
            Children = new List<dtoScormOrganizationFolder>();
            Activities = new List<dtoScormActivitySettings>();
        }

        public static dtoScormOrganizationSettings CreateFrom(ScormOrganizationSettings source, dtoScormPackageSettings package)
        {
            dtoScormOrganizationSettings organization = new dtoScormOrganizationSettings();
            organization.Id = source.Id;
            organization.Name = source.Title;
            foreach (ScormItemSettings item in source.Items.Where(i => i.IdParentItem == 0))
            {
                if (source.Items.Any(i => i.IdParentItem == item.Id))
                {
                    organization.Children.Add(dtoScormOrganizationFolder.CreateFrom(item, source.Items.Where(i => i.IdParentItem != 0), organization, package));
                }
                else
                    organization.Activities.Add(dtoScormActivitySettings.CreateFrom(item, organization, package));
            }
            organization.DataChildren = "";
            if (organization.Children.Any() || organization.Activities.Any())
                organization.DataChildren = String.Join(",", organization.Children.Select(i => i.DataId).ToList().Union(organization.Children.Select(i => i.DataChildren).ToList().Union(organization.Activities.Select(a => a.DataId).ToList())));

            return organization;
        }              
    }
}