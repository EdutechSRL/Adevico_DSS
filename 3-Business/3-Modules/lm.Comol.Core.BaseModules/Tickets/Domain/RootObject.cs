using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
    /// <summary>
    /// Definisce gli url delle pagine
    /// </summary>
    [Serializable()]
    public class RootObject
    {
        /// <summary>
        /// Cartella con le pagine del servizio
        /// </summary>
        private readonly static String basePath = "Modules/Ticket/";

#region "Category"
        ///// <summary>
        ///// Categorie: lista
        ///// </summary>
        ///// <returns></returns>
        //public static String CategoryList()
        //{
        //    return basePath + "CategoriesList.aspx";
        //}
        /// <summary>
        /// Categorie: lista
        /// </summary>
        /// <returns></returns>
        public static String CategoryList(int CommunityId)
        {
            if (CommunityId > 0)
                return basePath + "CategoriesList.aspx" + "?CommunityId=" + CommunityId.ToString();
            else return basePath + "CategoriesList.aspx";
        }
        
        /// <summary>
        /// Categorie: albero
        /// </summary>
        /// <returns></returns>
        public static String CategoryListTree(int CommunityId)
        {
            if (CommunityId > 0)
                return basePath + "CategoriesTree.aspx" + "?CommunityId=" + CommunityId.ToString();
            return basePath + "CategoriesTree.aspx";
        }

        /// <summary>
        /// Categorie: lista basata sugli utenti
        /// </summary>
        /// <returns></returns>
        public static String CategoryListByUsers(int CommunityId)
        {
            if (CommunityId > 0)
                return basePath + "CategoriesUsers.aspx" + "?CommunityId=" + CommunityId.ToString();
            return basePath + "CategoriesUsers.aspx";
        }

        /// <summary>
        /// Categorie: modifica
        /// </summary>
        /// <returns></returns>
        
        public static String CategoryModify(int CommunityId, Int64 CategoryId)
        {
            if (CommunityId > 0)
                return basePath + "Category.aspx?Id=" + CategoryId.ToString() + "&CommunityId=" + CommunityId.ToString();
            else return basePath + "Category.aspx?Id=" + CategoryId.ToString();
        }
        //public static String CategoryModify(Int64 CategoryId)
        //{
        //    return basePath + "Category.aspx?Id=" + CategoryId.ToString();
        //}

        /// <summary>
        /// Categorie: aggiungi nuova
        /// </summary>
        /// <returns></returns>
        public static String CategoryAdd(int CommunityId)
        {
            if (CommunityId > 0)
                return basePath + "CategoryAdd.aspx" + "?CommunityId=" + CommunityId.ToString();
            else return basePath + "CategoryAdd.aspx";
        }
        //public static String CategoryAdd()
        //{
        //    return basePath + "CategoryAdd.aspx";
        //}
#endregion

        #region "Ticket"

        /// <summary>
        /// Ticket: apertura
        /// </summary>
        /// <returns></returns>
        public static String TicketAdd(int CommunityId)
        {
            if (CommunityId > 0)
                return basePath + "Add.aspx" + "?CommunityId=" + CommunityId.ToString();

            return basePath + "Add.aspx";
        }
        //public static String TicketAdd(int CommunityId)
        //{
        //    return basePath + "Add.aspx";
        //}


        //Todo: test & Check
        /// <summary>
        /// Ticket: modifica DRAFT
        /// </summary>
        /// <returns></returns>
        public static String TicketAdd(int CommunityId, String TicketCode)
        {
            String URL = basePath + "Add.aspx";

            if (CommunityId > 0)
            {
                URL += "?CommunityId=" + CommunityId.ToString();
                if (!String.IsNullOrEmpty(TicketCode))
                    URL += "&Id=" + TicketCode;
            }
            else if (!String.IsNullOrEmpty(TicketCode))
                URL += "?Id=" + TicketCode;

            return URL;
        }

        public static String TicketAdd(int CommunityId, String TicketCode, bool isNew)
        {
            String URL = basePath + "Add.aspx";

            if (CommunityId > 0)
            {
                URL += "?CommunityId=" + CommunityId.ToString();
                if (!String.IsNullOrEmpty(TicketCode))
                    URL += "&Id=" + TicketCode;
            }
            else if (!String.IsNullOrEmpty(TicketCode))
                URL += "?Id=" + TicketCode;

            if (isNew)
            {
                URL += "&n=1";
            }

            return URL;
        }


        //ToDo: check & test
        /// <summary>
        /// Ticket: modifica DRAFT, SENZA conferma in caso di creazione nuovo Ticket.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Si presume che CHI utilizza questo URL, richieda conferma all'utente per la creazione di un nuovo Ticket. Questo non tocca tutti gli altri controlli e meccaniche già presenti.
        /// </remarks>
        public static String TicketAddFromList(int CommunityId, String TicketCode)
        {
            String URL = basePath + "Add.aspx";

            URL += "?FL=1";
            
            if (CommunityId > 0)
            {
                URL += "&CommunityId=" + CommunityId.ToString();
            }

            if (!String.IsNullOrEmpty(TicketCode) && TicketCode != "0")
                URL += "&Id=" + TicketCode;
            
            return URL;
        }

        /// <summary>
        /// Ticket: lista utente
        /// </summary>
        /// <returns></returns>
        public static String TicketListUser(int CommunityId)
        {
            if (CommunityId > 0)
                return basePath + "ListUser.aspx" + "?CommunityId=" + CommunityId.ToString();

            return basePath + "ListUser.aspx";
        }

        public static String TicketListUser(int CommunityId, bool DelSuccess)
        {
            String URL = basePath + "ListUser.aspx";

            if (DelSuccess)
            {
                if (CommunityId > 0)
                    URL += "?ds=1" + "&CommunityId=" + CommunityId.ToString();
                else
                    URL += "?ds=1";
            }
            else if (CommunityId > 0)
                URL += "?CommunityId=" + CommunityId.ToString();
            
            return URL;
        }

        public static String TicketListExternal(bool DelSuccess)
        {
            String URL = basePath + "ListExternal.aspx";

            if (DelSuccess)
            {
                URL += "?ds=1";
            }
            
            return URL;
        }

        //public static String TicketListUser()
        //{
        //    return basePath + "ListUser.aspx";
        //}

        //ToDo: - Check dati
        //ToDo: - test
        /// <summary>
        /// Ticket: modifica
        /// </summary>
        /// <returns></returns>
        public static String TicketEditUser(int CommunityId, String TicketCode)
        {
            if (CommunityId > 0)
                return basePath + "EditUser.aspx?Id=" + TicketCode + "&CommunityId=" + CommunityId.ToString();

            return basePath + "EditUser.aspx?Id=" + TicketCode;
        }
        //public static String TicketEditUser(int CommunityId, String TicketCode)
        //{
        //    if (CommunityId > 0)
        //        return basePath + "EditUser.aspx?Id=" + TicketCode + "&CommunityId=" + CommunityId.ToString();

        //    return basePath + "EditUser.aspx?Id=" + TicketCode;
        //}
        
        //public static String TicketEditUser(Int64 TicketId)
        //{
        //    return basePath + "EditUser.aspx?Id=" + TicketId.ToString();
        //}

        /// <summary>
        /// Ticket: lista Resolver
        /// </summary>
        /// <returns></returns>
        public static String TicketListResolver(int CommunityId)
        {
            if (CommunityId > 0)
                return basePath + "ListResolver.aspx" + "?CommunityId=" + CommunityId.ToString();

            return basePath + "ListResolver.aspx";
        }

        public static String TicketListResolver(int CommunityId, Int64 CategoryId)
        {
            if (CommunityId > 0 && CategoryId > 0)
                return string.Format("{0}ListResolverListResolver.aspx?CommunityId={1}&CatId={2}", basePath, CommunityId,
                    CategoryId);

            if(CommunityId > 0)
                return string.Format("{0}ListResolverListResolver.aspx?CommunityId={1}", basePath, CommunityId);
            
            return basePath + "ListResolver.aspx";
        }
        //public static String TicketListResolver()
        //{
        //    return basePath + "ListResolver.aspx";
        //}

        //ToDo: - Check dati
        //ToDo: - test
        /// <summary>
        /// Ticket: modifica DRAFT
        /// </summary>
        /// <returns></returns>
        public static String TicketEditResolver(int CommunityId, String TicketCode)
        {
            if (CommunityId > 0)
                return basePath + "EditResolver.aspx?Id=" + TicketCode + "&CommunityId=" + CommunityId.ToString();

            return basePath + "EditResolver.aspx?Id=" + TicketCode;
        }
        //public static String TicketEditResolver(Int64 TicketId)
        //{
        //    return basePath + "EditResolver.aspx?Id=" + TicketId.ToString();
        //}
        #endregion

#region "Generiche"
        /// <summary>
        /// Generic: Amminsitrazione globale
        /// </summary>
        /// <returns></returns>
        public static String SettingsGlobal()
        {
            return basePath + "GlobalAdmin.aspx";
        }

        /// <summary>
        /// Generico: Impostazioni utente (notifiche)
        /// </summary>
        /// <returns></returns>
        public static String SettingsUser(int CommunityId)
        {
            if (CommunityId > 0)
                return basePath + "UserSettings.aspx" + "?CommunityId=" + CommunityId.ToString();
            return basePath + "UserSettings.aspx";
        }
#endregion

#region External

        public static String ExternalLogin()
        {
            return basePath + "AccessExternal.aspx";
        }

        public static String ExternalLogin(String Token)
        {
            return ExternalLogin() + "?Tk=" + Token;
        }

        public static String ExternalList()
        {
            return basePath + "ListExternal.aspx";
        }


        //ToDo: - Check dati
        //ToDo: - test
        public static String ExternalEdit(String TicketCode)
        {
            if (String.IsNullOrEmpty(TicketCode))
                return ExternalList();

            return basePath + "EditExternal.aspx?TkId=" + TicketCode;
        }

        public static String ExternalAdd()
        {
            return basePath + "AddExternal.aspx";
        }

        //ToDo: - Check dati
        //ToDo: - test
        public static String ExternalAdd(String TicketCode)
        {
            if (!String.IsNullOrEmpty(TicketCode))
                return basePath + "AddExternal.aspx?Id=" + TicketCode;
            
            return ExternalAdd();
        }

        public static String ExternalUserSettings()
        {
            return basePath + "UserSEttingsExternal.aspx";
        }
#endregion

        public static Int64 GetUserId(string userCode)
        {
            if (userCode.Length != 14)
                return -1;

            //return string.Format("U{0:00000000}-{1:0000}", Id, (Person == null) ? 0 : Person.Id);

            var regex = new Regex("U\\d{8}-\\d{4}");
            
            if (!regex.IsMatch(userCode))
            return -1;

            String idStr = userCode.Remove(0, 1).Remove(8, 5);
            Int64 id = 0;

            try
            {
                id = System.Convert.ToInt64(idStr);
            }
            catch (Exception)
            {
                return -1;
            }

            return id;
        }

        public static Int64 GetTicketId(string ticketCode)
        {
            //if (userCode.Length != 14)
            //    return -1;

            //return string.Format("TK{0}-{1}", this.OpenOn.ToString("yyyyMMdd"), Id);

            if (string.IsNullOrEmpty(ticketCode))
                return -1;

            var regex = new Regex("TK\\d{8}-\\d*");
            
            if (!regex.IsMatch(ticketCode))
                return -1;

            String idStr = ticketCode.Remove(0, 11);
            Int64 id = -1;

            try
            {
                id = System.Convert.ToInt64(idStr);
            }
            catch (Exception)
            {
                return -1;
            }

            return id;
        }
    }
}