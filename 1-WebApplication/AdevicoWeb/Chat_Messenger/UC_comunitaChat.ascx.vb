Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports lm.Comol.Modules.Chat
Public Class UC_comunitaChat
    Inherits System.Web.UI.UserControl

    Protected oResource As ResourceManager
    Protected WithEvents LblComunita As System.Web.UI.WebControls.Label

    Dim oMessaggi As New WS_Chat.WS_ChatSoapClient
    Dim oNumCom As Integer

    'Serve per lanciare o meno il bind comunita'...
    Public ReadOnly Property NumCom() As Integer
        Get
            Return oNumCom
            'Return Me.LBComunita.Items.Count()
        End Get
    End Property

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LblComTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents BtnSelCom As System.Web.UI.WebControls.Button

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If
        If Not Page.IsPostBack Then ' = False
            Me.SetupInternazionalizzazione()
        End If
        Me.BindComunita()
    End Sub

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_Chat_Ext"
        oResource.Folder_Level1 = "Chat_Messenger"
        oResource.Folder_Level2 = "UC"
        oResource.setCulture()
    End Sub

    Private Sub SetupInternazionalizzazione()
        With oResource
            .setButton(Me.BtnSelCom, False, False, False, False)
            .setLabel(Me.LblComTitolo)
        End With
    End Sub

    'Public Sub SelLabelError(ByVal Tipo As String)
    '    'SelUT - Utente non selezionato
    '    Me.LBError.Text = Me.oLocate.getValue(Tipo) 'oNome as String
    'End Sub

#End Region

#Region "Raccolta lista comunita"
    Public Sub BindComunita()
        Dim StrLbl As String = ""
        Dim oPersona As New COL_Persona
        oPersona = Session("objPersona")

        Dim oChat As New Chat
        Dim oDataset As New DataSet
        oNumCom = 0

        Try
            oDataset = oChat.GetComunitaByUtente(oPersona.Id, UCServices.Abstract.MyServices.PermissionType.Admin, UCServices.Abstract.MyServices.PermissionType.Write, UCServices.Abstract.MyServices.PermissionType.Read)

            Dim DCTemp As DataColumn
            DCTemp = New DataColumn
            DCTemp.DataType = System.Type.GetType("System.Int32")
            DCTemp.ColumnName = "NumUtenti"
            oDataset.Tables(0).Columns.Add(DCTemp)

            For Each row As DataRow In oDataset.Tables(0).Rows
                row.Item("NumUtenti") = Me.oMessaggi.NumeroUtenti(row.Item("CMNT_id"))
                StrLbl &= "<a href=Chat_Ext.aspx?IDCom="
                StrLbl &= row.Item("CMNT_id").ToString
                StrLbl &= ">"
                StrLbl &= row.Item("NumUtenti") 'me.oMessaggi.getNumUtenti(row.Item("CMNT_id"))
                StrLbl &= " - "
                StrLbl &= "<img src=./../"
                StrLbl &= row.Item("TPCM_icona").ToString
                StrLbl &= " alt="
                StrLbl &= row.Item("TPCM_descrizione").ToString
                StrLbl &= "> - "
                StrLbl &= row.Item("CMNT_nome").ToString
                StrLbl &= "</a><br>"
                oNumCom += 1

            Next
            'With Me.LBComunita
            '    .DataSource = oDataset.Tables(0)
            '    .DataTextField = "CMNT_nome"
            '    .DataValueField = "CMNT_id"
            '    .DataBind()
            'End With
            Me.LblComunita.Text = StrLbl

            'TOLGO I DATI CHE NON MI SERVONO:
            'TPCM_descrizione
            'TPCM_icona
            'LKSC_Permessi
            oDataset.AcceptChanges()
            'E li metto in sessione cosi' ce li ho... (x l'header...)
            Session("ListaCom") = oDataset

        Catch ex As Exception
            'AL MASSIMO inserire che non e' in nessuna comunita'...
        End Try
        Me.DataBind()

    End Sub

#End Region

    'Private Sub BtnSelCom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelCom.Click
    '    Dim IDCom As Integer
    '    IDCom = Me.LBComunita.SelectedValue
    '    Response.Redirect("Chat_Ext.aspx?IDCom=" & IDCom)
    'End Sub

    '   	<TR>
    '	<TD><br>
    '		<asp:listbox id="LBComunita" Font-Size="xx-small" Width="262px" Rows="15" runat="server"></asp:listbox></TD>
    '</TR>
    '<TR>
    '	<TD align="center">
    '<asp:Button ID="BtnSelCom" Runat="server" Text="Entra" Font-Size="XX-Small" CssClass="Chat_Pulsante"></asp:Button>
    '		</TD>
    '</TR>

End Class