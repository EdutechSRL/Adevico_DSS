Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports COL_Questionario
Imports System.Web.UI.WebControls
Imports System.Math
Imports System.Linq
Imports System.Security.AccessControl
Imports lm.Comol.Modules.Base.DomainModel
Imports Telerik.Charting
Imports Telerik.Web.UI
Imports Telerik.Web.UI.HtmlChart
Imports Telerik.Web.UI.HtmlChart.PlotArea

Public Class GestioneRisposte
    Inherits PageBaseQuestionario
    Dim oGestioneDomande As New GestioneDomande
    Private _ApplicationUrlBase As String
    Private _SmartTagsAvailable As SmartTags

    Private Const _MaxCharacterForchartInline As Integer = 25
    Private Const _MaxCharacterForchartNewline As Integer = 80

    Private Const _CorrectString As String = "<span class=""TestoRispostaCorretta"">&nbsp;</span>"
    Private Const _FormatPerc As String = "N1"
    Private Const _FormatAvg As String = "N1"
    Private Const _FormatDev As String = "N2"

    Private Const _ClassText As String = "tdTextual"
    Private Const _ClassData As String = "tdData"
    Private Const _ClassNoData As String = "tdData empty"
    Private Const _ClassLink As String = "tdLink"
    Private Const _ClassIndex As String = "tdIndex"


    Private Const _NewLineCharacter As Integer = 20
    Private Const _NewLineMaxHeaderChar As Integer = 50

    Public Function SalvaRisposta(
                                 ByRef oQuest As Questionario,
                                 ByVal UserId As Integer,
                                 ByVal CloseAnswer As Boolean) As String

        oQuest.rispostaQuest.oStatistica = calcoloPunteggioAutovalutazione(oQuest)

        Return Salva(
            oQuest,
            oQuest.durata,
            Me.QuestionarioCorrente.tipo = QuestionnaireType.AutoEvaluation OrElse Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts,
            Me.isUtenteAnonimo Or Me.isAnonymousCompiler Or Invito.PersonaID = idUtenteAnonimo(),
            UserId,
            CloseAnswer)

    End Function
    Public Function Salva(
                         ByRef oQuest As Questionario,
                         ByVal durata As Integer,
                         ByVal isAutoval As Boolean,
                         ByRef isUtenteAnonimo As Boolean,
                         ByVal UserId As Integer,
                         ByVal CloseAnswer As Boolean) As String




        'valutare se convertire il tipo della funzione in integer
        Dim retVal As String = String.Empty
        Dim oDalRisposte As New DALRisposte
        If durata > 0 Then
            ' If (DateDiff("s", oQuest.rispostaQuest.dataFine, oQuest.rispostaQuest.dataInizio) > RootObject.maxOvertimeSalvataggio) And (DateDiff("s", oQuest.rispostaQuest.dataModifica, oQuest.rispostaQuest.dataInizio) > RootObject.maxOvertimeSalvataggio) Then'doppio controllo necessario perche' la data di fine non viene impostata quanto viene semplicemente salvato, 

            If (DateDiff("s", oQuest.rispostaQuest.dataInizio, Now) > (oQuest.durata * 60 + RootObject.maxOvertimeSalvataggio)) Then 'se non e' a "now" il salvataggio automatico delle risposte "salta" perche' datamodifica e/o datafine non sono impostati come si crede
                retVal = "-1"
                Return retVal
            End If
        End If
        Try
            If oQuest.rispostaQuest.id > 0 Then
                ' NO:RIMOSSO IL 19/02/2014
                'Aggiornato il 21/11/2017 con controllo che sia l'utente corrente ad aggiornare le proprie risposte. E SOLO LUI!
                Dim IsCurrent As Boolean = False
                Try
                    ''aaa
                    IsCurrent = oDalRisposte.RispostaQuestionario_CheckUpdateUpdate(oQuest.rispostaQuest.id, UserId)
                Catch ex As Exception

                End Try

                If Not IsCurrent Then
                    Return "-1"
                End If
                oDalRisposte.clearRisposte(oQuest.rispostaQuest) 'Necessario per ri-compilazioni, ma PERICOLOSSSSISSSISMO!
                'Tag: cancella risposte questinario
                oDalRisposte.RispostaQuestionario_Update(CloseAnswer, oQuest.rispostaQuest)
            Else
                Dim idPersona As Integer
                If Invito.ID > 0 AndAlso Not isAutoval AndAlso Not isUtenteAnonimo AndAlso oDalRisposte.countRisposteByIdPersona(Invito.PersonaID, oQuest.rispostaQuest.idQuestionario) > 0 Then
                    Return "0"
                ElseIf oDalRisposte.countRisposteByIdPersona(oQuest.rispostaQuest.idPersona, oQuest.rispostaQuest.idQuestionario) > 0 AndAlso Not isAutoval AndAlso Not isUtenteAnonimo Then
                    Return "0"
                Else
                    RispostaQuestionario_Insert(oQuest, CloseAnswer)
                    retVal = "1"
                End If
            End If
        Catch ex As Exception
            retVal = "0"
        End Try
        Return retVal
    End Function
    Public Function RispostaQuestionario_Insert(ByRef oQuest As Questionario, ByVal CloseAnswer As Boolean) As Integer
        oQuest.rispostaQuest.oStatistica = calcoloPunteggioAutovalutazione(oQuest)

        '?? Ci proviamoci...
        'If oQuest.rispostaQuest.oStatistica.isFinito AndAlso CloseAnswer Then
        '    oQuest.rispostaQuest.dataFine = DateTime.Now()
        'End If

        If CloseAnswer Then
            oQuest.rispostaQuest.dataFine = DateTime.Now()
        End If


        If isAnonymousCompiler Then
            DALRisposte.RispostaQuestionario_Insert(oQuest.rispostaQuest, idUtenteAnonimo)
        ElseIf Invito.ID > 0 Then
            DALRisposte.RispostaQuestionario_Insert(oQuest.rispostaQuest, Me.Invito.PersonaID)
        Else
            DALRisposte.RispostaQuestionario_Insert(oQuest.rispostaQuest, oQuest.rispostaQuest.idPersona)
        End If
    End Function
    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            If _SmartTagsAvailable Is Nothing Then
                _SmartTagsAvailable = ManagerConfiguration.GetSmartTags(_ApplicationUrlBase)
            End If
            Return _SmartTagsAvailable
        End Get
    End Property
    Public Function getRisposte(
                               ByVal DLPagine As DataList,
                               ByRef isValida As Boolean,
                               ByRef RisposteObbligatorieSaltate As Integer,
                               Optional ByVal isTempoScaduto As Boolean = False) _
                               As RispostaQuestionario

        RisposteObbligatorieSaltate = 0


        Dim IdUtenteQuestionario As Integer = Me.UtenteCorrente.ID

        If Not IsNothing(UtenteQuestionario) Then
            IdUtenteQuestionario = UtenteQuestionario.ID
        End If

        'isTempoScaduto = true impedisce il salvataggio delle risposte multiple se sono state date piu' risposte dell'ammesso a una singola domanda
        Dim numeroTroppeRisposte As Int16 = 0
        Dim oRispostaQ As New RispostaQuestionario
        If Not Me.QuestionarioCorrente.rispostaQuest Is Nothing Then
            oRispostaQ = Me.QuestionarioCorrente.rispostaQuest
        Else
            'oRispostaQ.findRispostaByIDPersona(Me.QuestionarioCorrente.risposteQuestionario, Me.UtenteCorrente.Id)
            ' se è la prima risposta della persona creo l'istanza
            oRispostaQ = New COL_Questionario.RispostaQuestionario
            oRispostaQ.idPersona = IdUtenteQuestionario 'Me.UtenteCorrente.ID
            oRispostaQ.idQuestionario = Me.QuestionarioCorrente.id
            oRispostaQ.dataInizio = Now()
            oRispostaQ.indirizzoIPStart = OLDpageUtility.ProxyIPadress() & " / " & OLDpageUtility.ClientIPadress
            oRispostaQ.dataFine = Date.MinValue
            oRispostaQ.ultimaRisposta = 0
            oRispostaQ.dataModifica = Now()
            oRispostaQ.indirizzoIPEdit = OLDpageUtility.ProxyIPadress() & " / " & OLDpageUtility.ClientIPadress
        End If
        For Each itemP As DataListItem In DLPagine.Items
            Dim dlDomande As New DataList
            dlDomande = DLPagine.Controls(itemP.ItemIndex).FindControl("DLDomande")
            For Each itemD As DataListItem In dlDomande.Items 'deve eliminare tutte le risposte della pagina prima di caricare le nuove
                Dim idDomanda As String = dlDomande.DataKeys.Item(itemD.ItemIndex).ToString
                Dim oDomanda As New Domanda
                oDomanda = oDomanda.findDomandaBYID(Me.QuestionarioCorrente.domande, idDomanda)
                Dim oRisp As New RispostaDomanda
                Dim oRisposte As New List(Of RispostaDomanda)
                oRisposte = oRispostaQ.findRisposteByIDDomanda(Me.QuestionarioCorrente.rispostaQuest.risposteDomande, idDomanda)
                For Each oRisp In oRisposte
                    oRispostaQ.risposteDomande.Remove(oRisp)
                    oDomanda.risposteDomanda.Remove(oRisp)
                Next
            Next
            For Each itemD As DataListItem In dlDomande.Items
                Dim idQuestion As String = dlDomande.DataKeys.Item(itemD.ItemIndex).ToString
                Dim oDomanda As New Domanda
                Dim isRisposta As Boolean = False
                oDomanda = oDomanda.findDomandaBYID(Me.QuestionarioCorrente.domande, idQuestion)

                Dim isMandatory As Boolean = oDomanda.isObbligatoria

                Dim hasAnswer As Boolean = False    ''ToDo: controllare se ho risposta          <--------------------

                Select Case oDomanda.tipo
                    Case Domanda.TipoDomanda.Multipla And Not oDomanda.isMultipla
                        Dim tabella As New Table
                        tabella = FindControlRecursive(itemD, "TBLDomandaSingola_" + itemD.ItemIndex.ToString())
                        Dim iOpzione As Integer = 0
                        If Not tabella Is Nothing Then
                            For Each row As TableRow In tabella.Rows
                                Dim opzioneSelezionata As Boolean
                                Dim testoIsAltro As String = String.Empty
                                For Each cell As TableCell In row.Cells
                                    For Each ctrl As Control In cell.Controls
                                        If ctrl.GetType() Is GetType(RadioButton) Then
                                            Dim rbOpzione As New RadioButton
                                            rbOpzione = DirectCast(ctrl, RadioButton)
                                            opzioneSelezionata = rbOpzione.Checked
                                        ElseIf ctrl.GetType() Is GetType(HtmlGenericControl) And opzioneSelezionata Then
                                            testoIsAltro = String.Empty
                                            For Each c As Control In ctrl.Controls
                                                If c.GetType() Is GetType(TextBox) Then
                                                    Dim txbOpzione As New TextBox
                                                    txbOpzione = DirectCast(c, TextBox)
                                                    testoIsAltro = txbOpzione.Text
                                                End If
                                            Next
                                        Else
                                            testoIsAltro = String.Empty
                                        End If
                                    Next
                                Next
                                If opzioneSelezionata Then
                                    Dim oRisp As New RispostaDomanda
                                    oRisp = setRisposta(oDomanda.domandaMultiplaOpzioni(iOpzione).id, oDomanda.domandaMultiplaOpzioni(iOpzione).numero, 0, testoIsAltro, Domanda.TipoDomanda.Multipla)
                                    oRisp.idDomanda = idQuestion
                                    oRisp.tipo = oDomanda.tipo
                                    oDomanda.risposteDomanda.Add(oRisp)
                                    oRispostaQ.risposteDomande.Add(oRisp)

                                    hasAnswer = True
                                End If
                                iOpzione = iOpzione + 1
                            Next
                        End If
                    Case Domanda.TipoDomanda.Multipla And oDomanda.isMultipla
                        Dim tabella As New Table
                        tabella = FindControlRecursive(itemD, "TBLDomandaMultipla_" + itemD.ItemIndex.ToString())
                        'controllo se la domanda ha troppe risposte selezionate
                        Dim quanteChecked As Int32 = 0
                        If Not tabella Is Nothing Then
                            For Each riga As TableRow In tabella.Rows
                                For Each controllo As Control In riga.Cells(0).Controls
                                    If controllo.GetType() Is GetType(CheckBox) Then
                                        Dim CBopzione As New CheckBox
                                        CBopzione = DirectCast(controllo, CheckBox)
                                        If CBopzione.Checked Then
                                            quanteChecked += 1
                                        End If
                                    End If
                                Next
                            Next

                            If quanteChecked > oDomanda.numeroMaxRisposte And oDomanda.numeroMaxRisposte > 0 Then
                                oDomanda.isValida = False
                                numeroTroppeRisposte = numeroTroppeRisposte + 1
                            Else

                                If (quanteChecked > 0) Then
                                    hasAnswer = True
                                End If

                                oDomanda.isValida = True
                            End If



                            If Not (isTempoScaduto And Not oDomanda.isValida) Then
                                Dim iOpzione As Integer = 0
                                For Each row As TableRow In tabella.Rows
                                    Dim opzioneSelezionata As Boolean
                                    Dim testoIsAltro As String = String.Empty
                                    For Each cell As TableCell In row.Cells
                                        For Each ctrl As Control In cell.Controls
                                            If ctrl.GetType() Is GetType(Label) Then 'se e' la riga della stringa di errore non fare il set
                                                Dim LBerr As New Label
                                                LBerr = DirectCast(ctrl, Label)
                                                If Not LBerr.ID Is Nothing Then
                                                    If LBerr.ID.TrimEnd("_", itemD.ItemIndex.ToString) = "LBtroppeRisposte" Then
                                                        opzioneSelezionata = False
                                                    End If
                                                End If
                                            ElseIf ctrl.GetType() Is GetType(CheckBox) Then
                                                Dim cbOpzione As New CheckBox
                                                cbOpzione = DirectCast(ctrl, CheckBox)
                                                opzioneSelezionata = cbOpzione.Checked
                                            ElseIf ctrl.GetType() Is GetType(HtmlGenericControl) And opzioneSelezionata Then
                                                testoIsAltro = String.Empty
                                                For Each c As Control In ctrl.Controls
                                                    If c.GetType() Is GetType(TextBox) Then
                                                        Dim txbOpzione As New TextBox
                                                        txbOpzione = DirectCast(c, TextBox)
                                                        testoIsAltro = txbOpzione.Text
                                                    End If
                                                Next
                                            Else
                                                testoIsAltro = String.Empty
                                            End If
                                        Next
                                    Next
                                    If opzioneSelezionata Then
                                        Dim oRisp As New RispostaDomanda
                                        oRisp = setRisposta(oDomanda.domandaMultiplaOpzioni(iOpzione).id, oDomanda.domandaMultiplaOpzioni(iOpzione).numero, 0, testoIsAltro, Domanda.TipoDomanda.Multipla)
                                        oRisp.idDomanda = idQuestion
                                        oRisp.tipo = oDomanda.tipo
                                        oDomanda.risposteDomanda.Add(oRisp)
                                        oRispostaQ.risposteDomande.Add(oRisp)
                                    End If
                                    iOpzione = iOpzione + 1
                                Next
                            End If
                        End If
                    Case Domanda.TipoDomanda.DropDown
                        Dim dlOpzioni As New DropDownList
                        dlOpzioni = FindControlRecursive(itemD, "DDLOpzioni")
                        If Not dlOpzioni Is Nothing Then
                            If Not dlOpzioni.SelectedItem.Value Is Nothing Then
                                Dim oRisp As New RispostaDomanda
                                oRisp = setRisposta(oDomanda.domandaDropDown.id, dlOpzioni.SelectedItem.Value, 0, dlOpzioni.SelectedItem.Text, Domanda.TipoDomanda.DropDown)
                                oRisp.idDomanda = idQuestion
                                oRisp.tipo = oDomanda.tipo
                                oDomanda.risposteDomanda.Add(oRisp)
                                oRispostaQ.risposteDomande.Add(oRisp)

                                hasAnswer = True
                            End If
                        End If
                    Case Domanda.TipoDomanda.Rating

                        Dim answerRows As Integer = 0
                        Dim answeredRow As Integer = 0
                        Dim tabella As New Table
                        tabella = FindControlRecursive(itemD, "TBLRadiobutton_" + itemD.ItemIndex.ToString())
                        If Not tabella Is Nothing Then
                            Dim iRiga As Integer = 0
                            For Each row As TableRow In tabella.Rows

                                answerRows += 1

                                Dim iCella As Integer = 0
                                If iRiga > 0 Then
                                    Dim testoIsAltro As String = String.Empty
                                    For Each ctrl As Control In row.Cells(0).Controls
                                        If ctrl.GetType() Is GetType(TextBox) Then
                                            Dim txbOpzione As New TextBox
                                            txbOpzione = DirectCast(ctrl, TextBox)
                                            testoIsAltro = txbOpzione.Text
                                        End If
                                    Next
                                    For Each cell As TableCell In row.Cells
                                        'If iCella > 0 Then
                                        For Each ctrl As Control In cell.Controls
                                            If ctrl.GetType() Is GetType(RadioButton) Then
                                                Dim dlOpzioni As New RadioButton
                                                dlOpzioni = DirectCast(ctrl, RadioButton)
                                                If Not dlOpzioni Is Nothing Then
                                                    If dlOpzioni.Checked Then
                                                        Dim oRisp As New RispostaDomanda
                                                        oRisp = setRisposta(oDomanda.domandaRating.opzioniRating(iRiga - 1).id, oDomanda.domandaRating.opzioniRating(iRiga - 1).numero, iCella, testoIsAltro, Domanda.TipoDomanda.Rating)
                                                        oRisp.idDomanda = idQuestion
                                                        oRisp.tipo = oDomanda.tipo
                                                        oDomanda.risposteDomanda.Add(oRisp)
                                                        oRispostaQ.risposteDomande.Add(oRisp)

                                                        answeredRow += 1
                                                    End If
                                                End If
                                            End If
                                        Next
                                        'End If
                                        iCella = iCella + 1
                                    Next
                                End If
                                iRiga = iRiga + 1
                            Next
                        End If

                        If answeredRow >= (answerRows - 1) Then 'conta l'intestazione...
                            hasAnswer = True
                        End If
                    Case Domanda.TipoDomanda.RatingStars

                        Dim answerRows As Integer = 0
                        Dim answeredRow As Integer = 0

                        Dim tabella As New Table
                        tabella = FindControlRecursive(itemD, "TBLRadiobutton_" + itemD.ItemIndex.ToString())


                        If Not tabella Is Nothing Then
                            Dim iRiga As Integer = 0
                            For Each row As TableRow In tabella.Rows

                                answerRows += 1

                                Dim iCella As Integer = 0
                                'If iRiga > 0 Then
                                Dim testoIsAltro As String = String.Empty
                                For Each ctrl As Control In row.Cells(0).Controls
                                    If ctrl.GetType() Is GetType(TextBox) Then
                                        Dim txbOpzione As New TextBox
                                        txbOpzione = DirectCast(ctrl, TextBox)
                                        testoIsAltro = txbOpzione.Text
                                    End If
                                Next
                                For Each cell As TableCell In row.Cells
                                    'If iCella > 0 Then
                                    'Dim rdStars As New Telerik.Web.UI.RadRating

                                    For Each ctrl As Control In cell.Controls

                                        Dim container As Uc_StarBind = TryCast(ctrl, Uc_StarBind)

                                        If Not IsNothing(container) Then
                                            'If ctrl.GetType() Is GetType(UserControl) Then
                                            'If ctrl.GetType() Is GetType(Uc_StarBind) Then 
                                            '                  Is GetType(Telerik.Web.UI.RadRating) Then


                                            Dim rdStars As Telerik.Web.UI.RadRating = container.RadRating 'DirectCast(ctrl, Telerik.Web.UI.RadRating)
                                            Dim value As Integer = rdStars.Value

                                            Dim oRisp As New RispostaDomanda
                                            'iCella
                                            oRisp = setRisposta(oDomanda.domandaRating.opzioniRating(iRiga).id, oDomanda.domandaRating.opzioniRating(iRiga).numero, value, testoIsAltro, Domanda.TipoDomanda.RatingStars)
                                            oRisp.idDomanda = idQuestion
                                            oRisp.tipo = oDomanda.tipo
                                            oDomanda.risposteDomanda.Add(oRisp)
                                            oRispostaQ.risposteDomande.Add(oRisp)

                                            answeredRow += 1
                                            '        Dim dlOpzioni As New RadioButton
                                            '        dlOpzioni = DirectCast(ctrl, RadioButton)
                                            '        If Not dlOpzioni Is Nothing Then
                                            '            If dlOpzioni.Checked Then
                                            '                Dim oRisp As New RispostaDomanda
                                            '                oRisp = setRisposta(oDomanda.domandaRating.opzioniRating(iRiga - 1).id, oDomanda.domandaRating.opzioniRating(iRiga - 1).numero, iCella, testoIsAltro, Domanda.TipoDomanda.Rating)
                                            '                oRisp.idDomanda = idQuestion
                                            '                oRisp.tipo = oDomanda.tipo
                                            '                oDomanda.risposteDomanda.Add(oRisp)
                                            '                oRispostaQ.risposteDomande.Add(oRisp)
                                            '            End If
                                            '        End If
                                        End If
                                    Next
                                    'End If
                                    iCella = iCella + 1
                                Next
                                'End If
                                iRiga = iRiga + 1
                            Next
                        End If

                        If answeredRow >= (answerRows - 1) Then  'conta l'intestazione!
                            hasAnswer = True
                        End If

                    Case Domanda.TipoDomanda.Meeting
                        Dim tabella As New Table
                        tabella = FindControlRecursive(itemD, "TBLRadiobutton_" + itemD.ItemIndex.ToString())
                        If Not tabella Is Nothing Then
                            Dim iRiga As Integer = 0
                            Dim isUpdated As Boolean = False
                            For Each row As TableRow In tabella.Rows
                                Dim iCella As Integer = 0
                                If iRiga > 0 Then
                                    For Each cell As TableCell In row.Cells
                                        'If iCella > 0 Then
                                        For Each ctrl As Control In cell.Controls
                                            If ctrl.GetType() Is GetType(CheckBox) Then
                                                Dim dlOpzioni As New CheckBox
                                                dlOpzioni = DirectCast(ctrl, CheckBox)
                                                If Not dlOpzioni Is Nothing Then
                                                    If dlOpzioni.Checked Then
                                                        Dim oRisp As New RispostaDomanda
                                                        oRisp = setRisposta(oDomanda.domandaRating.opzioniRating(iRiga - 1).id, oDomanda.domandaRating.opzioniRating(iRiga - 1).numero, iCella, "", Domanda.TipoDomanda.Meeting)
                                                        oRisp.idDomanda = idQuestion
                                                        oRisp.tipo = oDomanda.tipo
                                                        oDomanda.risposteDomanda.Add(oRisp)
                                                        oRispostaQ.risposteDomande.Add(oRisp)
                                                        isUpdated = True

                                                        hasAnswer = True
                                                    End If
                                                End If
                                            End If
                                        Next
                                        'End If
                                        iCella = iCella + 1
                                    Next
                                End If
                                iRiga = iRiga + 1
                            Next
                        End If
                    Case Domanda.TipoDomanda.TestoLibero
                        Dim tabella As New Table
                        tabella = FindControlRecursive(itemD, "TBLTestoLibero_" + itemD.ItemIndex.ToString())
                        If Not tabella Is Nothing Then
                            Dim iOpzione As Integer = 0
                            For Each row As TableRow In tabella.Rows
                                For Each ctrl As Control In row.Cells(0).Controls
                                    If ctrl.GetType() Is GetType(TextBox) Then
                                        Dim txOpzioni As New TextBox
                                        txOpzioni = DirectCast(ctrl, TextBox)
                                        If Not txOpzioni Is Nothing Then
                                            If Not txOpzioni.Text = String.Empty Then
                                                Dim oRisp As New RispostaDomanda
                                                oRisp = setRisposta(oDomanda.opzioniTestoLibero(iOpzione).id, oDomanda.opzioniTestoLibero(iOpzione).numero, 0, txOpzioni.Text, Domanda.TipoDomanda.TestoLibero)
                                                oRisp.idDomanda = idQuestion
                                                oRisp.tipo = oDomanda.tipo
                                                oDomanda.risposteDomanda.Add(oRisp)
                                                oRispostaQ.risposteDomande.Add(oRisp)

                                                hasAnswer = True
                                            End If
                                        End If
                                        iOpzione = iOpzione + 1
                                    End If
                                Next
                            Next
                        End If
                    Case Domanda.TipoDomanda.Numerica
                        Dim tabella As New Table
                        tabella = FindControlRecursive(itemD, "TBLNumerica_" + itemD.ItemIndex.ToString())
                        If Not tabella Is Nothing Then
                            Dim iOpzione As Integer = 0
                            For Each row As TableRow In tabella.Rows
                                For Each ctrl As Control In row.Cells(1).Controls
                                    If ctrl.GetType() Is GetType(TextBox) Then
                                        Dim txOpzioni As New TextBox
                                        txOpzioni = DirectCast(ctrl, TextBox)
                                        If Not txOpzioni Is Nothing Then
                                            Dim oRisp As New RispostaDomanda
                                            If Not txOpzioni.Text = String.Empty Then
                                                oRisp = setRisposta(oDomanda.opzioniNumerica(iOpzione).id, oDomanda.opzioniNumerica(iOpzione).numero, txOpzioni.Text, txOpzioni.Text, Domanda.TipoDomanda.Numerica)
                                                isRisposta = True

                                                hasAnswer = True
                                            Else
                                                oRisp = setRisposta(oDomanda.opzioniNumerica(iOpzione).id, oDomanda.opzioniNumerica(iOpzione).numero, Integer.MinValue, Integer.MinValue, Domanda.TipoDomanda.Numerica)
                                            End If
                                            oRisp.idDomanda = idQuestion
                                            oRisp.tipo = oDomanda.tipo
                                            oDomanda.risposteDomanda.Add(oRisp)
                                            oRispostaQ.risposteDomande.Add(oRisp)
                                        End If
                                    End If
                                Next
                                iOpzione = iOpzione + 1
                            Next
                        End If
                End Select

                If isMandatory AndAlso Not hasAnswer Then
                    RisposteObbligatorieSaltate += 1
                End If


            Next
        Next
        If numeroTroppeRisposte > 0 Then
            isValida = False
        Else
            isValida = True
        End If
        Return oRispostaQ
    End Function
    Public Shared Function checkMandatoryAnswers(ByRef QuestionarioCorrente As Questionario) As Integer
        Dim pages As List(Of Integer) = GetPageNumberOfSkippedMandatoryQuestion(QuestionarioCorrente)
        If IsNothing(pages) OrElse Not pages.Any() Then
            Return -10
        Else
            Return pages.FirstOrDefault()
        End If
    End Function
    Public Shared Function GetPageNumberOfSkippedMandatoryQuestion(ByRef QuestionarioCorrente As Questionario) As List(Of Integer)
        Dim mQuestions As List(Of Domanda) = (From oDom In QuestionarioCorrente.domande Where oDom.isObbligatoria Select oDom).ToList()
        Dim rQuestions As List(Of Integer) = (From rispId In QuestionarioCorrente.rispostaQuest.risposteDomande Select rispId.idDomanda).ToList()
        Dim sQuestions As List(Of Integer) = (From d In mQuestions Where Not rQuestions.Contains(d.id) Select d.id).ToList()


        Return QuestionarioCorrente.pagine.Where(Function(p) p.domande.Where(Function(d) sQuestions.Contains(d.id)).Any).Select(Function(p) p.numeroPagina).ToList.Distinct.ToList()
    End Function
    Public Shared Function trovaPosizione(ByVal item As RispostaQuestionario.PosizioneDomanda, ByVal oPosizioneDomanda As RispostaQuestionario.PosizioneDomanda) As Boolean
        Return (item.numero = oPosizioneDomanda.numero And item.numeroPagina = oPosizioneDomanda.numeroPagina)
    End Function
    Protected Function setRisposta(ByVal idDomanda As String, ByVal numeroOpzione As String, ByVal valoreOpzione As String, ByVal testoOpzione As String, ByVal tipo As String) As RispostaDomanda
        Dim oRispostaDomanda As New RispostaDomanda
        oRispostaDomanda.idDomandaOpzione = idDomanda
        oRispostaDomanda.numeroOpzione = numeroOpzione
        oRispostaDomanda.testoOpzione = testoOpzione
        oRispostaDomanda.tipo = tipo
        oRispostaDomanda.valore = valoreOpzione
        Return oRispostaDomanda
    End Function
    Private Sub setRispostaNumerica(ByRef oDomanda As Domanda, ByRef itemD As DataListItem, ByRef oRisposta As RispostaQuestionario, ByRef isEditabile As Boolean)
        Dim tabella As New Table
        tabella = FindControlRecursive(itemD, "TBLNumerica_" + itemD.ItemIndex.ToString())
        Dim iOpzione As Integer = 0
        For Each row As TableRow In tabella.Rows
            Dim ris As New RispostaDomanda
            ris = oRisposta.findRispostaByNumeroOpzione(oDomanda.risposteDomanda, oDomanda.opzioniNumerica(iOpzione).numero)
            For Each ctrl As Control In row.Cells(1).Controls
                If ctrl.GetType() Is GetType(TextBox) Then
                    Dim txOpzioni As New TextBox
                    txOpzioni = DirectCast(ctrl, TextBox)
                    If Not txOpzioni Is Nothing Then
                        If Not ris Is Nothing And oDomanda.risposteDomanda.Count > iOpzione Then
                            If ris.valore = Integer.MinValue Then
                                txOpzioni.Text = String.Empty
                            Else
                                txOpzioni.Text = ris.valore
                            End If
                        End If
                    End If
                    txOpzioni.Enabled = isEditabile
                End If
            Next
            iOpzione = iOpzione + 1
        Next
    End Sub

    Public Function setRispostePagina(ByVal DLPagine As DataList, ByRef oDomandeList As List(Of Domanda), Optional ByRef isValida As Boolean = True, Optional ByVal isEditabile As Boolean = True) As DataList
        If isEditabile Then
            If Me.QuestionarioCorrente.durata > 0 AndAlso (DateDiff("s", Me.QuestionarioCorrente.rispostaQuest.dataInizio, Now) > (Me.QuestionarioCorrente.durata * 60 + RootObject.maxOvertimeSalvataggio)) Then
                'doppio controllo necessario perche' la data di fine non viene impostata quanto viene semplicemente salvato, 
                isEditabile = False
            Else
                isEditabile = (Me.QuestionarioCorrente.editaRisposta Or Me.QuestionarioCorrente.isPrimaRisposta)
            End If
        End If
        Try
            If Me.QuestionarioCorrente.visualizzaCorrezione And Not RootObject.setNullDate(Me.QuestionarioCorrente.rispostaQuest.dataFine) Is System.DBNull.Value And Not Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
                DLPagine = setRispostePaginaCorrette(DLPagine, Me.QuestionarioCorrente.domande)
            Else
                For Each itemP As DataListItem In DLPagine.Items

                    Dim dlDomande As New DataList
                    dlDomande = DLPagine.Controls(itemP.ItemIndex).FindControl("DLDomande")
                    For Each itemD As DataListItem In dlDomande.Items
                        Dim idDomanda As String = dlDomande.DataKeys.Item(itemD.ItemIndex).ToString
                        Dim oDomanda As New Domanda
                        oDomanda = oDomanda.findDomandaBYID(oDomandeList, idDomanda)
                        Dim oRisposta As New RispostaQuestionario
                        If oDomanda.risposteDomanda.Count > 0 Then
                            Select Case oDomanda.tipo
                                Case Domanda.TipoDomanda.Multipla And Not oDomanda.isMultipla
                                    Dim tabella As New Table
                                    tabella = FindControlRecursive(itemD, "TBLDomandaSingola_" + itemD.ItemIndex.ToString())
                                    Dim iOpzione As Integer = 0
                                    For Each row As TableRow In tabella.Rows
                                        Dim opzioneSelezionata As Boolean
                                        Dim testoIsAltro As String
                                        Dim ris As New RispostaDomanda
                                        ris = oRisposta.findRispostaByNumeroOpzione(oDomanda.risposteDomanda, oDomanda.domandaMultiplaOpzioni(iOpzione).numero)
                                        For Each cell As TableCell In row.Cells
                                            For Each ctrl As Control In cell.Controls
                                                If ctrl.GetType() Is GetType(RadioButton) Then
                                                    Dim rbOpzione As New RadioButton
                                                    rbOpzione = DirectCast(ctrl, RadioButton)
                                                    If Not ris Is Nothing Then
                                                        If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                            rbOpzione.Checked = True
                                                        End If
                                                    End If
                                                    rbOpzione.Enabled = isEditabile

                                                ElseIf ctrl.GetType() Is GetType(HtmlGenericControl) Then

                                                    For Each c As Control In ctrl.Controls
                                                        If c.GetType() Is GetType(TextBox) Then
                                                            Dim txbOpzione As New TextBox
                                                            txbOpzione = DirectCast(c, TextBox)
                                                            If Not ris Is Nothing Then
                                                                If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                                    txbOpzione.Text = ris.testoOpzione
                                                                End If
                                                            End If
                                                            txbOpzione.Enabled = isEditabile
                                                        End If
                                                    Next
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
                                        ris = oRisposta.findRispostaByNumeroOpzione(oDomanda.risposteDomanda, oDomanda.domandaMultiplaOpzioni(iOpzione).numero)
                                        For Each cell As TableCell In row.Cells
                                            For Each ctrl As Control In cell.Controls
                                                If ctrl.GetType() Is GetType(CheckBox) Then
                                                    Dim rbOpzione As New CheckBox
                                                    rbOpzione = DirectCast(ctrl, CheckBox)
                                                    If Not ris Is Nothing Then
                                                        If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                            rbOpzione.Checked = True
                                                        End If
                                                    End If
                                                    rbOpzione.Enabled = isEditabile Or Not isValida
                                                ElseIf ctrl.GetType() Is GetType(HtmlGenericControl) Then
                                                    For Each c As Control In ctrl.Controls
                                                        If c.GetType() Is GetType(TextBox) Then
                                                            Dim txbOpzione As New TextBox
                                                            txbOpzione = DirectCast(c, TextBox)
                                                            If Not ris Is Nothing Then
                                                                If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                                    txbOpzione.Text = ris.testoOpzione
                                                                End If
                                                            End If
                                                            txbOpzione.Enabled = isEditabile
                                                        End If
                                                    Next
                                                End If
                                            Next
                                        Next
                                        iOpzione = iOpzione + 1
                                    Next

                                Case Domanda.TipoDomanda.DropDown
                                    Dim dlOpzioni As New DropDownList
                                    dlOpzioni = FindControlRecursive(itemD, "DDLOpzioni")
                                    Dim iOpzione As Integer = 0
                                    Dim ris As New RispostaDomanda

                                    For Each DDItem As ListItem In dlOpzioni.Items
                                        ris = oRisposta.findRispostaByNumeroOpzione(oDomanda.risposteDomanda, DDItem.Value)
                                        If Not ris Is Nothing Then
                                            Exit For
                                        End If
                                    Next

                                    If Not ris Is Nothing Then
                                        dlOpzioni.SelectedValue = ris.numeroOpzione
                                    End If

                                    dlOpzioni.Enabled = isEditabile

                                Case Domanda.TipoDomanda.Rating

                                    Dim tabella As New Table
                                    tabella = FindControlRecursive(itemD, "TBLRadiobutton_" + itemD.ItemIndex.ToString())
                                    Dim indiceRiga As Integer = 0
                                    For Each row As TableRow In tabella.Rows
                                        Dim iCella As Integer = 0
                                        If indiceRiga > 0 Then
                                            Dim ris As New RispostaDomanda
                                            ris = oRisposta.findRispostaByNumeroOpzione(oDomanda.risposteDomanda, oDomanda.domandaRating.opzioniRating(indiceRiga - 1).numero)

                                            For Each cell As TableCell In row.Cells
                                                For Each ctrl As Control In cell.Controls
                                                    If ctrl.GetType() Is GetType(RadioButton) Then
                                                        Dim dlOpzioni As New RadioButton
                                                        dlOpzioni = DirectCast(ctrl, RadioButton)
                                                        If Not dlOpzioni Is Nothing Then
                                                            If Not ris Is Nothing Then
                                                                If iCella = ris.valore And ris.numeroOpzione = oDomanda.domandaRating.opzioniRating(indiceRiga - 1).numero Then
                                                                    dlOpzioni.Checked = True
                                                                End If
                                                            End If
                                                        End If
                                                        dlOpzioni.Enabled = isEditabile
                                                    ElseIf ctrl.GetType() Is GetType(TextBox) Then
                                                        Dim dlOpzioni As New TextBox
                                                        dlOpzioni = DirectCast(ctrl, TextBox)
                                                        If Not dlOpzioni Is Nothing Then
                                                            If Not ris Is Nothing Then
                                                                If ris.numeroOpzione = oDomanda.domandaRating.opzioniRating(indiceRiga - 1).numero Then
                                                                    dlOpzioni.Text = ris.testoOpzione
                                                                End If
                                                            End If
                                                        End If
                                                        dlOpzioni.Enabled = isEditabile
                                                    End If

                                                Next
                                                iCella = iCella + 1
                                            Next
                                        End If
                                        indiceRiga = indiceRiga + 1
                                    Next
                                Case Domanda.TipoDomanda.RatingStars
                                    Dim tabella As New Table
                                    tabella = FindControlRecursive(itemD, "TBLRadiobutton_" + itemD.ItemIndex.ToString())
                                    Dim indiceRiga As Integer = 0
                                    For Each row As TableRow In tabella.Rows
                                        Dim iCella As Integer = 0
                                        'If indiceRiga > 0 Then
                                        Dim ris As New RispostaDomanda
                                        If oDomanda.risposteDomanda.Count > 0 Then
                                            ris = oRisposta.findRispostaByNumeroOpzione(oDomanda.risposteDomanda, oDomanda.domandaRating.opzioniRating(indiceRiga).numero)
                                        End If

                                        For Each cell As TableCell In row.Cells
                                            For Each ctrl As Control In cell.Controls
                                                'Dim rdStars As New Telerik.Web.UI.RadRating
                                                If ctrl.GetType() Is GetType(Telerik.Web.UI.RadRating) Then
                                                    Try
                                                        Dim rdStars As Telerik.Web.UI.RadRating = DirectCast(ctrl, Telerik.Web.UI.RadRating)
                                                        rdStars.Value = CInt(ris.valore)
                                                        rdStars.Enabled = False
                                                    Catch ex As Exception

                                                    End Try

                                                    'If ctrl.GetType() Is GetType(HtmlGenericControl) Then
                                                    '    For Each dControl As Control In ctrl.Controls
                                                    '        If dControl.GetType() Is GetType(RadioButton) Then
                                                    '            Dim dlOpzioni As New RadioButton
                                                    '            dlOpzioni = DirectCast(dControl, RadioButton)
                                                    '            If Not dlOpzioni Is Nothing Then
                                                    '                If Not ris Is Nothing Then
                                                    '                    If iCella = ris.valore And ris.numeroOpzione = oDomanda.domandaRating.opzioniRating(indiceRiga).numero Then
                                                    '                        dlOpzioni.Checked = True
                                                    '                    End If
                                                    '                End If
                                                    '            End If
                                                    '            dlOpzioni.Enabled = False
                                                    '        ElseIf dControl.GetType() Is GetType(TextBox) Then
                                                    '            Dim dlOpzioni As New TextBox
                                                    '            dlOpzioni = DirectCast(dControl, TextBox)
                                                    '            If Not dlOpzioni Is Nothing Then
                                                    '                If Not ris Is Nothing Then
                                                    '                    If ris.numeroOpzione = oDomanda.domandaRating.opzioniRating(indiceRiga - 1).numero Then
                                                    '                        dlOpzioni.Text = ris.testoOpzione
                                                    '                    End If
                                                    '                End If
                                                    '            End If
                                                    '            dlOpzioni.Enabled = False
                                                    '        End If
                                                    '    Next

                                                    'ElseIf ctrl.GetType() Is GetType(RadioButton) Then
                                                    '    Dim dlOpzioni As New RadioButton
                                                    '    dlOpzioni = DirectCast(ctrl, RadioButton)
                                                    '    If Not dlOpzioni Is Nothing Then
                                                    '        If Not ris Is Nothing Then
                                                    '            If iCella = ris.valore And ris.numeroOpzione = oDomanda.domandaRating.opzioniRating(indiceRiga - 1).numero Then
                                                    '                dlOpzioni.Checked = True
                                                    '            End If
                                                    '        End If
                                                    '    End If
                                                    '    dlOpzioni.Enabled = False
                                                ElseIf ctrl.GetType() Is GetType(TextBox) Then
                                                    Dim dlOpzioni As New TextBox
                                                    dlOpzioni = DirectCast(ctrl, TextBox)
                                                    If Not dlOpzioni Is Nothing Then
                                                        If Not ris Is Nothing Then
                                                            If ris.numeroOpzione = oDomanda.domandaRating.opzioniRating(indiceRiga - 1).numero Then
                                                                dlOpzioni.Text = ris.testoOpzione
                                                            End If
                                                        End If
                                                    End If
                                                    dlOpzioni.Enabled = False
                                                End If
                                            Next
                                            iCella = iCella + 1
                                        Next
                                        'End If
                                        indiceRiga = indiceRiga + 1
                                    Next
                                Case Domanda.TipoDomanda.Meeting

                                    Dim tabella As New Table
                                    tabella = FindControlRecursive(itemD, "TBLRadiobutton_" + itemD.ItemIndex.ToString())
                                    Dim indiceRiga As Integer = 0
                                    For Each row As TableRow In tabella.Rows
                                        Dim iCella As Integer = 0
                                        If indiceRiga > 0 Then

                                            For Each cell As TableCell In row.Cells
                                                For Each ctrl As Control In cell.Controls
                                                    If ctrl.GetType() Is GetType(CheckBox) Then
                                                        Dim dlOpzioni As New CheckBox
                                                        dlOpzioni = DirectCast(ctrl, CheckBox)
                                                        If Not dlOpzioni Is Nothing Then
                                                            Dim check = (From ri In oDomanda.risposteDomanda Where ri.valore = iCella And ri.idDomandaOpzione = oDomanda.domandaRating.opzioniRating(indiceRiga - 1).id Select ri.id).ToList.Count
                                                            dlOpzioni.Checked = check
                                                        End If
                                                        dlOpzioni.Enabled = isEditabile
                                                    End If
                                                Next
                                                iCella = iCella + 1
                                            Next
                                        End If
                                        indiceRiga = indiceRiga + 1
                                    Next
                                    If Me.QuestionarioCorrente.visualizzaRisposta Then
                                        Dim oDomandaTemp As New Domanda
                                        oDomandaTemp.id = oDomanda.id
                                        oDomandaTemp.tipo = Domanda.TipoDomanda.Meeting
                                        oDomandaTemp.domandaRating = oDomanda.domandaRating
                                        Dim oGestioneRisposte As New GestioneRisposte
                                        oDomandaTemp.risposteDomanda = DALRisposte.readRispostaRatingByIdDomanda(oDomanda.id)
                                        itemD.Controls.Add(oGestioneRisposte.loadTabellaRisposte(oDomandaTemp))
                                    End If

                                Case Domanda.TipoDomanda.TestoLibero
                                    Dim tabella As New Table
                                    tabella = FindControlRecursive(itemD, "TBLTestoLibero_" + itemD.ItemIndex.ToString())
                                    Dim iOpzione As Integer = 0
                                    For Each row As TableRow In tabella.Rows
                                        If row.Cells.Count > 0 AndAlso row.Cells(0).HasControls Then
                                            For Each ctrl As Control In row.Cells(0).Controls
                                                If ctrl.GetType() Is GetType(TextBox) Then
                                                    Dim ris As New RispostaDomanda
                                                    'If oDomanda.risposteDomanda.Count > iOpzione Then
                                                    'ris = oRisposta.findRispostaByIDDomandaOpzione(oDomanda.risposteDomanda, oDomanda.opzioniTestoLibero(iOpzione).numero)
                                                    ris = oRisposta.findRispostaByNumeroOpzione(oDomanda.risposteDomanda, oDomanda.opzioniTestoLibero(iOpzione).numero)
                                                    'End If

                                                    Dim txOpzioni As New TextBox
                                                    txOpzioni = DirectCast(ctrl, TextBox)
                                                    If Not txOpzioni Is Nothing Then

                                                        If Not ris Is Nothing Then
                                                            If ris.numeroOpzione = oDomanda.opzioniTestoLibero(iOpzione).numero Then
                                                                txOpzioni.Text = ris.testoOpzione
                                                            End If
                                                        End If
                                                    End If
                                                    txOpzioni.ReadOnly = Not isEditabile
                                                    iOpzione = iOpzione + 1
                                                End If
                                            Next
                                        End If
                                    Next
                                Case Domanda.TipoDomanda.Numerica
                                    setRispostaNumerica(oDomanda, itemD, oRisposta, isEditabile)
                            End Select
                        ElseIf oDomanda.isObbligatoria And isCompileForm Then
                            'DirectCast(itemD.FindControl("LBisObbligatoria"), Label).Text = Me.Resource.getValue("LBisObbligatoria.text")
                            DirectCast(itemD.FindControl("LBisObbligatoria"), Label).Visible = True
                        End If
                    Next
                Next
            End If
            Return DLPagine
        Catch ex As Exception
            Dim errorMessage As String = ex.Message
        End Try

    End Function
    Public Function setRispostePaginaCorrette(ByVal DLPagine As DataList, ByRef oDomandeList As List(Of Domanda)) As DataList
        Try

            For Each itemP As DataListItem In DLPagine.Items
                Dim dlDomande As New DataList
                dlDomande = DLPagine.Controls(itemP.ItemIndex).FindControl("DLDomande")
                For Each itemD As DataListItem In dlDomande.Items
                    Dim idDomanda As String = dlDomande.DataKeys.Item(itemD.ItemIndex).ToString
                    Dim oDomanda As New Domanda
                    oDomanda = oDomanda.findDomandaBYID(oDomandeList, idDomanda)
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
                                        ElseIf ctrl.GetType() Is GetType(HtmlGenericControl) Then
                                            For Each dControl As Control In ctrl.Controls
                                                If dControl.GetType() Is GetType(Label) Then
                                                    Dim LBlabel As New Label
                                                    LBlabel = DirectCast(dControl, Label)
                                                    If Not LBlabel.ID Is Nothing Then
                                                        If LBlabel.ID.Contains("suggestion") And visualizzaSuggerimento Then
                                                            Dim suggerimento As String
                                                            suggerimento = SmartTagsAvailable.TagAll(oDomanda.domandaMultiplaOpzioni(iOpzione).suggestion).Trim
                                                            If suggerimento = String.Empty Then
                                                                LBlabel.Text = String.Empty
                                                            Else
                                                                LBlabel.Text &= vbCrLf & suggerimento
                                                            End If
                                                            LBlabel.CssClass = "suggestion"
                                                            LBlabel.Visible = Not String.IsNullOrEmpty(LBlabel.Text)
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
                                                ElseIf dControl.GetType() Is GetType(TextBox) Then
                                                    Dim txbOpzione As New TextBox
                                                    txbOpzione = DirectCast(dControl, TextBox)
                                                    If Not ris Is Nothing Then
                                                        If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                            txbOpzione.Text = ris.testoOpzione
                                                        End If
                                                    End If
                                                    txbOpzione.Enabled = False
                                                End If
                                            Next
                                        End If
                                        'ElseIf ctrl.GetType() Is GetType(HtmlGenericControl) Then
                                        '    For Each c As Control In ctrl.Controls
                                        '        If c.GetType() Is GetType(TextBox) Then
                                        '            Dim txbOpzione As New TextBox
                                        '            txbOpzione = DirectCast(dControl, TextBox)
                                        '            If Not ris Is Nothing Then
                                        '                If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                        '                    txbOpzione.Text = ris.testoOpzione
                                        '                End If
                                        '            End If
                                        '            txbOpzione.Enabled = False
                                        '        End If
                                        '    Next
                                        'End If
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
                                        ElseIf ctrl.GetType() Is GetType(HtmlGenericControl) Then
                                            For Each dControl As Control In ctrl.Controls
                                                If dControl.GetType() Is GetType(Label) Then
                                                    Dim LBlabel As New Label
                                                    LBlabel = DirectCast(dControl, Label)
                                                    If Not LBlabel.ID Is Nothing Then
                                                        Dim suggerimento As String
                                                        suggerimento = SmartTagsAvailable.TagAll(oDomanda.domandaMultiplaOpzioni(iOpzione).suggestion).Trim
                                                        If suggerimento = String.Empty Then
                                                            LBlabel.Text = String.Empty
                                                        Else
                                                            LBlabel.Text &= vbCrLf & suggerimento
                                                        End If
                                                        LBlabel.CssClass = "suggestion"
                                                        LBlabel.Visible = Not String.IsNullOrEmpty(LBlabel.Text)
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
                                                ElseIf dControl.GetType() Is GetType(TextBox) Then
                                                    Dim txbOpzione As New TextBox
                                                    txbOpzione = DirectCast(dControl, TextBox)
                                                    If Not ris Is Nothing Then
                                                        If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
                                                            txbOpzione.Text = ris.testoOpzione
                                                        End If
                                                    End If
                                                    txbOpzione.Enabled = False
                                                End If
                                            Next
                                        End If

                                    Next
                                Next
                                iOpzione = iOpzione + 1
                            Next
                        Case Domanda.TipoDomanda.DropDown
                            Dim dlOpzioni As New DropDownList
                            dlOpzioni = FindControlRecursive(itemD, "DDLOpzioni")
                            Dim risposteCorrette As String = String.Empty
                            Dim iOpzione As Integer = 0
                            Dim ris As New RispostaDomanda
                            Dim LBRispostaErrata As New Label
                            Dim LBRispostaCorretta As New Label
                            Dim LBsuggerimento As New Label
                            LBRispostaErrata = FindControlRecursive(itemD, "LBRispostaErrata")
                            LBRispostaCorretta = FindControlRecursive(itemD, "LBRispostaCorretta")
                            LBsuggerimento = FindControlRecursive(itemD, "LBsuggerimentoOpzione")
                            If oDomanda.risposteDomanda.Count > 0 Then
                                For Each DDItem As ListItem In dlOpzioni.Items
                                    ris = oRisposta.findRispostaByNumeroOpzione(oDomanda.risposteDomanda, DDItem.Value)
                                    If Not ris Is Nothing Then
                                        Dim suggerimento As String
                                        suggerimento = SmartTagsAvailable.TagAll(oDomanda.domandaDropDown.dropDownItems(ris.numeroOpzione - 1).suggestion).Trim
                                        If suggerimento = String.Empty Then
                                            LBsuggerimento.Text = String.Empty
                                        Else
                                            LBsuggerimento.Text &= suggerimento
                                        End If
                                        LBsuggerimento.Visible = Not String.IsNullOrEmpty(LBsuggerimento.Text)
                                        LBsuggerimento.CssClass = "suggestion"
                                        Exit For
                                    End If
                                Next
                            End If
                            If Not ris Is Nothing And oDomanda.domandaDropDown.dropDownItems.Count > 0 Then
                                If oDomanda.isValutabile Then
                                    If ris.numeroOpzione > 0 Then
                                        LBRispostaErrata.Visible = Not oDomanda.domandaDropDown.dropDownItems(ris.numeroOpzione - 1).isCorretta
                                        LBRispostaErrata.Text = Me.Resource.getValue("LBRispostaErrata.text") & oDomanda.domandaDropDown.dropDownItems(ris.numeroOpzione - 1).testo
                                    End If
                                    dlOpzioni.Visible = False
                                    LBRispostaCorretta.Visible = True
                                    MyBase.SetCulture("pg_ucStatisticheUtentiQuest", "Questionari")
                                    LBRispostaCorretta.Text = Me.Resource.getValue("LBRispostaCorretta.text")
                                    LBRispostaErrata.CssClass = "TestoRispostaErrata"
                                    LBRispostaCorretta.CssClass = "TestoRispostaCorretta"
                                    For Each oDDItem As DropDownItem In oDomanda.domandaDropDown.dropDownItems
                                        If oDDItem.isCorretta Then
                                            If Not risposteCorrette = String.Empty Then
                                                risposteCorrette = risposteCorrette & ", "
                                            End If
                                            risposteCorrette = risposteCorrette & oDDItem.testo
                                        End If
                                        'If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
                                        '    If ris.idDomandaOpzione = oDDItem.idDropDown Then
                                        '        LBsuggerimento.Text &= oDDItem.suggestion
                                        '        LBsuggerimento.Visible = True
                                        '    End If
                                        'End If
                                    Next
                                    LBRispostaCorretta.Text = LBRispostaCorretta.Text & risposteCorrette
                                Else
                                    dlOpzioni.SelectedValue = ris.numeroOpzione
                                    'dlOpzioni.Enabled = isEditabile
                                End If

                            End If
                        Case Domanda.TipoDomanda.Rating
                            Dim tabella As New Table
                            tabella = FindControlRecursive(itemD, "TBLRadiobutton_" + itemD.ItemIndex.ToString())
                            Dim indiceRiga As Integer = 0
                            For Each row As TableRow In tabella.Rows
                                Dim iCella As Integer = 0
                                If indiceRiga > 0 Then
                                    Dim ris As New RispostaDomanda
                                    If oDomanda.risposteDomanda.Count > 0 Then
                                        ris = oRisposta.findRispostaByNumeroOpzione(oDomanda.risposteDomanda, oDomanda.domandaRating.opzioniRating(indiceRiga - 1).numero)
                                    End If

                                    For Each cell As TableCell In row.Cells
                                        For Each ctrl As Control In cell.Controls
                                            If ctrl.GetType() Is GetType(HtmlGenericControl) Then
                                                For Each dControl As Control In ctrl.Controls
                                                    If dControl.GetType() Is GetType(RadioButton) Then
                                                        Dim dlOpzioni As New RadioButton
                                                        dlOpzioni = DirectCast(dControl, RadioButton)
                                                        If Not dlOpzioni Is Nothing Then
                                                            If Not ris Is Nothing Then
                                                                If iCella = ris.valore And ris.numeroOpzione = oDomanda.domandaRating.opzioniRating(indiceRiga - 1).numero Then
                                                                    dlOpzioni.Checked = True
                                                                End If
                                                            End If
                                                        End If
                                                        dlOpzioni.Enabled = False
                                                    ElseIf dControl.GetType() Is GetType(TextBox) Then
                                                        Dim dlOpzioni As New TextBox
                                                        dlOpzioni = DirectCast(dControl, TextBox)
                                                        If Not dlOpzioni Is Nothing Then
                                                            If Not ris Is Nothing Then
                                                                If ris.numeroOpzione = oDomanda.domandaRating.opzioniRating(indiceRiga - 1).numero Then
                                                                    dlOpzioni.Text = ris.testoOpzione
                                                                End If
                                                            End If
                                                        End If
                                                        dlOpzioni.Enabled = False
                                                    End If
                                                Next

                                            ElseIf ctrl.GetType() Is GetType(RadioButton) Then
                                                Dim dlOpzioni As New RadioButton
                                                dlOpzioni = DirectCast(ctrl, RadioButton)
                                                If Not dlOpzioni Is Nothing Then
                                                    If Not ris Is Nothing Then
                                                        If iCella = ris.valore And ris.numeroOpzione = oDomanda.domandaRating.opzioniRating(indiceRiga - 1).numero Then
                                                            dlOpzioni.Checked = True
                                                        End If
                                                    End If
                                                End If
                                                dlOpzioni.Enabled = False
                                            ElseIf ctrl.GetType() Is GetType(TextBox) Then
                                                Dim dlOpzioni As New TextBox
                                                dlOpzioni = DirectCast(ctrl, TextBox)
                                                If Not dlOpzioni Is Nothing Then
                                                    If Not ris Is Nothing Then
                                                        If ris.numeroOpzione = oDomanda.domandaRating.opzioniRating(indiceRiga - 1).numero Then
                                                            dlOpzioni.Text = ris.testoOpzione
                                                        End If
                                                    End If
                                                End If
                                                dlOpzioni.Enabled = False
                                            End If
                                        Next
                                        iCella = iCella + 1
                                    Next
                                End If
                                indiceRiga = indiceRiga + 1
                            Next
                        Case Domanda.TipoDomanda.RatingStars
                            Dim tabella As New Table
                            tabella = FindControlRecursive(itemD, "TBLRadiobutton_" + itemD.ItemIndex.ToString())
                            Dim indiceRiga As Integer = 0
                            For Each row As TableRow In tabella.Rows
                                Dim iCella As Integer = 0
                                'If indiceRiga > 0 Then
                                Dim ris As New RispostaDomanda
                                If oDomanda.risposteDomanda.Count > 0 Then
                                    ris = oRisposta.findRispostaByNumeroOpzione(oDomanda.risposteDomanda, oDomanda.domandaRating.opzioniRating(indiceRiga).numero)
                                End If

                                For Each cell As TableCell In row.Cells
                                    For Each ctrl As Control In cell.Controls
                                        'Dim rdStars As New Telerik.Web.UI.RadRating
                                        If ctrl.GetType() Is GetType(Telerik.Web.UI.RadRating) Then
                                            Try
                                                Dim rdStars As Telerik.Web.UI.RadRating = DirectCast(ctrl, Telerik.Web.UI.RadRating)
                                                rdStars.Value = CInt(ris.valore)
                                                rdStars.Enabled = False
                                            Catch ex As Exception

                                            End Try

                                            'If ctrl.GetType() Is GetType(HtmlGenericControl) Then
                                            '    For Each dControl As Control In ctrl.Controls
                                            '        If dControl.GetType() Is GetType(RadioButton) Then
                                            '            Dim dlOpzioni As New RadioButton
                                            '            dlOpzioni = DirectCast(dControl, RadioButton)
                                            '            If Not dlOpzioni Is Nothing Then
                                            '                If Not ris Is Nothing Then
                                            '                    If iCella = ris.valore And ris.numeroOpzione = oDomanda.domandaRating.opzioniRating(indiceRiga).numero Then
                                            '                        dlOpzioni.Checked = True
                                            '                    End If
                                            '                End If
                                            '            End If
                                            '            dlOpzioni.Enabled = False
                                            '        ElseIf dControl.GetType() Is GetType(TextBox) Then
                                            '            Dim dlOpzioni As New TextBox
                                            '            dlOpzioni = DirectCast(dControl, TextBox)
                                            '            If Not dlOpzioni Is Nothing Then
                                            '                If Not ris Is Nothing Then
                                            '                    If ris.numeroOpzione = oDomanda.domandaRating.opzioniRating(indiceRiga - 1).numero Then
                                            '                        dlOpzioni.Text = ris.testoOpzione
                                            '                    End If
                                            '                End If
                                            '            End If
                                            '            dlOpzioni.Enabled = False
                                            '        End If
                                            '    Next

                                            'ElseIf ctrl.GetType() Is GetType(RadioButton) Then
                                            '    Dim dlOpzioni As New RadioButton
                                            '    dlOpzioni = DirectCast(ctrl, RadioButton)
                                            '    If Not dlOpzioni Is Nothing Then
                                            '        If Not ris Is Nothing Then
                                            '            If iCella = ris.valore And ris.numeroOpzione = oDomanda.domandaRating.opzioniRating(indiceRiga - 1).numero Then
                                            '                dlOpzioni.Checked = True
                                            '            End If
                                            '        End If
                                            '    End If
                                            '    dlOpzioni.Enabled = False
                                        ElseIf ctrl.GetType() Is GetType(TextBox) Then
                                            Dim dlOpzioni As New TextBox
                                            dlOpzioni = DirectCast(ctrl, TextBox)
                                            If Not dlOpzioni Is Nothing Then
                                                If Not ris Is Nothing Then
                                                    If ris.numeroOpzione = oDomanda.domandaRating.opzioniRating(indiceRiga - 1).numero Then
                                                        dlOpzioni.Text = ris.testoOpzione
                                                    End If
                                                End If
                                            End If
                                            dlOpzioni.Enabled = False
                                        End If
                                    Next
                                    iCella = iCella + 1
                                Next
                                'End If
                                indiceRiga = indiceRiga + 1
                            Next
                        Case Domanda.TipoDomanda.TestoLibero
                            Dim tabella As New Table
                            tabella = FindControlRecursive(itemD, "TBLTestoLibero_" + itemD.ItemIndex.ToString())
                            Dim iOpzione As Integer = 0
                            For Each row As TableRow In tabella.Rows
                                If row.Cells.Count > 0 AndAlso row.Cells(0).HasControls Then
                                    For Each ctrl As Control In row.Cells(0).Controls
                                        If ctrl.GetType() Is GetType(TextBox) Then
                                            Dim ris As RispostaDomanda = Nothing
                                            If oDomanda.risposteDomanda.Count > 0 Then
                                                ris = oRisposta.findRispostaByNumeroOpzione(oDomanda.risposteDomanda, oDomanda.opzioniTestoLibero(iOpzione).numero)
                                            End If

                                            Dim txOpzioni As New TextBox
                                            txOpzioni = DirectCast(ctrl, TextBox)
                                            If Not txOpzioni Is Nothing Then
                                                If Not ris Is Nothing Then
                                                    txOpzioni.Text = ris.testoOpzione
                                                End If
                                            End If
                                            txOpzioni.Enabled = False
                                            iOpzione = iOpzione + 1
                                        End If
                                    Next


                                    'For Each ctrl As Control In row.Cells(1).Controls
                                    '    If ctrl.GetType() Is GetType(HtmlGenericControl) Then
                                    '        For Each dControl As Control In ctrl.Controls
                                    '            If dControl.GetType() Is GetType(TextBox) Then
                                    '                Dim txOpzioni As New TextBox
                                    '                txOpzioni = DirectCast(dControl, TextBox)
                                    '                If Not txOpzioni Is Nothing Then
                                    '                    If Not ris Is Nothing Then
                                    '                        txOpzioni.Text = ris.testoOpzione
                                    '                    End If
                                    '                End If
                                    '                txOpzioni.Enabled = False
                                    '            End If
                                    '        Next
                                    '    ElseIf ctrl.GetType() Is GetType(TextBox) Then
                                    '        Dim txOpzioni As New TextBox
                                    '        txOpzioni = DirectCast(ctrl, TextBox)
                                    '        If Not txOpzioni Is Nothing Then
                                    '            If Not ris Is Nothing Then
                                    '                txOpzioni.Text = ris.testoOpzione
                                    '            End If
                                    '        End If
                                    '        txOpzioni.Enabled = False
                                    '    End If
                                    'Next

                                End If
                            Next

                        Case Domanda.TipoDomanda.Numerica
                            'Dim isCorretta As Boolean
                            Dim tabella As New Table
                            tabella = FindControlRecursive(itemD, "TBLNumerica_" + itemD.ItemIndex.ToString())
                            Dim iOpzione As Integer = 0
                            For Each row As TableRow In tabella.Rows
                                'isCorretta = True
                                Dim ris As New RispostaDomanda
                                If oDomanda.risposteDomanda.Count > 0 Then
                                    ris = oRisposta.findRispostaByNumeroOpzione(oDomanda.risposteDomanda, oDomanda.opzioniNumerica(iOpzione).numero)
                                End If
                                'cells(1) dipende da addRispostaNumerica
                                If oDomanda.isValutabile Then
                                    For Each ctrl As Control In row.Cells(1).Controls
                                        If ctrl.GetType() Is GetType(HtmlGenericControl) Then
                                            For Each dControl As Control In ctrl.Controls
                                                If dControl.GetType() Is GetType(Label) Then
                                                    Dim LBTemp As New Label
                                                    LBTemp = DirectCast(dControl, Label)
                                                    LBTemp.Visible = False
                                                ElseIf dControl.GetType() Is GetType(TextBox) Then
                                                    Dim txOpzioni As New TextBox
                                                    txOpzioni = DirectCast(dControl, TextBox)
                                                    txOpzioni.Visible = False
                                                End If
                                            Next
                                        ElseIf ctrl.GetType() Is GetType(Label) Then
                                            Dim LBTemp As New Label
                                            LBTemp = DirectCast(ctrl, Label)
                                            LBTemp.Visible = False
                                        ElseIf ctrl.GetType() Is GetType(TextBox) Then
                                            Dim txOpzioni As New TextBox
                                            txOpzioni = DirectCast(ctrl, TextBox)
                                            txOpzioni.Visible = False
                                        End If
                                    Next
                                    Dim LBRispostaErrata As New Label
                                    Dim LBRispostaCorretta As New Label
                                    'cells(0) e controls(n) dipendono da addRispostaNumerica
                                    LBRispostaCorretta = DirectCast(row.Cells(0).Controls(2), Label)
                                    LBRispostaErrata = DirectCast(row.Cells(0).Controls(1), Label)
                                    MyBase.SetCulture("pg_ucStatisticheUtentiQuest", "Questionari")

                                    LBRispostaCorretta.Text = Me.Resource.getValue("LBRispostaCorretta.text") & oDomanda.opzioniNumerica(iOpzione).rispostaCorretta & " " & oDomanda.opzioniNumerica(iOpzione).testoDopo
                                    LBRispostaErrata.CssClass = "TestoRispostaErrata"
                                    LBRispostaCorretta.CssClass = "TestoRispostaCorretta"
                                    If Not ris Is Nothing Then
                                        If ris.valore > Integer.MinValue Then
                                            LBRispostaErrata.Visible = Not (ris.valore = oDomanda.opzioniNumerica(iOpzione).rispostaCorretta)
                                            LBRispostaCorretta.Visible = True

                                            LBRispostaErrata.Text = Me.Resource.getValue("LBRispostaErrata.text")
                                            LBRispostaErrata.Text = LBRispostaErrata.Text & ris.valore & " " & oDomanda.opzioniNumerica(iOpzione).testoDopo
                                        Else
                                            LBRispostaCorretta.Visible = True
                                            LBRispostaErrata.Visible = True
                                            LBRispostaErrata.Text = Me.Resource.getValue("MSGNoRisposta")
                                        End If
                                    End If
                                Else
                                    setRispostaNumerica(oDomanda, itemD, oRisposta, False)
                                End If
                                iOpzione = iOpzione + 1
                            Next
                    End Select
                    'End If
                Next
            Next
            Return DLPagine
        Catch ex As Exception

        End Try

    End Function
    'tabella per lo ucStatisticheGenerali
#Region "ucStatGenerali"
    Public Function loadTabellaRisposte(ByVal oDomanda As Domanda) As Table
        Select Case oDomanda.tipo
            Case Domanda.TipoDomanda.DropDown       '
                Return CreateTableForDropDownQuestion(oDomanda)

            Case Domanda.TipoDomanda.Multipla       'To verify
                Return CreateTableForMultipleChoiceQuestion(oDomanda)

            Case Domanda.TipoDomanda.Rating         '
                Return CreateTableForRatingQuestion(oDomanda)

            Case Domanda.TipoDomanda.RatingStars    '
                Return CreateTableForRatingStarsQuestion_V2(oDomanda) 'NOTA: al momento è uguale a quella rating normale: copia incolla, tenuta diversa per metterci le stelline, eventualmente.

            Case Domanda.TipoDomanda.TestoLibero    '
                Return CreateTableForFreeTextQuestion(oDomanda)
            Case Domanda.TipoDomanda.Numerica       '
                Return CreateTableForNumericQuestion(oDomanda)


            Case Domanda.TipoDomanda.Meeting        'unused on 0404 2018
                Return creaTabellaMeeting(oDomanda)
        End Select
        Return New Table

    End Function



    'Risposta multipla - Telerick HTML 5 - Counter risposte - V
    Private Function CreateTableForMultipleChoiceQuestion(ByVal oDomanda As Domanda) As Table

        Dim labelLenght As Integer = 0
        'Dim headerLenght As Integer = 2

        Dim mainTable As New Table
        mainTable.CssClass = "multiplechoicequestion statistics"
        Dim dataTable As New Table
        dataTable.CssClass = "tableData"

        'Header
        Dim RowHeader As New TableHeaderRow()
        Dim CellHeader As New TableHeaderCell()
        CellHeader.Text = "#"
        CellHeader.CssClass = _ClassData
        RowHeader.Cells.Add(CellHeader)

        CellHeader = New TableHeaderCell()
        CellHeader.Text = "Testo opzione"
        CellHeader.CssClass = _ClassText
        RowHeader.Cells.Add(CellHeader)

        CellHeader = New TableHeaderCell()
        CellHeader.Text = "Freq."
        CellHeader.CssClass = _ClassData
        RowHeader.Cells.Add(CellHeader)

        CellHeader = New TableHeaderCell()
        CellHeader.Text = "Freq. %"
        CellHeader.CssClass = _ClassData
        RowHeader.Cells.Add(CellHeader)

        dataTable.Rows.Add(RowHeader)


        'Dati
        Dim cellaGrafico As New TableCell
        cellaGrafico.CssClass = "answergraphic"

        Dim putTextOutsideChart As Boolean = True

        ' controllo se i testi delle opzioni contengono HTML
        For Each oOpz As DomandaOpzione In oDomanda.domandaMultiplaOpzioni
            If Not Me.SmartTagsAvailable.TagAll(oOpz.testo) = oOpz.testo Or oOpz.testo.Contains("<") Or oOpz.isAltro Then
                cellaGrafico.CssClass &= " small"
                Exit For
            End If
        Next

        If Not putTextOutsideChart Then
            oDomanda.domandaMultiplaOpzioni.Reverse()
        End If


        Dim nRispsPercent As New List(Of Decimal)
        Dim nRispsCount As New List(Of Integer)



        For i As Int16 = 0 To (oDomanda.domandaMultiplaOpzioni.Count - 1) 'To 0 Step -1

            Dim oOpz As DomandaOpzione = oDomanda.domandaMultiplaOpzioni(i)
            'Next
            'For Each oOpz As DomandaOpzione In oDomanda.domandaMultiplaOpzioni
            oOpz.numeroRisposte = (From oRis In oDomanda.risposteDomanda Select oRis.numeroOpzione Where numeroOpzione = oOpz.numero).Count

            If oDomanda.risposteDomanda.Count > 0 Then
                nRispsPercent.Add(oOpz.numeroRisposte / oDomanda.risposteDomanda.Count)
                nRispsCount.Add(oOpz.numeroRisposte)
            End If
        Next

        MyBase.SetCulture("pg_ucStatisticheGeneraliQuest", "Questionari")
        Dim addDisplayoptionanwsers As Boolean = False
        For iOpzione As Int16 = 0 To (oDomanda.domandaMultiplaOpzioni.Count - 1) Step 1
            Dim oOpz As DomandaOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione)
            If oOpz.isAltro Then
                addDisplayoptionanwsers = True
                Exit For
            End If
        Next



        'Grafico - NEW HTML 5
        Dim values As New List(Of Double)()
        Dim PieValues As New List(Of KeyValuePair(Of String, Double))()

        Dim xAxisLabel As New List(Of String)()

        Dim RisposteTotali As Integer = 0

        Dim Alternate As Boolean = True

        Dim ColSpan As Integer = 0

        For iOpzione As Int16 = 0 To (oDomanda.domandaMultiplaOpzioni.Count - 1) Step 1

            Dim rigaOpz As New TableRow
            rigaOpz.CssClass = "optiontext"


            Dim oOpz As DomandaOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione)

            Dim Cella As New TableCell
            Cella.CssClass = _ClassData
            Cella.Text = oOpz.numero
            rigaOpz.Cells.Add(Cella)


            Cella = New TableCell
            Cella.CssClass = _ClassText


            If oOpz.testo.Length > labelLenght Then
                labelLenght = oOpz.testo.Length
            End If

            If oOpz.isCorretta Then
                Cella.Text = String.Format("{0}{1}", oOpz.testo, _CorrectString)
            Else
                Cella.Text = oOpz.testo
            End If

            rigaOpz.Cells.Add(Cella)


            Cella = New TableCell
            Cella.CssClass = _ClassData
            Cella.Text = oOpz.numeroRisposte
            rigaOpz.Cells.Add(Cella)

            RisposteTotali = RisposteTotali + oOpz.numeroRisposte

            Cella = New TableCell
            Cella.CssClass = _ClassData
            Cella.Text = String.Format("{0}%", (oOpz.numeroRisposte * 100 / oDomanda.risposteDomanda.Count).ToString(_FormatPerc))

            If oOpz.isAltro Then
                Dim LNKVisualizzaRisposte As New LinkButton
                MyBase.SetCulture("pg_ucStatisticheGeneraliQuest", "Questionari")
                LNKVisualizzaRisposte.Text = Me.Resource.getValue("VisualizzaRisposteAltro")
                LNKVisualizzaRisposte.CommandName = "visualizzaRisposte"
                LNKVisualizzaRisposte.CommandArgument = oOpz.id
                Cella.Controls.Add(LNKVisualizzaRisposte)
                'Else
                '    Cella.CssClass &= " empty"
            End If

            rigaOpz.Cells.Add(Cella)

            'Cella = New TableCell
            'Cella.CssClass = _ClassLink

            'rigaOpz.Cells.Add(Cella)

            If Alternate Then
                rigaOpz.CssClass &= " alternate"
            End If

            Alternate = Not Alternate

            If ColSpan < rigaOpz.Cells.Count Then
                ColSpan = rigaOpz.Cells.Count
            End If

            dataTable.Rows.Add(rigaOpz)


            'NEW HTML 5


            Dim val As Double = 0
            If oDomanda.risposteDomanda.Count > 0 Then
                If putTextOutsideChart Then
                    val = nRispsPercent(iOpzione)
                    'serie.SetItemValue(iOpzione, )
                Else
                    val = nRispsPercent(oDomanda.domandaMultiplaOpzioni.Count - 1 - iOpzione)
                    'serie.SetItemValue(iOpzione, nRispsPercent(oDomanda.domandaMultiplaOpzioni.Count - 1 - iOpzione))
                End If
                'Else
                '    oItem = New SeriesItem(0)
                'serie.SetItemValue(iOpzione, 0)
            End If

            If Double.IsNaN(val) Then
                val = 0
            End If

            values.Add(val)

            'Dim chartOptText As String = TxtHelper_HtmlCutToString(oOpz.testo, _MaxCharacterForchart)

            'PieValues.Add(New KeyValuePair(Of String, Double)(chartOptText, val))
            PieValues.Add(New KeyValuePair(Of String, Double)(oOpz.testo, val))

            'String.Format("Opzione {0}", iOpzione) '

            Dim xAxisValue As String = oDomanda.domandaMultiplaOpzioni(iOpzione).testo 'TxtHelper_HtmlCutToString(oDomanda.domandaMultiplaOpzioni(iOpzione).testo, _MaxCharacterForchart)

            If (oDomanda.isValutabile AndAlso oDomanda.domandaMultiplaOpzioni(iOpzione).isCorretta) Then
                xAxisValue = String.Format("* {0}", xAxisValue)
            End If

            xAxisLabel.Add(xAxisValue)
        Next

        Dim TotalValPerc As Double = values.Sum()

        'Footer Dati
        Dim rigaRiepilogo As New TableRow()
        rigaRiepilogo.CssClass = "summary"
        Dim cellaRiepilogo As New TableCell()

        cellaRiepilogo.Text = "Totale"
        cellaRiepilogo.CssClass = _ClassText
        cellaRiepilogo.ColumnSpan = 2
        rigaRiepilogo.Cells.Add(cellaRiepilogo)

        cellaRiepilogo = New TableCell()
        cellaRiepilogo.Text = RisposteTotali
        cellaRiepilogo.CssClass = _ClassData
        rigaRiepilogo.Cells.Add(cellaRiepilogo)

        cellaRiepilogo = New TableCell()
        cellaRiepilogo.Text = String.Format("{0}%", (TotalValPerc * 100).ToString(_FormatPerc))
        cellaRiepilogo.CssClass = _ClassData
        rigaRiepilogo.Cells.Add(cellaRiepilogo)

        dataTable.Rows.Add(rigaRiepilogo)
        'NEW HTML5

        Dim ChartTitle As String = ""

        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
            ChartTitle = TxtHelper_CutString(Me.QuestionarioCorrente.nome, _MaxCharacterForchartInline)
        End If


        Dim isNewLine As Boolean = False

        If (labelLenght > _NewLineMaxHeaderChar) Then
            isNewLine = True
        End If

        'NEW HTML5 RadChart
        Dim oChart As RadHtmlChart

        Dim chartContainer As New System.Web.UI.HtmlControls.HtmlGenericControl("DIV")

        Dim textLenght As Integer = _MaxCharacterForchartInline

        If isNewLine Then
            textLenght = _MaxCharacterForchartNewline
        End If

        If oDomanda.isMultipla Then
            oChart = ChartHelper.CreateBarChar(
            values, xAxisLabel, True, 100, textLenght, ChartTitle)
            chartContainer.Attributes.Add("class", "optionChart barChart")
        Else
            oChart = ChartHelper.CreatePieChar(PieValues, textLenght)
            chartContainer.Attributes.Add("class", "optionChart pieChart")
        End If

        chartContainer.Controls.Add(oChart)

        cellaGrafico.ColumnSpan = ColSpan

        Dim tdData As New TableCell

        Dim chartClass As String = "dataContainer" 'maxLabelLenght

        If isNewLine Then
            chartClass = "dataContainer longLabel"
        End If

        tdData.CssClass = chartClass '"dataContainer"
        tdData.Controls.Add(dataTable)
        tdData.Controls.Add(chartContainer)

        Dim trData As New TableRow
        trData.Cells.Add(tdData)

        mainTable.Rows.Add(trData)

        Return mainTable
    End Function




    Private Function CreateTableForDropDownQuestion(ByVal question As Domanda) As Table

        Dim labelLenght As Integer = 0
        'Dim headerLenght As Integer = 2

        Dim mainTable As New Table
        mainTable.CssClass = "dropdownquestion statistics"

        Dim dataTable As New Table
        dataTable.CssClass = "tableData"

        Dim RowHeader As New TableHeaderRow
        Dim HeadCell As New TableHeaderCell
        HeadCell.CssClass = _ClassData
        HeadCell.Text = "#"
        RowHeader.Cells.Add(HeadCell)

        HeadCell = New TableHeaderCell
        HeadCell.CssClass = _ClassText
        HeadCell.Text = "Testo Opzione"
        RowHeader.Cells.Add(HeadCell)

        HeadCell = New TableHeaderCell
        HeadCell.CssClass = _ClassData
        HeadCell.Text = "Freq."
        RowHeader.Cells.Add(HeadCell)

        HeadCell = New TableHeaderCell
        HeadCell.CssClass = _ClassData
        HeadCell.Text = "Freq. %"
        RowHeader.Cells.Add(HeadCell)

        dataTable.Rows.Add(RowHeader)
        'NEW HTML 5
        Dim values As New List(Of KeyValuePair(Of String, Double))()
        Dim xAxisLabel As New List(Of String)()

        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
            Title = Me.QuestionarioCorrente.nome
        End If

        Dim Alternate As Boolean = True

        Dim TotaleRisposte As Integer = 0

        For Each oOpz As DropDownItem In question.domandaDropDown.dropDownItems
            Dim iOpzione As Int16
            Dim count As Integer = 0
            For Each oRisp As RispostaDomanda In question.risposteDomanda
                If oOpz.numero = oRisp.numeroOpzione Then
                    count = count + 1
                End If
            Next

            oOpz.numeroRisposte = count

            Dim nRispPercent As Decimal = 0
            If question.risposteDomanda.Count > 0 Then
                nRispPercent = oOpz.numeroRisposte / question.risposteDomanda.Count

            End If

            TotaleRisposte = TotaleRisposte + oOpz.numeroRisposte

            'creo la tabella per le risposte dropdown

            Dim riga As New TableRow
            riga.CssClass = "optiontext"
            Dim cella As New TableCell
            cella.CssClass = _ClassData
            cella.Text = oOpz.numero
            riga.Cells.Add(cella)

            cella = New TableCell
            cella.CssClass = _ClassText

            If oOpz.testo.Length > labelLenght Then
                labelLenght = oOpz.testo.Length
            End If

            If (oOpz.isCorretta) Then
                cella.Text = String.Format("{0} {1}", oOpz.testo, _CorrectString)
            Else
                cella.Text = oOpz.testo
            End If

            riga.Cells.Add(cella)

            cella = New TableCell
            cella.CssClass = _ClassData
            cella.Text = oOpz.numeroRisposte
            riga.Cells.Add(cella)

            cella = New TableCell
            cella.CssClass = _ClassData
            cella.Text = String.Format("{0}%", (oOpz.numeroRisposte * 100 / question.risposteDomanda.Count).ToString(_FormatPerc))
            riga.Cells.Add(cella)
            If Alternate Then
                riga.CssClass &= " alternate"
            End If

            Alternate = Not Alternate

            dataTable.Rows.Add(riga)

            xAxisLabel.Add(oOpz.testo)


            Dim chartItem As New ChartSeriesItem
            MyBase.SetCulture("pg_ucStatisticheGeneraliQuest", "Questionari")
            chartItem.Name = Me.Resource.getValue("opzioneLegenda") & oOpz.numero

            If Double.IsNaN(nRispPercent) Then
                nRispPercent = 0
            End If
            values.Add(New KeyValuePair(Of String, Double)(oOpz.testo, nRispPercent))
            iOpzione += 1
        Next

        Dim RowSummaries As New TableRow
        RowSummaries.CssClass = "summary"
        Dim sumCell As New TableCell
        sumCell.ColumnSpan = 2
        sumCell.CssClass = _ClassText
        sumCell.Text = "Totale"
        RowSummaries.Cells.Add(sumCell)

        sumCell = New TableCell
        sumCell.CssClass = _ClassData
        sumCell.Text = TotaleRisposte

        RowSummaries.Cells.Add(sumCell)

        sumCell = New TableCell
        sumCell.CssClass = _ClassData
        If question.risposteDomanda.Count > 0 Then
            sumCell.Text = String.Format("{0}%", (TotaleRisposte * 100 / question.risposteDomanda.Count).ToString(_FormatPerc))
        Else
            sumCell.Text = "0%"
        End If
        RowSummaries.Cells.Add(sumCell)

        dataTable.Rows.Add(RowSummaries)

        'HTML 5 chart

        Dim isnewLine As Boolean = False
        Dim charLenght As Integer = _MaxCharacterForchartInline

        If (labelLenght >= _NewLineMaxHeaderChar) Then
            isnewLine = True
            charLenght = _MaxCharacterForchartNewline
        End If


        Dim oChart As RadHtmlChart = ChartHelper.CreatePieChar(values, charLenght)

        Dim chartContainer As New System.Web.UI.HtmlControls.HtmlGenericControl("DIV")
        chartContainer.Controls.Add(oChart)
        chartContainer.Attributes.Add("class", "optionChart pieChart")

        Dim tdData As New TableCell
        Dim chartClass As String = "dataContainer" 'maxLabelLenght

        If isnewLine Then
            chartClass = "dataContainer longLabel"
        End If

        tdData.CssClass = chartClass '"dataContainer"
        tdData.Controls.Add(dataTable)
        tdData.Controls.Add(chartContainer)

        Dim trData As New TableRow
        trData.Cells.Add(tdData)

        mainTable.Rows.Add(trData)

        Return mainTable
    End Function




    Private Function CreateTableForRatingQuestion(ByVal oDomanda As Domanda) As Table
        Dim labelLenght As Integer = 0
        Dim headerLenght As Integer = 2
        'INTESTAZIONI

        Dim averages As New List(Of Double)()
        Dim labels As New List(Of String)()

        Dim Summaries As New Dictionary(Of Integer, Integer)()

        'Dim currentAvg As Double = 0

        Dim mainTable As New Table
        mainTable.CssClass = "ratingquestion statistics"


        Dim tableData As New Table
        tableData.CssClass = "tableData"

        Dim numeroRating As Integer = oDomanda.domandaRating.numeroRating
        Dim rigaInt As New TableHeaderRow


        Dim hasAverage As Boolean = False

        Dim TestoND As String
        If String.IsNullOrEmpty(oDomanda.domandaRating.testoND) Then
            TestoND = "--"
        Else
            TestoND = oDomanda.domandaRating.testoND
        End If

        If String.IsNullOrWhiteSpace(TestoND) Then
            TestoND = "&nbsp;"
        End If

        'Cella colonna opzioni
        'If oDomanda.domandaRating.opzioniRating.Count > 0 Then

        Dim cellaInt1 As New TableHeaderCell
        cellaInt1.Text = "#"
        cellaInt1.CssClass = _ClassIndex
        rigaInt.Cells.Add(cellaInt1)

        cellaInt1 = New TableHeaderCell
        cellaInt1.Text = "Opzione"
        cellaInt1.CssClass = _ClassText
        rigaInt.Cells.Add(cellaInt1)
        'End If

        'Celle valori
        If oDomanda.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Numerazione Then
            For i As Integer = 1 To oDomanda.domandaRating.numeroRating
                Dim cella As New TableHeaderCell
                cella.Text = i
                cella.CssClass = _ClassData
                rigaInt.Cells.Add(cella)

                headerLenght += 2
            Next
            hasAverage = True
        Else
            For Each oInt As DomandaOpzione In oDomanda.domandaRating.intestazioniRating
                Dim cella As New TableHeaderCell
                cella.Text = oInt.testo

                headerLenght += cella.Text.Length + 1

                cella.CssClass = _ClassData
                rigaInt.Cells.Add(cella)
            Next
            hasAverage = False
        End If

        If hasAverage Then
            'Cella media
            Dim cellaAVGheader As New TableHeaderCell
            cellaAVGheader.Text = Me.Resource.getValue("HeaderValue.Average")
            cellaAVGheader.Text = "Avg."
            cellaAVGheader.CssClass = _ClassData
            rigaInt.Cells.Add(cellaAVGheader)

            'Cella Dev
            Dim cellaDevheader As New TableHeaderCell
            cellaDevheader.Text = Me.Resource.getValue("HeaderValue.Dev")
            cellaDevheader.Text = "Dev.St."
            cellaDevheader.CssClass = _ClassData
            rigaInt.Cells.Add(cellaDevheader)

            headerLenght += 4
        End If

        rigaInt.CssClass = "header"

        'Cella ND
        If oDomanda.domandaRating.mostraND Then
            Dim cellaND As New TableHeaderCell
            cellaND.CssClass = _ClassData
            cellaND.Text = TestoND
            rigaInt.Cells.Add(cellaND)
            'numeroRating = numeroRating + 1
            headerLenght += 2
        End If

        'Dim ColSpan As Integer = rigaInt.Cells.Count()

        tableData.Rows.Add(rigaInt)

        'RIGHE VALORI
        Dim iOpzione As Integer = 0

        Dim chrValues As New List(Of KeyValuePair(Of String, List(Of KeyValuePair(Of String, Integer))))

        Dim BarCount As Integer = oDomanda.domandaRating.opzioniRating.Count

        If BarCount < 1 Then
            BarCount = 0
        Else
            BarCount = BarCount - 1
        End If

        Dim index As Integer = 1

        'Dim countND As Integer = 0
        For Each oOpz As DomandaOpzione In oDomanda.domandaRating.opzioniRating

            Dim chrSerie As New List(Of KeyValuePair(Of String, Integer))

            Dim riga As New TableRow

            Dim cella As New TableCell
            cella.CssClass = _ClassIndex
            cella.Text = index
            index = index + 1
            riga.Cells.Add(cella)

            cella = New TableCell

            'If oDomanda.domandaRating.opzioniRating.Count > 1 Then
            Dim text As String = SmartTagsAvailable.TagAll(oOpz.testo)

            If (text.Length > labelLenght) Then
                labelLenght = text.Length
            End If

            If String.IsNullOrEmpty(text) Then
                text = "&nbsp;"
            End If

            Dim lbTesto As New Label
            lbTesto.Text = text

            cella.CssClass = _ClassText
            cella.Controls.Add(lbTesto)

            labels.Add(oOpz.testo)

            If oOpz.isAltro Then
                Dim LNKVisualizzaRisposte As New LinkButton
                MyBase.SetCulture("pg_ucStatisticheGenerali", "Questionari")
                LNKVisualizzaRisposte.Text = Me.Resource.getValue("VisualizzaRisposteAltro")
                LNKVisualizzaRisposte.CommandName = "visualizzaRisposte"
                LNKVisualizzaRisposte.CommandArgument = oOpz.id
                cella.Controls.Add(LNKVisualizzaRisposte)
            End If

            riga.Cells.Add(cella)
            'End If

            Dim avgValues As New List(Of Integer)

            Dim chrSeries As New List(Of KeyValuePair(Of String, Integer))




            For c As Integer = 1 To numeroRating
                Dim cellaRB As New TableCell
                cellaRB.CssClass = _ClassData
                Dim count As Integer = 0
                For Each oRisp As RispostaDomanda In oDomanda.risposteDomanda
                    'notext serve per visualizzare le statistiche corrette quando non c'e' un testo nell'opz. rating. se si cambia la lettura dei dati dal quest, va cambiato anche questo
                    If oOpz.numero = oRisp.numeroOpzione And c = oRisp.valore Then
                        count = count + 1
                    End If
                Next

                If Not hasAverage Then

                    Dim opz As DomandaOpzione = oDomanda.domandaRating.intestazioniRating _
                    .FirstOrDefault(Function(o) o.numero = c)

                    If Not IsNothing(opz) AndAlso Not String.IsNullOrWhiteSpace(opz.testo) Then
                        chrSeries.Add(New KeyValuePair(Of String, Integer)(TxtHelper_HtmlToString(opz.testo), count))
                    Else
                        chrSeries.Add(New KeyValuePair(Of String, Integer)(TxtHelper_HtmlToString(c.ToString()), count))
                    End If

                End If

                If (Summaries.ContainsKey(c)) Then

                    Summaries(c) = Summaries(c) + count
                Else
                    Summaries.Add(c, count)
                End If

                cellaRB.Text = count
                Dim chartItem As New ChartSeriesItem
                chartItem.Appearance.FillStyle.MainColor = GetColor(iOpzione)
                chartItem.YValue = count
                riga.Cells.Add(cellaRB)

                If count > 0 Then
                    For i As Integer = 1 To count
                        avgValues.Add(c)
                    Next
                End If

            Next



            If hasAverage Then
                Dim currentAvg As Double = 0
                Dim currentDev As Double = Deviation(avgValues, currentAvg)

                If (Double.IsNaN(currentAvg)) Then
                    currentAvg = 0
                End If
                averages.Add(currentAvg)


                'Average
                Dim cellaAvg As New TableCell()
                cellaAvg.CssClass = _ClassData
                cellaAvg.Text = currentAvg.ToString(_FormatAvg)
                riga.Cells.Add(cellaAvg)

                'Deviation
                Dim cellaDev As New TableCell()
                cellaDev.CssClass = _ClassData
                cellaDev.Text = currentDev.ToString(_FormatDev)
                riga.Cells.Add(cellaDev)

            Else
                Dim kvpSeries As KeyValuePair(Of String, List(Of KeyValuePair(Of String, Integer))) =
                    New KeyValuePair(Of String, List(Of KeyValuePair(Of String, Integer)))(TxtHelper_HtmlToString(oOpz.testo), chrSeries)

                chrValues.Add(kvpSeries)

            End If

            MyBase.SetCulture("pg_ucStatisticheGenerali", "Questionari")

            If (iOpzione Mod 2 > 0) Then
                riga.CssClass = "alternate"
            End If

            iOpzione = iOpzione + 1


            'Celle "No Risposta"

            If oDomanda.domandaRating.mostraND Then
                Dim cellaND As New TableCell
                cellaND.CssClass = _ClassData
                Dim count As Integer = 0

                For Each oRisp As RispostaDomanda In oDomanda.risposteDomanda
                    'notext serve per visualizzare le statistiche corrette quando non c'e' un testo nell'opz. rating. se si cambia la lettura dei dati dal quest, va cambiato anche questo
                    If oOpz.numero = oRisp.numeroOpzione And oRisp.valore = numeroRating + 1 Then
                        count = count + 1
                    End If
                Next

                If Not Summaries.ContainsKey(numeroRating + 1) Then
                    Summaries(numeroRating + 1) = 0
                End If

                Summaries(numeroRating + 1) += count
                cellaND.Text = count
                riga.Cells.Add(cellaND)
            End If

            tableData.Rows.Add(riga)
        Next

        Dim chartTitle As String = ""

        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
            chartTitle = Me.QuestionarioCorrente.nome
        End If



        'Riga riepilogo
        Dim rowSummary As New TableRow
        rowSummary.CssClass = "summary"

        Dim cellSummary As New TableCell()
        cellSummary.Text = "Totali"
        cellSummary.CssClass = _ClassText
        cellSummary.ColumnSpan = 2
        rowSummary.Cells.Add(cellSummary)

        For i As Integer = 1 To numeroRating
            cellSummary = New TableCell()
            cellSummary.CssClass = _ClassData
            If Summaries.ContainsKey(i) Then
                cellSummary.Text = Summaries(i)
            Else
                cellSummary.Text = "0"
            End If
            rowSummary.Cells.Add(cellSummary)
        Next

        If hasAverage Then
            cellSummary = New TableCell()
            cellSummary.CssClass = _ClassData
            cellSummary.Text = "--"
            rowSummary.Cells.Add(cellSummary)
            cellSummary = New TableCell()
            cellSummary.CssClass = _ClassData
            cellSummary.Text = "--"
            rowSummary.Cells.Add(cellSummary)
        End If

        If oDomanda.domandaRating.mostraND Then
            cellSummary = New TableCell()
            cellSummary.CssClass = _ClassData
            If Summaries.ContainsKey(numeroRating + 1) Then
                cellSummary.Text = Summaries(numeroRating + 1)
            Else
                cellSummary.Text = "0"
            End If
            rowSummary.Cells.Add(cellSummary)
        End If

        tableData.Rows.Add(rowSummary)



        'Summaries(c)


        ''Cella tabella dati

        'Dim cellaDati As New TableCell
        'cellaDati.Controls.Add(tableData)

        'Dim rigaDati As New TableRow
        'rigaDati.Cells.Add(cellaDati)


        'GRafico HTML 5

        Dim charLenght As Integer = _MaxCharacterForchartInline
        Dim newLine As Boolean = False
        If (labelLenght > _NewLineCharacter) OrElse (labelLenght + headerLenght > _NewLineMaxHeaderChar) Then
            newLine = True
            charLenght = _MaxCharacterForchartNewline
        End If

        Dim oChart As RadHtmlChart
        If hasAverage Then
            oChart = ChartHelper.CreateBarChar(averages, labels, False, numeroRating, charLenght, chartTitle)
        Else
            oChart = ChartHelper.CreateStackedBarCharForRatingReverse(BarCount, chrValues, charLenght)
        End If


        'Dim cellaGrafico As New TableCell
        'cellaGrafico.CssClass = "tdchart"

        'If Not IsNothing(oChart) Then
        '    cellaGrafico.Controls.Add(oChart)
        'End If


        'Dim rigaGrafico As New TableRow
        ''Dim colSpan As Integer = numeroRating + 2
        ''If oDomanda.domandaRating.mostraND Then
        ''    colSpan += 1
        ''End If
        ''cellaGrafico.ColumnSpan = colSpan
        'rigaGrafico.Cells.Add(cellaGrafico)

        Dim chartContainer As New System.Web.UI.HtmlControls.HtmlGenericControl("DIV")
        chartContainer.Controls.Add(oChart)

        Dim chartClass As String = "dataContainer" 'maxLabelLenght

        If newLine Then
            chartClass = "dataContainer longLabel"
        End If

        chartContainer.Attributes.Add("class", "optionChart barChart")

        Dim tdData As New TableCell
        tdData.CssClass = chartClass '"dataContainer"
        tdData.Controls.Add(tableData)
        tdData.Controls.Add(chartContainer)

        Dim trData As New TableRow
        trData.Cells.Add(tdData)

        mainTable.Rows.Add(trData)

        ''Main:
        'mainTable.Rows.Add(rigaDati)
        'mainTable.Rows.Add(rigaGrafico)

        Return mainTable
    End Function

    Private Function CreateTableForRatingStarsQuestion_V2(ByVal oDomanda As Domanda) As Table

        Dim labelLenght As Integer = 0
        Dim headerLenght As Integer = 2

        'INTESTAZIONI
        Dim mainTable As New Table
        mainTable.CssClass = "ratingquestion statistics"

        Dim tabella As New Table
        tabella.CssClass = "tableData"

        Dim averages As New List(Of Double)()
        Dim labels As New List(Of String)()

        'Dim currentAvg As Double = 0
        Dim Summaries As New Dictionary(Of Integer, Integer)()



        Dim numeroRating As Integer = oDomanda.domandaRating.numeroRating
        Dim rigaInt As New TableHeaderRow
        rigaInt.CssClass = "header"

        'Cella vuota (colonna opzioni)
        'If oDomanda.domandaRating.opzioniRating.Count > 1 Then
        Dim cellaInt1 As New TableHeaderCell
            cellaInt1.Text = "#"
            cellaInt1.CssClass = _ClassIndex
            rigaInt.Cells.Add(cellaInt1)

            cellaInt1 = New TableHeaderCell
            cellaInt1.Text = "Testo opzione"
            cellaInt1.CssClass = _ClassText
            rigaInt.Cells.Add(cellaInt1)
        'End If


        'Celle valori
        'If oDomanda.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Numerazione Then
        For i As Integer = 1 To oDomanda.domandaRating.numeroRating
            Dim cella As New TableHeaderCell
            cella.CssClass = _ClassData
            cella.Text = i
            rigaInt.Cells.Add(cella)
            headerLenght += 2
        Next



        'Cella media
        Dim cellaAVGheader As New TableHeaderCell
        cellaAVGheader.CssClass = _ClassData
        cellaAVGheader.Text = Me.Resource.getValue("HeaderValue.Average")
        cellaAVGheader.Text = "Avg."
        rigaInt.Cells.Add(cellaAVGheader)

        'Cella Dev
        Dim cellaDevheader As New TableHeaderCell
        cellaDevheader.CssClass = _ClassData
        cellaDevheader.Text = Me.Resource.getValue("HeaderValue.Dev")
        cellaDevheader.Text = "Dev.St."
        rigaInt.Cells.Add(cellaDevheader)

        headerLenght += 4
        'Dim colSpan As Integer = rigaInt.Cells.Count

        tabella.Rows.Add(rigaInt)

        'RIGHE VALORI
        Dim iOpzione As Integer = 0

        Dim chrValues As New List(Of KeyValuePair(Of String, List(Of KeyValuePair(Of String, Integer))))

        For Each oOpz As DomandaOpzione In oDomanda.domandaRating.opzioniRating

            'If oDomanda.domandaRating.opzioniRating.Count > 1 Then
            '    Dim cellaopz As New TableCell
            '    cellaopz.Text = (iOpzione + 1)
            '    rigaInt.Cells.Add(cellaopz)
            'End If

            Dim chrSerie As New List(Of KeyValuePair(Of String, Integer))

            Dim riga As New TableRow
            Dim cella As New TableCell

            cella.CssClass = _ClassIndex
            cella.Text = iOpzione
            riga.Cells.Add(cella)

            cella = New TableCell

            'If oDomanda.domandaRating.opzioniRating.Count > 1 Then
            Dim lbTesto As New Label
                lbTesto.Text = SmartTagsAvailable.TagAll(oOpz.testo)

                If lbTesto.Text.Length > labelLenght Then
                    labelLenght = lbTesto.Text.Length
                End If

                cella.CssClass = _ClassText
                cella.Controls.Add(lbTesto)

                labels.Add(oOpz.testo)

                If oOpz.isAltro Then
                    Dim LNKVisualizzaRisposte As New LinkButton
                    MyBase.SetCulture("pg_ucStatisticheGenerali", "Questionari")
                    LNKVisualizzaRisposte.Text = Me.Resource.getValue("VisualizzaRisposteAltro")
                    LNKVisualizzaRisposte.CommandName = "visualizzaRisposte"
                    LNKVisualizzaRisposte.CommandArgument = oOpz.id
                    cella.Controls.Add(LNKVisualizzaRisposte)
                End If
                riga.Cells.Add(cella)
            ' End If

            'currentAvg = 0
            'Dim currentCount As Integer = 0



            'Celle risposte previste

            Dim avgValues As New List(Of Integer)

            Dim chrSeries As New List(Of KeyValuePair(Of String, Integer))

            For c As Integer = 1 To numeroRating
                Dim cellaRB As New TableCell
                cellaRB.CssClass = _ClassData

                Dim count As Integer = 0
                For Each oRisp As RispostaDomanda In oDomanda.risposteDomanda
                    'notext serve per visualizzare le statistiche corrette quando non c'e' un testo nell'opz. rating. se si cambia la lettura dei dati dal quest, va cambiato anche questo
                    If oOpz.numero = oRisp.numeroOpzione And c = oRisp.valore Then
                        count = count + 1
                    End If
                Next

                cellaRB.Text = count
                Dim chartItem As New ChartSeriesItem
                chartItem.Appearance.FillStyle.MainColor = GetColor(iOpzione)
                chartItem.YValue = count
                riga.Cells.Add(cellaRB)

                If count > 0 Then
                    For i As Integer = 1 To count
                        avgValues.Add(c)
                    Next
                End If

                If (Summaries.ContainsKey(c)) Then

                    Summaries(c) = Summaries(c) + count
                Else
                    Summaries(c) = count
                End If
            Next




            Dim currentAvg As Double = 0
            Dim currentDev As Double = Deviation(avgValues, currentAvg)

            If (Double.IsNaN(currentAvg)) Then
                currentAvg = 0
            End If
            averages.Add(currentAvg)


            'Average
            Dim cellaAvg As New TableCell()
            cellaAvg.CssClass = _ClassData
            cellaAvg.Text = currentAvg.ToString(_FormatAvg)
            riga.Cells.Add(cellaAvg)

            'Deviation
            Dim cellaDev As New TableCell()
            cellaDev.CssClass = _ClassData
            cellaDev.Text = currentDev.ToString(_FormatDev)
            riga.Cells.Add(cellaDev)


            MyBase.SetCulture("pg_ucStatisticheGenerali", "Questionari")

            If (iOpzione Mod 2 > 0) Then
                riga.CssClass = "alternate"
            End If

            iOpzione = iOpzione + 1




            tabella.Rows.Add(riga)
        Next

        'Riga riepilogo
        Dim rowSummary As New TableRow
        rowSummary.CssClass = "summary"

        Dim cellSummary As New TableCell()
        cellSummary.CssClass = _ClassText
        cellSummary.ColumnSpan = 2
        cellSummary.Text = "Totali"
        rowSummary.Cells.Add(cellSummary)

        For i As Integer = 1 To numeroRating
            cellSummary = New TableCell()
            cellSummary.CssClass = _ClassData
            If Summaries.ContainsKey(i) Then
                cellSummary.Text = Summaries(i)
            Else
                cellSummary.Text = "0"
            End If
            rowSummary.Cells.Add(cellSummary)
        Next


        cellSummary = New TableCell()
        cellSummary.CssClass = _ClassData
        cellSummary.Text = "--"
        rowSummary.Cells.Add(cellSummary)
        cellSummary.CssClass = _ClassData
        cellSummary = New TableCell()
        cellSummary.Text = "--"
        rowSummary.Cells.Add(cellSummary)

        tabella.Rows.Add(rowSummary)

        Dim chartTitle As String = ""

        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
            chartTitle = Me.QuestionarioCorrente.nome
        End If

        'Dim BarCount As Integer = tabella.Rows.Count

        'If BarCount < 1 Then
        '    BarCount = 0
        'Else
        '    BarCount = BarCount - 1
        'End If

        'Tabella contenitore

        'Cella tabella dati

        'Dim cellaDati As New TableCell
        'cellaDati.CssClass = _ClassData
        'cellaDati.Controls.Add(tabella)

        'Dim rigaDati As New TableRow
        'rigaDati.Cells.Add(cellaDati)


        'GRafico HTML 5
        Dim isnewLine As Boolean = False
        Dim charLenght As Integer = _MaxCharacterForchartInline

        If (labelLenght > _NewLineCharacter) OrElse (labelLenght + headerLenght > _NewLineMaxHeaderChar) Then
            isnewLine = True
            charLenght = _MaxCharacterForchartNewline
        End If

        Dim oChart As RadHtmlChart

        oChart = ChartHelper.CreateBarChar(averages, labels, False, numeroRating, charLenght, chartTitle)


        Dim chartContainer As New System.Web.UI.HtmlControls.HtmlGenericControl("DIV")
        chartContainer.Controls.Add(oChart)
        chartContainer.Attributes.Add("class", "optionChart barChart")


        Dim chartClass As String = "dataContainer" 'maxLabelLenght

        If isnewLine Then
            chartClass = "dataContainer longLabel"
        End If

        Dim tdData As New TableCell
        tdData.CssClass = chartClass '"dataContainer"
        tdData.Controls.Add(tabella)
        tdData.Controls.Add(chartContainer)

        Dim trData As New TableRow
        trData.Cells.Add(tdData)

        mainTable.Rows.Add(trData)
        'Dim cellaGrafico As New TableCell
        'cellaGrafico.CssClass = "tdchart"

        'If Not IsNothing(oChart) Then
        '    cellaGrafico.Controls.Add(oChart)
        'End If


        'Dim rigaGrafico As New TableRow
        'rigaGrafico.Cells.Add(cellaGrafico)

        'mainTable.Rows.Add(rigaDati)
        'mainTable.Rows.Add(rigaGrafico)

        Return mainTable
    End Function

    'Private Function CreateTableForRatingStarsQuestion_OLD2(ByVal oDomanda As Domanda) As Table

    '    Dim averages As New List(Of Double)()
    '    Dim labels As New List(Of String)()

    '    Dim currentAvg As Double = 0


    '    Dim tabella As New Table
    '    tabella.CssClass = "datatable"

    '    Dim numeroRating As Integer = oDomanda.domandaRating.numeroRating
    '    Dim rigaInt As New TableRow

    '    If oDomanda.domandaRating.opzioniRating.Count > 1 Then
    '        Dim cellaInt1 As New TableCell
    '        rigaInt.Cells.Add(cellaInt1)
    '    End If

    '    If oDomanda.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Numerazione Then
    '        For i As Integer = 1 To oDomanda.domandaRating.numeroRating
    '            Dim cella As New TableCell
    '            cella.Text = i
    '            rigaInt.Cells.Add(cella)
    '        Next

    '    Else
    '        For Each oInt As DomandaOpzione In oDomanda.domandaRating.intestazioniRating
    '            Dim cella As New TableCell
    '            cella.Text = oInt.testo
    '            rigaInt.Cells.Add(cella)
    '        Next

    '    End If

    '    If oDomanda.domandaRating.mostraND Then
    '        Dim cellaND As New TableCell
    '        cellaND.Text = oDomanda.domandaRating.testoND
    '        cellaND.CssClass = "cellarisposte"
    '        rigaInt.Cells.Add(cellaND)
    '        numeroRating = numeroRating + 1
    '    End If

    '    Dim cellaAVGheader As New TableCell
    '    cellaAVGheader.Text = Me.Resource.getValue("VisualizzaRisposteAltro")
    '    cellaAVGheader.Text = "Avg"
    '    rigaInt.Cells.Add(cellaAVGheader)
    '    rigaInt.CssClass = "header"
    '    tabella.Rows.Add(rigaInt)

    '    Dim iOpzione As Integer = 0

    '    For Each oOpz As DomandaOpzione In oDomanda.domandaRating.opzioniRating
    '        Dim noText As Integer = 0
    '        If oOpz.testo = String.Empty Then
    '            noText = 1
    '        End If
    '        Dim riga As New TableRow

    '        Dim cella As New TableCell

    '        If oDomanda.domandaRating.opzioniRating.Count > 1 Then
    '            Dim lbTesto As New Label
    '            lbTesto.Text = SmartTagsAvailable.TagAll(oOpz.testo)
    '            cella.CssClass = "cellarisposte"
    '            cella.Controls.Add(lbTesto)

    '            labels.Add(oOpz.testo)

    '            If oOpz.isAltro Then
    '                Dim LNKVisualizzaRisposte As New LinkButton
    '                MyBase.SetCulture("pg_ucStatisticheGenerali", "Questionari")
    '                LNKVisualizzaRisposte.Text = Me.Resource.getValue("VisualizzaRisposteAltro")
    '                LNKVisualizzaRisposte.CommandName = "visualizzaRisposte"
    '                LNKVisualizzaRisposte.CommandArgument = oOpz.id
    '                cella.Controls.Add(LNKVisualizzaRisposte)
    '            End If
    '            riga.Cells.Add(cella)
    '        End If

    '        currentAvg = 0
    '        Dim currentCount As Integer = 0

    '        For c As Integer = 1 To numeroRating
    '            Dim cellaRB As New TableCell
    '            Dim count As Integer = 0
    '            For Each oRisp As RispostaDomanda In oDomanda.risposteDomanda
    '                'notext serve per visualizzare le statistiche corrette quando non c'e' un testo nell'opz. rating. se si cambia la lettura dei dati dal quest, va cambiato anche questo
    '                If oOpz.numero = oRisp.numeroOpzione And c = oRisp.valore + noText Then
    '                    count = count + 1
    '                End If
    '            Next
    '            cellaRB.Text = count
    '            Dim chartItem As New ChartSeriesItem
    '            chartItem.Appearance.FillStyle.MainColor = GetColor(iOpzione)
    '            chartItem.YValue = count
    '            riga.Cells.Add(cellaRB)
    '            currentAvg += count * c
    '            currentCount += count
    '        Next

    '        currentAvg = (currentAvg / currentCount)
    '        If (Double.IsNaN(currentAvg)) Then
    '            currentAvg = 0
    '        End If
    '        averages.Add(currentAvg)

    '        Dim cellaAvg As New TableCell()
    '        cellaAvg.Text = currentAvg.ToString(_FormatAvg)
    '        riga.Cells.Add(cellaAvg)

    '        MyBase.SetCulture("pg_ucStatisticheGenerali", "Questionari")

    '        If (iOpzione Mod 2 > 0) Then
    '            riga.CssClass = "alternate"
    '        End If
    '        tabella.Rows.Add(riga)
    '        iOpzione = iOpzione + 1

    '    Next

    '    Dim chartTitle As String = ""

    '    If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
    '        chartTitle = Me.QuestionarioCorrente.nome
    '    End If



    '    'Tabella contenitore

    '    'Cella tabella dati

    '    Dim cellaDati As New TableCell
    '    cellaDati.CssClass = _ClassData
    '    cellaDati.Controls.Add(tabella)

    '    Dim rigaDati As New TableRow
    '    rigaDati.Cells.Add(cellaDati)


    '    'GRafico HTML 5

    '    Dim oChart As RadHtmlChart = ChartHelper.CreateBarChar(averages, labels, False, numeroRating, _MaxCharacterForchartNewline, chartTitle)

    '    Dim cellaGrafico As New TableCell
    '    cellaGrafico.CssClass = "tdchart"
    '    cellaGrafico.Controls.Add(oChart)

    '    Dim rigaGrafico As New TableRow
    '    rigaGrafico.Cells.Add(cellaGrafico)



    '    'Main:

    '    Dim tabellaMain As New Table
    '    tabellaMain.CssClass = "ratingquestion statistics"

    '    tabellaMain.Rows.Add(rigaDati)
    '    tabellaMain.Rows.Add(rigaGrafico)


    '    Return tabellaMain
    'End Function
    'Private Function CreateTableForRatingQuestion_OLD(ByVal oDomanda As Domanda) As Table
    '    Dim oGrafico As New RadChart
    '    oGrafico.DefaultType = ChartSeriesType.Line
    '    oGrafico.ChartTitle.TextBlock.Visible = False
    '    oGrafico.Skin = "Office2007"
    '    oGrafico.AutoLayout = True
    '    oGrafico.Width = "880"
    '    oGrafico.PlotArea.YAxis.Step = 1

    '    Dim cellaGrafico As New TableCell
    '    cellaGrafico.CssClass = "answergraphic"
    '    'creo la tabella per le risposte rating

    '    Dim tabella As New Table
    '    tabella.CssClass = "ratingquestion statistics"
    '    tabella.BorderWidth = 1
    '    tabella.GridLines = GridLines.Horizontal
    '    Dim oOpzioniRating As New List(Of DomandaOpzione)
    '    Dim numeroRating As Integer = oDomanda.domandaRating.numeroRating
    '    Dim rigaInt As New TableRow

    '    If oDomanda.domandaRating.opzioniRating.Count > 1 Then
    '        ' cella vuota intestazione
    '        Dim cellaInt1 As New TableCell
    '        cellaInt1.ColumnSpan = 2
    '        rigaInt.Cells.Add(cellaInt1)
    '    End If

    '    If oDomanda.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Numerazione Then
    '        For i As Integer = 1 To oDomanda.domandaRating.numeroRating
    '            Dim cella As New TableCell
    '            cella.HorizontalAlign = HorizontalAlign.Center
    '            cella.Text = i
    '            rigaInt.Cells.Add(cella)
    '        Next

    '    Else
    '        For Each oInt As DomandaOpzione In oDomanda.domandaRating.intestazioniRating
    '            Dim cella As New TableCell
    '            cella.HorizontalAlign = HorizontalAlign.Center
    '            cella.Text = oInt.testo
    '            rigaInt.Cells.Add(cella)
    '        Next

    '    End If

    '    If oDomanda.domandaRating.mostraND Then
    '        Dim cellaND As New TableCell
    '        cellaND.HorizontalAlign = HorizontalAlign.Left
    '        cellaND.Text = oDomanda.domandaRating.testoND
    '        rigaInt.Cells.Add(cellaND)
    '        numeroRating = numeroRating + 1
    '    End If

    '    tabella.Rows.Add(rigaInt)

    '    Dim iOpzione As Integer = 0
    '    Dim datiGrafico As Integer(,)
    '    For Each oOpz As DomandaOpzione In oDomanda.domandaRating.opzioniRating
    '        Dim noText As Integer = 0
    '        If oOpz.testo = String.Empty Then
    '            noText = 1
    '        End If
    '        Dim riga As New TableRow

    '        Dim cella As New TableCell

    '        If oDomanda.domandaRating.opzioniRating.Count > 1 Then
    '            Dim lbTesto As New Label
    '            lbTesto.Text = SmartTagsAvailable.TagAll(oOpz.testo)
    '            cella.Controls.Add(lbTesto)

    '            If oOpz.isAltro Then
    '                Dim LNKVisualizzaRisposte As New LinkButton
    '                MyBase.SetCulture("pg_ucStatisticheGenerali", "Questionari")
    '                LNKVisualizzaRisposte.Text = Me.Resource.getValue("VisualizzaRisposteAltro")
    '                LNKVisualizzaRisposte.CommandName = "visualizzaRisposte"
    '                LNKVisualizzaRisposte.CommandArgument = oOpz.id
    '                cella.Controls.Add(LNKVisualizzaRisposte)
    '            End If

    '            cella.HorizontalAlign = HorizontalAlign.Left

    '            Dim cellaColore As New TableCell
    '            cellaColore.BackColor = GetColor(iOpzione)
    '            cellaColore.ForeColor = GetColor(iOpzione)
    '            cellaColore.Text = "....."

    '            riga.Cells.Add(cellaColore)
    '            riga.Cells.Add(cella)
    '        End If

    '        'cella.Text = oOpz.testo

    '        Dim serie As New ChartSeries

    '        For c As Integer = 1 To numeroRating
    '            Dim cellaRB As New TableCell
    '            cellaRB.HorizontalAlign = HorizontalAlign.Left
    '            'cellaRB.Height = 80%
    '            Dim count As Integer = 0
    '            For Each oRisp As RispostaDomanda In oDomanda.risposteDomanda
    '                'notext serve per visualizzare le statistiche corrette quando non c'e' un testo nell'opz. rating. se si cambia la lettura dei dati dal quest, va cambiato anche questo
    '                If oOpz.numero = oRisp.numeroOpzione And c = oRisp.valore + noText Then
    '                    count = count + 1
    '                End If
    '            Next
    '            cellaRB.Text = count
    '            cellaRB.HorizontalAlign = HorizontalAlign.Center
    '            Dim chartItem As New ChartSeriesItem
    '            chartItem.Appearance.FillStyle.MainColor = GetColor(iOpzione)
    '            'chartItem.MainColor = GetColor(iOpzione)
    '            chartItem.YValue = count '+ (0.01 * iOpzione)
    '            serie.Items.Add(chartItem)
    '            riga.Cells.Add(cellaRB)
    '        Next

    '        MyBase.SetCulture("pg_ucStatisticheGenerali", "Questionari")
    '        serie.Name = Me.Resource.getValue("opzioneLegenda") & oOpz.numero
    '        serie.Type = ChartSeriesType.Line
    '        serie.Appearance.FillStyle.MainColor = GetColor(iOpzione)

    '        oGrafico.Series.Add(serie)
    '        tabella.Rows.Add(riga)

    '        iOpzione = iOpzione + 1
    '    Next

    '    Dim j As Integer = 0

    '    If oDomanda.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Testi Then
    '        oGrafico.PlotArea.XAxis.AutoScale = False
    '        oGrafico.PlotArea.XAxis.AddRange(0, oDomanda.domandaRating.intestazioniRating.Count - 1, 1)
    '        For j = 0 To oDomanda.domandaRating.intestazioniRating.Count - 1
    '            oGrafico.PlotArea.XAxis(j).TextBlock.Text = oDomanda.domandaRating.intestazioniRating(j).testo
    '        Next
    '    End If

    '    oGrafico.PlotArea.XAxis.Appearance.Color = Color.Black
    '    oGrafico.PlotArea.YAxis.Appearance.Color = Color.Black
    '    'oGrafico.Margins.Right = 90 * Ceiling((iOpzione / 13))
    '    oGrafico.Legend.Visible = False
    '    cellaGrafico.Controls.Add(oGrafico)
    '    cellaGrafico.RowSpan = oDomanda.domandaRating.opzioniRating.Count + 1
    '    oGrafico.PlotArea.YAxis.Step = 1
    '    Dim cellaVuota As New TableCell
    '    tabella.Rows(0).Cells.Add(cellaVuota)
    '    Dim rigaGrafico As New TableRow
    '    cellaGrafico.ColumnSpan = numeroRating + 2
    '    rigaGrafico.Cells.Add(cellaGrafico)
    '    tabella.Rows.Add(rigaGrafico)
    '    'oGrafico.Clear()

    '    Return tabella
    'End Function



    Private Function CreateTableForRatingStarsQuestion_OLD(ByVal oDomanda As Domanda) As Table
        Dim oGrafico As New RadChart
        oGrafico.DefaultType = ChartSeriesType.Line
        oGrafico.ChartTitle.TextBlock.Visible = False
        oGrafico.Skin = "Office2007"
        oGrafico.AutoLayout = True
        oGrafico.Width = "880"
        oGrafico.PlotArea.YAxis.Step = 1

        Dim cellaGrafico As New TableCell
        cellaGrafico.CssClass = "answergraphic"
        'creo la tabella per le risposte rating

        Dim tabella As New Table
        tabella.CssClass = "ratingquestion statistics"
        tabella.BorderWidth = 1
        Dim oOpzioniRating As New List(Of DomandaOpzione)
        Dim numeroRating As Integer = oDomanda.domandaRating.numeroRating
        Dim rigaInt As New TableRow

        If oDomanda.domandaRating.opzioniRating.Count > 1 Then
            ' cella vuota intestazione
            Dim cellaInt1 As New TableCell
            cellaInt1.ColumnSpan = 2
            rigaInt.Cells.Add(cellaInt1)
        End If

        'If oDomanda.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Numerazione Then
        For i As Integer = 1 To oDomanda.domandaRating.numeroRating
            Dim cella As New TableCell
            cella.Text = i
            rigaInt.Cells.Add(cella)
        Next

        'Else
        'For Each oInt As DomandaOpzione In oDomanda.domandaRating.intestazioniRating
        '    Dim cella As New TableCell
        '    cella.HorizontalAlign = HorizontalAlign.Center
        '    cella.Text = oInt.testo
        '    rigaInt.Cells.Add(cella)
        'Next

        'End If

        If oDomanda.domandaRating.mostraND Then
            Dim cellaND As New TableCell
            cellaND.Text = oDomanda.domandaRating.testoND
            rigaInt.Cells.Add(cellaND)
            numeroRating = numeroRating + 1
        End If

        tabella.Rows.Add(rigaInt)

        Dim iOpzione As Integer = 0
        Dim datiGrafico As Integer(,)
        For Each oOpz As DomandaOpzione In oDomanda.domandaRating.opzioniRating
            Dim noText As Integer = 0
            If oOpz.testo = String.Empty Then
                noText = 1
            End If
            Dim riga As New TableRow

            Dim cella As New TableCell

            If oDomanda.domandaRating.opzioniRating.Count > 1 Then
                Dim lbTesto As New Label
                lbTesto.Text = SmartTagsAvailable.TagAll(oOpz.testo)
                cella.Controls.Add(lbTesto)

                If oOpz.isAltro Then
                    Dim LNKVisualizzaRisposte As New LinkButton
                    MyBase.SetCulture("pg_ucStatisticheGenerali", "Questionari")
                    LNKVisualizzaRisposte.Text = Me.Resource.getValue("VisualizzaRisposteAltro")
                    LNKVisualizzaRisposte.CommandName = "visualizzaRisposte"
                    LNKVisualizzaRisposte.CommandArgument = oOpz.id
                    cella.Controls.Add(LNKVisualizzaRisposte)
                End If

                Dim cellaColore As New TableCell
                cellaColore.BackColor = GetColor(iOpzione)
                cellaColore.ForeColor = GetColor(iOpzione)
                cellaColore.Text = "....."

                riga.Cells.Add(cellaColore)
                riga.Cells.Add(cella)
            End If

            'cella.Text = oOpz.testo

            Dim serie As New ChartSeries

            For c As Integer = 1 To numeroRating
                Dim cellaRB As New TableCell
                'cellaRB.Height = 80%
                Dim count As Integer = 0
                For Each oRisp As RispostaDomanda In oDomanda.risposteDomanda
                    'notext serve per visualizzare le statistiche corrette quando non c'e' un testo nell'opz. rating. se si cambia la lettura dei dati dal quest, va cambiato anche questo
                    If oOpz.numero = oRisp.numeroOpzione And c = oRisp.valore + noText Then
                        count = count + 1
                    End If
                Next
                cellaRB.Text = count
                Dim chartItem As New ChartSeriesItem
                chartItem.Appearance.FillStyle.MainColor = GetColor(iOpzione)
                'chartItem.MainColor = GetColor(iOpzione)
                chartItem.YValue = count '+ (0.01 * iOpzione)
                serie.Items.Add(chartItem)
                riga.Cells.Add(cellaRB)
            Next

            MyBase.SetCulture("pg_ucStatisticheGenerali", "Questionari")
            serie.Name = Me.Resource.getValue("opzioneLegenda") & oOpz.numero
            serie.Type = ChartSeriesType.Line
            serie.Appearance.FillStyle.MainColor = GetColor(iOpzione)

            oGrafico.Series.Add(serie)
            tabella.Rows.Add(riga)

            iOpzione = iOpzione + 1
        Next

        Dim j As Integer = 0

        If oDomanda.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Testi Then
            oGrafico.PlotArea.XAxis.AutoScale = False
            oGrafico.PlotArea.XAxis.AddRange(0, oDomanda.domandaRating.intestazioniRating.Count - 1, 1)
            For j = 0 To oDomanda.domandaRating.intestazioniRating.Count - 1
                oGrafico.PlotArea.XAxis(j).TextBlock.Text = oDomanda.domandaRating.intestazioniRating(j).testo
            Next
        End If

        oGrafico.PlotArea.XAxis.Appearance.Color = Color.Black
        oGrafico.PlotArea.YAxis.Appearance.Color = Color.Black
        'oGrafico.Margins.Right = 90 * Ceiling((iOpzione / 13))
        oGrafico.Legend.Visible = False
        cellaGrafico.Controls.Add(oGrafico)
        cellaGrafico.RowSpan = oDomanda.domandaRating.opzioniRating.Count + 1
        oGrafico.PlotArea.YAxis.Step = 1
        Dim cellaVuota As New TableCell
        tabella.Rows(0).Cells.Add(cellaVuota)
        Dim rigaGrafico As New TableRow
        cellaGrafico.ColumnSpan = numeroRating + 2
        rigaGrafico.Cells.Add(cellaGrafico)
        tabella.Rows.Add(rigaGrafico)
        'oGrafico.Clear()

        Return tabella
    End Function

    Private Function creaTabellaMeeting(ByVal oDomanda As Domanda) As Table

        'Dim oGrafico As New RadChart
        'oGrafico.DefaultType = ChartSeriesType.Bar
        'oGrafico.ChartTitle.TextBlock.Visible = False
        'oGrafico.Skin = "Office2007"
        'oGrafico.AutoLayout = True
        'oGrafico.Width = 800
        'oGrafico.PlotArea.YAxis.Step = 1

        'Dim cellaGrafico As New TableCell

        'creo la tabella per le risposte rating
        Dim tabella As New Table
        'tabella.Width = 800
        'tabella.BorderWidth = 1
        'tabella.GridLines = GridLines.Horizontal
        'Dim oOpzioniRating As New List(Of DomandaOpzione)
        'Dim numeroRating As Integer = oDomanda.domandaRating.numeroRating
        'Dim rigaInt As New TableRow

        'If oDomanda.domandaRating.opzioniRating.Count > 1 Then
        '    ' cella vuota intestazione
        '    Dim cellaInt1 As New TableCell
        '    cellaInt1.ColumnSpan = 2
        '    rigaInt.Cells.Add(cellaInt1)
        'End If


        'For Each oInt As DomandaOpzione In oDomanda.domandaRating.intestazioniRating
        '    Dim cella As New TableCell
        '    cella.HorizontalAlign = HorizontalAlign.Center
        '    cella.Text = oInt.testo
        '    rigaInt.Cells.Add(cella)
        'Next

        'tabella.Rows.Add(rigaInt)

        'Dim iOpzione As Integer = 0
        ''Dim datiGrafico As Integer(,)
        'For Each oOpz As DomandaOpzione In oDomanda.domandaRating.opzioniRating

        '    Dim riga As New TableRow

        '    Dim cella As New TableCell

        '    If oDomanda.domandaRating.opzioniRating.Count > 1 Then
        '        Dim lbTesto As New Label
        '        lbTesto.Text = SmartTagsAvailable.TagAll(oOpz.testo)
        '        cella.Controls.Add(lbTesto)
        '        cella.HorizontalAlign = HorizontalAlign.Left
        '        Dim cellaColore As New TableCell
        '        cellaColore.BackColor = GetColor(iOpzione)
        '        cellaColore.ForeColor = GetColor(iOpzione)
        '        cellaColore.Text = "....."
        '        riga.Cells.Add(cellaColore)
        '        riga.Cells.Add(cella)
        '    End If

        'Dim serie As New ChartSeries


        'For c As Integer = 1 To numeroRating

        '    Dim cellaRB As New TableCell
        '    cellaRB.HorizontalAlign = HorizontalAlign.Left
        '    Dim count = (From ri In oDomanda.risposteDomanda Where ri.valore = c And ri.idDomandaOpzione = oOpz.id Select ri.id).ToList.Count
        '    cellaRB.Text = count
        '    cellaRB.HorizontalAlign = HorizontalAlign.Center
        '    Dim chartItem As New ChartSeriesItem
        '    chartItem.Appearance.FillStyle.MainColor = GetColor(iOpzione)
        '    'chartItem.MainColor = GetColor(iOpzione)
        '    chartItem.YValue = count '+ (0.01 * iOpzione)
        '    serie.Items.Add(chartItem)
        '    riga.Cells.Add(cellaRB)
        'Next

        'MyBase.SetCulture("pg_ucStatisticheGenerali", "Questionari")
        'serie.Name = Me.Resource.getValue("opzioneLegenda") & oOpz.numero
        'serie.Type = ChartSeriesType.Bar
        'serie.Appearance.FillStyle.MainColor = GetColor(iOpzione)

        'oGrafico.Series.Add(serie)
        'tabella.Rows.Add(riga)

        'iOpzione = iOpzione + 1
        'Next

        'Dim j As Integer = 0

        'If oDomanda.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Testi Then
        '    oGrafico.PlotArea.XAxis.AutoScale = False
        '    oGrafico.PlotArea.XAxis.AddRange(0, oDomanda.domandaRating.intestazioniRating.Count - 1, 1)
        '    For j = 0 To oDomanda.domandaRating.intestazioniRating.Count - 1
        '        oGrafico.PlotArea.XAxis(j).TextBlock.Text = oDomanda.domandaRating.intestazioniRating(j).testo
        '    Next
        'End If

        'oGrafico.PlotArea.XAxis.Appearance.Color = Color.Black
        'oGrafico.PlotArea.YAxis.Appearance.Color = Color.Black
        ''oGrafico.Margins.Right = 90 * Ceiling((iOpzione / 13))
        'oGrafico.Legend.Visible = False
        'cellaGrafico.Controls.Add(oGrafico)
        'cellaGrafico.RowSpan = oDomanda.domandaRating.opzioniRating.Count + 1
        'oGrafico.PlotArea.YAxis.Step = 1
        'Dim cellaVuota As New TableCell
        'tabella.Rows(0).Cells.Add(cellaVuota)
        'Dim rigaGrafico As New TableRow
        'cellaGrafico.ColumnSpan = numeroRating + 2
        'rigaGrafico.Cells.Add(cellaGrafico)
        'tabella.Rows.Add(rigaGrafico)
        Return tabella
    End Function
    Private Function CreateTableForFreeTextQuestion(ByVal question As Domanda) As Table
        Dim tabella As New Table
        tabella.CssClass = "freetextquestion statistics notNested"
        For Each oOpz As DomandaTestoLibero In question.opzioniTestoLibero
            Dim row As New TableRow
            Dim cell As New TableCell


            Dim oDiv As New HtmlGenericControl("div")
            oDiv.Attributes.Add("class", "inlinewrapper")
            Dim oLabel As New Label

            oLabel.CssClass = "answer renderedtext"
            oLabel.Text = SmartTagsAvailable.TagAll(oOpz.etichetta) + "  "
            oDiv.Controls.Add(oLabel)
            cell.CssClass = "optiontext"
            cell.Controls.Add(oDiv)
            row.Cells.Add(cell)

            Dim cellOther As New TableCell
            Dim oLinkButton As New LinkButton
            MyBase.SetCulture("pg_ucStatisticheGenerali", "Questionari")
            oLinkButton.Text = Me.Resource.getValue("VisualizzaRisposteAltro")
            oLinkButton.CommandName = "visualizzaRisposte"
            oLinkButton.CommandArgument = oOpz.id
            cellOther.CssClass = "displayoptionanwsers"
            cellOther.Controls.Add(oLinkButton)
            row.Cells.Add(cellOther)
            tabella.Rows.Add(row)
        Next

        Return tabella
    End Function
    Private Function CreateTableForNumericQuestion(ByVal oDomanda As Domanda) As Table
        Dim tabella As New Table
        tabella.CssClass = "numericquestion statistics notNested"
        For Each oOpz As DomandaNumerica In oDomanda.opzioniNumerica
            Dim row As New TableRow
            Dim cell As New TableCell
            Dim LNKVisualizzaRisposte As New LinkButton
            Dim oDiv As New HtmlGenericControl("div")
            oDiv.Attributes.Add("class", "inlinewrapper")
            Dim oLabel As New Label

            oLabel.CssClass = "answer renderedtext"
            oLabel.Text = oOpz.testoPrima 'String.Format("{0} ({1})", SmartTagsAvailable.TagAll(oOpz.testoPrima), oOpz.numeroRisposte)
            oDiv.Controls.Add(oLabel)
            cell.CssClass = "optiontext"
            cell.Controls.Add(oDiv)
            row.Cells.Add(cell)

            cell = New TableCell()
            MyBase.SetCulture("pg_ucStatisticheGenerali", "Questionari")
            LNKVisualizzaRisposte.Text = Me.Resource.getValue("VisualizzaRisposteAltro")
            LNKVisualizzaRisposte.OnClientClick = "visualizzaRisposte"
            LNKVisualizzaRisposte.CommandArgument = oOpz.id
            cell.CssClass = "displayoptionanwsers"
            cell.Controls.Add(LNKVisualizzaRisposte)

            row.Cells.Add(cell)
            tabella.Rows.Add(row)
        Next
        Return tabella
    End Function
#End Region

    Public Function calcoloPunteggioByQuestionario(ByRef oQuestionario As Questionario, ByRef oStat As Statistica, Optional ByRef scalaValutazione As Integer = 0) As Statistica
        Dim oRisposta As New RispostaQuestionario
        oRisposta = oQuestionario.rispostaQuest

        If scalaValutazione = 0 Then
            If oQuestionario.scalaValutazione = 0 Then
                scalaValutazione = RootObject.scalaValutazione
            Else
                scalaValutazione = oQuestionario.scalaValutazione
            End If
        End If

        If oQuestionario.rispostaQuest.dataFine = Date.MaxValue.ToString() _
            OrElse oQuestionario.rispostaQuest.dataFine = Date.MinValue.ToString() Then
            oStat.isFinito = False
        Else
            oStat.isFinito = True
        End If
        oStat.reset()

        For Each oDomanda As Domanda In oQuestionario.domande
            If oDomanda.isValutabile Then
                oStat.pesoTotaleDomande = oStat.pesoTotaleDomande + oDomanda.peso
                Dim rispostaTrovata As New RispostaDomanda
                Select Case oDomanda.tipo
                    Case Domanda.TipoDomanda.DropDown
                        Dim isValutabile As Boolean = False
                        Dim risposteDomanda As List(Of RispostaDomanda) = oQuestionario.rispostaQuest.findRisposteByIDDomanda(oDomanda.risposteDomanda, oDomanda.id)
                        For Each ddItem As DropDownItem In oDomanda.domandaDropDown.dropDownItems
                            If ddItem.isCorretta = True Then
                                isValutabile = True
                                Exit For
                            End If
                        Next
                        If isValutabile Then
                            If risposteDomanda.Count > 0 And oDomanda.domandaDropDown.dropDownItems.Count > 0 Then
                                If oDomanda.domandaDropDown.dropDownItems(risposteDomanda(0).numeroOpzione - 1).isCorretta Then
                                    oStat.nRisposteCorrette = oStat.nRisposteCorrette + 1
                                Else
                                    oStat.nRisposteErrate += 1
                                End If
                                oStat.punteggio = oStat.punteggio + ((oDomanda.peso * oDomanda.domandaDropDown.dropDownItems(risposteDomanda(0).numeroOpzione - 1).peso / 100))
                            End If
                        Else
                            oStat.nRisposteNonValutate += 1
                        End If
                    Case Domanda.TipoDomanda.Multipla
                        Dim nOpzioniErrate As Int16 = 0
                        Dim nOpzioniCorrette As Int16 = 0
                        Dim isValutabile As Boolean = False
                        Dim punteggioDomanda As Decimal = 0
                        'Dim isSaltata As Boolean = True
                        Dim risposteDomanda As List(Of RispostaDomanda) = oQuestionario.rispostaQuest.findRisposteByIDDomanda(oDomanda.risposteDomanda, oDomanda.id)

                        For Each opzione As DomandaOpzione In oDomanda.domandaMultiplaOpzioni
                            'se nessuna opzione e' selezionata come corretta la risposta non e' valutabile
                            If opzione.isCorretta Then
                                isValutabile = True
                                Exit For
                            End If
                        Next
                        For Each risposta As RispostaDomanda In risposteDomanda
                            If oDomanda.domandaMultiplaOpzioni(risposta.numeroOpzione - 1).isCorretta Then
                                nOpzioniCorrette += 1
                            Else
                                nOpzioniErrate += 1
                            End If
                            punteggioDomanda += oDomanda.domandaMultiplaOpzioni(risposta.numeroOpzione - 1).peso / 100
                        Next
                        If isValutabile Then
                            oStat.punteggio += punteggioDomanda * oDomanda.peso
                            If nOpzioniErrate = 0 Then
                                If nOpzioniCorrette = 0 Then
                                    oStat.nRisposteSaltate += 1
                                Else
                                    If punteggioDomanda >= 1 Then
                                        oStat.nRisposteCorrette += 1
                                    Else
                                        oStat.nRisposteParzialmenteCorrette += 1
                                    End If
                                End If
                            Else
                                If nOpzioniCorrette = 0 Then
                                    oStat.nRisposteErrate += 1
                                Else
                                    oStat.nRisposteParzialmenteCorrette += 1
                                End If
                            End If
                        Else
                            oStat.nRisposteNonValutate += 1
                        End If
                    Case Domanda.TipoDomanda.Numerica
                        Dim nCorrette As Int16 = 0
                        Dim risposteDomanda As List(Of RispostaDomanda) = oQuestionario.rispostaQuest.findRisposteByIDDomanda(oDomanda.risposteDomanda, oDomanda.id)
                        For Each risposta As RispostaDomanda In risposteDomanda
                            If oDomanda.opzioniNumerica(risposta.numeroOpzione - 1).rispostaCorretta = risposta.valore Then
                                oStat.punteggio = oStat.punteggio + ((oDomanda.peso * oDomanda.opzioniNumerica(risposta.numeroOpzione - 1).peso / 100))
                                nCorrette += 1
                            End If
                        Next
                        If risposteDomanda.Count = 0 Then
                            oStat.nRisposteSaltate += 1
                            'If nCorrette = oDomanda.numeroRisposteDomanda Then
                        ElseIf nCorrette = oDomanda.opzioniNumerica.Count Then
                            oStat.nRisposteCorrette = oStat.nRisposteCorrette + 1
                        ElseIf nCorrette = 0 Then
                            oStat.nRisposteErrate += 1
                        Else
                            oStat.nRisposteParzialmenteCorrette += 1
                        End If
                    Case Domanda.TipoDomanda.TestoLibero
                        Dim punteggioDomanda As Decimal = 0
                        Dim risposteDomanda As List(Of RispostaDomanda) = oQuestionario.rispostaQuest.findRisposteByIDDomanda(oDomanda.risposteDomanda, oDomanda.id)
                        Dim nOpzioniCorrette As Int16 = 0
                        Dim nOpzioniErrate As Int16 = 0
                        Dim pesoOpzioni As Decimal = 0
                        Dim isParzialmenteCorretta As Boolean = False 'senza questo non distinguo le parziali dalle saltate
                        For Each risposta As RispostaDomanda In risposteDomanda
                            If risposta.valutazione Is Nothing Then
                                oStat.nRisposteNonValutate += 1
                                Exit Select
                            Else
                                If risposta.valutazione.Trim(" ") = String.Empty Then
                                    oStat.nRisposteNonValutate += 1
                                    Exit Select
                                Else
                                    Select Case CInt(risposta.valutazione)
                                        Case Is >= 100
                                            If nOpzioniErrate > 0 Then
                                                isParzialmenteCorretta = True
                                            End If
                                            nOpzioniCorrette += 1
                                        Case Is <= 0
                                            If nOpzioniCorrette > 0 Then
                                                isParzialmenteCorretta = True
                                            End If
                                            nOpzioniErrate += 1
                                        Case Else
                                            isParzialmenteCorretta = True
                                    End Select
                                End If
                            End If
                            oStat.punteggio += CInt(risposta.valutazione) * oDomanda.opzioniTestoLibero(Max(0, (risposta.numeroOpzione - 1))).peso / 10000 '(100 per il peso, 100 per la valutazione)
                        Next

                        If isParzialmenteCorretta Then
                            oStat.nRisposteParzialmenteCorrette += 1
                        ElseIf nOpzioniCorrette > 0 Then
                            oStat.nRisposteCorrette += 1
                        ElseIf nOpzioniErrate > 0 Then
                            oStat.nRisposteErrate += 1
                        Else
                            oStat.nRisposteSaltate += 1
                        End If
                    Case Else
                        oStat.nRisposteNonValutate += 1
                End Select
                oStat.nRisposteTotali = oStat.nRisposteTotali + 1
            Else
                Dim risposteDomanda As List(Of RispostaDomanda) = oQuestionario.rispostaQuest.findRisposteByIDDomanda(oDomanda.risposteDomanda, oDomanda.id)
                If risposteDomanda.Count = 0 Then
                    oStat.nRisposteSaltate += 1
                Else
                    oStat.nRisposteNonValutate += 1
                End If
                oStat.nRisposteTotali += 1
            End If
            oStat.coeffDifficolta = (oStat.coeffDifficolta * (oStat.nRisposteTotali - 1) + oDomanda.difficolta) / oStat.nRisposteTotali
        Next
        If Not oStat.pesoTotaleDomande * scalaValutazione = 0 And Not oStat.nRisposteTotali = oStat.nRisposteNonValutate Then
            oStat.punteggio = oStat.punteggio / oStat.pesoTotaleDomande * scalaValutazione
            oStat.punteggioRelativo = oStat.punteggio * oStat.nRisposteTotali / (oStat.nRisposteTotali - oStat.nRisposteNonValutate)
        End If
        Return oStat
    End Function
    Public Function calcoloPunteggioAutovalutazione(ByRef oQuestionario As Questionario) As Statistica
        Dim contatore As Integer = -1
        Dim oRisposta As New RispostaQuestionario
        oRisposta = oQuestionario.rispostaQuest

        Dim oStat As New Statistica
        oStat.idPersona = oQuestionario.idDestinatario_Persona
        Return calcoloPunteggioByQuestionario(oQuestionario, oStat, oQuestionario.scalaValutazione)
    End Function
    Public Function calcoloPunteggio(ByVal oPersone As List(Of UtenteInvitato)) As List(Of UtenteInvitato)
        Dim contatore As Integer = -1
        For Each oPersona As UtenteInvitato In oPersone
            Dim oRisposta As New RispostaQuestionario
            oRisposta = Me.QuestionarioCorrente.rispostaQuest
            Dim oStat As New Statistica

            oStat.idPersona = oPersona.ID
            oStat.nomeUtente = oPersona.Nome
            oPersona.statistica = calcoloPunteggioByQuestionario(Me.QuestionarioCorrente, oStat) ', scalaValutazione)
        Next
        Return oPersone
    End Function
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
    Private Function GetColor(ByVal Index As Integer) As Drawing.Color
        Dim Color As Drawing.Color

        Select Case Index
            Case 0
                Color = Color.Gray '
            Case 1
                Color = Color.IndianRed '
            Case 2
                Color = Color.Green '
            Case 3
                Color = Color.Violet '
            Case 4
                Color = Color.Yellow '
            Case 5
                Color = Color.Blue '
            Case 6
                Color = Color.Aqua '
            Case 7
                Color = Color.LightCoral '
            Case 8
                Color = Color.Brown '
            Case 9
                Color = Color.Indigo '
            Case 10
                Color = Color.DodgerBlue '
            Case 11
                Color = Color.Red '
            Case 12
                Color = Color.SkyBlue '
            Case 13
                Color = Color.LightPink '
            Case 14
                Color = Color.Gold '
            Case 15
                Color = Color.PaleGoldenrod '
            Case 16
                Color = Color.LightGray  '<- !!! in html lightgray diventa lightgrEy!!! A differenza di tutto il resto del mondo... -ARGH!-
            Case 17
                Color = Color.Orange '
            Case Else
                Color = Color.IndianRed
        End Select
        Return Color
    End Function
    Public Overrides Sub SetControlliByPermessi()

    End Sub
    'Public Function setRisposteUtenti(ByVal oQuest As Questionario, ByVal DLPagine As DataList) As DataList

    '    For Each itemP As DataListItem In DLPagine.Items

    '        Dim dlDomande As New DataList
    '        dlDomande = DLPagine.Controls(itemP.ItemIndex).FindControl("DLDomande")

    '        For Each itemD As DataListItem In dlDomande.Items

    '            Dim idDomanda As String = dlDomande.DataKeys.Item(itemD.ItemIndex).ToString
    '            Dim oDomanda As New Domanda
    '            oDomanda = oDomanda.findDomandaBYID(Me.QuestionarioCorrente.domande, idDomanda)
    '            Dim oRisposta As New RispostaQuestionario

    '            Select Case oDomanda.tipo
    '                Case Domanda.TipoDomanda.Multipla And Not oDomanda.isMultipla
    '                    Dim tabella As New Table
    '                    tabella = FindControlRecursive(itemD, "TBLDomandaSingola_" + itemD.ItemIndex.ToString())
    '                    Dim iOpzione As Integer = 0
    '                    For Each row As TableRow In tabella.Rows
    '                        Dim opzioneSelezionata As Boolean
    '                        Dim testoIsAltro As String
    '                        Dim ris As New RispostaDomanda
    '                        ris = oRisposta.findRispostaByIDDomandaOpzione(oDomanda.risposteDomanda, oDomanda.domandaMultiplaOpzioni(iOpzione).id)
    '                        For Each cell As TableCell In row.Cells
    '                            For Each ctrl As Control In cell.Controls
    '                                If ctrl.GetType() Is GetType(RadioButton) Then
    '                                    Dim rbOpzione As New RadioButton
    '                                    rbOpzione = DirectCast(ctrl, RadioButton)
    '                                    If Not ris Is Nothing Then
    '                                        If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
    '                                            rbOpzione.Checked = True
    '                                        End If
    '                                    End If
    '                                    rbOpzione.Enabled = Me.QuestionarioCorrente.editaRisposta
    '                                End If
    '                                If ctrl.GetType() Is GetType(TextBox) Then
    '                                    Dim txbOpzione As New TextBox
    '                                    txbOpzione = DirectCast(ctrl, TextBox)
    '                                    If Not ris Is Nothing Then
    '                                        If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
    '                                            txbOpzione.Text = ris.testoOpzione
    '                                        End If
    '                                    End If
    '                                    txbOpzione.Enabled = Me.QuestionarioCorrente.editaRisposta
    '                                End If
    '                            Next
    '                        Next
    '                        iOpzione = iOpzione + 1
    '                    Next

    '                Case Domanda.TipoDomanda.Multipla And oDomanda.isMultipla
    '                    Dim tabella As New Table
    '                    tabella = FindControlRecursive(itemD, "TBLDomandaMultipla_" + itemD.ItemIndex.ToString())
    '                    Dim iOpzione As Integer = 0
    '                    For Each row As TableRow In tabella.Rows
    '                        Dim opzioneSelezionata As Boolean
    '                        Dim testoIsAltro As String
    '                        Dim ris As New RispostaDomanda
    '                        ris = oRisposta.findRispostaByIDDomandaOpzione(oDomanda.risposteDomanda, oDomanda.domandaMultiplaOpzioni(iOpzione).id)
    '                        For Each cell As TableCell In row.Cells
    '                            For Each ctrl As Control In cell.Controls
    '                                If ctrl.GetType() Is GetType(CheckBox) Then
    '                                    Dim rbOpzione As New CheckBox
    '                                    rbOpzione = DirectCast(ctrl, CheckBox)
    '                                    If Not ris Is Nothing Then
    '                                        If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
    '                                            rbOpzione.Checked = True
    '                                        End If
    '                                    End If
    '                                    rbOpzione.Enabled = Me.QuestionarioCorrente.editaRisposta
    '                                End If
    '                                If ctrl.GetType() Is GetType(TextBox) Then
    '                                    Dim txbOpzione As New TextBox
    '                                    txbOpzione = DirectCast(ctrl, TextBox)
    '                                    If Not ris Is Nothing Then
    '                                        If ris.numeroOpzione = oDomanda.domandaMultiplaOpzioni(iOpzione).numero Then
    '                                            txbOpzione.Text = ris.testoOpzione
    '                                        End If
    '                                    End If
    '                                    txbOpzione.Enabled = Me.QuestionarioCorrente.editaRisposta
    '                                End If
    '                            Next
    '                        Next

    '                        iOpzione = iOpzione + 1
    '                    Next

    '                Case Domanda.TipoDomanda.DropDown
    '                    Dim dlOpzioni As New DropDownList
    '                    dlOpzioni = FindControlRecursive(itemD, "DDLOpzioni")

    '                    Dim risposteDomanda As List(Of RispostaDomanda) = oRisposta.findRisposteByIDDomanda(oDomanda.risposteDomanda, idDomanda)

    '                    If risposteDomanda.Count > 0 Then
    '                        dlOpzioni.SelectedValue = risposteDomanda(0).numeroOpzione
    '                    End If
    '                    dlOpzioni.Enabled = Me.QuestionarioCorrente.editaRisposta
    '                Case Domanda.TipoDomanda.Rating

    '                    Dim tabella As New Table
    '                    tabella = FindControlRecursive(itemD, "TBLRadiobutton_" + itemD.ItemIndex.ToString())
    '                    Dim indiceRiga As Integer = 0
    '                    For Each row As TableRow In tabella.Rows
    '                        Dim iCella As Integer = 0
    '                        If indiceRiga > 0 Then
    '                            Dim ris As New RispostaDomanda
    '                            ris = oRisposta.findRispostaByIDDomandaOpzione(oDomanda.risposteDomanda, oDomanda.domandaRating.opzioniRating(indiceRiga - 1).id)

    '                            For Each cell As TableCell In row.Cells
    '                                For Each ctrl As Control In cell.Controls
    '                                    If ctrl.GetType() Is GetType(RadioButton) Then
    '                                        Dim dlOpzioni As New RadioButton
    '                                        dlOpzioni = DirectCast(ctrl, RadioButton)
    '                                        If Not dlOpzioni Is Nothing Then
    '                                            If Not ris Is Nothing Then
    '                                                If iCella = ris.valore Then
    '                                                    If ris.numeroOpzione = oDomanda.domandaRating.opzioniRating(indiceRiga - 1).numero Then
    '                                                        dlOpzioni.Checked = True
    '                                                    End If
    '                                                End If
    '                                            End If
    '                                        End If
    '                                        dlOpzioni.Enabled = Me.QuestionarioCorrente.editaRisposta
    '                                    ElseIf ctrl.GetType() Is GetType(TextBox) Then
    '                                        Dim dlOpzioni As New TextBox
    '                                        dlOpzioni = DirectCast(ctrl, TextBox)
    '                                        If Not dlOpzioni Is Nothing Then
    '                                            If Not ris Is Nothing Then
    '                                                If ris.numeroOpzione = oDomanda.domandaRating.opzioniRating(indiceRiga - 1).numero Then
    '                                                    dlOpzioni.Text = ris.testoOpzione
    '                                                End If
    '                                            End If
    '                                        End If
    '                                        dlOpzioni.Enabled = Me.QuestionarioCorrente.editaRisposta
    '                                    End If

    '                                Next
    '                                iCella = iCella + 1
    '                            Next
    '                        End If
    '                        indiceRiga = indiceRiga + 1
    '                    Next

    '                Case Domanda.TipoDomanda.TestoLibero

    '                    Dim tabella As New Table
    '                    tabella = FindControlRecursive(itemD, "TBLTestoLibero_" + itemD.ItemIndex.ToString())
    '                    Dim iOpzione As Integer = 0
    '                    For Each row As TableRow In tabella.Rows
    '                        Dim ris As New RispostaDomanda
    '                        ris = oRisposta.findRispostaByIDDomandaOpzione(oDomanda.risposteDomanda, oDomanda.opzioniTestoLibero(iOpzione).id)

    '                        For Each ctrl As Control In row.Cells(1).Controls
    '                            If ctrl.GetType() Is GetType(TextBox) Then
    '                                Dim txOpzioni As New TextBox
    '                                txOpzioni = DirectCast(ctrl, TextBox)
    '                                If Not txOpzioni Is Nothing Then
    '                                    If Not ris Is Nothing Then
    '                                        If ris.numeroOpzione = oDomanda.opzioniTestoLibero(iOpzione).numero Then
    '                                            txOpzioni.Text = ris.testoOpzione
    '                                        End If
    '                                    End If
    '                                End If
    '                                txOpzioni.Enabled = Me.QuestionarioCorrente.editaRisposta
    '                            End If
    '                        Next
    '                        iOpzione = iOpzione + 1
    '                    Next

    '                Case Domanda.TipoDomanda.Numerica
    '                    Dim tabella As New Table
    '                    tabella = FindControlRecursive(itemD, "TBLNumerica_" + itemD.ItemIndex.ToString())
    '                    Dim iOpzione As Integer = 0
    '                    For Each row As TableRow In tabella.Rows
    '                        Dim ris As New RispostaDomanda
    '                        ris = oRisposta.findRispostaByIDDomandaOpzione(oDomanda.risposteDomanda, oDomanda.opzioniNumerica(iOpzione).id)
    '                        For Each ctrl As Control In row.Cells(1).Controls
    '                            If ctrl.GetType() Is GetType(TextBox) Then
    '                                Dim txOpzioni As New TextBox
    '                                txOpzioni = DirectCast(ctrl, TextBox)
    '                                If Not txOpzioni Is Nothing Then
    '                                    If Not ris Is Nothing Then
    '                                        If ris.numeroOpzione = oDomanda.opzioniNumerica(iOpzione).numero Then
    '                                            txOpzioni.Text = ris.valore
    '                                        End If
    '                                    End If
    '                                End If
    '                                txOpzioni.Enabled = Me.QuestionarioCorrente.editaRisposta
    '                            End If
    '                        Next
    '                        iOpzione = iOpzione + 1
    '                    Next

    '            End Select


    '        Next

    '    Next

    '    Return DLPagine

    'End Function

    '#Region "Export"
    '    Const delimiter As Char = ";"
    '    Const NULLitem As String = "#NR#"
    '    Const question As Char = "Q"
    '    Const pagina As Char = "p"
    '    Const opzione As Char = "o"
    '    Const freeText = "FT"
    '    Class RispLineare
    '        Public id As Integer
    '        Public text As String
    '        Public Sub New(ByRef idR As Integer)
    '            id = idR
    '            text = Chr(34) & idR.ToString & Chr(34) & delimiter
    '        End Sub
    '        Public Sub New(ByRef firstCell As String)
    '            id = 0
    '            text = Chr(34) & firstCell & Chr(34) & delimiter
    '        End Sub
    '        Public Sub AppendItem(ByVal lineText As String)
    '            'aggiunge virgolette a inizio e fine item, aggiunge delimiter, elimina eventuale delimiter a fine item
    '            lineText = Chr(34) & lineText.Replace(Chr(34), "''") & Chr(34) & delimiter
    '            text &= lineText
    '        End Sub
    '    End Class
    '    Public Function CreateExportText(ByRef oQuest As Questionario) As String
    '        Dim exportText As String = String.Empty
    '        Dim nDomande As Integer = (oQuest.domande.Count - 1)
    '        Dim lines As New List(Of RispLineare)
    '        lines.Add(New RispLineare("IDAnswer"))
    '        For Each oRisp As RispostaQuestionario In oQuest.risposteQuestionario
    '            lines.Add(New RispLineare(oRisp.id))
    '        Next
    '        For Each oDom As Domanda In oQuest.domande
    '            Select Case oDom.tipo
    '                Case Domanda.TipoDomanda.DropDown
    '                    ConvertDropDownToString(oDom, lines)
    '                Case Domanda.TipoDomanda.Multipla
    '                    ConvertMultiplaToString(oDom, lines)
    '                Case Domanda.TipoDomanda.Numerica
    '                    ConvertNumericaToString(oDom, lines)
    '                Case Domanda.TipoDomanda.Rating
    '                    ConvertRatingToString(oDom, lines)
    '                Case Domanda.TipoDomanda.TestoLibero
    '                    ConvertTestoLiberoToString(oDom, lines)
    '            End Select
    '        Next
    '        For Each line As RispLineare In lines
    '            exportText = exportText & line.text & vbCrLf
    '        Next
    '        Return exportText
    '    End Function
    '    Private Function ConvertDropDownToString(ByRef oDom As Domanda, ByRef lines As List(Of RispLineare)) As String
    '        Dim firstRun As Boolean = True
    '        For Each oLine As RispLineare In lines
    '            If Not oLine.id = 0 Then
    '                Dim numeroOpzList As List(Of Integer) = (From oRisp In oDom.risposteDomanda Where oRisp.idRispostaQuestionario = oLine.id Select oRisp.numeroOpzione).ToList
    '                If numeroOpzList.Count = 0 Then
    '                    '"##############################"
    '                    oLine.AppendItem(NULLitem)
    '                Else
    '                    oLine.AppendItem(numeroOpzList(0).ToString)
    '                End If

    '                If firstRun Then
    '                    lines(0).AppendItem(pagina & oDom.numeroPagina & question & oDom.numero)
    '                End If
    '                firstRun = False
    '            End If
    '        Next
    '    End Function
    '    Private Function ConvertMultiplaToString(ByRef oDom As Domanda, ByRef lines As List(Of RispLineare)) As String
    '        Dim firstRun As Boolean = True
    '        For Each oLine As RispLineare In lines
    '            If Not oLine.id = 0 Then
    '                Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Order By oRisp.numeroOpzione Where oRisp.idRispostaQuestionario = oLine.id Select oRisp.numeroOpzione, oRisp.testoOpzione).ToList
    '                Dim nOpzioni As Integer = oDom.domandaMultiplaOpzioni.Count - 1
    '                Dim listIndex As Integer = 0
    '                For i As Integer = 0 To nOpzioni
    '                    If listIndex < numeroOpzList.count AndAlso numeroOpzList(listIndex).numeroOpzione = i + 1 Then
    '                        oLine.AppendItem("1")
    '                        If firstRun Then
    '                            lines(0).AppendItem(pagina & oDom.numeroPagina & question & oDom.numero & opzione & (i + 1).ToString)
    '                        End If
    '                        If oDom.domandaMultiplaOpzioni(i).isAltro Then
    '                            oLine.AppendItem(numeroOpzList(listIndex).testoOpzione)
    '                            If firstRun Then
    '                                lines(0).AppendItem(pagina & oDom.numeroPagina & question & oDom.numero & opzione & (i + 1).ToString & freeText)
    '                            End If
    '                        End If
    '                        listIndex += 1
    '                    Else
    '                        oLine.AppendItem("0")
    '                        If firstRun Then
    '                            lines(0).AppendItem(pagina & oDom.numeroPagina & question & oDom.numero & opzione & (i + 1).ToString)
    '                        End If
    '                        If oDom.domandaMultiplaOpzioni(i).isAltro Then
    '                            oLine.AppendItem(NULLitem)
    '                            If firstRun Then
    '                                lines(0).AppendItem(pagina & oDom.numeroPagina & question & oDom.numero & opzione & (i + 1).ToString & freeText)
    '                            End If
    '                        End If
    '                    End If
    '                Next
    '                firstRun = False
    '            End If
    '        Next
    '    End Function
    '    Private Function ConvertNumericaToString(ByRef oDom As Domanda, ByRef lines As List(Of RispLineare)) As String
    '        Dim firstRun As Boolean = True
    '        For Each oLine As RispLineare In lines
    '            If Not oLine.id = 0 Then
    '                Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Where oRisp.idRispostaQuestionario = oLine.id Order By oRisp.numeroOpzione Select oRisp.valore, oRisp.numeroOpzione).ToList
    '                Dim nOpzioni As Integer = oDom.opzioniNumerica.Count - 1
    '                For i As Integer = 0 To nOpzioni
    '                    If IsNothing(numeroOpzList) OrElse numeroOpzList.Count = 0 Then
    '                        '"##############################"
    '                        oLine.AppendItem(NULLitem)
    '                    Else
    '                        If Not numeroOpzList(i).valore = Integer.MinValue Then 'valore di default quando la risposta non e' stata inserita
    '                            oLine.AppendItem(numeroOpzList(i).valore.ToString)
    '                        Else
    '                            oLine.AppendItem(NULLitem)
    '                        End If
    '                    End If

    '                    If firstRun Then
    '                        lines(0).AppendItem(pagina & oDom.numeroPagina & question & oDom.numero & opzione & (i + 1).ToString)
    '                    End If
    '                Next
    '                firstRun = False
    '            End If
    '        Next
    '    End Function
    '    Private Function ConvertTestoLiberoToString(ByRef oDom As Domanda, ByRef lines As List(Of RispLineare)) As String
    '        Dim firstRun As Boolean = True
    '        For Each oLine As RispLineare In lines
    '            If Not oLine.id = 0 Then
    '                Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Where oRisp.idRispostaQuestionario = oLine.id Order By oRisp.numeroOpzione Select oRisp.testoOpzione, oRisp.numeroOpzione).ToList
    '                Dim nOpzioni As Integer = oDom.opzioniTestoLibero.Count - 1
    '                For i As Integer = 0 To nOpzioni
    '                    If i < numeroOpzList.count AndAlso (numeroOpzList(i).numeroOpzione = i + 1 Or (oDom.opzioniTestoLibero.Count = 1 AndAlso numeroOpzList(i).numeroOpzione = 0)) Then
    '                        'quando ho una TL con una sola opzione, l'indice dell'opzione parte da 0. verificare.
    '                        oLine.AppendItem(numeroOpzList(i).testoOpzione)
    '                    Else
    '                        oLine.AppendItem(NULLitem)
    '                    End If
    '                    If firstRun Then
    '                        lines(0).AppendItem(pagina & oDom.numeroPagina & question & oDom.numero & opzione & (i + 1).ToString & freeText)
    '                    End If
    '                Next
    '                firstRun = False
    '            End If
    '        Next
    '    End Function
    '    Private Function ConvertRatingToString(ByVal oDom As Domanda, ByRef lines As List(Of RispLineare)) As String
    '        Dim firstRun As Boolean = True
    '        For Each oLine As RispLineare In lines
    '            If Not oLine.id = 0 Then
    '                Dim nOpzioni As Integer = oDom.domandaRating.opzioniRating.Count - 1
    '                Dim listIndex As Integer = 0
    '                For j As Integer = 0 To nOpzioni
    '                    Dim isSelectedRow As Boolean = False
    '                    Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Order By oRisp.idDomandaOpzione Where oRisp.idRispostaQuestionario = oLine.id And oRisp.idDomandaOpzione = oDom.domandaRating.opzioniRating(j).id Select oRisp.valore, oRisp.testoOpzione).ToList
    '                    For i As Integer = 0 To oDom.domandaRating.numeroRating
    '                        If numeroOpzList.count > 0 AndAlso numeroOpzList(0).valore = i + 1 Then
    '                            oLine.AppendItem("1")
    '                            If firstRun Then
    '                                lines(0).AppendItem(pagina & oDom.numeroPagina & question & oDom.numero & opzione & (j + 1).ToString & "," & (i + 1).ToString)
    '                            End If
    '                            isSelectedRow = True
    '                        Else
    '                            oLine.AppendItem("0")
    '                            If firstRun Then
    '                                lines(0).AppendItem(pagina & oDom.numeroPagina & question & oDom.numero & opzione & (j + 1).ToString & "," & (i + 1).ToString)
    '                            End If
    '                        End If
    '                    Next
    '                    If oDom.domandaRating.opzioniRating(j).isAltro Then
    '                        If isSelectedRow Then
    '                            oLine.AppendItem(numeroOpzList(0).testoOpzione)
    '                        Else
    '                            oLine.AppendItem(NULLitem)
    '                        End If
    '                        If firstRun Then
    '                            lines(0).AppendItem(pagina & oDom.numeroPagina & question & oDom.numero & opzione & (j + 1).ToString & freeText)
    '                        End If
    '                    End If
    '                Next
    '                firstRun = False
    '            End If
    '        Next
    '    End Function
    '#End Region
    'Public Shared Sub PrintQuestionnaire(appContext As lm.Comol.Core.DomainModel.iApplicationContext, ByVal idQuest As Integer, ByVal idLanguage As Integer, ByVal anonymousUser As String, ByVal http As HttpResponse)
    '    Dim oGestioneRis As New GestioneRisposte
    '    Dim quest As Questionario = DALQuestionario.readQuestionarioBYLingua(appContext, idQuest, idLanguage, True)


    '    '   Dim questionnaire As LazyQuestionnaire = DALQuestionario.GetQuestionnaire(appContext, idQuest)
    '    Dim name As String = quest.nome
    '    If String.IsNullOrEmpty(name) Then
    '        name = "FileExport.csv"
    '    Else
    '        name = lm.Comol.Core.DomainModel.Helpers.TextUtility.SanitizeFileName(name, "_")
    '        If name.Length > 100 Then
    '            name = Left(name, 100) & "_Export.csv"
    '        Else
    '            name &= "_Export.csv"
    '        End If
    '        name = Replace(name, " ", "_")
    '    End If
    '    http.Clear()
    '    http.Buffer = True
    '    http.AddHeader("Content-Disposition", "attachment; filename=" & name)
    '    http.ContentType = "text/csv"
    '    http.Charset = ""
    '    http.ContentEncoding = System.Text.Encoding.Default
    '    Dim helper As New Business.HelperExportToCsv()

    '    If quest.risultatiAnonimi Then
    '        http.Write(helper.ExportQuestionnaireResponses(quest, anonymousUser, New Dictionary(Of String, String)))
    '    Else
    '        http.Write(helper.ExportQuestionnaireResponses(quest, anonymousUser, DALQuestionario.GetDisplayNameByResponses(appContext, quest.risposteQuestionario, anonymousUser)))
    '    End If

    '    http.End()
    'End Sub
    ''' <summary>
    ''' QUESTIONARIO: esportazione di tutte le risposte date !
    ''' </summary>
    ''' <param name="appContext"></param>
    ''' <param name="Quest"></param>
    ''' <param name="status"></param>
    ''' <param name="idLanguage"></param>
    ''' <param name="anonymousUser"></param>
    ''' <param name="translations"></param>
    ''' <param name="type"></param>
    ''' <param name="openCloseConnection"></param>
    ''' <param name="clientFilename"></param>
    ''' <param name="webResponse"></param>
    ''' <param name="cookie"></param>
    ''' <remarks></remarks>
    Public Shared Sub ExportQuestionnaireAnswers(
                                                appContext As lm.Comol.Core.DomainModel.iApplicationContext,
                                                ByVal Quest As Questionario,
                                                ByVal platformTaxCodeRequired As Boolean,
                                                ByVal status As AnswerStatus,
                                                ByVal idLanguage As Integer,
                                                ByVal anonymousUser As String,
                                                ByVal translations As Dictionary(Of QuestionnaireExportTranslations, String),
                                                ByVal type As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType,
                                                openCloseConnection As Boolean,
                                                clientFilename As String,
                                                webResponse As System.Web.HttpResponse,
                                                cookie As System.Web.HttpCookie,
                                                Optional ByVal oneColumnForEachQuestion As Boolean = True)

        If oneColumnForEachQuestion Then
            ExportQuestionnaireAnswersOneColumnForEachQuestion(
                appContext,
                Quest.id,
                platformTaxCodeRequired,
                status,
                idLanguage,
                anonymousUser,
                translations,
                type,
                openCloseConnection,
                clientFilename,
                webResponse,
                cookie)

        Else
            ExportQuestionnaireAnswersPlain(
                appContext,
                Quest,
                platformTaxCodeRequired,
                status,
                idLanguage,
                anonymousUser,
                translations,
                type,
                openCloseConnection,
                clientFilename,
                webResponse,
                cookie)
        End If
    End Sub

    Private Shared Sub ExportQuestionnaireAnswersOneColumnForEachQuestion(
                    appContext As lm.Comol.Core.DomainModel.iApplicationContext,
                    ByVal idQuest As Integer,
                    ByVal platformTaxCodeRequired As Boolean,
                    ByVal status As AnswerStatus,
                    ByVal idLanguage As Integer,
                    ByVal anonymousUser As String,
                    ByVal translations As Dictionary(Of QuestionnaireExportTranslations, String),
                    ByVal type As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType,
                    openCloseConnection As Boolean,
                    clientFilename As String,
                    webResponse As System.Web.HttpResponse,
                    cookie As System.Web.HttpCookie)


        Dim quest As Questionario = DALQuestionario.readQuestionarioBYLingua(appContext, idQuest, idLanguage, True)
        Dim displayTaxCode As Boolean = platformTaxCodeRequired AndAlso Not quest.risultatiAnonimi

        Dim name As String = quest.nome
        Try
            Dim s As New Business.ServiceQuestionnaire(appContext)
            If openCloseConnection Then
                webResponse.Clear()
            End If
            If Not String.IsNullOrEmpty(name) Then
                clientFilename = lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(String.Format(clientFilename, name))
                If clientFilename.Length > 100 Then
                    clientFilename = lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(String.Format(clientFilename, ""))
                End If
            Else
                clientFilename = String.Format(clientFilename, "")
            End If
            webResponse.ContentEncoding = System.Text.Encoding.Default

            webResponse.AppendCookie(cookie)
            webResponse.AddHeader("Content-Disposition", "attachment; filename=" & clientFilename & "." & type.ToString())

            Dim displayNames As New Dictionary(Of String, dtoDisplayName)
            If Not quest.risultatiAnonimi Then
                displayNames = DALQuestionario.GetDtoDisplayNameByResponses(appContext, quest.risposteQuestionario, anonymousUser)
            Else
                displayNames = DALQuestionario.GetDtoDisplayNameByAnonymousResponses(appContext, quest.risposteQuestionario, anonymousUser)
            End If
            Dim libraries As New List(Of Questionario)
            Dim answers As New Dictionary(Of Long, QuestionnaireAnswer)
            If quest.librerieQuestionario.Any Then
                For Each library As LibreriaQuestionario In quest.librerieQuestionario
                    libraries.Add(DALQuestionario.readQuestionarioBYLingua(appContext, library.idLibreria, idLanguage, False, True))
                Next
                For Each answer As RispostaQuestionario In quest.risposteQuestionario
                    answers.Add(answer.id, DALQuestionario.GetQuestionnaireAnswers(appContext, answer.id, idQuest, answer.idPersona, answer.idUtenteInvitato, answer.idQuestionarioRandom))
                Next
            End If
            Dim p As lm.Comol.Core.DomainModel.Person = s.GetItem(Of lm.Comol.Core.DomainModel.Person)(appContext.UserContext.CurrentUserID)
            Select Case type
                Case lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.xml
                    webResponse.ContentType = "application/ms-excel"
                    Dim helper As New Business.HelperExportToXml(translations)

                    webResponse.Write(helper.QuestionnaireAnswers(p, True, quest, answers, libraries, anonymousUser, displayNames))
                Case lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.csv
                    webResponse.ContentType = "text/csv"
                    webResponse.BinaryWrite(webResponse.ContentEncoding.GetPreamble())
                    Dim helper As New Business.HelperExportToCsv(translations)

                    webResponse.Write(helper.QuestionnaireAnswers(p, platformTaxCodeRequired, True, quest, answers, libraries, anonymousUser, displayNames))
            End Select


            If openCloseConnection Then
                webResponse.End()
            End If
        Catch de As Exception

        End Try
    End Sub
    Private Shared Sub ExportQuestionnaireAnswersPlain(
                                                      appContext As lm.Comol.Core.DomainModel.iApplicationContext,
                                                      ByVal Quest As Questionario,
                                                      ByVal platformTaxCodeRequired As Boolean,
                                                      ByVal status As AnswerStatus,
                                                      ByVal idLanguage As Integer,
                                                      ByVal anonymousUser As String,
                                                      ByVal translations As Dictionary(Of QuestionnaireExportTranslations, String),
                                                      ByVal type As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType,
                                                      openCloseConnection As Boolean,
                                                      clientFilename As String,
                                                      webResponse As System.Web.HttpResponse,
                                                      cookie As System.Web.HttpCookie)
        Try
            Dim service As New Business.ServiceQuestionnaire(appContext)
            'Dim name As String = service.GetQuestionaireName(idQuest, idLanguage)
            Dim idQuest As Integer = Quest.id
            Dim name As String = Quest.nome

            If openCloseConnection Then
                webResponse.Clear()
            End If
            If Not String.IsNullOrEmpty(name) Then
                clientFilename = lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(String.Format(clientFilename, name))
                If clientFilename.Length > 100 Then
                    clientFilename = lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(String.Format(clientFilename, ""))
                End If
            Else
                clientFilename = String.Format(clientFilename, "")
            End If
            webResponse.ContentEncoding = System.Text.Encoding.Default

            webResponse.AppendCookie(cookie)
            webResponse.AddHeader("Content-Disposition", "attachment; filename=" & clientFilename & "." & type.ToString())

            Dim displayNames As New Dictionary(Of String, dtoDisplayName)
            Dim isAnonymous As Boolean = service.QuestionnaireHasAnonymousValues(idQuest)

            'Messo internamente: controllo il TIPO PERSONA (sysAdmin, Admin,  Administrative)
            'Dim displayTaxCode As Boolean = platformTaxCodeRequired AndAlso Not isAnonymous

            Dim qAnswers As List(Of dtoFullUserAnswerItem) = service.GetQuestionnaireAnswers(idQuest, status, False)

            If Not isAnonymous Then
                displayNames = service.GetDtoDisplayNameByResponses(qAnswers, anonymousUser)
            Else
                displayNames = service.GetDtoDisplayNameByAnonymousResponses(qAnswers, anonymousUser)
            End If
            Dim answers As New Dictionary(Of Long, QuestionnaireAnswer)
            For Each answer As dtoFullUserAnswerItem In qAnswers
                answers.Add(
                    answer.Id,
                    DALQuestionario.GetQuestionnaireAnswers(
                        appContext,
                        answer.Id,
                        idQuest,
                        answer.Answer.IdPerson,
                        answer.Answer.IdInvitedUser,
                        answer.Answer.IdRandomQuestionnaire,
                        True)
                    )
            Next
            Dim p As lm.Comol.Core.DomainModel.Person = service.GetItem(Of lm.Comol.Core.DomainModel.Person)(appContext.UserContext.CurrentUserID)
            Select Case type
                Case lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.xml
                    webResponse.ContentType = "application/ms-excel"
                    Dim helper As New Business.HelperExportToXml(translations)

                    webResponse.Write(
                        helper.QuestionnaireAnswers(
                            p,
                            True,
                            idLanguage,
                            qAnswers,
                            answers,
                            anonymousUser,
                            displayNames,
                            Quest)
                        )
                Case lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.csv
                    webResponse.ContentType = "text/csv"
                    webResponse.BinaryWrite(webResponse.ContentEncoding.GetPreamble())
                    Dim helper As New Business.HelperExportToCsv(translations)

                    webResponse.Write(
                        helper.QuestionnaireAnswers(
                                p,
                                True,
                                idLanguage,
                                qAnswers,
                                answers,
                                anonymousUser,
                                displayNames,
                                Quest
                            )
                        )
            End Select


            If openCloseConnection Then
                webResponse.End()
            End If
        Catch de As Exception

        End Try
    End Sub
    Public Shared Sub ExportUserQuestionnaireAnswers(appContext As lm.Comol.Core.DomainModel.iApplicationContext, ByVal displayTaxCode As Boolean, ByVal idUser As Integer, ByVal idInvite As Integer, ByVal idQuest As Integer, ByVal status As AnswerStatus, ByVal idLanguage As Integer, ByVal anonymousUser As String, ByVal translations As Dictionary(Of QuestionnaireExportTranslations, String), ByVal type As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType, openCloseConnection As Boolean, clientFilename As String, webResponse As System.Web.HttpResponse, cookie As System.Web.HttpCookie, Optional ByVal oneColumnForEachQuestion As Boolean = True)
        If oneColumnForEachQuestion Then
            ExportUserQuestionnaireAnswersOneColumnForEachQuestion(appContext, displayTaxCode, idUser, idInvite, idQuest, status, idLanguage, anonymousUser, translations, type, openCloseConnection, clientFilename, webResponse, cookie)
        Else
            ExportUserQuestionnaireAnswersPlain(appContext, displayTaxCode, idUser, idInvite, idQuest, status, idLanguage, anonymousUser, translations, type, openCloseConnection, clientFilename, webResponse, cookie)
        End If
    End Sub
    Private Shared Sub ExportUserQuestionnaireAnswersOneColumnForEachQuestion(
                        appContext As lm.Comol.Core.DomainModel.iApplicationContext,
                        ByVal displayTaxCode As Boolean,
                        ByVal idUser As Integer,
                        ByVal idInvite As Integer,
                        ByVal idQuest As Integer,
                        ByVal status As AnswerStatus,
                        ByVal idLanguage As Integer,
                        ByVal anonymousUser As String,
                        ByVal translations As Dictionary(Of QuestionnaireExportTranslations, String),
                        ByVal type As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType,
                        openCloseConnection As Boolean,
                        clientFilename As String,
                        webResponse As System.Web.HttpResponse, cookie As System.Web.HttpCookie)

        'QUESTA è presumibilmente una "vecchia cosa", che NON LEGGE le risposte.
        'Infatti esporta tutto vuoto!!!
        'Dim quest As Questionario = DALQuestionario.readQuestionarioBYLingua(appContext, idQuest, idLanguage, False)

        'QUESTA, invece, è quella utilizzata anche "dall'altra parte" che legge le varie risposte!
        'DA TESTARE: PF ed utente DIVERSO da quello corrente!
        Dim quest As New Questionario
        quest = DALQuestionario.readQuestionarioByPersona(appContext, False, idQuest, idLanguage, idUser, idInvite)


        Dim questName As String = quest.nome
        Dim userDisplayName As dtoDisplayName = New dtoDisplayName(anonymousUser)
        Try
            Dim s As New Business.ServiceQuestionnaire(appContext)
            displayTaxCode = displayTaxCode AndAlso Not quest.risultatiAnonimi
            Dim p As lm.Comol.Core.DomainModel.Person = s.GetItem(Of lm.Comol.Core.DomainModel.Person)(appContext.UserContext.CurrentUserID)
            If openCloseConnection Then
                webResponse.Clear()
            End If
            If Not quest.risultatiAnonimi Then
                userDisplayName = s.GetUserDtoDisplayName(idUser, idInvite, anonymousUser)
            End If

            If Not String.IsNullOrEmpty(questName) Then
                clientFilename = lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(String.Format(clientFilename, userDisplayName.DisplayName, questName))
                If clientFilename.Length > 100 Then
                    clientFilename = lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(String.Format(clientFilename, userDisplayName.DisplayName, ""))
                End If
            Else
                clientFilename = String.Format(clientFilename, userDisplayName.DisplayName, "")
            End If
            webResponse.ContentEncoding = System.Text.Encoding.Default
            webResponse.AppendCookie(cookie)
            webResponse.AddHeader("Content-Disposition", "attachment; filename=" & clientFilename & "." & type.ToString())

            Dim displayNames As New Dictionary(Of String, dtoDisplayName)
            If Not quest.risultatiAnonimi AndAlso idUser > 0 Then
                Dim user As lm.Comol.Core.DomainModel.litePerson = s.GetItem(Of lm.Comol.Core.DomainModel.litePerson)(idUser)
                If Not IsNothing(user) Then
                    displayNames.Add("p_" & idUser, New dtoDisplayName(user))
                End If
            ElseIf idInvite > 0 AndAlso quest.risultatiAnonimi Then
                Dim oInvited As LazyInvitedUser = s.GetItem(Of LazyInvitedUser)(idInvite)
                If Not IsNothing(oInvited) Then
                    displayNames.Add("i_" & idInvite, New dtoDisplayName(anonymousUser) With {.OtherInfos = oInvited.Description})
                End If
            End If
            Dim libraries As New List(Of Questionario)
            Dim answers As New Dictionary(Of Long, QuestionnaireAnswer)

            For Each library As LibreriaQuestionario In quest.librerieQuestionario
                libraries.Add(DALQuestionario.readQuestionarioBYLingua(appContext, library.idLibreria, idLanguage, False, True))
            Next

            Dim items As List(Of LazyUserResponse) = s.GetQuestionnaireAttempts(idQuest, idUser, 0)
            For Each answer As LazyUserResponse In items.Where(Function(r) (status = AnswerStatus.All OrElse (status = AnswerStatus.Completed AndAlso r.CompletedOn.HasValue) OrElse (status = AnswerStatus.Compiling AndAlso Not r.CompletedOn.HasValue))).ToList()
                ' answers.Add(answer.Id, DALQuestionario.readQuestionarioByPersona(appContext, False, idQuest, idLanguage, answer.IdPerson, idInvite, answer.Id))
                answers.Add(answer.Id, DALQuestionario.GetQuestionnaireAnswers(appContext, answer.Id, idQuest, answer.IdPerson, idInvite, answer.IdRandomQuestionnaire))
            Next

            Select Case type
                Case lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.xml
                    webResponse.ContentType = "application/ms-excel"
                    Dim helper As New Business.HelperExportToXml(translations)

                    webResponse.Write(helper.QuestionnaireAnswers(p, True, quest, answers, libraries, anonymousUser, displayNames, True, True, userDisplayName))
                Case lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.csv
                    webResponse.ContentType = "text/csv"
                    Dim helper As New Business.HelperExportToCsv(translations)

                    webResponse.Write(helper.QuestionnaireAnswers(p, displayTaxCode, True, quest, answers, libraries, anonymousUser, displayNames, True, True, userDisplayName))
            End Select


            If openCloseConnection Then
                webResponse.End()
            End If
        Catch de As Exception

        End Try
    End Sub
    Public Shared Sub ExportUserQuestionnaireAnswersPlain(appContext As lm.Comol.Core.DomainModel.iApplicationContext, ByVal displayTaxCode As Boolean, ByVal idUser As Integer, ByVal idInvite As Integer, ByVal idQuest As Integer, ByVal status As AnswerStatus, ByVal idLanguage As Integer, ByVal anonymousUser As String, ByVal translations As Dictionary(Of QuestionnaireExportTranslations, String), ByVal type As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType, openCloseConnection As Boolean, clientFilename As String, webResponse As System.Web.HttpResponse, cookie As System.Web.HttpCookie)
        Dim quest As Questionario = DALQuestionario.readQuestionarioBYLingua(appContext, idQuest, idLanguage, False)
        Dim questName As String = quest.nome
        Dim userDisplayName As dtoDisplayName = New dtoDisplayName(anonymousUser)
        Try
            Dim s As New Business.ServiceQuestionnaire(appContext)
            displayTaxCode = displayTaxCode AndAlso Not quest.risultatiAnonimi
            If openCloseConnection Then
                webResponse.Clear()
            End If
            If Not quest.risultatiAnonimi Then
                userDisplayName = s.GetUserDtoDisplayName(idUser, idInvite, anonymousUser)
            End If

            If Not String.IsNullOrEmpty(questName) Then
                clientFilename = lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(String.Format(clientFilename, userDisplayName.DisplayName, questName))
                If clientFilename.Length > 100 Then
                    clientFilename = lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(String.Format(clientFilename, userDisplayName.DisplayName, ""))
                End If
            Else
                clientFilename = String.Format(clientFilename, userDisplayName.DisplayName, "")
            End If
            webResponse.ContentEncoding = System.Text.Encoding.Default
            webResponse.AppendCookie(cookie)
            webResponse.AddHeader("Content-Disposition", "attachment; filename=" & clientFilename & "." & type.ToString())

            Dim displayNames As New Dictionary(Of String, dtoDisplayName)
            If Not quest.risultatiAnonimi AndAlso idUser > 0 Then
                Dim user As lm.Comol.Core.DomainModel.litePerson = s.GetItem(Of lm.Comol.Core.DomainModel.litePerson)(idUser)
                If Not IsNothing(user) Then
                    displayNames.Add("p_" & idUser, New dtoDisplayName(user))
                End If
            ElseIf idInvite > 0 AndAlso quest.risultatiAnonimi Then
                Dim oInvited As LazyInvitedUser = s.GetItem(Of LazyInvitedUser)(idInvite)
                If Not IsNothing(oInvited) Then
                    displayNames.Add("i_" & idInvite, New dtoDisplayName(anonymousUser) With {.OtherInfos = oInvited.Description})
                End If
            End If
            Dim libraries As New List(Of Questionario)
            Dim answers As New Dictionary(Of Long, QuestionnaireAnswer)

            For Each library As LibreriaQuestionario In quest.librerieQuestionario
                libraries.Add(DALQuestionario.readQuestionarioBYLingua(appContext, library.idLibreria, idLanguage, False, True))
            Next

            Dim items As List(Of LazyUserResponse) = s.GetQuestionnaireAttempts(idQuest, idUser, 0)
            For Each answer As LazyUserResponse In items.Where(Function(r) (status = AnswerStatus.All OrElse (status = AnswerStatus.Completed AndAlso r.CompletedOn.HasValue) OrElse (status = AnswerStatus.Compiling AndAlso Not r.CompletedOn.HasValue))).ToList()
                ' answers.Add(answer.Id, DALQuestionario.readQuestionarioByPersona(appContext, False, idQuest, idLanguage, answer.IdPerson, idInvite, answer.Id))
                answers.Add(answer.Id, DALQuestionario.GetQuestionnaireAnswers(appContext, answer.Id, idQuest, answer.IdPerson, idInvite, answer.IdRandomQuestionnaire))
            Next
            Dim p As lm.Comol.Core.DomainModel.Person = s.GetItem(Of lm.Comol.Core.DomainModel.Person)(appContext.UserContext.CurrentUserID)
            Select Case type
                Case lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.xml
                    webResponse.ContentType = "application/ms-excel"
                    Dim helper As New Business.HelperExportToXml(translations)

                    webResponse.Write(helper.QuestionnaireAnswers(p, True, quest, answers, libraries, anonymousUser, displayNames))
                Case lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.csv
                    webResponse.ContentType = "text/csv"
                    Dim helper As New Business.HelperExportToCsv(translations)

                    webResponse.Write(helper.QuestionnaireAnswers(p, displayTaxCode, True, quest, answers, libraries, anonymousUser, displayNames, False, True, userDisplayName))
            End Select


            If openCloseConnection Then
                webResponse.End()
            End If
        Catch de As Exception

        End Try
    End Sub
    Public Shared Sub ExportQuestionnaireAttempts(appContext As lm.Comol.Core.DomainModel.iApplicationContext, ByVal idQuest As Integer, ByVal status As AnswerStatus, ByVal idLanguage As Integer, ByVal anonymousUser As String, ByVal translations As Dictionary(Of QuestionnaireExportTranslations, String), ByVal type As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType, openCloseConnection As Boolean, clientFilename As String, webResponse As System.Web.HttpResponse, cookie As System.Web.HttpCookie, Optional ByVal oneColumnForEachQuestion As Boolean = True)
        Dim quest As Questionario = DALQuestionario.readQuestionarioBYLingua(appContext, idQuest, idLanguage, False)
        Dim name As String = quest.nome
        Try
            Dim s As New Business.ServiceQuestionnaire(appContext)
            If openCloseConnection Then
                webResponse.Clear()
            End If
            If Not String.IsNullOrEmpty(name) Then
                clientFilename = lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(String.Format(clientFilename, name))
                If clientFilename.Length > 100 Then
                    clientFilename = lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(String.Format(clientFilename, ""))
                End If
            Else
                clientFilename = String.Format(clientFilename, "")
            End If
            webResponse.AppendCookie(cookie)
            webResponse.AddHeader("Content-Disposition", "attachment; filename=" & clientFilename & "." & type.ToString())
            Select Case type
                Case lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.xml
                    webResponse.ContentType = "application/ms-excel"
                    Dim helper As New Business.HelperExportToXml(translations)

                    webResponse.Write(helper.QuestionnaireAttempts(quest.MinScore, quest.scalaValutazione, s.GetQuestionnaireAttempts(idQuest, status, anonymousUser), (quest.tipo = QuestionnaireType.RandomMultipleAttempts)))
                Case lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.csv
                    webResponse.ContentType = "text/csv"
                    Dim helper As New Business.HelperExportToCsv(translations)

                    webResponse.Write(helper.QuestionnaireAttempts(quest.MinScore, quest.scalaValutazione, s.GetQuestionnaireAttempts(idQuest, status, anonymousUser), (quest.tipo = QuestionnaireType.RandomMultipleAttempts)))
            End Select

            If openCloseConnection Then
                webResponse.End()
            End If
        Catch de As Exception

        End Try
    End Sub
    Public Shared Sub ExportQuestionnaireAttempts(appContext As lm.Comol.Core.DomainModel.iApplicationContext, ByVal idUser As Integer, ByVal idInvite As Integer, ByVal idQuest As Integer, ByVal status As AnswerStatus, ByVal idLanguage As Integer, ByVal anonymousUser As String, ByVal translations As Dictionary(Of QuestionnaireExportTranslations, String), ByVal type As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType, openCloseConnection As Boolean, clientFilename As String, webResponse As System.Web.HttpResponse, cookie As System.Web.HttpCookie, Optional ByVal oneColumnForEachQuestion As Boolean = True)
        Dim quest As Questionario = DALQuestionario.readQuestionarioBYLingua(appContext, idQuest, idLanguage, False)
        Dim questName As String = quest.nome
        Dim userName As String = anonymousUser
        Try
            Dim s As New Business.ServiceQuestionnaire(appContext)
            If openCloseConnection Then
                webResponse.Clear()
            End If
            If Not quest.risultatiAnonimi Then
                userName = s.GetUserDisplayName(idUser, idInvite, anonymousUser)
            End If
            If Not String.IsNullOrEmpty(questName) Then
                clientFilename = lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(String.Format(clientFilename, userName, questName))
                If clientFilename.Length > 100 Then
                    clientFilename = lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper.HtmlCheckFileName(String.Format(clientFilename, userName, ""))
                End If
            Else
                clientFilename = String.Format(clientFilename, userName, "")
            End If
            webResponse.AppendCookie(cookie)
            webResponse.AddHeader("Content-Disposition", "attachment; filename=" & clientFilename & "." & type.ToString())
            Select Case type
                Case lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.xml
                    webResponse.ContentType = "application/ms-excel"
                    Dim helper As New Business.HelperExportToXml(translations)

                    webResponse.Write(helper.QuestionnaireAttempts(quest.MinScore, quest.scalaValutazione, s.GetQuestionnaireAttempts(idQuest, idUser, idInvite, status, anonymousUser), (quest.tipo = QuestionnaireType.RandomMultipleAttempts)))
                Case lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType.csv
                    webResponse.ContentType = "text/csv"
                    Dim helper As New Business.HelperExportToCsv(translations)

                    webResponse.Write(helper.QuestionnaireAttempts(quest.MinScore, quest.scalaValutazione, s.GetQuestionnaireAttempts(idQuest, idUser, idInvite, status, anonymousUser), (quest.tipo = QuestionnaireType.RandomMultipleAttempts)))
            End Select

            If openCloseConnection Then
                webResponse.End()
            End If
        Catch de As Exception

        End Try
    End Sub
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property

    ''' <summary>
    ''' Estra il testo piano da una stringa HTML
    ''' </summary>
    ''' <param name="html"></param>
    ''' <returns></returns>
    ''' <remarks>Usa l'Editor Telerick</remarks>
    Public Function TxtHelper_HtmlToString(ByVal html As String)
        Dim oEditor As New RadEditor()
        oEditor.Content = html
        Return oEditor.Text
    End Function

    ''' <summary>
    ''' Tronca una stringa, prendendo i primi caratteri ed aggiungendo ... se maggiore della lunghezza indicata.
    ''' </summary>
    ''' <param name="text"></param>
    ''' <param name="numChar"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TxtHelper_CutString(ByVal text As String, Optional ByVal numChar As Integer = 0)

        text = text.Replace("\r", "").Replace("\n", "").Replace("/r", "").Replace("/n", "")

        If (numChar > 0) AndAlso text.Count() > numChar Then
            Return String.Format("{0}...", text.Substring(0, numChar))
        End If

        Return text
    End Function

    ''' <summary>
    ''' Estrae il testo piano da una stringa HTML prendendo i primi numchar caratteri.
    ''' </summary>
    ''' <param name="html"></param>
    ''' <param name="numChar"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TxtHelper_HtmlCutToString(ByVal html As String, Optional ByVal numChar As Integer = 0)
        Return TxtHelper_CutString(TxtHelper_HtmlToString(html), numChar)
    End Function


    Private Function Deviation(ByVal values As List(Of Integer), ByRef Average As Double) As Double
        ', Optional ByVal IsCampione As Boolean = True

        If IsNothing(values) OrElse Not values.Any() Then
            Return 0
        End If

        Dim count As Integer = values.Count
        Average = 0
        'If IsCampione AndAlso count <= 1 Then
        '    Return 0
        'End If

        Dim sum As Integer = 0

        For Each val As Integer In values
            sum += val
        Next

        Average = sum / count

        Dim divSum As Double = 0

        For Each val As Integer In values
            divSum += ((val - Average) ^ 2)
        Next

        Dim Dev As Double = 0


        'If (IsCampione) Then
        '    Dev = divSum / (count - 1)
        'Else
        Dev = divSum / count
        'End If

        Return Dev
    End Function

End Class
