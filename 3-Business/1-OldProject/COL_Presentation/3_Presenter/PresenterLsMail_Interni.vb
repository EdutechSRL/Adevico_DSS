Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports System.Collections.Generic

Public Class PresenterLsMail_Interni
    Inherits GenericPresenter
    Const MinChar As Integer = 2

	Public Sub New(ByVal view As IViewCommon)
		MyBase.view = view
	End Sub
    Private Shadows ReadOnly Property View() As IviewLsMail_interni
        Get
            View = MyBase.view
        End Get
    End Property
    Public Sub Initialize()

    End Sub


    ''' <summary>
    '''  Funzione per il filtraggio dei dati tramite mail pubblica
    ''' </summary>
    ''' <param name="item"></param>
    ''' <param name="argument"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''   Utilizzare nella solita modalità per il filtraggio delle liste
    ''' </remarks>
    Private Shared Function IscrittiFiltraMailPubblica(ByVal item As Iscritto, ByVal argument As Boolean) As Boolean
        Return item.RLPC_PRSN_mostraMail
    End Function

    ''' <summary>
    '''   Funzione per il filtraggio dei dati tramite mail pubblica
    ''' </summary>
    ''' <param name="item"></param>
    ''' <param name="argument"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''   Utilizzare nella solita modalità per il filtraggio delle liste
    ''' </remarks>
    Private Shared Function IscrittiFiltraMail(ByVal item As Iscritto, ByVal argument As Boolean) As Boolean
        Return item.Persona.MostraMail
    End Function

    ''' <summary>
    ''' Aggiunge un nuovo utente INTERNO alla lista
    ''' </summary>
    ''' <param name="PersonaId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddInterno(ByVal PersonaId As Integer) As Boolean
        Dim oAddress As New MailingAddress()
        oAddress.PersonaID = PersonaId
        oAddress.IdLista = Me.View.AddressId
        Return oAddress.SaveNew()
    End Function

#Region "Algoritmi di ricerca"
    ''' <summary>
    '''  Effettua la ricerca, impostando poi la lista nella view
    ''' </summary>
    ''' <param name="ComunitaId">Stringa con l'elenco degli ID comunità.
    '''  Devono essere separati da virgola. Es: ,1, o ,1,2,3,</param>
    ''' <param name="Nome">Parte del nome da ricercare</param>
    ''' <param name="Cognome">Parte del cognome da ricercare</param>
    ''' <param name="Matricola">Parte della matricola da ricercare</param>
    ''' <param name="Mail">Parte della mail da ricercare</param>
    ''' <param name="Login">Parte della mail da ricercare</param>
    ''' <param name="TipoPersonaID">Id del tipo persona</param>
    ''' <param name="SortExpr">Campo su cui effettuare l'ordinamento</param>
    ''' <param name="SortDir">Direzione dell'ordinamento</param>
    ''' <remarks></remarks>
    Public Sub Cerca( _
        Optional ByVal ComunitaId As Integer = -1, _
        Optional ByVal Nome As String = "", _
        Optional ByVal Cognome As String = "", _
        Optional ByVal Matricola As String = "", _
        Optional ByVal Mail As String = "", _
        Optional ByVal Login As String = "", _
        Optional ByVal TipoPersonaID As Integer = -1, _
        Optional ByVal SortExpr As String = "", _
        Optional ByVal SortDir As String = "asc")

        Me.View.SaveCurrent()

        Dim CanSearch As Boolean = False

        If Nome.Length >= MinChar Then
            CanSearch = True
        ElseIf Cognome.Length >= MinChar Then
            CanSearch = True
        ElseIf Matricola.Length >= MinChar Then
            CanSearch = True
        ElseIf Mail.Length >= MinChar Then
            CanSearch = True
        ElseIf Login.Length >= MinChar Then
            CanSearch = True
        End If

        If Not CanSearch Then
            Me.View.ShowNoParameter()
            Exit Sub
        End If

        Dim Totale As Integer
        Dim oIscritti As New System.Collections.Generic.List(Of Iscritto)

        '1. Cerco l'elenco con gli ID delle comunità a cui sono iscritto
        Dim ElencoID As String
        If ComunitaId < 0 Then
            ElencoID = COL_Comunita.ElencaListaID_ForServizio(Me.View.LinguaID, COL_BusinessLogic_v2.UCServices.Services_Mail.Codex, Me.View.UtenteCorrente.Id, FiltroStatoComunita.Tutte)
        Else
            ElencoID = "," & ComunitaId & ","
        End If

        '2. Recupero l'elenco degli iscritti
        oIscritti = COL_Comunita.GetAllIscrittiComunitaMultipleFiltrate(ElencoID, Me.View.LinguaID, Nome, Cognome, Matricola, Mail, Login, TipoPersonaID)

        '3. Eventualmente filtro per TipoPersona (no da DB)
        Totale = oIscritti.Count

        '4. Ordino. NOTA: siccome persona è una proprietà di iscritti,
        '                 per casini con la gridview eseguo una conversione...
        '                 Non è elegante e non ha molto modo di esistere, 
        '                 a tempo debito verrà tolto.
        Dim oPersone As New List(Of COL_Persona)
        oPersone = Me.ConvertIscrittiToPersona(oIscritti)

        If (Not SortExpr Is Nothing AndAlso SortExpr <> String.Empty) Then
            oPersone.Sort(New GenericComparer(Of COL_Persona)(SortExpr))
        End If

        If (Not SortDir Is Nothing AndAlso SortDir = "desc") Then
            oPersone.Reverse()
        End If

        '5. Imposto la griglia
        Me.View.SetLista(oPersone)

        If Totale > 0 Then
            Me.View.Showgriglia()
        Else
            Me.View.ShowNoItem()
        End If

    End Sub

    ''' <summary>
    '''  Dato un elenco di "iscritto", restituisce un elenco di oggetti "persona".
    ''' </summary>
    ''' <param name="oIscritti"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Nei filtri ho bisogno di alcuni dati contenuti in "iscritto", ma per comodità di datagrid (ordinamento, paginazione, etc...) ho bisogno di un elenco di persona... (vedi Iscritto.persona.nome)
    ''' </remarks>
    Private Function ConvertIscrittiToPersona(ByVal oIscritti As System.Collections.Generic.List(Of Iscritto)) As List(Of COL_BusinessLogic_v2.CL_persona.COL_Persona)
        Dim oListPersone As New List(Of COL_BusinessLogic_v2.CL_persona.COL_Persona)
        For Each item As Iscritto In oIscritti
            oListPersone.Add(item.Persona)
        Next
        Return oListPersone
    End Function
#End Region
End Class