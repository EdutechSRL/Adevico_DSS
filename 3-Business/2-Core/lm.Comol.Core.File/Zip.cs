using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;

namespace lm.Comol.Core.File
{
    public class Zip
    {
        //public static FileMessage UnzipFile(String filename, String destinationDir, List<String> acceptedExt)
        //{
        //    FileMessage result = FileMessage.None;
        //    FastZip zipfile = null;
        //    try
        //    {
        //        zipfile = new FastZip();
        //        zipfile.ExtractZip(filename,destinationDir, "");
        //    }
        //    catch (Exception ex)
        //    {

        //        SaveFileStatus(currentFile, FileStatusEnumNew.UnableToUnzip);
        //        OnWriteEntry(ex.ToString(), EventLogEntryType.Error, 0);
        //    }
        //    finally
        //    {

        //        SaveFileStatus(currentFile, FileStatusEnumNew.Unzipped);

        //        if (currentApp.DeleteAfterDecompress && System.IO.File.Exists(filename))
        //        {
        //            try
        //            {

        //                SaveFileStatus(currentFile, FileStatusEnumNew.ReadyToDelete);

        //                System.IO.File.Delete(filename);

        //                SaveFileStatus(currentFile, FileStatusEnumNew.Deleted);
        //            }
        //            catch (Exception ex)
        //            {
        //                OnWriteEntry(ex.ToString(), EventLogEntryType.Error, 0);

        //                SaveFileStatus(currentFile, FileStatusEnumNew.Error);
        //            }
        //        }
        //    }

        //    return Result;
        //}
        public static FileMessage ZipFilesRename(List<dtoFileToZip> zipFiles, String OutputFile)
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
                    using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(OutputFile)))
                    {
                        s.SetLevel(9); // 0-9, 9 being the highest compression

                        byte[] buffer = new byte[4096];

                        foreach (dtoFileToZip file in zipFiles)
                        {

                            //String newFilename = Path.GetFileName(file.Replace(".dll", "._dll"));

                            String newFilename = file.FileName;
                            ZipEntry entry = new ZipEntry(newFilename);

                            entry.DateTime = DateTime.Now;

                            s.PutNextEntry(entry);

                            using (System.IO.FileStream fs = System.IO.File.OpenRead(file.StoredName))
                            {

                                int sourceBytes;

                                do
                                {

                                    sourceBytes = fs.Read(buffer, 0, buffer.Length);

                                    s.Write(buffer, 0, sourceBytes);

                                }

                                while (sourceBytes > 0);

                            }

                        }

                        s.Finish();

                        s.Close();

                        s.Dispose();
                        return FileMessage.None;
                    }
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