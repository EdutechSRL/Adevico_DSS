Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers
Imports Eco = lm.Comol.Modules.CallForPapers.AdvEconomic


Imports lm.ActionDataContract
Imports System.Linq
Imports lm.Comol.Modules.CallForPapers.AdvEconomic.dto
Imports lm.Comol.Modules.CallForPapers.AdvEconomic.Presentation.View
Imports lm.Comol.Modules.CallForPapers.Presentation

Imports lm.Comol.Modules.CallForPapers.Domain

Public Class cpAdvEconomicEvaluation
    Inherits PageBase
    Implements Eco.Presentation.View.iViewEcoEvaluation

    Private Shared CurrencyFormat As String = "C2"
    Private Shared CurrencyCulture As String = "it-IT"

    Private _canModify As Boolean = False

#Region "Context"
    Private _Presenter As Eco.Presentation.AdvEcoEvaluationPresenter
    Private ReadOnly Property CurrentPresenter() As Eco.Presentation.AdvEcoEvaluationPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New Eco.Presentation.AdvEcoEvaluationPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "PageBase"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property



    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub BindNoPermessi()

        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvEvaluationsView,
                Me.EvalId,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.Evaluation,
                "NoPermission;CommissionType:Economic")

        Master.ShowNoPermission = True
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides Sub BindDati()

        If Not Page.IsPostBack Then
            CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvEvaluationsView,
                Me.EvalId,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.Evaluation,
                "Economic")
        End If


        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function


#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


#Region "DefaultView"
    Public Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvEvaluationsView,
                Me.EvalId,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.Evaluation,
                "SessionTimeOut;CommissionType:Economic")
    End Sub

    Public Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.Master.ShowNoPermission = True
    End Sub
#End Region


#Region "View"
    Public ReadOnly Property CommId As Long Implements iViewEcoEvaluation.CommId
        Get
            Dim cmId As Long = 0
            Try
                cmId = ViewStateOrDefault("CommissionId", 0)
            Catch ex As Exception

            End Try

            If (cmId = 0) Then
                Try
                    cmId = Request.QueryString("cmId")
                Catch ex As Exception

                End Try
            End If

            ViewState("CommissionId") = cmId

            Return cmId

        End Get
    End Property

    Public ReadOnly Property EvalId As Long Implements iViewEcoEvaluation.EvalId
        Get
            Dim cmId As Long = 0
            Try
                cmId = ViewStateOrDefault("EvaluationId", 0)
            Catch ex As Exception

            End Try

            If (cmId = 0) Then
                Try
                    cmId = Request.QueryString("evId")
                Catch ex As Exception

                End Try
            End If

            ViewState("EvaluationId") = cmId

            Return cmId
        End Get
    End Property

    Public Sub BindView(evaluations As dtoEcoEvaluation) Implements iViewEcoEvaluation.BindView



        _canModify = evaluations.CanModify

        'Chiusura presidente
        LKBClose.Visible = evaluations.CanClose
        LKBClose.Enabled = evaluations.CanClose

        'Conferma dati membri
        LKBconfirm.Visible = evaluations.CanModify
        LKBconfirm.Enabled = evaluations.CanModify

        LKBdraft.Visible = evaluations.CanReopen
        LKBdraft.Enabled = evaluations.CanReopen

        LKBSave.Visible = evaluations.CanModify
        LKBSave.Enabled = evaluations.CanModify


        HypSummary.NavigateUrl = BaseUrl & RootObject.EcoSummaries(Me.CommId)

        LBowner.Text = evaluations.SubmissionName
        LBsubmitterType.Text = evaluations.SubmissionType
        LBFinalTotalRequest.Text = evaluations.RequestTotal.ToString(CurrencyFormat, System.Globalization.CultureInfo.CreateSpecificCulture(CurrencyCulture)).Remove(0, 2)

        LBFinalTotalAdmit.Text = evaluations.AdmitTotal.ToString(CurrencyFormat, System.Globalization.CultureInfo.CreateSpecificCulture(CurrencyCulture)).Remove(0, 2)

        RPTtables.DataSource = evaluations.Tables
        RPTtables.DataBind()


        If Not IsNothing(evaluations.Tables) AndAlso evaluations.Tables.Any() Then
            RPTtables.Visible = True
            RPTtables.DataSource = evaluations.Tables
            RPTtables.DataBind()

            LBFinalMAXadmit.Text = evaluations.Tables.Sum(Function(t) t.AdmitMax).ToString(CurrencyFormat, System.Globalization.CultureInfo.CreateSpecificCulture(CurrencyCulture)).Remove(0, 2)

        Else

            RPTtables.Visible = False
            LBFinalMAXadmit.Text = 0
        End If

    End Sub

    Private Sub RPTtables_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTtables.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim Table As Eco.dto.dtoEcoEvTable = e.Item.DataItem

            If IsNothing(Table) Then
                Exit Sub
            End If

            Dim LBL As Label = e.Item.FindControl("LBLfieldName")
            If Not IsNothing(LBL) Then
                LBL.Text = Table.FieldName
            End If

            LBL = e.Item.FindControl("LBLfielddescription")
            If Not IsNothing(LBL) Then
                LBL.Text = Table.FieldDescription
            End If

            Dim HFfieldId As HiddenField = e.Item.FindControl("HFfieldId")

            If Not IsNothing(HFfieldId) Then
                HFfieldId.Value = Table.FieldId
            End If

            'Localizzazione
            'Dim Lit As Literal = e.Item.FindControl("LTname_t")
            'If Not IsNothing(Lit) Then
            '    Lit.Text = Resource.getValue("")
            'End If
            'Lit = e.Item.FindControl("LT..")
            'If Not IsNothing(Lit) Then
            '    Lit.Text = Resource.getValue("")
            'End If

            Dim RPT As Repeater = e.Item.FindControl("RPTtableHeader")
            If Not IsNothing(RPT) Then
                RPT.DataSource = Table.HeadValues
                RPT.DataBind()
            End If

            RPT = e.Item.FindControl("RTPtable")
            If Not IsNothing(RPT) Then
                RPT.DataSource = Table.Rows
                RPT.DataBind()
            End If

            Dim Lit As Literal = e.Item.FindControl("LTcolSpan")
            If Not IsNothing(Lit) Then
                Lit.Text = String.Format(Lit.Text, _colSpan + 1)
                'ToDo: rivedere la conta!!!!
            End If


            LBL = e.Item.FindControl("LBLfielddescription")
            If Not IsNothing(LBL) Then
                LBL.Text = Table.FieldDescription
            End If



            Lit = e.Item.FindControl("LTtotalReqFoot")
            If Not IsNothing(Lit) Then
                Lit.Text = Table.TotalRequired.ToString(CurrencyFormat, System.Globalization.CultureInfo.CreateSpecificCulture(CurrencyCulture)).Remove(0, 2)
            End If

            Lit = e.Item.FindControl("LTAdmitTotalFoot")
            If Not IsNothing(Lit) Then
                Lit.Text = Table.TotalAdmit.ToString(CurrencyFormat, System.Globalization.CultureInfo.CreateSpecificCulture(CurrencyCulture)).Remove(0, 2)
            End If

            Lit = e.Item.FindControl("LTadmitMax")
            If Not IsNothing(Lit) Then
                Lit.Text = Table.AdmitMax.ToString(CurrencyFormat, System.Globalization.CultureInfo.CreateSpecificCulture(CurrencyCulture)).Remove(0, 2)
            End If


        End If
    End Sub

    Public Sub RPTtableHeader_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim Value As String = e.Item.DataItem

            Dim Lit As Literal = e.Item.FindControl("LTcontentHead")
            If Not IsNothing(Lit) Then
                Lit.Text = Value
            End If
        End If
    End Sub

    Private _colSpan As Integer = 2

    Public Sub RTPtable_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim RowItem As Eco.dto.dtoEcoEvlItem = e.Item.DataItem

            If IsNothing(RowItem) Then
                Exit Sub
            End If

            Dim HFfieldId As HiddenField = e.Item.FindControl("HFitmId")

            If Not IsNothing(HFfieldId) Then
                HFfieldId.Value = RowItem.itmId
            End If

            Dim RPT As Repeater = e.Item.FindControl("RPTtableValue")
            If Not IsNothing(RPT) Then
                RPT.DataSource = RowItem.values
                RPT.DataBind()
            End If

            _colSpan = RowItem.values.Count()

            Dim Lit As Literal = e.Item.FindControl("LTquantity")
            If Not IsNothing(Lit) Then
                Lit.Text = RowItem.ReqQuantity
            End If

            Lit = e.Item.FindControl("LTunitPrice")
            If Not IsNothing(Lit) Then
                Lit.Text = RowItem.ReqPrice.ToString(CurrencyFormat, System.Globalization.CultureInfo.CreateSpecificCulture(CurrencyCulture)).Remove(0, 2)
            End If

            Lit = e.Item.FindControl("LTtotal")
            If Not IsNothing(Lit) Then
                Lit.Text = RowItem.ReqTotal.ToString(CurrencyFormat, System.Globalization.CultureInfo.CreateSpecificCulture(CurrencyCulture)).Remove(0, 2)
            End If

            Dim Cbx As CheckBox = e.Item.FindControl("CBXadmit")
            If Not IsNothing(Cbx) Then
                Cbx.Checked = RowItem.isAdmit
                Cbx.Enabled = _canModify
            End If

            Dim Txb As TextBox = e.Item.FindControl("TXBadmitValue")
            If Not IsNothing(Txb) Then
                Txb.Text = RowItem.AdmitTotal.ToString(CurrencyFormat, System.Globalization.CultureInfo.CreateSpecificCulture(CurrencyCulture)).Remove(0, 2)
                Txb.Enabled = _canModify
            End If


            Txb = e.Item.FindControl("TXBadmitQntt")
            If Not IsNothing(Txb) Then
                Txb.Text = RowItem.AdmitQuantity
                Txb.Enabled = _canModify
            End If

            Txb = e.Item.FindControl("TXBcomment")
            If Not IsNothing(Txb) Then
                Txb.Text = RowItem.Comment
                Txb.Enabled = _canModify
            End If


        End If
    End Sub

    Public Sub RPTtableValue_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim Value As String = e.Item.DataItem

            Dim Lit As Literal = e.Item.FindControl("LTcontent")
            If Not IsNothing(Lit) Then
                Lit.Text = Value
            End If
        End If
    End Sub

    Private Sub LKBSave_Click(sender As Object, e As EventArgs) Handles LKBSave.Click
        Save(True)
    End Sub

    Private Sub Save(refresh As Boolean)

        Dim evalValues As New List(Of Eco.dto.dtoEcoEvlItem)

        For Each Item As RepeaterItem In Me.RPTtables.Items
            If (Item.ItemType = ListItemType.Item OrElse Item.ItemType = ListItemType.AlternatingItem) Then

                Dim RTPtable As Repeater = Item.FindControl("RTPtable")

                If Not IsNothing(RTPtable) Then
                    For Each itm As RepeaterItem In RTPtable.Items
                        If (itm.ItemType = ListItemType.Item OrElse itm.ItemType = ListItemType.AlternatingItem) Then

                            Dim Eval As Eco.dto.dtoEcoEvlItem = New dtoEcoEvlItem()

                            Dim HFitmId As HiddenField = itm.FindControl("HFitmId")

                            If Not IsNothing(HFitmId) Then
                                Try
                                    Eval.itmId = System.Convert.ToInt64(HFitmId.Value)
                                Catch ex As Exception

                                End Try
                            End If

                            Dim Cbx As CheckBox = itm.FindControl("CBXadmit")
                            If Not IsNothing(Cbx) Then
                                Eval.isAdmit = Cbx.Checked
                            End If




                            Dim Txb As TextBox = itm.FindControl("TXBadmitValue")
                            If Not IsNothing(Txb) Then
                                Try
                                    Eval.AdmitTotal = System.Convert.ToDouble(Txb.Text)
                                Catch ex As Exception

                                End Try

                            End If


                            Txb = itm.FindControl("TXBadmitQntt")
                            If Not IsNothing(Txb) Then
                                Try
                                    Eval.AdmitQuantity = System.Convert.ToDouble(Txb.Text)
                                Catch ex As Exception

                                End Try

                            End If

                            Txb = itm.FindControl("TXBcomment")
                            If Not IsNothing(Txb) Then
                                Try
                                    Eval.Comment = Txb.Text
                                Catch ex As Exception

                                End Try

                            End If

                            evalValues.Add(Eval)


                            'For Each c As Control In Item.Controls

                            '    If GetType(HiddenField) = c.[GetType]() Then
                            '        Dim HFitmId2 As HiddenField = DirectCast(c, HiddenField)
                            '        Try
                            '            Eval.itmId = System.Convert.ToInt64(HFitmId.Value)
                            '        Catch ex As Exception
                            '        End Try

                            '    ElseIf GetType(CheckBox) = c.[GetType]() Then
                            '        Dim checkBox As CheckBox = DirectCast(c, CheckBox)
                            '        If Not IsNothing(c) Then
                            '            Eval.isAdmit = checkBox.Checked
                            '        End If

                            '    ElseIf GetType(TextBox) = c.[GetType]() Then
                            '        Dim Txb2 As TextBox = DirectCast(c, TextBox)

                            '        If Not IsNothing(Txb2) Then
                            '            Try
                            '                Eval.AdmitTotal = System.Convert.ToDouble(Txb2.Text)
                            '            Catch ex As Exception

                            '            End Try

                            '        End If


                            '    End If
                            'Next

                        End If
                    Next


                End If








            End If

        Next

        Me.CurrentPresenter.SaveEvaluation(evalValues, refresh)
    End Sub

    Private Sub LKBdraft_Click(sender As Object, e As EventArgs) Handles LKBdraft.Click


        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.EvaluationSaveDraft,
                Me.EvalId,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.Evaluation,
                "Economic")

        Me.CurrentPresenter.DraftEvaluation()
    End Sub

    Private Sub LKBconfirm_Click(sender As Object, e As EventArgs) Handles LKBconfirm.Click

        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvEvaluationsConfirm,
                Me.EvalId,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.Evaluation,
                "Economic")

        Save(False)

        Me.CurrentPresenter.CompleteEvaluation()
    End Sub

    Private Sub LKBClose_Click(sender As Object, e As EventArgs) Handles LKBClose.Click

        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvEvaluationsConfirm,
                Me.EvalId,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.Evaluation,
                "Economic")

        Me.CurrentPresenter.CloseEvaluation()
    End Sub

#End Region



#Region "Action"

    Private Sub SendUserAction(
                              idCommunity As Integer,
                              idModule As Integer,
                              action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType,
                              objectType As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType,
                              IdObject As String) _
                Implements iViewEcoEvaluation.SendUserAction

        PageUtility.AddActionToModule(idCommunity,
                                      idModule,
                                      action,
                                      PageUtility.CreateObjectsList(
                                        idModule,
                                        objectType,
                                        IdObject),
                                        InteractionType.UserWithLearningObject)
    End Sub


#End Region

End Class