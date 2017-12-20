using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.PolicyManagement.Presentation
{
    public interface IViewProfilePolicySubmission : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        Int32 CurrentIdUser { get; set; }
        List<dtoUserPolicyInfo> GetItemsValue();
        Boolean AcceptedMandatoryPolicy { get; }
        void InitializeControl(Int32 idUser);
        void InitializeControl();
        void SaveItems();
        void LoadItems(List<dtoUserDataPolicy> items);
        void LoadItemsError();
        void DisplayNoPolicyToAccept();
        void DisplayItemsToAccept(List<dtoUserDataPolicy> items);
        void DisplayUnknownUser();
    }
}
