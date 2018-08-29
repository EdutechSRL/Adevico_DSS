Imports COL_DataLayer
<Serializable()>
 Public Class COL_Stato
    Inherits ObjectBase

#Region "Private Property"
    Private _id As Integer
    Private _Descrizione As String
    Private _errore As Errori_Db
#End Region

#Region "Public Property"
    Public Property ID() As Integer
        Get
            ID = _id
        End Get
        Set(ByVal Value As Integer)
            _id = Value
        End Set
    End Property
    Public Property Descrizione() As String
        Get
            Descrizione = _Descrizione
        End Get
        Set(ByVal Value As String)
            _Descrizione = Value
        End Set
    End Property
    Public ReadOnly Property ErroreDB() As String
        Get
            ErroreDB = _errore
        End Get
    End Property
#End Region

#Region "Metodi New"
    Sub New()
        Me._errore = Errori_Db.None
    End Sub
    Sub New(ByVal StatoID As Integer, ByVal StatoDesc As String)
        Me._id = StatoID
        Me._Descrizione = StatoDesc
        Me._errore = Errori_Db.None
    End Sub
#End Region


#Region "Metodi"
    'Public Function Elenca() As DataSet
    '	Dim oRequest As New COL_Request
    '	Dim oParam As New COL_Request.Parameter
    '	Dim dsTable As New DataSet
    '	Dim objAccesso As New COL_DataAccess

    '	With oRequest
    '		.Command = "sp_Stato_Elenca"
    '		.CommandType = CommandType.StoredProcedure
    '		.Role = COL_Request.UserRole.Admin
    '		.transactional = False
    '	End With
    '	Try
    '		dsTable = objAccesso.GetdataSet(oRequest)
    '	Catch ex As Exception
    '		Me._errore = Errori_Db.DBError
    '	End Try
    '	Return dsTable
    'End Function
    Public Sub Modifica()
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Stato_Modifica"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@STTO_Id", _id, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@STTO_descrizione", _Descrizione.Trim, ParameterDirection.Input, SqlDbType.VarChar, , 150)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            PurgeCacheItems(CachePolicy.Stato)
        Catch ex As Exception
            Me._errore = Errori_Db.DBChange
        End Try
    End Sub
    Public Sub Aggiungi()
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_Stato_Aggiungi"
            .CommandType = CommandType.StoredProcedure
            oParam = objAccesso.GetAdvancedParameter("@STTO_Id", "", ParameterDirection.Output, SqlDbType.Int)
            .Parameters.Add(oParam)
            oParam = objAccesso.GetAdvancedParameter("@STTO_descrizione", _Descrizione.Trim, ParameterDirection.Input, SqlDbType.VarChar, , 150)
            .Parameters.Add(oParam)
            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With
        Try
            objAccesso.GetExecuteNotQuery(oRequest)
            PurgeCacheItems(CachePolicy.Stato)
        Catch ex As Exception
            Me._errore = Errori_Db.DBInsert
        End Try
    End Sub

    Public Shared Function GetByID(ByVal StatoID As Integer) As COL_Stato
        Dim oLista As New List(Of COL_Stato)
        oLista = COL_Stato.List()

        If oLista.Count = 0 Then
            Return Nothing
        Else
            Dim oStato As COL_Stato
            oStato = oLista.Find(New GenericPredicate(Of COL_Stato, Integer)(StatoID, AddressOf FindByID))
            Return oStato
        End If
    End Function
    Public Shared Function GetByName(ByVal Name As String) As COL_Stato
        Dim oLista As New List(Of COL_Stato)
        oLista = COL_Stato.List()

        If oLista.Count = 0 Then
            Return Nothing
        Else
            Dim oStato As COL_Stato
            oStato = oLista.Find(New GenericPredicate(Of COL_Stato, String)(Name, AddressOf FindByName))
            Return oStato
        End If
    End Function
    Public Shared Function List(Optional ByVal sortExpression As String = "", Optional ByVal sortDirection As String = "") As List(Of COL_Stato)
        Dim oLista As New List(Of COL_Stato)
        Dim cacheKey As String = CachePolicy.Stato

        If sortDirection <> String.Empty Then
            sortDirection = sortDirection.ToLower
        End If

        If ObjectBase.Cache(cacheKey) Is Nothing Then
            oLista = COL_Stato.RetrieveListFromDB()
            ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.ScadenzaMensile)
        Else
            oLista = CType(ObjectBase.Cache(cacheKey), List(Of COL_Stato))
        End If


        If (Not sortExpression Is Nothing AndAlso sortDirection <> String.Empty) Then
            oLista.Sort(New GenericComparer(Of Provincia)(sortExpression))
        End If

        If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
            oLista.Reverse()
        End If
        Return oLista
    End Function
    Private Shared Function RetrieveListFromDB() As List(Of COL_Stato)
        Dim oLista As New List(Of COL_Stato)
        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim objAccesso As New COL_DataAccess
        Dim oDatareader As IDataReader

        With oRequest
            .Command = "sp_Stato_Elenca"
            .CommandType = CommandType.StoredProcedure

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            oDatareader = objAccesso.GetdataReader(oRequest)
            While oDatareader.Read
                Try
                    oLista.Add(New COL_Stato(oDatareader("STTO_Id"), GenericValidator.ValString(oDatareader("STTO_descrizione"), "")))
                Catch ex As Exception

                End Try
            End While
            oDatareader.Close()
        Catch ex As Exception
        Finally
            If Not IsNothing(oDatareader) Then
                If oDatareader.IsClosed = False Then
                    oDatareader.Close()
                End If
            End If
        End Try
        Return oLista
    End Function
    Private Shared Function FindByID(ByVal item As COL_Stato, ByVal argument As Integer) As Boolean
        Return item.ID = argument
    End Function
    Private Shared Function FindByName(ByVal item As COL_Stato, ByVal argument As String) As Boolean
        Return item.Descrizione = argument
    End Function
#End Region

End Class