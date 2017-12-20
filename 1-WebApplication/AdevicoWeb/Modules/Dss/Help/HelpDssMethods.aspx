<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ExternalService.Master" CodeBehind="HelpDssMethods.aspx.vb" Inherits="Comunita_OnLine.HelpDssMethods" %>

<%@ MasterType VirtualPath="~/ExternalService.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        ul li {
            list-style-type: circle;
        }
        ul li ul li {
            list-style-type: circle;
        }
         .imagecaption{
            text-align: center;
         }
         .imagecaption  img{
                border-width: 1px;
             border-style: solid;
         }
         .noborder  img{
             border-style:none;
         }
        .imagecaption h4{
           font-weight: bolder;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPHservice" runat="server">
    <h1>Utilizzo DSS - Servizio Bandi</h1>
    <div>
        <h2>Impostazioni generali</h2>
        <div>
            <p>All'interno delle impostazioni generali bandi è possibile definire quale sistema di valutazione utilizzare:</p>
            <ol>
                <li>somma dei valori;</li>
                <li>media dei valori;</li>
                <li>utilizzo algoritmi DSS;</li>
            </ol>
            <p>
                Prima di creare una commissione è possibile modificare il sistema di valutazione, successivamente vi sono delle limitazioni:
            </p>
            <ul>
                <li><b>somma/media:</b> posso passare dall'una all'altra ma NON all'utilizzo di algoritmi DSS;</li>
                <li><b>algoritmi DSS:</b>non posso passare alla somma/media valori fino a che non ho cancellato tutte le commissioni del bando;</li>
            </ul>
        </div>
    </div>
    <div>
        <h2>Algoritmi disponibili</h2>
        <div>
            <p>Gli algoritmi DSS disponibili per il servizo bandi possono essere suddivisi in queste tre macro-categorie:</p>
            <ul>
                <li>Algoritmi basati su numeri reali:
                <ul>
                    <li>Media pesata (Weighted average);</li>
                    <li>Media pesata ordinata (OWA);</li>
                    <li>Topsis;</li>
                </ul>
                </li>
                <li>Algoritmi basati su numeri fuzzy triangolari:
                <ul>
                    <li>Media pesata fuzzy (Fuzzy Weighted Average);</li>
                    <li>Media pesata ordinata fuzzy (Fuzzy Owa);</li>
                    <li>Topsis fuzzy: sfrutta criteri fuzzy ma i risultati finali sono espressi sempre in un numero crisp/reale compreso tra zero ed uno. Come nel Topsis crisp/reale, anche per Topsis Fuzzy la somma dei risultati finali deve dare sempre uno.</li>
                </ul>
                </li>
            </ul>
        </div>
    </div>
    <div>
        <h2>Impostazioni commissione</h2>
        <p>
            Dopo aver creato una commissione il sistema propone all'utente di selezionare l'algoritmo di aggregazione dei criteri.
            Ovviamente affinchè l'aggregazione dei criteri abbia un senso è necessario per alcuni algoritmi (es. OWA) che in una commissione vi siano almeno due criteri di valutazione.
        </p>
        <p>
             Quando posso definire i pesi dei criteri all'interno della commissione ? Ovviamente dopo averli creati !
        </p>
    </div>
    <div>
        <h2>Criteri</h2>
        <div>
            <h3>Creazione criteri</h3>
            <p>
                L'interfaccia di creazione dei criteri è analoga a quella dei criteri utilizzati per le valutazioni
            </p>
            <p>In base all'algoritmo selezionato si potranno trovare diverse tipologie di criteri:</p>
            <ul>
                <li>Criteri quantitativi su numeri interi: disponibili per tutti gli algoritmi;</li>
                <li>Criteri quantitativi su numeri decimali: disponibili per tutti gli algoritmi;</li>
                <li>Scala qualitativa NON fuzzy: disponibile per tutti gli algoritmi NON fuzzy</li>
                <li>Scala qualitativa Fuzzy: disponibile solo per gli algoritmi fuzzy</li>
            </ul>
        </div>
        <div>
            <h3>Impostazioni criteri</h3>
            <p>
                Il metodo di impostazione dei criteri è strettamente correlato al sistema di aggregazione selezionato.
                E' necesario definire i "pesi" in modo diverso per ciascun algoritmo, di conseguenza il supporto di un esperto di dominio
                risulta indispensabile per ottenere i risultati desiderati.
            </p>
            <div>
                <h4><b>Media pesata (Weighted average) / Media pesata Fuzzy (Fuzzy Weighted average)</b></h4>
                <p>
                    Nel caso della media pesata è necessario definire manualmente il peso di ciascun criterio, il sistema provvede a segnalare all'utente la modalità corretta di inserimento dati:
                    <ul>
                        <li>Numeri decimali con il carattere <b>.</b> come separatore della parte intera da quella decimale;</li>
                        <li>Numeri fuzzy triangolari con con il carattere <b>;</b> come separatore dei singoli elementi del numero fuzzy;</li>
                    </ul>
                    Per maggiori dettagli esaminare il manuale pdf allegato, il supporto di un esperto di dominio è indispensabile.
                </p>

            </div>
            <div>
                <h4><b>Media pesata ordinata (OWA) / Media pesata ordinata Fuzzy (Fuzzy OWA)</b></h4>
                <p>
                    Nel caso della media pesata ordinata l'inserimento dei pesi è simile a quello descritto nel caso precedente. Posso inserire dei numeri decimali o dei numeri fuzzy a seconda che l'algoritmo 
                    di media pesata ordinata sia o meno fuzzy.
                    Nel caso della media pesata ordinata l'interfaccia di inserimento dati si adatta però alle caratteristiche dell'algoritmo selezionato, in particolare si consente 
                    l'inserimento dei pesi da dare:
                    <ul>
                        <li>al criterio che otterrà il valore più alto;</li>
                        <li>al criterio che otterrà il valore più basso;</li>
                        <li>ai criteri n-2 che otterranno una valutazione più bassa di quello più alto;</li>
                    </ul>
                    Per maggiori dettagli esaminare il manuale pdf allegato, il supporto di un esperto di dominio è indispensabile.
                </p>
            </div>
            <div>
                <h4><b>Topsis / Fuzzy Topsis</b></h4>
                <p>
                    Nel caso di utilizzo di Topsis l'assegnazione dei pesi dei singoli criteri deve essere fatta in maniera puntuale.
                </p>
                <p>
                    Selezionando, ad esempio, la scala internazionalie "<i>5  point linguistic scale Yang and Hung</i>" l'utente dovrà indicare manualmente il peso di ciascun
                    criterio definito per la commissione.
                </p>
            </div>
        </div>
    </div>
    <div>
        <h2>Multi commissione</h2>
          <p>
            Cosa succede se ho più di una commissione ?
        </p>
        <p>
            Lo schema di funzionamento è analogo a quanto avviene per i criteri di una commissione, ossia:
        </p>
        <ol>
            <li>Creo le commissioni che desidero;</li>
            <li>Seleziono l'algoritmo di aggregazione dei risultati più consono (algoritmo individuato da esperto di dominio);</li>
            <li>Definisco i pesi delle singole commissioni in base all'algoritmo selezionato in modo analogo con quanto fatto a livello dei criteri;</li>
        </ol>
        <p>Ecco alcuni esempi di un bando con due commissioni di valutazione e diversi algoritmi di aggregazione:</p>
        <div>
             <h3>Media pesata (Weighted average) / Media pesata Fuzzy (Fuzzy Weighted average)</h3>
            <p>
                 In modo analogo con i criteri all'interno di una commissione deve essere specificato il peso di ogni singola commissione espresso in numeri decimali (se si utilizza la media pesata standard)
                o in numeri fuzzy (se si usa la media pesata fuzzy).
             </p>
        </div>
         <div>
             <h3>Media pesata ordinata (OWA) / Media pesata ordinata Fuzzy (Fuzzy OWA)</h3>
             <p>
                 In modo analogo con i criteri all'interno di una commissione deve essere specificato il peso de:
                <ul>
                    <li>la commissione che genererà la valutazione più alta;</li>
                    <li>la commissione che genererà il valutazione più bassa;</li>
                    <li>le eventuali altre n-2 commissioni che genereranno una valutazione compresa tra la più alta e la più bassa;</li>
                </ul>
             </p>
        </div>   
         <div>
             <h3>Topsis / Fuzzy Topsis</h3>
             <p>
                 Anche in questo caso, in modo analogo con quanto avviene con i criteri all'interno di una commissione, è necessario definire per ciascuna
                 commissione il proprio peso all'interno del sistema di aggregazione.
             </p>
        </div>   
    </div>
    <div>
        <h2>Valutazione domanda</h2>
        <p>
            Dal punto di vista della valutazione il sistema funziona in modo del tutto analogo con quanto avviene con i sistemi di valutazione tradizionali.
        </p>
        <p>
            Il valutatore per ciascuna domanda sottomessa ha la possibilità di valutare i vari criteri definiti a livello di commissione.
            Nel caso di critery fuzzy è ovvio che verrà mostrata all'utente l'interfaccia ad-hoc per l'inserimento dei valori.
        </p>
        <p>
            Nel caso di valutazione "fuzzy" anche il valutatore deve essere consapevole del significato della propria valutazione in particolar modo
            nel casi in cui decida di utilizzare una valutazione "intermedia" piuttosto che una ben precisa. 
        </p>
    </div>
    <div>
        <h2>Visualizzazione valutazioni</h2>
        <div>
            <h3>Premessa</h3>
            <p>
                Nell'affrontare l'argomento relativo alla visualizzazione delle valutazioni delle domande di un bando con commissioni e criteri gestiti tramite i DSS
                va fatta questa <b>importante premessa</b>:
                molti degli algoritmi utilizzati danno valori significativi <b>solo</b> quando <b>l'intero quadro valutativo è al completo</b>.
            </p>
            <p>
                In particolare:
            </p>
            <ul>
                <li><b>Nessuno degli algoritmo</b> consente di avere dei punteggi finali senza che tutte le domande siano state valutate da tutti i valutatori di ciascuna commissione;</li>
                <li><b>Solo la media pesata (standard o fuzzy)</b> consente al singolo valutatore <b>vedere il punteggio ottenuto</b> da una singola domanda, <b>a patto che la valutazione dello stesso sia ovviamente stata data in sua interezza</b>  (ogni criterio è stato opportunamente valutato dal valutatore);</li>
                <li><b>Tutti gli algoritmi</b> consentono ad un valutatore di vedere il punteggio ottenuto dalle singole domande in una commissione <b>a patto di aver valutato nella loro interezza tutte le domande a lui associate</b> in quella commissione;</li>
            </ul>
        </div>
        <div>
            <h3>Visualizzazione valutazioni del singolo valutatore</h3>
            <p>
                Ogni valutatore ha una propria pagina di riepilogo delle valutazioni assegnate suddivise per commissione di appartenenza.
                Come indicato in precedenza in base all'algoritmo il valutatore potrà vedere o meno una sorta di classifica parziale delle domande valutate.
            </p>
            <p>
                Se, ad esempio, un valutatore ha assegnate 10 domande da valutare e ne ha almeno una incompleta allora proprio la domanda incompleta <b>NON gli consentirà</b>
                di vedere la classifica parziale delle domande di partecipazione al bando riferita alle <b>proprie valutazioni</b>.
            </p>
            <p>
                In caso contrario, tutte le domande sono state valutate, il valutatore potrà vedere la sua classifica parziale che sarà puramente indicativa
                perchè priva di qualisasi significato finchè anche gli altri commissari non avranno completato le loro valutazioni.
            </p>
        </div>
        <div>
            <h3>Visualizzazione complessiva</h3>
            <p>
                La visualizzazione complessiva dei risultati di una procedura di valutazione è strettamente correlata con il sistema di valutazione adottato, di conseguenza
                con l'utilizzo degli algoritmi DSS sarà sempre mostrato all'utente un messaggio in cui vengono indicati i riferimenti orari dell'ultimo aggiornamento e l'eventuale indicazione
                relativa alla visuallizzazione di dati puramente indicativi o definitivi.
            </p>
            <p>
                Cosa accadrà se qualcuno cercherà di visionare lo stato dell'arte delle valutazioni (ad esempio cercando di vedere le valutazioni per una specifica commissione) ?
                Il sistema provvederà a far vedere per ciascuna commissione i soli dati "definitivi"
            </p>
             <p>
                 Ad esempio in una commissione con tre valutatori ("S.", "R.", "A.") potrebbe accadere che:
                 <ul>
                     <li>solo il valutare "S." abbia concluso la valutazione delle due domande di partecipazione, solo il suo risultato parziale sarà quindi visibile;</li>
                     <li>il valutatore "R." abbia iniziato le valutazion, ma non concluse. Non sarà possibile vederne il risultato;</li>
                     <li>il valutatore "A." NON abbia iniziato le valutazioni. Non sarà possibile vederne il risultato;</li>
                 </ul>
                 In nessun caso è possibile fornire una classifica parziale essendovi ancora domande parzialmente valutate.
            </p>
            <p>
               Nel caso in cui tutte le valutazioni siano state completate si potrà visualizzare il dettaglio dell'intero processo di valutazione in modo analogo con quanto avviene 
                con gli altri sistemi di valutazione.
            </p>
        </div>
    </div>
</asp:Content>