using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.Business;
using lm.Comol.Modules.Standard.Glossary.Domain;

using NHibernate;
using NHibernate.Linq;
using NHibernate.Criterion;
using System.Diagnostics;
using System.Linq.Expressions;

//using System.Web;

namespace lm.Comol.Modules.Standard.Glossary.Business
{
    public class ServiceGlossary : iLinkedService
    {
        //TEST
        private iDataContext DC { set; get; }
        //TEST

        private BaseManager Manager { get; set; }
        private iUserContext UC { set; get; }
        #region initClass
        public ServiceGlossary() { }
        public ServiceGlossary(iApplicationContext oContext)
        {
            this.Manager = new BaseManager(oContext.DataContext);
            DC = oContext.DataContext;
            this.UC = oContext.UserContext;
        }
        public ServiceGlossary(iDataContext oDC)
        {
            DC = oDC;
            this.Manager = new BaseManager(oDC);
            this.UC = null;

        }

        #endregion
        public int ServiceModuleID()
        {
            return this.Manager.GetModuleID(ModuleGlossary.UniqueCode);
        }

        #region iLinkedService
        public List<StandardActionType> GetAllowedStandardAction(ModuleObject source, ModuleObject destination, int idUser, int idRole, int idCommunity, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            return new List<StandardActionType>();
        }
        public bool AllowActionExecution(ModuleLink link, int idUser, int idCommunity, int idRole, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            //Person person = Manager.GetPerson(idUser);
            switch (link.SourceItem.ObjectTypeID)
            {
                case (int)ModuleGlossary.ObjectType.Item:
                    return false;
                case (int)ModuleGlossary.ObjectType.Group:
                    return false;
                default:
                    return false;
            }
        }
        public bool AllowStandardAction(StandardActionType actionType, ModuleObject source, ModuleObject destination, int idUser, int idRole, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            return false;
        }
        public void SaveActionExecution(ModuleLink link, bool isStarted, bool isPassed, short Completion, bool isCompleted, short mark, int idUser, bool alreadyCompleted, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
        }
        public void SaveActionsExecution(List<dtoItemEvaluation<ModuleLink>> evaluatedLinks, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
        }

        public List<dtoItemEvaluation<long>> EvaluateModuleLinks(List<ModuleLink> links, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            return new List<dtoItemEvaluation<long>>();
        }
        public dtoEvaluation EvaluateModuleLink(ModuleLink link, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            return new dtoEvaluation() { isCompleted = false, Completion = 0, isPassed = false, isStarted = false };
        }
        public void PhisicalDeleteCommunity(Int32 idCommunity, Int32 idUser, String baseFilePath, String baseThumbnailPath)
        {
        }
        public void PhisicalDeleteRepositoryItem(long idFileItem, int idCommunity, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
        }

        public StatTreeNode<StatFileTreeLeaf> GetObjectItemFilesForStatistics(long objectId, int objectTypeId, Dictionary<int, string> translations, int idCommunity, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            return new StatTreeNode<StatFileTreeLeaf>();
        }
        #endregion

        #region Get
        #region "Web Service"
        public DTO_GlsItem GetItemDto(Int64 ItemID)
        {
            DTO_GlsItem dtoGi = new DTO_GlsItem();

            GlossaryItem GI = Manager.Get<GlossaryItem>(ItemID);

            if (GI != null)
            {
                dtoGi.Term = GI.Term;
                dtoGi.Definition = GI.Definition;
                dtoGi.ID = GI.Id;
                dtoGi.Group = new DTO_GlsGroup();

                if (GI.Group != null)
                {
                    dtoGi.Group.ID = GI.Group.Id;
                    dtoGi.Group.Name = GI.Group.Name;
                }
            }

            return dtoGi;
        }

        public List<DTO_GlsGroup> GetAvailableDtoGoup()
        {
            List<DTO_GlsGroup> dtoGroups = (from GlossaryGroup g in
                                                Manager.GetIQ<Domain.GlossaryGroup>()
                                            where (g.OwnerType == 0 && g.OwnerId == this.UC.CurrentCommunityID)
                                            select new DTO_GlsGroup() { ID = g.Id, Name = g.Name }).ToList();
            if (dtoGroups == null)
            {
                dtoGroups = new List<DTO_GlsGroup>();
            }

            return dtoGroups;
        }

        public List<DTO_GlsItem> GetAvailableDtoItem(Int64 GroupId)
        {
            List<DTO_GlsItem> dtoItems = (from GlossaryItem gi in
                                              Manager.GetIQ<Domain.GlossaryItem>()
                                          where gi.Group.Id == GroupId
                                          select new DTO_GlsItem()
                                          {
                                              ID = gi.Id,
                                              Definition = gi.Definition,
                                              Term = gi.Term,
                                              Group = new DTO_GlsGroup() { ID = gi.Group.Id, Name = gi.Group.Name }
                                          }).Distinct().ToList();
            return dtoItems;
        }

        #endregion


        public Model.ListModel GetListModel()
        {
            return this.GetInternalListModel('*', 1, DefaultView.NotSet, -1);
        }

        // * --> ALL
        // # --> Number
        // a-z --> letter
        public Model.ListModel GetListModel(String Letter, Int32 CurrentPage, DefaultView View, Int64 GroupId)
        {
            Char Chr = GlossaryHelpers.GetLetter(Letter)[0];
            return this.GetInternalListModel(Chr, CurrentPage, View, GroupId);
        }

        private Model.ListModel GetInternalListModel(Char Chr, Int32 CurPage, DefaultView View, Int64 GroupId)
        {
            Model.ListModel lm = new Model.ListModel();

            // Gruppi
            var AllGroups = Manager.GetAll<Domain.GlossaryGroup>(x => (x.OwnerType == 0 && x.OwnerId == this.UC.CurrentCommunityID));

            //Manager.Get<GlossaryGroup>(GroupId);
            //CONTROLLO OGNI VOLTA che il GROUPID corrente sia effettivamente valido per la comunità corrente.
            //Altrimenti ne recupero uno dalla comunità, prima quello di default e se non c'è il primo della lista...

            lm.Group = (from GlossaryGroup g in AllGroups
                        where (g.Id == GroupId)
                        select g)
                            .FirstOrDefault<GlossaryGroup>();

            if (lm.Group == null)
            {
                lm.Group = (from GlossaryGroup g in AllGroups
                            where (g.IsDefault == true)
                            select g)
                            .FirstOrDefault<GlossaryGroup>();
            }

            if (lm.Group == null)
            {
                lm.Group = (from GlossaryGroup g in AllGroups
                            select g)
                        .FirstOrDefault<GlossaryGroup>();
            }

            //if(lm.Group == null)
            //{
            //    lm.Group = new List<GlossaryGroup>();
            //}

            //Int64 NewGroupId = 0;
            //Se a questo punto NON  ho un gruppo, ne creo uno di defaul, con il nome della comunità...
            //if (lm.Group == null)
            //{
            //    Domain.Dto.AddItemGroupDto GroupDto = new Domain.Dto.AddItemGroupDto();
            //    GroupDto.Name = Manager.Get<Community>(this.UC.CurrentCommunityID).Name;
            //    GroupDto.DefaultView = DefaultView.MultiColumn;
            //    GroupDto.IsDefault = true;
            //    GroupDto.IsPaged = true;
            //    GroupDto.ItemPerPage = 50;
            //    NewGroupId = this.SaveOrUpdateGroup(GroupDto);

            //    if (NewGroupId > 0)
            //    {
            //        lm.Group = Manager.GetAll<Domain.GlossaryGroup>(x => (x.Id == NewGroupId && x.OwnerType == 0 && x.OwnerId == this.UC.CurrentCommunityID && x.IsDefault == true)).FirstOrDefault<GlossaryGroup>();
            //    }
            //}

            ////SE è ancora a null, c'è qualche casino "ignoto"...
            //if (lm.Group == null || NewGroupId < 0)
            //{
            //    throw new Exception("Glossary unknow error: Can't create/retrive group!");
            //}

            //if (View == DefaultView.NotSet)
            //{
            //    View = lm.Group.DefaultView;
            //}

            //Se supera il numero massimo di elementi, comunque mostro il multicolumn

            if (lm.Group != null)
            {
                if (View == DefaultView.NotSet)
                    View = lm.Group.DefaultView;

                lm.CurrentView = View;
                switch (View)
                {
                    case DefaultView.AllDefinition:
                        {
                            //Terms

                            var Terms = from GlossaryItem term in
                                            Manager.GetAll<Domain.GlossaryItem>(term => (term.Group.Id == lm.Group.Id))
                                        orderby term.Term
                                        select term;


                            switch (Chr)
                            {
                                case '*':
                                    lm.Items = new List<Domain.GlossaryItem>();
                                    break;
                                case '#':
                                    lm.Items = (from Domain.GlossaryItem itm in Terms where (Char.IsDigit(itm.FirstLetter)) select itm).ToList<Domain.GlossaryItem>();
                                    break;
                                case '$':
                                    lm.Items = (from Domain.GlossaryItem itm in Terms where (!Char.IsLetterOrDigit(itm.FirstLetter)) select itm).ToList<Domain.GlossaryItem>();
                                    break;
                                default:
                                    lm.Items = (from Domain.GlossaryItem itm in Terms where (itm.FirstLetter == Chr) select itm).ToList<Domain.GlossaryItem>();
                                    break;
                            }

                            if (lm.Items.Count == 0)
                            {
                                Chr = '*';
                                lm.Items = Terms.ToList<Domain.GlossaryItem>();
                            }

                            // Letter

                            lm.UsedLetters = (from GlossaryItem itm in Terms select itm.FirstLetter).Distinct().ToList();

                            lm.AllLetters = new List<Domain.Dto.LetterDto>();
                            lm.LetterRecurrence = new Dictionary<Char, Int32>();

                            Int32 Rec = 0;

                            Domain.Dto.LetterDto All = new Domain.Dto.LetterDto();
                            All.value = '*';
                            All.Letter = "All";
                            All.IsEnable = true;
                            All.IsSelected = (All.value == Chr);
                            lm.AllLetters.Add(All);
                            //lm.AllLetters.Add(All.value, All);

                            Domain.Dto.LetterDto Symb = new Domain.Dto.LetterDto();
                            Symb.value = '$';
                            Symb.Letter = "Simbol";
                            Symb.IsSelected = (Symb.value == Chr);
                            Rec = (from GlossaryItem itm in Terms where (!Char.IsLetterOrDigit(itm.FirstLetter)) select itm.FirstLetter).Count();
                            Symb.IsEnable = (Rec >= 0);
                            lm.LetterRecurrence.Add('$', Rec);
                            lm.AllLetters.Add(Symb);


                            Domain.Dto.LetterDto Num = new Domain.Dto.LetterDto();
                            Num.value = '#';
                            Num.Letter = "0-9";
                            Num.IsSelected = (Num.value == Chr);
                            Rec = (from GlossaryItem itm in Terms where (Char.IsDigit(itm.FirstLetter)) select itm.FirstLetter).Count();
                            Symb.IsEnable = (Rec >= 0);
                            lm.LetterRecurrence.Add('#', Rec);
                            lm.AllLetters.Add(Num);

                            for (int i = 97; i <= 122; i++)
                            {
                                Domain.Dto.LetterDto cr = new Domain.Dto.LetterDto();
                                cr.value = (char)i;
                                cr.Letter = cr.value.ToString();
                                cr.IsEnable = lm.UsedLetters.Contains(Char.ToLower(cr.value));
                                if (cr.IsEnable)
                                {
                                    Rec = (from GlossaryItem itm in Terms where (itm.FirstLetter == cr.value) select itm.FirstLetter).Count();
                                    lm.LetterRecurrence.Add(cr.value, Rec);
                                }
                                else
                                {
                                    //LetterRecurrece.Add(cr.value, 0);
                                }
                                cr.IsSelected = (cr.value == Chr);
                                lm.AllLetters.Add(cr);
                            }



                            //Int32 TotalItem = lm.Items.Count();
                            //Int32 PageSize = lm.Group.ItemPerPage;
                            //CurPage

                            if ((lm.Group.IsPaged) && lm.Group.TotalItems <= this.MaxGlossaryItemPaging)
                            {

                                Int32 TotalPage = (lm.Items.Count() / lm.Group.ItemPerPage);
                                if ((lm.Items.Count() % lm.Group.ItemPerPage) > 0) { TotalPage++; }

                                if (lm.Group.ItemPerPage <= 0) { lm.Group.ItemPerPage = 20; }
                                if (CurPage > TotalPage) { CurPage = TotalPage; }
                                if (CurPage < 1) { CurPage = 1; }

                                //Int32 Skipelement = (CurPage - 1) * lm.Group.ItemPerPage;
                                //Int32 TakeElement = lm.Group.ItemPerPage;

                                lm.Items = lm.Items.Skip((CurPage - 1) * lm.Group.ItemPerPage).Take(lm.Group.ItemPerPage).ToList();

                                lm.CurrentPage = CurPage;
                                lm.TotalPage = TotalPage;
                            }
                            else
                            {
                                lm.CurrentPage = 1;
                                lm.TotalPage = 1;
                            }

                            lm.SelectedLetter = Chr;

                            break;
                        }
                    case DefaultView.MultiColumn:
                        {
                            var TermsDTO = from Domain.Dto.ListItemDTO term in
                                               Manager.GetAll<Domain.Dto.ListItemDTO>(term => (term.Group.Id == lm.Group.Id))
                                           orderby term.Term
                                           select term;

                            //Char

                            switch (Chr)
                            {
                                case '*':
                                    lm.ColumnsItems = new List<Domain.Dto.ListItemDTO>();
                                    break;
                                case '#':
                                    lm.ColumnsItems = (from Domain.Dto.ListItemDTO itm in TermsDTO where (Char.IsDigit(itm.FirstLetter)) select itm).ToList<Domain.Dto.ListItemDTO>();
                                    break;
                                case '$':
                                    lm.ColumnsItems = (from Domain.Dto.ListItemDTO itm in TermsDTO where (!Char.IsLetterOrDigit(itm.FirstLetter)) select itm).ToList<Domain.Dto.ListItemDTO>();
                                    break;
                                default:
                                    lm.ColumnsItems = (from Domain.Dto.ListItemDTO itm in TermsDTO where (itm.FirstLetter == Chr) select itm).ToList<Domain.Dto.ListItemDTO>();
                                    break;
                            }

                            if (lm.ColumnsItems.Count == 0)
                            {
                                Chr = '*';
                                lm.ColumnsItems = TermsDTO.ToList<Domain.Dto.ListItemDTO>();
                            }

                            //Letter

                            lm.UsedLetters = (from Domain.Dto.ListItemDTO itm in TermsDTO select itm.FirstLetter).Distinct().ToList();

                            lm.AllLetters = new List<Domain.Dto.LetterDto>();
                            lm.LetterRecurrence = new Dictionary<Char, Int32>();

                            Int32 Rec = 0;

                            // Tutte
                            Domain.Dto.LetterDto All = new Domain.Dto.LetterDto();
                            All.value = '*';
                            All.Letter = "All";
                            All.IsEnable = true;
                            All.IsSelected = (Chr == '*');
                            lm.AllLetters.Add(All);

                            //Simboli
                            Domain.Dto.LetterDto Symb = new Domain.Dto.LetterDto();
                            Symb.value = '$';
                            Symb.Letter = "Simbol";
                            Symb.IsSelected = (Symb.value == Chr);
                            Rec = (from Domain.Dto.ListItemDTO itm in TermsDTO where (!Char.IsLetterOrDigit(itm.FirstLetter)) select itm.FirstLetter).Count();
                            Symb.IsEnable = (Rec >= 0);
                            lm.LetterRecurrence.Add('$', Rec);
                            lm.AllLetters.Add(Symb);

                            //Cifre
                            Domain.Dto.LetterDto Num = new Domain.Dto.LetterDto();
                            Num.value = '#';
                            Num.Letter = "0-9";
                            Num.IsSelected = (Num.value == Chr);
                            Rec = (from Domain.Dto.ListItemDTO itm in TermsDTO where (Char.IsDigit(itm.FirstLetter)) select itm.FirstLetter).Count();
                            Symb.IsEnable = (Rec >= 0);
                            lm.LetterRecurrence.Add('#', Rec);
                            lm.AllLetters.Add(Num);

                            //Lettere
                            for (int i = 97; i <= 122; i++)
                            {
                                Domain.Dto.LetterDto cr = new Domain.Dto.LetterDto();
                                cr.value = (char)i;
                                cr.Letter = cr.value.ToString();
                                cr.IsEnable = lm.UsedLetters.Contains(Char.ToLower(cr.value));
                                if (cr.IsEnable)
                                {
                                    Rec = (from Domain.Dto.ListItemDTO itm in TermsDTO where (itm.FirstLetter == cr.value) select itm.FirstLetter).Count();
                                    lm.LetterRecurrence.Add(cr.value, Rec);
                                }
                                cr.IsSelected = (cr.value == Chr);
                                lm.AllLetters.Add(cr);
                            }


                            if (lm.Group.IsPaged || lm.Group.TotalItems > this.MaxGlossaryItemPaging)
                            {
                                Int32 TotalPage = (lm.ColumnsItems.Count() / lm.Group.ItemPerPage);
                                if ((lm.ColumnsItems.Count() % lm.Group.ItemPerPage) > 0) { TotalPage++; }

                                if (lm.Group.ItemPerPage <= 0) { lm.Group.ItemPerPage = 20; }
                                if (CurPage > TotalPage) { CurPage = TotalPage; }
                                if (CurPage < 1) { CurPage = 1; }

                                lm.ColumnsItems = lm.ColumnsItems.Skip((CurPage - 1) * lm.Group.ItemPerPage).Take(lm.Group.ItemPerPage).ToList();

                                lm.SelectedLetter = Chr;

                                lm.CurrentPage = CurPage;
                                lm.TotalPage = TotalPage;
                            }
                            else
                            {
                                lm.CurrentPage = 1;
                                lm.TotalPage = 1;
                            }

                            break;
                        }
                }

                lm.HasElement = ((lm.ColumnsItems != null && lm.ColumnsItems.Count > 0)
                    & (lm.CurrentView == DefaultView.MultiColumn))
                    || ((lm.Items != null && lm.Items.Count > 0)
                    & (lm.CurrentView == DefaultView.AllDefinition));
            }
            else
            {
                lm.HasElement = false;
            }


            return lm;
        }

        public Model.EditItemModel GetEditItemModel(Int64 GlossaryItemID, Int64 GlossaryGroupID)
        {
            Model.EditItemModel EIM = new Model.EditItemModel();

            EIM.Item = Manager.Get<GlossaryItem>(GlossaryItemID);
            if (EIM.Item == null)
            {
                return GetAddItemModel(GlossaryGroupID);
            }

            EIM.Groups = Manager.GetAll<Domain.GlossaryGroup>(x => (x.OwnerType == 0 && x.OwnerId == this.UC.CurrentCommunityID)).ToList<GlossaryGroup>();

            return EIM;
        }

        public Model.EditItemModel GetAddItemModel(Int64 GlossaryGroupID)
        {
            Model.EditItemModel EIM = new Model.EditItemModel();

            EIM.Groups = Manager.GetAll<Domain.GlossaryGroup>(x => (x.OwnerType == 0 && x.OwnerId == this.UC.CurrentCommunityID)).ToList<GlossaryGroup>();

            EIM.Item = new GlossaryItem();
            EIM.Item.Group = new GlossaryGroup();

            if (GlossaryGroupID <= 0)
            {
                GlossaryGroupID = (from GlossaryGroup g in EIM.Groups where (g.IsDefault = true) select g.Id).FirstOrDefault<Int64>();

                if (GlossaryGroupID == null || GlossaryGroupID == 0)
                {
                    GlossaryGroupID = -1;
                }
            }

            EIM.Item.Group.Id = GlossaryGroupID;
            return EIM;
        }

        public Model.EditGroup GetEditGroupModel(Int64 GroupID)
        {
            //Aggiungere controllo per verificare che il gruppo sia nella comunità...

            Model.EditGroup model = new Model.EditGroup();

            if (GroupID > 0)
            {
                model.Group = Manager.GetAll<Domain.GlossaryGroup>(x => (
                                       x.OwnerType == 0 &&
                                       x.OwnerId == this.UC.CurrentCommunityID &&
                                       x.Id == GroupID))
                                .First<GlossaryGroup>();

                model.CanShowDetailedList = (model.Group.TotalItems <= MaxGlossaryItemView);
                model.CanShowNotPaged = (model.Group.TotalItems <= MaxGlossaryItemPaging);
            }
            else
            {
                model.Group = new GlossaryGroup();
                model.Group.Id = 0;
                model.Group.IsDefault = false;
                model.Group.IsPaged = true;
                model.Group.ItemPerPage = 10;
                model.Group.Name = "";
                model.Group.TotalItems = 0;
                model.CanShowDetailedList = true;
                model.CanShowNotPaged = true;
            }

            return model;
        }

        public Model.ListGlossaryModel GetGlossaryListModel()
        {
            Model.ListGlossaryModel model = new Model.ListGlossaryModel();

            model.Groups = (from GlossaryGroup g in
                                Manager.GetAll<Domain.GlossaryGroup>
                                (x => (
                                    x.OwnerType == 0 &&
                                    x.OwnerId == this.UC.CurrentCommunityID
                                    ))
                            orderby g.Name
                            select g).ToList<GlossaryGroup>();

            //if(model == null || model.Groups.Count <= 0)
            //{
            //    Domain.Dto.AddItemGroupDto GroupDto = new Domain.Dto.AddItemGroupDto();
            //    GroupDto.Name = Manager.Get<Community>(this.UC.CurrentCommunityID).Name;
            //    GroupDto.DefaultView = DefaultView.MultiColumn;
            //    GroupDto.IsDefault = true;
            //    GroupDto.IsPaged = true;
            //    GroupDto.ItemPerPage = 50;
            //    Int64 NewGroupId = this.SaveOrUpdateGroup(GroupDto);

            //    if(NewGroupId > 0)
            //    {
            //        model.Groups = new List<GlossaryGroup>();
            //        model.Groups.Add(Manager.Get<GlossaryGroup>(NewGroupId));
            //    }

            //}
            if (model == null)
                model = new Model.ListGlossaryModel();

            return model;
        }

        public IList<GlossaryGroup> GetGlossaryList(Int32 CommunityID)
        {
            return (from GlossaryGroup g in
                        Manager.GetAll<Domain.GlossaryGroup>
                        (x => (
                            x.OwnerType == 0 &&
                            x.OwnerId == CommunityID
                            ))
                    orderby g.Name
                    select g).ToList<GlossaryGroup>();
        }
        #endregion

        public Boolean SaveOrUpdateItem(Domain.Dto.AddEditItemDto ItemDTO)
        {
            Int64 OldGroupId = ItemDTO.GroupId;
            Boolean isOk = false;
            GlossaryItem Item;
            GlossaryGroup Group = Manager.Get<GlossaryGroup>(ItemDTO.GroupId);

            Person user = Manager.Get<Person>(UC.CurrentUserID);


            if (ItemDTO.Id > 0)
            {
                Item = Manager.Get<GlossaryItem>(ItemDTO.Id);
                OldGroupId = Item.Group.Id;

                Item.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
            }
            else
            {
                Item = new GlossaryItem();
                Item.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
            }

            Item.Group = Group;
            Item.Term = ItemDTO.Term;
            Item.Definition = ItemDTO.Definition;
            Item.SetFirstLetter();

            try
            {
                Manager.SaveOrUpdate<GlossaryItem>(Item);
                isOk = true;
            }
            catch (Exception ex)
            {

            }




            if (isOk)
            {
                UpdateGlossaryItemNumber(ItemDTO.GroupId, true);
                if (OldGroupId != ItemDTO.GroupId)
                {
                    UpdateGlossaryItemNumber(OldGroupId, false);
                }
            }

            return isOk;
        }
        private Boolean UpdateGlossaryItemNumber(Int64 GlossaryId, Boolean ChangeView)
        {
            GlossaryGroup Group = Manager.Get<GlossaryGroup>(GlossaryId);

            Group.TotalItems = (from Domain.Dto.ListItemDTO term in
                                    Manager.GetAll<Domain.Dto.ListItemDTO>(term => (term.Group.Id == GlossaryId))
                                orderby term.Term
                                select term.Id).Count();

            if ((Group.TotalItems > MaxGlossaryItemPaging) && ChangeView)
            {
                Group.IsPaged = true;
            }


            if ((Group.TotalItems > MaxGlossaryItemView) && ChangeView)
            {
                Group.DefaultView = DefaultView.MultiColumn;
            }

            try
            {
                Manager.SaveOrUpdate<GlossaryGroup>(Group);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public Int64 SaveOrUpdateGroup(Domain.Dto.AddItemGroupDto GroupDTO)
        {
            GlossaryGroup Group;
            Person user = Manager.Get<Person>(UC.CurrentUserID);




            if (GroupDTO.Id > 0)
            {
                Group = Manager.Get<GlossaryGroup>(GroupDTO.Id);
                Group.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
            }
            else
            {
                Group = new GlossaryGroup();
                Group.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                Group.OwnerType = 0; //Community
                Group.OwnerId = this.UC.CurrentCommunityID;
                Group.ItemPerPage = 20;
                Group.DefaultView = DefaultView.MultiColumn;
            }

            Group.Name = GroupDTO.Name;


            Group.DefaultView = GroupDTO.DefaultView;
            Group.IsPaged = GroupDTO.IsPaged;
            Group.ItemPerPage = GroupDTO.ItemPerPage;

            Group.IsDefault = GroupDTO.IsDefault;

            var AllGroups = Manager.GetAll<Domain.GlossaryGroup>
                                 (x => (
                                     x.OwnerType == 0 &&
                                     x.OwnerId == this.UC.CurrentCommunityID &&
                                     x.IsDefault == true &&
                                     x.Id != Group.Id
                                     ));

            if (Group.IsDefault && AllGroups.Count() > 0)
            {
                foreach (GlossaryGroup grp in AllGroups)
                {
                    grp.IsDefault = false;
                }

                Manager.SaveOrUpdateList<GlossaryGroup>(AllGroups);
            }
            else if ((Group.IsDefault) && AllGroups.Count() == 0)
            {
                Group.IsDefault = true;
            }

            try
            {
                Manager.SaveOrUpdate<GlossaryGroup>(Group);
                return Group.Id;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public Boolean DeleteItem(Int64 ItemID)
        {
            GlossaryItem Item = Manager.Get<GlossaryItem>(ItemID);
            Boolean isOk = false;
            try
            {
                Manager.DeletePhysical<GlossaryItem>(Item);
                isOk = true;
            }
            catch (Exception ex)
            {
                //return false;
            }


            if (isOk)
            {
                UpdateGlossaryItemNumber(Item.Group.Id, false);
            }

            return isOk;
        }
        public Boolean DeleteGroup(Int64 GroupID)
        {
            GlossaryGroup Group = Manager.Get<GlossaryGroup>(GroupID);

            if (Group == null)
            {
                return false;
            }

            int CommunityGroups = (from Domain.GlossaryGroup grp in Manager.GetIQ<GlossaryGroup>()
                                   where grp.OwnerType == 0 && grp.OwnerId == this.UC.CurrentCommunityID
                                   select grp.Id).Count();

            if (CommunityGroups <= 1)
            {
                return false;
            }

            Boolean isOk = false;

            Boolean isDefault = Group.IsDefault;

            IList<GlossaryItem> Items = Manager.GetAll<Domain.GlossaryItem>(term => (term.Group.Id == Group.Id)).ToList();
            if (Items != null && Items.Count > 0)
            {
                try { Manager.DeletePhysicalList<GlossaryItem>(Items); }
                catch { return false; }
            }

            try
            {
                Manager.DeletePhysical<GlossaryGroup>(Group);
                isOk = true;
            }
            catch (Exception ex)
            {
                //return false;
            }

            if (isOk && isDefault)
            {
                GlossaryGroup DefGroup = Manager.GetAll<Domain.GlossaryGroup>(x => (x.OwnerType == 0 && x.OwnerId == this.UC.CurrentCommunityID)).FirstOrDefault();
                if (DefGroup != null)
                {
                    Person user = Manager.Get<Person>(UC.CurrentUserID);
                    DefGroup.UpdateMetaInfo(user, this.UC.IpAddress, this.UC.ProxyIpAddress);
                    DefGroup.IsDefault = true;
                    Manager.SaveOrUpdate<GlossaryGroup>(DefGroup);
                }
            }

            return isOk;
        }

        private int MaxGlossaryItemPaging
        {
            get
            {
                return 150;
            }
        }
        private int MaxGlossaryItemView
        {
            get
            {
                return 1000;
            }
        }

        public Int32 CopyGlossary(IList<Int64> SourceGlossariesIds, Int32 DestCommunityId)
        {
            Person user = Manager.Get<Person>(UC.CurrentUserID);
            //UC.IpAddress, UC.ProxyIpAddress)

            Int32 GlossaryCount = 0;
            Int32 TermsCount = 0;

            IList<GlossaryGroup> SourceGlossaries = (from GlossaryGroup g in
                                                         Manager.GetAll<Domain.GlossaryGroup>
                                                         (x => (
                                                             SourceGlossariesIds.Contains(x.Id)
                                                             ))
                                                     select g).ToList<GlossaryGroup>();

            IList<GlossaryGroup> DestGlossaries = new List<GlossaryGroup>();

            foreach (GlossaryGroup GlGroup in SourceGlossaries)
            {
                GlossaryGroup DestGlGroup = new GlossaryGroup();
                DestGlGroup.CreatedBy = GlGroup.CreatedBy;
                DestGlGroup.CreatedOn = GlGroup.CreatedOn;
                DestGlGroup.CreatorIpAddress = GlGroup.CreatorIpAddress;
                DestGlGroup.CreatorProxyIpAddress = GlGroup.CreatorProxyIpAddress;
                DestGlGroup.DefaultView = GlGroup.DefaultView;
                DestGlGroup.Deleted = GlGroup.Deleted;
                DestGlGroup.IsDefault = false;
                DestGlGroup.IsPaged = GlGroup.IsPaged;
                DestGlGroup.ItemPerPage = GlGroup.ItemPerPage;
                //DestGlGroup.ModifiedBy = UserId;
                //DestGlGroup.ModifiedIpAddress = 
                DestGlGroup.ModifiedOn = DateTime.Now;
                //DestGlGroup.ModifiedProxyIpAddress =
                DestGlGroup.Name = GlGroup.Name;
                DestGlGroup.OwnerId = DestCommunityId;
                DestGlGroup.OwnerType = GlGroup.OwnerType;
                // ! ATTENZIONE NEL CASO DI COPINO SOLO SINGOLI TERMINI ! //
                DestGlGroup.TotalItems = GlGroup.TotalItems;
                DestGlGroup.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);


                Manager.SaveOrUpdate<GlossaryGroup>(DestGlGroup);
                GlossaryCount += 1;

                if (DestGlGroup.TotalItems > 0)
                {
                    IList<GlossaryItem> DestTerms = new List<GlossaryItem>();

                    IList<GlossaryItem> Items = Manager.GetAll<Domain.GlossaryItem>(term => (term.Group.Id == GlGroup.Id)).ToList<GlossaryItem>();

                    foreach (GlossaryItem Term in Items)
                    {
                        GlossaryItem DestTerm = new GlossaryItem();
                        DestTerm.CreatedBy = Term.CreatedBy;
                        DestTerm.CreatedOn = Term.CreatedOn;
                        DestTerm.CreatorIpAddress = Term.CreatorIpAddress;
                        DestTerm.CreatorProxyIpAddress = Term.CreatorProxyIpAddress;
                        DestTerm.Definition = Term.Definition;
                        DestTerm.Deleted = Term.Deleted;
                        DestTerm.FirstLetter = Term.FirstLetter;
                        DestTerm.Group = DestGlGroup;
                        DestTerm.IsPublic = Term.IsPublic;
                        DestTerm.Link = Term.Link;
                        //DestTerm.ModifiedBy =
                        //DestTerm.ModifiedIpAddress =
                        DestTerm.ModifiedOn = DateTime.Now;
                        //DestTerm.ModifiedProxyIpAddress =
                        DestTerm.Term = Term.Term;

                        DestTerm.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

                        DestTerms.Add(DestTerm);

                        TermsCount += 1;
                    }
                    Manager.SaveOrUpdateList<GlossaryItem>(DestTerms);
                }
                //DestGlossaries.Add(DestGlGroup);

            }



            return GlossaryCount;
        }
    }
}