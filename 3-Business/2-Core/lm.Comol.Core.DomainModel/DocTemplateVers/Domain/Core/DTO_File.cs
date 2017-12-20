using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Domain.Core
{
    public class dtoFileCopyBlock
    {
        public Int64 OldVersionId { get; set; }
        public Int64 OldTemplateId { get; set; }
        public Int64 NewTemplateId { get; set; }
        public Int64 NewVersionId { get; set; }

        public string BasePath { get; set; }
        public string BaseTempPath { get; set; }

        public IList<dtoFileNames> Files { get; set; }

        public dtoFileCopyBlock()
        {
            OldVersionId = 0;
            OldTemplateId = 0;
            NewTemplateId = 0;
            NewVersionId = 0;

            BasePath = "";
            BaseTempPath = "";

            Files = new List<dtoFileNames>();
        }

        public String AddFile(String SourceFile)
        {
            string newfile = "";

            if (!String.IsNullOrEmpty(SourceFile))
            {
                int dotPos = SourceFile.LastIndexOf('.');
                newfile = System.Guid.NewGuid().ToString() + SourceFile.Remove(0, dotPos);
                Files.Add(new dtoFileNames(SourceFile, newfile));
            }
            return newfile;
        }
    }

    public class dtoFileNames
    {
        public dtoFileNames(string source, string dest)
        {
            SourceFile = source;
            DestFile = dest;
        }
        public string SourceFile { get; set; }
        public string DestFile { get; set; }
    }

    public class dtoFileCopy
    {
        public string SourceFile { get; set; }
        public Int64 srcTempalteId { get; set; }
        public Int64 srcVersionId { get; set; }

        public string DestFile { get; set; }
        public Int64 dstTempalteId { get; set; }
        public Int64 dstVersionId { get; set; }

        public dtoFileCopy(string SourceFile, Int64 srcTempalteId, Int64 srcVersionId, string DestFile, Int64 dstTempalteId, Int64 dstVersionId)
        {
            this.SourceFile = SourceFile;
            this.srcTempalteId = srcTempalteId;
            this.srcVersionId = srcVersionId;

            this.DestFile = DestFile;
            this.dstTempalteId = dstTempalteId;
            this.dstVersionId = dstVersionId;
        }
            
    }

    public class dtoFileDeleteRemTmp
    {
        public string SourceFile { get; set; }
        public Int64 TempalteId { get; set; }
        public Int64 VersionId { get; set; }

        public dtoFileDeleteRemTmp(string SourceFile, Int64 TempalteId, Int64 VersionId)
        {
            this.SourceFile = SourceFile;
            this.TempalteId = TempalteId;
            this.VersionId = VersionId;
        }
    }
}
