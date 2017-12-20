using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tiles.Business
{
    public class TileException : Exception
    {
        public ErrorMessageType ErrorType { get; set; }
        public TileException()
        {
        }
        public TileException(ErrorMessageType errorType)
        {
            ErrorType = errorType;
        }
        public TileException(ErrorMessageType errorType, Exception inner, String message = "")
            : base(message, inner)
        {
            ErrorType = errorType;
        }
    }

    public enum ErrorMessageType
    {
        None = 0,
        DefaultTranslationMissing = 1,
        NoPermissionToEdit = 2,
        NoPermissionToAddToSystem = 3,
        NoPermissionToAddToCommunity = 4,
        NoPermissionToAddToCommunities = 5,
        SavingTranslations = 6,
        SavingTileAssignments = 7,
        SavingTile = 8,
        DefaultTranslationDuplicate = 9,
        GenericError = 10,
        SessionTimeout = 11,
        UnavailableTile = 12,
        MissingTags = 13,
        SavingActionItems = 14,
        SavingDashboardAssignments = 15,
        SavingTagTranslations = 16,
        SavingTagLinks = 17
    }
}