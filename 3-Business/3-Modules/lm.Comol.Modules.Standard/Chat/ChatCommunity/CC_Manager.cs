using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ChatCommunity
{
    /// <summary>
    /// Chat manager.
    /// All method to manage a session (Session data, file, users, permission, messages, ecc...)
    /// </summary>
    public class CC_Manager
    {
        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            throw new NotImplementedException();
        }

        public void AddSession(CC_Session Session)
        {
            throw new NotImplementedException();
        }

        /*
         ALCUNE NOTE IMPLEMENTATIVE
         
         Valutare un sistema di cache che preveda di tenere in memoria le sessioni "attive".
         Allo start-up valutare se ci sono sessioni che vanno ripristinate ed eventualemente caricare in cache quelle.

         Quando un utente attiva la chat in una sessione, questa viene caricata in cache se non è già presente.
         
         Le modifiche ad una sessione vengono salvate non solo in cache, ma anche direttamente sul DB.
         
         La gestione dei messaggi, invece, risulta più complessa. I vari messaggi, infatti, dovranno rimanere in cache per un periodo limite,
         ma prima o poi DEVONO essere salvati su DB. Si prevedere quindi che i messaggi nuovi vengano inseriti nella sessione in memoria ED
         inviati ad una coda che man mano salva i dati su DB. Le sessioni in memoria dovranno avere un meccanismo per "svuotare" i messaggi più
         vecchi ed eventualmente recuperarli se necessario. Ad esempio un utente che entra in chat vedrà solo i messaggi "più recenti" e gli verrà
         mostrato un link per visualizzare anche i messaggi precedenti.
         Una chat "scaduta" o "bloccata" non manterrà i dati in cache, ma li recupera direttamente da database, inoltre per questo tipo di chat non è
         prevista l'aggiunta di nuovi messaggi. SOLO in tale modalità l'amministratore ha la possibilità di modificare/cancellare i vari messaggi.
         Tutto il meccanismo è comunque da valutare: (esempio se tenere comunque i messaggi originali visibili solo agli amministratori, se attivare la
         modifica SOLO in modalità "bloccata", etc...)...
         
         */
    }
}
