using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewCategoryAdd : iViewBase
    {
        /// <summary>
        /// Nome nuova categoria
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Descrizione nuova categoria
        /// </summary>
        String Description { get; set; }

        ///// <summary>
        ///// Se iscrivere l'utente corrente come MANAGER (o se selezionare un manager specifico)
        ///// </summary>
        //Boolean IsManagerCurrentUsr { get; set; }

        /// <summary>
        /// Tipo categoria (visibilità)
        /// </summary>
        Domain.Enums.CategoryType Type { get; set; }

        ////Elenco ID persona + IsManager selezionati
        ////Sarà eventualmente a carico della VIEW fare il merge tra selettori diversi,
        ////nel caso si scelga di mettere:
        ////SOLO UN manager
        ////Più Manager
        ////più manager e più resolver...
        //IList<Domain.DTO.DTO_CategoryRole> Roles { get; set; }

        ///// <summary>
        ///// ID comunità della view, che deriva da:
        ///// Se omesso restituisce -1
        ///// </summary>
        //Int32 ViewCommunityId { get; set; }

        /// <summary>
        /// Dopo la creazione, redireziona alla pagina di edit, se tutto ok.
        /// </summary>
        /// <param name="NewCategoryId"></param>
        void NavigateToEdit(Int64 NewCategoryId);

        /// <summary>
        /// Inizializzazione View
        /// </summary>
        void Initialize(Domain.DTO.DTO_CategoryTypeComPermission CatTypePermission);

        ////To iView Base Internal
        //void DisplaySessionTimeout();

        void ShowNoPermission();

        Int32 SelectedManagerID { get; }

        void SendAction(
            ModuleTicket.ActionType Action,
            Int32 idCommunity,
            ModuleTicket.InteractionType Type,
            IList<KeyValuePair<Int32, String>> Objects = null);

        void UpdateUserName(String Name, bool IsCurrent);
    }
}
