using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public interface iViewSkinShare : iDomainView
    {
        Int64 SkinId { get; }

        void BindMainList(Domain.DTO.DtoSkinShares Shares);
        void BindOrganizations(IList<Domain.DTO.DtoSkinOrganization> Organization);
        void BindCommunities(IList<Int32> CommunitiesId);

    }
}
