using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using System.IO;

namespace lm.Comol.Core.DomainModel.Helpers.Export
{
    [Serializable]
    public abstract class ExportXmlBaseHelper 
    {

        #region "Export To file / Stream"
            #region "Web Export"
                public String WebExport(String fileName, System.Web.HttpResponse webResponse)
                {
                    return WebExport(false, fileName,  webResponse, null);
                }
                public String WebExport(Boolean openCloseConnection, String fileName, System.Web.HttpResponse webResponse)
                {
                    return WebExport(openCloseConnection, fileName, webResponse, null);
                }
            #endregion

            //#region "File Export"
            //    public String FileExport(System.IO.FileStream fileStream)
            //    {
            //        return ExportTo(fileStream,false);
            //    }
            //#endregion

        #region "Export"
            public String WebExport(Boolean openCloseConnection, String fileName, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
            {
                String content="";
                webResponse.Write(content);
                if (openCloseConnection)
                    webResponse.Clear();
                if (cookie != null)
                    webResponse.AppendCookie(cookie);
                webResponse.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "." + ExportFileType.xml.ToString());
                webResponse.ContentType = "application/ms-excel";

                content = ExportTo(false);
                webResponse.Write(content);
                if (!String.IsNullOrEmpty(content) && openCloseConnection)
                    webResponse.End();
                return content;
            }
            public String GetErrorDocument(Boolean openCloseConnection, String fileName, System.Web.HttpResponse webResponse)
            {
                return GetErrorDocument(openCloseConnection,fileName, webResponse, null);
            }
            public String GetErrorDocument(Boolean openCloseConnection, String fileName, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
            {
                String content = "";
                if (openCloseConnection)
                    webResponse.Clear();
                if (cookie!=null)
                    webResponse.AppendCookie(cookie);
                webResponse.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".rtf");
                // + ExportFileType.rtf.ToString());
                webResponse.ContentType = "application/rtf";

                content = ExportTo(true);
                webResponse.Write(content);
                if (!String.IsNullOrEmpty(content) && openCloseConnection)
                    webResponse.End();
                return content;
            }

            private String ExportTo(Boolean forErrorContent)
            {
                String content = "";
                try
                {                  
                    ////Compilazione documento
                    
                    if (forErrorContent)
                        content = RenderErrorDocument();
                    else
                        content = RenderDocument();
                }
                catch (Exception ex)
                {
                    content = "";
                }
                return content;
            }
            //private String ExportTo(System.IO.FileStream stream, Boolean forErrorContent)
            //{
            //    String content = "";
            //    try
            //    {
            //        ////Compilazione documento

            //        if (forErrorContent)
            //            content = RenderErrorDocument();
            //        else
            //            content = RenderDocument();
            //    }
            //    catch (Exception ex)
            //    {
            //        content = "";
            //    }
            //    return content;
            //}
        #endregion

            protected abstract String RenderDocument();
            protected abstract String RenderErrorDocument();

        #endregion
    }
}