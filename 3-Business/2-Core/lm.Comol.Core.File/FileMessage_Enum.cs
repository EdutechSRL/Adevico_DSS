using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.File
{
    public enum FileMessage
    {
        None = 0,		// nessun errore
        FileExist = -1,			// file già inserito
        NotUploaded = -2,		// mancato upload
        ZeroByte = -3,		// lunghezza file nulla
        NotFound = -4,	// file non trovato
        MismatchType = -5, // tipo 
        NotDeleted = -6,
        NotCreated = -7,
        EditError = -8,
        UploadError = -9,
        DirectoryExist = -10,
        DirectoryDoesntExist = -11,
        FileDoesntExist = -12,
        NoPermission = -13,
        NoItemSpecified = -14,
        ImpersonationFailed = -15,
        NoImpersonationRequired = -16,
        NoZipFile = -17,
        Catch = -1000,

        FolderCreated = 1,
        FileCreated = 2,
        ChangeSaved = 3,
        Impersonated = 4,
        Deleted = 5,
        Read = 6,
        InvalidFileName = -18,
        InvalidPath = -19
    }
}