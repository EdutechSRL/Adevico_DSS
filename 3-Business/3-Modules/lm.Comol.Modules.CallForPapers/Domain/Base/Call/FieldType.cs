using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    /// <summary>
    /// Tipo di campo
    /// </summary>
    [Serializable]
    public enum FieldType
    {
        /// <summary>
        /// Unused
        /// </summary>
        None = 0,
        /// <summary>
        /// Linea singola
        /// </summary>
        SingleLine = 1,
        /// <summary>
        /// Linea multipla
        /// </summary>
        MultiLine =2,
        /// <summary>
        /// Disclamer (sola lettura, accetta/rifiuta, personalizzato, etc...)
        /// </summary>
        Disclaimer =3,
        /// <summary>
        /// Indirizzo mail
        /// </summary>
        Mail = 4,
        /// <summary>
        /// Numero di telefono
        /// </summary>
        TelephoneNumber=5,
        /// <summary>
        /// Data
        /// </summary>
        Date = 6,
        /// <summary>
        /// Data ed ora
        /// </summary>
        DateTime=7,
        /// <summary>
        /// Ora
        /// </summary>
        Time =8,
        /// <summary>
        /// Codifce fiscale
        /// </summary>
        TaxCode= 9,
        /// <summary>
        /// CAP
        /// </summary>
        ZipCode = 10,
        /// <summary>
        /// RadiobuttonList: scelta singola, opzioni visibili
        /// </summary>
        RadioButtonList = 11,
        /// <summary>
        /// DropDown: scelta singola, menu a tendina
        /// </summary>
        DropDownList = 12,
        /// <summary>
        /// Scelta multipla
        /// </summary>
        CheckboxList = 13,
        /// <summary>
        /// Codice azienda (R.E.A.)
        /// </summary>
        CompanyCode = 14,
        /// <summary>
        /// Codice fiscale Azienda
        /// </summary>
        /// <remarks>
        /// Dal 2001 la partita iva ed il codice fiscale coincidono per i soggetti diversi dalle persone fisiche 
        /// (società)che al momento di iniziare l'attività rilevanti ai fini IVA non possiedevano già un codice fiscale.
        /// </remarks>
        CompanyTaxCode = 15,
        /// <summary>
        /// Partita iva, sia per professionisti che per imprese (da quanto dedotto via internet)
        /// </summary>
        VatCode = 16,
        /// <summary>
        /// Nome (anagrafica)
        /// </summary>
        Name = 17,
        /// <summary>
        /// Cognome (anagrafica)
        /// </summary>
        Surname = 18,
        /// <summary>
        /// Note
        /// </summary>
        Note = 19,
        /// <summary>
        /// Caricamento allegati (file)
        /// </summary>
        FileInput = 20,
        /// <summary>
        /// Tabella semplice: inserimento dati in forma tabellare
        /// </summary>
        TableSimple = 30,
        /// <summary>
        /// Tabella Report: inserimento dati in tabelle con campi quantità, prezzo unitario e riepiloghi (somme)
        /// </summary>
        /// <remarks>
        /// Diventano sorgente dati per successive approvazinoi del massimale erogabile e delle rendicontazioni
        /// </remarks>
        TableReport = 32,
        /// <summary>
        /// Tabella sommario.
        /// </summary>
        /// <remarks>
        /// NON salva dati, ma permette solamente dei riepiloghi visuali dei dati sottommessi nelle tabelle economiche.
        /// </remarks>
        TableSummary = 34
    }
}