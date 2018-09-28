using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{

    [Serializable]
    [FlagsAttribute]
    public enum PmActivityPermission
    {
      //generico
      None=0,
      ManageProject = 1,
      ManageResources = 2,
      AddAttachments = 4,
      SetCompleteness = 8,
      SetStartDate = 16,
      SetEndDate=32,
      SetDeadline = 64,
      SetStatus = 128,
      ViewDetails = 256,
      UpdateDetails = 512,
      ManageLinks = 1024,
      VirtualDelete=2048,
      VirtualUnDelete = 4096,
      PhisicalDelete = 8192,
      Add = 16384,
      ViewAttachments = 32768,
      DownloadAttacchments = 65536,
      VirtualDeleteAttachments = 131072,
      VirtualUnDeleteAttachments = 262144,
      PhisicalDeleteAttachments = 524288,
      ViewProjectMap= 1048576,
      ChangeOwner =  2097152,
      SetMyCompleteness = 4194304,
        ManageAttachments = 8388608,
        ManageActivityAttachments =16777216
    }
}