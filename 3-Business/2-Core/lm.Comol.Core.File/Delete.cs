using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using System.Collections;

namespace lm.Comol.Core.File
{
    public class Delete
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path">Complete path with filename</param>
        /// <returns>Deleted=ok, ImpersonationFailed, FileDoesntExist,  None = catch </returns>
        public static FileMessage File_FM(string Path)
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
                    if (System.IO.File.Exists(Path))
                    {
                        System.IO.File.Delete(Path);
                        return FileMessage.Deleted;
                    }
                    else return FileMessage.FileDoesntExist;
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

        public static Boolean File(string Path)
        {
            FileMessage FM = File_FM(Path);
            return ((FM == FileMessage.Deleted) || (FM == FileMessage.FileDoesntExist));
        }

        /// <summary>
        /// if the deletion of a file fails, deletion of other files continue, but the return value will be "false"
        /// </summary>
        /// <param name="Path"></param>
        /// <returns>"false" if impersonation fails, "catch" or a single path delete fails, true in EVERY other case. </returns>
        public static Boolean Files(List<string> Path)
        {
            if (Path.Count > 0)
            {
                Impersonate oImpersonate = new Impersonate(); Boolean wasImpersonated = Impersonate.isImpersonated();
                try
                {
                    Boolean retVal = true;
                    if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                    {
                        return false;
                    }
                    else
                    {
                        foreach (string filePath in Path)
                        {
                            retVal = Delete.File(filePath) && retVal;
                        }
                    }
                    return retVal;
                }
                catch
                {
                    if (!wasImpersonated) { oImpersonate.UndoImpersonation(); }
                    return false;
                }
                finally
                {
                    if (!wasImpersonated) { oImpersonate.UndoImpersonation(); }
                }
            }
            else return true;
        }
        public static Boolean Files(IList<string> IPath)
        {
            return Files(IPath.ToList());
        }

        /// <summary>
        /// if the deletion of a file fails, deletion of other files continue, but the return value will be "false"
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Pre">Added before every path</param>
        /// <param name="Post">Added after every path</param>
        /// <returns>"false" if impersonation fails, "catch" or a single path delete fails, true in EVERY other case. </returns>
        public static Boolean Files(string Pre, List<string> Path, string Post)
        {
            if (Path.Count > 0)
            {
                Impersonate oImpersonate = new Impersonate(); Boolean wasImpersonated = Impersonate.isImpersonated();
                try
                {
                    Boolean retVal = true;
                    if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                    {
                        return false;
                    }
                    else
                    {
                        foreach (string filePath in Path)
                        {
                            string file = Pre + filePath + Post;
                            retVal = Delete.File(file) && retVal;
                        }
                    }
                    return retVal;
                }
                catch
                {
                    if (!wasImpersonated) { oImpersonate.UndoImpersonation(); }
                    return false;
                }
                finally
                {
                    if (!wasImpersonated) { oImpersonate.UndoImpersonation(); }
                }
            }
            else return true;
        }
        /// <summary>
        /// if the deletion of a file fails, deletion of other files continue, but the return value will be "false"
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Pre">Added before every path</param>
        /// <param name="Post">Added after every path</param>
        /// <returns>"false" if impersonation fails, "catch" or a single path delete fails, true in EVERY other case. </returns>
        public static Boolean Files(string Pre, List<Guid> Path, string Post)
        {
            if (Path.Count > 0)
            {
                Impersonate oImpersonate = new Impersonate(); Boolean wasImpersonated = Impersonate.isImpersonated();
                try
                {
                    Boolean retVal = true;
                    if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                    {
                        return false;
                    }
                    else
                    {
                        foreach (Guid filePath in Path)
                        {
                            string file = Pre + filePath.ToString() + Post;
                            retVal = Delete.File(file) && retVal;
                        }
                    }
                    return retVal;
                }
                catch
                {
                    if (!wasImpersonated) { oImpersonate.UndoImpersonation(); }
                    return false;
                }
                finally
                {
                    if (!wasImpersonated) { oImpersonate.UndoImpersonation(); }
                }
            }
            else return true;
        }
        /// <summary>
        /// if the deletion of a file fails, deletion of other files continue, but the return value will be "false"
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Pre">Added before every path</param>
        /// <param name="Post">Added after every path</param>
        /// <returns>"false" if impersonation fails, "catch" or a single path delete fails, true in EVERY other case. </returns>
        public static Boolean Files(string Pre, ArrayList Path, string Post)
        {
            if (Path.Count > 0)
            {
                Impersonate oImpersonate = new Impersonate(); Boolean wasImpersonated = Impersonate.isImpersonated();
                try
                {
                    Boolean retVal = true;
                    if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                    {
                        return false;
                    }
                    else
                    {
                        foreach (Guid filePath in Path)
                        {
                            string file = Pre + filePath.ToString() + Post;
                            retVal = Delete.File(file) && retVal;
                        }
                    }
                    return retVal;
                }
                catch
                {
                    if (!wasImpersonated) { oImpersonate.UndoImpersonation(); }
                    return false;
                }
                finally
                {
                    if (!wasImpersonated) { oImpersonate.UndoImpersonation(); }
                }
            }
            else return true;
        }

        #region Cancellare

        //public static Boolean isFiles(List<string> path)
        //{ return Files(path); }
        //public static Boolean isFiles(IList<string> path)
        //{ return Files(path); }
        //public static Boolean isFiles(string Pre, ArrayList Path, string Post)
        //{ return Files(Pre, Path, Post); }
        //public static Boolean isFiles(string Pre, List<Guid> Path, string Post)
        //{ return Files(Pre, Path, Post); }
        //public static Boolean isFiles(string Pre, List<string> Path, string Post)
        //{ return Files(Pre, Path, Post); }
        //public static Boolean isFile(string Path)
        //{ return File(Path); }
        #endregion

        public static FileMessage Directory(String path, Boolean deleteContent)
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
                        System.IO.Directory.Delete(path, deleteContent);
                        return FileMessage.Deleted;
                    }
                    else return FileMessage.DirectoryDoesntExist;
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
    }
}
