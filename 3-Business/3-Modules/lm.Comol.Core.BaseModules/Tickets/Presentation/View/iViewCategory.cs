using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewCategory : iViewBase
    {
        /// <summary>
        /// ID Categorie: null o 0 per nuova categoria
        /// By Querystring se presente al PageLoad!
        /// </summary>
        Int64 CurrentCategoryId { get; set; }

        /// <summary>
        /// Categorie della comunità corrente, per selezione padre.
        /// </summary>
        /// <remarks>
        /// Impostare PRIMA della Category.
        /// In alternativa, controllare l'UC di visualizzazione... (Selezione)
        /// Rivedere secondo le logiche di selezione/impostazione del padre... ???
        /// </remarks>
        //IList<Domain.Category> CommunityCategories { set; }

        /// <summary>
        /// Dati categoria corrente.
        /// Se nuova, dati "di default"
        /// </summary>
        Domain.Category Category { get; set; }
        
        Boolean HasFather { get; }
        Int64 FathersId { get; }

       IList<Domain.DTO.DTO_UserRole> AssociatedUsers { set; }

        void BindLanguages(
            List<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem> availableLanguages,
            List<lm.Comol.Core.DomainModel.Languages.LanguageItem> Languages, lm.Comol.Core.DomainModel.Languages.LanguageItem current);

        lm.Comol.Core.DomainModel.Languages.LanguageItem PreviousLanguage { get; set; }
        lm.Comol.Core.DomainModel.Languages.LanguageItem CurrentLanguage { get; }

        Boolean ShowDeleteManagers { get; set; }

        IDictionary<Int64, Boolean> UsersSettings { get; }

        void InitCategories(
            Domain.DTO.DTO_CategoryTypeComPermission EnabledCategories, 
            Domain.Enums.CategoryType CurrentType,
            Boolean IsDefault);

        void ShowNoPermission();
        void ShowWrongCategory();

        ///// <summary>
        ///// ID comunità della view, che deriva da:
        ///// Se omesso restituisce -1
        ///// </summary>
        //Int32 ViewCommunityId { get; set; }

        /// <summary>
        /// Restituisce TRUE se la pagina è valida (i dati possono essere salvati)
        /// altrimenti mostra errore e non continua con il comando.
        /// </summary>
        /// <returns></returns>
        Boolean Validate();

        ////To iView Base Internal
        //void DisplaySessionTimeout();

        void RedirectToList();


        //void SendAction(
        //    ModuleTicket.ActionType Action,
        //    Int32 idCommunity,
        //    ModuleTicket.InteractionType Type,
        //    IList<KeyValuePair<Int32, String>> Objects = null);

        void ShowForcedReassigned(Domain.DTO.DTO_CategoryCheckResponse response);

        void ShowReassignError(Domain.Enums.CategoryAssignersError AssignError);

        void ShowSendResponse(bool sended);
    }
}
