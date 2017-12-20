using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.File;
using DT = lm.Comol.Core.DomainModel.DocTemplateVers;

namespace lm.Comol.Core.BaseModules.DocTemplate.Helpers
{
    public class DocTempalteFileHelper
    {
        public static  String UploadFile(System.Web.UI.WebControls.FileUpload UpdControl, String BaseTempPath)
        {
            BaseTempPath.Replace("/", "\\");
            if (!BaseTempPath.EndsWith("\\"))
                BaseTempPath += "\\";
        
            String FileName = System.Guid.NewGuid().ToString();

            String SourceFileName = UpdControl.PostedFile.FileName;

            int extIndex = SourceFileName.LastIndexOf(".");

            FileName += SourceFileName.Remove(0, extIndex);

            //Eventuale controllo su estensione

            if(Delete.File(FileName))
            {

                //String fullFileName = ;
                System.Web.HttpPostedFile file = UpdControl.PostedFile;

                if (!File.Exists.Directory(BaseTempPath))
                {
                    try
                    {
                        File.Create.Directory(BaseTempPath);
                    }
                    catch (Exception ex)
                    { 

                    }
                }

                FileMessage result = Create.UploadFile(ref file, BaseTempPath + FileName);

                switch(result)
                {
                    case FileMessage.FileCreated:
                        return FileName;
                    case FileMessage.NotDeleted:
                        return "ErrorCode.FileExist";
                    case FileMessage.UploadError:
                        return "ErrorCode.UploadError";
                    default:
                        return "ErrorCode.GenericError";
                }
                
            } else {
                return "ErrorCode.FileExist";
            }
        }
        public static bool DeleteFile(String filePath)
        {
            return Delete.File(filePath);
        }

        public static bool CopyFile(String SourceFile, String DestFile)
        {
            if (File.Exists.File(SourceFile))
            {
                return File.Create.CopyFile(SourceFile, DestFile);
            }
            return false;
        }

        public static int CopyBlockFile(lm.Comol.Core.DomainModel.DocTemplateVers.Domain.Core.dtoFileCopyBlock dtoFile) //, string BasePath, string BaseTemplPath)
        {
            int copied = 0;
            if (dtoFile.Files != null && dtoFile.Files.Count > 0)
            {

                string sourcePathtemp = DT.Business.ImageHelper.GetImageUrl("", dtoFile.BaseTempPath, dtoFile.OldTemplateId, dtoFile.OldVersionId);
                string sourcePath = DT.Business.ImageHelper.GetImageUrl("", dtoFile.BasePath, dtoFile.OldTemplateId, dtoFile.OldVersionId);
                string destPath = DT.Business.ImageHelper.GetImageUrl("", dtoFile.BasePath, dtoFile.NewTemplateId, dtoFile.NewVersionId);
                
                if (!File.Exists.Directory(destPath))
                    File.Create.Directory(destPath);

                foreach (lm.Comol.Core.DomainModel.DocTemplateVers.Domain.Core.dtoFileNames dtFile in dtoFile.Files)
                {
                    if (!string.IsNullOrEmpty(dtFile.SourceFile) && !string.IsNullOrEmpty(dtFile.SourceFile))
                    {
                        if (dtFile.SourceFile.StartsWith("#"))
                        {
                            string sourceFile = sourcePathtemp + dtFile.SourceFile.Remove(0, 1);

                            if (CopyFile(sourceFile, destPath + dtFile.DestFile))
                                copied++;
                        }
                        else
                        {
                            string sourceFile = sourcePath + dtFile.SourceFile;

                            if (CopyFile(sourceFile, destPath + dtFile.DestFile))
                                copied++;
                        }
                    }
                }
            }
            
            return copied;
        }

        public static bool MoveFile(String SourceFile, String DestFile)
        {
            if (File.Exists.File(SourceFile))
            {
                if (File.Create.CopyFile(SourceFile, DestFile))
                    File.Delete.File(SourceFile);
            }
            return false;
        }

        //public static string GetFullPath(string BasePath, Int64 TemplateId, Int64 VersionId)
        //{
        //    return BasePath + "\\" + TemplateId.ToString() + "\\" + VersionId.ToString() + "\\";
        //}
    
    }
}
