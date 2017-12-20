using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tags.Business
{
    public class TagException : Exception
    {
        public lm.Comol.Core.DomainModel.Domain.JsonError<lm.Comol.Core.Tag.Domain.dtoTag> Error { get; set; }
        public ErrorMessageType ErrorType { get; set; }
        public TagException()
        {
        }
        public TagException(ErrorMessageType errorType, lm.Comol.Core.Tag.Domain.dtoTag obj = null )
        {
            ErrorType = errorType;
            Error = new DomainModel.Domain.JsonError<Tag.Domain.dtoTag>() { ReturnObject = obj };
        }
        public TagException(ErrorMessageType errorType, Exception inner, lm.Comol.Core.Tag.Domain.dtoTag obj = null ,String message="")
            : base(message, inner)
        {
            ErrorType = errorType;
            Error = new DomainModel.Domain.JsonError<Tag.Domain.dtoTag>() { ReturnObject = obj };
        }
    }

    public enum ErrorMessageType
    {
        None = 0,
        DefaultTranslationMissing = 1,
        NoPermissionToEdit = 2,
        NoPermissionToAddToSystem = 3,
        NoPermissionToAddToOrganization = 4,
        SavingTranslations = 5,
        SavingOrganizationAssignments = 6,
        SavingTag = 7,
        SavingTileTranslations = 8,
        SavingTile = 9,
        DefaultTranslationDuplicate = 10,
        MissingIdOrganization = 11,
        GenericError = 12,
        SessionTimeout = 13,
        MissingTag = 14,
        SavingCommunityTypes = 15,
    }
}