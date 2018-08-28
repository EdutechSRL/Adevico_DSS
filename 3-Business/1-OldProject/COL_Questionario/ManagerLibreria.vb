Imports lm.Comol.Core.DomainModel

Namespace Comol.Manager
    Public Class ManagerLibreria
        Inherits COL_BusinessLogic_v2.ObjectBase

        Public Shared Function GetLibreria(appContext As iApplicationContext, ByRef idLibreria As Integer, ByRef idLingua As Integer, Optional ByVal ForceRetrieve As Boolean = False) As Questionario
            Dim oLibreria As New Questionario
            Dim cacheKey As String = CachePolicyEvent.Libreria(oLibreria.id, idLingua)
            If Cache(cacheKey) Is Nothing Or ForceRetrieve Then
                oLibreria = DALQuestionario.readQuestionarioBYLingua(appContext, idLibreria, idLingua, False)
                Cache.Insert(cacheKey, oLibreria, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
            Else
                oLibreria = CType(Cache(cacheKey), Questionario)
            End If

            Return oLibreria
        End Function

    End Class

End Namespace