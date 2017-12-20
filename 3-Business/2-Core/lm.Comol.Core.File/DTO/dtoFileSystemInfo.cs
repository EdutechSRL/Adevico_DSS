using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.File
{    
    [Serializable]
    public class dtoFileSystemInfo
    {
        //
        // Summary:
        //     Gets or sets the creation time of the current System.IO.FileSystemInfo object.
        //
        // Returns:
        //     The creation date and time of the current System.IO.FileSystemInfo object.
        //
        // Exceptions:
        //   System.IO.IOException:
        //     System.IO.FileSystemInfo.Refresh() cannot initialize the data.
        //
        //   System.IO.DirectoryNotFoundException:
        //     The specified path is invalid, such as being on an unmapped drive.
        //
        //   System.PlatformNotSupportedException:
        //     The current operating system is not Microsoft Windows NT or later.
        public virtual DateTime CreationTime { get; set; }
        //
        // Summary:
        //     Gets the string representing the extension part of the file.
        //
        // Returns:
        //     A string containing the System.IO.FileSystemInfo extension.
        public virtual string Extension { get; set; }
        //
        // Summary:
        //     Gets the full path of the directory or file.
        //
        // Returns:
        //     A string containing the full path.
        //
        // Exceptions:
        //   System.Security.SecurityException:
        //     The caller does not have the required permission.
        public virtual string FullName { get; set; }
        public virtual string Name { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether a file exists.
        //
        // Returns:
        //     true if the file exists; false if the file does not exist or if the file
        //     is a directory.
        public virtual bool Exists { get; set; }
        //
        // Summary:
        //     Gets or sets a value that determines if the current file is read only.
        //
        // Returns:
        //     true if the current file is read only; otherwise, false.
        //
        // Exceptions:
        //   System.IO.FileNotFoundException:
        //     The file described by the current System.IO.FileInfo object could not be
        //     found.
        //
        //   System.IO.IOException:
        //     An I/O error occurred while opening the file.
        //
        //   System.UnauthorizedAccessException:
        //     The file described by the current System.IO.FileInfo object is read-only.
        //      -or- This operation is not supported on the current platform.  -or- The
        //     caller does not have the required permission.
        public bool IsReadOnly { get; set; }
        //
        // Summary:
        //     Gets the size, in bytes, of the current file.
        //
        // Returns:
        //     The size of the current file in bytes.
        //
        // Exceptions:
        //   System.IO.IOException:
        //     System.IO.FileSystemInfo.Refresh() cannot update the state of the file or
        //     directory.
        //
        //   System.IO.FileNotFoundException:
        //     The file does not exist.  -or- The Length property is called for a directory.
        public long Length { get; set; } 
    }
}
