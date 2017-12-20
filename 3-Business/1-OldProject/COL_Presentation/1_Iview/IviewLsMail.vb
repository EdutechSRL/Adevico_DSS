''' <summary>
''' Vista generica dell'UC che contiene gli altri USER CONTROL per la gestione delle liste di distribuzione
''' </summary>
''' <remarks></remarks>
Public Interface IviewLsMail
    Inherits IviewGeneric

    ''' <summary>
    ''' Fa il bind delle liste di "liste di distribuzione"
    ''' </summary>
    ''' <param name="Items"></param>
    ''' <remarks></remarks>
    Sub BindListe(ByVal Items As System.Collections.Generic.List(Of MailingList))

	''' <summary>
	''' Recupero dati servizio
	''' </summary>
	''' <value></value>
	''' <remarks></remarks>
	ReadOnly Property Servizio() As COL_BusinessLogic_v2.UCServices.Services_Mail


    ''' <summary>
    ''' Oggetto che poi viene vagliato e spedito dal presenter
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property oMail() As COL_E_Mail

    ''' <summary>
    ''' Per la sola visualizzazione del mittente
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    WriteOnly Property Mittente() As String

    ''' <summary>
    ''' Per visualizzare i vari UC a seconda della necessità
    ''' </summary>
    ''' <param name="Id"></param>
    ''' <remarks></remarks>
    Sub Show(ByVal Id As Panel)

    ''' <summary>
    ''' Enumera gli elementi da visualizzare
    ''' </summary>
    ''' <remarks></remarks>
    Enum Panel
        NoPermessi = 0
        InvioMail = 1
        GestioneListe = 2
        MessageOK = 3
        MessageError = 4
    End Enum

    ''' <summary>
    ''' Bind della lista di file che verranno caricati come attachment della mail
    ''' </summary>
    ''' <param name="mostraErrore"></param>
    ''' <param name="Bind"></param>
    ''' <param name="StrErrore"></param>
    ''' <remarks></remarks>
    Sub LoadAttachments(Optional ByVal mostraErrore As Boolean = True, Optional ByVal Bind As Boolean = False, Optional ByVal StrErrore As String = "")
    Sub ShowNoDestinatari(ByVal Show As Boolean)

End Interface
