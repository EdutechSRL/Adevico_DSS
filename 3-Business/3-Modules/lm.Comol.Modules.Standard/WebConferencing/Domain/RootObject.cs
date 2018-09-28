using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain
{
    /// <summary>
    /// Navigazione: gestione creazione url vari
    /// </summary>
    public static class RootObject
    {
        /// <summary>
        /// Cartella con le pagine del servizio
        /// </summary>
        private readonly static String basePath = "Modules/WebConferencing/";
    
        /// <summary>
        /// Pagina lista stanze
        /// </summary>
        /// <returns>URL Pagina lista stanze</returns>
        public static String List()
        {
            return basePath + "List.aspx";
        }

        /// <summary>
        /// Pagina lista stanze
        /// </summary>
        /// <param name="idCommunity">Comnuità specifica</param>
        /// <param name="idRoom">Stanza da visualizzare (anchor)</param>
        /// <returns>URL Pagina lista stanze</returns>
        public static String List(Int32 idCommunity, long idRoom=0)
        {
            return basePath + "List.aspx?idCommunity=" + idCommunity.ToString() + ((idRoom > 0) ? "#r_" + idRoom.ToString() : "");
        }

        /// <summary>
        /// Ancora lista stanza
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        /// <returns>HTML anchor stanza per lista</returns>
        public static String ListAnchor(Int64 RoomId)
        {
            //<a id=""r_" & room.Id.ToString() & """></a>
            return "<a id=\"r_" + RoomId.ToString() + "\"></a>";
        }
        
        /// <summary>
        /// Aggiungi stanza
        /// </summary>
        /// <param name="idCommunity">Comunità corrente</param>
        /// <returns>URL wizard creazione stanza</returns>
        public static String Add(Int32 idCommunity)
        {
            return basePath + "Add.aspx?idCommunity=" + idCommunity.ToString();
        }

        /// <summary>
        /// Edit stanza
        /// </summary>
        /// <param name="idRoom">Id stanza</param>
        /// <param name="idCommunity">Id Comunità</param>
        /// <returns>URL modifica stanza</returns>
        public static String Edit(long idRoom, Int32 idCommunity=-1)
        {
            return basePath + "Edit.aspx?Id=" + idRoom.ToString() + "&View=0" + ((idCommunity == -1) ? "" : "&idCommunity=" + idCommunity.ToString());
        }

        /// <summary>
        /// Edit stanza su tab particolare
        /// </summary>
        /// <param name="idRoom">Id Stanza</param>
        /// <param name="view">Tab da visualizzare</param>
        /// <param name="idCommunity">Id Comunità</param>
        /// <returns>URL modifica stanza</returns>
        public static String Edit(long idRoom, Domain.EditViews view,Int32 idCommunity = -1)
        {
            Int16 viewcode = (Int16)view;
            return basePath + "Edit.aspx?Id=" + idRoom.ToString() + "&View=" + viewcode.ToString() + ((idCommunity == -1) ? "" : "&idCommunity=" + idCommunity.ToString());
        }

        /// <summary>
        /// Pagina accesso INTERNO alla stanza
        /// </summary>
        /// <param name="idRoom">Id stanza</param>
        /// <returns>Url accesso stanza (solo interno)</returns>
        public static String Enter(long idRoom)
        {
            return basePath + "Access.aspx?Id=" + idRoom.ToString();
        }

        /// <summary>
        /// Pagina accesso ESTERNO alla stanza
        /// </summary>
        /// <param name="RoomCode">CODICE stanza</param>
        /// <returns>URL accesso ESTERNO stanza</returns>
        public static String ExternalAccess(string RoomCode)
        {
            return basePath + "ExternalAccess.aspx?Code=" + RoomCode;
        }

    }
}