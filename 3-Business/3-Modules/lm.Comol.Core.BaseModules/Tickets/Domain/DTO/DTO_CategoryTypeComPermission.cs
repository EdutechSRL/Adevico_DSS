using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    public class DTO_CategoryTypeComPermission
    {
        public bool CanPublic { get; set; }
        public bool CanPrivate { get; set; }
        public bool CanTicket { get; set; }

        public bool CanCreate
        {
            get
            {
                return CanPublic || CanPrivate || CanTicket;
            }
        }

        public bool GetByType(Domain.Enums.CategoryType Type)
        {
            switch(Type)
            {
                case Enums.CategoryType.Public:
                    return CanPublic;
                case Enums.CategoryType.Current:
                    return CanPrivate;
                case Enums.CategoryType.Ticket:
                    return CanTicket;
            }

            return false;
        }

    }
}
