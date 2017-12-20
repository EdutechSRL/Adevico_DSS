using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Business
{
    public class ImageHelper
    {
        public static string GetImageUrl(ImageDataDTO Data)
        {
            return GetImageUrl(Data.ImageUrl, Data.BaseUrl, Data.TemplateId, Data.VersionId);
        }

        public static string GetImageUrl(ImageDataDTO Data, String ImageUrl)
        {
            return GetImageUrl(ImageUrl, Data.BaseUrl, Data.TemplateId, Data.VersionId);
        }

        public static string GetImageUrl(String ImageUrl, String BaseUrl, Int64 TemplateId, Int64 VersionId)
        {
            return BaseUrl + "/" + TemplateId.ToString() + "/" + VersionId.ToString() + "/" + ImageUrl;
        }

        public static string GetImagePath(String ImageUrl, String BasePath, Int64 TemplateId, Int64 VersionId)
        {
            return (BasePath + "\\" + TemplateId.ToString() + "\\" + VersionId.ToString() + "\\" + ImageUrl).Replace("\\\\", "\\");
        }
    }

    public class ImageDataDTO
    {
        public String ImageUrl { get; set; }
        public String BaseUrl { get; set; }
        public Int64 TemplateId { get; set; }
        public Int64 VersionId { get; set; }
    }
}
