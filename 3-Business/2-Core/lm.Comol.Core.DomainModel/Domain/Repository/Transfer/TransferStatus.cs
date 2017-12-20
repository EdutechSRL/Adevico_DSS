using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Repository
{
    public enum TransferStatus
    {
        #region "Multimedia related errors -20 -> -29"
        Multimedia_AnalyzeError = -20,        
        #endregion
        #region "Scorm related errors -10 -> -19"
        Scorm_AnalyzeError = -10,        
        #endregion
        FileTypeError = -6,
        UnableToDeleteAfterUnzip = -5,
        UnzipFileNotFound = -4,
        UploadFileNotFound = -3,
        UnableToUnzip = -2,        
        Error = -1,
        ReadyForTransfer = 0,
        Copying = 1,
        ReadyToUnzip = 2,
        Unzipping = 3,
        Unzipped = 4,
        ReadyToDelete = 5,
        Completed = 6,
        Deleting = 7,
        Deleted = 8,
        ReadyToAnalyze = 9,
        Analyzed = 10
    }
}