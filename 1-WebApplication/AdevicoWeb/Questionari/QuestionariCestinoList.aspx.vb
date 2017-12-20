Imports COL_Questionario

Partial Public Class QuestionariCestinoList
    Inherits PageBaseQuestionario


    ' indica la modalità della pagina: 0=amministrazione, 1=statistiche
    Private Property pageMode() As String
        Get
            Return Request.QueryString("mode")
        End Get
        Set(ByVal value As String)
            ViewState("pageMode") = Request.QueryString("mode")
        End Set
    End Property



    Private Property listaQuestionari() As List(Of Questionario)
        Get
            Return ViewState("listaQuestionari")
        End Get
        Set(ByVal value As List(Of Questionario))
            ViewState("listaQuestionari") = value
        End Set
    End Property

    Dim oQuest As New COL_Questionario.Questionario


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides Sub BindDati()

        caricaQuestionariCancellati()

        If listaQuestionari.Count > 0 Then
            Me.MLVquestionari.SetActiveView(Me.VIWdati)
            GRVElenco.DataSource = listaQuestionari
            GRVElenco.DataBind()
        Else
            Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
            LBerrore.Text = Me.Resource.getValue("MessaggioCestinoVuoto")
        End If

    End Sub

    Private Function caricaQuestionariCancellati()
        listaQuestionari = DALQuestionario.readQuestionariCancellatiByComunita(Me.ComunitaCorrenteID, Me.qs_questIdType)
    End Function


    Public Overrides Sub BindNoPermessi()
        Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return (MyBase.Servizio.Admin Or MyBase.Servizio.CancellaQuestionario)
    End Function

    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = False
        End Get
    End Property

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        Select Case Me.qs_questIdType
            Case Questionario.TipoQuestionario.Questionario
                MyBase.SetCulture("pg_QuestionariCestinoList", "Questionari")
            Case Questionario.TipoQuestionario.Sondaggio
                MyBase.SetCulture("pg_SondaggiCestinoList", "Questionari")
            Case Questionario.TipoQuestionario.Meeting
                MyBase.SetCulture("pg_MeetingCestinoList", "Questionari")
            Case Questionario.TipoQuestionario.Modello
                MyBase.SetCulture("pg_ModelliCestinoList", "Questionari")
            Case Questionario.TipoQuestionario.LibreriaDiDomande
                MyBase.SetCulture("pg_LibrerieCestinoList", "Questionari")
        End Select
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            Master.ServiceTitle = .getValue("ServiceTitle")
            .setLinkButton(LNBIndietro, False, False)
            .setLabel(LBerrore)
            .setLinkButton(LNBSvuotaCestino, False, False, False, True)
            .setHeaderGridView(Me.GRVElenco, 0, "headerNome", True)
            .setHeaderGridView(Me.GRVElenco, 1, "headerDataInizio", True)
            .setHeaderGridView(Me.GRVElenco, 2, "headerDataFine", True)
            .setHeaderGridView(Me.GRVElenco, 3, "headerElimina", True)
            .setImageButton(IMBHelp, False, False, True, False)
            .setLabel(LBHelp)
        End With
    End Sub

    Protected Sub setInternazionalizzaioneGRVElencoControls(ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        With Me.Resource
            .setImageButton(e.Row.FindControl("IMBElimina"), False, True, True, True)
            .setImageButton(e.Row.FindControl("IMBRipristina"), False, True, True, True)
        End With
    End Sub

    Protected Sub GRVElenco_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GRVElenco.RowDataBound

        If e.Row.RowIndex >= 0 Then
            Dim dataI As DateTime = Convert.ToDateTime(e.Row.Cells(1).Text)
            If dataI = Date.MinValue Or dataI > Date.MaxValue.AddDays(-1) Then
                e.Row.Cells(1).Text = String.Empty
            End If
            Dim dataF As DateTime = Convert.ToDateTime(e.Row.Cells(2).Text)
            If dataF = Date.MinValue Or dataF > Date.MaxValue.AddDays(-1) Then
                e.Row.Cells(2).Text = String.Empty
            End If

            setInternazionalizzaioneGRVElencoControls(e)

        End If

    End Sub

    Protected Sub GRVElenco_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GRVElenco.RowCommand

        If CType(e.CommandSource, Control).NamingContainer.GetType() Is GetType(GridViewRow) Then
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            oQuest.idQuestionarioMultilingua = GRVElenco.DataKeys(row.RowIndex).Value
            oQuest.id = Integer.Parse(DirectCast(row.FindControl("LBIDQuestionario"), Label).Text)
            Me.LinguaQuestionario = Integer.Parse(DirectCast(row.FindControl("LBIDLingua"), Label).Text)

            Me.QuestionarioCorrente = oQuest 'ok

            Select Case e.CommandName
                Case "Anteprima"
                    Me.RedirectToUrl(RootObject.QuestionarioView + "?mode=1")
                Case "Ripristina"
                    DALQuestionario.RipristinaQuestionario(oQuest)
                    BindDati()
                Case "Elimina"
                    DALQuestionario.DeleteQuestionario_Physical(oQuest.idQuestionarioMultilingua)
                    BindDati()
            End Select
        End If


    End Sub

    Protected Sub GRVElenco_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GRVElenco.PageIndexChanging
        GRVElenco.PageIndex = e.NewPageIndex
        BindDati()
    End Sub

    Public Overrides Sub SetControlliByPermessi()

    End Sub

    Protected Sub LNBIndietro_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBIndietro.Click
        Select Case Me.qs_questIdType
            Case Questionario.TipoQuestionario.Modello
                Me.RedirectToUrl(RootObject.ModelliGestioneList & "&" & qs_questType & CInt(Me.qs_questIdType))
            Case Else
                Me.RedirectToUrl(RootObject.QuestionariGestioneList & "&" & qs_questType & CInt(Me.qs_questIdType))
        End Select
    End Sub

    Protected Sub LNBSvuotaCestino_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBSvuotaCestino.Click

        For Each row As GridViewRow In GRVElenco.Rows
            oQuest.idQuestionarioMultilingua = GRVElenco.DataKeys(row.RowIndex).Value
            DALQuestionario.DeleteQuestionario_Physical(oQuest.idQuestionarioMultilingua)
        Next

        BindDati()

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