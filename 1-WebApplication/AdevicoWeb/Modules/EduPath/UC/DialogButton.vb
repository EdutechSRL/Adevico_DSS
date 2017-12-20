Namespace MyUC
    Public Class DialogButton
        Inherits Button

        Private _defaultValue As Integer
        Public Property DefaultValue As Integer
            Get
                Return _defaultValue
            End Get
            Set(ByVal value As Integer)
                _defaultValue = value
            End Set
        End Property

        Private _defaultValues As IList(Of Integer) = New List(Of Integer)
        Public Property DefaultValues As IList(Of Integer)
            Get
                Return _defaultValues
            End Get
            Set(ByVal value As IList(Of Integer))
                _defaultValues = value
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
                        code += "dlg.find('input[id$=\'_CHBoptions_" + item.ToString() + "\']').attr('checked','checked'); "
                    Next
                Else
                    code += "dlg.find('input[id$=\'_RBLoptions_" + DefaultValue.ToString() + "\']').attr('checked','checked');"

                End If

                code += "return false;"

                Me.OnClientClick = code
            End Set
        End Property

    End Class
End Namespace