Public Partial Class GestioneIscritti_NEW
    Inherits PageBase
    Implements IviewComunitaGestioneIScritti

	Public Overrides ReadOnly Property AlwaysBind() As Boolean
		Get
			Return False
		End Get
	End Property
	Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
		Get
			Return True
		End Get
	End Property
    Private oPresenter As ComunitaGestioneIscrittiPresenter

    Private Sub GestioneIscritti_NEW_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        oPresenter = New ComunitaGestioneIscrittiPresenter(Me)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            oPresenter.Initialize()
        Else
            'oPresenter.Applyfilters()
        End If
    End Sub

#Region "Proprietà vista"
    Public Property ItemsCount() As Integer Implements IviewListGeneric.ItemsCount
        Get
            Return CInt(Me.ViewState("intItemCount"))
        End Get
        Set(ByVal value As Integer)
            Me.LBLNumRow.Text = value
            Me.ViewState("intItemCount") = value
        End Set
    End Property
    Public Property ItemsForPage() As Integer Implements IviewListGeneric.ItemsForPage
        Get
            Dim NumRec As Integer
            Try
                NumRec = CInt(Me.DDLNumeroRecord.SelectedValue)
            Catch ex As Exception
                NumRec = 50
            End Try
            Return NumRec
        End Get
        Set(ByVal value As Integer)
            Try
                Me.DDLNumeroRecord.SelectedValue = value
            Catch ex As Exception
            End Try
        End Set
    End Property
    Public Property PageCount() As Integer Implements IviewListGeneric.PageCount
        Get
            Return CInt(Me.ViewState("intPageCount"))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("intPageCount") = value
        End Set
    End Property
    Public Property PageIndex() As Integer Implements IviewListGeneric.PageIndex
        Get
            Try
                Return CInt(Me.ViewState("intCurPage"))
            Catch ex As Exception
                Me.ViewState("intCurPage") = 0
                Return 0
            End Try
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("intCurPage") = value
        End Set
    End Property
#End Region


#Region "Filtri"
    'Public Property NumPagina() As Integer Implements IviewComunitaGestioneIScritti.NumPagina
    '    Get

    '    End Get
    '    Set(ByVal value As Integer)

    '    End Set
    'End Property

    'Public Property NumRecord() As Integer Implements IviewComunitaGestioneIScritti.NumRecord
    '    Get

    '    End Get
    '    Set(ByVal value As Integer)

    '    End Set
    'End Property

    'Public Property Lettera() As Char Implements IviewComunitaGestioneIScritti.Lettera
    '    Get

    '    End Get
    '    Set(ByVal value As Char)

    '    End Set
    'End Property

    Public Property Ruolo() As Integer Implements IviewComunitaGestioneIScritti.Ruolo
        Get
            Try
                Return CInt(Me.DDLruolo.SelectedValue)
            Catch ex As Exception
                Return -1
            End Try
        End Get
        Set(ByVal value As Integer)
            Try
                Me.DDLruolo.SelectedValue = value
            Catch ex As Exception
            End Try
        End Set
    End Property

    Public Property TipoRicerca() As Integer Implements IviewComunitaGestioneIScritti.TipoRicerca
        Get
            Try
                Return CInt(Me.DDLTipoRicerca.SelectedValue)
            Catch ex As Exception
                Return -1
            End Try
        End Get
        Set(ByVal value As Integer)
            Try
                Me.DDLTipoRicerca.SelectedValue = value
            Catch ex As Exception

            End Try
        End Set
    End Property

    Public Property ValoreRicerca() As String Implements IviewComunitaGestioneIScritti.ValoreRicerca
        Get
            Return Me.TXBValue.Text
        End Get
        Set(ByVal value As String)
            Me.TXBValue.Text = value
        End Set
    End Property

    Public Property VisualizzaIscrizione() As Integer Implements IviewComunitaGestioneIScritti.VisualizzaIscrizione
        Get
            Try
                Return CInt(Me.DDLiscrizione.SelectedValue)
            Catch ex As Exception
                Return -1
            End Try
        End Get
        Set(ByVal value As Integer)
            Try
                Me.DDLiscrizione.SelectedValue = value
            Catch ex As Exception
            End Try
        End Set
    End Property

    'Public Property Totale() As Integer Implements IviewComunitaGestioneIScritti.Totale
    '    Get

    '    End Get
    '    Set(ByVal value As Integer)

    '    End Set
    'End Property

    'Get
    '    
    'End Get
    'Set(ByVal value As COL_BusinessLogic_v2.Main.FiltroAnagrafica)
    '    
    'End Set


    'Filtro per lettera
    Public Property oAnagrafica() As COL_BusinessLogic_v2.Main.FiltroAnagrafica Implements IviewComunitaGestioneIScritti.oAnagrafica
        Get
            Dim oAnagraficaout As COL_BusinessLogic_v2.Main.FiltroAnagrafica

            Try
                oAnagraficaout = CInt(Me.ViewState("intAnagrafica"))
            Catch ex As Exception
                oAnagraficaout = FiltroAnagrafica.tutti
                Me.ViewState("intAnagrafica") = FiltroAnagrafica.tutti
            End Try
            Return oAnagraficaout 'FiltroAnagrafica.tutti
        End Get
        Set(ByVal value As COL_BusinessLogic_v2.Main.FiltroAnagrafica)
            Me.ViewState("intAnagrafica") = value
        End Set
    End Property

    Public Property oFiltroAbilitazione() As COL_BusinessLogic_v2.Main.FiltroAbilitazione Implements IviewComunitaGestioneIScritti.oFiltroAbilitazione
        Get
            Return Me.DDLiscrizione.SelectedValue
        End Get
        Set(ByVal value As COL_BusinessLogic_v2.Main.FiltroAbilitazione)
            Try
                Me.DDLiscrizione.SelectedValue = value
            Catch ex As Exception
                Me.DDLiscrizione.SelectedValue = -1
            End Try
        End Set
    End Property

    Public Property oFiltroRicercaAnagrafica() As COL_BusinessLogic_v2.Main.FiltroRicercaAnagrafica Implements IviewComunitaGestioneIScritti.oFiltroRicercaAnagrafica
        Get
            Return Me.DDLTipoRicerca.SelectedValue
        End Get
        Set(ByVal value As COL_BusinessLogic_v2.Main.FiltroRicercaAnagrafica)
            Try
                Me.DDLTipoRicerca.SelectedValue = value
            Catch ex As Exception
            End Try
        End Set
    End Property

    Public Property TipoRuoloId() As Integer Implements IviewComunitaGestioneIScritti.TipoRuoloId
        Get
            If Me.DDLTipoRuolo.SelectedValue = "" Then
                Return -1
            Else
                Try
                    Return CInt(Me.DDLTipoRuolo.SelectedValue)
                Catch ex As Exception
                    Return -1
                End Try
            End If

        End Get
        Set(ByVal value As Integer)
            Try
                Me.DDLTipoRuolo.SelectedValue = value
            Catch ex As Exception
                Me.DDLTipoRuolo.SelectedValue = -1
            End Try
        End Set
    End Property

    'Public Property Valore() As String Implements IviewComunitaGestioneIScritti.Valore
    '    Get
    '        Return Me.TXBValue.Text
    '    End Get
    '    Set(ByVal value As String)
    '        Me.TXBValue.Text = value
    '    End Set
    'End Property

    'Public Property WithFoto() As Boolean Implements IviewComunitaGestioneIScritti.WithFoto
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property
#End Region

#Region "Eventi pagina"
#Region "Filtri"
    Private Sub BTNCerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        Me.oPresenter.GotoPage(Me.PageIndex)
    End Sub
    Private Sub DDLTipoRuolo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipoRuolo.SelectedIndexChanged
        Me.oPresenter.GotoPage(Me.PageIndex)
    End Sub
    Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        Me.oPresenter.GotoPage(Me.PageIndex)
    End Sub
    Public Sub FiltroLinkLettere_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click
        If sender.commandArgument <> "" Then
            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
            Me.ViewState("intAnagrafica") = sender.commandArgument
            sender.CssClass = "lettera_Selezionata"
        Else
            Me.ViewState("intAnagrafica") = -1
            Me.LKBtutti.CssClass = "lettera_Selezionata"
        End If

        Me.oPresenter.GotoPage(Me.PageIndex)
        'Me.oPresenter.bindDatiTest()

        'Me.ViewState("intCurPage") = 0
        'Me.DGiscritti.CurrentPageIndex = 0
        'Me.Bind_Griglia(True, Me.CBXautoUpdate.Checked)
    End Sub
    Private Sub DeselezionaLink(ByVal Lettera As String)
        Dim oFiltro As Main.FiltroAnagrafica
        Lettera = CType(CInt(Lettera), Main.FiltroAnagrafica).ToString

        Dim oLink As System.Web.UI.WebControls.LinkButton
        oLink = Me.FindControlRecursive(Me.Master, "LKB" & Lettera)
        If IsNothing(oLink) = False Then
            oLink.CssClass = "lettera"
        End If
    End Sub
#End Region
#Region "Eventi GridView"
    Private Sub GViscritti_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GViscritti.PageIndexChanged
        Dim strtest As Object = e
    End Sub
    Private Sub GViscritti_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GViscritti.PageIndexChanging
        Me.oPresenter.GotoPage(e.NewPageIndex)
    End Sub
    Private Sub GViscritti_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GViscritti.RowCommand
        Dim IdPersona As Integer = CInt(e.CommandArgument)
        Select Case e.CommandName
            Case "Modifica"
                Me.oPresenter.Modifica(IdPersona)
            Case "Elimina"
                Me.oPresenter.Deiscrivi(IdPersona)
        End Select
    End Sub
#End Region
#Region "Menu"

#End Region
#End Region

#Region "binding"
	Public Sub CaricaRuoli(ByVal items As COL_BusinessLogic_v2.GenericCollection(Of Role)) Implements IviewComunitaGestioneIScritti.CaricaRuoli
		With Me.DDLTipoRuolo
			.Items.Clear()
			.DataSource = items
			.DataValueField = "ID"
			.DataTextField = "Description"
			.DataBind()
			.Visible = True
		End With
	End Sub
    Public Sub SetLista(ByVal items As System.Collections.IList) Implements IviewListGeneric.SetLista
        With GViscritti
            GViscritti.DataSource = items
            GViscritti.PageSize = Me.ItemsForPage
            GViscritti.PageIndex = Me.PageIndex
            GViscritti.DataBind()
        End With
    End Sub
#End Region

#Region "Altro"
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)
        If errorMessage = "" Then
            LBLMessage.Visible = False
        Else
            LBLMessage.Text = errorMessage
            LBLMessage.Visible = True
        End If
        Me.LBLMessage.DataBind()
    End Sub
    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean

    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region




#Region "Visualizzazioni"
    Private Sub Reset_ToModifica()
        Me.Reset_HideAllForm()
        Me.PNLmenuModifica.Visible = True
        Me.GViscritti.SelectedIndex = -1
        Me.PNLmodifica.Visible = True
    End Sub
    Private Sub Reset_ToListaUtenti()
        Me.Reset_HideAllForm()
        Me.PNLcontenuto.Visible = True
        Me.PNLpermessi.Visible = False
        Me.PNLmenuPrincipale.Visible = True
        Me.PNLmenu.Visible = True
        'Me.PNLiscritti.Visible = True
        Me.GViscritti.Visible = True
    End Sub
    Private Sub Reset_HideAllForm()
        'Me.PNLiscritti.Visible = False
        Me.GViscritti.Visible = False
        'Me.FRVIscritto.Visible = False
        Me.PNLdeiscrivi.Visible = False
        Me.PNLdeiscriviMultiplo.Visible = False
        Me.PNLmenu.Visible = False
        Me.PNLmenuDeIscrivi.Visible = False
        Me.PNLmenuDeIscriviMultiplo.Visible = False
        Me.PNLmenuModifica.Visible = False
        Me.PNLmenuPrincipale.Visible = False
        'Me.PNLmodifica.Visible = False
        Me.PNLpermessi.Visible = False
    End Sub
#End Region

#Region "Iscritto"
    Public WriteOnly Property Iscritto_Anagrafica() As String Implements IviewComunitaGestioneIScritti.Iscritto_Anagrafica
        Set(ByVal value As String)
            Me.LBNomeCognome.Text = value
        End Set
    End Property

    Public Property Iscritto_Id() As Integer Implements IviewComunitaGestioneIScritti.Iscritto_Id
        Get
            Try
                Return CInt(Me.HDNprsn_Id.Value)
            Catch ex As Exception
                Return 0
            End Try
        End Get
        Set(ByVal value As Integer)
            Me.HDNprsn_Id.Value = value
        End Set
    End Property

    Public Property Iscritto_IdRuolo() As Integer Implements IviewComunitaGestioneIScritti.Iscritto_IdRuolo
        Get
            Try
                Return CInt(Me.DDLruolo.SelectedValue)
            Catch ex As Exception
                Return 0
            End Try
        End Get
        Set(ByVal value As Integer)
            Try
                Me.DDLruolo.SelectedValue = value
            Catch ex As Exception

            End Try
        End Set
    End Property

    Public Property Iscritto_IsResponsabile() As Boolean Implements IviewComunitaGestioneIScritti.Iscritto_IsResponsabile
        Get
            Return Me.CHBresponsabile.Checked
        End Get
        Set(ByVal value As Boolean)
            CHBresponsabile.Checked = value
        End Set
    End Property

    Public Sub ShowGriglia() Implements IviewComunitaGestioneIScritti.ShowGriglia
        Reset_ToListaUtenti()
    End Sub

    Public Sub ShowModifica() Implements IviewComunitaGestioneIScritti.ShowModifica
        Me.Reset_ToModifica()
    End Sub

    Public Sub ModificaIscritto(ByVal IscrittoId As Integer)
        Me.oPresenter.Modifica(IscrittoId)
    End Sub

    Private Sub GViscritti_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GViscritti.RowDataBound
        If e.Row.RowType = DataControlRowType.Pager Then

            Dim oCell As TableCell
            Dim n As Integer
            oCell = CType(e.Row.Controls(0), TableCell)

            n = oCell.ColumnSpan

            Dim pagerTable As Table = CType(e.Row.Cells(0).Controls(0), Table)
            Dim oRow As TableRow = pagerTable.Rows(0)

            For Each oTableCell As TableCell In oRow.Cells
                Dim oWebControl As WebControl
                oWebControl = oTableCell.Controls(0)
                If TypeOf (oWebControl) Is Label Then
                    oWebControl.CssClass = "ROW_PagerSpan_Small"
                ElseIf TypeOf (oWebControl) Is LinkButton Then
                    oWebControl.CssClass = "ROW_PagerLink_Small"
                    'Me.Resource.setPageGridView("DGnewsGlobal", oWebControl)
                End If
            Next
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            Dim oIscritto As COL_BusinessLogic_v2.Comunita.Iscritto
            Try
                oIscritto = DirectCast(e.Row.DataItem, COL_BusinessLogic_v2.Comunita.Iscritto)


                '----------------    INFO  ---------------

                Dim i_link2 As String
                Dim oImageButton As ImageButton 'LinkButton
                Try
                    oImageButton = e.Row.Cells(0).FindControl("ImbInfo")
                    oImageButton.Attributes.Remove("onClick")

                  
                        i_link2 = "./InfoIscritto.aspx?TPPR_ID=" & oIscritto.Ruolo.Id & "&PRSN_ID=" & oIscritto.Persona.Id

                        'in base al tipo di utente decido la dimensione della finestra di popup
                        Select Case oIscritto.Ruolo.Id
                        Case Main.TipoPersonaStandard.Tutor
                            oImageButton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                            Case Main.TipoPersonaStandard.Esterno
                                oImageButton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                            Case Main.TipoPersonaStandard.Amministrativo
                                oImageButton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                            Case Main.TipoPersonaStandard.SysAdmin
                                oImageButton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                            Case Main.TipoPersonaStandard.Copisteria
                                oImageButton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                        Case Else
                            oImageButton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    End Select

                Catch ex As Exception

                End Try

                '------------ Modifica  ------------------
                'Dim oLinkButton As LinkButton

                Try
                    oImageButton = New ImageButton
                    oImageButton = e.Row.Cells(0).FindControl("ImbModifica")
                    'oLinkButton = e.Row.Cells(1).Controls(0)
                    If IsNothing(oImageButton) = False Then
                        oImageButton.CommandName = "Modifica"
                        oImageButton.CommandArgument = oIscritto.Persona.ID '.ToString
                        'NON SERVE AD UNA CIPPA! NON LO TIENE!!!
                        oImageButton.AlternateText = "Test"
                        oImageButton.CssClass = "ROW_ItemLink_Small"
                        oImageButton.DataBind()
                        'Me.Resource.setImageButton_GridView("DGnewsGlobali", Me.BaseUrl, oLinkButton, "IMBmodifica", True)
                    End If

                Catch ex As Exception

                End Try

                '--------------   Deiscrivi   ---------------------
                Try
                    oImageButton = e.Row.Cells(0).FindControl("ImbDeiscrivi")
                    oImageButton.Enabled = True
                    oImageButton.CommandName = "Deiscrivi"
                    oImageButton.CommandArgument = oIscritto.Persona.ID '.ToString
                    'Me.Resource.setImageButton_GridView("DGnewsGlobali", Me.BaseUrl, oIMBcancella, "IMBcancella", True, True, True, True)
                Catch ex As Exception

                End Try
            Catch ex As Exception
            End Try

        End If
    End Sub

    Private Sub GViscritti_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GViscritti.RowEditing


        If Me.GViscritti.PageIndex = 0 Then
            'Me.FRVIscritto.PageIndex = e.NewEditIndex
        Else
            'Me.FRVIscritto.PageIndex = (Me.GViscritti.PageSize * Me.GViscritti.PageIndex) + e.NewEditIndex
        End If
        Me.ShowModifica()
        'MostraFormModifica()
        e.Cancel = True





        'Dim Indice As Integer
        'If Me.GViscritti.PageIndex = 0 Then
        '    Indice = e.NewEditIndex
        'Else
        '    Indice = (Me.GViscritti.PageSize * Me.GViscritti.PageIndex) + e.NewEditIndex
        'End If

        'Dim id As Integer = Me.GViscritti.DataKeys(Indice).Value

        'Me.oPresenter.Modifica(id)


        'Dim s As String

        'Try
        '    s = Me.GViscritti.Rows(e.NewEditIndex).Cells(5).Text
        'Catch ex As Exception
        'End Try

        'Dim o As COL_BusinessLogic_v2.Comunita.Iscritto
        'Try
        '    o = sender.Rows(e.NewEditIndex)
        'Catch ex As Exception
        'End Try

        'Try
        '    o = DirectCast(sender.dataitem(), COL_BusinessLogic_v2.Comunita.Iscritto)
        '    Me.Iscritto_Anagrafica = o.Persona.Anagrafica
        '    Me.Iscritto_Id = o.Persona.Id
        '    Me.Iscritto_IdRuolo = o.Ruolo.Id
        '    Me.Iscritto_IsResponsabile = o.RLPC_Responsabile

        '    Me.ShowModifica()
        'Catch ex As Exception
        '    oPresenter.GotoPage(Me.PageIndex)
        'End Try



    End Sub
#End Region

    Private Function FindControlRecursive(ByVal Root As Control, ByVal Id As String) As Control
        If Root.ID = Id Then
            Return Root
        End If

        For Each Ctl As Control In Root.Controls
            Dim FoundCtl As Control = FindControlRecursive(Ctl, Id)
            If FoundCtl IsNot Nothing Then
                Return FoundCtl
            End If
        Next
        Return Nothing
    End Function

    'Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
    '    Get
    '        Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
    '    End Get
    'End Property

End Class

'<asp:CommandField ButtonType="Image" EditImageUrl="~/images/DG/m.gif" CancelImageUrl="~/images/DG/x.gif" ShowDeleteButton="true" ShowEditButton="true" ShowCancelButton="true" ShowInsertButton="false" />



'      <asp:CommandField ButtonType="Image" ShowEditButton="True"  EditImageUrl="~/images/DG/m.gif" />
'<asp:CommandField ButtonType="Image" ShowDeleteButton="True" DeleteImageUrl="~/images/DG/x.gif" />

'<script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>







'    <asp:datagrid id="DGiscritti" runat="server" width="850px" AllowSorting="true" Font-Size="8" Font-Name="verdana" ShowFooter="false" BackColor="transparent" BorderColor="#8080FF" AutoGenerateColumns="False"
'	DataKeyField="RLPC_ID" PageSize="50" AllowCustomPaging="True">
'	<AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
'	<HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
'	<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
'	<PagerStyle CssClass="ROW_Page_Small" Position="TopAndBottom" Mode="NumericPages" Visible="true" HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
'	<Columns>
'		<asp:BoundColumn DataField="RLPC_ID" HeaderText="RLPC" Visible="false"></asp:BoundColumn>
'		<asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-width="10" >
'			<ItemTemplate>
'				<asp:ImageButton id="IMBCancella" Runat="server" CausesValidation="False" CommandName="deiscrivi"
'					ImageUrl="../images/x.gif"></asp:ImageButton>
'			</ItemTemplate>
'		</asp:TemplateColumn>
'		<asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-width="10" >
'			<ItemTemplate>
'				<asp:ImageButton id="IMBmodifica" Runat="server" CausesValidation="False" CommandName="modifica"
'					ImageUrl="../images/m.gif"></asp:ImageButton>
'			</ItemTemplate>
'		</asp:TemplateColumn>
'		<asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-width="10" >
'			<ItemTemplate>
'				<asp:ImageButton id="IMBinfo" Runat="server" CausesValidation="False" CommandName="infoPersona" ImageUrl="../images/proprieta.gif"></asp:ImageButton>
'			</ItemTemplate>
'		</asp:TemplateColumn>
'		<asp:TemplateColumn ItemStyle-CssClass="ROW_TD_Small" HeaderText="Cognome" SortExpression="Persona.Cognome">
'			<ItemTemplate>
'				&nbsp;<%#Container.DataItem.Persona.Cognome%>
'			</ItemTemplate>
'		</asp:TemplateColumn>
'		<asp:TemplateColumn ItemStyle-CssClass="ROW_TD_Small" HeaderText="Nome" SortExpression="Persona.Nome">
'			<ItemTemplate>
'				&nbsp;<%#Container.Dataitem.Persona.Nome%>
'			</ItemTemplate>
'		</asp:TemplateColumn>
'		<asp:TemplateColumn ItemStyle-CssClass="ROW_TD_Small" HeaderText="Nome" SortExpression="Persona.Anagrafica" Visible="False">
'			<ItemTemplate>
'				&nbsp;<%#Container.Dataitem.Persona.Anagrafica%>
'			</ItemTemplate>
'		</asp:TemplateColumn>
'		<%--<asp:BoundColumn DataField="Persona.Anagrafica" HeaderText="Anagrafica" SortExpression="Persona.Anagrafica" Visible="False"></asp:BoundColumn>--%>

'		<asp:TemplateColumn runat="server" HeaderText="Mail" ItemStyle-HorizontalAlign="Center" ItemStyle-width="10"
'			ItemStyle-CssClass="ROW_TD_Small" HeaderStyle-CssClass="ROW_header_Small_Center">
'			<ItemTemplate>
'				&nbsp;<asp:HyperLink NavigateUrl='<%# "mailto:" & Container.Dataitem.Persona.Mail%>' text='<%# Container.Dataitem.Persona.Mail%>' Runat="server" ID="HYPMail" CssClass="ROW_ItemLink_Small" />&nbsp;
'			</ItemTemplate>
'		</asp:TemplateColumn>

'		<%--<asp:BoundColumn DataField="Ruolo.Id" HeaderText="idRuolo" SortExpression="Ruolo.Id" Visible="False"></asp:BoundColumn>--%>
'		<%--<asp:BoundColumn DataField="Ruolo.Id" HeaderText="idtipopersona" SortExpression="Ruolo.Id" Visible="False"></asp:BoundColumn>--%>
'		<asp:TemplateColumn ItemStyle-CssClass="ROW_TD_Small" HeaderText="Nome" SortExpression="Ruolo.Id" Visible="False">
'			<ItemTemplate>
'				&nbsp;<%#Container.Dataitem.Ruolo.Id%>
'			</ItemTemplate>
'		</asp:TemplateColumn>

'		<asp:TemplateColumn runat="server" HeaderText="Ruolo" SortExpression="Ruolo.Nome" ItemStyle-CssClass="ROW_TD_Small">
'			<ItemTemplate>
'				&nbsp;<%#Container.DataItem.Ruolo.Nome%>
'			</ItemTemplate>
'		</asp:TemplateColumn>

'		<asp:BoundColumn DataField="RLPC_IscrittoIl" HeaderText="Iscritto il" SortExpression="RLPC_IscrittoIl" Visible="true" ItemStyle-width="120" ItemStyle-CssClass="ROW_TD_Small" headerStyle-CssClass="ROW_Header_Small_center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>

'		<%--<asp:BoundColumn DataField="Ruolo.Gerarchia" visible="false"></asp:BoundColumn>--%>
'		<asp:TemplateColumn runat="server" visible="false">
'			<ItemTemplate>
'				&nbsp;<%#Container.DataItem.Ruolo.Gerarchia%>
'			</ItemTemplate>
'		</asp:TemplateColumn>

'		<%--<asp:BoundColumn DataField="Persona.Id" visible="false"></asp:BoundColumn>--%>
'		<asp:TemplateColumn runat="server" visible="false">
'			<ItemTemplate>
'				&nbsp;<%#Container.DataItem.Persona.Id%>
'			</ItemTemplate>
'		</asp:TemplateColumn>

'		<asp:BoundColumn DataField="RLPC_attivato" visible="false"></asp:BoundColumn>
'<%--		<asp:TemplateColumn runat="server" visible="false">
'			<ItemTemplate>
'				&nbsp;<%#Container.DataItem.RLPC_attivato%>
'			</ItemTemplate>
'		</asp:TemplateColumn>--%>

'		<asp:BoundColumn DataField="RLPC_abilitato" visible="false"></asp:BoundColumn>
'<%--		<asp:TemplateColumn runat="server" visible="false">
'			<ItemTemplate>
'				&nbsp;<%#Container.DataItem.RLPC_abilitato%>
'			</ItemTemplate>
'		</asp:TemplateColumn>--%>

'		<asp:BoundColumn DataField="RLPC_Responsabile" visible="false"></asp:BoundColumn>
'		<asp:BoundColumn DataField="RLPC_ultimoCollegamento" HeaderText="Last visit" SortExpression="RLPC_ultimoCollegamento" ItemStyle-width="120" visible="true" ItemStyle-CssClass="ROW_TD_Small" headerStyle-CssClass="ROW_Header_Small_center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>

'		<asp:TemplateColumn ItemStyle-CssClass="ROW_TD_Small" HeaderText="Login" SortExpression="PRSN_login" Visible="False">
'			<ItemTemplate>
'				&nbsp;<%#Container.DataItem.Persona.Login%>
'			</ItemTemplate>
'		</asp:TemplateColumn>
'		<asp:TemplateColumn ItemStyle-width="20px" ItemStyle-HorizontalAlign="Center">
'			<HeaderTemplate>
'				<input type="checkbox" id="SelectAll" name="SelectAll" onclick="SelectAll(this);" runat="server"/>
'			</HeaderTemplate>
'			<ItemTemplate>
'				<%--<input type="checkbox" value="<%# DataBinder.Eval(Container.DataItem.Persona.Id.ToString)%>" id="CBazione" name="CBazione" <%# DataBinder.Eval(Container.DataItem, "oCheck") %> onclick="SelectMe(this);" <%# DataBinder.Eval(Container.DataItem, "oCheckDisabled") %> >--%>
'			</ItemTemplate>
'		</asp:TemplateColumn>
'		<asp:BoundColumn DataField="LKPO_Default" Visible="False"></asp:BoundColumn>
'	</Columns>
'	<PagerStyle width="800px" PageButtonCount="5" mode="NumericPages"></PagerStyle>
'</asp:datagrid>



'<asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10">
'            <ItemTemplate>
'                <asp:ImageButton id="IMBCancella" Runat="server" CausesValidation="False" CommandName="deiscrivi"
'					ImageUrl="../images/x.gif"></asp:ImageButton>
'			</ItemTemplate>
'        </asp:TemplateField>
'        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10">
'            <ItemTemplate>
'                <asp:ImageButton id="IMBmodifica" Runat="server" CausesValidation="False" CommandName="modifica"
'					ImageUrl="../images/m.gif"></asp:ImageButton>
'			</ItemTemplate>
'        </asp:TemplateField>
'        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10">
'            <ItemTemplate>
'                <asp:ImageButton id="IMBinfo" Runat="server" CausesValidation="False" CommandName="infoPersona" ImageUrl="../images/proprieta.gif"></asp:ImageButton>
'            </ItemTemplate>
'        </asp:TemplateField>





'<asp:FormView ID="FRVIscritto" runat="server" DataKeyNames="IscrittoID" DefaultMode="Edit" Visible="False" Width="600px" BackColor="Navy" ForeColor="White" HeaderText="Iscritto" HorizontalAlign="Left">
'    <EditItemTemplate>
'      <- begin Formview ->
'        <div style="width:600px;">
'            <div style="width:600px; height:25px; clear:left">
'	            <div style="width:150px; height:25px; float: left;">
'	                <asp:Label ID="FRVLBanagrafica_t" CssClass="Titolo_campo" Runat="server">Anagrafica:&nbsp;</asp:Label>
'	            </div>
'	            <div style="width:450px; height:25px; float: left;">
'	                <asp:label id="FRVLBNomeCognome" Runat="server" CssClass="Testo_campo" Text='<%# Bind("ProductName") %>'></asp:label>
'	            </div>
'            </div>
'        </div>
'        <div style="width:600px; height:25px; clear:left">
'            <div style="width:150px; height:25px; float: left;">
'                <asp:Label ID="FRVLBruolo_t" CssClass="Titolo_campo" Runat="server">Ruolo:&nbsp;</asp:Label>
'            </div>
'            <div style="width:450px; height:25px; float: left;">
'                <asp:DropDownList id="FRVDDLruolo" Runat="server" CssClass="Testo_campo" SelectedValue='<%# Bind("RuoloID") %>'></asp:DropDownList>
'                <%--<asp:DropDownList ID="DDLcategorie" runat="server"  DataTextField="CategoryName"
'            DataValueField="CategoryID" SelectedValue='<%# Bind("CategoryID") %>' />--%>
'            </div>
'        </div>
'        <div style="width:600px; height:25px; clear:left">
'            <div style="width:150px; height:25px; float: left;">
'                <asp:Label ID="FRVLBresponsabile_t" CssClass="Titolo_campo" Runat="server">Responsabile:&nbsp;</asp:Label>
'            </div>
'            <div style="width:450px; height:25px; float: left;"> 
'                <asp:CheckBox ID="FRVCHBresponsabile" Runat="server" Text="Si" CssClass="Testo_campo" Checked='<%# Bind("IsResponsabile") %>'></asp:CheckBox>

'	            <%--<asp:DropDownList ID="DDLcompagina" runat="server" DataSourceID="SDScompagnie" DataTextField="CompanyName"
'            DataValueField="SupplierID" SelectedValue='<%# Bind("SupplierID") %>' />--%>
'            </div>
'        </div>
'        <- begin Formview ->
'    </EditItemTemplate>
'    <EditRowStyle BackColor="#FFFFC0" Font-Names="Verdana" Font-Size="Small" ForeColor="Navy" />

'</asp:FormView>