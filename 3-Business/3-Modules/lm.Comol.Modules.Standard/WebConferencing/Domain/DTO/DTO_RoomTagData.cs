using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.DTO
{
    [Serializable]
    public class DTO_RoomTagData
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public String AccessUrl
        { 
            get
            {
                return RootObject.ExternalAccess(Code);
            }
        }
        /// <summary>
        /// Codice Accesso! Necessario per recuperare correttamente l'url!
        /// </summary>
        public String Code { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? StartOn { get; set; }
        public DateTime? FinishOn { get; set; }
        public String CommunityName { get; set; }
        public lm.Comol.Core.DomainModel.Person CreatedBy { get; set; }
        public DTO_UserTagData DestUser { get; set; }

    }
}
