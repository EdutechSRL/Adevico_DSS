Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Business
Imports lm.Comol.Core.DomainModel.Helpers

Namespace Business
    Public Class HelperExportToCsv
        Inherits lm.Comol.Core.DomainModel.Helpers.Export.ExportCsvBaseHelper
        Private _Helper As BaseHelperExport
        Private m_CommonTranslations As New Dictionary(Of QuestionnaireExportTranslations, String)
        Private Property CommonTranslations() As Dictionary(Of QuestionnaireExportTranslations, String)
            Get
                Return m_CommonTranslations
            End Get
            Set(value As Dictionary(Of QuestionnaireExportTranslations, String))
                m_CommonTranslations = value
            End Set
        End Property

        Private ReadOnly Property LibraryIdentifier
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.LibraryIdentifier)
            End Get
        End Property
        Private ReadOnly Property PageIdentifier
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.PageIdentifier)
            End Get
        End Property
        Private ReadOnly Property QuestionIdentifier
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.QuestionIdentifier)
            End Get
        End Property
        Private ReadOnly Property DeletedQuestionIdentifier
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.DeletedQuestionIdentifier)
            End Get
        End Property
        Private ReadOnly Property OptionIdentifier
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.OptionIdentifier)
            End Get
        End Property
        Private ReadOnly Property FreeTextIdentifier
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.FreeTextIdentifier)
            End Get
        End Property
        Private ReadOnly Property ItemEmpty
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.ItemEmpty)
            End Get
        End Property
        Private ReadOnly Property NullItem
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.NullItem)
            End Get
        End Property
        Private ReadOnly Property QuestionNotUsed
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.QuestionNotUsed)
            End Get
        End Property
        Private ReadOnly Property QuestionNotUsedIdentifier
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.QuestionNotUsedIdentifier)
            End Get
        End Property

        Private ReadOnly Property CellTitleIdQuestion
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleIdQuestion)
            End Get
        End Property
        'Const NULLitem As String = "#NR#"
        'Const question As Char = "Q"
        'Const pagina As Char = "p"
        'Const opzione As Char = "o"
        'Const freeText = "FT"

        Public Sub New()
        End Sub
        Public Sub New(translations As Dictionary(Of QuestionnaireExportTranslations, String))
            CommonTranslations = translations
            _Helper = New BaseHelperExport(translations)
        End Sub
        Public Sub New(endRow As String, delimiter As Char, endField As Char)
            MyBase.New(endRow, delimiter, endField)
        End Sub
        Public Sub New(endRow As String, delimiter As Char, endField As Char, translations As Dictionary(Of QuestionnaireExportTranslations, String))
            MyBase.New(endRow, delimiter, endField)
            CommonTranslations = translations
            _Helper = New BaseHelperExport(translations)
        End Sub

#Region "Export Questionnaire"
        Public Function QuestionnaireAnswers(
                                            ByVal userRequiringExport As Person,
                                            ByVal displayTaxCode As Boolean,
                                            ByVal allowFullExport As Boolean,
                                            ByVal questionnaire As Questionario,
                                            answers As Dictionary(Of Long, QuestionnaireAnswer),
                                            libraries As List(Of Questionario),
                                            ByVal anonymousUser As String,
                                            ByVal displayNames As Dictionary(Of String, dtoDisplayName),
                                            Optional ByVal oneColumnForEachQuestion As Boolean = True,
                                            Optional onlyUserAnswer As Boolean = False,
                                            Optional userInfo As dtoDisplayName = Nothing) As String


            displayTaxCode = True   'Sì, by design. Se non sono tra gli utenti privilegiati, lo mette a false da solo.

            Dim exportText As String = String.Empty
            Dim nResponse As Integer = (questionnaire.domande.Count - 1)
            Dim dto As New dtoExportQuestionnaire

            Dim index As Integer = 1
            dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleIdAnswer))
            If allowFullExport Then
                dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleSurname))
                dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleName))
                If displayTaxCode Then
                    Select Case userRequiringExport.TypeID
                        Case CInt(UserTypeStandard.SysAdmin), CInt(UserTypeStandard.Administrator), CInt(UserTypeStandard.Administrative)
                            dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleTaxCode))
                        Case Else
                            displayTaxCode = False
                    End Select
                End If
                dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleOtherInfos))
            Else
                dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleUserName))
            End If

            If onlyUserAnswer Then
                For Each item As KeyValuePair(Of Long, QuestionnaireAnswer) In answers
                    dto.Rows.Add(New dtoExportRow() With {.Index = index, .IdAnswer = item.Key, .DisplayInfo = userInfo})

                    index += 1
                Next
            Else

                For Each answer As RispostaQuestionario In questionnaire.risposteQuestionario
                    Dim otherInfo As String = ""
                    If allowFullExport Then
                        If answer.idUtenteInvitato > 0 AndAlso displayNames.ContainsKey("i_" & answer.idUtenteInvitato) Then
                            otherInfo = displayNames("i_" & answer.idUtenteInvitato).OtherInfos
                        ElseIf answer.idPersona > 0 AndAlso displayNames.ContainsKey("p_" & answer.idPersona) Then
                            otherInfo = displayNames("p_" & answer.idPersona).OtherInfos
                        ElseIf displayNames.ContainsKey("p_0") Then
                            otherInfo = ""
                        Else
                            otherInfo = ""
                        End If
                    End If

                    If questionnaire.risultatiAnonimi Then
                        dto.Rows.Add(New dtoExportRow() With {.Index = index, .IdAnswer = answer.id, .DisplayInfo = New dtoDisplayName(anonymousUser) With {.OtherInfos = otherInfo}})
                    Else
                        If answer.idUtenteInvitato > 0 AndAlso displayNames.ContainsKey("i_" & answer.idUtenteInvitato) Then
                            dto.Rows.Add(New dtoExportRow() With {.Index = index, .IdAnswer = answer.id, .DisplayInfo = displayNames("i_" & answer.idUtenteInvitato)})
                        ElseIf answer.idPersona > 0 AndAlso displayNames.ContainsKey("p_" & answer.idPersona) Then
                            dto.Rows.Add(New dtoExportRow() With {.Index = index, .IdAnswer = answer.id, .DisplayInfo = displayNames("p_" & answer.idPersona)})
                        ElseIf displayNames.ContainsKey("p_0") Then
                            dto.Rows.Add(New dtoExportRow() With {.Index = index, .IdAnswer = answer.id, .DisplayInfo = displayNames("p_0")})
                        Else
                            dto.Rows.Add(New dtoExportRow() With {.Index = index, .IdAnswer = answer.id, .DisplayInfo = New dtoDisplayName(anonymousUser)})
                        End If
                    End If
                    index += 1
                Next
            End If

            If questionnaire.tipo = QuestionnaireType.RandomMultipleAttempts OrElse questionnaire.tipo = QuestionnaireType.Random Then
                Dim libraryNumber As Integer = 1
                For Each library As Questionario In libraries
                    _Helper.QuestionsRender(userRequiringExport, dto, libraryNumber, library.domande, answers)
                    libraryNumber += 1
                Next
            Else
                _Helper.QuestionsRender(dto, questionnaire.domande)
            End If

            If dto.Cells.Count > 0 Then
                dto.Cells.ForEach(Sub(c) exportText &= AppendItem(c))
                exportText &= vbCrLf
            End If
            If dto.Rows.Count > 0 Then
                dto.Rows.ForEach(Sub(c) exportText &= c.Export(allowFullExport, displayTaxCode) & EndRowItem)
                exportText &= vbCrLf
            End If
            Return exportText
        End Function

        Public Function QuestionnaireAnswers(
                                            ByVal userRequiringExport As Person,
                                            ByVal allowFullExport As Boolean,
                                            ByVal idLanguage As Integer,
                                            qAnswers As List(Of dtoFullUserAnswerItem),
                                            answers As Dictionary(Of Long, QuestionnaireAnswer),
                                            ByVal anonymousUser As String,
                                            ByVal displayNames As Dictionary(Of String, dtoDisplayName),
                                            ByVal oQuest As Questionario,
                                            Optional onlyUserAnswer As Boolean = False,
                                            Optional userName As String = ""
                                        ) As String

            Dim exportText As String = String.Empty
            Dim dto As New dtoExportQuestionnaire

            Dim addTaxCode As Boolean = False
            Select Case userRequiringExport.TypeID
                Case CInt(UserTypeStandard.SysAdmin),
                     CInt(UserTypeStandard.Administrator),
                     CInt(UserTypeStandard.Administrative)

                    addTaxCode = True
            End Select

            '      Dim questions As List(Of Domanda) = DALDomande.GetQuestionsForStatistics(answers.Values.SelectMany(Function(a) a.Answers).ToList(), idLanguage)
            _Helper.QuestionsRender(
                userRequiringExport,
                addTaxCode,
                allowFullExport,
                dto,
                displayNames,
                anonymousUser,
                oQuest,
                qAnswers,
                answers)

            If dto.Cells.Count > 0 Then
                dto.Cells.ForEach(Sub(c) exportText &= AppendItem(c))
                exportText &= vbCrLf
            End If
            If dto.Rows.Count > 0 Then
                dto.Rows.ForEach(Sub(c) exportText &= c.Export(allowFullExport, addTaxCode) & EndRowItem)
                exportText &= vbCrLf
            End If
            Return exportText
        End Function

        'Private Sub QuestionsRender(ByVal dto As dtoExportQuestionnaire, libraryNumber As Integer, ByVal questions As List(Of Domanda), answers As Dictionary(Of Long, QuestionnaireAnswer))
        '    For Each oDom As Domanda In questions
        '        Select Case oDom.tipo
        '            Case Domanda.TipoDomanda.DropDown
        '                ConvertDropDownToString(oDom, dto, libraryNumber, answers)
        '            Case Domanda.TipoDomanda.Multipla
        '                ConvertMultiplaToString(oDom, dto, libraryNumber, answers)
        '            Case Domanda.TipoDomanda.Numerica
        '                ConvertNumericaToString(oDom, dto, libraryNumber, answers)
        '            Case Domanda.TipoDomanda.Rating
        '                ConvertRatingToString(oDom, dto, libraryNumber, answers)
        '            Case Domanda.TipoDomanda.TestoLibero
        '                ConvertTestoLiberoToString(oDom, dto, libraryNumber, answers)
        '        End Select
        '    Next
        'End Sub
        'Private Sub QuestionsRender(ByVal dto As dtoExportQuestionnaire, ByVal questions As List(Of Domanda))
        '    For Each oDom As Domanda In questions
        '        Select Case oDom.tipo
        '            Case Domanda.TipoDomanda.DropDown
        '                ConvertDropDownToString(oDom, dto)
        '            Case Domanda.TipoDomanda.Multipla
        '                ConvertMultiplaToString(oDom, dto)
        '            Case Domanda.TipoDomanda.Numerica
        '                ConvertNumericaToString(oDom, dto)
        '            Case Domanda.TipoDomanda.Rating
        '                ConvertRatingToString(oDom, dto)
        '            Case Domanda.TipoDomanda.TestoLibero
        '                ConvertTestoLiberoToString(oDom, dto)
        '        End Select
        '    Next
        'End Sub

        'Private Sub ConvertDropDownToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
        '    ' dto.Cells.Add(PageIdentifier & oDom.numeroPagina & QuestionIdentifier & oDom.numero)
        '    dto.Cells.Add(CellTitleIdQuestion)
        '    dto.Cells.Add(GetCellIdentifier(oDom))
        '    For Each row As dtoExportRow In dto.Rows
        '        Dim idAnswer As Integer = row.IdAnswer
        '        Dim numeroOpzList As List(Of Integer) = (From oRisp In oDom.risposteDomanda Where oRisp.idRispostaQuestionario = idAnswer Select oRisp.numeroOpzione).ToList
        '        row.Items.Add(oDom.id)
        '        If numeroOpzList.Count = 0 Then
        '            row.Items.Add(NullItem)
        '        Else
        '            row.Items.Add(numeroOpzList(0).ToString)
        '        End If
        '    Next
        'End Sub
        'Private Sub ConvertDropDownToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, answers As Dictionary(Of Long, QuestionnaireAnswer))
        '    dto.Cells.Add(CellTitleIdQuestion)
        '    dto.Cells.Add(GetCellIdentifier(oDom, libraryNumber))

        '    For Each row As dtoExportRow In dto.Rows
        '        Dim idAnswer As Integer = row.IdAnswer
        '        Dim answer As QuestionnaireAnswer = answers(row.IdAnswer)

        '        row.Items.Add(oDom.id)
        '        If Not IsNothing(answer) Then
        '            Dim idQuestion As Integer = oDom.id
        '            '   Dim question As QuestionAnswer = (From q In answer.Answers Where q.IdQuestion = idQuestion Select q).FirstOrDefault()
        '            If (From q In answer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
        '                Dim numeroOpzList As List(Of Integer) = (From a In answer.Answers Where a.IdQuestion = idQuestion Select a.OptionNumber).ToList
        '                If numeroOpzList.Count = 0 Then
        '                    row.Items.Add(NullItem)
        '                Else
        '                    row.Items.Add(numeroOpzList(0).ToString)
        '                End If
        '            Else
        '                row.Items.Add(QuestionNotUsedIdentifier)
        '            End If
        '        Else
        '            row.Items.Add(QuestionNotUsedIdentifier)
        '        End If
        '    Next
        'End Sub
        'Private Sub ConvertMultiplaToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
        '    Dim firstRun As Boolean = True
        '    Dim nOpzioni As Integer = oDom.domandaMultiplaOpzioni.Count - 1

        '    dto.Cells.Add(CellTitleIdQuestion)

        '    For Each row As dtoExportRow In dto.Rows
        '        Dim idAnswer As Integer = row.IdAnswer
        '        Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Order By oRisp.numeroOpzione Where oRisp.idRispostaQuestionario = idAnswer Select oRisp.numeroOpzione, oRisp.testoOpzione).ToList
        '        row.Items.Add(oDom.id)
        '        Dim listIndex As Integer = 0
        '        For i As Integer = 0 To nOpzioni
        '            If firstRun Then
        '                dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1))
        '            End If

        '            If listIndex < numeroOpzList.count AndAlso numeroOpzList(listIndex).numeroOpzione = i + 1 Then
        '                row.Items.Add("1")

        '                If oDom.domandaMultiplaOpzioni(i).isAltro Then
        '                    row.Items.Add(numeroOpzList(listIndex).testoOpzione)
        '                    If firstRun Then
        '                        dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1) & FreeTextIdentifier)
        '                    End If
        '                End If
        '                listIndex += 1
        '            Else
        '                row.Items.Add("0")
        '                If oDom.domandaMultiplaOpzioni(i).isAltro Then
        '                    row.Items.Add(NULLitem)
        '                    If firstRun Then
        '                        dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1) & FreeTextIdentifier)
        '                    End If
        '                End If
        '            End If
        '        Next
        '        firstRun = False
        '    Next
        'End Sub
        'Private Sub ConvertMultiplaToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, answers As Dictionary(Of Long, QuestionnaireAnswer))
        '    Dim firstRun As Boolean = True
        '    Dim nOpzioni As Integer = oDom.domandaMultiplaOpzioni.Count - 1

        '    dto.Cells.Add(CellTitleIdQuestion)
        '    For Each row As dtoExportRow In dto.Rows
        '        Dim idAnswer As Integer = row.IdAnswer
        '        If idAnswer = 8244 Then
        '            idAnswer = 8244
        '        End If
        '        Dim qAnswer As QuestionnaireAnswer = answers(row.IdAnswer)
        '        Dim idQuestion As Integer = oDom.id

        '        row.Items.Add(idQuestion.ToString)
        '        If Not (From q In qAnswer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
        '            Dim listIndex As Integer = 0
        '            For i As Integer = 0 To nOpzioni
        '                If firstRun Then
        '                    dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber))
        '                End If
        '                row.Items.Add(QuestionNotUsedIdentifier)
        '            Next
        '        Else
        '            Dim numeroOpzList = (From a In qAnswer.Answers Where a.IdQuestion = idQuestion Order By a.OptionNumber Select OptionNumber = a.OptionNumber, OptionText = a.OptionText).ToList

        '            Dim listIndex As Integer = 0
        '            For i As Integer = 0 To nOpzioni
        '                If firstRun Then
        '                    dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber))
        '                End If

        '                If listIndex < numeroOpzList.count AndAlso numeroOpzList(listIndex).OptionNumber = i + 1 Then
        '                    row.Items.Add("1")

        '                    If oDom.domandaMultiplaOpzioni(i).isAltro Then
        '                        row.Items.Add(numeroOpzList(listIndex).OptionText)
        '                        If firstRun Then
        '                            dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber).ToString & FreeTextIdentifier)
        '                        End If
        '                    End If
        '                    listIndex += 1
        '                Else
        '                    row.Items.Add("0")
        '                    If oDom.domandaMultiplaOpzioni(i).isAltro Then
        '                        row.Items.Add(NullItem)
        '                        If firstRun Then
        '                            dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber).ToString & FreeTextIdentifier)
        '                        End If
        '                    End If
        '                End If
        '            Next
        '        End If


        '        firstRun = False
        '    Next
        'End Sub
        'Private Sub ConvertNumericaToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
        '    Dim nOpzioni As Integer = oDom.opzioniNumerica.Count - 1

        '    dto.Cells.Add(CellTitleIdQuestion)
        '    For i As Integer = 0 To nOpzioni
        '        dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1))
        '    Next

        '    For Each row As dtoExportRow In dto.Rows
        '        Dim idAnswer As Integer = row.IdAnswer
        '        Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Where oRisp.idRispostaQuestionario = idAnswer Order By oRisp.numeroOpzione Select oRisp.valore, oRisp.numeroOpzione).ToList
        '        row.Items.Add(oDom.id)
        '        For i As Integer = 0 To nOpzioni
        '            If IsNothing(numeroOpzList) OrElse numeroOpzList.Count = 0 Then
        '                '"##############################"
        '                row.Items.Add(NullItem)
        '            Else
        '                If Not numeroOpzList(i).valore = Integer.MinValue Then 'valore di default quando la risposta non e' stata inserita
        '                    row.Items.Add(numeroOpzList(i).valore.ToString)
        '                Else
        '                    row.Items.Add(NullItem)
        '                End If
        '            End If
        '        Next
        '    Next
        'End Sub
        'Private Sub ConvertNumericaToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, answers As Dictionary(Of Long, QuestionnaireAnswer))
        '    Dim nOpzioni As Integer = oDom.opzioniNumerica.Count - 1
        '    dto.Cells.Add(CellTitleIdQuestion)
        '    For i As Integer = 0 To nOpzioni
        '        dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber))
        '    Next

        '    For Each row As dtoExportRow In dto.Rows
        '        Dim idAnswer As Integer = row.IdAnswer
        '        Dim qAnswer As QuestionnaireAnswer = answers(row.IdAnswer)
        '        row.Items.Add(oDom.id)
        '        If Not IsNothing(qAnswer) Then
        '            Dim idQuestion As Integer = oDom.id

        '            Dim numeroOpzList = Nothing
        '            If (From q In qAnswer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
        '                numeroOpzList = (From a In qAnswer.Answers Where a.IdQuestion = idQuestion Order By a.OptionNumber Select Value = a.Value, OptionNumber = a.OptionNumber).ToList
        '            End If

        '            For i As Integer = 0 To nOpzioni
        '                If Not (From q In qAnswer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
        '                    row.Items.Add(QuestionNotUsedIdentifier)
        '                ElseIf IsNothing(numeroOpzList) OrElse numeroOpzList.Count = 0 Then
        '                    row.Items.Add(NullItem)
        '                Else
        '                    If Not numeroOpzList(i).Value = Integer.MinValue.ToString Then 'valore di default quando la risposta non e' stata inserita
        '                        row.Items.Add(numeroOpzList(i).Value.ToString)
        '                    Else
        '                        row.Items.Add(NullItem)
        '                    End If
        '                End If
        '            Next
        '        Else
        '            For i As Integer = 0 To nOpzioni
        '                row.Items.Add(QuestionNotUsedIdentifier)
        '            Next
        '        End If
        '    Next
        'End Sub
        'Private Sub ConvertTestoLiberoToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
        '    Dim nOpzioni As Integer = oDom.opzioniTestoLibero.Count - 1
        '    dto.Cells.Add(CellTitleIdQuestion)
        '    For i As Integer = 0 To nOpzioni
        '        dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1) & FreeTextIdentifier)
        '    Next
        '    For Each row As dtoExportRow In dto.Rows
        '        Dim idAnswer As Integer = row.IdAnswer
        '        Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Where oRisp.idRispostaQuestionario = idAnswer Order By oRisp.numeroOpzione Select oRisp.testoOpzione, oRisp.numeroOpzione).ToList
        '        row.Items.Add(oDom.id)
        '        For i As Integer = 0 To nOpzioni
        '            If i < numeroOpzList.count AndAlso (numeroOpzList(i).numeroOpzione = i + 1 Or (oDom.opzioniTestoLibero.Count = 1 AndAlso numeroOpzList(i).numeroOpzione = 0)) Then
        '                'quando ho una TL con una sola opzione, l'indice dell'opzione parte da 0. verificare.
        '                row.Items.Add(numeroOpzList(i).testoOpzione)
        '            Else
        '                row.Items.Add(NullItem)
        '            End If
        '        Next
        '    Next
        'End Sub
        'Private Sub ConvertTestoLiberoToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, answers As Dictionary(Of Long, QuestionnaireAnswer))
        '    Dim nOpzioni As Integer = oDom.opzioniTestoLibero.Count - 1

        '    dto.Cells.Add(CellTitleIdQuestion)
        '    For i As Integer = 0 To nOpzioni
        '        dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber) & FreeTextIdentifier)
        '    Next
        '    For Each row As dtoExportRow In dto.Rows
        '        Dim idAnswer As Integer = row.IdAnswer
        '        Dim qAnswer As QuestionnaireAnswer = answers(row.IdAnswer)
        '        row.Items.Add(oDom.id)
        '        If Not IsNothing(qAnswer) Then
        '            Dim idQuestion As Integer = CLng(oDom.id)
        '            If Not (From q In qAnswer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
        '                For i As Integer = 0 To nOpzioni
        '                    row.Items.Add(QuestionNotUsedIdentifier)
        '                Next
        '            Else
        '                Dim numeroOpzList = (From a In qAnswer.Answers Where a.IdQuestion = idQuestion Order By a.OptionNumber Select OptionText = a.OptionText, OptionNumber = a.OptionNumber).ToList
        '                For i As Integer = 0 To nOpzioni
        '                    If i < numeroOpzList.count AndAlso (numeroOpzList(i).OptionNumber = i + 1 Or (oDom.opzioniTestoLibero.Count = 1 AndAlso numeroOpzList(i).OptionNumber = 0)) Then
        '                        'quando ho una TL con una sola opzione, l'indice dell'opzione parte da 0. verificare.
        '                        row.Items.Add(numeroOpzList(i).OptionText)
        '                    Else
        '                        row.Items.Add(NullItem)
        '                    End If
        '                Next
        '            End If
        '        Else
        '            For i As Integer = 0 To nOpzioni
        '                row.Items.Add(QuestionNotUsedIdentifier)
        '            Next
        '        End If
        '    Next
        'End Sub

        'Private Sub ConvertRatingToString(ByVal oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
        '    Dim firstRun As Boolean = True
        '    Dim nOpzioni As Integer = oDom.domandaRating.opzioniRating.Count - 1

        '    dto.Cells.Add(CellTitleIdQuestion)
        '    For Each row As dtoExportRow In dto.Rows
        '        Dim idAnswer As Integer = row.IdAnswer

        '        row.Items.Add(oDom.id)
        '        For j As Integer = 0 To nOpzioni
        '            Dim isSelectedRow As Boolean = False
        '            Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Order By oRisp.idDomandaOpzione Where oRisp.idRispostaQuestionario = idAnswer And oRisp.idDomandaOpzione = oDom.domandaRating.opzioniRating(j).id Select oRisp.valore, oRisp.testoOpzione).ToList
        '            For i As Integer = 0 To oDom.domandaRating.numeroRating
        '                If numeroOpzList.count > 0 AndAlso numeroOpzList(0).valore = i + 1 Then
        '                    row.Items.Add("1")
        '                    isSelectedRow = True
        '                Else
        '                    row.Items.Add("0")
        '                End If
        '                If firstRun Then
        '                    dto.Cells.Add(GetCellOptionIdentifier(oDom, j + 1) & "," & (i + 1).ToString)
        '                End If
        '            Next
        '            If oDom.domandaRating.opzioniRating(j).isAltro Then
        '                If isSelectedRow Then
        '                    row.Items.Add(numeroOpzList(0).testoOpzione)
        '                Else
        '                    row.Items.Add(NULLitem)
        '                End If
        '                If firstRun Then
        '                    dto.Cells.Add(GetCellOptionIdentifier(oDom, j + 1) & FreeTextIdentifier)
        '                End If
        '            End If
        '        Next
        '        firstRun = False
        '    Next
        'End Sub
        'Private Sub ConvertRatingToString(ByVal oDom As Domanda, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, answers As Dictionary(Of Long, QuestionnaireAnswer))
        '    Dim firstRun As Boolean = True
        '    Dim nOpzioni As Integer = oDom.domandaRating.opzioniRating.Count - 1

        '    dto.Cells.Add(CellTitleIdQuestion)
        '    For Each row As dtoExportRow In dto.Rows
        '        Dim idAnswer As Integer = row.IdAnswer
        '        Dim answer As QuestionnaireAnswer = answers(row.IdAnswer)
        '        row.Items.Add(oDom.id)
        '        If Not IsNothing(answer) Then
        '            Dim idQuestion As Integer = oDom.id
        '            'Dim question As Domanda = (From oRisp In quest.domande Where oRisp.id = idQuestion Select oRisp).FirstOrDefault()


        '            If Not (From q In answer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
        '                SetNotUsedRating(row, dto, libraryNumber, oDom, nOpzioni, firstRun)
        '            Else
        '                For j As Integer = 0 To nOpzioni
        '                    Dim isSelectedRow As Boolean = False
        '                    Dim index As Integer = j
        '                    Dim numeroOpzList = (From a In answer.Answers Where a.IdQuestion = idQuestion Order By a.IdQuestionOption Where a.IdQuestionOption = oDom.domandaRating.opzioniRating(index).id Select Value = a.Value, OptionText = a.OptionText).ToList
        '                    For i As Integer = 0 To oDom.domandaRating.numeroRating
        '                        If numeroOpzList.count > 0 AndAlso numeroOpzList(0).Value = i + 1 Then
        '                            row.Items.Add("1")
        '                            isSelectedRow = True
        '                        Else
        '                            row.Items.Add("0")
        '                        End If
        '                        If firstRun Then
        '                            dto.Cells.Add(GetCellOptionIdentifier(oDom, j + 1, libraryNumber) & "-" & (i + 1).ToString)
        '                        End If
        '                    Next
        '                    If oDom.domandaRating.opzioniRating(j).isAltro Then
        '                        If isSelectedRow Then
        '                            row.Items.Add(numeroOpzList(0).OptionText)
        '                        Else
        '                            row.Items.Add(NullItem)
        '                        End If
        '                        If firstRun Then
        '                            dto.Cells.Add(GetCellOptionIdentifier(oDom, j + 1, libraryNumber) & FreeTextIdentifier)
        '                        End If
        '                    End If
        '                Next
        '            End If
        '        Else
        '            SetNotUsedRating(row, dto, libraryNumber, oDom, nOpzioni, firstRun)
        '        End If

        '        firstRun = False
        '    Next
        'End Sub

        'Private Sub SetNotUsedRating(row As dtoExportRow, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, ByVal oDom As Domanda, optionsNumber As Integer, firstRun As Boolean)
        '    For j As Integer = 0 To optionsNumber
        '        For i As Integer = 0 To oDom.domandaRating.numeroRating
        '            row.Items.Add(QuestionNotUsedIdentifier)
        '            If firstRun Then
        '                dto.Cells.Add(LibraryIdentifier & libraryNumber & QuestionIdentifier & oDom.numero & OptionIdentifier & (j + 1).ToString & "-" & (i + 1).ToString)
        '            End If
        '        Next
        '        If oDom.domandaRating.opzioniRating(j).isAltro Then
        '            row.Items.Add(QuestionNotUsedIdentifier)
        '            If firstRun Then
        '                dto.Cells.Add(LibraryIdentifier & oDom.numeroPagina & QuestionIdentifier & oDom.numero & OptionIdentifier & (j + 1).ToString & FreeTextIdentifier)
        '            End If
        '        End If
        '    Next
        'End Sub


        'Private Function GetCellIdentifier(ByRef oDom As Domanda, Optional ByVal libraryNumber As Integer = -200) As String
        '    If libraryNumber <> -200 Then
        '        Return LibraryIdentifier & libraryNumber & IIf(oDom.numero > 0, QuestionIdentifier & oDom.numero, DeletedQuestionIdentifier & oDom.VirtualNumber)
        '    Else
        '        Return PageIdentifier & oDom.numeroPagina & IIf(oDom.numero > 0, QuestionIdentifier & oDom.numero, DeletedQuestionIdentifier & oDom.VirtualNumber)
        '    End If
        'End Function

        'Private Function GetCellOptionIdentifier(ByRef oDom As Domanda, ByVal optionNumber As Integer, Optional ByVal libraryNumber As Integer = -200) As String
        '    If libraryNumber <> -200 Then
        '        Return LibraryIdentifier & libraryNumber & IIf(oDom.numero > 0, QuestionIdentifier & oDom.numero, DeletedQuestionIdentifier & oDom.VirtualNumber) & OptionIdentifier & optionNumber.ToString
        '    Else
        '        Return PageIdentifier & oDom.numeroPagina & IIf(oDom.numero > 0, QuestionIdentifier & oDom.numero, DeletedQuestionIdentifier & oDom.VirtualNumber) & OptionIdentifier & optionNumber.ToString
        '    End If
        'End Function
#End Region

#Region "Attempts Statistics"
        Public Function QuestionnaireAttempts(minScore As Integer, maxScore As Integer, answers As List(Of dtoUserAnswerItem), multipleAttempts As Boolean) As String
            Dim export As String = ""
            Dim content As String = ""
            Dim hasCompleted As Boolean = answers.Where(Function(a) a.CompletedOn.HasValue).Any()
            Dim hasSemiCorrectAnswers As Boolean = answers.Where(Function(a) a.SemiCorrectAnswers.HasValue).Any()
            Dim hasUngradedAnswers As Boolean = answers.Where(Function(a) a.UngradedAnswers.HasValue).Any()
            Dim hasQuestionsSkipped As Boolean = answers.Where(Function(a) a.QuestionsSkipped.HasValue).Any()

            Dim header As String = AttemptsHeader(answers, multipleAttempts, hasCompleted, hasSemiCorrectAnswers, hasUngradedAnswers, hasQuestionsSkipped)
            Dim rowNumber As Integer = 0
            For Each answer As dtoUserAnswerItem In answers
                content += QuestionnaireAttemptRow(minScore, maxScore, answer, multipleAttempts, hasCompleted, hasSemiCorrectAnswers, hasUngradedAnswers, hasQuestionsSkipped, rowNumber)
            Next
            Return header & content
        End Function
        Private Function AttemptsHeader(answers As List(Of dtoUserAnswerItem), multipleAttempts As Boolean, hasCompleted As Boolean, hasSemiCorrectAnswers As Boolean, hasUngradedAnswers As Boolean, hasQuestionsSkipped As Boolean) As String
            Dim header As String = AppendItem(CommonTranslations(QuestionnaireExportTranslations.CellTitleIdAnswer))
            header += AppendItem(CommonTranslations(QuestionnaireExportTranslations.CellTitleUserName))
            If multipleAttempts Then
                header += AppendItem(CommonTranslations(QuestionnaireExportTranslations.CellTitleAttemptNumber))
            End If
            If hasCompleted Then
                header += AppendItem(CommonTranslations(QuestionnaireExportTranslations.CellTitleAttemptDate))
            End If
            header += AppendItem(CommonTranslations(QuestionnaireExportTranslations.CellTitleQuestionsCount))
            header += AppendItem(CommonTranslations(QuestionnaireExportTranslations.CellTitleCorrectAnswers))
            If hasSemiCorrectAnswers Then
                header += AppendItem(CommonTranslations(QuestionnaireExportTranslations.CellTitleSemiCorrectAnswers))
            End If
            If hasUngradedAnswers Then
                header += AppendItem(CommonTranslations(QuestionnaireExportTranslations.CellTitleUngradedAnswers))
            End If
            If answers.Where(Function(a) a.QuestionsSkipped.HasValue).Any() Then
                header += AppendItem(CommonTranslations(QuestionnaireExportTranslations.CellTitleQuestionsSkipped))
            End If
            header += AppendItem(CommonTranslations(QuestionnaireExportTranslations.CellTitleWrongAnswers))
            header += AppendItem(CommonTranslations(QuestionnaireExportTranslations.CellTitleAttemptScore))
            header += AppendItem(CommonTranslations(QuestionnaireExportTranslations.CellTitleMinScore))
            header += AppendItem(CommonTranslations(QuestionnaireExportTranslations.CellTitleMaxScore))

            Return header & EndRowItem
        End Function
        Private Function QuestionnaireAttemptRow(minScore As Integer, maxScore As Integer, answer As dtoUserAnswerItem, multipleAttempts As Boolean, hasCompleted As Boolean, hasSemiCorrectAnswers As Boolean, hasUngradedAnswers As Boolean, hasQuestionsSkipped As Boolean, rowNumber As Integer) As String
            Dim row As String = ""

            row += AppendItem(answer.Id)
            row += AppendItem(answer.DisplayName)

            If multipleAttempts Then
                row += AppendItem(answer.AttemptNumber)
            End If

            If hasCompleted Then
                If answer.CompletedOn.HasValue Then
                    row += AppendItem(answer.CompletedOn)
                Else
                    row += AppendItem(CommonTranslations(QuestionnaireExportTranslations.ItemEmpty))
                End If
            End If
            If answer.QuestionsCount.HasValue Then
                row += AppendItem(answer.QuestionsCount.Value)
            Else
                row += AppendItem(0)
            End If
            If answer.CorrectAnswers.HasValue Then
                row += AppendItem(answer.CorrectAnswers.Value)
            Else
                row += AppendItem(0)
            End If
            If hasSemiCorrectAnswers Then
                If answer.SemiCorrectAnswers.HasValue Then
                    row += AppendItem(answer.SemiCorrectAnswers.Value)
                Else
                    row += AppendItem(0)
                End If
            End If
            If hasUngradedAnswers Then
                If answer.UngradedAnswers.HasValue Then
                    row += AppendItem(answer.UngradedAnswers.Value)
                Else
                    row += AppendItem(0)
                End If
            End If
            If hasQuestionsSkipped Then
                If answer.QuestionsSkipped.HasValue Then
                    row += AppendItem(answer.QuestionsSkipped.Value)
                Else
                    row += AppendItem(0)
                End If
            End If
            If answer.WrongAnswers.HasValue Then
                row += AppendItem(answer.WrongAnswers.Value)
            Else
                row += AppendItem(0)
            End If

            row += AppendItem(answer.RelativeScore)
            row += AppendItem(minScore)
            row += AppendItem(maxScore)

            Return row & EndRowItem
        End Function
#End Region

    End Class
End Namespace