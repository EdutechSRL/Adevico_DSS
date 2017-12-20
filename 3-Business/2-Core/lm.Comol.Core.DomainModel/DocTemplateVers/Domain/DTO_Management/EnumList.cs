using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management
{
    public enum TemplateFilter
    {
        ALL = -1,
        Definitive = 1,
        Draft = 2,
        Deleted = 3
    }

    public enum TemplateOrderCol
    {
        Name = 1,
        Status = 3,
        UpdatedOn = 4
    }

    public enum VersionEditError
    {
        none = 0,
        NotFound = 1,
        NotVersionForTemplate = 2,
        NotDraft = 3,
        NoPremission = 4,
        NoTemplate = 5,
        IsSystem = 6,
        IsDeleted = 7
    }
}
