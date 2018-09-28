using System;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
{
    [Serializable]
    public class DTO_Share
    {
        public DTO_Share()
        {
        }

        public DTO_Share(Share share)
        {
            if (share != null)
            {
                Id = share.Id;
                IdCommunity = share.IdCommunity;
                IdGlossary = share.IdGlossary;
                IdTerm = share.IdTerm;
                Status = share.Status;
                Permissions = share.Permissions;
                Type = share.Type;
                Visible = share.Visible;
            }
        }

        public Int64 Id { get; set; }
        public Int32 IdCommunity { get; set; }
        public Int64 IdGlossary { get; set; }
        public Int64 IdTerm { get; set; }
        public Int32 IdFromCommunity { get; set; }
        public Boolean Visible { get; set; }
        public String FromCommunityName { get; set; }
        public String GlossaryDescription { get; set; }
        public ShareStatusEnum Status { get; set; }
        public SharePermissionEnum Permissions { get; set; }
        public ShareTypeEnum Type { get; set; }

        public bool HasPermission(SharePermissionEnum perm)
        {
            return (Permissions & perm) == perm;
        }
    }
}