Public Class UC_TagEdit
    Inherits TGbaseControl

#Region "Internal"
    Private _service As lm.Comol.Core.Business.BaseModuleManager

    Private ReadOnly Property service As lm.Comol.Core.Business.BaseModuleManager
        Get
            If IsNothing(_service) Then
                Me._service = New lm.Comol.Core.Business.BaseModuleManager(PageUtility.CurrentContext)
            End If
            Return _service
        End Get
    End Property
    Public ReadOnly Property EditTagTitle As String
        Get
            Return Resource.getValue("EditTagTitle")
        End Get
    End Property
    Public ReadOnly Property ViewTagTitle As String
        Get
            Return Resource.getValue("ViewTagTitle")
        End Get
    End Property
    Public ReadOnly Property NewTagTitle As String
        Get
            Return Resource.getValue("NewTagTitle")
        End Get
    End Property
    Public ReadOnly Property EditTagLinkTitle As String
        Get
            Return Resource.getValue("EditTagLinkTitle")
        End Get
    End Property
    Public ReadOnly Property ViewTagLinkTitle As String
        Get
            Return Resource.getValue("ViewTagLinkTitle")
        End Get
    End Property
    Public ReadOnly Property BulkInsertTile As String
        Get
            Return Resource.getValue("BulkInsertTile")
        End Get
    End Property
    Private Property IdLanguages As List(Of Integer)
        Get
            Return ViewStateOrDefault("IdLanguages", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            ViewState("IdLanguages") = value
        End Set
    End Property

    Public Event BulkInsert(results As List(Of Dictionary(Of Integer, String)))

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBtagIsDefault_t)
            .setLiteral(LTtagIsDefault)
            .setLabel(LBtagTranslations_t)
            .setLabel(LBtagName_t)
            .setLabel(LBtagDescription_t)
            .setButton(BTNsaveTag, True)
            .setLabel(LBtagOrganizations_t)

            .setLiteral(LTapplyTagDescription)
            .setLabel(LBapplyTagOn_t)
            .setLiteral(LTapplyTagOn)
            .setLabel(LBapplyTagOnAllCommunityTypes_t)
            .setLiteral(LTapplyTagOnAllCommunityTypes)
            .setLabel(LBtagDefaultFor_t)

            .setLabel(LBapplyThisTagOn_t)
            .setLiteral(LTapplyThisTagOn)

            .setLabel(LBselectCommunityTypesForTagApply)
            .setButton(BTNapplyTag, True)


            .setLiteral(LTbulkInsertDescription)
            .setLabel(LBtagBulkInsert)
            .setButton(BTNbulkInsert, True)
            If Not Page.IsPostBack Then
                Dim languages As List(Of lm.Comol.Core.DomainModel.Language) = service.GetAllLanguages().OrderByDescending(Function(l) l.isDefault).ThenBy(Function(l) l.Name).ToList()
                IdLanguages = languages.Select(Function(l) l.Id).ToList()
                If (LBtagBulkInsert.Text.Contains("{0}")) Then
                    LBtagBulkInsert.Text = String.Format(LBtagBulkInsert.Text, String.Join("#", languages.Select(Function(l) l.Name).ToList()))
                End If
            End If
        End With
    End Sub

    Private Sub BTNbulkInsert_Click(sender As Object, e As EventArgs) Handles BTNbulkInsert.Click
        Dim languages As List(Of Integer) = IdLanguages
        Dim rows As List(Of String) = TXBbulkInsert.Text.Split(vbCrLf.ToCharArray, StringSplitOptions.RemoveEmptyEntries).ToList()
        Dim results As New List(Of Dictionary(Of Integer, String))
        If rows.Any() Then
            For Each row As String In rows
                Try
                    Dim translations As List(Of String) = row.Split("#".ToCharArray, StringSplitOptions.RemoveEmptyEntries).ToList()

                    If translations.Any() Then
                        Dim result As New Dictionary(Of Integer, String)()
                        result.Add(0, translations(0))

                        Dim index As Integer = 1
                        For index = 1 To translations.Count - 1
                            result.Add(languages(index - 1), translations(index))
                        Next
                        results.Add(result)
                    End If
                Catch ex As Exception

                End Try
            Next
            RaiseEvent BulkInsert(results)
        End If
        TXBbulkInsert.Text = ""
    End Sub
End Class