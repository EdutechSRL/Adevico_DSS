using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Dashboard.Domain
{
    [Serializable]
    public class dtoCommunityForTags : lm.Comol.Core.Dashboard.Domain.dtoBaseCommunityItem
    {
        public List<lm.Comol.Core.Tag.Domain.dtoTagSelectItem> AvailableTags { get; set; }

        public dtoCommunityForTags()
        {
            AvailableTags = new List<lm.Comol.Core.Tag.Domain.dtoTagSelectItem>();
        }
        public dtoCommunityForTags(lm.Comol.Core.BaseModules.CommunityManagement.dtoTreeCommunityNode node, Dictionary<Int32, List<long>> associations)
        {
            AvailableTags = new List<lm.Comol.Core.Tag.Domain.dtoTagSelectItem>();
            Id = node.Id;
            Name = node.Name;
            IdOrganization = node.IdOrganization;
            IdType = node.IdCommunityType;
            Status = node.Status;
            IdTags = (associations.ContainsKey(node.Id) ? associations[node.Id] : new List<long>());
        }
    }
}