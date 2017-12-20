Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports COL_Questionario

Public Class GestioneDomande
    Inherits PageBaseQuestionario

    Private _ApplicationUrlBase As String
    Private _SmartTagsAvailable As SmartTags

    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            If _SmartTagsAvailable Is Nothing Then
                _SmartTagsAvailable = ManagerConfiguration.GetSmartTags(_ApplicationUrlBase)
            End If
            Return _SmartTagsAvailable
        End Get
    End Property
    Sub New()

    End Sub
    Sub New(ByVal iApplicationUrlBase As String)
        _ApplicationUrlBase = iApplicationUrlBase
    End Sub




#Region "Action"
    Public Sub CreateActionAdd()
        Select Case Me.QuestionarioCorrente.tipo
            Case Questionario.TipoQuestionario.Sondaggio
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CreateSondaggio, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Questionario
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CreateQuestionario, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Meeting
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CreateMeetingPoll, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.LibreriaDiDomande
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CreateLibreria, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Autovalutazione
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CreateTestAutovalutazione, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Else
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CreateElse, , lm.ActionDataContract.InteractionType.UserWithUser)
        End Select
    End Sub
    Public Sub CompileStartActionAdd()
        Select Case Me.QuestionarioCorrente.tipo
            Case Questionario.TipoQuestionario.Questionario
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileStartQuestionario, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Sondaggio
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileStartSondaggio, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Meeting
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileStartMeetingPoll, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Autovalutazione
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileStartTestAutovalutazione, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Else
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileStartElse, , lm.ActionDataContract.InteractionType.UserWithUser)
        End Select
    End Sub
    Public Sub CompileEndActionAdd()
        Select Case Me.QuestionarioCorrente.tipo
            Case Questionario.TipoQuestionario.Sondaggio
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileEndSondaggio, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Questionario
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileEndQuestionario, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Meeting
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileEndMeetingPoll, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Autovalutazione
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileEndTestAutovalutazione, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Else
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.CompileEndElse, , lm.ActionDataContract.InteractionType.UserWithUser)
        End Select
    End Sub
    Public Sub DeleteOneAnswerActionAdd()
        Select Case Me.QuestionarioCorrente.tipo
            Case Questionario.TipoQuestionario.Sondaggio
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteOneAnswerSondaggio, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Questionario
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteOneAnswerQuestionario, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Meeting
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteOneAnswerMeetingPoll, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Autovalutazione
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteOneAnswerTestAutovalutazione, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Else
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteOneAnswerElse, , lm.ActionDataContract.InteractionType.UserWithUser)
        End Select
    End Sub
    Public Sub DeleteAllAnswersActionAdd()
        Select Case Me.QuestionarioCorrente.tipo
            Case Questionario.TipoQuestionario.Sondaggio
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteAllAnswersSondaggio, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Questionario
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteAllAnswersQuestionario, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Meeting
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteAllAnswersMeetingPoll, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Autovalutazione
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteAllAnswersTestAutovalutazione, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Else
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteAllAnswersElse, , lm.ActionDataContract.InteractionType.UserWithUser)
        End Select
    End Sub
    Public Sub DeleteActionAdd()
        Select Case Me.QuestionarioCorrente.tipo
            Case Questionario.TipoQuestionario.Questionario
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteQuestionario, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Sondaggio
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteSondaggio, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Meeting
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteMeetingPoll, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.LibreriaDiDomande
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteLibreria, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Autovalutazione
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteTestAutovalutazione, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Else
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.DeleteElse, , lm.ActionDataContract.InteractionType.UserWithUser)
        End Select
    End Sub
    Public Sub ViewListActionAdd(ByVal questType As Integer)
        Select Case questType
            Case Questionario.TipoQuestionario.Questionario
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.QuestionariList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Sondaggio
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.SondaggiList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Meeting
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.MeetingPollList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.LibreriaDiDomande
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.LibrerieList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Autovalutazione
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.TestAutovalutazioneList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Else
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.ElseList, , lm.ActionDataContract.InteractionType.UserWithUser)
        End Select
    End Sub
    Public Sub ViewAdminListActionAdd(ByVal questType As Integer)
        Select Case questType
            Case Questionario.TipoQuestionario.Questionario
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.QuestionariAdminList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Sondaggio
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.SondaggiAdminList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Meeting
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.MeetingPollAdminList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.LibreriaDiDomande
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.LibrerieAdminList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Questionario.TipoQuestionario.Autovalutazione
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.TestAutovalutazioneAdminList, , lm.ActionDataContract.InteractionType.UserWithUser)
            Case Else
                Me.PageUtility.AddAction(COL_Questionario.ModuleQuestionnaire.ActionType.ElseAdminList, , lm.ActionDataContract.InteractionType.UserWithUser)
        End Select
    End Sub


    Public Sub AddAction(idItem As Integer, ByVal oType As COL_Questionario.ModuleQuestionnaire.ObjectType, ByVal action As COL_Questionario.ModuleQuestionnaire.ActionType, type As QuestionnaireType)
        Select Case type
            Case QuestionnaireType.QuestionLibrary
                Select Case action
                    Case ModuleQuestionnaire.ActionType.QuestionAdd
                        AddAction(idItem, oType, ModuleQuestionnaire.ActionType.QuestionAddToLibrary)
                    Case ModuleQuestionnaire.ActionType.QuestionDelete
                        AddAction(idItem, oType, ModuleQuestionnaire.ActionType.QuestionDeleteFromLibrary)
                    Case ModuleQuestionnaire.ActionType.QuestionEdit
                        AddAction(idItem, oType, ModuleQuestionnaire.ActionType.QuestionEditFromLibrary)
                    Case ModuleQuestionnaire.ActionType.QuestionVirtualRemove
                        AddAction(idItem, oType, ModuleQuestionnaire.ActionType.QuestionVirtualRemoveFromLibrary)
                    Case Else
                        AddAction(idItem, oType, action)
                End Select
            Case Else
                AddAction(idItem, oType, action)
        End Select
    End Sub
    Public Sub AddAction(idItem As Integer, ByVal oType As COL_Questionario.ModuleQuestionnaire.ObjectType, ByVal action As COL_Questionario.ModuleQuestionnaire.ActionType)
        Me.PageUtility.AddAction(action, PageUtility.CreateObjectsList(oType, idItem), lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub


#End Region
#Region "Interfaccia Domande"

    ' carica dinamicamente lo user control per la visualizzazione della domanda in base al tipo
    Public Function loadDomandeOpzioni(ByVal oQuest As Questionario, ByVal iPag As String, ByVal iDom As String, ByVal isReadOnly As Boolean)


        Dim ctrl As New Control
        Try
            Select Case oQuest.pagine(iPag).domande.Item(iDom).tipo
                Case Domanda.TipoDomanda.Multipla And oQuest.pagine(iPag).domande.Item(iDom).isMultipla
                    ctrl = addDomandaMultipla(oQuest, iPag, iDom, isReadOnly)
                Case Domanda.TipoDomanda.Multipla And Not oQuest.pagine(iPag).domande.Item(iDom).isMultipla
                    ctrl = addDomandaSingola(oQuest, iPag, iDom, isReadOnly)
                Case Domanda.TipoDomanda.DropDown
                    ctrl = addDomandaDropDown(oQuest, iPag, iDom, isReadOnly)
                Case Domanda.TipoDomanda.Rating
                    ctrl = addDomandaRating(oQuest, iPag, iDom, isReadOnly)
                Case Domanda.TipoDomanda.Meeting
                    ctrl = addDomandaMeeting(oQuest, iPag, iDom, isReadOnly)
                Case Domanda.TipoDomanda.TestoLibero
                    ctrl = addDomandaTestoLibero(oQuest, iPag, iDom, isReadOnly)
                Case Domanda.TipoDomanda.Numerica
                    ctrl = addDomandaNumerica(oQuest, iPag, iDom, isReadOnly)
            End Select

        Catch ex As Exception
        End Try

        Return ctrl
    End Function

    ' imposta la visualizzazione della domanda, carica le checkbox, imposta gli stili..
    Protected Function addDomandaMultipla(ByVal oQuest As Questionario, ByVal iPag As String, ByVal iDom As String, ByVal isReadOnly As Boolean) As Control
        Try
            Dim TBLDomandaMultipla As New Table
            TBLDomandaMultipla.ID = "TBLDomandaMultipla_" + iDom
            TBLDomandaMultipla.CssClass = "questionmultiple"
            Dim counter As Integer = 0
            For Each oOpzione As DomandaOpzione In oQuest.pagine(iPag).domande.Item(iDom).domandaMultiplaOpzioni
                Dim riga As New TableRow
                Dim cella As New TableCell

                Dim LBOpzione As New Label
                Dim CBOpzione As New CheckBox
                CBOpzione.CssClass = "selection"
                CBOpzione.Enabled = Not isReadOnly
                cella.Controls.Add(CBOpzione)

                Dim oDiv As New HtmlGenericControl("div")
                oDiv.Attributes.Add("class", "inlinewrapper")

                LBOpzione.CssClass = "answer renderedtext"
                LBOpzione.Text = SmartTagsAvailable.TagAll(oOpzione.testo) + "  "
                cella.CssClass = "Risposte"
                oDiv.Controls.Add(LBOpzione)
                If oQuest.pagine(iPag).domande.Item(iDom).domandaMultiplaOpzioni.Item(counter).isAltro = True Then
                    Dim txbTestoRisposta As New TextBox
                    txbTestoRisposta.MaxLength = 250
                    txbTestoRisposta.Enabled = Not isReadOnly
                    txbTestoRisposta.CssClass = "inputother"
                    oDiv.Controls.Add(txbTestoRisposta)
                End If

                'If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
                If Me.QuestionarioCorrente.visualizzaSuggerimenti Then
                    Dim LBSuggerimentoOpzione As New Label
                    LBSuggerimentoOpzione.ID = "suggestion" & counter.ToString
                    LBSuggerimentoOpzione.Style.Item("font-style") = "italic"
                    LBSuggerimentoOpzione.CssClass = "suggestion"
                    oDiv.Controls.Add(LBSuggerimentoOpzione)
                End If
                cella.Controls.Add(oDiv)
                riga.Cells.Add(cella)
                TBLDomandaMultipla.Rows.Add(riga)
                counter = counter + 1
            Next
            If Not oQuest.pagine(iPag).domande.Item(iDom).isValida Then
                Dim LBtroppeRisposte As New Label
                MyBase.SetCulture("pg_QuestionarioCompile", "Questionari")
                LBtroppeRisposte.Text = Me.Resource.getValue("LBtroppeRisposte.text")
                If oQuest.pagine(iPag).domande.Item(iDom).numeroMaxRisposte > 0 Then
                    LBtroppeRisposte.Text &= String.Format(Me.Resource.getValue("MSGnumMaxRisposte"), oQuest.pagine(iPag).domande.Item(iDom).numeroMaxRisposte)
                End If
                LBtroppeRisposte.ForeColor = Color.Red
                LBtroppeRisposte.ID = "LBtroppeRisposte_" + iDom.ToString
                Dim riga2 As New TableRow
                Dim cella2 As New TableCell
                cella2.Controls.Add(LBtroppeRisposte)
                riga2.Cells.Add(cella2)
                TBLDomandaMultipla.Rows.Add(riga2)
            End If
            Return TBLDomandaMultipla
        Catch ex As Exception
            Dim errore As String
            errore = ex.Message
        End Try
    End Function

    'Public Function numeroMaxRisposte_Validate(ByRef TBLDomadaMultipla, ByRef numeroMaxRisposte) As Boolean
    '    Try
    '        Dim quanteChecked As Int32 = 0
    '        For Each row As TableRow In TBLDomadaMultipla.rows
    '            For Each ctrl As Control In row.Cells(0).Controls
    '                If ctrl.GetType() Is GetType(CheckBox) Then
    '                    Dim CBopzione As New CheckBox
    '                    CBopzione = DirectCast(ctrl, CheckBox)
    '                    If CBopzione.Checked Then
    '                        quanteChecked += 1
    '                        If quanteChecked > numeroMaxRisposte Then
    '                            Return False
    '                        End If
    '                    End If
    '                End If

    '            Next
    '        Next
    '        Return True
    '    Catch ex As Exception
    '        Dim errore As String
    '        errore = ex.Message
    '    End Try
    'End Function

    ' carica lo user control domanda singola
    Protected Function addDomandaSingola(ByVal oQuest As Questionario, ByVal iPag As String, ByVal iDom As String, ByVal isReadOnly As Boolean) As Control
        Dim TBLDomandaSingola As New Table
        TBLDomandaSingola.ID = "TBLDomandaSingola_" + iDom
        TBLDomandaSingola.CssClass = "questionsingle"
        Dim counter As New Integer
        For Each oOpzione As DomandaOpzione In oQuest.pagine(iPag).domande.Item(iDom).domandaMultiplaOpzioni
            Dim riga As New TableRow
            Dim cella As New TableCell
            Dim LBOpzione As New Label
            Dim RBOpzione As New RadioButton
            RBOpzione.GroupName = "RBGOpzione"
            RBOpzione.Enabled = Not isReadOnly
            RBOpzione.CssClass = "selection"
 
            cella.CssClass = "Risposte"
            cella.Controls.Add(RBOpzione)

            Dim oDiv As New HtmlGenericControl("div")
            oDiv.Attributes.Add("class", "inlinewrapper")

            LBOpzione.Text = SmartTagsAvailable.TagAll(oOpzione.testo) + "  "
            LBOpzione.CssClass = "answer renderedtext"
            oDiv.Controls.Add(LBOpzione)



            'Dim cella2 As New TableCell
            If oQuest.pagine(iPag).domande.Item(iDom).domandaMultiplaOpzioni.Item(counter).isAltro = True Then
                Dim txbTestoRisposta As New TextBox
                txbTestoRisposta.Enabled = Not isReadOnly
                txbTestoRisposta.CssClass = "inputother"
                oDiv.Controls.Add(txbTestoRisposta)
            End If
            'If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
            If Me.QuestionarioCorrente.visualizzaSuggerimenti Then
                Dim LBSuggerimentoOpzione As New Label
                LBSuggerimentoOpzione.ID = "suggestion" & counter.ToString
                LBSuggerimentoOpzione.Style.Item("font-style") = "italic"
                LBSuggerimentoOpzione.CssClass = "suggestion"
                oDiv.Controls.Add(LBSuggerimentoOpzione)
            End If
            cella.Controls.Add(oDiv)
            riga.Cells.Add(cella)
            TBLDomandaSingola.Rows.Add(riga)
            counter = counter + 1
        Next
        Return TBLDomandaSingola


    End Function

    ' carica lo user control domanda drop down
    Protected Function addDomandaDropDown(ByVal oQuest As Questionario, ByVal iPag As String, ByVal iDom As String, ByVal isReadOnly As Boolean) As Control

        Dim ctrl As New Control
        ctrl = Page.LoadControl(RootObject.ucDomandaDropDown)
        ctrl.ID = "UCDomandaDropDown_" + iDom
        Dim labelDrop As New Label
        labelDrop = DirectCast(ctrl.Controls(0).FindControl("LBEtichettaDropDown"), Label)
        labelDrop.Text = oQuest.pagine(iPag).domande.Item(iDom).domandaDropDown.etichetta
        Dim ddl As New DropDownList
        ddl = DirectCast(ctrl.Controls(0).FindControl("DDLOpzioni"), DropDownList)
        ddl.DataSource = oQuest.pagine(iPag).domande.Item(iDom).domandaDropDown.dropDownItems
        ddl.DataTextField = "testo"
        ddl.DataValueField = "numero"
        ddl.DataBind()
        ddl.Enabled = Not isReadOnly
        Return ctrl

    End Function

    ' carica lo user control domanda testo libero
    Protected Function addDomandaTestoLibero(ByVal oQuest As Questionario, ByVal iPag As String, ByVal iDom As String, ByVal isReadOnly As Boolean) As Control

        Dim TBLTestoLibero As New Table
        TBLTestoLibero.ID = "TBLTestoLibero_" + iDom
        TBLTestoLibero.CssClass = "questionfreetext"
        For Each oOpzione As DomandaTestoLibero In oQuest.pagine(iPag).domande.Item(iDom).opzioniTestoLibero
            Dim row As New TableRow
            Dim oCell As New TableCell
            If Not String.IsNullOrEmpty(oOpzione.etichetta) Then
                Dim hRow As New TableRow
                Dim oLabel As New Label
                oLabel.CssClass = "answer renderedtext"
                oLabel.Text = SmartTagsAvailable.TagAll(oOpzione.etichetta) + "  "
                oCell.Controls.Add(oLabel)
                hRow.Cells.Add(oCell)
                TBLTestoLibero.Rows.Add(hRow)
            End If

            Dim oCell2 As New TableCell
            Dim TXBTesto As New TextBox
            TXBTesto.Width = 500
            TXBTesto.TextMode = TextBoxMode.MultiLine
            TXBTesto.Rows = 3
            TXBTesto.Enabled = Not isReadOnly
            oCell2.Controls.Add(TXBTesto)
            row.Cells.Add(oCell2)

            TBLTestoLibero.Rows.Add(row)
        Next
        Return TBLTestoLibero
    End Function

    ' carica lo user control domanda numerica
    Protected Function addDomandaNumerica(ByVal oQuest As Questionario, ByVal iPag As String, ByVal iDom As String, ByVal isReadOnly As Boolean) As Control

        Dim TBLNumerica As New Table
        TBLNumerica.ID = "TBLNumerica_" + iDom
        TBLNumerica.CssClass = "questionnumeric"
        Dim iOpzione As Integer = 0

        For Each oOpzione As DomandaNumerica In oQuest.pagine(iPag).domande.Item(iDom).opzioniNumerica
            Dim row As New TableRow
            Dim cell1 As New TableCell
            Dim cell2 As New TableCell

            'ATTENZIONE!!
            'se si modifica la posiozione dei controlli va aggiornato setRisposte e setRisposteCorrette
            Dim lbTestoPrima As New Label
            lbTestoPrima.Text = SmartTagsAvailable.TagAll(oOpzione.testoPrima)
            cell1.CssClass = "Risposte"
            cell1.Controls.Add(lbTestoPrima)

            Dim LBRispostaErrata As New Label
            LBRispostaErrata.Visible = False
            cell1.Controls.Add(LBRispostaErrata)
            Dim LBRispostaCorretta As New Label
            LBRispostaCorretta.Visible = False
            cell1.Controls.Add(LBRispostaCorretta)


            Dim txbTesto As New TextBox
            txbTesto.ID = "TXBTestoNumerica_" + iOpzione.ToString()
            txbTesto.Enabled = Not isReadOnly
            cell2.Controls.Add(txbTesto)

            Dim lbTestoDopo As New Label
            lbTestoDopo.Text = oOpzione.testoDopo
            cell2.CssClass = "Risposte"
            cell2.Controls.Add(lbTestoDopo)

            Dim cvNumero As New CompareValidator
            cvNumero.ID = "COVOpzioneNumerica_" + iOpzione.ToString()
            cvNumero.Operator = ValidationCompareOperator.DataTypeCheck
            cvNumero.Type = ValidationDataType.Double
            cvNumero.Display = ValidatorDisplay.Dynamic
            cvNumero.ControlToValidate = "TXBTestoNumerica_" + iOpzione.ToString()

            Dim RVisOverflow As New RangeValidator
            RVisOverflow.ControlToValidate = "TXBTestoNumerica_" + iOpzione.ToString()
            RVisOverflow.ID = "CVisOverflowOpzioneNumerica_" + iOpzione.ToString()
            RVisOverflow.Type = ValidationDataType.Double
            RVisOverflow.Display = ValidatorDisplay.Dynamic
            RVisOverflow.MinimumValue = RootObject.ValidatorMaxDouble * -1
            RVisOverflow.MaximumValue = RootObject.ValidatorMaxDouble

            MyBase.SetCulture("pg_ucDomandaNumericaEdit", "Questionari")
            cvNumero.ErrorMessage = Me.Resource.getValue("cvNumero")
            RVisOverflow.ErrorMessage = Me.Resource.getValue("CVisOverflow")


            cell2.Controls.Add(cvNumero)
            cell2.Controls.Add(RVisOverflow)

            row.Cells.Add(cell1)
            row.Cells.Add(cell2)

            TBLNumerica.Rows.Add(row)

            iOpzione = iOpzione + 1
        Next

        'Dim lbTestoPrima As New Label
        'lbTestoPrima = DirectCast(ctrl.FindControl("LBTestoPrima"), Label)
        'lbTestoPrima.Text = oQuest.pagine(iPag).domande.Item(iDom).domandaNumerica.testoPrima

        'Dim lbTestoDopo As New Label
        'lbTestoDopo = DirectCast(ctrl.FindControl("LBTestoDopo"), Label)
        'lbTestoDopo.Text = oQuest.pagine(iPag).domande.Item(iDom).domandaNumerica.testoDopo

        Return TBLNumerica
    End Function

    ' carica lo user control domanda rating
    Protected Function addDomandaRating(ByVal oQuest As Questionario, ByVal iPag As String, ByVal iDom As String, ByVal isReadOnly As Boolean) As Control

        'Dim dlOpzioniRating As New PlaceHolder
        'dlOpzioniRating.ID = "PHOpzioniRating"

        Dim tabella As New Table
        tabella.ID = "TBLRadiobutton_" + iDom
        tabella.CssClass = "questionrating"
        tabella.BorderWidth = 1
        tabella.Width = 810
        tabella.CellPadding = 10
        tabella.CellSpacing = 0
        'tabella.GridLines = GridLines.Both

        Dim numeroRating As Integer = oQuest.pagine(iPag).domande.Item(iDom).domandaRating.numeroRating

        Dim rigaInt As New TableRow
        'If oQuest.pagine(iPag).domande.Item(iDom).domandaRating.opzioniRating.Count > 1 Then
        If Not (oQuest.pagine(iPag).domande.Item(iDom).domandaRating.opzioniRating.Count = 1 And oQuest.pagine(iPag).domande.Item(iDom).domandaRating.opzioniRating(0).testo = String.Empty) Then
            Dim cellaInt1 As New TableCell
            cellaInt1.CssClass = "CellaVuota"
            cellaInt1.Width = 0
            rigaInt.Cells.Add(cellaInt1)
        End If


        If oQuest.pagine(iPag).domande.Item(iDom).domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Numerazione Then
            For i As Integer = 1 To oQuest.pagine(iPag).domande.Item(iDom).domandaRating.numeroRating
                Dim cella As New TableCell
                cella.CssClass = "CellaRisposta"
                'cella.HorizontalAlign = HorizontalAlign.Center
                Dim lbTesto As New Label
                lbTesto.Text = i
                'lbTesto.Font.Italic = True
                cella.Controls.Add(lbTesto)
                rigaInt.Cells.Add(cella)
            Next

        Else
            For Each oInt As DomandaOpzione In oQuest.pagine(iPag).domande.Item(iDom).domandaRating.intestazioniRating
                Dim cella As New TableCell
                ' cella.HorizontalAlign = HorizontalAlign.Center
                cella.CssClass = "CellaRisposta"
                Dim lbTesto As New Label

                lbTesto.Text = oInt.testo
                'lbTesto.Font.Italic = True
                cella.Controls.Add(lbTesto)
                rigaInt.Cells.Add(cella)
            Next

        End If

        If oQuest.pagine(iPag).domande.Item(iDom).domandaRating.mostraND Then
            Dim cellaND As New TableCell
            cellaND.CssClass = "CellaRisposta"
            cellaND.HorizontalAlign = HorizontalAlign.Left
            Dim lbTestoND As New Label
            lbTestoND.Text = oQuest.pagine(iPag).domande.Item(iDom).domandaRating.testoND
            cellaND.Controls.Add(lbTestoND)
            rigaInt.Cells.Add(cellaND)
            numeroRating = numeroRating + 1
        End If


        tabella.Rows.Add(rigaInt)

        Dim indiceRiga As Integer = 1

        For Each oOpzione As DomandaOpzione In oQuest.pagine(iPag).domande.Item(iDom).domandaRating.opzioniRating

            Dim riga As New TableRow
            If Not (oQuest.pagine(iPag).domande.Item(iDom).domandaRating.opzioniRating.Count = 1 And oOpzione.testo = String.Empty) Then
                Dim cella As New TableCell
                cella.CssClass = "CellaDomanda"
                Dim LBOpzione As New Label
                Dim TBisAltro As New TextBox
                TBisAltro.MaxLength = 250
                TBisAltro.Enabled = Not isReadOnly
                LBOpzione.Text = oOpzione.testo + "  "
                cella.Controls.Add(LBOpzione)
                If oOpzione.isAltro Then
                    cella.Controls.Add(TBisAltro)
                End If
                riga.Cells.Add(cella)
            End If

            For c As Integer = 1 To numeroRating
                Dim cellaRB As New TableCell
                cellaRB.CssClass = "CellaRisposta"
                Dim rbOpzione As New RadioButton
                rbOpzione.GroupName = "RBGOpzione_" + indiceRiga.ToString()
                Try
                    rbOpzione.Visible = Boolean.Parse(oOpzione.arrayCBisVisible.Chars(c - 1) = "0")
                Catch
                    rbOpzione.Visible = True
                End Try
                rbOpzione.Enabled = Not isReadOnly
                cellaRB.Controls.Add(rbOpzione)
                riga.Cells.Add(cellaRB)
            Next

            tabella.Rows.Add(riga)

            indiceRiga = indiceRiga + 1

        Next


        Return tabella

    End Function

    Protected Function addDomandaRatingStars(ByVal oQuest As Questionario, ByVal iPag As String, ByVal iDom As String, ByVal isReadOnly As Boolean) As Control

        'Dim dlOpzioniRating As New PlaceHolder
        'dlOpzioniRating.ID = "PHOpzioniRating"

        Dim tabella As New Table
        tabella.ID = "TBLRadiobutton_" + iDom
        tabella.CssClass = "questionrating"
        tabella.BorderWidth = 1
        tabella.Width = 810
        tabella.CellPadding = 10
        tabella.CellSpacing = 0

        Dim numeroRating As Integer = oQuest.pagine(iPag).domande.Item(iDom).domandaRating.numeroRating

        Dim indiceRiga As Integer = 1

        For Each oOpzione As DomandaOpzione In oQuest.pagine(iPag).domande.Item(iDom).domandaRating.opzioniRating

            Dim riga As New TableRow
            If Not (oQuest.pagine(iPag).domande.Item(iDom).domandaRating.opzioniRating.Count = 1 And oOpzione.testo = String.Empty) Then
                Dim cella As New TableCell
                cella.CssClass = "CellaDomanda"
                Dim LBOpzione As New Label

                LBOpzione.Text = oOpzione.testo + "  "
                cella.Controls.Add(LBOpzione)

                riga.Cells.Add(cella)
            End If

            Dim cellaRB As New TableCell
            cellaRB.CssClass = "CellaRisposta"

            Dim rdStars As New Telerik.Web.UI.RadRating

            Try

                'Tentativo recupero dato
                Dim oRisposta As New RispostaQuestionario

                Dim ris As New RispostaDomanda
                If oQuest.pagine(iPag).domande.Item(iDom).risposteDomanda.Count > 0 Then
                    ris = oRisposta.findRispostaByNumeroOpzione(oQuest.pagine(iPag).domande.Item(iDom).risposteDomanda, oQuest.pagine(iPag).domande.Item(iDom).domandaRating.opzioniRating(indiceRiga - 1).numero)
                End If

                rdStars.Value = CInt(ris.valore)

                'end tentativo
            Catch ex As Exception
                rdStars.Value = 0
            End Try


            rdStars.ItemCount = numeroRating
            rdStars.SelectionMode = Telerik.Web.UI.RatingSelectionMode.Continuous
            rdStars.Precision = Telerik.Web.UI.RatingPrecision.Item
            rdStars.Orientation = System.Web.UI.WebControls.Orientation.Horizontal
            rdStars.ReadOnly = isReadOnly

            cellaRB.Controls.Add(rdStars)

            riga.Cells.Add(cellaRB)

            tabella.Rows.Add(riga)

            indiceRiga = indiceRiga + 1

        Next


        Return tabella

    End Function

    ' carica lo user control domanda meeting
    Protected Function addDomandaMeeting(ByVal oQuest As Questionario, ByVal iPag As String, ByVal iDom As String, ByVal isReadOnly As Boolean) As Control

        'Dim dlOpzioniRating As New PlaceHolder
        'dlOpzioniRating.ID = "PHOpzioniRating"

        Dim tabella As New Table
        tabella.ID = "TBLRadiobutton_" + iDom
        tabella.BorderWidth = 1
        tabella.Width = 810
        tabella.CellPadding = 10
        tabella.CellSpacing = 0
        'tabella.GridLines = GridLines.Both

        Dim numeroRating As Integer = oQuest.pagine(iPag).domande.Item(iDom).domandaRating.numeroMeeting

        Dim rigaInt As New TableRow
        'If oQuest.pagine(iPag).domande.Item(iDom).domandaRating.opzioniRating.Count > 1 Then
        If Not (oQuest.pagine(iPag).domande.Item(iDom).domandaRating.opzioniRating.Count = 1 And oQuest.pagine(iPag).domande.Item(iDom).domandaRating.opzioniRating(0).testo = String.Empty) Then
            Dim cellaInt1 As New TableCell
            cellaInt1.CssClass = "CellaVuota"
            cellaInt1.Width = 0
            rigaInt.Cells.Add(cellaInt1)
        End If


        If oQuest.pagine(iPag).domande.Item(iDom).domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Numerazione Then
            For i As Integer = 1 To oQuest.pagine(iPag).domande.Item(iDom).domandaRating.numeroRating
                Dim cella As New TableCell
                cella.CssClass = "CellaRisposta"
                'cella.HorizontalAlign = HorizontalAlign.Center
                Dim lbTesto As New Label
                lbTesto.Text = i
                'lbTesto.Font.Italic = True
                cella.Controls.Add(lbTesto)
                rigaInt.Cells.Add(cella)
            Next

        Else
            For Each oInt As DomandaOpzione In oQuest.pagine(iPag).domande.Item(iDom).domandaRating.intestazioniRating
                Dim cella As New TableCell
                ' cella.HorizontalAlign = HorizontalAlign.Center
                cella.CssClass = "CellaRisposta"
                Dim lbTesto As New Label

                lbTesto.Text = oInt.testo
                'lbTesto.Font.Italic = True
                cella.Controls.Add(lbTesto)
                rigaInt.Cells.Add(cella)
            Next

        End If

        If oQuest.pagine(iPag).domande.Item(iDom).domandaRating.mostraND Then
            Dim cellaND As New TableCell
            cellaND.CssClass = "CellaRisposta"
            cellaND.HorizontalAlign = HorizontalAlign.Left
            Dim lbTestoND As New Label
            lbTestoND.Text = oQuest.pagine(iPag).domande.Item(iDom).domandaRating.testoND
            cellaND.Controls.Add(lbTestoND)
            rigaInt.Cells.Add(cellaND)
            numeroRating = numeroRating + 1
        End If


        tabella.Rows.Add(rigaInt)

        Dim indiceRiga As Integer = 1

        For Each oOpzione As DomandaOpzione In oQuest.pagine(iPag).domande.Item(iDom).domandaRating.opzioniRating

            Dim riga As New TableRow
            If Not (oQuest.pagine(iPag).domande.Item(iDom).domandaRating.opzioniRating.Count = 1 And oOpzione.testo = String.Empty) Then
                Dim cella As New TableCell
                cella.CssClass = "CellaDomanda"
                Dim LBOpzione As New Label
                Dim TBisAltro As New TextBox
                TBisAltro.MaxLength = 250
                TBisAltro.Enabled = Not isReadOnly
                LBOpzione.Text = oOpzione.testo + "  "
                cella.Controls.Add(LBOpzione)
                If oOpzione.isAltro Then
                    cella.Controls.Add(TBisAltro)
                End If
                riga.Cells.Add(cella)
            End If

            For c As Integer = 1 To numeroRating
                Dim cellaCB As New TableCell
                cellaCB.CssClass = "CellaRisposta"
                Dim cbOpzione As New CheckBox
                Try
                    If oOpzione.arrayCBisVisible = String.Empty Then
                        cbOpzione.Visible = True
                    Else
                        cbOpzione.Visible = Boolean.Parse(oOpzione.arrayCBisVisible.Chars(c - 1) = "0")
                    End If
                Catch
                    cbOpzione.Visible = True
                End Try
                cbOpzione.Enabled = Not isReadOnly
                cellaCB.Controls.Add(cbOpzione)
                riga.Cells.Add(cellaCB)
            Next

            tabella.Rows.Add(riga)

            indiceRiga = indiceRiga + 1

        Next


        Return tabella

    End Function

    ' sposta su di una posizione la domanda
    Public Function domandaUP(ByVal oQuest As Questionario, ByVal idDomandaUP As String, ByVal idDomandaDown As String)
        'Dim idDomandaUP As String = DirectCast(sender, DataList).DataKeys(e.Item.ItemIndex)
        Dim oDomandaUP As New Domanda
        oDomandaUP = oDomandaUP.findDomandaBYID(oQuest.domande, idDomandaUP)

        If Not idDomandaDown Is Nothing Then
            oDomandaUP.numero = oDomandaUP.numero - 1
            'Dim idDomandaDOWN As String = DirectCast(sender, DataList).DataKeys(e.Item.ItemIndex - 1)
            Dim oDomandaDOWN As New Domanda
            oDomandaDOWN = oDomandaDOWN.findDomandaBYID(oQuest.domande, idDomandaDown)
            oDomandaDOWN.numero = oDomandaDOWN.numero + 1

            DALDomande.DomandaInvertiNumero_Update(idDomandaUP, idDomandaDown, oDomandaUP.numero, oDomandaDOWN.numero, oQuest.id)

        Else
            ' sposto la domanda nella pagina superiore
            If oDomandaUP.numeroPagina - 1 > 0 Then

                ' rimuovo la domanda spostata dalla pagina
                oQuest.pagine(oDomandaUP.numeroPagina - 1).domande.Remove(oDomandaUP)
                ' se la pagina rimane senza domande imposto a 0 i campi DA - A
                If oQuest.pagine(oDomandaUP.numeroPagina - 1).domande.Count > 0 Then
                    oQuest.pagine(oDomandaUP.numeroPagina - 1).dallaDomanda = oDomandaUP.numero + 1
                Else
                    oQuest.pagine(oDomandaUP.numeroPagina - 1).dallaDomanda = 0
                    oQuest.pagine(oDomandaUP.numeroPagina - 1).allaDomanda = 0
                End If
                ' aggiungo la domanda spostata alla pagina sopra
                oQuest.pagine(oDomandaUP.numeroPagina - 2).domande.Add(oDomandaUP)
                oQuest.pagine(oDomandaUP.numeroPagina - 2).allaDomanda = oDomandaUP.numero
                If oQuest.pagine(oDomandaUP.numeroPagina - 2).domande.Count = 1 Then
                    oQuest.pagine(oDomandaUP.numeroPagina - 2).dallaDomanda = oDomandaUP.numero
                End If

                For Each pagina As QuestionarioPagina In oQuest.pagine
                    DALPagine.Pagina_Update(pagina)
                Next


            End If


        End If

        Return oQuest
    End Function

    ' sposta giù di una posizione la domanda
    Public Function domandaDOWN(ByVal oQuest As Questionario, ByVal idDomandaUP As String, ByVal idDomandaDown As String)
        'Dim idDomandaUP As String = DirectCast(sender, DataList).DataKeys(e.Item.ItemIndex)
        Dim oDomandaUP As New Domanda
        oDomandaUP = oDomandaUP.findDomandaBYID(oQuest.domande, idDomandaUP)

        If Not idDomandaDown Is Nothing Then
            oDomandaUP.numero = oDomandaUP.numero + 1

            'Dim idDomandaDOWN As String = DirectCast(sender, DataList).DataKeys(e.Item.ItemIndex + 1)

            Dim oDomandaDOWN As New Domanda
            oDomandaDOWN = oDomandaDOWN.findDomandaBYID(oQuest.domande, idDomandaDown)
            oDomandaDOWN.numero = oDomandaDOWN.numero - 1

            DALDomande.DomandaInvertiNumero_Update(idDomandaUP, idDomandaDown, oDomandaUP.numero, oDomandaDOWN.numero, oQuest.id)


        Else
            If oDomandaUP.numeroPagina < oQuest.pagine.Count Then

                ' rimuovo la domanda spostata dalla pagina
                oQuest.pagine(oDomandaUP.numeroPagina - 1).domande.Remove(oDomandaUP)

                If oQuest.pagine(oDomandaUP.numeroPagina - 1).domande.Count > 0 Then
                    oQuest.pagine(oDomandaUP.numeroPagina - 1).allaDomanda = oDomandaUP.numero - 1
                Else
                    oQuest.pagine(oDomandaUP.numeroPagina - 1).dallaDomanda = 0
                    oQuest.pagine(oDomandaUP.numeroPagina - 1).allaDomanda = 0
                End If

                ' aggiungo la domanda spostata alla pagina sopra
                oQuest.pagine(oDomandaUP.numeroPagina).domande.Add(oDomandaUP)
                oQuest.pagine(oDomandaUP.numeroPagina).dallaDomanda = oDomandaUP.numero
                If oQuest.pagine(oDomandaUP.numeroPagina).domande.Count = 1 Then
                    oQuest.pagine(oDomandaUP.numeroPagina).allaDomanda = oDomandaUP.numero
                End If

                For Each pagina As QuestionarioPagina In oQuest.pagine
                    DALPagine.Pagina_Update(pagina)
                Next

            End If

        End If

        Return oQuest
    End Function

    ' carica dinamicamente lo user control per l'edit della domanda in base al tipo
    Public Function addUCDomandaEdit(ByVal tipodomanda As Integer) As Control

        Dim ctrl As New Control
        Try
            Select Case tipodomanda
                Case Domanda.TipoDomanda.Multipla
                    ctrl = Page.LoadControl(RootObject.ucDomandaMultiplaEdit)
                    ctrl.ID = "UCDomandaMultipla"
                Case Domanda.TipoDomanda.DropDown
                    ctrl = Page.LoadControl(RootObject.ucDomandaDropDownEdit)
                    ctrl.ID = "UCDomandaDropDown"
                Case Domanda.TipoDomanda.Rating
                    ctrl = Page.LoadControl(RootObject.ucDomandaRatingEdit)
                    ctrl.ID = "UCDomandaRating"
                'Case Domanda.TipoDomanda.RatingStars
                '    ctrl = Page.LoadControl(RootObject.ucDomandaRatingStarsEdit)
                '    ctrl.ID = "UCDomandaRatingStars"
                Case Domanda.TipoDomanda.TestoLibero
                    ctrl = Page.LoadControl(RootObject.ucDomandaTestoLiberoEdit)
                    ctrl.ID = "UCDomandaTestoLibero"
                Case Domanda.TipoDomanda.Numerica
                    ctrl = Page.LoadControl(RootObject.ucDomandaNumericaEdit)
                    ctrl.ID = "UCDomandaNumerica"
                Case Domanda.TipoDomanda.Meeting
                    ctrl = Page.LoadControl(RootObject.ucDomandaMeetingEdit)
                    ctrl.ID = "UCDomandaMeeting"
            End Select

        Catch ex As Exception

        End Try

        Return ctrl

    End Function

    Public Function DLDomandeEditCommand(ByVal sender As Object, ByVal e As DataListCommandEventArgs, ByVal oQuest As Questionario) As String

        Dim idDomandaUP, idDomandaDOWN As String
        idDomandaUP = DirectCast(sender, DataList).DataKeys(e.Item.ItemIndex)
        idDomandaDOWN = Nothing

        Select Case e.CommandName
            Case "su"
                ' se e.Item.ItemIndex > 0 vuol dire che scambio le domande della stessa pagina
                If e.Item.ItemIndex > 0 Then
                    idDomandaDOWN = DirectCast(sender, DataList).DataKeys(e.Item.ItemIndex - 1)
                End If
                oQuest = domandaUP(oQuest, idDomandaUP, idDomandaDOWN)
                AddAction(idDomandaDOWN, ModuleQuestionnaire.ObjectType.Domanda, ModuleQuestionnaire.ActionType.QuestionMovedDown)
                AddAction(idDomandaUP, ModuleQuestionnaire.ObjectType.Domanda, ModuleQuestionnaire.ActionType.QuestionMovedUp)
                Return String.Empty

            Case "giu"
                ' se e.Item.ItemIndex < Items.Count - 1 vuol dire che scambio le domande della stessa pagina
                If e.Item.ItemIndex < DirectCast(sender, DataList).Items.Count - 1 Then
                    idDomandaDOWN = DirectCast(sender, DataList).DataKeys(e.Item.ItemIndex + 1)
                End If
                oQuest = domandaDOWN(oQuest, idDomandaUP, idDomandaDOWN)
                AddAction(idDomandaDOWN, ModuleQuestionnaire.ObjectType.Domanda, ModuleQuestionnaire.ActionType.QuestionMovedUp)
                AddAction(idDomandaUP, ModuleQuestionnaire.ObjectType.Domanda, ModuleQuestionnaire.ActionType.QuestionMovedDown)
                Return String.Empty

            Case "elimina"
                Try
                    Dim oDomandaCancel As New Domanda
                    oDomandaCancel = oDomandaCancel.findDomandaBYID(oQuest.domande, idDomandaUP)
                    ' rimuovo la domanda
                    Domanda.removeDomandaBYID(oQuest.pagine(oDomandaCancel.numeroPagina - 1).domande, oDomandaCancel.id)

                    'oQuest.pagine(oDomandaCancel.numeroPagina - 1).domande.RemoveAt(e.Item.ItemIndex)

                    For Each oPag As QuestionarioPagina In oQuest.pagine
                        If Integer.Parse(oPag.numeroPagina) >= Integer.Parse(oDomandaCancel.numeroPagina) Then
                            For Each oDom As Domanda In oPag.domande
                                If Integer.Parse(oDom.numero) > Integer.Parse(oDomandaCancel.numero) Then
                                    oDom.numero = oDom.numero - 1
                                End If
                            Next
                            If oPag.domande.Count > 0 Then
                                oPag.dallaDomanda = oPag.domande(0).numero
                                oPag.allaDomanda = oPag.domande(oPag.domande.Count - 1).numero
                            Else
                                oPag.dallaDomanda = 0
                                oPag.allaDomanda = 0
                            End If
                            DALPagine.Pagina_Update(oPag)
                        End If
                    Next
                    If oQuest.tipo = QuestionnaireType.QuestionLibrary Then
                        AddAction(oDomandaCancel.id, ModuleQuestionnaire.ObjectType.Domanda, ModuleQuestionnaire.ActionType.QuestionVirtualRemoveFromLibrary)
                        DALDomande.DomandaQuestionarioLK_Set_isOld(oDomandaCancel.id, oDomandaCancel.numero, oQuest.id)
                    Else
                        If DALDomande.Domanda_Delete(oQuest.id, oDomandaCancel.numero, idDomandaUP) > 0 Then
                            AddAction(oDomandaCancel.id, ModuleQuestionnaire.ObjectType.Domanda, ModuleQuestionnaire.ActionType.QuestionDelete)
                        End If
                    End If
                Catch ex As Exception
                    AddAction(QuestionarioCorrente.id, ModuleQuestionnaire.ObjectType.Questionario, ModuleQuestionnaire.ActionType.QuestionErrorRemoving)
                End Try
               

                Return String.Empty
            Case "edit"
                Dim oDomandaEdit As New Domanda
                oDomandaEdit = oDomandaEdit.findDomandaBYID(oQuest.domande, DirectCast(sender, DataList).DataKeys(e.Item.ItemIndex))

                Me.DomandaCorrente = oDomandaEdit
                AddAction(oDomandaEdit.id, ModuleQuestionnaire.ObjectType.Domanda, ModuleQuestionnaire.ActionType.QuestionStartEditing)
                Return RootObject.DomandaEdit

            Case "associaDomandaDaLibreria"
                Dim oDomandaNew As New COL_Questionario.Domanda
                Dim oLib As COL_Questionario.Questionario = Me.LibreriaCorrente
                oQuest = Me.QuestionarioCorrente
                oDomandaNew = oDomandaNew.findDomandaBYID(oLib.domande, DirectCast(sender, DataList).DataKeys(e.Item.ItemIndex))
                oDomandaNew.id = 0
                oDomandaNew.idQuestionario = oQuest.id
                oDomandaNew.idPagina = Me.PaginaCorrenteID
                generaNumeroDomanda(oDomandaNew, oQuest)
                ricalcoloPagine(oDomandaNew, oQuest)
                If DALDomande.Salva(oDomandaNew, Me.QuestionarioCorrente.isReadOnly, Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.LibreriaDiDomande) Then
                    AddAction(oDomandaNew.id, ModuleQuestionnaire.ObjectType.Domanda, ModuleQuestionnaire.ActionType.QuestionAddFromLibrary)
                End If
                Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, oQuest.id, 0, False)
                Return RootObject.QuestionarioEdit + "?type=" + oQuest.tipo.ToString()
        End Select

    End Function

    'Public Function addRadToolbarButton(ByVal oEditor As RadEditor)
    '    Dim toolbar As New Telerik.WebControls.RadEditorUtils.Toolbar("My Toolbar")
    '    Dim tb As New Telerik.WebControls.RadEditorUtils.ToolbarButton("CustomDialog")
    '    'tb.IconUrl = "img/latex.gif"
    '    toolbar.Tools.Add(tb)
    '    oEditor.Toolbars.Add(toolbar)
    '    Dim paramCol As New Telerik.WebControls.RadEditorUtils.ParameterCollection()
    '    paramCol.Add(New Telerik.WebControls.RadEditorUtils.Parameter("PARAM1", "Value1"))
    '    paramCol.Add(New Telerik.WebControls.RadEditorUtils.Parameter("PARAM2", "Value2"))
    '    oEditor.DialogParameters.Add("CustomDialog", paramCol)
    'End Function

#End Region
#Region "Binding e Setting Domande"

    Public Function setDomanda(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal oQuest As Questionario) As Integer

        Dim oDomandaSet As New Domanda
        Dim nuovoPeso As Integer
        Dim nuovaDifficolta As Integer
        Dim numeroOpzioni As Integer
        Dim isUpdated As Boolean = True
        Dim isMinorUpdate As Boolean = True 'se e' stata modificata e non sono aggiornati campi che influenzano cio' che viene visto da chi compila il questionario viene fatto l'update della domanda invece che creazione di una nuova domanda
        Dim nuovoTesto As String
        Dim nuovoTestoDopo As String

        If Not oDomanda.tipo = Domanda.TipoDomanda.Rating _
            AndAlso Not oDomanda.tipo = Domanda.TipoDomanda.Meeting Then
            'le rating e le meeting non hanno TXBPeso
            If Not DirectCast(FindControlRecursive(FRVDomanda, "TXBPeso"), TextBox).Text = String.Empty Then
                nuovoPeso = DirectCast(FindControlRecursive(FRVDomanda, "TXBPeso"), TextBox).Text
            Else
                nuovoPeso = 1
            End If
        End If

        If Not oDomanda.tipo = Domanda.TipoDomanda.Meeting Then
            nuovaDifficolta = DirectCast(FindControlRecursive(FRVDomanda, "DDLDifficolta"), DropDownList).SelectedIndex
        End If

        oDomanda.idQuestionario = oQuest.id

        Dim editor As Comunita_OnLine.UC_Editor = FindControlRecursive(FRVDomanda, "CTRLeditorTestoDomanda")
        If Not IsNothing(editor) Then
            If Not editor.isInitialized Then
                editor.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
            End If
            nuovoTesto = editor.HTML
        End If

        nuovoTestoDopo = DirectCast(FindControlRecursive(FRVDomanda, "TXBTestoDopoDomanda"), TextBox).Text

        If Not (oDomanda.testo = nuovoTesto And oDomanda.testoDopo = nuovoTestoDopo) And (oQuest.tipo = Questionario.TipoQuestionario.LibreriaDiDomande Or oDomanda.id = 0) Then
            isMinorUpdate = False
        Else
            isMinorUpdate = oQuest.isReadOnly
        End If

        oDomanda.testo = RootObject.removeBRfromStringEnd(nuovoTesto)
        oDomanda.testoDopo = nuovoTestoDopo

        Dim ddlPagina As New DropDownList
        'meeting e sondaggi hanno una sola pagina
        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting Then
            If Me.QuestionarioCorrente.pagine.Count > 0 Then
                oDomanda.idPagina = Me.QuestionarioCorrente.pagine(0).id
            End If
        Else
            ddlPagina = DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList)
            If ddlPagina.SelectedValue <> String.Empty Then
                oDomanda.idPagina = DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).SelectedValue
            End If
        End If

        If Not oDomanda.tipo = Domanda.TipoDomanda.Meeting Then
            oDomanda.isObbligatoria = DirectCast(FindControlRecursive(FRVDomanda, "CHKisObbligatoria"), CheckBox).Checked
            oDomanda.suggerimento = DirectCast(FindControlRecursive(FRVDomanda, "TXBSuggerimento"), TextBox).Text
        Else
            oDomanda.isObbligatoria = False
            oDomanda.suggerimento = String.Empty
        End If
        Select Case oDomanda.tipo
            Case Domanda.TipoDomanda.DropDown
                oDomandaSet = setDomandaDropDown(FRVDomanda, oDomanda, oQuest, isMinorUpdate)
            Case Domanda.TipoDomanda.Multipla
                oDomandaSet = setDomandaMultipla(FRVDomanda, oDomanda, oQuest, isMinorUpdate)
            Case Domanda.TipoDomanda.Numerica
                oDomandaSet = setDomandaNumerica(FRVDomanda, oDomanda, oQuest, isMinorUpdate)
            Case Domanda.TipoDomanda.Rating
                oDomandaSet = setDomandaRating(FRVDomanda, oDomanda, oQuest)

            Case Domanda.TipoDomanda.TestoLibero
                oDomandaSet = setDomandaTestoLibero(FRVDomanda, oDomanda, oQuest, isMinorUpdate)
            Case Domanda.TipoDomanda.Meeting
                oDomandaSet = setDomandaMeeting(FRVDomanda, oDomanda, oQuest)
        End Select

        If oDomanda.id = 0 Then
            'se la domanda e' nuova peso e counter difficolta' del questionario vengono aggiornati tramite la SP Domanda_insert
            generaNumeroDomanda(oDomanda, oQuest)
        Else

            If Not oDomanda.peso = nuovoPeso Then 'e se il peso non e' stato modificato non si accede al DB
                oQuest.pesoTotale = oQuest.pesoTotale + (nuovoPeso - oDomanda.peso) 'se la domanda era gi' presente va aggiutna solo la differenza tra il peso nuovo e il vecchio
                isUpdated = False
            End If
            If Not oDomanda.difficolta = nuovaDifficolta Then
                isUpdated = False
                Select Case oDomanda.difficolta
                    Case Domanda.DifficoltaDomanda.Bassa
                        oQuest.nDomandeDiffBassa -= 1
                    Case Domanda.DifficoltaDomanda.Media
                        oQuest.nDomandeDiffMedia -= 1
                    Case Domanda.DifficoltaDomanda.Alta
                        oQuest.nDomandeDiffAlta -= 1
                End Select
                Select Case nuovaDifficolta
                    Case Domanda.DifficoltaDomanda.Bassa
                        oQuest.nDomandeDiffBassa += 1
                    Case Domanda.DifficoltaDomanda.Media
                        oQuest.nDomandeDiffMedia += 1
                    Case Domanda.DifficoltaDomanda.Alta
                        oQuest.nDomandeDiffAlta += 1
                End Select
            End If



        End If
        oDomanda.peso = nuovoPeso
        oDomanda.difficolta = nuovaDifficolta
        Dim retVal As Integer
        If Not oDomanda.testo.Trim() = String.Empty Or oDomanda.testo.Trim() = "<br>" Then

            Dim saveOK As Boolean

            saveOK = DALDomande.Salva(oDomanda, isMinorUpdate, Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.LibreriaDiDomande)

            If saveOK Then


                ' ricarico i dati
                'Select Case Me.QuestionarioCorrente.tipo
                '    Case Questionario.TipoQuestionario.Questionario
                ' se è un insert ricalcolo i numeri di domanda e le pagine
                ricalcoloPagine(oDomanda, Me.QuestionarioCorrente)

                Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.idLingua, False)

                retVal = 0
            Else
                ' 1 = errore opzioni
                retVal = 1
            End If
        Else
            ' 2 = errore testo domanda
            retVal = 2

        End If
        If Not isUpdated And retVal = 0 Then
            DALQuestionario.AggiornaPesoEDifficolta(oQuest)
        End If
        oDomanda = oDomandaSet 'ma perche' non usare solo oDomanda?
        Return retVal
    End Function
    Public Function setDomandePaginaEdit(ByVal DLPagine As DataList) As DataList


        For Each itemP As DataListItem In DLPagine.Items

            Dim dlDomande As New DataList
            dlDomande = DLPagine.Controls(itemP.ItemIndex).FindControl("DLDomande")

            For Each itemD As DataListItem In dlDomande.Items
                Try
                    Dim idDomanda As String = dlDomande.DataKeys.Item(itemD.ItemIndex).ToString
                    Dim oDomanda As New Domanda
                    oDomanda = oDomanda.findDomandaBYID(Me.QuestionarioCorrente.domande, idDomanda)
                    Dim oRisposta As New RispostaQuestionario
                    Select Case oDomanda.tipo
                        Case Domanda.TipoDomanda.Multipla And Not oDomanda.isMultipla
                            Dim tabella As New Table
                            tabella = FindControlRecursive(itemD, "TBLDomandaSingola_" + itemD.ItemIndex.ToString())
                            Dim iOpzione As Integer = 0
                            For Each row As TableRow In tabella.Rows
                                Dim opzioneSelezionata As Boolean
                                Dim testoIsAltro As String
                                Dim ris As New RispostaDomanda
                                If oDomanda.risposteDomanda.Count > 0 Then
                                    ris = oRisposta.findRispostaByNumeroOpzione(oDomanda.risposteDomanda, oDomanda.domandaMultiplaOpzioni(iOpzione).numero)
                                End If
                                For Each cell As TableCell In row.Cells
                                    Dim visualizzaSuggerimento As Boolean = False
                                    For Each ctrl As Control In cell.Controls
                                        If ctrl.GetType() Is GetType(RadioButton) Then
                                            Dim rbOpzione As New RadioButton
                                            rbOpzione = DirectCast(ctrl, RadioButton)
                                            rbOpzione.Enabled = False
                                            If Not ris Is Nothing Then
                                                If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                    rbOpzione.Checked = True
                                                    visualizzaSuggerimento = True
                                                End If
                                            End If
                                            'rbOpzione.Enabled = Me.QuestionarioCorrente.editaRisposta
                                        End If

                                        If ctrl.GetType() Is GetType(Label) Then
                                            Dim LBlabel As New Label
                                            LBlabel = DirectCast(ctrl, Label)
                                            If Not LBlabel.ID Is Nothing Then
                                                If LBlabel.ID.Contains("suggestion") And visualizzaSuggerimento Then
                                                    LBlabel.Text &= vbCrLf & SmartTagsAvailable.TagAll(oDomanda.domandaMultiplaOpzioni(iOpzione).suggestion)
                                                End If
                                            ElseIf oDomanda.isValutabile Then
                                                If oDomanda.domandaMultiplaOpzioni(iOpzione).isCorretta Then
                                                    LBlabel.Style.Clear()
                                                    LBlabel.CssClass = "TestoRispostaNonSegnata answer renderedtext"
                                                End If

                                                If Not ris Is Nothing Then
                                                    If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                        LBlabel.Style.Clear()
                                                        LBlabel.CssClass = "TestoRispostaErrata answer renderedtext"
                                                    End If

                                                    If oDomanda.domandaMultiplaOpzioni(iOpzione).isCorretta And ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                        LBlabel.Style.Clear()
                                                        LBlabel.CssClass = "TestoRispostaCorretta answer renderedtext"
                                                    End If
                                                End If
                                            End If
                                        End If

                                        If ctrl.GetType() Is GetType(TextBox) Then
                                            Dim txbOpzione As New TextBox
                                            txbOpzione = DirectCast(ctrl, TextBox)
                                            If Not ris Is Nothing Then
                                                If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                    txbOpzione.Text = ris.testoOpzione
                                                End If
                                            End If
                                            txbOpzione.Enabled = False

                                        End If
                                    Next
                                Next
                                iOpzione = iOpzione + 1
                            Next
                        Case Domanda.TipoDomanda.Multipla And oDomanda.isMultipla
                            Dim tabella As New Table
                            tabella = FindControlRecursive(itemD, "TBLDomandaMultipla_" + itemD.ItemIndex.ToString())
                            Dim iOpzione As Integer = 0
                            For Each row As TableRow In tabella.Rows
                                Dim opzioneSelezionata As Boolean
                                Dim testoIsAltro As String
                                Dim ris As New RispostaDomanda
                                If row.Cells(0).Controls(0).GetType() Is GetType(Label) Then
                                    Dim LBerr As New Label
                                    LBerr = DirectCast(row.Cells(0).Controls(0), Label)
                                    If LBerr.ID.TrimEnd("_", itemD.ItemIndex.ToString) = "LBtroppeRisposte" Then
                                        Exit Select
                                    End If
                                End If
                                If oDomanda.risposteDomanda.Count > 0 Then
                                    ris = oRisposta.findRispostaByNumeroOpzione(oDomanda.risposteDomanda, oDomanda.domandaMultiplaOpzioni(iOpzione).numero)
                                End If
                                For Each cell As TableCell In row.Cells
                                    Dim visualizzaSuggerimento As Boolean = False
                                    For Each ctrl As Control In cell.Controls
                                        If ctrl.GetType() Is GetType(CheckBox) Then
                                            Dim rbOpzione As New CheckBox
                                            rbOpzione = DirectCast(ctrl, CheckBox)
                                            rbOpzione.Enabled = False
                                            If Not ris Is Nothing Then
                                                If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                    rbOpzione.Checked = True
                                                    visualizzaSuggerimento = True
                                                End If
                                            End If
                                            'rbOpzione.Enabled = Me.QuestionarioCorrente.editaRisposta
                                        End If

                                        If ctrl.GetType() Is GetType(Label) Then
                                            Dim LBlabel As New Label
                                            LBlabel = DirectCast(ctrl, Label)
                                            If Not LBlabel.ID Is Nothing Then
                                                If LBlabel.ID.Contains("suggestion") And visualizzaSuggerimento Then
                                                    LBlabel.Text &= vbCrLf & SmartTagsAvailable.TagAll(oDomanda.domandaMultiplaOpzioni(iOpzione).suggestion)
                                                End If
                                            ElseIf oDomanda.isValutabile Then
                                                If oDomanda.domandaMultiplaOpzioni(iOpzione).isCorretta Then
                                                    LBlabel.Style.Clear()
                                                    LBlabel.CssClass = "TestoRispostaNonSegnata answer renderedtext"
                                                End If

                                                If Not ris Is Nothing Then
                                                    If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                        LBlabel.Style.Clear()
                                                        LBlabel.CssClass = "TestoRispostaErrata answer renderedtext"
                                                    End If

                                                    If oDomanda.domandaMultiplaOpzioni(iOpzione).isCorretta And ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                        LBlabel.Style.Clear()
                                                        LBlabel.CssClass = "TestoRispostaCorretta answer renderedtext"
                                                    End If
                                                End If
                                            End If
                                        End If
                                        If ctrl.GetType() Is GetType(TextBox) Then
                                            Dim txbOpzione As New TextBox
                                            txbOpzione = DirectCast(ctrl, TextBox)
                                            If Not ris Is Nothing Then
                                                If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                    txbOpzione.Text = ris.testoOpzione
                                                End If
                                            End If
                                            txbOpzione.Enabled = False
                                        End If
                                    Next
                                Next
                                iOpzione = iOpzione + 1
                            Next

                    End Select
                Catch ex As Exception
                End Try
            Next
        Next
        Return DLPagine


    End Function

    Public Function setDomandePaginaEdit(ByVal DLPagine As DataList, dto As dtoWorkingLibrary) As DataList


        For Each itemP As DataListItem In DLPagine.Items

            Dim dlDomande As New DataList
            dlDomande = DLPagine.Controls(itemP.ItemIndex).FindControl("DLDomande")

            For Each itemD As DataListItem In dlDomande.Items
                Try
                    Dim idDomanda As String = dlDomande.DataKeys.Item(itemD.ItemIndex).ToString
                    Dim oDomanda As New Domanda
                    oDomanda = oDomanda.findDomandaBYID(dto.Library.domande, idDomanda)
                    Dim oRisposta As New RispostaQuestionario
                    Select Case oDomanda.tipo
                        Case Domanda.TipoDomanda.Multipla And Not oDomanda.isMultipla
                            Dim tabella As New Table
                            tabella = FindControlRecursive(itemD, "TBLDomandaSingola_" + itemD.ItemIndex.ToString())
                            Dim iOpzione As Integer = 0
                            For Each row As TableRow In tabella.Rows
                                Dim opzioneSelezionata As Boolean
                                Dim testoIsAltro As String
                                Dim ris As New RispostaDomanda
                                If oDomanda.risposteDomanda.Count > 0 Then
                                    ris = oRisposta.findRispostaByNumeroOpzione(oDomanda.risposteDomanda, oDomanda.domandaMultiplaOpzioni(iOpzione).numero)
                                End If
                                For Each cell As TableCell In row.Cells
                                    Dim visualizzaSuggerimento As Boolean = False
                                    For Each ctrl As Control In cell.Controls
                                        If ctrl.GetType() Is GetType(RadioButton) Then
                                            Dim rbOpzione As New RadioButton
                                            rbOpzione = DirectCast(ctrl, RadioButton)
                                            rbOpzione.Enabled = False
                                            If Not ris Is Nothing Then
                                                If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                    rbOpzione.Checked = True
                                                    visualizzaSuggerimento = True
                                                End If
                                            End If
                                            'rbOpzione.Enabled = Me.QuestionarioCorrente.editaRisposta
                                        End If

                                        If ctrl.GetType() Is GetType(Label) Then
                                            Dim LBlabel As New Label
                                            LBlabel = DirectCast(ctrl, Label)
                                            If Not LBlabel.ID Is Nothing Then
                                                If LBlabel.ID.Contains("suggestion") And visualizzaSuggerimento Then
                                                    LBlabel.Text &= vbCrLf & SmartTagsAvailable.TagAll(oDomanda.domandaMultiplaOpzioni(iOpzione).suggestion)
                                                End If
                                            ElseIf oDomanda.isValutabile Then
                                                If oDomanda.domandaMultiplaOpzioni(iOpzione).isCorretta Then
                                                    LBlabel.Style.Clear()
                                                    LBlabel.CssClass = "TestoRispostaNonSegnata answer renderedtext"
                                                End If

                                                If Not ris Is Nothing Then
                                                    If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                        LBlabel.Style.Clear()
                                                        LBlabel.CssClass = "TestoRispostaErrata answer renderedtext"
                                                    End If

                                                    If oDomanda.domandaMultiplaOpzioni(iOpzione).isCorretta And ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                        LBlabel.Style.Clear()
                                                        LBlabel.CssClass = "TestoRispostaCorretta answer renderedtext"
                                                    End If
                                                End If
                                            End If
                                        End If

                                        If ctrl.GetType() Is GetType(TextBox) Then
                                            Dim txbOpzione As New TextBox
                                            txbOpzione = DirectCast(ctrl, TextBox)
                                            If Not ris Is Nothing Then
                                                If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                    txbOpzione.Text = ris.testoOpzione
                                                End If
                                            End If
                                            txbOpzione.Enabled = False

                                        End If
                                    Next
                                Next
                                iOpzione = iOpzione + 1
                            Next
                        Case Domanda.TipoDomanda.Multipla And oDomanda.isMultipla
                            Dim tabella As New Table
                            tabella = FindControlRecursive(itemD, "TBLDomandaMultipla_" + itemD.ItemIndex.ToString())
                            Dim iOpzione As Integer = 0
                            For Each row As TableRow In tabella.Rows
                                Dim opzioneSelezionata As Boolean
                                Dim testoIsAltro As String
                                Dim ris As New RispostaDomanda
                                If row.Cells(0).Controls(0).GetType() Is GetType(Label) Then
                                    Dim LBerr As New Label
                                    LBerr = DirectCast(row.Cells(0).Controls(0), Label)
                                    If LBerr.ID.TrimEnd("_", itemD.ItemIndex.ToString) = "LBtroppeRisposte" Then
                                        Exit Select
                                    End If
                                End If
                                If oDomanda.risposteDomanda.Count > 0 Then
                                    ris = oRisposta.findRispostaByNumeroOpzione(oDomanda.risposteDomanda, oDomanda.domandaMultiplaOpzioni(iOpzione).numero)
                                End If
                                For Each cell As TableCell In row.Cells
                                    Dim visualizzaSuggerimento As Boolean = False
                                    For Each ctrl As Control In cell.Controls
                                        If ctrl.GetType() Is GetType(CheckBox) Then
                                            Dim rbOpzione As New CheckBox
                                            rbOpzione = DirectCast(ctrl, CheckBox)
                                            rbOpzione.Enabled = False
                                            If Not ris Is Nothing Then
                                                If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                    rbOpzione.Checked = True
                                                    visualizzaSuggerimento = True
                                                End If
                                            End If
                                            'rbOpzione.Enabled = Me.QuestionarioCorrente.editaRisposta
                                        End If

                                        If ctrl.GetType() Is GetType(Label) Then
                                            Dim LBlabel As New Label
                                            LBlabel = DirectCast(ctrl, Label)
                                            If Not LBlabel.ID Is Nothing Then
                                                If LBlabel.ID.Contains("suggestion") And visualizzaSuggerimento Then
                                                    LBlabel.Text &= vbCrLf & SmartTagsAvailable.TagAll(oDomanda.domandaMultiplaOpzioni(iOpzione).suggestion)
                                                End If
                                            ElseIf oDomanda.isValutabile Then
                                                If oDomanda.domandaMultiplaOpzioni(iOpzione).isCorretta Then
                                                    LBlabel.Style.Clear()
                                                    LBlabel.CssClass = "TestoRispostaNonSegnata answer renderedtext"
                                                End If

                                                If Not ris Is Nothing Then
                                                    If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                        LBlabel.Style.Clear()
                                                        LBlabel.CssClass = "TestoRispostaErrata answer renderedtext"
                                                    End If

                                                    If oDomanda.domandaMultiplaOpzioni(iOpzione).isCorretta And ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                        LBlabel.Style.Clear()
                                                        LBlabel.CssClass = "TestoRispostaCorretta answer renderedtext"
                                                    End If
                                                End If
                                            End If
                                        End If
                                        If ctrl.GetType() Is GetType(TextBox) Then
                                            Dim txbOpzione As New TextBox
                                            txbOpzione = DirectCast(ctrl, TextBox)
                                            If Not ris Is Nothing Then
                                                If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                    txbOpzione.Text = ris.testoOpzione
                                                End If
                                            End If
                                            txbOpzione.Enabled = False
                                        End If
                                    Next
                                Next
                                iOpzione = iOpzione + 1
                            Next

                    End Select
                Catch ex As Exception
                End Try
            Next
        Next
        Return DLPagine


    End Function

    ''' <summary>
    ''' Ricorsiva: serve per identificare se la pagina corrente contiene domande, se non ne contiene, trova la prima pagina precedente ad averne
    ''' </summary>
    ''' <param name="oPage">Pagina corrente</param>
    ''' <param name="oQuest">Questionario con .pagine caricate</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getLastPageNotEmpty(ByRef oPage As QuestionarioPagina, ByRef oQuest As Questionario) As QuestionarioPagina
        If Not oPage.numeroPagina > 1 OrElse oPage.domande.Count > 0 Then
            Return oPage
        Else
            Dim oPagPrec As New QuestionarioPagina
            oPagPrec = oQuest.pagine(oPage.numeroPagina - 2)
            'se la pagina non contiene domande, devo prendere il numero della pagina precedente
            If oPagPrec.allaDomanda = oPagPrec.dallaDomanda Then
                Return getLastPageNotEmpty(oPagPrec, oQuest)
            Else
                Return oPagPrec
            End If
        End If
    End Function
    ' genera il numero di domanda e il setta il numero di pagina
    Public Sub generaNumeroDomanda(ByRef oDomanda As Domanda, ByVal oQuest As Questionario)

        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting Then
            'workaround per bug creazione sondaggi al primo accesso
            oDomanda.numero = 1
            oDomanda.numeroPagina = 1
        Else
            Dim oPagina As QuestionarioPagina
            oPagina = QuestionarioPagina.findPaginaBYID(oQuest.pagine, oDomanda.idPagina)
            oDomanda.numero = getLastPageNotEmpty(oPagina, oQuest).allaDomanda + 1
            oDomanda.numeroPagina = oPagina.numeroPagina
        End If
    End Sub
    Public Sub ricalcoloPagine(ByVal oDomanda As Domanda, ByVal oQuest As Questionario)
        Dim oPagina As New QuestionarioPagina
        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting Then
            oPagina = oQuest.pagine(0)
        Else
            oPagina = QuestionarioPagina.findPaginaBYID(oQuest.pagine, oDomanda.idPagina)
        End If
        If oDomanda.isNew Then
            oPagina.domande.Add(oDomanda)
            For Each oPag As QuestionarioPagina In oQuest.pagine
                If oPag.numeroPagina = oDomanda.numeroPagina Then
                    oPag.allaDomanda = oDomanda.numero
                    If oPag.dallaDomanda = 0 Then
                        oPag.dallaDomanda = oPag.allaDomanda
                    End If
                End If
                If oPag.numeroPagina > oDomanda.numeroPagina Then
                    If oPag.domande.Count > 0 Then
                        oPag.dallaDomanda = oPag.dallaDomanda + 1
                        oPag.allaDomanda = oPag.allaDomanda + 1
                    Else
                        oPag.dallaDomanda = 0
                        oPag.allaDomanda = 0
                    End If
                End If
                DALPagine.Pagina_Update(oPag)
            Next
        End If
    End Sub

    'fa la lettura dei dati dalla pagina e li setta in memoria (dopo edit domanda)
    Public Function setDomandaDropDown(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal oQuest As Questionario, Optional ByRef isMinorUpdate As Boolean = True) As Domanda
        Dim oPagina As New QuestionarioPagina
        Dim numeroOpzioniTestoLungo As Int16 = 0

        oDomanda.domandaDropDown.etichetta = DirectCast(FindControlRecursive(FRVDomanda, "TXBEtichetta"), TextBox).Text
        oDomanda.domandaDropDown.nome = DirectCast(FindControlRecursive(FRVDomanda, "TXBNomeDropDown"), TextBox).Text
        oDomanda.domandaDropDown.ordinata = DirectCast(FindControlRecursive(FRVDomanda, "CBOrdina"), CheckBox).Checked
        oDomanda.domandaDropDown.tipo = DropDown.TipoDropDown.Normale
        oDomanda.isValutabile = DirectCast(FindControlRecursive(FRVDomanda, "CHKisValutabile"), CheckBox).Checked

        Dim dlopzioni As New DataList
        dlopzioni = FindControlRecursive(FRVDomanda, "DLOpzioni")
        Dim nOpzione As Integer = 1
        Dim nRisposteCorrette As Integer = 0

        If isMinorUpdate = True And oQuest.tipo = Questionario.TipoQuestionario.LibreriaDiDomande Then

            Dim numeroOpzioni As Integer = DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedItem.Value

            If Not numeroOpzioni = oDomanda.domandaDropDown.dropDownItems.Count Then
                isMinorUpdate = False
            End If

            'update di domanda e di testo, corretta e peso per le opzioni
            Dim counter As Int16
            For counter = 0 To oDomanda.domandaDropDown.dropDownItems.Count - 1
                Dim nuovoTestoOpzione As String
                nuovoTestoOpzione = DirectCast(FindControlRecursive(dlopzioni.Items(counter), "TXBScelta"), TextBox).Text
                ' se è una libreria non posso modificare i testi delle opzioni e imposto minorUpdate a false
                If Not nuovoTestoOpzione = oDomanda.domandaDropDown.dropDownItems(counter).testo And oQuest.tipo = Questionario.TipoQuestionario.LibreriaDiDomande Then
                    isMinorUpdate = False
                    Exit For
                End If

                If oQuest.tipo = Questionario.TipoQuestionario.Questionario Then
                    oDomanda.domandaDropDown.dropDownItems(counter).testo = DirectCast(FindControlRecursive(dlopzioni.Items(counter), "TXBScelta"), TextBox).Text
                End If

                oDomanda.domandaDropDown.dropDownItems(counter).peso = DirectCast(FindControlRecursive(dlopzioni.Items(counter), "TXBPesoRisposta"), TextBox).Text
                oDomanda.domandaDropDown.dropDownItems(counter).suggestion = DirectCast(FindControlRecursive(dlopzioni.Items(counter), "TXBsuggestionOption"), TextBox).Text
                oDomanda.domandaDropDown.dropDownItems(counter).isCorretta = DirectCast(FindControlRecursive(dlopzioni.Items(counter), "CBisCorretta"), CheckBox).Checked
            Next
        End If

        If isMinorUpdate And Not oQuest.tipo = Questionario.TipoQuestionario.LibreriaDiDomande Then
            'update di domanda e di testo, corretta e peso per le opzioni
            Dim counter As Int16
            For counter = 0 To oDomanda.domandaDropDown.dropDownItems.Count - 1
                oDomanda.domandaDropDown.dropDownItems(counter).testo = DirectCast(FindControlRecursive(dlopzioni.Items(counter), "TXBScelta"), TextBox).Text 'DirectCast(FindControlRecursive(dlopzioni.Items(counter), "CTRLeditorScelta"), Comunita_OnLine.UC_VisualEditor).HTML()
                oDomanda.domandaDropDown.dropDownItems(counter).suggestion = DirectCast(FindControlRecursive(dlopzioni.Items(counter), "TXBsuggestionOption"), TextBox).Text
            Next
        Else
            isMinorUpdate = False
        End If

        If isMinorUpdate = False Then

            oDomanda.domandaDropDown.dropDownItems.Clear()
            For Each item As DataListItem In dlopzioni.Items
                'If item.FindControl("TXBScelta").GetType() Is GetType(WebControls.TextBox) Then
                Dim txbScelta As New TextBox
                txbScelta = DirectCast(item.FindControl("TXBScelta"), TextBox)
                Dim txbPeso As New TextBox
                txbPeso = DirectCast(item.FindControl("TXBPesoRisposta"), TextBox)
                Dim cbCorretta As New CheckBox
                cbCorretta = DirectCast(item.FindControl("CBisCorretta"), CheckBox)
                Dim txbSuggestionOption As New TextBox
                txbSuggestionOption = DirectCast(item.FindControl("TXBsuggestionOption"), TextBox)

                If Not txbScelta.Text = String.Empty Then
                    Dim oDomandaOpzione As New DropDownItem
                    oDomandaOpzione.testo = txbScelta.Text
                    oDomandaOpzione.suggestion = txbSuggestionOption.Text
                    oDomandaOpzione.numero = nOpzione
                    oDomandaOpzione.peso = txbPeso.Text
                    oDomandaOpzione.isCorretta = cbCorretta.Checked
                    oDomandaOpzione.indice = nOpzione
                    'If oDomandaOpzione.testo.Length > RootObject.lunghezzaOpzioniDropDown Then
                    '    oDomandaOpzione.isValida = False
                    '    numeroOpzioniTestoLungo = numeroOpzioniTestoLungo + 1
                    'Else
                    '    oDomandaOpzione.isValida = True
                    'End If
                    oDomanda.domandaDropDown.dropDownItems.Add(oDomandaOpzione)
                    nOpzione = nOpzione + 1
                End If


                'End If
            Next
        End If
        'If numeroOpzioniTestoLungo > 0 Then
        '    oDomanda.isValida = False
        '    bindItemsDomandaDropDown(FRVDomanda, oDomanda, oPagina, oQuest)
        'Else
        '    oDomanda.isValida = True
        'End If
        Return oDomanda
    End Function
    Public Sub bindFieldDomandaDropDown(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal opagina As QuestionarioPagina, ByVal oQuest As Questionario)
        Dim listDom As New List(Of DropDown)
        listDom.Add(oDomanda.domandaDropDown)
        FRVDomanda.DataSource = listDom
        FRVDomanda.DataBind()

        'oQuest = Session("oQuest")
        'If oQuest.tipo = Questionario.TipoQuestionario.Questionario Then

        DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).DataSource = oQuest.pagine
        DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).DataBind()
        If oDomanda.idPagina > 0 Then
            DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).SelectedValue = oDomanda.idPagina
        Else
            DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).SelectedValue = opagina.id
        End If
        Dim editor As Comunita_OnLine.UC_Editor = FindControlRecursive(FRVDomanda, "CTRLeditorTestoDomanda")
        If Not IsNothing(editor) Then
            If Not editor.isInitialized Then
                editor.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
            End If
            editor.HTML = oDomanda.testo
        End If

        DirectCast(FindControlRecursive(FRVDomanda, "TXBTestoDopoDomanda"), TextBox).Text = oDomanda.testoDopo
        DirectCast(FindControlRecursive(FRVDomanda, "TXBPeso"), TextBox).Text = oDomanda.peso
        DirectCast(FindControlRecursive(FRVDomanda, "DDLDifficolta"), DropDownList).SelectedIndex = oDomanda.difficolta
        DirectCast(FindControlRecursive(FRVDomanda, "CHKisValutabile"), CheckBox).Checked = oDomanda.isValutabile
        DirectCast(FindControlRecursive(FRVDomanda, "CHKisObbligatoria"), CheckBox).Checked = oDomanda.isObbligatoria
        DirectCast(FindControlRecursive(FRVDomanda, "TXBSuggerimento"), TextBox).Text = oDomanda.suggerimento
        Dim div As HtmlGenericControl = FindControlRecursive(FRVDomanda, "DIVpaginaCorrente")
        If Not IsNothing(div) Then
            div.Visible = (oQuest.tipo <> QuestionnaireType.QuestionLibrary AndAlso oQuest.tipo <> QuestionnaireType.Poll AndAlso oQuest.tipo <> QuestionnaireType.Model AndAlso oQuest.tipo <> QuestionnaireType.Meeting)
        End If
    End Sub
    Public Sub bindItemsDomandaDropDown(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal opagina As QuestionarioPagina, ByVal oQuest As Questionario)
        Dim dlItems As New DataList
        dlItems = DirectCast(FindControlRecursive(FRVDomanda, "DLOpzioni"), DataList)
        If oDomanda.domandaDropDown.dropDownItems.Count = 0 Then
            Dim oOpzione1 As New DropDownItem
            oDomanda.domandaDropDown.dropDownItems.Add(oOpzione1)
            oDomanda.domandaDropDown.dropDownItems.Add(oOpzione1)
            oDomanda.domandaDropDown.dropDownItems.Add(oOpzione1)
        End If
        dlItems.DataSource = oDomanda.domandaDropDown.dropDownItems
        dlItems.DataBind()
        DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedValue = oDomanda.domandaDropDown.dropDownItems.Count
  
    End Sub
    Public Sub bindFieldDomandaMultipla(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal opagina As QuestionarioPagina, ByVal oQuest As Questionario)
        Dim listDom As New List(Of Domanda)
        listDom.Add(oDomanda)
        FRVDomanda.DataSource = listDom
        FRVDomanda.DataBind()
        DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).DataSource = oQuest.pagine
        DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).DataBind()
        If listDom(0).idPagina > 0 Then
            DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).SelectedValue = listDom(0).idPagina
        Else
            If Not opagina Is Nothing Then
                DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).SelectedValue = opagina.id
            End If

        End If
        DirectCast(FindControlRecursive(FRVDomanda, "TXBTestoDopoDomanda"), TextBox).Text = oDomanda.testoDopo
        DirectCast(FindControlRecursive(FRVDomanda, "DDLDifficolta"), DropDownList).SelectedIndex = oDomanda.difficolta
        DirectCast(FindControlRecursive(FRVDomanda, "TXBSuggerimento"), TextBox).Text = oDomanda.suggerimento


        Dim div As HtmlGenericControl = FindControlRecursive(FRVDomanda, "DIVpaginaCorrente")
        If Not IsNothing(div) Then
            div.Visible = (oQuest.tipo <> QuestionnaireType.QuestionLibrary AndAlso oQuest.tipo <> QuestionnaireType.Poll AndAlso oQuest.tipo <> QuestionnaireType.Model AndAlso oQuest.tipo <> QuestionnaireType.Meeting)
        End If



        Dim editor As Comunita_OnLine.UC_Editor = FindControlRecursive(FRVDomanda, "CTRLeditorTestoDomanda")
        If Not IsNothing(editor) Then
            If Not editor.isInitialized Then
                editor.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
            End If
            editor.HTML = oDomanda.testo
        End If
        'DirectCast(FindControlRecursive(FRVDomanda, "DIVpaginaCorrente"), HtmlControl).Visible = Not (Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio)
    End Sub
    Public Sub bindOpzioniDomandaMultipla(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal opagina As QuestionarioPagina, ByVal oQuest As Questionario)
        Dim counter As Int32
        Dim nOpzioni As Integer = Me.DomandaCorrente.domandaMultiplaOpzioni.Count
        ' se non ho opzioni (domanda nuova) allora inserisco quelle di default (3)
        If nOpzioni = 0 Then
            nOpzioni = 3
        End If
        For counter = 0 To nOpzioni
            DirectCast(FindControlRecursive(FRVDomanda, "DDLnumeroMaxRisposte"), DropDownList).Items.Add(counter)
        Next

        MyBase.SetCulture("pg_ucDomandaMultiplaEdit", "Questionari")
        DirectCast(FindControlRecursive(FRVDomanda, "DDLnumeroMaxRisposte"), DropDownList).Items(0).Text = Me.Resource.getValue("0.DDLnumeroMaxRisposte")

        Dim dlOpzioni As New DataList
        dlOpzioni = DirectCast(FindControlRecursive(FRVDomanda, "DLOpzioni"), DataList)
        If oDomanda.domandaMultiplaOpzioni.Count = 0 Then
            Dim oOpzione1 As New DomandaOpzione
            oDomanda.domandaMultiplaOpzioni.Add(oOpzione1)
            oDomanda.domandaMultiplaOpzioni.Add(oOpzione1)
            oDomanda.domandaMultiplaOpzioni.Add(oOpzione1)
        End If
        dlOpzioni.DataSource = oDomanda.domandaMultiplaOpzioni
        dlOpzioni.DataBind()

        'If oDomanda.numeroMaxRisposte = 0 And DirectCast(FindControlRecursive(FRVDomanda, "DDLnumeroMaxRisposte"), DropDownList).Items.Count > 0 Then
        '    oDomanda.numeroMaxRisposte = 1
        'End If

        DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedValue = oDomanda.domandaMultiplaOpzioni.Count
        DirectCast(FindControlRecursive(FRVDomanda, "DDLnumeroMaxRisposte"), DropDownList).SelectedIndex = Math.Max(0, CInt(oDomanda.numeroMaxRisposte)) '"tutte le risposte" ha index = 0
    End Sub

    Public Sub eliminaOpzioneMultipla(ByVal FRVDomanda As FormView, ByVal DLItemIndex As Integer)

        Dim editor As Comunita_OnLine.UC_Editor = FRVDomanda.FindControl("DLOpzioni").Controls(DLItemIndex).FindControl("CTRLeditorScelta")
        If Not IsNothing(editor) Then
            If Not editor.isInitialized Then
                editor.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
            End If
            editor.HTML = String.Empty
        End If
        DirectCast(FRVDomanda.FindControl("DLOpzioni").Controls(DLItemIndex).FindControl("TXBSuggerimentoOpzione"), TextBox).Text = String.Empty

        Me.DomandaCorrente.domandaMultiplaOpzioni.RemoveAt(DLItemIndex)

    End Sub

    Public Sub FRVDomandaDataBound(ByVal FRVDomanda As FormView)

        Dim oImg As System.Web.UI.WebControls.ImageButton

        oImg = DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "IMBHelp"), ImageButton)

        oImg.Attributes.Add("onclick", RootObject.apriPopUp(RootObject.helpDomandaMultipla, "target", "yes", "yes"))
    End Sub

    Public Sub DLOpzioniMultipla_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        Dim oCheck As CheckBox
        'Dim oCheckIsAltro As CheckBox
        Dim oText As TextBox

        Dim editor As Comunita_OnLine.UC_Editor = e.Item.FindControl("CTRLeditorScelta")
        If Not IsNothing(editor) Then
            Dim opt As DomandaOpzione = DirectCast(e.Item.DataItem, DomandaOpzione)
            If Not editor.isInitialized Then
                editor.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
            End If
            editor.HTML = opt.testo
        End If

        oCheck = e.Item.FindControl("CBisCorretta")
        'oCheckIsAltro = e.Item.FindControl("CBisAltro")
        oText = e.Item.FindControl("TXBPeso")
        oCheck.Attributes.Add("onclick", "changeText(this,'" & oText.ClientID & "'); return true")
    End Sub
    Public Sub DLOpzioniFreeText_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        Dim editor As Comunita_OnLine.UC_Editor = e.Item.FindControl("CTRLeditorEtichetta")
        If Not IsNothing(editor) Then
            Dim opt As DomandaTestoLibero = DirectCast(e.Item.DataItem, DomandaTestoLibero)
            If Not editor.isInitialized Then
                editor.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
            End If
            editor.HTML = opt.etichetta
        End If
    End Sub

    Public Sub eliminaOpzioneTestoLibero(ByVal FRVDomanda As FormView, ByVal DLItemIndex As Integer)

        DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "DLOpzioni").Controls(DLItemIndex).FindControl("TXBEtichetta"), TextBox).Text = String.Empty
        Me.DomandaCorrente.opzioniTestoLibero.RemoveAt(DLItemIndex)

    End Sub

    Public Sub eliminaOpzioneNumerica(ByVal FRVDomanda As FormView, ByVal DLItemIndex As Integer)

        DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "DLOpzioni").Controls(DLItemIndex).FindControl("TXBTestoPrima"), TextBox).Text = String.Empty
        Me.DomandaCorrente.opzioniNumerica.RemoveAt(DLItemIndex)

    End Sub

    Public Sub eliminaOpzioneRating(ByVal FRVDomanda As FormView, ByVal DLItemIndex As Integer)

        DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "DLOpzioni").Controls(DLItemIndex).FindControl("TXBTestoMin"), TextBox).Text = String.Empty
        Me.DomandaCorrente.domandaRating.opzioniRating.RemoveAt(DLItemIndex)

    End Sub

    Public Sub eliminaOpzioneDropDown(ByVal FRVDomanda As FormView, ByVal DLItemIndex As Integer)

        DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "DLOpzioni").Controls(DLItemIndex).FindControl("TXBScelta"), TextBox).Text = String.Empty
        Me.DomandaCorrente.domandaDropDown.dropDownItems.RemoveAt(DLItemIndex)

    End Sub

    Public Sub creaTabellaRadioButton(ByVal FRVDomanda As FormView, ByVal isChangingTable As Boolean)
        Dim tabella As New Table
        tabella = DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "TBLCheckbox"), Table)
        tabella.Controls.Clear()
        tabella.BorderWidth = 1
        tabella.BorderColor = Color.Black
        tabella.CellPadding = 0
        tabella.CellSpacing = 0

        Dim nRighe As Integer = DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedValue 'Me.DomandaCorrente.domandaRating.opzioniRating.Count

        Dim nColonne As Integer = DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "DDLNumeroColonne"), DropDownList).SelectedValue 'Me.DomandaCorrente.domandaRating.intestazioniRating.Count

        For i As Integer = 1 To nRighe
            Dim r As New TableRow
            For c As Integer = 1 To nColonne
                Dim cell As New TableCell
                If Me.DomandaCorrente.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Numerazione Then
                    cell.Width = 20 + 1
                Else
                    For Each DLItem As DataListItem In DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "DLIntestazioni"), DataList).Items
                        DirectCast(DLItem.FindControl("TXBIntestazione"), TextBox).Width = 500 / Me.DomandaCorrente.domandaRating.intestazioniRating.Count
                    Next
                    cell.Width = (500 / nColonne) + 1
                End If
                cell.Height = 60
                cell.BorderWidth = 1
                cell.BorderColor = Color.Black
                cell.VerticalAlign = VerticalAlign.Middle
                cell.HorizontalAlign = HorizontalAlign.Center
                Dim cb As New CheckBox
                Try
                    cb.Checked = Boolean.Parse(Me.DomandaCorrente.domandaRating.opzioniRating.Item(i - 1).arrayCBisVisible.Chars(c - 1) = "1")
                    cb.Enabled = Not isChangingTable
                Catch
                    cb.Checked = False
                    cb.Enabled = False
                End Try
                cell.Controls.Add(cb)
                r.Cells.Add(cell)
            Next
            tabella.Rows.Add(r)
        Next
    End Sub
    Public Sub crea_Stars(ByVal FRVDomanda As FormView, ByVal isChangingTable As Boolean)

        Dim tabella As New Table
        tabella = DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "TBLCheckbox"), Table)
        tabella.Controls.Clear()
        tabella.CssClass = "starcontainertable"
        'tabella.BorderWidth = 1
        'tabella.BorderColor = Color.Black
        tabella.CellPadding = 0
        tabella.CellSpacing = 0
        'tabella.Height = 60

        Dim nRighe As Integer = DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedValue 'Me.DomandaCorrente.domandaRating.opzioniRating.Count

        Dim nColonne As Integer = DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "DDLNumeroColonne"), DropDownList).SelectedValue 'Me.DomandaCorrente.domandaRating.intestazioniRating.Count

        For i As Integer = 1 To nRighe
            Dim r As New TableRow

            Dim starcell As New TableCell
            starcell.Height = Unit.Pixel(60)
            starcell.CssClass = "staritemcontainer"

            Dim rdStars As New Telerik.Web.UI.RadRating

            rdStars.Value = 0
            rdStars.ItemCount = nColonne
            rdStars.SelectionMode = Telerik.Web.UI.RatingSelectionMode.Continuous
            rdStars.Precision = Telerik.Web.UI.RatingPrecision.Item
            rdStars.Orientation = System.Web.UI.WebControls.Orientation.Horizontal
            'rdStars.ReadOnly = true

            starcell.Controls.Add(rdStars)
            r.Cells.Add(starcell)
            tabella.Rows.Add(r)
        Next

    End Sub

    Public Function salvaDomanda(ByVal oFRVDomanda As FormView) As Integer

        'il prossimo if sara' sostituito da 
        '            If Not DirectCast(GestioneDomande.FindControlRecursive(PHOpzioni, "CUVLunghezzaOpzione"), CustomValidator).IsValid Then
        'quando il controllo sara' attivato per tutte le domande.
        Dim odomanda As New Domanda
        odomanda = Me.DomandaCorrente

        If odomanda.id = 0 Then
            odomanda.idLingua = Me.QuestionarioCorrente.idLingua
            odomanda.idPersonaCreator = Me.UtenteCorrente.ID
            odomanda.dataCreazione = Now
            odomanda.idQuestionario = Me.QuestionarioCorrente.id
        End If
        odomanda.dataModifica = Now
        odomanda.idPersonaEditor = Me.UtenteCorrente.ID

        Return setDomanda(oFRVDomanda, odomanda, Me.QuestionarioCorrente)
        'If oDomanda.isValida = True Then

    End Function

    Public Function setDomandaMultipla(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal oQuest As Questionario, Optional ByRef isMinorUpdate As Boolean = True) As Domanda

        Dim oPagina As New QuestionarioPagina
        oDomanda.numeroMaxRisposte = DirectCast(FindControlRecursive(FRVDomanda, "DDLnumeroMaxRisposte"), DropDownList).SelectedIndex
        oDomanda.isValutabile = DirectCast(FindControlRecursive(FRVDomanda, "CHKisValutabile"), CheckBox).Checked
        Dim dlopzioni As New DataList
        dlopzioni = FindControlRecursive(FRVDomanda, "DLOpzioni")
        Dim nOpzione As Integer = 1
        Dim nRisposteCorrette As Integer = 0
        If isMinorUpdate = True And oQuest.tipo = Questionario.TipoQuestionario.LibreriaDiDomande Then

            Dim numeroOpzioni As Integer = DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedItem.Value

            If Not numeroOpzioni = oDomanda.domandaMultiplaOpzioni.Count Then
                isMinorUpdate = False
            End If
            'update di domanda e di testo, corretta e peso per le opzioni
            For Each opzione As DomandaOpzione In oDomanda.domandaMultiplaOpzioni
                Dim nuovoTestoOpzione As String

                Dim editor As Comunita_OnLine.UC_Editor = FindControlRecursive(dlopzioni.Items(opzione.numero - 1), "CTRLeditorScelta")
                If Not IsNothing(editor) Then
                    If Not editor.isInitialized Then
                        'editor.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
                    End If
                    nuovoTestoOpzione = editor.HTML
                End If

                ' se è una libreria non posso modificare i testi delle opzioni e imposto minorUpdate a false
                If Not nuovoTestoOpzione = opzione.testo Then
                    isMinorUpdate = False
                    Exit For
                End If

                'per i questionari minorUpdate è sempre a true quindi memorizzo anche i testi
                If oQuest.tipo = Questionario.TipoQuestionario.Questionario Then
                    opzione.testo = RootObject.removeBRfromStringEnd(nuovoTestoOpzione)
                    opzione.isAltro = DirectCast(FindControlRecursive(dlopzioni.Items(opzione.numero - 1), "CBisAltro"), CheckBox).Checked
                End If

                opzione.peso = DirectCast(FindControlRecursive(dlopzioni.Items(opzione.numero - 1), "TXBPeso"), TextBox).Text
                opzione.suggestion = DirectCast(FindControlRecursive(dlopzioni.Items(opzione.numero - 1), "TXBSuggerimentoOpzione"), TextBox).Text
                opzione.isCorretta = DirectCast(FindControlRecursive(dlopzioni.Items(opzione.numero - 1), "CBisCorretta"), CheckBox).Checked
            Next
        End If

        If isMinorUpdate And Not oQuest.tipo = Questionario.TipoQuestionario.LibreriaDiDomande Then
            'update di domanda e di testo, corretta e peso per le opzioni
            For Each opzione As DomandaOpzione In oDomanda.domandaMultiplaOpzioni
                Dim editor As Comunita_OnLine.UC_Editor = FindControlRecursive(dlopzioni.Items(opzione.numero - 1), "CTRLeditorScelta")
                If Not IsNothing(editor) Then
                    If Not editor.isInitialized Then
                        'editor.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
                    End If
                    opzione.testo = editor.HTML
                End If
                opzione.suggestion = DirectCast(FindControlRecursive(dlopzioni.Items(opzione.numero - 1), "TXBSuggerimentoOpzione"), TextBox).Text
            Next
        Else
            isMinorUpdate = False
        End If

        If isMinorUpdate = False Then
            'oDomanda.domandaMultiplaOpzioni.Clear()
            Dim oDomandaNew As New Domanda

            For Each item As DataListItem In dlopzioni.Items
                'If item.FindControl("TXBScelta").GetType() Is GetType(WebControls.TextBox) Then
                Dim txbScelta As New Comunita_OnLine.UC_Editor
                txbScelta = DirectCast(item.FindControl("CTRLeditorScelta"), Comunita_OnLine.UC_Editor)
                Dim txbPeso As New TextBox
                txbPeso = DirectCast(item.FindControl("TXBPeso"), TextBox)
                Dim txbSuggestion As New TextBox
                txbSuggestion = DirectCast(item.FindControl("TXBSuggerimentoOpzione"), TextBox)

                If txbPeso.Text = String.Empty Then
                    txbPeso.Text = 0
                End If
                Dim cbCorretta As New CheckBox
                cbCorretta = DirectCast(item.FindControl("CBisCorretta"), CheckBox)
                Dim cbIsAltro As New CheckBox
                cbIsAltro = DirectCast(item.FindControl("CBisAltro"), CheckBox)

                If Not txbScelta.HTML = String.Empty Then
                    Dim oDomandaOpzione As New DomandaOpzione
                    oDomandaOpzione.numero = nOpzione
                    oDomandaOpzione.id = oDomanda.domandaMultiplaOpzioni(nOpzione - 1).id
                    oDomandaOpzione.testo = RootObject.removeBRfromStringEnd(txbScelta.HTML)
                    oDomandaOpzione.peso = txbPeso.Text
                    oDomandaOpzione.isCorretta = cbCorretta.Checked
                    oDomandaOpzione.isAltro = cbIsAltro.Checked
                    oDomandaOpzione.suggestion = txbSuggestion.Text
                    oDomandaNew.domandaMultiplaOpzioni.Add(oDomandaOpzione)
                    nOpzione = nOpzione + 1
                    If oDomandaOpzione.isCorretta = True Then
                        nRisposteCorrette = nRisposteCorrette + 1
                    End If
                End If
                'End If
            Next
            oDomanda.domandaMultiplaOpzioni.Clear()
            oDomanda.domandaMultiplaOpzioni = oDomandaNew.domandaMultiplaOpzioni

        End If


        If nRisposteCorrette > 1 Or Not DirectCast(FindControlRecursive(FRVDomanda, "DDLnumeroMaxRisposte"), DropDownList).SelectedIndex = 1 Then
            oDomanda.isMultipla = True
        Else
            oDomanda.isMultipla = False
        End If

        Return oDomanda
    End Function

    Public Sub bindFieldDomandaRating(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal opagina As QuestionarioPagina, ByVal oQuest As Questionario)

        Dim listDom As New List(Of DomandaRating)
        listDom.Add(oDomanda.domandaRating)
        FRVDomanda.DataSource = listDom
        FRVDomanda.DataBind()

        'If oQuest.tipo = Questionario.TipoQuestionario.Questionario Then

        DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).DataSource = oQuest.pagine
        DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).DataBind()
        If oDomanda.idPagina > 0 Then
            DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).SelectedValue = oDomanda.idPagina
        Else
            DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).SelectedValue = opagina.id
        End If
        Dim editor As Comunita_OnLine.UC_Editor = FindControlRecursive(FRVDomanda, "CTRLeditorTestoDomanda")
        If Not IsNothing(editor) Then
            If Not editor.isInitialized Then
                editor.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
            End If
            editor.HTML = oDomanda.testo
        End If

        DirectCast(FindControlRecursive(FRVDomanda, "TXBTestoDopoDomanda"), TextBox).Text = oDomanda.testoDopo
        DirectCast(FindControlRecursive(FRVDomanda, "DDLDifficolta"), DropDownList).SelectedIndex = oDomanda.difficolta
        DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroColonne"), DropDownList).SelectedValue = listDom(0).intestazioniRating.Count
        DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedValue = listDom(0).opzioniRating.Count
        DirectCast(FindControlRecursive(FRVDomanda, "RBLTipoIntestazione"), RadioButtonList).SelectedValue = oDomanda.domandaRating.tipoIntestazione
        DirectCast(FindControlRecursive(FRVDomanda, "CHKisObbligatoria"), CheckBox).Checked = oDomanda.isObbligatoria
        DirectCast(FindControlRecursive(FRVDomanda, "CBmostraND"), CheckBox).Checked = listDom(0).mostraND
        DirectCast(FindControlRecursive(FRVDomanda, "TXBSuggerimento"), TextBox).Text = oDomanda.suggerimento
        Dim div As HtmlGenericControl = FindControlRecursive(FRVDomanda, "DIVpaginaCorrente")
        If Not IsNothing(div) Then
            div.Visible = (oQuest.tipo <> QuestionnaireType.QuestionLibrary AndAlso oQuest.tipo <> QuestionnaireType.Poll AndAlso oQuest.tipo <> QuestionnaireType.Model AndAlso oQuest.tipo <> QuestionnaireType.Meeting)
        End If
    End Sub
    Public Sub bindFieldDomandaRatingStars(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal opagina As QuestionarioPagina, ByVal oQuest As Questionario)

        Dim listDom As New List(Of DomandaRating)
        listDom.Add(oDomanda.domandaRating)
        FRVDomanda.DataSource = listDom
        FRVDomanda.DataBind()

        'If oQuest.tipo = Questionario.TipoQuestionario.Questionario Then

        DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).DataSource = oQuest.pagine
        DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).DataBind()
        If oDomanda.idPagina > 0 Then
            DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).SelectedValue = oDomanda.idPagina
        Else
            DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).SelectedValue = opagina.id
        End If
        Dim editor As Comunita_OnLine.UC_Editor = FindControlRecursive(FRVDomanda, "CTRLeditorTestoDomanda")
        If Not IsNothing(editor) Then
            If Not editor.isInitialized Then
                editor.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
            End If
            editor.HTML = oDomanda.testo
        End If

        DirectCast(FindControlRecursive(FRVDomanda, "TXBTestoDopoDomanda"), TextBox).Text = oDomanda.testoDopo
        DirectCast(FindControlRecursive(FRVDomanda, "DDLDifficolta"), DropDownList).SelectedIndex = oDomanda.difficolta
        DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroColonne"), DropDownList).SelectedValue = listDom(0).intestazioniRating.Count
        DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedValue = listDom(0).opzioniRating.Count
        'DirectCast(FindControlRecursive(FRVDomanda, "RBLTipoIntestazione"), RadioButtonList).SelectedValue = oDomanda.domandaRating.tipoIntestazione
        DirectCast(FindControlRecursive(FRVDomanda, "CHKisObbligatoria"), CheckBox).Checked = oDomanda.isObbligatoria
        DirectCast(FindControlRecursive(FRVDomanda, "CBmostraND"), CheckBox).Checked = listDom(0).mostraND
        DirectCast(FindControlRecursive(FRVDomanda, "TXBSuggerimento"), TextBox).Text = oDomanda.suggerimento
        Dim div As HtmlGenericControl = FindControlRecursive(FRVDomanda, "DIVpaginaCorrente")
        If Not IsNothing(div) Then
            div.Visible = (oQuest.tipo <> QuestionnaireType.QuestionLibrary AndAlso oQuest.tipo <> QuestionnaireType.Poll AndAlso oQuest.tipo <> QuestionnaireType.Model AndAlso oQuest.tipo <> QuestionnaireType.Meeting)
        End If
    End Sub

    Public Sub bindFieldDomandaMeeting(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal oPagina As QuestionarioPagina, ByVal oQuest As Questionario)

        Dim listDom As New List(Of DomandaRating)
        listDom.Add(oDomanda.domandaRating)
        FRVDomanda.DataSource = listDom
        FRVDomanda.DataBind()

        Dim editor As Comunita_OnLine.UC_Editor = FindControlRecursive(FRVDomanda, "CTRLeditorTestoDomanda")
        If Not IsNothing(editor) Then
            If Not editor.isInitialized Then
                editor.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
            End If
            editor.HTML = oDomanda.testo
        End If

        DirectCast(FindControlRecursive(FRVDomanda, "TXBTestoDopoDomanda"), TextBox).Text = oDomanda.testoDopo
        Dim oCal As RadCalendar
        oCal = DirectCast(FindControlRecursive(FRVDomanda, "RDCLCalendar"), RadCalendar)
        For Each item As Date In oDomanda.domandaRating.intestazioniMeeting
            Dim oDate As New RadDate
            oDate.Date = item
            oCal.SelectedDates.Add(oDate)
        Next
        If oDomanda.domandaRating.opzioniRating.Count < 1 Then
            Dim opz As New DomandaOpzione
            oDomanda.domandaRating.opzioniRating.Add(opz)
        End If
        DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedValue = oDomanda.domandaRating.opzioniRating.Count
    End Sub
    Public Function setDomandaRating(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal oQuest As Questionario, Optional ByVal isChangingTable As Boolean = False) As Domanda

        Dim oPagina As New QuestionarioPagina

        oDomanda.domandaRating.mostraND = DirectCast(FindControlRecursive(FRVDomanda, "CBmostraND"), CheckBox).Checked
        oDomanda.domandaRating.numeroRating = DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroColonne"), DropDownList).SelectedValue

        If DirectCast(FindControlRecursive(FRVDomanda, "RBLTipoIntestazione"), RadioButtonList).SelectedItem.Value = DomandaRating.TipoIntestazioneRating.Testi Then
            ' creo il DATALIST delle Intestazioni di Rating
            Dim dlintest As New DataList
            dlintest = FindControlRecursive(FRVDomanda, "DLIntestazioni")
            Dim n As Integer = 1
            If oQuest.isReadOnly Then
                For Each item As DataListItem In dlintest.Items
                    Dim txbScelta As New TextBox
                    txbScelta = DirectCast(item.FindControl("TXBIntestazione"), TextBox)
                    'If Not txbScelta.Text = String.Empty Then
                    oDomanda.domandaRating.intestazioniRating(n - 1).testo = txbScelta.Text
                    oDomanda.domandaRating.intestazioniRating(n - 1).numero = n
                    n = n + 1
                    'End If
                Next
            Else
                oDomanda.domandaRating.intestazioniRating.Clear()
                For Each item As DataListItem In dlintest.Items
                    Dim txbScelta As New TextBox
                    txbScelta = DirectCast(item.FindControl("TXBIntestazione"), TextBox)
                    'If Not txbScelta.Text = String.Empty Then
                    Dim oDomandaOpzione As New DomandaOpzione
                    oDomandaOpzione.numero = n
                    oDomandaOpzione.testo = txbScelta.Text
                    oDomanda.domandaRating.intestazioniRating.Add(oDomandaOpzione)
                    n = n + 1
                    'End If
                Next
            End If
            oDomanda.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Testi
        Else
            oDomanda.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Numerazione
        End If

        ' creo il DATALIST delle Opzioni di Rating
        Dim dlopzioni As New DataList
        dlopzioni = FindControlRecursive(FRVDomanda, "DLOpzioni")
        Dim nOpzione As Integer = 1
        Dim nRisposteCorrette As Integer = 0

        If oQuest.isReadOnly Then
            'update di domanda e di testo, corretta e peso per le opzioni
            For Each opzioneRating As DomandaOpzione In oDomanda.domandaRating.opzioniRating
                opzioneRating.testo = DirectCast(FindControlRecursive(dlopzioni.Items(opzioneRating.numero - 1), "TXBTestoMin"), TextBox).Text
            Next
        Else
            oDomanda.domandaRating.opzioniRating.Clear()
            For Each item As DataListItem In dlopzioni.Items
                'If item.FindControl("TXBScelta").GetType() Is GetType(WebControls.TextBox) Then
                Dim txbScelta As New TextBox
                txbScelta = DirectCast(item.FindControl("TXBTestoMin"), TextBox)
                Dim txbScelta2 As New TextBox
                txbScelta2 = DirectCast(item.FindControl("TXBTestoMax"), TextBox)
                If Not txbScelta.Text = String.Empty Then
                    Dim oDomandaOpzione As New DomandaOpzione
                    oDomandaOpzione.numero = nOpzione
                    oDomandaOpzione.testo = txbScelta.Text
                    oDomandaOpzione.testoDopo = txbScelta2.Text
                    oDomandaOpzione.isAltro = DirectCast(item.FindControl("CBisAltro"), CheckBox).Checked
                    Dim c As Int16
                    Dim tabella As New Table
                    Dim cb As New CheckBox
                    tabella = DirectCast(FindControlRecursive(FRVDomanda, "TBLCheckbox"), Table)
                    If Not isChangingTable Then
                        For c = 0 To oDomanda.domandaRating.numeroRating - 1
                            Try
                                cb = DirectCast(tabella.Rows(nOpzione - 1).Cells(c).Controls(0), CheckBox)
                                oDomandaOpzione.arrayCBisVisible = oDomandaOpzione.arrayCBisVisible & CStr(Math.Abs(CInt(cb.Checked)))
                            Catch
                                oDomandaOpzione.arrayCBisVisible = oDomandaOpzione.arrayCBisVisible & "0"
                            End Try
                        Next
                    End If
                    oDomanda.domandaRating.opzioniRating.Add(oDomandaOpzione)
                    nOpzione = nOpzione + 1
                End If
            Next
        End If
        oDomanda.domandaRating.testoND = DirectCast(FindControlRecursive(FRVDomanda, "TXBTestoND"), TextBox).Text
        Return oDomanda
    End Function

    Public Function setDomandaRatingStars(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal oQuest As Questionario, Optional ByVal isChangingTable As Boolean = False) As Domanda

        Dim oPagina As New QuestionarioPagina

        oDomanda.domandaRating.mostraND = DirectCast(FindControlRecursive(FRVDomanda, "CBmostraND"), CheckBox).Checked
        oDomanda.domandaRating.numeroRating = DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroColonne"), DropDownList).SelectedValue
        'oDomanda.tipo = Domanda.TipoDomanda.RatingStars <- SE lo metto qui, salta il bind sulle righe, anche aggiungendo il case...
        ' Viene impostato direttamente su "Salva" usando il rating normale per tutto il resto...

        'Considero le intestazioni come numeri a prescindere.
        oDomanda.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Numerazione

        ' creo il DATALIST delle Opzioni di Rating
        Dim dlopzioni As New DataList
        dlopzioni = FindControlRecursive(FRVDomanda, "DLOpzioni")
        Dim nOpzione As Integer = 1
        Dim nRisposteCorrette As Integer = 0

        If oQuest.isReadOnly Then
            'update di domanda e di testo, corretta e peso per le opzioni
            For Each opzioneRating As DomandaOpzione In oDomanda.domandaRating.opzioniRating
                opzioneRating.testo = DirectCast(FindControlRecursive(dlopzioni.Items(opzioneRating.numero - 1), "TXBTestoMin"), TextBox).Text
            Next
        Else
            oDomanda.domandaRating.opzioniRating.Clear()
            For Each item As DataListItem In dlopzioni.Items
                'If item.FindControl("TXBScelta").GetType() Is GetType(WebControls.TextBox) Then
                Dim txbScelta As New TextBox
                txbScelta = DirectCast(item.FindControl("TXBTestoMin"), TextBox)
                Dim txbScelta2 As New TextBox
                txbScelta2 = DirectCast(item.FindControl("TXBTestoMax"), TextBox)
                If Not txbScelta.Text = String.Empty Then
                    Dim oDomandaOpzione As New DomandaOpzione
                    oDomandaOpzione.numero = nOpzione
                    oDomandaOpzione.testo = txbScelta.Text
                    oDomandaOpzione.testoDopo = txbScelta2.Text
                    oDomandaOpzione.isAltro = DirectCast(item.FindControl("CBisAltro"), CheckBox).Checked
                    Dim c As Int16
                    Dim tabella As New Table
                    Dim cb As New CheckBox
                    tabella = DirectCast(FindControlRecursive(FRVDomanda, "TBLCheckbox"), Table)
                    If Not isChangingTable Then
                        For c = 0 To oDomanda.domandaRating.numeroRating - 1
                            'Try
                            '    cb = DirectCast(tabella.Rows(nOpzione - 1).Cells(c).Controls(0), CheckBox)
                            '    oDomandaOpzione.arrayCBisVisible = oDomandaOpzione.arrayCBisVisible & CStr(Math.Abs(CInt(cb.Checked)))
                            'Catch
                            '    oDomandaOpzione.arrayCBisVisible = oDomandaOpzione.arrayCBisVisible & "0"
                            'End Try
                            oDomandaOpzione.arrayCBisVisible = True
                        Next
                    End If
                    oDomanda.domandaRating.opzioniRating.Add(oDomandaOpzione)
                    nOpzione = nOpzione + 1
                End If
            Next
        End If
        oDomanda.domandaRating.testoND = DirectCast(FindControlRecursive(FRVDomanda, "TXBTestoND"), TextBox).Text
        Return oDomanda
    End Function

    'legge la domanda da domandaEdit
    Public Function setDomandaMeeting(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal oQuest As Questionario, Optional ByVal isChangingTable As Boolean = False) As Domanda

        Dim oPagina As New QuestionarioPagina

        'bisogna ordinare gli elementi in uscita dal radControl, altrimenti escono nell'ordine nel quale son stati selezionati
        oDomanda.domandaRating.intestazioniMeeting = (From x In RootObject.RadDate_ToList(DirectCast(FRVDomanda.FindControl("RDCLCalendar"), Telerik.Web.UI.RadCalendar).SelectedDates) Order By x.Date).ToList

        oDomanda.domandaRating.mostraND = False
        oDomanda.domandaRating.numeroRating = oDomanda.domandaRating.intestazioniMeeting.Count  'oDates.Count  'oDate.Count 'DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroColonne"), DropDownList).SelectedValue

        Dim RPTZone As New Repeater
        RPTZone = DirectCast(FRVDomanda.FindControl("RPTZone"), Repeater)
        Dim index As Int16 = 0
        oDomanda.domandaRating.opzioniRating.Clear()

        For Each item As RepeaterItem In RPTZone.Items
            Dim oDomandaOpzione As New DomandaOpzione
            Dim txb As New TextBox
            txb = DirectCast(item.Controls(1), TextBox)
            If txb Is Nothing Then
                oDomandaOpzione.testo = String.Empty
            Else
                oDomandaOpzione.testo = txb.Text
            End If
            oDomanda.domandaRating.opzioniRating.Add(oDomandaOpzione)
            index = index + 1
        Next
        oDomanda.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Testi
        Return oDomanda
    End Function

    Public Sub bindFieldDomandaTestoLibero(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal opagina As QuestionarioPagina, ByVal oQuest As Questionario)

        Dim listDom As New List(Of Domanda)
        listDom.Add(oDomanda)
        FRVDomanda.DataSource = listDom
        FRVDomanda.DataBind()
        'If oQuest.tipo = Questionario.TipoQuestionario.Questionario Then
        DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).DataSource = oQuest.pagine
        DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).DataBind()
        If oDomanda.idPagina > 0 Then
            DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).SelectedValue = oDomanda.idPagina
        ElseIf Not opagina Is Nothing Then
            DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).SelectedValue = opagina.id
        End If
        bindOpzioniDomandaTestoLibero(FRVDomanda, oDomanda)
        Dim editor As Comunita_OnLine.UC_Editor = FindControlRecursive(FRVDomanda, "CTRLeditorTestoDomanda")
        If Not IsNothing(editor) Then
            If Not editor.isInitialized Then
                editor.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
            End If
            editor.HTML = oDomanda.testo
        End If

        DirectCast(FindControlRecursive(FRVDomanda, "TXBTestoDopoDomanda"), TextBox).Text = oDomanda.testoDopo
        DirectCast(FindControlRecursive(FRVDomanda, "TXBPeso"), TextBox).Text = oDomanda.peso
        DirectCast(FindControlRecursive(FRVDomanda, "DDLDifficolta"), DropDownList).SelectedIndex = oDomanda.difficolta
        DirectCast(FindControlRecursive(FRVDomanda, "TXBSuggerimento"), TextBox).Text = oDomanda.suggerimento
        Dim div As HtmlGenericControl = FindControlRecursive(FRVDomanda, "DIVpaginaCorrente")
        If Not IsNothing(div) Then
            div.Visible = (oQuest.tipo <> QuestionnaireType.QuestionLibrary AndAlso oQuest.tipo <> QuestionnaireType.Poll AndAlso oQuest.tipo <> QuestionnaireType.Model AndAlso oQuest.tipo <> QuestionnaireType.Meeting)
        End If
    End Sub
    Public Sub bindOpzioniDomandaTestoLibero(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda)
        Dim dlOpzioni As New DataList
        dlOpzioni = DirectCast(FindControlRecursive(FRVDomanda, "DLOpzioni"), DataList)
        If oDomanda.opzioniTestoLibero.Count = 0 Then
            Dim oOpzione1 As New DomandaTestoLibero
            oDomanda.opzioniTestoLibero.Add(oOpzione1)
        End If
        dlOpzioni.DataSource = oDomanda.opzioniTestoLibero
        dlOpzioni.DataBind()
        DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedValue = oDomanda.opzioniTestoLibero.Count
    End Sub
    Public Function setDomandaTestoLibero(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal oQuest As Questionario, Optional ByRef isMinorUpdate As Boolean = True) As Domanda
        Dim oPagina As New QuestionarioPagina
        Dim dlOpzioni As New DataList
        dlOpzioni = FindControlRecursive(FRVDomanda, "DLOpzioni")
        oDomanda.isValutabile = DirectCast(FindControlRecursive(FRVDomanda, "CHKisValutabile"), CheckBox).Checked
        oDomanda.testo = DirectCast(FindControlRecursive(FRVDomanda, "CTRLeditorTestoDomanda"), Comunita_OnLine.UC_Editor).HTML
        Dim nOpzione As Integer = 1
        If dlOpzioni.Items.Count > 1 Then
            dlOpzioni.Visible = True
        End If
        If isMinorUpdate = True And oQuest.tipo = Questionario.TipoQuestionario.LibreriaDiDomande Then

            Dim numeroOpzioni As Integer = DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedItem.Value

            If Not numeroOpzioni = oDomanda.opzioniTestoLibero.Count Then
                isMinorUpdate = False
            End If

            'update di domanda e di testo, corretta e peso per le opzioni
            For Each opzioneTestoLibero As DomandaTestoLibero In oDomanda.opzioniTestoLibero
                If opzioneTestoLibero.numero > 0 Then
                    Dim nuovaEtichetta As String = DirectCast(FindControlRecursive(dlOpzioni.Items(opzioneTestoLibero.numero - 1), "CTRLeditorEtichetta"), Comunita_OnLine.UC_Editor).HTML

                    If Not nuovaEtichetta = opzioneTestoLibero.etichetta Then
                        isMinorUpdate = False
                        Exit For
                    End If
                    opzioneTestoLibero.peso = DirectCast(FindControlRecursive(dlOpzioni.Items(opzioneTestoLibero.numero - 1), "TXBPesoRisposta"), TextBox).Text
                End If
            Next
        End If

        If isMinorUpdate And Not oQuest.tipo = Questionario.TipoQuestionario.LibreriaDiDomande Then
            'update di domanda e di testo, corretta e peso per le opzioni
            For Each opzioneTestoLibero As DomandaTestoLibero In oDomanda.opzioniTestoLibero
                If opzioneTestoLibero.numero > 0 Then
                    opzioneTestoLibero.etichetta = DirectCast(FindControlRecursive(dlOpzioni.Items(opzioneTestoLibero.numero - 1), "CTRLeditorEtichetta"), Comunita_OnLine.UC_Editor).HTML
                End If
            Next
        Else
            isMinorUpdate = False
        End If

        If isMinorUpdate = False Then
            'If nOpzione > oDomanda.opzioniTestoLibero.Count Then
            '    Dim txbScelta As New Comunita_OnLine.UC_VisualEditor
            '    txbScelta = DirectCast(dlOpzioni.Items(nOpzione - 1).FindControl("CTRLeditorEtichetta"), Comunita_OnLine.UC_VisualEditor)
            '    Dim oDomandaOpzione As New DomandaTestoLibero
            '    oDomandaOpzione.etichetta = txbScelta.HTML
            '    oDomandaOpzione.idDomanda = oDomanda.idDomandaMultilingua
            '    oDomandaOpzione.numeroColonne = 1
            '    oDomandaOpzione.numeroRighe = 1
            '    oDomandaOpzione.numero = nOpzione
            '    oDomandaOpzione.peso = DirectCast(dlOpzioni.Items(nOpzione - 1).FindControl("TXBPesoRisposta"), TextBox).Text
            '    oDomanda.opzioniTestoLibero.Add(oDomandaOpzione)
            '    nOpzione = nOpzione + 1
            'Else

            'End If


            oDomanda.opzioniTestoLibero.Clear()
            For Each item As DataListItem In dlOpzioni.Items
                Dim txbScelta As New Comunita_OnLine.UC_Editor
                txbScelta = DirectCast(item.FindControl("CTRLeditorEtichetta"), Comunita_OnLine.UC_Editor)
                If Not txbScelta.HTML = String.Empty Then
                    Dim oDomandaOpzione As New DomandaTestoLibero
                    oDomandaOpzione.etichetta = txbScelta.HTML
                    oDomandaOpzione.idDomanda = oDomanda.idDomandaMultilingua
                    oDomandaOpzione.numeroColonne = 1
                    oDomandaOpzione.numeroRighe = 1
                    oDomandaOpzione.numero = nOpzione
                    oDomandaOpzione.peso = DirectCast(item.FindControl("TXBPesoRisposta"), TextBox).Text
                    oDomanda.opzioniTestoLibero.Add(oDomandaOpzione)
                    nOpzione = nOpzione + 1
                End If
            Next
        End If
        Return oDomanda
    End Function
    Public Sub bindFieldDomandaNumerica(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal opagina As QuestionarioPagina, ByVal oQuest As Questionario)
        Dim listDom As New List(Of Domanda)
        listDom.Add(oDomanda)
        FRVDomanda.DataSource = listDom
        FRVDomanda.DataBind()

        DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).DataSource = oQuest.pagine
        DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).DataBind()
        If oDomanda.idPagina > 0 Then
            DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).SelectedValue = oDomanda.idPagina
        Else
            DirectCast(FindControlRecursive(FRVDomanda, "DDLPagina"), DropDownList).SelectedValue = opagina.id
        End If

        bindOpzioniDomandaNumerica(FRVDomanda, oDomanda)

        Dim editor As Comunita_OnLine.UC_Editor = FindControlRecursive(FRVDomanda, "CTRLeditorTestoDomanda")
        If Not IsNothing(editor) Then
            If Not editor.isInitialized Then
                editor.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
            End If
            editor.HTML = oDomanda.testo
        End If
        DirectCast(FindControlRecursive(FRVDomanda, "TXBTestoDopoDomanda"), TextBox).Text = oDomanda.testoDopo
        DirectCast(FindControlRecursive(FRVDomanda, "TXBPeso"), TextBox).Text = oDomanda.peso
        DirectCast(FindControlRecursive(FRVDomanda, "DDLDifficolta"), DropDownList).SelectedIndex = oDomanda.difficolta
        DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedValue = oDomanda.opzioniNumerica.Count
        DirectCast(FindControlRecursive(FRVDomanda, "TXBSuggerimento"), TextBox).Text = oDomanda.suggerimento
        Dim div As HtmlGenericControl = FindControlRecursive(FRVDomanda, "DIVpaginaCorrente")
        If Not IsNothing(div) Then
            div.Visible = (oQuest.tipo <> QuestionnaireType.QuestionLibrary AndAlso oQuest.tipo <> QuestionnaireType.Poll AndAlso oQuest.tipo <> QuestionnaireType.Model AndAlso oQuest.tipo <> QuestionnaireType.Meeting)
        End If
    End Sub
    Public Sub bindOpzioniDomandaNumerica(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda)

        Dim dlOpzioni As New DataList
        dlOpzioni = DirectCast(FindControlRecursive(FRVDomanda, "DLOpzioni"), DataList)
        If oDomanda.opzioniNumerica.Count = 0 Then
            Dim oOpzione1 As New DomandaNumerica
            oOpzione1.numero = 1
            oDomanda.opzioniNumerica.Add(oOpzione1)
        End If
        dlOpzioni.DataSource = oDomanda.opzioniNumerica
        dlOpzioni.DataBind()

    End Sub
    Public Function setDomandaNumerica(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal oQuest As Questionario, Optional ByRef isMinorUpdate As Boolean = True) As Domanda

        Dim oPagina As New QuestionarioPagina
        oDomanda.isValutabile = DirectCast(FindControlRecursive(FRVDomanda, "CHKisValutabile"), CheckBox).Checked
        oDomanda.testo = DirectCast(FindControlRecursive(FRVDomanda, "CTRLeditorTestoDomanda"), Comunita_OnLine.UC_Editor).HTML
        Dim dlopzioni As New DataList
        dlopzioni = FindControlRecursive(FRVDomanda, "DLOpzioni")
        Dim nOpzione As Integer = 1

        If isMinorUpdate = True And oQuest.tipo = Questionario.TipoQuestionario.LibreriaDiDomande Then

            Dim numeroOpzioni As Integer = DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedItem.Value

            If Not numeroOpzioni = oDomanda.opzioniNumerica.Count Then
                isMinorUpdate = False
            End If

            'update di domanda e di testo, corretta e peso per le opzioni
            For Each opzioneNumerica As DomandaNumerica In oDomanda.opzioniNumerica
                Dim nuovaEtichetta As String = opzioneNumerica.testoPrima = DirectCast(FindControlRecursive(dlopzioni.Items(opzioneNumerica.numero - 1), "CTRLeditorTestoPrima"), Comunita_OnLine.UC_Editor).HTML

                If Not nuovaEtichetta = opzioneNumerica.testoPrima Then
                    isMinorUpdate = False
                    Exit For
                End If
                opzioneNumerica.peso = DirectCast(FindControlRecursive(dlopzioni.Items(opzioneNumerica.numero - 1), "TXBPesoRisposta"), TextBox).Text

            Next
        End If

        If isMinorUpdate And Not oQuest.tipo = Questionario.TipoQuestionario.LibreriaDiDomande Then
            'update di domanda e di testo, corretta e peso per le opzioni
            For Each opzioneNumerica As DomandaNumerica In oDomanda.opzioniNumerica.Where(Function(o) Not String.IsNullOrEmpty(o.numero)).Where(Function(o) CInt(o.numero) > 0).ToList()
                opzioneNumerica.testoPrima = DirectCast(FindControlRecursive(dlopzioni.Items(opzioneNumerica.numero - 1), "CTRLeditorTestoPrima"), Comunita_OnLine.UC_Editor).HTML
            Next
        Else
            isMinorUpdate = False
        End If

        If isMinorUpdate = False Then
            oDomanda.opzioniNumerica.Clear()

            For Each item As DataListItem In dlopzioni.Items
                'If item.FindControl("TXBScelta").GetType() Is GetType(WebControls.TextBox) Then
                Dim txbRispCorretta As New TextBox
                txbRispCorretta = DirectCast(item.FindControl("TXBRispostaCorretta"), TextBox)
                If txbRispCorretta.Text = String.Empty Then
                    txbRispCorretta.Text = 0
                End If
                Dim txbTestoPrima As New Comunita_OnLine.UC_Editor
                txbTestoPrima = DirectCast(item.FindControl("CTRLeditorTestoPrima"), Comunita_OnLine.UC_Editor)

                Dim txbPesoRisposta As New TextBox
                txbPesoRisposta = DirectCast(item.FindControl("TXBPesoRisposta"), TextBox)
                If txbPesoRisposta.Text = String.Empty Then
                    txbPesoRisposta.Text = 100
                End If

                Dim txbTestoDopo As New TextBox
                txbTestoDopo = DirectCast(item.FindControl("TXBTestoDopo"), TextBox)

                Dim txbDimensione As New TextBox
                txbDimensione = DirectCast(item.FindControl("TXBDimensione"), TextBox)
                If txbDimensione.Text = String.Empty Then
                    txbDimensione.Text = 0
                End If

                If Not txbTestoPrima.HTML = String.Empty Then
                    Dim oDomandaOpzione As New DomandaNumerica
                    oDomandaOpzione.testoPrima = txbTestoPrima.HTML
                    oDomandaOpzione.idDomanda = oDomanda.idDomandaMultilingua
                    oDomandaOpzione.testoDopo = txbTestoDopo.Text
                    oDomandaOpzione.rispostaCorretta = txbRispCorretta.Text
                    oDomandaOpzione.dimensione = txbDimensione.Text
                    oDomandaOpzione.numero = nOpzione
                    oDomandaOpzione.peso = txbPesoRisposta.Text
                    oDomanda.opzioniNumerica.Add(oDomandaOpzione)
                    nOpzione = nOpzione + 1

                End If
                'End If
            Next
        End If

        ' se non ho inserito opzioni ne metto una di default con numero = 1
        If oDomanda.opzioniNumerica.Count = 0 Then
            Dim oDomandaOpzione As New DomandaNumerica
            oDomandaOpzione.idDomanda = oDomanda.idDomandaMultilingua
            oDomandaOpzione.numero = 1
            oDomanda.opzioniNumerica.Add(oDomandaOpzione)
        End If



        Return oDomanda
    End Function
    Public Sub selezionaNumeroOpzioni(ByVal oDomanda As Domanda, ByVal nOpzioniScelte As Integer, ByRef dlOpzioni As DataList)

        Select Case oDomanda.tipo
            Case Domanda.TipoDomanda.Multipla

                Dim nOpzioniCorrenti As Integer = oDomanda.domandaMultiplaOpzioni.Count

                If nOpzioniScelte > nOpzioniCorrenti Then
                    For n As Integer = 1 To nOpzioniScelte - nOpzioniCorrenti
                        Dim newOpzione As New DomandaOpzione
                        oDomanda.domandaMultiplaOpzioni.Add(newOpzione)
                    Next
                Else
                    For n As Integer = 1 To nOpzioniCorrenti - nOpzioniScelte
                        oDomanda.domandaMultiplaOpzioni.RemoveAt(oDomanda.domandaMultiplaOpzioni.Count - 1)
                    Next
                End If
            Case Domanda.TipoDomanda.DropDown

                Dim nOpzioniCorrenti As Integer = oDomanda.domandaDropDown.dropDownItems.Count

                If nOpzioniScelte > nOpzioniCorrenti Then
                    For n As Integer = 1 To nOpzioniScelte - nOpzioniCorrenti
                        Dim newOpzione As New DropDownItem
                        newOpzione.testo = "--"
                        oDomanda.domandaDropDown.dropDownItems.Add(newOpzione)
                    Next
                Else
                    For n As Integer = 1 To nOpzioniCorrenti - nOpzioniScelte
                        oDomanda.domandaDropDown.dropDownItems.RemoveAt(oDomanda.domandaDropDown.dropDownItems.Count - 1)
                    Next
                End If

            Case Domanda.TipoDomanda.Rating

                Dim nOpzioniCorrenti As Integer = oDomanda.domandaRating.opzioniRating.Count

                If nOpzioniScelte > nOpzioniCorrenti Then
                    For n As Integer = 1 To nOpzioniScelte - nOpzioniCorrenti
                        Dim newOpzione As New DomandaOpzione
                        oDomanda.domandaRating.opzioniRating.Add(newOpzione)
                    Next
                Else
                    For n As Integer = 1 To nOpzioniCorrenti - nOpzioniScelte
                        oDomanda.domandaRating.opzioniRating.RemoveAt(oDomanda.domandaRating.opzioniRating.Count - 1)
                    Next
                End If

            Case Domanda.TipoDomanda.Meeting

                Dim nOpzioniCorrenti As Integer = oDomanda.domandaRating.opzioniRating.Count

                If nOpzioniScelte > nOpzioniCorrenti Then
                    For n As Integer = 1 To nOpzioniScelte - nOpzioniCorrenti
                        Dim newOpzione As New DomandaOpzione
                        oDomanda.domandaRating.opzioniRating.Add(newOpzione)
                    Next
                Else
                    For n As Integer = 1 To nOpzioniCorrenti - nOpzioniScelte
                        oDomanda.domandaRating.opzioniRating.RemoveAt(oDomanda.domandaRating.opzioniRating.Count - 1)
                    Next
                End If


            Case Domanda.TipoDomanda.TestoLibero

                Dim nOpzioniCorrenti As Integer = oDomanda.opzioniTestoLibero.Count
                'If nOpzioniScelte > 1 Then
                '    dlOpzioni.Visible = True
                'End If
                If nOpzioniScelte > nOpzioniCorrenti Then
                    For n As Integer = 1 To nOpzioniScelte - nOpzioniCorrenti
                        Dim newOpzione As New DomandaTestoLibero
                        oDomanda.opzioniTestoLibero.Add(newOpzione)
                    Next
                Else
                    For n As Integer = 1 To nOpzioniCorrenti - nOpzioniScelte
                        oDomanda.opzioniTestoLibero.RemoveAt(oDomanda.opzioniTestoLibero.Count - 1)
                    Next
                End If

            Case Domanda.TipoDomanda.Numerica

                Dim nOpzioniCorrenti As Integer = oDomanda.opzioniNumerica.Count

                If nOpzioniScelte > nOpzioniCorrenti Then
                    For n As Integer = 1 To nOpzioniScelte - nOpzioniCorrenti
                        Dim newOpzione As New DomandaNumerica
                        newOpzione.testoPrima = "--"
                        oDomanda.opzioniNumerica.Add(newOpzione)
                    Next
                Else
                    For n As Integer = 1 To nOpzioniCorrenti - nOpzioniScelte
                        oDomanda.opzioniNumerica.RemoveAt(oDomanda.opzioniNumerica.Count - 1)
                    Next
                End If
        End Select

    End Sub
    Public Sub selezionaNumeroIntestazioni(ByVal oDomanda As Domanda, ByVal ddlIntestazioni As DropDownList)

        Dim nOpzioniScelte As Integer = ddlIntestazioni.SelectedValue

        Dim nOpzioniCorrenti As Integer = oDomanda.domandaRating.intestazioniRating.Count

        If nOpzioniScelte > nOpzioniCorrenti Then
            For n As Integer = 1 To nOpzioniScelte - nOpzioniCorrenti
                Dim newOpzione As New DomandaOpzione
                oDomanda.domandaRating.intestazioniRating.Add(newOpzione)
            Next
        Else
            For n As Integer = 1 To nOpzioniCorrenti - nOpzioniScelte
                oDomanda.domandaRating.intestazioniRating.RemoveAt(oDomanda.domandaRating.intestazioniRating.Count - 1)
            Next
        End If

    End Sub
    Public Sub bindOpzioniDomandaRating(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal opagina As QuestionarioPagina, ByVal oQuest As Questionario)

        Dim dlOpzioni As New DataList
        dlOpzioni = DirectCast(FindControlRecursive(FRVDomanda, "DLOpzioni"), DataList)
        If oDomanda.domandaRating.opzioniRating.Count = 0 Then
            Dim oOpzione1 As New DomandaOpzione
            oDomanda.domandaRating.opzioniRating.Add(oOpzione1)
        End If
        dlOpzioni.DataSource = oDomanda.domandaRating.opzioniRating
        dlOpzioni.DataBind()

        DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedValue = oDomanda.domandaRating.opzioniRating.Count


    End Sub

    Public Sub bindOpzioniDomandaRatingStars(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal opagina As QuestionarioPagina, ByVal oQuest As Questionario)

        Dim dlOpzioni As New DataList
        dlOpzioni = DirectCast(FindControlRecursive(FRVDomanda, "DLOpzioni"), DataList)
        If oDomanda.domandaRating.opzioniRating.Count = 0 Then
            Dim oOpzione1 As New DomandaOpzione
            oDomanda.domandaRating.opzioniRating.Add(oOpzione1)
        End If
        dlOpzioni.DataSource = oDomanda.domandaRating.opzioniRating
        dlOpzioni.DataBind()

        DirectCast(FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedValue = oDomanda.domandaRating.opzioniRating.Count


    End Sub

    Public Sub bindZoneDomandaMeeting(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal opagina As QuestionarioPagina, ByVal oQuest As Questionario)
        Dim rptZone As New Repeater
        rptZone = FRVDomanda.FindControl("RPTZone")
        rptZone.DataSource = oDomanda.domandaRating.opzioniRating
        rptZone.DataBind()

        'Dim PHZone As New PlaceHolder
        'PHZone = DirectCast(FRVDomanda.FindControl("PHZone"), PlaceHolder)

        'For Each TXBZone As TextBox In PHZone.Controls
        '    Dim index As Int16 = CInt(TXBZone.ID.Remove(7))
        '    If index > oDomanda.domandaRating.opzioniRating.Count Then
        '        TXBZone.Text = String.Empty
        '    Else
        '        TXBZone.Text = oDomanda.domandaRating.opzioniRating(index).testo
        '    End If
        'Next


        'If oDomanda.domandaRating.opzioniRating.Count = 0 Then
        '    Dim oOpzione1 As New DomandaOpzione
        '    oDomanda.domandaRating.opzioniRating.Add(oOpzione1)
        'End If

    End Sub
    Public Sub bindIntestazioniDomandaRating(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal opagina As QuestionarioPagina, ByVal oQuest As Questionario)

        If oDomanda.domandaRating.intestazioniRating.Count = 0 Then
            For i As Integer = 1 To 2
                Dim oInt As New DomandaOpzione
                oInt.testo = i.ToString()
                oDomanda.domandaRating.intestazioniRating.Add(oInt)
            Next
        End If

        Dim DLIntestazioni As New DataList
        DLIntestazioni = DirectCast(FindControlRecursive(FRVDomanda, "DLIntestazioni"), DataList)

        If oDomanda.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Numerazione Then
            DLIntestazioni.Enabled = False
            Dim i As Integer = 1
            For Each oInt As DomandaOpzione In oDomanda.domandaRating.intestazioniRating
                oInt.testo = i.ToString()
                i = i + 1
            Next
        Else
            DLIntestazioni.Enabled = Not oQuest.isReadOnly
            Dim i As Integer = 1
            For Each oInt As DomandaOpzione In oDomanda.domandaRating.intestazioniRating
                Try
                    If Integer.Parse(oInt.testo) Then
                        oInt.testo = String.Empty
                        i = i + 1
                    End If
                Catch ex As Exception

                End Try

            Next

        End If


        DLIntestazioni.DataSource = oDomanda.domandaRating.intestazioniRating
        DLIntestazioni.DataBind()

        'End If

        ' DirectCast(FRVDomanda.FindControl("DDLNumero"), DropDownList).SelectedValue = oDomanda.domandaRating.numeroRating


    End Sub


    Public Sub bindIntestazioniDomandaRatingStars(ByVal FRVDomanda As FormView, ByVal oDomanda As Domanda, ByVal opagina As QuestionarioPagina, ByVal oQuest As Questionario)

        'If oDomanda.domandaRating.intestazioniRating.Count = 0 Then
        '    For i As Integer = 1 To 2
        '        Dim oInt As New DomandaOpzione
        '        oInt.testo = i.ToString()
        '        oDomanda.domandaRating.intestazioniRating.Add(oInt)
        '    Next
        'End If

        'Dim DLIntestazioni As New DataList
        'DLIntestazioni = DirectCast(FindControlRecursive(FRVDomanda, "DLIntestazioni"), DataList)

        'If oDomanda.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Numerazione Then
        '    DLIntestazioni.Enabled = False
        '    Dim i As Integer = 1
        '    For Each oInt As DomandaOpzione In oDomanda.domandaRating.intestazioniRating
        '        oInt.testo = i.ToString()
        '        i = i + 1
        '    Next
        'Else
        '    DLIntestazioni.Enabled = Not oQuest.isReadOnly
        '    Dim i As Integer = 1
        '    For Each oInt As DomandaOpzione In oDomanda.domandaRating.intestazioniRating
        '        Try
        '            If Integer.Parse(oInt.testo) Then
        '                oInt.testo = String.Empty
        '                i = i + 1
        '            End If
        '        Catch ex As Exception

        '        End Try

        '    Next

        'End If


        'DLIntestazioni.DataSource = oDomanda.domandaRating.intestazioniRating
        'DLIntestazioni.DataBind()

        'DLIntestazioni.Visible = False


        'End If

        ' DirectCast(FRVDomanda.FindControl("DDLNumero"), DropDownList).SelectedValue = oDomanda.domandaRating.numeroRating


    End Sub
#End Region

#Region "Metodi di Ricerca"

    'Public Function findDomandaBYID(ByVal listaDom As List(Of Domanda), ByVal idDomanda As String) As Domanda

    '    Dim odomanda As New Domanda
    '    odomanda = listaDom.Find(New PredicateWrapper(Of Domanda, String)(idDomanda, AddressOf trovaID))

    '    Return odomanda

    'End Function

    'Public Shared Function trovaID(ByVal item As Domanda, ByVal argument As String) As Boolean

    '    Return item.id = argument

    'End Function

    'Public Shared Function findDomandaBYNumero(ByVal listaDom As List(Of Domanda), ByVal numeroDomanda As String) As Domanda

    '    Dim odomanda As New Domanda
    '    odomanda = listaDom.Find(New PredicateWrapper(Of Domanda, String)(numeroDomanda, AddressOf trovaNumero))

    '    Return odomanda

    'End Function

    'Public Shared Function trovaNumero(ByVal item As Domanda, ByVal argument As String) As Boolean

    '    Return item.numero = argument

    'End Function

#End Region

    Public Shared Function FindControlRecursive(ByVal owner As Control, ByVal controlID As String) As Control
        Dim myControl As Control = Nothing
        If owner.Controls.Count > 0 Then
            For Each c As Control In owner.Controls
                myControl = FindControlRecursive(c, controlID)
                If Not (myControl Is Nothing) Then
                    Return myControl
                End If
            Next
        End If
        If controlID.Equals(owner.ID) Then
            Return owner
        End If
        Return Nothing
    End Function
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
    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get

        End Get
    End Property
    Public Overrides Sub SetControlliByPermessi()

    End Sub
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
End Class
