Namespace MyUC
    Public Class DialogLinkButton
        Inherits LinkButton

        Private _defaultValue As Integer = 0
        Public Property DefaultValue As Integer
            Get
                Return _defaultValue
            End Get
            Set(ByVal value As Integer)
                _defaultValue = value
            End Set
        End Property

        Private _selectedItemsIndex As IList(Of Integer) = New List(Of Integer)
        Public Property DefaultValues As IList(Of Integer)
            Get
                Return _selectedItemsIndex
            End Get
            Set(ByVal value As IList(Of Integer))
                _selectedItemsIndex = value
            End Set
        End Property

        Private _multiSelection As Boolean = False
        Public Property MultiSelection As Boolean
            Get
                Return _multiSelection
            End Get
            Set(ByVal value As Boolean)
                _multiSelection = value
            End Set
        End Property


        Private _dialogID As String
        Public Property DialogID As String
            Get
                Return _dialogID
            End Get
            Set(ByVal value As String)
                _dialogID = value
                Dim code As String = "var dlg=$('.dialog').filter('#" + DialogID + "').dialog('open'); dlg.children('.CommandArgument').val('" + CommandArgument + "'); dlg.children('.CommandName').val('" + CommandName + "');"

                If (MultiSelection) Then
                    code += "dlg.find('input[id*=\'_CHBoptions_\']').attr('checked',''); "
                    For Each item As Integer In DefaultValues

                        code += "dlg.find('input[id$=\'_CHBoptions_" + item.ToString() + "\']').attr('checked','checked'); "
                    Next
                Else
                    code += "dlg.find('input[id$=\'_RBLoptions_" + DefaultValue.ToString() + "\']').attr('checked','checked');"

                End If

                code += "return false;"
                Me.OnClientClick = code
            End Set
        End Property

        Private _dialogClass As String
        Public Property DialogClass As String
            Get
                Return _dialogClass
            End Get
            Set(ByVal value As String)
                _dialogClass = value

                Dim code As String = "var dlg=$('.dialog').filter('." + DialogClass + "').dialog('open'); dlg.children('.CommandArgument').val('" + CommandArgument + "'); dlg.children('.CommandName').val('" + CommandName + "');"

                If (MultiSelection) Then
                    For Each item As Integer In DefaultValues
                        code += "dlg.find('input[id$=\'_CHBoptions_\']').attr('checked',false); "
                        code += "dlg.find('input[id$=\'_CHBoptions_" + item.ToString() + "\']').attr('checked','checked'); "
                    Next
                Else
                    code += "dlg.find('input[id$=\'_RBLoptions_" + DefaultValue.ToString() + "\']').attr('checked','checked');"

                End If

                code += "return false;"

                Me.OnClientClick = code
            End Set
        End Property
        Public Sub InitializeMultiSelectControlByClass(ByVal DialogClass As String, ByVal selectItemsIndex As List(Of Integer))
            _selectedItemsIndex = selectItemsIndex
            _multiSelection = True

            _dialogClass = DialogClass
            Dim code As String = ""

            code += "dlg.find('input[id*=\'_CHBoptions_\']').attr('checked',''); "
            For Each item As Integer In selectItemsIndex
                code += "dlg.find('input[id$=\'_CHBoptions_" + item.ToString() + "\']').attr('checked','checked'); "
            Next
            InitializeControlByClass(DialogClass, code)
        End Sub
        Public Sub InitializeSingleSelectControlByClass(ByVal DialogClass As String, ByVal selectItemIndex As Integer)
            _defaultValue = selectItemIndex
            _multiSelection = False

            _dialogClass = DialogClass
            Dim code As String = "dlg.find('input[id$=\'_RBLoptions_" + selectItemIndex.ToString() + "\']').attr('checked','checked');"
            InitializeControlByClass(DialogClass, code)
        End Sub
        'Public Sub InitializeMultiSelectControlByClass(ByVal DialogClass As String, ByVal selectItemsIndex As List(Of Integer), ByVal unselectItemsIndex As List(Of Integer))
        '    _selectedItemsIndex = selectItemsIndex
        '    _multiSelection = True

        '    _dialogClass = DialogClass
        '    Dim code As String = ""

        '    For Each item As Integer In selectItemsIndex
        '        code += "dlg.find('input[id$=\'_CHBoptions_" + item.ToString() + "\']').attr('checked','checked'); "
        '    Next

        '    For Each item As Integer In unselectItemsIndex
        '        code += "dlg.find('input[id$=\'_CHBoptions_" + item.ToString() + "\']').attr('checked',''); "
        '    Next
        '    InitializeControlByClass(DialogClass, code)
        'End Sub
        'Public Sub InitializeSingleSelectControlByClass(ByVal DialogClass As String, ByVal selectItemIndex As Integer)
        '    _defaultValue = selectItemIndex
        '    _multiSelection = False

        '    _dialogClass = DialogClass
        '    Dim code As String = "dlg.find('input[id$=\'_RBLoptions_" + selectItemIndex.ToString() + "\']').attr('checked','checked');"
        '    InitializeControlByClass(DialogClass, code)
        'End Sub
        Private Sub InitializeControlByClass(ByVal dialog As String, ByVal selectionCode As String)
            Dim code As String = "var dlg=$('.dialog').filter('." + dialog + "').dialog('open'); dlg.children('.CommandArgument').val('" + CommandArgument + "'); dlg.children('.CommandName').val('" + CommandName + "');"
            code += selectionCode
            code += "return false;"
            Me.OnClientClick = code
        End Sub
    End Class
End Namespace