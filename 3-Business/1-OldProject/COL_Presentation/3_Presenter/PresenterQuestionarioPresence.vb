Public Class PresenterQuestionarioPresence
    Inherits GenericPresenter

    Public Sub New(ByVal view As IViewCommon)
        MyBase.view = view
    End Sub
    Private Shadows ReadOnly Property View() As IviewQuestionarioPresence
        Get
            View = MyBase.view
        End Get
    End Property

   
End Class