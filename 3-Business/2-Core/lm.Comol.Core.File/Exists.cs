using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.File
{
    public class Exists
    {
        public static FileMessage Directory_FM(string path)
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
                    if (System.IO.Directory.Exists(path))
                    {
                        return FileMessage.DirectoryExist;
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
        public static Boolean Directory(string path)
        {
            return (Directory_FM(path) == FileMessage.DirectoryExist);
        }
        public static Boolean Directory(DirectoryInfo dirInfo)
        {
            return Directory(dirInfo.ToString());
        }
        /// <summary>
        /// Come File(), ma restituisce un enum con più informazioni
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileMessage File_FM(String path)
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
                    if (System.IO.File.Exists(path))
                    {
                        return FileMessage.FileExist;
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
        public static Boolean File(String path)
        {
            return (File_FM(path) == FileMessage.FileExist);
        }
    }
}