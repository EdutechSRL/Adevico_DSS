Imports lm.Comol.Core.DomainModel
Imports Telerik.Web.UI
Imports lm.Comol.Core.Business
Imports lm.Comol.Core.DomainModel.Helpers

Namespace Business
    Public Class BaseHelperExport

#Region "Translations"
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
        Private ReadOnly Property CellTitleIdOption
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleIdOption)
            End Get
        End Property
        Private ReadOnly Property CellTitleOptionNumber
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleOptionNumber)
            End Get
        End Property
        Private ReadOnly Property CellTitleOptionValue
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleOptionValue)
            End Get
        End Property
        Private ReadOnly Property CellTitleOptionFreeText
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleOptionFreeText)
            End Get
        End Property
        Private ReadOnly Property CellTitleEvaluation
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleEvaluation)
            End Get
        End Property
        Private ReadOnly Property CellTitleOptionCorrect
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleOptionCorrect)
            End Get
        End Property
        Private ReadOnly Property CellTitleOptionPoints
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleOptionPoints)
            End Get
        End Property

        Private ReadOnly Property CellTitleIdUser
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleIdUser)
            End Get
        End Property
        Private ReadOnly Property CellTitleIdInvitedUser
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleIdInvitedUser)
            End Get
        End Property

        Private ReadOnly Property CellTitleStartedOn
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleStartedOn)
            End Get
        End Property
        Private ReadOnly Property CellTitleModifiedOn
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleModifiedOn)
            End Get
        End Property
        Private ReadOnly Property CellTitleEndedOn
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleEndedOn)
            End Get
        End Property
        Private ReadOnly Property CellTitleOptionText
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleOptionText)
            End Get
        End Property

#End Region

        Public Sub New()
        End Sub
        Public Sub New(translations As Dictionary(Of QuestionnaireExportTranslations, String))
            CommonTranslations = translations
        End Sub

#Region "Export Questionnaire"
        Public Sub QuestionsRender(ByVal userRequiringExport As Person, ByVal dto As dtoExportQuestionnaire, libraryNumber As Integer, ByVal questions As List(Of Domanda), answers As Dictionary(Of Long, QuestionnaireAnswer))
            For Each oDom As Domanda In questions
                Select Case oDom.tipo
                    Case Domanda.TipoDomanda.DropDown
                        ConvertDropDownToString(oDom, dto, libraryNumber, answers)
                    Case Domanda.TipoDomanda.Multipla
                        ConvertMultiplaToString(oDom, dto, libraryNumber, answers)
                    Case Domanda.TipoDomanda.Numerica
                        ConvertNumericaToString(oDom, dto, libraryNumber, answers)
                    Case Domanda.TipoDomanda.Rating
                        ConvertRatingToString(oDom, dto, libraryNumber, answers)
                    Case Domanda.TipoDomanda.TestoLibero
                        ConvertTestoLiberoToString(oDom, dto, libraryNumber, answers)
                End Select
            Next
        End Sub

        'Aggiungere QUESTIONARIO per avere TUTTE le informazioni!
        Public Sub QuestionsRender(
                                  ByVal currentuser As Person,
                                  ByVal displayTaxCode As Boolean,
                                  ByVal allowFullExport As Boolean,
                                  ByVal dto As dtoExportQuestionnaire,
                                  ByVal displayNames As Dictionary(Of String, dtoDisplayName),
                                  ByVal anonymousUser As String,
                                  ByVal colQuestionario As COL_Questionario.Questionario,
                                  qAnswers As List(Of dtoFullUserAnswerItem),
                                  answers As Dictionary(Of Long, QuestionnaireAnswer)
                                  )


            'Dictionari Id-Testo delle domande/risposte del questionario

            Dim dicQuestions As New Dictionary(Of Integer, String)()
            Dim dicOption As New Dictionary(Of Integer, String)()

            'Dim dicRatingEmptyOptionValue As New Dictionary(Of Integer, String)()
            'Dim dicRatingEmptyOptionText As New Dictionary(Of Integer, String)()
            Dim dicRatingOptionHeader As New Dictionary(Of String, String)()

            For Each sourceDomanda As Domanda In colQuestionario.domande
                If Not dicQuestions.ContainsKey(sourceDomanda.id) Then
                    dicQuestions.Add(sourceDomanda.id, sourceDomanda.testo)
                End If

                Select Case sourceDomanda.tipo

                    Case Domanda.TipoDomanda.DropDown
                        If Not IsNothing(sourceDomanda.domandaDropDown) Then
                            For Each itemOpt As DropDownItem In sourceDomanda.domandaDropDown.dropDownItems
                                If Not dicOption.ContainsKey(itemOpt.id) Then
                                    dicOption.Add(itemOpt.id, itemOpt.testo)
                                End If
                            Next
                        End If

                    Case Domanda.TipoDomanda.TestoLibero
                        For Each itemOpt As DomandaTestoLibero In sourceDomanda.opzioniTestoLibero
                            If Not dicOption.ContainsKey(itemOpt.id) Then
                                dicOption.Add(itemOpt.id, itemOpt.etichetta)
                            End If
                        Next


                    Case Domanda.TipoDomanda.Multipla
                        For Each itemOpt As DomandaOpzione In sourceDomanda.domandaMultiplaOpzioni
                            If Not dicOption.ContainsKey(itemOpt.id) Then
                                dicOption.Add(itemOpt.id, itemOpt.testo)
                            End If
                        Next

                    Case Domanda.TipoDomanda.Numerica
                        For Each itemOpt As DomandaNumerica In sourceDomanda.opzioniNumerica
                            If Not dicOption.ContainsKey(itemOpt.id) Then
                                dicOption.Add(itemOpt.id, itemOpt.testoPrima)
                            End If
                        Next

                    Case Domanda.TipoDomanda.Rating,
                         Domanda.TipoDomanda.RatingStars

                        If Not IsNothing(sourceDomanda.domandaRating) Then

                            For Each itemOpt As DomandaOpzione In sourceDomanda.domandaRating.opzioniRating
                                If Not dicOption.ContainsKey(itemOpt.id) Then
                                    dicOption.Add(itemOpt.id, itemOpt.testo)
                                End If
                            Next

                            If sourceDomanda.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Testi Then
                                For Each HeaderValue As DomandaOpzione In sourceDomanda.domandaRating.intestazioniRating
                                    Dim Key As String = String.Format("{0}-{1}", sourceDomanda.id, HeaderValue.numero) 'HeaderValue.id
                                    If Not String.IsNullOrEmpty(Key) Then
                                        If Not IsNothing(HeaderValue) AndAlso Not dicRatingOptionHeader.ContainsKey(Key) Then
                                            dicRatingOptionHeader.Add(Key, HeaderValue.testo)
                                        End If
                                    End If
                                Next
                            End If

                            If sourceDomanda.domandaRating.mostraND Then
                                'Dim intVal As Integer = 1

                                Dim Key As String = String.Format("{0}-{1}", sourceDomanda.id, sourceDomanda.domandaRating.opzioniRating.Count())

                                If Not dicRatingOptionHeader.ContainsKey(Key) Then
                                    dicRatingOptionHeader.Add(Key, sourceDomanda.domandaRating.testoND)
                                End If


                                'If Not dicRatingEmptyOptionValue.ContainsKey(sourceDomanda.domandaRating.id) Then
                                '    dicRatingEmptyOptionValue.Add(sourceDomanda.domandaRating.id, sourceDomanda.domandaRating.testoND)
                                'End If

                                'If Not dicRatingEmptyOptionText.ContainsKey(sourceDomanda.domandaRating.id) Then
                                '    dicRatingEmptyOptionText.Add(sourceDomanda.domandaRating.id, sourceDomanda.domandaRating.opzioniRating.Count() + 1)
                                'End If
                            End If




                        End If
                End Select
            Next

            'Intestazione
            'dto.Cells => celle intestazione (prima riga)

            dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleIdAnswer))

            If allowFullExport Then
                dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleSurname))
                dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleName))
                If displayTaxCode Then
                    Select Case currentuser.TypeID
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


            dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleIdUser))
            dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleIdInvitedUser))
            dto.Cells.Add(CellTitleStartedOn)
            dto.Cells.Add(CellTitleModifiedOn)
            dto.Cells.Add(CellTitleEndedOn)

            dto.Cells.Add(CellTitleIdQuestion)
            dto.Cells.Add("Testo domanda")
            dto.Cells.Add("Tipo Domanda")
            'dto.Cells.Add(CellTitleOptionCorrect)
            dto.Cells.Add(CellTitleIdOption)
            'dto.Cells.Add(CellTitleOptionNumber)
            dto.Cells.Add(CellTitleOptionValue)
            'dto.Cells.Add("Testo opzione")
            dto.Cells.Add(CellTitleOptionText)

            dto.Cells.Add(CellTitleOptionFreeText)
            dto.Cells.Add(CellTitleEvaluation)
            '  dto.Cells.Add(CellTitleOptionPoints)

            'Dati:
            'dto.Rows  => righe dati (successive)

            Dim index As Long = 1
            For Each qAnswerItem As dtoFullUserAnswerItem In qAnswers           '<- per ogni questionario

                Dim answer As QuestionnaireAnswer = answers(qAnswerItem.Id)

                If Not IsNothing(answer) Then
                    Dim info As New List(Of String)
                    'ID risposta
                    info.Add(answer.IdUser)

                    info.Add(answer.IdInvitedUser)
                    If qAnswerItem.Answer.StartedOn.HasValue Then
                        info.Add(qAnswerItem.Answer.StartedOn.Value)
                    Else
                        info.Add("")
                    End If
                    If qAnswerItem.Answer.ModifiedOn.HasValue Then
                        info.Add(qAnswerItem.Answer.ModifiedOn.Value)
                    Else
                        info.Add("")
                    End If
                    If qAnswerItem.Answer.CompletedOn.HasValue Then
                        info.Add(qAnswerItem.Answer.CompletedOn.Value)
                    Else
                        info.Add("")
                    End If

                    For Each item As LazyAssociatedQuestion In answer.Questions.OrderBy(Function(q) q.Number).ThenBy(Function(q) q.IdQuestion)
                        Dim row As dtoExportRow = GenerateAnswerRow(index, qAnswerItem, displayNames, anonymousUser)
                        row.Items.AddRange(info)

                        'Id domanda
                        row.Items.Add(item.IdQuestion)

                        Dim QuestionText As String = ""
                        'Testo domanda
                        If dicQuestions.ContainsKey(item.IdQuestion) Then
                            QuestionText = dicQuestions(item.IdQuestion)
                        End If

                        Dim idQuestion As Integer = item.IdQuestion
                        Dim questionAnswer As QuestionAnswer = answer.Answers.Where(Function(a) a.IdQuestion.Equals(idQuestion)).FirstOrDefault()

                        QuestionText = TxtHelper_HtmlToString(QuestionText)

                        'Se non ho risposte, metto una riga vuota...
                        If IsNothing(questionAnswer) Then
                            row.Items.Add(QuestionText)
                            For i As Integer = 1 To 7
                                row.Items.Add("")
                            Next
                        Else

                            Dim QuestionType As String = String.Format("{0}", questionAnswer.QuestionType)

                            Select Case questionAnswer.QuestionType

                                Case Domanda.TipoDomanda.DropDown   'OK! - 12/04/2018
                                    row.Items.Add(QuestionText)
                                    row.Items.Add(QuestionType)

                                    'Id Opzione
                                    row.Items.Add(questionAnswer.IdQuestionOption)

                                    Dim newRow As dtoExportRow = row.Copy()
                                    newRow.Index = index

                                    'Numerica: ok option text
                                    newRow.Items.Add(questionAnswer.OptionNumber)
                                    newRow.Items.Add(TxtHelper_HtmlToString(questionAnswer.OptionText))

                                    newRow.Items.Add("")
                                    newRow.Items.Add("")
                                    newRow.Items.Add("")

                                    index += 1
                                    dto.Rows.Add(newRow)

                                Case Domanda.TipoDomanda.TestoLibero    'OK! 12/04/2018
                                    'row.Items.Add(QuestionText)
                                    'row.Items.Add(QuestionType)
                                    For Each qMultiAns As QuestionAnswer In answer.Answers.Where(Function(ans) ans.IdQuestion = questionAnswer.IdQuestion).ToList()

                                        Dim newRow As dtoExportRow = row.Copy()
                                        newRow.Index = index

                                        'row.Items.Add(QuestionText)

                                        'Testo opzione
                                        If dicOption.ContainsKey(qMultiAns.IdQuestionOption) Then
                                            newRow.Items.Add(dicOption(qMultiAns.IdQuestionOption))
                                        Else
                                            newRow.Items.Add(TxtHelper_HtmlToString(QuestionText))
                                        End If

                                        'newRow.Items.Add(qMultiAns.OptionText)
                                        newRow.Items.Add(QuestionType)

                                        'Id Opzione
                                        newRow.Items.Add(questionAnswer.IdQuestionOption)

                                        newRow.Items.Add(qMultiAns.OptionNumber)



                                        newRow.Items.Add(qMultiAns.OptionText)
                                        newRow.Items.Add(qMultiAns.Evaluation)

                                        index += 1
                                        dto.Rows.Add(newRow)
                                    Next

                                Case Domanda.TipoDomanda.Multipla   'OK!    12/04/2018
                                    row.Items.Add(QuestionText)
                                    row.Items.Add(QuestionType)
                                    For Each qMultiAns As QuestionAnswer In answer.Answers.Where(Function(ans) ans.IdQuestion = questionAnswer.IdQuestion).ToList()

                                        Dim newRow As dtoExportRow = row.Copy()
                                        newRow.Index = index

                                        'Id Opzione
                                        newRow.Items.Add(qMultiAns.IdQuestionOption)
                                        newRow.Items.Add(qMultiAns.OptionNumber)
                                        If dicOption.ContainsKey(qMultiAns.IdQuestionOption) Then
                                            newRow.Items.Add(TxtHelper_HtmlToString(dicOption(qMultiAns.IdQuestionOption)))
                                        Else
                                            newRow.Items.Add("")
                                        End If

                                        newRow.Items.Add(qMultiAns.Value)
                                        newRow.Items.Add("")
                                        newRow.Items.Add("")

                                        index += 1
                                        dto.Rows.Add(newRow)
                                    Next

                                Case Domanda.TipoDomanda.Numerica   'OK! - 12/04/2018

                                    For Each qMultiAns As QuestionAnswer In answer.Answers.Where(Function(ans) ans.IdQuestion = questionAnswer.IdQuestion).ToList()

                                        Dim newRow As dtoExportRow = row.Copy()
                                        newRow.Index = index

                                        'row.Items.Add(QuestionText)
                                        If dicOption.ContainsKey(qMultiAns.IdQuestionOption) Then
                                            newRow.Items.Add(TxtHelper_HtmlToString(dicOption(qMultiAns.IdQuestionOption)))
                                        Else
                                            newRow.Items.Add(TxtHelper_HtmlToString(qMultiAns.OptionText))
                                        End If

                                        newRow.Items.Add(QuestionType)

                                        'Id Opzione
                                        newRow.Items.Add(qMultiAns.IdQuestionOption)
                                        newRow.Items.Add(qMultiAns.OptionNumber)

                                        newRow.Items.Add(qMultiAns.Value)
                                        newRow.Items.Add("")
                                        newRow.Items.Add(qMultiAns.Evaluation)


                                        index += 1
                                        dto.Rows.Add(newRow)
                                    Next

                                Case Domanda.TipoDomanda.Rating, Domanda.TipoDomanda.RatingStars

                                    For Each qMultiAns As QuestionAnswer In answer.Answers.Where(Function(ans) ans.IdQuestion = questionAnswer.IdQuestion).ToList()

                                        Dim newRow As dtoExportRow = row.Copy()

                                        'Testo opzione
                                        Dim questText As String = ""

                                        If dicOption.ContainsKey(qMultiAns.IdQuestionOption) Then
                                            questText = dicOption(qMultiAns.IdQuestionOption)
                                        End If

                                        If String.IsNullOrWhiteSpace(questText) Then
                                            questText = QuestionText
                                        End If

                                        newRow.Items.Add(TxtHelper_HtmlToString(questText))

                                        newRow.Items.Add(QuestionType)
                                        newRow.Index = index

                                        'ID opzione
                                        newRow.Items.Add(qMultiAns.IdQuestionOption)

                                        'Valore Opzione
                                        newRow.Items.Add(qMultiAns.OptionNumber)

                                        'Testo inserito (rating)
                                        Dim optKey As String = String.Format("{0}-{1}", qMultiAns.IdQuestion, qMultiAns.Value)
                                        'Dim Key As String = String.Format("{0}-{1}", HeaderValue.id, HeaderValue.numero)

                                        Dim optionText As String = ""
                                        If Not String.IsNullOrEmpty(optKey) AndAlso dicRatingOptionHeader.ContainsKey(optKey) Then
                                            optionText = dicRatingOptionHeader(optKey)
                                        Else

                                            Dim intValue As Int32 = 0
                                            If IsNumeric(qMultiAns.Value) Then
                                                intValue = CInt(qMultiAns.Value)
                                            End If

                                            'If dicRatingEmptyOptionValue.ContainsKey(qMultiAns.IdQuestionOption) _
                                            '    AndAlso dicRatingEmptyOptionValue(qMultiAns.IdQuestionOption) = intValue Then
                                            '    optionText = dicRatingEmptyOptionValue.ContainsKey(qMultiAns.IdQuestionOption)
                                            'Else
                                            optionText = intValue
                                            'End If

                                        End If

                                        newRow.Items.Add(optionText)



                                        'If intValue > 
                                        'QUI! Verificare!!!

                                        'If intValue > qMultiAns.OptionNumber Then
                                        '    value = "--"
                                        'Else
                                        '    value = intValue
                                        'End If
                                        'newRow.Items.Add(value)




                                        newRow.Items.Add(qMultiAns.OptionText)

                                        'Valutazione
                                        newRow.Items.Add("")
                                        index += 1
                                        dto.Rows.Add(newRow)
                                    Next


                            End Select
                        End If

                    Next
                Else
                    Dim row As dtoExportRow = GenerateAnswerRow(index, qAnswerItem, displayNames, anonymousUser)
                    For i As Integer = 1 To 13
                        row.Items.Add("")
                    Next
                    index += 1
                    dto.Rows.Add(row)
                End If
            Next
        End Sub
        'Public Sub QuestionsRender_OLD(ByVal currentuser As Person, ByVal displayTaxCode As Boolean, ByVal allowFullExport As Boolean, ByVal dto As dtoExportQuestionnaire, ByVal displayNames As Dictionary(Of String, dtoDisplayName), ByVal anonymousUser As String, qAnswers As List(Of dtoFullUserAnswerItem), answers As Dictionary(Of Long, QuestionnaireAnswer))
        '    dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleIdAnswer))

        '    'Dim addTaxCode As Boolean = False
        '    If allowFullExport Then
        '        dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleSurname))
        '        dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleName))
        '        If displayTaxCode Then
        '            Select Case currentuser.TypeID
        '                Case CInt(UserTypeStandard.SysAdmin), CInt(UserTypeStandard.Administrator), CInt(UserTypeStandard.Administrative)
        '                    dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleTaxCode))
        '                Case Else
        '                    displayTaxCode = False
        '            End Select
        '        End If
        '        dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleOtherInfos))
        '    Else
        '        dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleUserName))
        '    End If


        '    dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleIdUser))
        '    dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleIdInvitedUser))
        '    dto.Cells.Add(CellTitleStartedOn)
        '    dto.Cells.Add(CellTitleModifiedOn)
        '    dto.Cells.Add(CellTitleEndedOn)

        '    dto.Cells.Add(CellTitleIdQuestion)
        '    '  dto.Cells.Add(CellTitleOptionCorrect)
        '    dto.Cells.Add(CellTitleIdOption)
        '    dto.Cells.Add(CellTitleOptionNumber)
        '    dto.Cells.Add(CellTitleOptionValue)
        '    dto.Cells.Add(CellTitleOptionText)

        '    dto.Cells.Add(CellTitleOptionFreeText)
        '    dto.Cells.Add(CellTitleEvaluation)

        '    '  dto.Cells.Add(CellTitleOptionPoints)

        '    Dim index As Long = 1
        '    For Each qAnswerItem As dtoFullUserAnswerItem In qAnswers
        '        Dim answer As QuestionnaireAnswer = answers(qAnswerItem.Id)

        '        If Not IsNothing(answer) Then
        '            Dim info As New List(Of String)
        '            info.Add(answer.IdUser)
        '            info.Add(answer.IdInvitedUser)
        '            If qAnswerItem.Answer.StartedOn.HasValue Then
        '                info.Add(qAnswerItem.Answer.StartedOn.Value)
        '            Else
        '                info.Add("")
        '            End If
        '            If qAnswerItem.Answer.ModifiedOn.HasValue Then
        '                info.Add(qAnswerItem.Answer.ModifiedOn.Value)
        '            Else
        '                info.Add("")
        '            End If
        '            If qAnswerItem.Answer.CompletedOn.HasValue Then
        '                info.Add(qAnswerItem.Answer.CompletedOn.Value)
        '            Else
        '                info.Add("")
        '            End If

        '            For Each item As LazyAssociatedQuestion In answer.Questions.OrderBy(Function(q) q.Number).ThenBy(Function(q) q.IdQuestion)
        '                Dim row As dtoExportRow = GenerateAnswerRow(index, qAnswerItem, displayNames, anonymousUser)
        '                row.Items.AddRange(info)
        '                row.Items.Add(item.IdQuestion)
        '                Dim idQuestion As Integer = item.IdQuestion
        '                Dim questionAnswer As QuestionAnswer = answer.Answers.Where(Function(a) a.IdQuestion.Equals(idQuestion)).FirstOrDefault()

        '                If IsNothing(questionAnswer) Then
        '                    For i As Integer = 1 To 6
        '                        row.Items.Add("")
        '                    Next
        '                Else
        '                    row.Items.Add(questionAnswer.IdQuestionOption)

        '                    Select Case questionAnswer.QuestionType
        '                        Case Domanda.TipoDomanda.DropDown
        '                            '  row.Items.Add("")
        '                            row.Items.Add(questionAnswer.OptionNumber)
        '                            row.Items.Add(questionAnswer.OptionText)
        '                            row.Items.Add("")
        '                            row.Items.Add("")

        '                            row.Items.Add("")

        '                            row.Items.Add("")
        '                            'dto.Cells.Add(CellTitleOptionCorrect)
        '                            'dto.Cells.Add(CellTitleOptionPoints)

        '                        Case Domanda.TipoDomanda.Multipla
        '                            '  row.Items.Add("")
        '                            row.Items.Add(questionAnswer.OptionNumber)
        '                            row.Items.Add("")
        '                            row.Items.Add("")

        '                            row.Items.Add(questionAnswer.OptionText)
        '                            row.Items.Add("")
        '                            row.Items.Add("")
        '                            'dto.Cells.Add(CellTitleOptionCorrect)
        '                            'dto.Cells.Add(CellTitleOptionPoints)
        '                        Case Domanda.TipoDomanda.Numerica
        '                            '  row.Items.Add("")
        '                            row.Items.Add(questionAnswer.OptionNumber)
        '                            row.Items.Add(questionAnswer.Value)
        '                            row.Items.Add(questionAnswer.OptionNumber)

        '                            row.Items.Add("")
        '                            row.Items.Add("")
        '                            row.Items.Add("")
        '                            'dto.Cells.Add(CellTitleOptionCorrect)
        '                            'dto.Cells.Add(CellTitleOptionPoints)

        '                        Case Domanda.TipoDomanda.Rating
        '                            '  row.Items.Add("")
        '                            row.Items.Add(questionAnswer.OptionNumber)
        '                            row.Items.Add(questionAnswer.Value)
        '                            row.Items.Add("")

        '                            row.Items.Add(questionAnswer.OptionText)
        '                            row.Items.Add(questionAnswer.Evaluation)
        '                            row.Items.Add("")

        '                            'dto.Cells.Add(CellTitleOptionCorrect)
        '                            'dto.Cells.Add(CellTitleOptionPoints)

        '                        Case Domanda.TipoDomanda.TestoLibero
        '                            '  row.Items.Add("")
        '                            row.Items.Add(questionAnswer.OptionNumber)
        '                            row.Items.Add(questionAnswer.Value)
        '                            row.Items.Add("")

        '                            row.Items.Add(questionAnswer.OptionText)
        '                            row.Items.Add(questionAnswer.Evaluation)

        '                            row.Items.Add("")
        '                            'dto.Cells.Add(CellTitleOptionCorrect)
        '                            'dto.Cells.Add(CellTitleOptionPoints)
        '                    End Select
        '                End If
        '                index += 1
        '                dto.Rows.Add(row)
        '            Next
        '        Else
        '            Dim row As dtoExportRow = GenerateAnswerRow(index, qAnswerItem, displayNames, anonymousUser)
        '            For i As Integer = 1 To 13
        '                row.Items.Add("")
        '            Next
        '            index += 1
        '            dto.Rows.Add(row)
        '        End If
        '    Next
        'End Sub
        Private Function GenerateAnswerRow(index As Integer, qAnswerItem As dtoFullUserAnswerItem, ByVal displayNames As Dictionary(Of String, dtoDisplayName), ByVal anonymousUser As String) As dtoExportRow
            Dim row As dtoExportRow
            If Not displayNames.Values.Any() Then
                row = New dtoExportRow() With {
                    .Index = index,
                    .IdAnswer = qAnswerItem.Id,
                    .DisplayInfo = New dtoDisplayName(anonymousUser)
                }
            Else
                If qAnswerItem.Answer.IdInvitedUser.HasValue _
                    AndAlso displayNames.ContainsKey("i_" & qAnswerItem.Answer.IdInvitedUser) Then
                    row = New dtoExportRow() With
                        {
                        .Index = index,
                        .IdAnswer = qAnswerItem.Id,
                        .DisplayInfo = displayNames("i_" & qAnswerItem.Answer.IdInvitedUser)
                    }

                ElseIf qAnswerItem.Answer.IdPerson > 0 _
                    AndAlso displayNames.ContainsKey("p_" & qAnswerItem.Answer.IdPerson) Then
                    row = New dtoExportRow() With
                    {
                        .Index = index,
                        .IdAnswer = qAnswerItem.Id,
                        .DisplayInfo = displayNames("p_" & qAnswerItem.Answer.IdPerson)
                    }

                ElseIf displayNames.ContainsKey("p_0") Then
                    row = New dtoExportRow() With
                    {
                        .Index = index,
                        .IdAnswer = qAnswerItem.Id,
                        .DisplayInfo = displayNames("p_0")
                    }
                Else
                    row = New dtoExportRow() With
                    {
                        .Index = index,
                        .IdAnswer = qAnswerItem.Id,
                        .DisplayInfo = New dtoDisplayName(anonymousUser)
                    }
                End If
            End If
            Return row
        End Function

        Public Sub QuestionsRender(ByVal dto As dtoExportQuestionnaire, ByVal questions As List(Of Domanda))
            For Each oDom As Domanda In questions

                Try
                    Select Case oDom.tipo
                        Case Domanda.TipoDomanda.Rating             'ok - 2018-04-30
                            ConvertRatingToString_V2(oDom, dto)
                        Case Domanda.TipoDomanda.RatingStars        'ok - 2018-04-30
                            ConvertRatingToString_V2(oDom, dto)
                        Case Domanda.TipoDomanda.DropDown           'ok - 2018-04-30
                            ConvertDropDownToString_V2(oDom, dto)
                        Case Domanda.TipoDomanda.TestoLibero        'ok - 2018-04-30
                            ConvertTestoLiberoToString_V2(oDom, dto)
                        Case Domanda.TipoDomanda.Numerica           'ok - 2018-04-30
                            ConvertNumericaToString_V2(oDom, dto)
                        Case Domanda.TipoDomanda.Multipla           '    W.I.P.
                            ConvertMultiplaToString_V2(oDom, dto)
                    End Select
                Catch ex As Exception

                    'dto.Cells.Add(String.Format(HeaderOptionFormat, oDom.id, -1))
                    'For Each row As dtoExportRow In dto.Rows
                    '    row.Items.Add(ex.ToString())
                    'Next
                End Try

            Next
        End Sub

        Private Sub ConvertDropDownToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
            ' dto.Cells.Add(PageIdentifier & oDom.numeroPagina & QuestionIdentifier & oDom.numero)
            dto.Cells.Add(CellTitleIdQuestion & "_" & oDom.numero)
            dto.Cells.Add(GetCellIdentifier(oDom))
            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim numeroOpzList As List(Of Integer) = (From oRisp In oDom.risposteDomanda Where oRisp.idRispostaQuestionario = idAnswer Select oRisp.numeroOpzione).ToList
                row.Items.Add(oDom.id)
                If numeroOpzList.Count = 0 Then
                    row.Items.Add(NullItem)
                Else
                    row.Items.Add(numeroOpzList(0).ToString)
                End If
            Next
        End Sub



        Private Sub ConvertDropDownToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, answers As Dictionary(Of Long, QuestionnaireAnswer))
            dto.Cells.Add(CellTitleIdQuestion)
            dto.Cells.Add(GetCellIdentifier(oDom, libraryNumber))

            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim answer As QuestionnaireAnswer = answers(row.IdAnswer)

                row.Items.Add(oDom.id)
                If Not IsNothing(answer) Then
                    Dim idQuestion As Integer = oDom.id
                    'Dim question As QuestionAnswer = (From q In answer.Answers Where q.IdQuestion = idQuestion Select q).FirstOrDefault()
                    If (From q In answer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
                        Dim numeroOpzList As List(Of Integer) = (From a In answer.Answers Where a.IdQuestion = idQuestion Select a.OptionNumber).ToList
                        If numeroOpzList.Count = 0 Then
                            row.Items.Add(NullItem)
                        Else
                            row.Items.Add(numeroOpzList(0).ToString)
                        End If
                    Else
                        row.Items.Add(QuestionNotUsedIdentifier)
                    End If
                Else
                    row.Items.Add(QuestionNotUsedIdentifier)
                End If
            Next
        End Sub
        Private Sub ConvertMultiplaToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
            Dim firstRun As Boolean = True
            Dim nOpzioni As Integer = oDom.domandaMultiplaOpzioni.Count - 1

            dto.Cells.Add(CellTitleIdQuestion & "_" & oDom.numero)

            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Order By oRisp.numeroOpzione Where oRisp.idRispostaQuestionario = idAnswer Select oRisp.numeroOpzione, oRisp.testoOpzione).ToList
                row.Items.Add(oDom.id)
                Dim listIndex As Integer = 0
                For i As Integer = 0 To nOpzioni
                    If firstRun Then
                        dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1))
                    End If

                    If listIndex < numeroOpzList.count AndAlso numeroOpzList(listIndex).numeroOpzione = i + 1 Then
                        row.Items.Add("1")

                        If oDom.domandaMultiplaOpzioni(i).isAltro Then
                            row.Items.Add(numeroOpzList(listIndex).testoOpzione)
                            If firstRun Then
                                dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1) & FreeTextIdentifier)
                            End If
                        End If
                        listIndex += 1
                    Else
                        row.Items.Add("0")
                        If oDom.domandaMultiplaOpzioni(i).isAltro Then
                            row.Items.Add(NullItem)
                            If firstRun Then
                                dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1) & FreeTextIdentifier)
                            End If
                        End If
                    End If
                Next
                firstRun = False
            Next
        End Sub
        Private Sub ConvertMultiplaToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, answers As Dictionary(Of Long, QuestionnaireAnswer))
            Dim firstRun As Boolean = True
            Dim nOpzioni As Integer = oDom.domandaMultiplaOpzioni.Count - 1

            dto.Cells.Add(CellTitleIdQuestion)
            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim qAnswer As QuestionnaireAnswer = answers(row.IdAnswer)
                Dim idQuestion As Integer = oDom.id

                row.Items.Add(idQuestion.ToString)
                If Not (From q In qAnswer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
                    Dim listIndex As Integer = 0
                    For i As Integer = 0 To nOpzioni
                        If firstRun Then
                            dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber))
                        End If
                        row.Items.Add(QuestionNotUsedIdentifier)
                    Next
                Else
                    Dim numeroOpzList = (From a In qAnswer.Answers Where a.IdQuestion = idQuestion Order By a.OptionNumber Select OptionNumber = a.OptionNumber, OptionText = a.OptionText).ToList

                    Dim listIndex As Integer = 0
                    For i As Integer = 0 To nOpzioni
                        If firstRun Then
                            dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber))
                        End If

                        If listIndex < numeroOpzList.count AndAlso numeroOpzList(listIndex).OptionNumber = i + 1 Then
                            row.Items.Add("1")

                            If oDom.domandaMultiplaOpzioni(i).isAltro Then
                                row.Items.Add(numeroOpzList(listIndex).OptionText)
                                If firstRun Then
                                    dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber).ToString & FreeTextIdentifier)
                                End If
                            End If
                            listIndex += 1
                        Else
                            row.Items.Add("0")
                            If oDom.domandaMultiplaOpzioni(i).isAltro Then
                                row.Items.Add(NullItem)
                                If firstRun Then
                                    dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber).ToString & FreeTextIdentifier)
                                End If
                            End If
                        End If
                    Next
                End If


                firstRun = False
            Next
        End Sub
        Private Sub ConvertNumericaToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
            Dim nOpzioni As Integer = oDom.opzioniNumerica.Count - 1

            dto.Cells.Add(CellTitleIdQuestion & "_" & oDom.numero)
            For i As Integer = 0 To nOpzioni
                dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1))
            Next

            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Where oRisp.idRispostaQuestionario = idAnswer Order By oRisp.numeroOpzione Select oRisp.valore, oRisp.numeroOpzione).ToList
                row.Items.Add(oDom.id)
                For i As Integer = 0 To nOpzioni
                    If IsNothing(numeroOpzList) OrElse numeroOpzList.Count = 0 Then
                        '"##############################"
                        row.Items.Add(NullItem)
                    Else
                        If Not numeroOpzList(i).valore = Integer.MinValue Then 'valore di default quando la risposta non e' stata inserita
                            row.Items.Add(numeroOpzList(i).valore.ToString)
                        Else
                            row.Items.Add(NullItem)
                        End If
                    End If
                Next
            Next
        End Sub
        Private Sub ConvertNumericaToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, answers As Dictionary(Of Long, QuestionnaireAnswer))
            Dim nOpzioni As Integer = oDom.opzioniNumerica.Count - 1
            dto.Cells.Add(CellTitleIdQuestion)
            For i As Integer = 0 To nOpzioni
                dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber))
            Next

            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim qAnswer As QuestionnaireAnswer = answers(row.IdAnswer)
                row.Items.Add(oDom.id)
                If Not IsNothing(qAnswer) Then
                    Dim idQuestion As Integer = oDom.id

                    Dim numeroOpzList = Nothing
                    If (From q In qAnswer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
                        numeroOpzList = (From a In qAnswer.Answers Where a.IdQuestion = idQuestion Order By a.OptionNumber Select Value = a.Value, OptionNumber = a.OptionNumber).ToList
                    End If

                    For i As Integer = 0 To nOpzioni
                        If Not (From q In qAnswer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
                            row.Items.Add(QuestionNotUsedIdentifier)
                        ElseIf IsNothing(numeroOpzList) OrElse numeroOpzList.Count = 0 Then
                            row.Items.Add(NullItem)
                        Else
                            If Not numeroOpzList(i).Value = Integer.MinValue.ToString Then 'valore di default quando la risposta non e' stata inserita
                                row.Items.Add(numeroOpzList(i).Value.ToString)
                            Else
                                row.Items.Add(NullItem)
                            End If
                        End If
                    Next
                Else
                    For i As Integer = 0 To nOpzioni
                        row.Items.Add(QuestionNotUsedIdentifier)
                    Next
                End If
            Next
        End Sub
        Private Sub ConvertTestoLiberoToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
            Dim nOpzioni As Integer = oDom.opzioniTestoLibero.Count - 1
            dto.Cells.Add(CellTitleIdQuestion & "_" & oDom.numero)
            For i As Integer = 0 To nOpzioni
                dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1) & FreeTextIdentifier)
            Next
            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Where oRisp.idRispostaQuestionario = idAnswer Order By oRisp.numeroOpzione Select oRisp.testoOpzione, oRisp.numeroOpzione).ToList
                row.Items.Add(oDom.id)
                For i As Integer = 0 To nOpzioni
                    If i < numeroOpzList.count AndAlso (numeroOpzList(i).numeroOpzione = i + 1 Or (oDom.opzioniTestoLibero.Count = 1 AndAlso numeroOpzList(i).numeroOpzione = 0)) Then
                        'quando ho una TL con una sola opzione, l'indice dell'opzione parte da 0. verificare.
                        row.Items.Add(numeroOpzList(i).testoOpzione)
                    Else
                        row.Items.Add(NullItem)
                    End If
                Next
            Next
        End Sub
        Private Sub ConvertTestoLiberoToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, answers As Dictionary(Of Long, QuestionnaireAnswer))
            Dim nOpzioni As Integer = oDom.opzioniTestoLibero.Count - 1

            dto.Cells.Add(CellTitleIdQuestion)
            For i As Integer = 0 To nOpzioni
                dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber) & FreeTextIdentifier)
            Next
            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim qAnswer As QuestionnaireAnswer = answers(row.IdAnswer)
                row.Items.Add(oDom.id)
                If Not IsNothing(qAnswer) Then
                    Dim idQuestion As Integer = CLng(oDom.id)
                    If Not (From q In qAnswer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
                        For i As Integer = 0 To nOpzioni
                            row.Items.Add(QuestionNotUsedIdentifier)
                        Next
                    Else
                        Dim numeroOpzList = (From a In qAnswer.Answers Where a.IdQuestion = idQuestion Order By a.OptionNumber Select OptionText = a.OptionText, OptionNumber = a.OptionNumber).ToList
                        For i As Integer = 0 To nOpzioni
                            If i < numeroOpzList.count AndAlso (numeroOpzList(i).OptionNumber = i + 1 Or (oDom.opzioniTestoLibero.Count = 1 AndAlso numeroOpzList(i).OptionNumber = 0)) Then
                                'quando ho una TL con una sola opzione, l'indice dell'opzione parte da 0. verificare.
                                row.Items.Add(numeroOpzList(i).OptionText)
                            Else
                                row.Items.Add(NullItem)
                            End If
                        Next
                    End If
                Else
                    For i As Integer = 0 To nOpzioni
                        row.Items.Add(QuestionNotUsedIdentifier)
                    Next
                End If
            Next
        End Sub

        Private Sub ConvertRatingToString(ByVal oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
            Dim firstRun As Boolean = True
            Dim nOpzioni As Integer = oDom.domandaRating.opzioniRating.Count - 1

            dto.Cells.Add(CellTitleIdQuestion & "_" & oDom.numero)
            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer

                row.Items.Add(oDom.id)
                For j As Integer = 0 To nOpzioni
                    Dim isSelectedRow As Boolean = False
                    Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Order By oRisp.idDomandaOpzione Where oRisp.idRispostaQuestionario = idAnswer And oRisp.idDomandaOpzione = oDom.domandaRating.opzioniRating(j).id Select oRisp.valore, oRisp.testoOpzione).ToList
                    For i As Integer = 0 To oDom.domandaRating.numeroRating
                        If numeroOpzList.count > 0 AndAlso numeroOpzList(0).valore = i + 1 Then
                            row.Items.Add("1")
                            isSelectedRow = True
                        Else
                            row.Items.Add("0")
                        End If
                        If firstRun Then
                            dto.Cells.Add(GetCellOptionIdentifier(oDom, j + 1) & "," & (i + 1).ToString)
                        End If
                    Next
                    If oDom.domandaRating.opzioniRating(j).isAltro Then
                        If isSelectedRow Then
                            row.Items.Add(numeroOpzList(0).testoOpzione)
                        Else
                            row.Items.Add(NullItem)
                        End If
                        If firstRun Then
                            dto.Cells.Add(GetCellOptionIdentifier(oDom, j + 1) & FreeTextIdentifier)
                        End If
                    End If
                Next
                firstRun = False
            Next
        End Sub

        'Private Const HeaderQuestionFormat = "{0}_{1}"
        Private Const HeaderOptionFormat = "D{0}_O{1}"
        Private Const HeaderOptionFormatTextValue = "D{0}_O{1}_TL"

        Private Sub ConvertRatingToString_V2(ByVal oDom As Domanda, ByRef dto As dtoExportQuestionnaire)

            If IsNothing(oDom.domandaRating) Then
                Return
            End If


            Dim firstRun As Boolean = True
            Dim nOpzioni As Integer = oDom.domandaRating.opzioniRating.Count - 1

            'Dim title As String = String.Format(HeaderQuestionFormat, CellTitleIdQuestion, oDom.numero)
            'dto.Cells.Add(title)
            'dto.Cells.Add(CellTitleIdQuestion & "_" & oDom.numero)


            Dim dicValues As New Dictionary(Of Integer, String)()
            Dim FreeTextOpziontsIds As New List(Of Integer)()

            'Intestazioni colonne opzioni
            For Each opzione As DomandaOpzione In oDom.domandaRating.opzioniRating
                dto.Cells.Add(String.Format(HeaderOptionFormat, oDom.id, (opzione.numero + 1)))

                If opzione.isAltro Then 'Cioè: se c'è il testo libero! -SIGH!?- (però forse, ora vediamo...)
                    dto.Cells.Add(String.Format(HeaderOptionFormatTextValue, oDom.id, (opzione.numero + 1)))

                    FreeTextOpziontsIds.Add(opzione.id)
                End If
            Next

            If Not IsNothing(oDom.domandaRating) _
                        AndAlso oDom.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Testi _
                        AndAlso Not IsNothing(oDom.domandaRating.intestazioniRating) _
                        AndAlso oDom.domandaRating.intestazioniRating.Any() Then

                For Each opz As DomandaOpzione In oDom.domandaRating.intestazioniRating
                    dicValues.Add(CInt(opz.numero), opz.testo)   'CHECK HTML!!! Ok, lascio numero, anche se potrei mettere id...
                Next

                'If oDom.domandaRating.mostraND Then

                'End If

            End If

            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer

                For j As Integer = 0 To nOpzioni
                    Dim isSelectedRow As Boolean = False
                    Dim Risposte As List(Of RispostaDomanda) = (From oRisp In oDom.risposteDomanda Order By oRisp.idDomandaOpzione Where oRisp.idRispostaQuestionario = idAnswer And oRisp.idDomandaOpzione = oDom.domandaRating.opzioniRating(j).id Select oRisp).ToList()


                    For Each risp As RispostaDomanda In Risposte
                        'New KeyValuePair(Of String, String)(oRisp.valore, oRisp.testoOpzione)

                        If IsNumeric(risp.valore) Then
                            Dim key As Integer = CInt(risp.valore)


                            If dicValues.ContainsKey(key) Then
                                row.Items.Add(dicValues(key))
                            Else
                                If key > oDom.domandaRating.numeroRating Then
                                    row.Items.Add("")
                                Else
                                    row.Items.Add(key)
                                End If
                            End If

                        End If

                        If FreeTextOpziontsIds.Contains(risp.idDomandaOpzione) Then
                            row.Items.Add(risp.testoOpzione) '?? Vediamo, che tanto qui mettono valori dove capita in proprietà a caso a seconda di chissàché...
                        End If

                    Next



                    'Dim Value As Integer = 0

                    'Dim MaxValue As Integer = oDom.domandaRating.numeroRating
                    'If (oDom.domandaRating.mostraND) Then
                    '    MaxValue += 1
                    'End If

                    'For i As Integer = 0 To MaxValue
                    '    If numeroOpzList.count > 0 AndAlso numeroOpzList(0).valore = i + 1 Then
                    '        'row.Items.Add("1")
                    '        Value += 1
                    '        isSelectedRow = True
                    '        'Else
                    '        'row.Items.Add("0")
                    '    End If
                    '    'If firstRun Then
                    '    '    'Intestazione ?!
                    '    '    'dto.Cells.Add(GetCellOptionIdentifier(oDom, j + 1) & "," & (i + 1).ToString)
                    '    'End If
                    'Next

                    'If dicValues.ContainsKey(Value) Then
                    '    row.Items.Add(dicValues(Value))
                    'Else
                    '    row.Items.Add(Value)
                    'End If

                    ''If oDom.domandaRating.opzioniRating(j).isAltro Then
                    ''    If isSelectedRow Then
                    ''        row.Items.Add(numeroOpzList(0).testoOpzione)
                    ''    Else
                    ''        row.Items.Add(NullItem)
                    ''    End If
                    ''    If firstRun Then
                    ''        dto.Cells.Add(GetCellOptionIdentifier(oDom, j + 1) & FreeTextIdentifier)
                    ''    End If
                    ''End If
                Next

                'firstRun = False
            Next
        End Sub
        Private Sub ConvertRatingToString(ByVal oDom As Domanda, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, answers As Dictionary(Of Long, QuestionnaireAnswer))
            Dim firstRun As Boolean = True
            Dim nOpzioni As Integer = oDom.domandaRating.opzioniRating.Count - 1

            dto.Cells.Add(CellTitleIdQuestion)
            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim answer As QuestionnaireAnswer = answers(row.IdAnswer)
                row.Items.Add(oDom.id)
                If Not IsNothing(answer) Then
                    Dim idQuestion As Integer = oDom.id
                    'Dim question As Domanda = (From oRisp In quest.domande Where oRisp.id = idQuestion Select oRisp).FirstOrDefault()


                    If Not (From q In answer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
                        SetNotUsedRating(row, dto, libraryNumber, oDom, nOpzioni, firstRun)
                    Else
                        For j As Integer = 0 To nOpzioni
                            Dim isSelectedRow As Boolean = False
                            Dim index As Integer = j
                            Dim numeroOpzList = (From a In answer.Answers Where a.IdQuestion = idQuestion Order By a.IdQuestionOption Where a.IdQuestionOption = oDom.domandaRating.opzioniRating(index).id Select Value = a.Value, OptionText = a.OptionText).ToList
                            For i As Integer = 0 To oDom.domandaRating.numeroRating
                                If numeroOpzList.count > 0 AndAlso numeroOpzList(0).Value = i + 1 Then
                                    row.Items.Add("1")
                                    isSelectedRow = True
                                Else
                                    row.Items.Add("0")
                                End If
                                If firstRun Then
                                    dto.Cells.Add(GetCellOptionIdentifier(oDom, j + 1, libraryNumber) & "-" & (i + 1).ToString)
                                End If
                            Next
                            If oDom.domandaRating.opzioniRating(j).isAltro Then
                                If isSelectedRow Then
                                    row.Items.Add(numeroOpzList(0).OptionText)
                                Else
                                    row.Items.Add(NullItem)
                                End If
                                If firstRun Then
                                    dto.Cells.Add(GetCellOptionIdentifier(oDom, j + 1, libraryNumber) & FreeTextIdentifier)
                                End If
                            End If
                        Next
                    End If
                Else
                    SetNotUsedRating(row, dto, libraryNumber, oDom, nOpzioni, firstRun)
                End If

                firstRun = False
            Next
        End Sub


        Private Sub ConvertDropDownToString_V2(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire)


            Dim dicValues As New Dictionary(Of Integer, String)()
            'Dim FreeTextOpziontsIds As New List(Of Integer)()

            ''Intestazioni colonne opzioni
            'For Each opzione As DomandaOpzione In oDom.domandaRating.opzioniRating
            '    dto.Cells.Add(String.Format(HeaderOptionFormat, oDom.id, (opzione.numero + 1)))

            '    If opzione.isAltro Then 'Cioè: se c'è il testo libero! -SIGH!?- (però forse, ora vediamo...)
            '        dto.Cells.Add(String.Format(HeaderOptionFormatTextValue, oDom.id, (opzione.numero + 1)))

            '        FreeTextOpziontsIds.Add(opzione.id)
            '    End If
            'Next

            'Testo valori selezinati

            If Not IsNothing(oDom.domandaDropDown) _
                        AndAlso Not IsNothing(oDom.domandaDropDown.dropDownItems) _
                        AndAlso oDom.domandaDropDown.dropDownItems.Any() Then

                For Each ddi As DropDownItem In oDom.domandaDropDown.dropDownItems
                    If Not dicValues.ContainsKey(ddi.idDropDown) Then
                        dicValues.Add(ddi.numero, ddi.testo)
                    End If
                Next

            End If


            '----------------------------------------------

            'Intestazione colonna
            dto.Cells.Add(String.Format(HeaderOptionFormat, oDom.id, "1"))

            'Valori selezionati
            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim numeroOpzList As List(Of Integer) = (From oRisp In oDom.risposteDomanda Where oRisp.idRispostaQuestionario = idAnswer Select oRisp.numeroOpzione).ToList
                'row.Items.Add(oDom.id)
                If numeroOpzList.Count = 0 Then
                    row.Items.Add(NullItem)
                Else
                    If dicValues.ContainsKey(numeroOpzList(0)) Then
                        row.Items.Add(dicValues(numeroOpzList(0)))
                    Else
                        row.Items.Add(numeroOpzList(0).ToString())
                    End If
                End If
            Next

            '----------------------------------------


            '' dto.Cells.Add(PageIdentifier & oDom.numeroPagina & QuestionIdentifier & oDom.numero)
            'dto.Cells.Add(CellTitleIdQuestion & "_" & oDom.numero)
            'dto.Cells.Add(GetCellIdentifier(oDom))
            'For Each row As dtoExportRow In dto.Rows
            '    Dim idAnswer As Integer = row.IdAnswer
            '    Dim numeroOpzList As List(Of Integer) = (From oRisp In oDom.risposteDomanda Where oRisp.idRispostaQuestionario = idAnswer Select oRisp.numeroOpzione).ToList
            '    row.Items.Add(oDom.id)
            '    If numeroOpzList.Count = 0 Then
            '        row.Items.Add(NullItem)
            '    Else
            '        row.Items.Add(numeroOpzList(0).ToString)
            '    End If
            'Next
        End Sub


        Private Sub ConvertTestoLiberoToString_V2(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
            Dim nOpzioni As Integer = oDom.opzioniTestoLibero.Count - 1
            'dto.Cells.Add(CellTitleIdQuestion & "_" & oDom.numero)
            For i As Integer = 0 To nOpzioni
                'dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1) & FreeTextIdentifier)
                dto.Cells.Add(String.Format(HeaderOptionFormat, oDom.id, i + 1))
            Next
            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Where oRisp.idRispostaQuestionario = idAnswer Order By oRisp.numeroOpzione Select oRisp.testoOpzione, oRisp.numeroOpzione).ToList
                'row.Items.Add(oDom.id)
                For i As Integer = 0 To nOpzioni
                    If i < numeroOpzList.count AndAlso (numeroOpzList(i).numeroOpzione = i + 1 Or (oDom.opzioniTestoLibero.Count = 1 AndAlso numeroOpzList(i).numeroOpzione = 0)) Then
                        'quando ho una TL con una sola opzione, l'indice dell'opzione parte da 0. verificare.
                        row.Items.Add(numeroOpzList(i).testoOpzione)
                    Else
                        row.Items.Add(NullItem)
                    End If
                Next
            Next
        End Sub

        Private Sub ConvertNumericaToString_V2(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
            Dim nOpzioni As Integer = oDom.opzioniNumerica.Count - 1

            'dto.Cells.Add(CellTitleIdQuestion & "_" & oDom.numero)
            For i As Integer = 0 To nOpzioni
                'dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1))
                dto.Cells.Add(String.Format(HeaderOptionFormat, oDom.id, i + 1))
            Next

            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Where oRisp.idRispostaQuestionario = idAnswer Order By oRisp.numeroOpzione Select oRisp.valore, oRisp.numeroOpzione).ToList
                'row.Items.Add(oDom.id)
                For i As Integer = 0 To nOpzioni
                    If IsNothing(numeroOpzList) OrElse numeroOpzList.Count = 0 Then
                        '"##############################"
                        row.Items.Add(NullItem)
                    Else
                        If Not numeroOpzList(i).valore = Integer.MinValue Then 'valore di default quando la risposta non e' stata inserita
                            row.Items.Add(numeroOpzList(i).valore.ToString)
                        Else
                            row.Items.Add(NullItem)
                        End If
                    End If
                Next
            Next
        End Sub






        Private Sub ConvertMultiplaToString_V2(ByVal oDom As Domanda, ByRef dto As dtoExportQuestionnaire)

            If IsNothing(oDom.domandaMultiplaOpzioni) _
                OrElse Not oDom.domandaMultiplaOpzioni.Any() Then
                Exit Sub
            End If

            Dim dicValues As New Dictionary(Of Integer, String)()
            'Dim FreeTextOpziontsIds As New List(Of Integer)()

            For Each opz As DomandaOpzione In oDom.domandaMultiplaOpzioni
                If Not dicValues.ContainsKey(opz.numero) Then
                    dicValues.Add(opz.numero, opz.testo)   'CHECK HTML!!! Ok, lascio numero, anche se potrei mettere id...
                End If
            Next

            Dim nOpzioni As Integer = oDom.domandaMultiplaOpzioni.Count - 1
            Dim firstRun As Boolean = True

            If oDom.isMultipla Then


                ''Intestazioni colonne opzioni
                'For Each opzione As DomandaOpzione In oDom.domandaRating.opzioniRating
                '    dto.Cells.Add(String.Format(HeaderOptionFormat, oDom.id, (opzione.numero + 1)))

                '    If opzione.isAltro Then 'Cioè: se c'è il testo libero! -SIGH!?- (però forse, ora vediamo...)
                '        dto.Cells.Add(String.Format(HeaderOptionFormatTextValue, oDom.id, (opzione.numero + 1)))

                '        'FreeTextOpziontsIds.Add(opzione.id)
                '    End If
                'Next


                'For Each row As dtoExportRow In dto.Rows
                '    Dim idAnswer As Integer = row.IdAnswer

                '    For j As Integer = 0 To nOpzioni
                '        Dim isSelectedRow As Boolean = False
                '        Dim Risposte As List(Of RispostaDomanda) = (From oRisp In oDom.risposteDomanda Order By oRisp.idDomandaOpzione Where oRisp.idRispostaQuestionario = idAnswer And oRisp.idDomandaOpzione = oDom.domandaRating.opzioniRating(j).id Select oRisp).ToList()


                '        For Each risp As RispostaDomanda In Risposte


                '            If IsNumeric(risp.valore) Then
                '                Dim key As Integer = CInt(risp.valore)


                '                If dicValues.ContainsKey(key) Then
                '                    row.Items.Add(dicValues(key))
                '                Else
                '                    row.Items.Add("")
                '                End If

                '            End If

                '            'If FreeTextOpziontsIds.Contains(risp.idDomandaOpzione) Then
                '            '    row.Items.Add(risp.testoOpzione) '?? Vediamo, che tanto qui mettono valori dove capita in proprietà a caso a seconda di chissàché...
                '            'End If

                '        Next
                '    Next
                'Next


                For Each row As dtoExportRow In dto.Rows
                    Dim idAnswer As Integer = row.IdAnswer
                    Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Order By oRisp.numeroOpzione Where oRisp.idRispostaQuestionario = idAnswer Select oRisp.numeroOpzione, oRisp.testoOpzione).ToList
                    'row.Items.Add(oDom.id)
                    Dim listIndex As Integer = 0
                    For i As Integer = 0 To nOpzioni
                        If firstRun Then
                            'dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1))
                            dto.Cells.Add(String.Format(HeaderOptionFormat, oDom.id, i + 1))
                        End If

                        If listIndex < numeroOpzList.count AndAlso numeroOpzList(listIndex).numeroOpzione = i + 1 Then
                            'row.Items.Add("1")
                            row.Items.Add(dicValues(i + 1))

                            'VERIFICA "IsALTRO" => TESTO LIBERO!!!
                            If oDom.domandaMultiplaOpzioni(i).isAltro Then
                                row.Items.Add(numeroOpzList(listIndex).testoOpzione)
                                If firstRun Then
                                    'dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1) & FreeTextIdentifier)
                                    dto.Cells.Add(String.Format(HeaderOptionFormatTextValue, oDom.id, i + 1))
                                End If
                            End If
                            listIndex += 1
                        Else
                            row.Items.Add("")
                            If oDom.domandaMultiplaOpzioni(i).isAltro Then
                                row.Items.Add("")
                                If firstRun Then
                                    'dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1) & FreeTextIdentifier)
                                    dto.Cells.Add(String.Format(HeaderOptionFormatTextValue, oDom.id, i + 1))
                                End If
                            End If
                        End If
                    Next
                    firstRun = False
                Next

                '-----------------------

            Else


                'Intestazioni colonne opzioni
                dto.Cells.Add(String.Format(HeaderOptionFormat, oDom.id, 1))
                'For Each row As dtoExportRow In dto.Rows
                '    row.Items.Add("ToDo")
                'Next

                'Exit Sub

                Dim value As String = ""
                Dim ValueAltro As String = ""
                Dim isaltro As Boolean = False



                'Valore selezionato
                For Each row As dtoExportRow In dto.Rows
                    Dim idAnswer As Integer = row.IdAnswer
                    Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Order By oRisp.numeroOpzione Where oRisp.idRispostaQuestionario = idAnswer Select oRisp.numeroOpzione, oRisp.testoOpzione).ToList
                    'row.Items.Add(oDom.id)
                    Dim listIndex As Integer = 0
                    For i As Integer = 0 To nOpzioni
                        'If firstRun Then
                        '    'dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1))
                        '    dto.Cells.Add(String.Format(HeaderOptionFormat, oDom.id, i + 1))
                        'End If

                        If listIndex < numeroOpzList.count AndAlso numeroOpzList(listIndex).numeroOpzione = i + 1 Then
                            'row.Items.Add("1")
                            value = dicValues(i + 1)

                            'VERIFICA "IsALTRO" => TESTO LIBERO!!!
                            If oDom.domandaMultiplaOpzioni(i).isAltro Then
                                isaltro = True
                                'row.Items.Add(numeroOpzList(listIndex).testoOpzione)

                                ValueAltro = numeroOpzList(listIndex).testoOpzione

                                If firstRun Then
                                    'dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1) & FreeTextIdentifier)
                                    dto.Cells.Add(String.Format(HeaderOptionFormatTextValue, oDom.id, i + 1))
                                End If
                            End If
                            listIndex += 1
                        Else
                            'row.Items.Add("")
                            If oDom.domandaMultiplaOpzioni(i).isAltro Then
                                isaltro = True
                                'row.Items.Add("")
                                If firstRun Then
                                    'dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1) & FreeTextIdentifier)
                                    dto.Cells.Add(String.Format(HeaderOptionFormatTextValue, oDom.id, i + 1))
                                End If
                            End If
                        End If
                    Next
                    firstRun = False

                    'If Not hasSelection Then
                    row.Items.Add(value) 'VERIFICARE!!!!
                    If isaltro Then
                        row.Items.Add(ValueAltro)
                    End If
                    'End If

                Next



            End If

            '--------------------------------------------------

            'Dim firstRun As Boolean = True
            'Dim nOpzioni As Integer = oDom.domandaMultiplaOpzioni.Count - 1

            ''dto.Cells.Add(CellTitleIdQuestion & "_" & oDom.numero)

            'For Each row As dtoExportRow In dto.Rows
            '    Dim idAnswer As Integer = row.IdAnswer
            '    Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Order By oRisp.numeroOpzione Where oRisp.idRispostaQuestionario = idAnswer Select oRisp.numeroOpzione, oRisp.testoOpzione).ToList
            '    'row.Items.Add(oDom.id)
            '    Dim listIndex As Integer = 0
            '    For i As Integer = 0 To nOpzioni
            '        If firstRun Then
            '            'dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1))

            '            'Intestazione colonna
            '            dto.Cells.Add(String.Format(HeaderOptionFormat, oDom.id, i + 1))

            '        End If

            '        If listIndex < numeroOpzList.count AndAlso numeroOpzList(listIndex).numeroOpzione = i + 1 Then
            '            row.Items.Add("1")

            '            If oDom.domandaMultiplaOpzioni(i).isAltro Then
            '                row.Items.Add(numeroOpzList(listIndex).testoOpzione)
            '                If firstRun Then
            '                    dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1) & FreeTextIdentifier)
            '                End If
            '            End If
            '            listIndex += 1
            '        Else
            '            row.Items.Add("0")
            '            If oDom.domandaMultiplaOpzioni(i).isAltro Then
            '                row.Items.Add(NullItem)
            '                If firstRun Then
            '                    dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1) & FreeTextIdentifier)
            '                End If
            '            End If
            '        End If
            '    Next
            '    firstRun = False
            'Next
        End Sub




        Private Sub SetNotUsedRating(row As dtoExportRow, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, ByVal oDom As Domanda, optionsNumber As Integer, firstRun As Boolean)
            For j As Integer = 0 To optionsNumber
                For i As Integer = 0 To oDom.domandaRating.numeroRating
                    row.Items.Add(QuestionNotUsedIdentifier)
                    If firstRun Then
                        dto.Cells.Add(LibraryIdentifier & libraryNumber & QuestionIdentifier & oDom.numero & OptionIdentifier & (j + 1).ToString & "-" & (i + 1).ToString)
                    End If
                Next
                If oDom.domandaRating.opzioniRating(j).isAltro Then
                    row.Items.Add(QuestionNotUsedIdentifier)
                    If firstRun Then
                        dto.Cells.Add(LibraryIdentifier & oDom.numeroPagina & QuestionIdentifier & oDom.numero & OptionIdentifier & (j + 1).ToString & FreeTextIdentifier)
                    End If
                End If
            Next
        End Sub


        Private Function GetCellIdentifier(ByRef oDom As Domanda, Optional ByVal libraryNumber As Integer = -200) As String
            If libraryNumber <> -200 Then
                Return LibraryIdentifier & libraryNumber & IIf(oDom.numero > 0, QuestionIdentifier & oDom.numero, DeletedQuestionIdentifier & oDom.VirtualNumber)
            Else
                Return PageIdentifier & oDom.numeroPagina & IIf(oDom.numero > 0, QuestionIdentifier & oDom.numero, DeletedQuestionIdentifier & oDom.VirtualNumber)
            End If
        End Function

        Private Function GetCellOptionIdentifier(ByRef oDom As Domanda, ByVal optionNumber As Integer, Optional ByVal libraryNumber As Integer = -200) As String
            If libraryNumber <> -200 Then
                Return LibraryIdentifier & libraryNumber & IIf(oDom.numero > 0, QuestionIdentifier & oDom.numero, DeletedQuestionIdentifier & oDom.VirtualNumber) & OptionIdentifier & optionNumber.ToString
            Else
                Return PageIdentifier & oDom.numeroPagina & IIf(oDom.numero > 0, QuestionIdentifier & oDom.numero, DeletedQuestionIdentifier & oDom.VirtualNumber) & OptionIdentifier & optionNumber.ToString
            End If
        End Function
#End Region


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

    End Class
End Namespace