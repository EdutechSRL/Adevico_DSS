using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced.Helpers
{
    /// <summary>
    /// Helper esportazione dati commissione.
    /// Trasforma i dati della commissione in un HTML per la successiva trasformazione in documenti (PDF, rtf, etc...)
    /// Modificando le stringhe dei formati è possibile modificare l'HTML prodotto oppure ottenere esportazioni di altro tipo.
    /// </summary>
    public class CommissionHTMLExpHelper
    {
        /// <summary>
        /// 0: Nome campo
        /// 1: valore campo
        /// </summary>
        public String CommissionItemFormat = "<i>{0}</i><b>{1}</b><br/><br/>";
        
        /// <summary>
        /// 0: Nome commissione
        /// 1: Presidente commissione
        /// 2: Segretario commissione
        /// 3: Membri commissione
        /// </summary>
        public String CommissionDataFormat = "{0}{1}{2}{3}";

        /// <summary>
        /// 0 : nome membro commissione
        /// </summary>
        public String CommissionMemberFormat = "<li><b>{0}</b></li>";

        /// <summary>
        /// 0: contenitore membri
        /// </summary>
        public String CommissionMemberContainerFormat = "<ul>{0}</ul>";

        /// <summary>
        /// Localizzazione: nome commissione
        /// </summary>
        public String CommissionName = "Nome commissione: ";
        /// <summary>
        /// Localizzazione: presidente
        /// </summary>
        public String CommissionPresident = "Presidente: ";
        /// <summary>
        /// Localizzazione: segretario
        /// </summary>
        public String CommissionSecretary = "Segretario: ";
        /// <summary>
        /// Localizzazione: membri
        /// </summary>
        public String CommissionMember = "Membri: ";

        /// <summary>
        /// Costruttore vuoto
        /// </summary>
        public CommissionHTMLExpHelper() { }

        /// <summary>
        /// Change default format for items
        /// </summary>
        /// <param name="Data">
        /// "{0}{1}{2}{3}{4}"
        /// 0: Nome commissione
        /// 1: Presidente commissione
        /// 2: Segretario commissione
        /// 3: Membri commissione
        /// </param>
        /// <param name="Item">
        /// "<i>{0}</i><b>{1}</b><br/>"
        /// 0: Nome campo
        /// 1: valore campo
        /// </param>
        /// <param name="Member">
        /// "<li><b>{0}</b></li>"
        /// 0 : nome membro commissione
        /// </param>
        /// <param name="memberContainer">
        /// "<ul>{0}</ul>"
        /// 0: contenitore membri
        /// </param>
        public void SetFormats(
            string Data,
            string Item,
            string Member,
            string memberContainer
            )
        {
            CommissionDataFormat = Data;
            CommissionItemFormat = Item;
            CommissionMemberFormat = Member;
        }

        /// <summary>
        /// Modifica le etichette di localizzazione
        /// </summary>
        /// <param name="Commission">Commissione</param>
        /// <param name="President">Presidente</param>
        /// <param name="Secretary">Segretario</param>
        /// <param name="Members">Memebri</param>
        public void SetLabels(
            string Commission,
            string President,
            string Secretary,
            string Members
            )
        {
            CommissionName = Commission;
            CommissionPresident = President;
            CommissionSecretary = Secretary;
            CommissionMember = Members;
        }

        /// <summary>
        /// Trasforma l'oggetto di dominio "commissione" in stringa da esportare
        /// </summary>
        /// <param name="commission">Oggetto commissione</param>
        /// <returns>HTML completo</returns>
        public String CommissionData(Advanced.Domain.AdvCommission commission)
        {

            string HTML = String.Format(
                CommissionDataFormat,
                String.Format(CommissionItemFormat,
                    CommissionName,
                    commission.Name
                    ),
                String.Format(CommissionItemFormat,
                    CommissionPresident,
                    (commission.President != null) ? commission.President.SurnameAndName : "Uknow"
                    ),
                String.Format(CommissionItemFormat,
                    CommissionSecretary,
                    (commission.Secretary != null) ? commission.Secretary.SurnameAndName : "Uknow"
                    ),
                String.Format(CommissionItemFormat,
                   CommissionMember,
                    CommissionMembersGet(commission)
                    )
                );
            
            return HTML;

        }

        /// <summary>
        /// Recupera l'HTML dell'elenco dei membri
        /// </summary>
        /// <param name="commission">Oggetto di dominio della commissione</param>
        /// <returns></returns>
        private String CommissionMembersGet(Advanced.Domain.AdvCommission commission)
        {
            string HTML = "";

            if(commission.Members != null && commission.Members.Any())
            {
                foreach(Domain.AdvMember mem in commission.Members)
                {
                    if (mem != null && mem.Member != null)
                    {
                        HTML = String.Format("{0}{1}", HTML,
                        String.Format(CommissionMemberFormat,
                            mem.Member.SurnameAndName
                            ));
                    }
                        
                }
            }
            
            return String.Format(CommissionMemberContainerFormat,
                HTML);
        }
    }
}
