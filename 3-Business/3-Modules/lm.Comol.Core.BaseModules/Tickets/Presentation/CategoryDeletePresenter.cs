using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Tickets;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class CategoryDeletePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
    #region "Initialize"
        
        private TicketService service;

        protected virtual new View.iViewCategoryDelete View
        {
            get { return (View.iViewCategoryDelete)base.View; }
        }

        public CategoryDeletePresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.service = new TicketService(oContext);
        }
        public CategoryDeletePresenter(iApplicationContext oContext, View.iViewCategoryDelete view)
            : base(oContext, view)
        {
            this.service = new TicketService(oContext);
        }

        //private lm.Comol.Core.BaseModules.Tickets.ModuleTicket _module;
        //private lm.Comol.Core.BaseModules.Tickets.ModuleTicket Module
        //{
        //    get
        //    {
        //        if ((_module == null))
        //        {
        //            Int32 idUser = UserContext.CurrentUserID;
        //            _module = service.PermissionGetService(idUser, CurrentCommunityId);
        //        }
        //        return _module;
        //    }
        //}
    #endregion

        public void SetTicketNum(Boolean IsRecursive)
        {
            View.TicketNum = service.CategoryGetAssociatedTicketNum(View.CategoryId, IsRecursive);
        }

        /// <summary>
        /// Inizializzazione controllo e bind delle DDL
        /// </summary>
        public void InitControl()
        {
            //IList<Domain.DTO.DTO_CategoryDDLItem> DelCategories = service.CategoryGetDDL(View.CategoryId);
            Boolean HasChildren = service.CategoryHasChildren(View.CategoryId);

            if (HasChildren)
            {
                View.TicketNum = 0;
                View.InitView(true, Domain.Enums.CategoryDeleteSteps.Step1_Children);
            }
            else
            {
                int TicketNum = service.CategoryGetAssociatedTicketNum(View.CategoryId, false);

                if (TicketNum > 0)
                {
                    View.TicketNum = TicketNum;
                    View.InitView(false, Domain.Enums.CategoryDeleteSteps.Step2_Ticket);
                }
                else
                {
                    View.TicketNum = 0;
                    View.InitView(false, Domain.Enums.CategoryDeleteSteps.Step4_Confirm);
                }
            }
        }

        /// <summary>
        /// Fa il bind dei dati in base allo step (SOLO FORWARD, in BACK non viene aggiornato!!!)
        /// </summary>
        /// <param name="Step"></param>
        public void BindStep(Domain.Enums.CategoryDeleteSteps Step)
        {
            if ((int)Step <= (int)View.StartStep)
                return;

            switch(Step)
            {
                case Domain.Enums.CategoryDeleteSteps.Step2_Ticket :
                    
                    break;

                case Domain.Enums.CategoryDeleteSteps.Step3a_ReassignAll :
                    Domain.DTO.DTO_CategoryTree cate = service.CategoryGetTreeDLLSingle(View.CommunityId, View.CategoryId);
                    if(cate == null)
                        //ATTENZIONE: GESTIONE ERRORI!!!
                        return;
                    View.SetReassignCategory(cate, service.CategoriesGetTreeDLLForDelete(View.CommunityId, View.CategoryId, false));
                    break;

                case Domain.Enums.CategoryDeleteSteps.Step3b_ReassignSingle :

                    View.SetReassignCategories(
                        service.CategoriesGetFromCategory(true, View.CommunityId, View.CategoryId),
                        service.CategoriesGetTreeDLLForDelete(View.CommunityId, View.CategoryId, false));
                        
                    break;
            }

        }

        ///// <summary>
        ///// Annulla e va al tree
        ///// </summary>
        //public void ToTree()
        //{
        //    //??
        //}

        ///// <summary>
        ///// Passa al secondo step
        ///// </summary>
        //public void Continue()
        //{
        //    View.ShowStep2();
        //}

        ///// <summary>
        ///// Riassegne le categorie... (TO DO)
        ///// </summary>
        ///// <param name="Reassigment"></param>
        //public void Reassign(IList<Domain.DTO.DTO_CategoryReassign> Reassigment)
        //{
        //    View.Close();
        //}
        

        public Boolean DeleteCategory(String ReassignMessage, long ReassignId, bool MoveUp)
        {
            Boolean Result = false;
            if (MoveUp)
                Result = service.CategoryDeletePutUp(View.CategoryId);
            else
                Result = service.CategoryDeleteAll(View.CategoryId, ReassignId, ReassignMessage);

            if (Result)
            { 
                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetPerson(this.UserContext.CurrentUserID));
                Objects.Add(ModuleTicket.KVPgetCategory(View.CategoryId));

                View.SendAction(ModuleTicket.ActionType.CategoryUndelete, View.CommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action
            }
            return Result;
        }

        public Boolean DeleteCategory(String ReassignMessage, IDictionary<long, long> Reassignments)
        {
            Boolean Result = service.CategoryDeleteAll(View.CategoryId, Reassignments, ReassignMessage);

            //Begin Action
            if (Result)
            {
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetPerson(this.UserContext.CurrentUserID));
                Objects.Add(ModuleTicket.KVPgetCategory(View.CategoryId));

                View.SendAction(ModuleTicket.ActionType.CategoryUndelete, View.CommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
            }
            //End Action

            return Result;
        }
    }
}