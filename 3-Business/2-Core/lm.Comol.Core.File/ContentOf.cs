using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using System.IO;
using System.Drawing;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.File
{
    public class ContentOf
    {
        //        public Int32  FileList(String path, List<String> arrFiles ) 
        //        {
        //            String[] fileList;
        //            String fileNameOnly;
        //            Int32 result, FileNameLength;
        //            List<String> tempArray = new List<String>();
        //Int32 lung;

        //            if (System.IO.Directory.Exists(path))
        //            {
        //              fileList = System.IO.Directory.GetFiles(path);
        //                lung = fileList.Length; //numero file directory
        //                foreach (String fileName in fileList)
        //                {
        //                    FileNameLength =  (1, StrReverse(fileName), "\");
        //                    fileNameOnly = Mid(fileName, (Len(fileName) - FileNameLength) + 2);
        //                    tempArray.Add(fileNameOnly);
        //                }
        //                result = lung;
        //                arrFiles = tempArray;
        //            } 
        //            else
        //            { n_errore = Errore_File.dirNotFound
        //        }

        //            return result
        //        }

        public static FileMessage Directory(String path, ref String[] files, Boolean absolutePath, Boolean orderByName = true)
        {
            Impersonate oImpersonate = new Impersonate(); Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                {
                    return FileMessage.ImpersonationFailed;
                }
                else
                {
                    if (System.IO.Directory.Exists(path))
                    {
                        files = System.IO.Directory.GetFiles(path);
                        if (absolutePath)
                        {
                            return FileMessage.Read;
                        }
                        else
                        {

                            for (Int32 i = 0; i < files.Length; i++)
                            {
                                Int32 fileNameLength = Helper.StringReverse(files[i]).IndexOf(@"\", 1);
                                files[i] = files[i].Substring(files[i].Length - fileNameLength);
                            }
                            return FileMessage.Read;
                        }
                    }
                    else { return FileMessage.DirectoryDoesntExist; }
                }
            }
            catch
            {
                if (!wasImpersonated) { oImpersonate.UndoImpersonation(); }
                return FileMessage.Catch;
            }
            finally
            {
                if (!wasImpersonated) { oImpersonate.UndoImpersonation(); }
            }
        }
        public static FileMessage Directory(String path, ref Int32 fileNumber, ref String[] files, Boolean absolutePath)
        {
            FileMessage fMSG = Directory(path, ref files, absolutePath);
            fileNumber = files == null ? 0 : files.Length;
            return fMSG;
        }
        public static FileMessage Directory(String path, ref String[] files, String searchPattern, Boolean absolutePath)
        {
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                {
                    files = null;
                    return FileMessage.ImpersonationFailed;
                }
                else
                {
                    if (System.IO.Directory.Exists(path))
                    {
                        files = System.IO.Directory.GetFiles(path, searchPattern);
                        if (absolutePath)
                        {
                            return FileMessage.Read;
                        }
                        else
                        {

                            for (Int32 i = 0; i < files.Length; i++)
                            {
                                Int32 fileNameLength = Helper.StringReverse(files[i]).IndexOf(@"\", 1);
                                files[i] = files[i].Substring(files[i].Length - fileNameLength);
                            }
                            return FileMessage.Read;
                        }
                    }
                    else
                    {
                        files = null;
                        return FileMessage.DirectoryDoesntExist;
                    }
                }
            }
            catch
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
                return FileMessage.Catch;
            }
            finally
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
            }
        }
        public static FileMessage Directory(String path, ref List<dtoFileSystemInfo> files, String searchPattern)
        {
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                    return FileMessage.ImpersonationFailed;
                else
                {
                    if (System.IO.Directory.Exists(path))
                    {
                        string[] fileNames = System.IO.Directory.GetFiles(path, searchPattern);
                        if (files == null)
                            files = new List<dtoFileSystemInfo>();
                        for (Int32 i = 0; i < fileNames.Length; i++)
                        {
                            try
                            {
                                FileInfo info = new System.IO.FileInfo(fileNames[i]);
                                if (info != null)
                                    files.Add(new dtoFileSystemInfo() { CreationTime = info.CreationTime, Exists = info.Exists, Extension = info.Extension, FullName = info.FullName, IsReadOnly = info.IsReadOnly, Length = info.Length, Name = info.Name });

                            }
                            catch (Exception ex)
                            {
                                return FileMessage.Catch;
                            }
                        }
                        return FileMessage.Read;
                    }
                    else
                        return FileMessage.DirectoryDoesntExist;
                }
            }
            catch
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
                return FileMessage.Catch;
            }
            finally
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
            }
        }

        /// <summary>
        /// if something fails, returns -1
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recursive">Calculate Size of Subdirectories or not</param>
        /// <returns>-1 when impersonation fails, or catch</returns>
        public static Int64 Directory_Size(String path, Boolean recursive)
        {
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                { return -1; }
                else
                {
                    Int64 size = 0;
                    DirectoryInfo dirInfo = new DirectoryInfo(path);
                    if (Exists.Directory(dirInfo))
                    {
                        FileInfo[] fileInfos = dirInfo.GetFiles();
                        foreach (FileInfo file in fileInfos)
                        {
                            size += file.Length;
                        }
                        if (recursive)
                        {
                            DirectoryInfo[] directories = dirInfo.GetDirectories();
                            foreach (DirectoryInfo directory in directories)
                            {
                                size += Directory_Size(dirInfo.ToString() + directory.ToString() + "\\", true);
                            }
                        }
                        return (size);
                    }
                    else
                        return -1;
                }
            }
            catch
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
                return -1;
            }
            finally
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
            }
            return -1;
        }


        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
        out ulong lpFreeBytesAvailable,
        out ulong lpTotalNumberOfBytes,
        out ulong lpTotalNumberOfFreeBytes);

        public static bool DriveFreeBytes(string folderName, out ulong freespace)
        {
            freespace = 0;
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                { return false; }
                else
                {
                    
                    if (string.IsNullOrEmpty(folderName))
                        throw new ArgumentNullException("folderName");

                    if (!folderName.EndsWith("\\"))
                        folderName += '\\';

                    ulong free = 0, dummy1 = 0, dummy2 = 0;

                    if (GetDiskFreeSpaceEx(folderName, out free, out dummy1, out dummy2))
                    {
                        freespace = free;
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
                return false;
            }
            finally
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
            }
            return false;
        }

        /// <summary>
        /// If something fails, it returns true
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Boolean Directory_isEmpty(String path)
        {
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();

            try
            {
                if (!wasImpersonated)
                { oImpersonate.ImpersonateValidUser(); }
                if (Exists.Directory(path))
                {
                    DirectoryInfo directory = new DirectoryInfo(path);
                    return (directory.GetFiles().Count() == 0 && directory.GetDirectories().Count() == 0);
                }
                else
                    return true;
            }
            catch
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
                return true;
            }
            finally
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
            }
        }
        /// <summary>
        /// Unused-Untested after impersonation implementation
        /// </summary>
        /// <param name="fullname">File name</param>
        /// <param name="lines">number of lines to retrieve</param>
        /// <returns></returns>
        public static CsvFile LoadCsvFile(String fullname, dtoCsvSettings settings)
        {
            return LoadCsvFile(fullname, settings, 0);
        }
        /// <summary>
        /// Unused-Untested after impersonation implementation
        /// </summary>
        /// <param name="fullname">File name</param>
        /// <param name="lines">number of lines to retrieve</param>
        /// <returns></returns>
        public static CsvFile LoadCsvFile(String fullname, dtoCsvSettings settings, int lines)
        {
            CsvFile result = null;
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated)
                { oImpersonate.ImpersonateValidUser(); }

                CsvFileReader reader = new CsvFileReader();
                result = reader.ReadFile(fullname, settings, lines);
            }
            catch
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
                result = null;
            }
            finally
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
            }
            return result;
        }


        public static dtoFileSystemInfo File_dtoInfo(String path)
        {
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated)
                { oImpersonate.ImpersonateValidUser(); }
                FileInfo info = new System.IO.FileInfo(path); //viene creato anche se non ha accesso al file, o se il file non esiste
                return new dtoFileSystemInfo() { CreationTime = info.CreationTime, Exists = info.Exists, Extension = info.Extension, FullName = info.FullName, IsReadOnly = info.IsReadOnly, Length = info.Length, Name = info.Name };
            }
            catch
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
                return null;
            }
            finally
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
            }
        }
        public static FileInfo File_Info(String path)
        {
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated)
                { oImpersonate.ImpersonateValidUser(); }
                return new System.IO.FileInfo(path); //viene creato anche se non ha accesso al file, o se il file non esiste
            }
            catch
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
                return null; //new System.IO.FileInfo(path);
            }
            finally
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
            }
        }
        public static Int64 File_Size(String path)
        {
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                { return -1; }
                else
                {
                    return new FileInfo(path).Length;
                }
            }
            catch
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
                return -1;
            }
            finally
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
            }
        }

        /// <summary>
        /// In case of error, returns DateTime.MinValue
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DateTime File_LastWrite(String path)
        {
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                { return DateTime.MinValue; }
                else
                { return System.IO.File.GetLastWriteTime(path); }
            }
            catch
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
                return DateTime.MinValue;
            }
            finally
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
            }

        }
        public static Byte[] File_ToByteArray(String path)
        {
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                {
                    return new Byte[0];
                }
                else
                {
                    byte[] buffer;
                    FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    try
                    {
                        int length = (int)fileStream.Length;  // get file length, converted from long to int
                        buffer = new byte[length];            // create buffer
                        int count;                            // actual number of bytes read
                        int sum = 0;                          // total number of bytes read

                        // read until Read method returns 0 (end of the stream has been reached)
                        while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                            sum += count;  // sum is a buffer offset for next reading
                    }
                    finally
                    {
                        fileStream.Close();
                    }
                    return buffer;
                }
            }
            catch
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
                return new Byte[0];
            }
            finally
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
            }
        }


        public static String TextFile(String filename)
        {
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated)
                { oImpersonate.ImpersonateValidUser(); }
                if (Exists.File(filename))
                {
                    return System.IO.File.ReadAllText(filename);
                }
                else return String.Empty;
            }
            catch
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
                return String.Empty;
            }
            finally
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
            }
        }
        public static FileMessage ImageSize(String path, ref Int32 height, ref Int32 width)
        {
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                {
                    return FileMessage.ImpersonationFailed;
                }
                else
                {
                    FileMessage exists = Exists.File_FM(path);
                    if (exists == FileMessage.FileExist){
                         using (FileStream fs = new FileStream(path,FileMode.Open, FileAccess.Read)){
                            System.Drawing.Image objImage = new System.Drawing.Bitmap(System.Drawing.Image.FromStream(fs));
                            width = objImage.Width;
                            height = objImage.Height;
                            objImage.Dispose();
                            fs.Close();
                        }
                        return FileMessage.FileExist;
                    }
                    else return exists;
                }
            }
            catch
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
                return FileMessage.Catch;
            }
            finally
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
            }
        }
        /// <summary>
        /// The BMP stream is loaded in mbpStream parameter
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bmpStream"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap ImageStream(String path, ref FileMessage fMessage)
        {
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                {
                    fMessage = FileMessage.ImpersonationFailed;
                    return null;
                }
                else
                {
                    FileMessage exists = Exists.File_FM(path);
                    if (exists == FileMessage.FileExist)
                    {
                        System.Drawing.Bitmap bmpStream = null;
                        using (FileStream fs = new FileStream(path,FileMode.Open, FileAccess.Read)){
                            bmpStream = new System.Drawing.Bitmap(System.Drawing.Image.FromStream(fs));
                            fMessage = FileMessage.Read;
                            fs.Close();
                        }

                      
                        return bmpStream;
                    }
                    else
                    {
                        fMessage = exists;
                        return null;
                    }
                }
            }
            catch
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
                fMessage = FileMessage.Catch;
                return null;
            }
            finally
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
            }
        }

    }
}


