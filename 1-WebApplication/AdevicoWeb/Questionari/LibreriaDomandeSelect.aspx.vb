Imports COL_Questionario
Imports System.Linq

Partial Public Class LibreriaDomandeSelect
    Inherits PageBaseQuestionario

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    'Private _Presenter As InvitedUsersPresenter
    'Private ReadOnly Property CurrentPresenter() As InvitedUsersPresenter
    '    Get
    '        If IsNothing(_Presenter) Then
    '            _Presenter = New InvitedUsersPresenter(Me.CurrentContext, Me)
    '        End If
    '        Return _Presenter
    '    End Get
    'End Property
#End Region


    Private _SmartTagsAvailable As SmartTags
    Public Shared _iPag As Integer ' indice pagina
    Public Shared iDom As Integer ' indice domanda
    Dim oGestioneDomande As New GestioneDomande

    Private Property LibreriaCorrente() As Questionario
        Get
            Return ViewState("LibreriaCorrente")
        End Get
        Set(ByVal value As Questionario)
            ViewState("LibreriaCorrente") = value
        End Set
    End Property
    Private Property iPag() As Integer
        Get
            Return _iPag
        End Get
        Set(ByVal value As Integer)
            _iPag = value
        End Set
    End Property
    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            If _SmartTagsAvailable Is Nothing Then
                _SmartTagsAvailable = ManagerConfiguration.GetSmartTags(Me.ApplicationUrlBase())
            End If
            Return _SmartTagsAvailable
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides Sub BindDati()
        Dim oList As New List(Of Questionario)
        Select Case qs_questIdType
            Case Questionario.TipoQuestionario.LibreriaDiDomande
                oList = DALQuestionario.readLibrerieByComunita(PageUtility.CurrentContext, Me.ComunitaCorrenteID)
            Case Questionario.TipoQuestionario.Questionario
                oList = DALQuestionario.readQuestionariByComunita(Me.ComunitaCorrenteID)
        End Select
        If oList.Count > 0 Then
            DDLSelectLibreria.DataSource = oList
            DDLSelectLibreria.DataBind()
        Else
            LBSelezionaLibreria.Visible = True
            LBSelezionaLibreria.Text = Me.Resource.getValue("ErroreNessunaLibreria")
            LNBSelezionaLibreria.Visible = False
            DDLSelectLibreria.Visible = False
            LNBConferma.Visible = False
        End If
        LNBConferma.Visible = False
        LBSelezionaDomande.Visible = False
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return (MyBase.Servizio.Admin Or MyBase.Servizio.GestioneDomande)
    End Function

    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get

        End Get
    End Property

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetControlliByPermessi()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_LibreriaDomandeSelect", "Questionari")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLinkButton(LNBSelezionaLibreria, False, False)
            .setLinkButton(LNBConferma, False, False)
            .setLinkButton(LNBGestioneDomande, False, False)
            .setLinkButton(LNBGestioneQuestionario, False, False)
            If qs_questIdType() = (Questionario.TipoQuestionario.LibreriaDiDomande) Then
                .setLabel(LBSelezionaLibreria)
            Else
                .setLabel_To_Value(LBSelezionaLibreria, "LBSelezionaQuestionario")
            End If
            .setLabel(LBSelezionaDomande)
        End With
        SetServiceTitle(Master)
    End Sub


    Protected Sub DLDomandeEditCommand(ByVal sender As Object, ByVal e As DataListCommandEventArgs)

        'oQuest = Session("oQuest")
        Dim redirectUrl As String = oGestioneDomande.DLDomandeEditCommand(sender, e, LibreriaCorrente)
        Session("isPrimoAccesso") = True
        If Not redirectUrl = String.Empty Then
            Me.RedirectToUrl(redirectUrl)
        End If

        'caricaQuestionario()

    End Sub

    Protected Sub loadDomandeOpzioni(ByVal sender As Object, ByVal e As DataListItemEventArgs)
        iDom = e.Item.ItemIndex
        'oQuest = Session("oQuest")
        DLPagine.Controls(iPag).FindControl("DLDomande").Controls(iDom).FindControl("PHOpzioni").Controls.Add(oGestioneDomande.loadDomandeOpzioni(LibreriaCorrente, iPag, iDom, True))
        Dim LBtestoDopoDomanda As New Label
        LBtestoDopoDomanda.Text = LibreriaCorrente.pagine(iPag).domande(iDom).testoDopo
        If Not LBtestoDopoDomanda.Text Is String.Empty Then
            Dim aCapo As New LiteralControl
            aCapo.Text = "<br>"

            DLPagine.Controls(iPag).FindControl("DLDomande").Controls(iDom).FindControl("PHOpzioni").Controls.Add(LBtestoDopoDomanda)
        End If
        'SetInternazionalizzazioneDLDomande(e)
    End Sub

    Protected Sub DLPagine_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLPagine.ItemDataBound
        iPag = e.Item.ItemIndex 'se e' >0, bisogna rivedere LNBConferma.click

        Dim dlDomande As New DataList
        dlDomande = DLPagine.Controls(iPag).FindControl("DLDomande")

        If Me.QuestionarioCorrente.ownerType = OwnerType_enum.EduPathSubActivity OrElse Me.QuestionarioCorrente.ownerType = OwnerType_enum.EduPathActivity Then
            Dim domande As List(Of Domanda) = (From oDom As Domanda In LibreriaCorrente.pagine.Item(iPag).domande Select oDom Where Not oDom.tipo = oDom.TipoDomanda.Rating).ToList
            LibreriaCorrente.pagine(iPag).domande = domande
            dlDomande.DataSource = domande
        Else
            dlDomande.DataSource = LibreriaCorrente.pagine.Item(iPag).domande
        End If

        dlDomande.DataBind()
    End Sub

    Public Sub SetInternazionalizzazioneDLPagine(ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        With Me.Resource
            .setImageButton(e.Item.FindControl("IMBPagina"), False, True, True, False)
            .setImageButton(e.Item.FindControl("IMBEliminaPag"), False, True, True, False)
            .setImageButton(e.Item.FindControl("IMBLibrary"), False, True, True, False)
        End With
    End Sub

    Public Sub SetInternazionalizzazioneDLDomande(ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        With Me.Resource
            .setLabel(e.Item.FindControl("LBSelezionaDomanda"))
        End With
    End Sub


    Protected Sub LNBSelezionaLibreria_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LNBSelezionaLibreria.Click
        If DDLSelectLibreria.Items.Count > 0 And Not DDLSelectLibreria.SelectedValue = String.Empty Then
            LibreriaCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, DDLSelectLibreria.SelectedValue, Me.LinguaQuestionario, False)
            DLPagine.DataSource = LibreriaCorrente.pagine
            DLPagine.DataBind()
            LNBConferma.Visible = True
            LBSelezionaDomande.Visible = True
        End If
    End Sub

    Protected Sub LNBGestioneQuestionario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBGestioneQuestionario.Click
        Me.RedirectToUrl(RootObject.QuestionarioAdmin + "?" & qs_questType + Me.QuestionarioCorrente.tipo.ToString() & "&IdQ=" & Me.QuestionarioCorrente.id & "&idLanguage=" & Me.QuestionarioCorrente.idLingua)
    End Sub

    Protected Sub LNBGestioneDomande_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBGestioneDomande.Click
        Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, Me.LinguaQuestionario, False)
        Me.RedirectToUrl(RootObject.QuestionarioEdit + "?" & qs_questType + Me.QuestionarioCorrente.tipo.ToString() & "&IdQ=" & Me.QuestionarioCorrente.id & "&idLanguage=" & Me.QuestionarioCorrente.idLingua)
    End Sub
    Private Function qs_PageIndex() As Integer
        If IsNumeric(Request.QueryString("pI")) Then
            Return Request.QueryString("pI")
        Else
            Return -1
        End If
    End Function
    'Private Function getPreviusQuestionNumber(ByRef pageIndex As Integer) As Integer
    '    If pageIndex = -1 Then
    '        Return 0
    '    ElseIf Me.QuestionarioCorrente.pagine(pageIndex).domande.Count = 0 Then
    '        Return getPreviusQuestionNumber(pageIndex - 1)
    '    Else
    '        Return Me.QuestionarioCorrente.pagine(pageIndex).domande.Last.numero
    '    End If
    'End Function
    Private Function getPreviusQuestionNumber(ByVal pag As QuestionarioPagina) As Integer
        If IsNothing(pag) Then
            Return 0
        ElseIf pag.domande.Count = 0 Then
            Return getPreviusQuestionNumber(Me.QuestionarioCorrente.pagine.Where(Function(p) p.numeroPagina = pag.numeroPagina - 1).FirstOrDefault())
        Else
            Return pag.domande.Last.numero
        End If
    End Function
    Protected Sub LNBConferma_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LNBConferma.Click
        Dim edited As Boolean = False
        Dim pageNumber As Integer = qs_PageIndex()
        If Not Me.QuestionarioCorrente.pagine.Where(Function(p) p.numeroPagina = pageNumber).Any() Then
            pageNumber = Me.QuestionarioCorrente.pagine.Last.numeroPagina
        End If
        ' ciclo il datalist delle domande
        For Each oItem As DataListItem In DLPagine.Items
            Dim dlDomande As DataList
            dlDomande = DirectCast(oItem.FindControl("DLDomande"), DataList)
            Dim indice As Integer = 0
            Dim oGestioneDomande As New GestioneDomande
            For Each oItemD As DataListItem In dlDomande.Items
                ' prelevo le domande selezionate da aggiungere al questionario
                If DirectCast(oItemD.FindControl("CHKSelect"), CheckBox).Checked Then
                    Dim idQuestion As Integer = 0
                    Dim idPage As Integer = 0
                    Integer.TryParse(DirectCast(oItemD.FindControl("LTidQuestion"), Literal).Text, idQuestion)
                    Integer.TryParse(DirectCast(oItemD.FindControl("LTidPage"), Literal).Text, idPage)

                    Dim sourcePag As QuestionarioPagina = LibreriaCorrente.pagine.Where(Function(p) p.id = idPage).FirstOrDefault()

                    If idQuestion > 0 AndAlso Not IsNothing(sourcePag) Then

                        Dim oDomandaSel As New Domanda
                        oDomandaSel = sourcePag.domande.Where(Function(d) d.id = idQuestion).FirstOrDefault()
                        If Not IsNothing(oDomandaSel) Then
                            oDomandaSel.id = 0
                            oDomandaSel.idQuestionario = Me.QuestionarioCorrente.id
                            '' reimposto i parametri della domanda che verrà copiata nell'ultima pagina del questionario
                            'oDomandaSel.idPagina = Me.QuestionarioCorrente.pagine(Me.QuestionarioCorrente.pagine.Count - 1).id
                            'oDomandaSel.numeroPagina = Me.QuestionarioCorrente.pagine(Me.QuestionarioCorrente.pagine.Count - 1).numeroPagina

                            'aggiungo alla pagina in QS o all'ultima se qs non corrisponde
                            Dim destPage As QuestionarioPagina = Me.QuestionarioCorrente.pagine.Where(Function(p) p.numeroPagina = pageNumber).FirstOrDefault()
                            If IsNothing(destPage) Then
                                destPage = Me.QuestionarioCorrente.pagine.OrderByDescending(Function(p) p.numeroPagina).Skip(0).Take(1).FirstOrDefault()
                            End If
                            oDomandaSel.idPagina = destPage.id
                            oDomandaSel.numeroPagina = destPage.numeroPagina
                            ' Me.QuestionarioCorrente.pagine(PageIndex).numeroPagina
                            ' oDomandaSel.numero = destPage. + 1
                            oGestioneDomande.generaNumeroDomanda(oDomandaSel, Me.QuestionarioCorrente)

                            destPage.domande.Add(oDomandaSel)

                            ' Me.QuestionarioCorrente.pagine.Last.domande.Add(oDomandaSel)
                            'Me.QuestionarioCorrente.domande.Add(oDomandaSel)

                            DALDomande.Salva(oDomandaSel, False, Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.LibreriaDiDomande)
                            destPage.domande.Add(oDomandaSel)
                            oGestioneDomande.ricalcoloPagine(oDomandaSel, Me.QuestionarioCorrente)
                            'Me.QuestionarioCorrente.pagine.Last.domande.Last.id = oDomandaSel.id
                            'Me.QuestionarioCorrente.domande.Last.id = oDomandaSel.id

                            edited = True
                        End If

                    Else
                        DirectCast(oItemD.FindControl("CHKSelect"), CheckBox).Checked = False
                    End If
                End If
                indice = indice + 1
            Next

        Next

        'aggiorno i numeri di domande per pagina
        If edited Then

            Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.idLingua, False)

            '    Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.idLingua, False)
            '    For Each pageQuest As QuestionarioPagina In Me.QuestionarioCorrente.pagine.Where(Function(p) p.numeroPagina >= pageNumber).ToList
            '        pageQuest.dallaDomanda = pageQuest.domande(0).numero
            '        pageQuest.allaDomanda = pageQuest.domande(pageQuest.domande.Count - 1).numero
            '        DALPagine.Pagina_Update(pageQuest)
            '    Next
        End If


        RedirectToUrl(RootObject.QuestionarioEdit)

    End Sub
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub
End Class