Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers.Export

Namespace Business
    Public Class HelperExportToXml
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
        Public Sub New()
        End Sub
        Public Sub New(translations As Dictionary(Of QuestionnaireExportTranslations, String))
            CommonTranslations = translations
            _Helper = New BaseHelperExport(translations)
        End Sub

#Region "Export Questionnaire"
        Public Function QuestionnaireAnswers(ByVal userRequiringExport As lm.Comol.Core.DomainModel.Person, ByVal allowFullExport As Boolean, ByVal questionnaire As Questionario, answers As Dictionary(Of Long, QuestionnaireAnswer), libraries As List(Of Questionario), ByVal anonymousUser As String, ByVal displayNames As Dictionary(Of String, dtoDisplayName), Optional ByVal oneColumnForEachQuestion As Boolean = True, Optional onlyUserAnswer As Boolean = False, Optional userInfo As dtoDisplayName = Nothing) As String
            Dim nResponse As Integer = (questionnaire.domande.Count - 1)
            Dim dto As New dtoExportQuestionnaire

            'Dim index As Integer = 1
            'Dim addTaxCode As Boolean = False
            'dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleSurname))
            'dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleName))

            'Select Case userRequiringExport.TypeID
            '    Case CInt(UserTypeStandard.SysAdmin), CInt(UserTypeStandard.Administrator), CInt(UserTypeStandard.Administrative)
            '        dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleTaxCode))
            '        addTaxCode = True
            'End Select
            'dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleOtherInfos))
            'If questionnaire.risposteQuestionario.Count > 0 Then
            '    For Each answer As RispostaQuestionario In questionnaire.risposteQuestionario
            '        If questionnaire.risultatiAnonimi Then
            '            dto.Rows.Add(New dtoExportRow() With {.Index = index, .IdAnswer = answer.id, .DisplayInfo = New dtoDisplayName(anonymousUser)})
            '        Else
            '            If answer.idUtenteInvitato = 0 AndAlso displayNames.ContainsKey("i_" & answer.idUtenteInvitato) Then
            '                dto.Rows.Add(New dtoExportRow() With {.Index = index, .IdAnswer = answer.id, .DisplayInfo = displayNames("i_" & answer.idUtenteInvitato)})
            '            ElseIf answer.idPersona > 0 AndAlso displayNames.ContainsKey("p_" & answer.idPersona) Then
            '                dto.Rows.Add(New dtoExportRow() With {.Index = index, .IdAnswer = answer.id, .DisplayInfo = displayNames("p_" & answer.idPersona)})
            '            ElseIf displayNames.ContainsKey("p_0") Then
            '                dto.Rows.Add(New dtoExportRow() With {.Index = index, .IdAnswer = answer.id, .DisplayInfo = displayNames("p_0")})
            '            Else
            '                dto.Rows.Add(New dtoExportRow() With {.Index = index, .IdAnswer = answer.id, .DisplayInfo = New dtoDisplayName(anonymousUser)})
            '            End If
            '        End If

            '        index += 1
            '    Next
            'Else
            '    For Each answer As KeyValuePair(Of Long, QuestionnaireAnswer) In answers.ToList
            '        Dim oQuest As QuestionnaireAnswer = answer.Value
            '        '       Dim r As RispostaQuestionario = oQuest.rispostaQuest
            '        If questionnaire.risultatiAnonimi Then
            '            dto.Rows.Add(New dtoExportRow() With {.Index = index, .IdAnswer = oQuest.Id, .DisplayInfo = New dtoDisplayName(anonymousUser)})
            '        Else
            '            If oQuest.IdInvitedUser = 0 AndAlso displayNames.ContainsKey("i_" & oQuest.IdInvitedUser) Then
            '                dto.Rows.Add(New dtoExportRow() With {.Index = index, .IdAnswer = oQuest.Id, .DisplayInfo = displayNames("i_" & oQuest.IdInvitedUser)})
            '            ElseIf oQuest.IdUser > 0 AndAlso displayNames.ContainsKey("p_" & oQuest.IdUser) Then
            '                dto.Rows.Add(New dtoExportRow() With {.Index = index, .IdAnswer = oQuest.Id, .DisplayInfo = displayNames("p_" & oQuest.IdUser)})
            '            ElseIf displayNames.ContainsKey("p_0") Then
            '                dto.Rows.Add(New dtoExportRow() With {.Index = index, .IdAnswer = oQuest.Id, .DisplayInfo = displayNames("p_0")})
            '            Else
            '                dto.Rows.Add(New dtoExportRow() With {.Index = index, .IdAnswer = oQuest.Id, .DisplayInfo = New dtoDisplayName(anonymousUser)})
            '            End If
            '        End If

            '        index += 1
            '    Next
            'End If
            Dim index As Integer = 1
            dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleIdAnswer))
            Dim addTaxCode As Boolean = False
            If allowFullExport Then
                dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleSurname))
                dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleName))

                Select Case userRequiringExport.TypeID
                    Case CInt(UserTypeStandard.SysAdmin), CInt(UserTypeStandard.Administrator), CInt(UserTypeStandard.Administrative)
                        dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleTaxCode))
                        addTaxCode = True
                End Select
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
                    If questionnaire.risultatiAnonimi Then
                        dto.Rows.Add(New dtoExportRow() With {.Index = index, .IdAnswer = answer.id, .DisplayInfo = New dtoDisplayName(anonymousUser)})
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
            If questionnaire.tipo = QuestionnaireType.RandomMultipleAttempts Then
                Dim libraryNumber As Integer = 1
                For Each library As Questionario In libraries
                    _Helper.QuestionsRender(userRequiringExport, dto, libraryNumber, library.domande, answers)
                    libraryNumber += 1
                Next
            Else
                _Helper.QuestionsRender(dto, questionnaire.domande)
            End If

            Dim export As String = ""
            Dim content As String = ""

            Dim header As String = QuestionnaireAnswersHeader(questionnaire)
            content += QuestionnaireAnswersTableHeader(dto)
            content += QuestionnaireAnswersTableContent(allowFullExport, addTaxCode, dto)

            export += BuilderXmlDocument.AddWorkSheet("--", header & content)
            Return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings())
        End Function
        Public Function QuestionnaireAnswers(
                                            ByVal userRequiringExport As lm.Comol.Core.DomainModel.Person,
                                            ByVal allowFullExport As Boolean,
                                            ByVal idLanguage As Integer,
                                            qAnswers As List(Of dtoFullUserAnswerItem),
                                            answers As Dictionary(Of Long, QuestionnaireAnswer),
                                            ByVal anonymousUser As String,
                                            ByVal displayNames As Dictionary(Of String, dtoDisplayName),
                                            ByVal oQuest As Questionario,
                                            Optional onlyUserAnswer As Boolean = False,
                                            Optional userInfo As dtoDisplayName = Nothing
                                        ) As String

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

            Dim export As String = ""
            Dim content As String = ""



            '   Dim header As String = QuestionnaireAnswersHeader(questionnaire)
            content += QuestionnaireAnswersTableHeader(dto)
            content += QuestionnaireAnswersTableContent(allowFullExport, addTaxCode, dto)

            export += BuilderXmlDocument.AddWorkSheet("--", content)
            Return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings())
        End Function
        Private Function QuestionnaireAnswersHeader(ByVal questionnaire As Questionario) As String
            Dim tableData As String = ""
            Dim row As String = ""

            row += BuilderXmlDocument.AddData(CommonTranslations(QuestionnaireExportTranslations.QuestionnaireName), DefaultXmlStyleElement.Title.ToString)
            row += BuilderXmlDocument.AddData(questionnaire.nome, DefaultXmlStyleElement.Title.ToString)
            tableData += BuilderXmlDocument.AddRow(row)
            If questionnaire.tipo = QuestionnaireType.RandomMultipleAttempts Then
                row = BuilderXmlDocument.AddData(CommonTranslations(QuestionnaireExportTranslations.QuestionnaireAttempts), DefaultXmlStyleElement.Title.ToString)
                If questionnaire.MaxAttempts <= 0 Then
                    row += BuilderXmlDocument.AddData(CommonTranslations(QuestionnaireExportTranslations.QuestionnaireAttemptsNoLimits), DefaultXmlStyleElement.Title.ToString)
                Else
                    row += BuilderXmlDocument.AddData(questionnaire.MaxAttempts, DefaultXmlStyleElement.Title.ToString)
                End If
                tableData += BuilderXmlDocument.AddRow(row)
            End If
            row = BuilderXmlDocument.AddData(CommonTranslations(QuestionnaireExportTranslations.QuestionnaireAnonymous), DefaultXmlStyleElement.Title.ToString)
            row += BuilderXmlDocument.AddData(IIf(questionnaire.risultatiAnonimi, CommonTranslations(QuestionnaireExportTranslations.YesOption), CommonTranslations(QuestionnaireExportTranslations.NoOption)), DefaultXmlStyleElement.Title.ToString)
            tableData += BuilderXmlDocument.AddRow(row)
            tableData += BuilderXmlDocument.AddEmptyRows(3)

            row = BuilderXmlDocument.AddData(CommonTranslations(QuestionnaireExportTranslations.ColumnsLegend), DefaultXmlStyleElement.Title.ToString)
            If questionnaire.tipo = QuestionnaireType.RandomMultipleAttempts Then
                row += BuilderXmlDocument.AddData(CommonTranslations(QuestionnaireExportTranslations.LibraryIdentifierDescription))
            Else
                row += BuilderXmlDocument.AddData(CommonTranslations(QuestionnaireExportTranslations.PageIdentifierDescription))
            End If
            tableData += BuilderXmlDocument.AddRow(row)
            row = BuilderXmlDocument.AddEmptyData(DefaultXmlStyleElement.Title.ToString)
            row += BuilderXmlDocument.AddData(CommonTranslations(QuestionnaireExportTranslations.QuestionIdentifierDescription))
            tableData += BuilderXmlDocument.AddRow(row)

            row = BuilderXmlDocument.AddEmptyData(DefaultXmlStyleElement.Title.ToString)
            row += BuilderXmlDocument.AddData(CommonTranslations(QuestionnaireExportTranslations.DeletedQuestionIdentifierDescription))
            tableData += BuilderXmlDocument.AddRow(row)

            row = BuilderXmlDocument.AddEmptyData(DefaultXmlStyleElement.Title.ToString)
            row += BuilderXmlDocument.AddData(CommonTranslations(QuestionnaireExportTranslations.OptionIdentifierDescription))
            tableData += BuilderXmlDocument.AddRow(row)

            row = BuilderXmlDocument.AddEmptyData(DefaultXmlStyleElement.Title.ToString)
            row += BuilderXmlDocument.AddData(CommonTranslations(QuestionnaireExportTranslations.FreeTextIdentifierDescription))
            tableData += BuilderXmlDocument.AddRow(row)

            Return tableData
        End Function
        Private Function QuestionnaireAnswersTableHeader(dto As dtoExportQuestionnaire) As String
            Dim tableData As String = ""
            Dim row As String = ""

            If dto.Cells.Count > 0 Then
                dto.Cells.ForEach(Sub(c) tableData += AddTableCellTitle(c))
            End If

            Return BuilderXmlDocument.AddRow(tableData)
        End Function
        Private Function QuestionnaireAnswersTableContent(ByVal allowFullExport As Boolean, addTaxCode As Boolean, dto As dtoExportQuestionnaire) As String
            Dim tableData As String = ""
            Dim rowNumber As Integer = 0

            For Each rowItem As dtoExportRow In dto.Rows
                Dim row As String = ""
                Dim rowstyle As DefaultXmlStyleElement = IIf(rowNumber Mod 2 = 0, DefaultXmlStyleElement.RowAlternatingItem, DefaultXmlStyleElement.RowItem)
                row += BuilderXmlDocument.AddData(rowItem.IdAnswer, rowstyle.ToString())
                If allowFullExport Then
                    row += BuilderXmlDocument.AddData(rowItem.DisplayInfo.Surname, rowstyle.ToString())
                    row += BuilderXmlDocument.AddData(rowItem.DisplayInfo.Name, rowstyle.ToString())
                    If addTaxCode Then
                        row += BuilderXmlDocument.AddData(rowItem.DisplayInfo.TaxCode, rowstyle.ToString())
                    End If
                    row += BuilderXmlDocument.AddData(rowItem.DisplayInfo.OtherInfos, rowstyle.ToString())
                Else
                    row += BuilderXmlDocument.AddData(rowItem.DisplayInfo.DisplayName, rowstyle.ToString())
                End If

               
                For Each item As String In rowItem.Items
                    row += BuilderXmlDocument.AddData(item, rowstyle.ToString())
                Next
                rowNumber += 1
                tableData += BuilderXmlDocument.AddRow(row)
            Next

            Return tableData
        End Function
        Private Sub QuestionsRender(ByVal dto As dtoExportQuestionnaire, libraryNumber As Integer, ByVal questions As List(Of Domanda), answers As Dictionary(Of Long, QuestionnaireAnswer))
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
        Private Sub QuestionsRender(ByVal dto As dtoExportQuestionnaire, ByVal questions As List(Of Domanda))
            For Each oDom As Domanda In questions
                Select Case oDom.tipo
                    Case Domanda.TipoDomanda.DropDown
                        ConvertDropDownToString(oDom, dto)
                    Case Domanda.TipoDomanda.Multipla
                        ConvertMultiplaToString(oDom, dto)
                    Case Domanda.TipoDomanda.Numerica
                        ConvertNumericaToString(oDom, dto)
                    Case Domanda.TipoDomanda.Rating
                        ConvertRatingToString(oDom, dto)
                    Case Domanda.TipoDomanda.TestoLibero
                        ConvertTestoLiberoToString(oDom, dto)
                End Select
            Next
        End Sub

        Private Sub ConvertDropDownToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
            dto.Cells.Add(CellTitleIdQuestion)
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
                Dim qAnswer As QuestionnaireAnswer = answers(row.IdAnswer)

                row.Items.Add(oDom.id)
                If Not IsNothing(qAnswer) Then
                    Dim idQuestion As Integer = oDom.id
                    If (From q In qAnswer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
                        Dim numeroOpzList As List(Of Integer) = (From a In qAnswer.Answers Where a.IdQuestion = idQuestion Select a.OptionNumber).ToList
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

            dto.Cells.Add(CellTitleIdQuestion)

            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Order By oRisp.numeroOpzione Where oRisp.idRispostaQuestionario = idAnswer Select oRisp.numeroOpzione, oRisp.testoOpzione).ToList

                Dim listIndex As Integer = 0
                row.Items.Add(oDom.id)

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
                '   Dim question As Domanda = (From oRisp In quest.domande Where oRisp.id = idQuestion Select oRisp).FirstOrDefault()
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

            dto.Cells.Add(CellTitleIdQuestion)
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
            dto.Cells.Add(CellTitleIdQuestion)
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
                    Dim idQuestion As Integer = oDom.id

                    If Not (From q In qAnswer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
                        For i As Integer = 0 To nOpzioni
                            row.Items.Add(QuestionNotUsedIdentifier)
                        Next
                    Else
                        Dim numeroOpzList = (From a In qAnswer.Answers Where a.IdQuestion = idQuestion Order By a.OptionNumber Select OptionText = a.OptionText, OptionNumber = a.OptionNumber).ToList
                        For i As Integer = 0 To nOpzioni
                            If i < numeroOpzList.count AndAlso (numeroOpzList(i).OptionNumber = i + 1 OrElse (oDom.opzioniTestoLibero.Count = 1 AndAlso numeroOpzList(i).OptionNumber = 0)) Then
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

            dto.Cells.Add(CellTitleIdQuestion)
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
        Private Sub ConvertRatingToString(ByVal oDom As Domanda, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, answers As Dictionary(Of Long, QuestionnaireAnswer))
            Dim firstRun As Boolean = True
            Dim nOpzioni As Integer = oDom.domandaRating.opzioniRating.Count - 1


            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim qAnswer As QuestionnaireAnswer = answers(row.IdAnswer)
                If Not IsNothing(qAnswer) Then
                    Dim idQuestion As Integer = oDom.id

                    If Not (From q In qAnswer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
                        SetNotUsedRating(row, dto, libraryNumber, oDom, nOpzioni, firstRun)
                    Else
                        For j As Integer = 0 To nOpzioni
                            Dim isSelectedRow As Boolean = False
                            Dim index As Integer = j
                            Dim numeroOpzList = (From a In qAnswer.Answers Where a.IdQuestion = idQuestion Order By a.IdQuestionOption Where a.IdQuestionOption = oDom.domandaRating.opzioniRating(index).id Select Value = a.Value, OptionText = a.OptionText).ToList
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
                rowNumber += 1
            Next
            export += BuilderXmlDocument.AddWorkSheet("--", header & content)
            Return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings())
        End Function
        Private Function AttemptsHeader(answers As List(Of dtoUserAnswerItem), multipleAttempts As Boolean, hasCompleted As Boolean, hasSemiCorrectAnswers As Boolean, hasUngradedAnswers As Boolean, hasQuestionsSkipped As Boolean) As String
            Dim tableData As String = ""
            tableData += AddTableCellTitle(CommonTranslations(QuestionnaireExportTranslations.CellTitleIdAnswer))
            tableData += AddTableCellTitle(CommonTranslations(QuestionnaireExportTranslations.CellTitleUserName))
            If multipleAttempts Then
                tableData += AddTableCellTitle(CommonTranslations(QuestionnaireExportTranslations.CellTitleAttemptNumber))
            End If
            If hasCompleted Then
                tableData += AddTableCellTitle(CommonTranslations(QuestionnaireExportTranslations.CellTitleAttemptDate))
            End If
            tableData += AddTableCellTitle(CommonTranslations(QuestionnaireExportTranslations.CellTitleQuestionsCount))
            tableData += AddTableCellTitle(CommonTranslations(QuestionnaireExportTranslations.CellTitleCorrectAnswers))
            If hasSemiCorrectAnswers Then
                tableData += AddTableCellTitle(CommonTranslations(QuestionnaireExportTranslations.CellTitleSemiCorrectAnswers))
            End If
            If hasUngradedAnswers Then
                tableData += AddTableCellTitle(CommonTranslations(QuestionnaireExportTranslations.CellTitleUngradedAnswers))
            End If
            If answers.Where(Function(a) a.QuestionsSkipped.HasValue).Any() Then
                tableData += AddTableCellTitle(CommonTranslations(QuestionnaireExportTranslations.CellTitleQuestionsSkipped))
            End If
            tableData += AddTableCellTitle(CommonTranslations(QuestionnaireExportTranslations.CellTitleWrongAnswers))
            tableData += AddTableCellTitle(CommonTranslations(QuestionnaireExportTranslations.CellTitleAttemptScore))
            tableData += AddTableCellTitle(CommonTranslations(QuestionnaireExportTranslations.CellTitleMinScore))
            tableData += AddTableCellTitle(CommonTranslations(QuestionnaireExportTranslations.CellTitleMaxScore))

            Return BuilderXmlDocument.AddRow(tableData)
        End Function
        Private Function QuestionnaireAttemptRow(minScore As Integer, maxScore As Integer, answer As dtoUserAnswerItem, multipleAttempts As Boolean, hasCompleted As Boolean, hasSemiCorrectAnswers As Boolean, hasUngradedAnswers As Boolean, hasQuestionsSkipped As Boolean, rowNumber As Integer) As String
            Dim rowstyle As DefaultXmlStyleElement = IIf(rowNumber Mod 2 = 0, DefaultXmlStyleElement.RowAlternatingItem, DefaultXmlStyleElement.RowItem)
            Dim row As String = ""

            row += BuilderXmlDocument.AddData(answer.Id, rowstyle.ToString())
            row += BuilderXmlDocument.AddData(answer.DisplayName, rowstyle.ToString())

            If multipleAttempts Then
                row += BuilderXmlDocument.AddData(answer.AttemptNumber, rowstyle.ToString())
            End If

            If hasCompleted Then
                If answer.CompletedOn.HasValue Then
                    row += BuilderXmlDocument.AddData(answer.CompletedOn.Value, rowstyle.ToString())
                Else
                    row += BuilderXmlDocument.AddData(CommonTranslations(QuestionnaireExportTranslations.ItemEmpty), rowstyle.ToString())
                End If
            End If
            If answer.QuestionsCount.HasValue Then
                row += BuilderXmlDocument.AddData(answer.QuestionsCount.Value, rowstyle.ToString())
            Else
                row += BuilderXmlDocument.AddData(0, rowstyle.ToString())
            End If
            If answer.CorrectAnswers.HasValue Then
                row += BuilderXmlDocument.AddData(answer.CorrectAnswers.Value, rowstyle.ToString())
            Else
                row += BuilderXmlDocument.AddData(0, rowstyle.ToString())
            End If
            If hasSemiCorrectAnswers Then
                If answer.SemiCorrectAnswers.HasValue Then
                    row += BuilderXmlDocument.AddData(answer.SemiCorrectAnswers.Value, rowstyle.ToString())
                Else
                    row += BuilderXmlDocument.AddData(0, rowstyle.ToString())
                End If
            End If
            If hasUngradedAnswers Then
                If answer.UngradedAnswers.HasValue Then
                    row += BuilderXmlDocument.AddData(answer.UngradedAnswers.Value, rowstyle.ToString())
                Else
                    row += BuilderXmlDocument.AddData(0, rowstyle.ToString())
                End If
            End If
            If hasQuestionsSkipped Then
                If answer.QuestionsSkipped.HasValue Then
                    row += BuilderXmlDocument.AddData(answer.QuestionsSkipped.Value, rowstyle.ToString())
                Else
                    row += BuilderXmlDocument.AddData(0, rowstyle.ToString())
                End If
            End If
            If answer.WrongAnswers.HasValue Then
                row += BuilderXmlDocument.AddData(answer.WrongAnswers.Value, rowstyle.ToString())
            Else
                row += BuilderXmlDocument.AddData(0, rowstyle.ToString())
            End If

            row += BuilderXmlDocument.AddData(answer.RelativeScore, rowstyle.ToString())
            row += BuilderXmlDocument.AddData(minScore, rowstyle.ToString())
            row += BuilderXmlDocument.AddData(maxScore, rowstyle.ToString())

            Return BuilderXmlDocument.AddRow(row)
        End Function
#End Region

#Region "Styles"
        Private Function AddTableCellTitle(data As String) As String
            Return BuilderXmlDocument.AddData(data, DefaultXmlStyleElement.HeaderTable.ToString())
        End Function
        Private Function AddTableCellTitle(data As String, colspan As Int32) As String
            Return BuilderXmlDocument.AddData(data, DefaultXmlStyleElement.HeaderTable.ToString(), colspan)
        End Function
        Private Function AddTableCellSubTitle(data As String) As String
            Return BuilderXmlDocument.AddData(data, DefaultXmlStyleElement.SubHeaderTable.ToString())
        End Function
        Private Function AddTableCellSubTitle(data As String, colspan As Int32) As String
            Return BuilderXmlDocument.AddData(data, DefaultXmlStyleElement.SubHeaderTable.ToString(), colspan)
        End Function
#End Region

        'Public Enum ExportContentType
        '    Ans = 1
        '    UsersStatistics = 2
        'End Enum
    End Class
End Namespace