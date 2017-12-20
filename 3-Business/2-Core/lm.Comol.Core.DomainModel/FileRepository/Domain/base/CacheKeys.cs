using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    public static class CacheKeys
    {
        public static String AllRepositoryKeys
        {
            get { return "FileRepository_"; }
        }

        public static String RepositoryIdentifierKey(RepositoryType type, Int32 idCommunity = -1)
        {
            return type.ToString() + "_" + (idCommunity >= 0 ? idCommunity.ToString() : "-") ;
        }
        public static String Repository(RepositoryIdentifier identifier)
        {
            return Repository(identifier.Type, identifier.IdCommunity);
        }
        public static String Repository(RepositoryType type,Int32 idCommunity)
        {
            return AllRepositoryKeys + RepositoryIdentifierKey(type, idCommunity);
        }
        public static String UsersViewOfRepository(RepositoryIdentifier identifier)
        {
            return UsersViewOfRepository(identifier.Type, identifier.IdCommunity);
        }
        public static String UsersViewOfRepository(RepositoryType type, Int32 idCommunity)
        {
            return Repository(RepositoryIdentifier.Create(type, idCommunity)) + "_UserView_";
        }
        public static String UserViewOfRepository(Int32 idCurrentUser, RepositoryIdentifier identifier, Boolean onlyFolder = false)
        {
            return UsersViewOfRepository(identifier) + idCurrentUser.ToString() + (onlyFolder ? "_Folders" : "");
        }
        public static String UserViewOfPartialRepository(Int32 idCurrentUser, RepositoryIdentifier identifier, Boolean onlyFolder = false)
        {
            return UsersViewOfRepository(identifier) + idCurrentUser.ToString() + "_partial" + (onlyFolder ? "_Folders" : "");
        }
        public static String UserViewOfRepository(Int32 idCurrentUser, RepositoryType type, Int32 idCommunity, Boolean onlyFolder = false)
        {
            return UserViewOfRepository(idCurrentUser, RepositoryIdentifier.Create(type, idCommunity), onlyFolder);
        }

        public static String UsersSizeViewOfRepository(RepositoryIdentifier identifier)
        {
            return UsersSizeViewOfRepository(identifier.Type, identifier.IdCommunity);
        }
        public static String UsersSizeViewOfRepository(RepositoryType type, Int32 idCommunity)
        {
            return Repository(type, idCommunity) + "_UserSize_";
        }
        public static String UserSizeOfRepository(Int32 idCurrentUser, RepositoryType type, Int32 idCommunity)
        {
            return UsersSizeViewOfRepository(type, idCommunity) + idCurrentUser.ToString();
        }
    }
}