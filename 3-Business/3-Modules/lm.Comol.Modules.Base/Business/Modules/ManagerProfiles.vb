Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.DomainModel

Imports NHibernate
Imports NHibernate.Linq

Namespace lm.Comol.Modules.Base.BusinessLogic
    Public Class ManagerProfiles
        Inherits COL_BusinessLogic_v2.ObjectBase
        Implements lm.Comol.Core.DomainModel.Common.iDomainManager

#Region "Private property"
        Private _UserContext As iUserContext
        Private _Datacontext As iDataContext
#End Region

#Region "Public property"
        Private ReadOnly Property DC() As iDataContext
            Get
                Return _Datacontext
            End Get
        End Property
        Private ReadOnly Property CurrentUserContext() As iUserContext
            Get
                Return _UserContext
            End Get
        End Property
#End Region

        Public Sub New()
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext)
            Me._UserContext = oContext.UserContext
            Me._Datacontext = oContext.DataContext
        End Sub
        Public Sub New(ByVal oUserContext As iUserContext, ByVal oDatacontext As iDataContext)
            Me._UserContext = oUserContext
            Me._Datacontext = oDatacontext
        End Sub

        Public Function GetProfileTypesID(ByVal OrganizationID As Integer) As List(Of Integer)
            Dim oList As List(Of Integer) = Nothing
            Try
                DC.BeginTransaction()
                oList = (From profile In DC.GetCurrentSession.Linq(Of OrganizationProfiles)() Where (OrganizationID = -1 OrElse profile.OrganizationID = OrganizationID) Distinct Select profile.Profile.TypeID).ToList
                DC.Commit()

            Catch ex As Exception
                Debug.Write(ex.ToString)
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                oList = New List(Of Integer)
            End Try
            Return oList
        End Function
        Public Function GetProfilesStatus(ByVal OrganizationID As Integer, ByVal ProfileTypeID As Integer) As List(Of ProfileStatus)
            Dim oList As New List(Of ProfileStatus)

            Try
                DC.BeginTransaction()

                If (From p In DC.GetCurrentSession.Linq(Of OrganizationProfiles)() Where (OrganizationID = -1 OrElse p.OrganizationID = OrganizationID) AndAlso (ProfileTypeID = -1 OrElse ProfileTypeID = p.Profile.TypeID) AndAlso p.Profile.isDisabled = False).Count > 0 Then
                    oList.Add(ProfileStatus.Active)
                End If
                If (From p In DC.GetCurrentSession.Linq(Of OrganizationProfiles)() Where (OrganizationID = -1 OrElse p.OrganizationID = OrganizationID) AndAlso (ProfileTypeID = -1 OrElse ProfileTypeID = p.Profile.TypeID) AndAlso p.Profile.isDisabled = True).Count > 0 Then
                    oList.Add(ProfileStatus.Disabled)
                End If

                If (From w In DC.GetCurrentSession.Linq(Of WaitingActivationPerson)() Where _
                    (OrganizationID = -1 OrElse (From p In DC.GetCurrentSession.Linq(Of OrganizationProfiles)() Where p.OrganizationID = OrganizationID).Count > 0) _
                    AndAlso (ProfileTypeID = -1 OrElse ProfileTypeID = w.WaitingProfile.TypeID) AndAlso w.WaitingProfile.isDisabled = True).Count > 0 Then
                    oList.Add(ProfileStatus.Waiting)
                End If

                'If oList.Count = 0 OrElse oList.Count > 1 Then
                '    oList.Add(ProfileStatus.All)
                'End If
                DC.Commit()

            Catch ex As Exception
                Debug.Write(ex.ToString)
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                oList.Add(ProfileStatus.All)
            End Try
            Return oList
        End Function
        Public Function GetAuthenticationTypeID(ByVal OrganizationID As Integer) As List(Of Integer)
            Dim oList As List(Of Integer) = Nothing
            Try
                DC.BeginTransaction()
                oList = (From p In DC.GetCurrentSession.Linq(Of OrganizationProfiles)() Where (OrganizationID = -1 OrElse p.OrganizationID = OrganizationID) Select p.Profile.AuthenticationTypeID).Distinct.ToList
                DC.Commit()

            Catch ex As Exception
                Debug.Write(ex.ToString)
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                oList = New List(Of Integer)
            End Try
            Return oList
        End Function

        Public Function GetProfilesCount(ByVal oDto As dtoProfileFilters) As Integer
            Dim iCount As Integer = 0
            Try
                DC.BeginTransaction()
                Dim Query = (From w In DC.GetCurrentSession.Linq(Of Person)())
                Dim oChars As List(Of String) = Me.DefaultChars
                Dim oOtherChars As List(Of String) = Me.DefaultOtherChars
                If oDto.SearchFor = ProfileSearchBy.Matricola AndAlso oDto.Value <> "" Then
                    Dim oWaiting As List(Of Integer)
                    If oDto.Status = ProfileStatus.Waiting OrElse oDto.Status = ProfileStatus.All Then
                        oWaiting = (From p In DC.GetCurrentSession.Linq(Of WaitingActivationPerson)() Where p.WaitingProfile.isDisabled Select p.Id).ToList
                    Else
                        oWaiting = New List(Of Integer)
                    End If
                    Dim UserQuery = (From p In DC.GetCurrentSession.Linq(Of OrganizationProfiles)() Where ((oDto.OrganizationID = -1 AndAlso p.isDefault = True) OrElse oDto.OrganizationID = p.OrganizationID) _
                                AndAlso (oDto.ProfileTypeID = -1 OrElse oDto.ProfileTypeID = p.Profile.TypeID) _
                          AndAlso (oDto.AuthenticationTypeID = -1 OrElse p.Profile.AuthenticationTypeID = oDto.AuthenticationTypeID) _
AndAlso ( _
                                                             (oDto.Status = ProfileStatus.All _
                                                              OrElse _
                                                             (oDto.Status = ProfileStatus.Active AndAlso p.Profile.isDisabled = False) _
                                                             OrElse _
                                                             (oDto.Status = ProfileStatus.Disabled AndAlso p.Profile.isDisabled AndAlso oWaiting.Contains(p.Profile.Id) = False) _
                                                             OrElse _
                                                             (oDto.Status = ProfileStatus.Waiting AndAlso oWaiting.Contains(p.Profile.Id)))))
                    iCount = UserQuery.Count
                Else
                    If oDto.Status <> ProfileStatus.Waiting Then
                        '(oDto.StartWith = "#" AndAlso oOtherChars.Contains(p.Profile.Surname.Chars(1).ToString)) OrElse
                        Dim UserQuery = (From p In DC.GetCurrentSession.Linq(Of OrganizationProfiles)() _
                                     Where ((oDto.OrganizationID = -1 AndAlso p.isDefault = True) OrElse p.OrganizationID = oDto.OrganizationID) _
                                     AndAlso (oDto.ProfileTypeID = -1 OrElse oDto.ProfileTypeID = p.Profile.TypeID) _
                                     AndAlso (oDto.AuthenticationTypeID = -1 OrElse (oDto.AuthenticationTypeID = -1 OrElse p.Profile.AuthenticationTypeID = oDto.AuthenticationTypeID)) _
                                     AndAlso (oDto.StartWith = "" OrElse p.Profile.Surname.StartsWith(oDto.StartWith, StringComparison.OrdinalIgnoreCase)) _
                                     AndAlso (oDto.Status = ProfileStatus.All OrElse (oDto.Status = ProfileStatus.Active AndAlso p.Profile.isDisabled = False) OrElse (oDto.Status = ProfileStatus.Disabled AndAlso p.Profile.isDisabled)) _
                        AndAlso (oDto.SearchFor = ProfileSearchBy.All OrElse oDto.Value = "" OrElse ( _
                               (oDto.SearchFor = ProfileSearchBy.Contains AndAlso (p.Profile.Name.Contains(oDto.Value) OrElse p.Profile.Surname.Contains(oDto.Value))) _
                               OrElse (oDto.SearchFor = ProfileSearchBy.Login AndAlso p.Profile.Login.Contains(oDto.Value)) _
                                  OrElse (oDto.SearchFor = ProfileSearchBy.Mail AndAlso p.Profile.Mail.Contains(oDto.Value)) _
                                    OrElse (oDto.SearchFor = ProfileSearchBy.Name AndAlso p.Profile.Name.Contains(oDto.Value)) _
                                       OrElse (oDto.SearchFor = ProfileSearchBy.Surname AndAlso p.Profile.Surname.Contains(oDto.Value)) _
                                       OrElse (oDto.SearchFor = ProfileSearchBy.TaxCode AndAlso p.Profile.TaxCode.Contains(oDto.Value))) _
                                   ))
                        iCount = UserQuery.Count
                    ElseIf oDto.Status = ProfileStatus.Waiting Then
                        'OrElse (oDto.StartWith = "#" AndAlso oOtherChars.Contains(p.WaitingProfile.Surname.Chars(1).ToString)) 
                        Dim UserQuery = (From p In DC.GetCurrentSession.Linq(Of WaitingActivationPerson)() _
                                                                       Where (From pp In DC.GetCurrentSession.Linq(Of OrganizationProfiles)() Where ((oDto.OrganizationID = -1 AndAlso pp.isDefault = True) OrElse pp.OrganizationID = oDto.OrganizationID) AndAlso pp.isDefault AndAlso pp.Profile.Id = p.WaitingProfile.Id).Count > 0 _
                                                                       AndAlso (oDto.ProfileTypeID = -1 OrElse oDto.ProfileTypeID = p.WaitingProfile.TypeID) _
                                                                       AndAlso (oDto.AuthenticationTypeID = -1 OrElse (oDto.AuthenticationTypeID = -1 OrElse p.WaitingProfile.AuthenticationTypeID = oDto.AuthenticationTypeID)) _
                                                                       AndAlso (oDto.StartWith = "" OrElse p.WaitingProfile.Surname.StartsWith(oDto.StartWith, StringComparison.OrdinalIgnoreCase)) _
                                                                       AndAlso p.WaitingProfile.isDisabled _
                                                                           AndAlso (oDto.SearchFor = ProfileSearchBy.All OrElse oDto.Value = "" OrElse ( _
                               (oDto.SearchFor = ProfileSearchBy.Contains AndAlso (p.WaitingProfile.Name.Contains(oDto.Value) OrElse p.WaitingProfile.Surname.Contains(oDto.Value))) _
                               OrElse (oDto.SearchFor = ProfileSearchBy.Login AndAlso p.WaitingProfile.Login.Contains(oDto.Value)) _
                                  OrElse (oDto.SearchFor = ProfileSearchBy.Mail AndAlso p.WaitingProfile.Mail.Contains(oDto.Value)) _
                                    OrElse (oDto.SearchFor = ProfileSearchBy.Name AndAlso p.WaitingProfile.Name.Contains(oDto.Value)) _
                                       OrElse (oDto.SearchFor = ProfileSearchBy.Surname AndAlso p.WaitingProfile.Surname.Contains(oDto.Value)) _
                                       OrElse (oDto.SearchFor = ProfileSearchBy.TaxCode AndAlso p.WaitingProfile.TaxCode.Contains(oDto.Value))) _
                                   ))
                        iCount = UserQuery.Count
                    End If
                End If
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                Debug.Write(ex.ToString)
            End Try
            Return iCount
        End Function



        Public Function GetProfiles(ByVal oDto As dtoProfileFilters, ByVal oOrder As ProfileOrder, ByVal Ascending As Boolean, ByVal pagesize As Integer, ByVal pageIndex As Integer) As List(Of dtoProfile)
            Dim oList As List(Of dtoProfile)
            Try
                DC.BeginTransaction()

                Dim oWaiting As List(Of Integer)
                If oDto.Status = ProfileStatus.Waiting OrElse oDto.Status = ProfileStatus.All Then
                    oWaiting = (From p In DC.GetCurrentSession.Linq(Of WaitingActivationPerson)() Where p.WaitingProfile.isDisabled Select p.WaitingProfile.Id).ToList
                Else
                    oWaiting = New List(Of Integer)
                End If
                oList = Me.GetDefaultProfiles(oWaiting, oDto, pagesize, pageIndex)


                For Each o In (From op In oList Where oWaiting.Contains(op.Id)).ToList
                    o.StatusID = ProfileStatus.Waiting
                Next
                    DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                oList = New List(Of dtoProfile)
                Debug.Write(ex.ToString)
            End Try
            Return oList
        End Function

        Private Function GetDefaultProfiles(ByVal oWaiting As List(Of Integer), ByVal oDto As dtoProfileFilters, ByVal pagesize As Integer, ByVal pageIndex As Integer) As List(Of dtoProfile)
            Dim ByOrganizationID As Integer = oDto.OrganizationID
            Dim oChars As List(Of String) = Me.DefaultChars
            Dim oOtherChars As List(Of String) = Me.DefaultOtherChars
            '(oDto.StartWith = "#" AndAlso oOtherChars.Contains(p.Profile.Surname.Chars(1).ToString)) OrElse
            Dim UserQuery = (From p In DC.GetCurrentSession.Linq(Of OrganizationProfiles)() Where ((ByOrganizationID = -1 AndAlso p.isDefault = True) OrElse ByOrganizationID = p.OrganizationID) _
                                 AndAlso (oDto.ProfileTypeID = -1 OrElse oDto.ProfileTypeID = p.Profile.TypeID) _
                           AndAlso (oDto.AuthenticationTypeID = -1 OrElse p.Profile.AuthenticationTypeID = oDto.AuthenticationTypeID) _
 AndAlso ( _
                                                              (oDto.Status = ProfileStatus.All _
                                                               OrElse _
                                                              (oDto.Status = ProfileStatus.Active AndAlso p.Profile.isDisabled = False) _
                                                              OrElse _
                                                              (oDto.Status = ProfileStatus.Disabled AndAlso p.Profile.isDisabled AndAlso oWaiting.Contains(p.Profile.Id) = False) _
                                                              OrElse _
                                                              (oDto.Status = ProfileStatus.Waiting AndAlso oWaiting.Contains(p.Profile.Id)))) _
                                                                AndAlso (oDto.StartWith = "" OrElse p.Profile.Surname.StartsWith(oDto.StartWith, StringComparison.OrdinalIgnoreCase)) _
                             AndAlso (oDto.SearchFor = ProfileSearchBy.All OrElse oDto.Value = "" OrElse ( _
                     (oDto.SearchFor = ProfileSearchBy.Contains AndAlso (p.Profile.Name.Contains(oDto.Value) OrElse p.Profile.Surname.Contains(oDto.Value))) _
                     OrElse (oDto.SearchFor = ProfileSearchBy.Login AndAlso p.Profile.Login.Contains(oDto.Value)) _
                        OrElse (oDto.SearchFor = ProfileSearchBy.Mail AndAlso p.Profile.Mail.Contains(oDto.Value)) _
                          OrElse (oDto.SearchFor = ProfileSearchBy.Name AndAlso p.Profile.Name.Contains(oDto.Value)) _
                             OrElse (oDto.SearchFor = ProfileSearchBy.Surname AndAlso p.Profile.Surname.Contains(oDto.Value)) _
                             OrElse (oDto.SearchFor = ProfileSearchBy.TaxCode AndAlso p.Profile.TaxCode.Contains(oDto.Value))) _
                          ) Order By p.Profile.Surname Ascending Order By p.Profile.Name Select New dtoProfile(p.Profile.Id, p.Profile.Name, p.Profile.Surname, p.Profile.TypeID, p.Profile.Login, p.Profile.TaxCode, p.Profile.Mail, ProfileStatus.Active, p.Profile.AuthenticationTypeID, p.Profile.isDisabled, p.isDefault, p.OrganizationID))

            Return UserQuery.Skip(pagesize * pageIndex).Take(pagesize).ToList
        End Function
        Private Function DefaultChars() As List(Of String)
            Dim iChars As New List(Of String)
            For i = 48 To 57
                iChars.Add(Chr(i))
            Next
            ' maiuscole
            For i = 65 To 90
                iChars.Add(Chr(i))
            Next
            ' minuscole
            For i = 97 To 122
                iChars.Add(Chr(i))
            Next
            Return iChars
        End Function
        Private Function DefaultOtherChars() As List(Of String)
            Dim iChars As New List(Of String)
            For i = 32 To 47
                iChars.Add(Chr(i))
            Next
            For i = 58 To 64
                iChars.Add(Chr(i))
            Next
            For i = 91 To 96
                iChars.Add(Chr(i))
            Next
            For i = 123 To 126
                iChars.Add(Chr(i))
            Next
            Return iChars
        End Function

        Public Sub ChangePassword(ByVal PersonID As Integer)
            Try
                DC.BeginTransaction()
                Dim oPerson As Person = Me.DC.GetCurrentSession.Get(Of Person)(PersonID)
                If Not IsNothing(oPerson) Then
                    Dim oPass As New COL_BusinessLogic_v2.PasswordGenerator

                    oPerson.Password = oPass.GenerateEncrypted
                    Me.DC.SaveOrUpdate(oPerson)
                End If

                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
        End Sub

        Public Function GetProfile(ByVal PersonID As Integer) As Person
            Dim oPerson As Person = Nothing
            Try
                DC.BeginTransaction()
                oPerson = DC.GetCurrentSession.Get(Of Person)(PersonID)
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                Debug.Write(ex.ToString)
            End Try
            Return oPerson
        End Function
    End Class
End Namespace