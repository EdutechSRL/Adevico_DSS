using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Telerik.WebControls;

namespace lm.Comol.Core.File
{
    public class Create
    {
        public static FileMessage Directory_FM(String path)
        {
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated && (oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed))
                {
                    return FileMessage.ImpersonationFailed;
                }
                else
                {
                    if (Exists.Directory(path))
                    {
                        return FileMessage.DirectoryExist;
                    }
                    else
                    {
                        String ShareName = string.Empty;
                        if (path.Contains("/"))
                        {
                            path = path.Replace(@"/", @"\");
                        }
                        if (path.StartsWith("\\"))
                        {
                            ShareName = path.Substring(0, path.IndexOf(@"\", 2));
                            path = path.Replace(ShareName, "");
                        }
                        if (path == "")
                        {
                            return FileMessage.DirectoryExist;
                        }
                        else
                        {
                            String TempPath = ShareName;
                            foreach (string oDirectoryPath in path.Split(System.IO.Path.PathSeparator)) //sarebbe meglio aggiungendo il parametro StringSplitOptions.RemoveEmptyEntries e togliendo l'if seguente, ma il compilatore ha deciso di non accettarlo.
                            {
                                TempPath += oDirectoryPath + @"\";
                                if (!Exists.Directory(TempPath))
                                {
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(TempPath);
                                        return FileMessage.FolderCreated;
                                    }
                                    catch
                                    {
                                        return FileMessage.NotCreated;
                                    }
                                }
                                else
                                { return FileMessage.DirectoryExist; }
                            }
                        }
                    }
                    return FileMessage.None;
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
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns>true if directory has been created or already exists</returns>
        public static Boolean Directory(String path)
        {
            FileMessage value = Directory_FM(path);
            if (value == FileMessage.FolderCreated || value == FileMessage.DirectoryExist)
            { return true; }
            else return false;
        }
        /// <summary>
        /// "overwrite" is stronger than "append"
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <param name="overwrite"></param>
        /// <param name="append">used only if overwrite is set to false</param>
        /// <returns></returns>
        public static FileMessage TextFile(String path, String content, Boolean overwrite, Boolean append)
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
                    if (overwrite || !Exists.File(path))
                    {
                        try
                        {
                            System.IO.File.WriteAllText(path, content);
                            return FileMessage.FileCreated;
                        }
                        catch
                        { return FileMessage.Catch; }
                    }
                    else
                        if (append)
                        {
                            try
                            {
                                System.IO.File.AppendAllText(path, content);
                                return FileMessage.FileCreated;
                            }
                            catch
                            { return FileMessage.Catch; }
                        }
                    return FileMessage.FileExist;
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
        /// if destination exists, files will not be overwritten. If destination path doesn't exist, copy fails
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static Boolean CopyFile(String source, String destination)
        {
            return (CopyFile_FM(source, destination) == FileMessage.FileCreated);
        }


        /// <summary>
        /// if destination exists, files will not be overwritten
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static FileMessage CopyFile_FM(String source, String destination)
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
                    string destinationPath = destination.Substring(0, destination.Length-Helper.StringReverse(destination).IndexOf(@"\"));
                    Directory(destinationPath);
                    System.IO.File.Copy(source, destination);
                    return FileMessage.FileCreated;
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
        public static FileMessage UploadFile(ref System.Web.UI.HtmlControls.HtmlInputFile file, String filePath)
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
                    file.PostedFile.SaveAs(filePath);
                    return FileMessage.FileCreated;
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
        public static FileMessage UploadFile(ref System.Web.HttpPostedFile file, String filePath)
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
                    file.SaveAs(filePath);
                    return FileMessage.FileCreated;
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
        public static FileMessage UploadFile(ref Telerik.Web.UI.UploadedFile file,  String filePath)
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
                    file.SaveAs(filePath);
                    return FileMessage.FileCreated;
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

        public static FileMessage FromStream(System.IO.Stream stream, String fileName)
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
                    using (System.IO.Stream fileStream = System.IO.File.Create(fileName))
                    {
                        for (int a = stream.ReadByte(); a != -1; a = stream.ReadByte())
                            fileStream.WriteByte((byte)a);

                    }

                    return FileMessage.FileCreated;
                }
            }
            catch (Exception ex)
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

        public static System.IO.FileStream FileStream( String fileName)
        {
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                {
                    return null;
                }
                else
                {
                    return new System.IO.FileStream(fileName, System.IO.FileMode.Create);
                }
            }
            catch (Exception ex)
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
    
        //public static FileMessage UploadFile(ref  Telerik.Web.UI.UploadedFile file, String filePath)
        //{
        //    Impersonate oImpersonate = new Impersonate(); Boolean wasImpersonated = oImpersonate.isImpersonated();
        //    try
        //    {
        //         if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
        //        {
        //            return FileMessage.ImpersonationFailed;
        //        }
        //        else
        //        {
        //            file.SaveAs(filePath);
        //            return FileMessage.FileCreated;
        //        }
        //    }
        //    catch
        //    {
        //        if (!wasImpersonated)
        //        return FileMessage.Catch;
        //    }
        //    finally
        //    {
        //        if (!wasImpersonated) { oImpersonate.UndoImpersonation(); }
        //    }
        //}
        public static FileMessage Image(System.Drawing.Bitmap bmp, String path, System.Drawing.Imaging.ImageFormat imageFormat)
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
                    bmp.Save(path, imageFormat);
                    return FileMessage.FileCreated;
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

        public static FileMessage Image( System.Drawing.Image image, String path, System.Drawing.Imaging.ImageFormat imageFormat)
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
                    image.Save(path, imageFormat);
                    return FileMessage.FileCreated;
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
