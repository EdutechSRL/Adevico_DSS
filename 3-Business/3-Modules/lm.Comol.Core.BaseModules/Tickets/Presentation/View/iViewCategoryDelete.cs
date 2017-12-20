using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewCategoryDelete : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        /// <summary>
        /// Solo per inizializzazione
        /// </summary>
        /// <param name="HasChildren">Indica SE la categoria corrente HA FIGLI</param>
        /// <param name="TicketNum">Indica il numero di Ticket Assegnati alla categoria corrente E le loro figlie</param>
        /// <param name="StartStep"></param>
        void InitView(bool HasChildren, Domain.Enums.CategoryDeleteSteps StartStep);

        //void ShowByStep(Domain.Enums.CategoryDeleteSteps Step);

        Domain.Enums.CategoryDeleteSteps CurrentStep { get; set; }
        Domain.Enums.CategoryDeleteSteps StartStep { get; }

        Boolean HasChildren { get; }
        Boolean CountChildren { get; }

        int TicketNum { get; set; }
        
        void SetReassignCategory(Domain.DTO.DTO_CategoryTree SourceCategories, IList<Domain.DTO.DTO_CategoryTree> DDLCategories);
        void SetReassignCategories(IList<Domain.DTO.DTO_CategoryList> SourceCategories, IList<Domain.DTO.DTO_CategoryTree> DDLCategories);

        Int64 CategoryId { get; set; }

        int CommunityId { get; }

        //Verificare SE la cancellazione avviene qui o nella pagina contenitore...

        void SendAction(
            ModuleTicket.ActionType Action,
            Int32 idCommunity,
            ModuleTicket.InteractionType Type,
            IList<KeyValuePair<Int32, String>> Objects = null);
    }
}
