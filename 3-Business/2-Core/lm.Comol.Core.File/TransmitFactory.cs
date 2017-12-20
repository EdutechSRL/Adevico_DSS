using System;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Security;
using System.Security.Principal;
using System.Security.Permissions;
using System.Configuration;
using lm.Comol.Core.DomainModel;
using System.IO;

namespace lm.Comol.Core.File
{
    public class TransmitFactory
    {

        public static FileMessage TransmitFile(String fullFilename, HttpResponse response)
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
                    if (System.IO.File.Exists(fullFilename))
                    {
                        int chunkSize = 64;
                        byte[] buffer = new byte[chunkSize];
                        int offset = 0;
                        int read = 0;
                        using (FileStream fs = System.IO.File.Open(fullFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            while ((read = fs.Read(buffer, offset, chunkSize)) > 0)
                            {
                                response.OutputStream.Write(buffer, 0, read);
                                response.Flush();
                            }
                        }

                        response.Close();
                        return FileMessage.Read;
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

     
        //public TransmitFactory(HttpResponse response){
        //    _HttpResponse = response;
        //}
        //public void TransmitFile(string filename, long offset, long length)
        //{
        //    if (filename == null)
        //    {
        //        throw new ArgumentNullException("filename");
        //    }
        //    if (offset < 0L)
        //    {
        //        throw new ArgumentException(SR.GetString("Invalid_range"), "offset");
        //    }
        //    if (length < -1L)
        //    {
        //        throw new ArgumentException(SR.GetString("Invalid_range"), "length");
        //    }
        //    filename = this.GetNormalizedFilename(filename);
        //    using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
        //    {
        //        long num = stream.Length;
        //        if (length == -1L)
        //        {
        //            length = num - offset;
        //        }
        //        if (num < offset)
        //        {
        //            throw new ArgumentException(SR.GetString("Invalid_range"), "offset");
        //        }
        //        if ((num - offset) < length)
        //        {
        //            throw new ArgumentException(SR.GetString("Invalid_range"), "length");
        //        }
        //        if (!this.UsingHttpWriter)
        //        {
        //            this.WriteStreamAsText(stream, offset, length);
        //            return;
        //        }
        //    }
        //    if (length > 0L)
        //    {
        //        bool supportsLongTransmitFile = (this._wr != null) && this._wr.SupportsLongTransmitFile;
        //        this._httpWriter.TransmitFile(filename, offset, length, this._context.IsClientImpersonationConfigured || HttpRuntime.IsOnUNCShareInternal, supportsLongTransmitFile);
        //    }
        //}



        //private void WriteStreamAsText(Stream f, long offset, long size)
        //{
        //    if (size < 0L)
        //    {
        //        size = f.Length - offset;
        //    }
        //    if (size > 0L)
        //    {
        //        if (offset > 0L)
        //        {
        //            f.Seek(offset, SeekOrigin.Begin);
        //        }
        //        byte[] buffer = new byte[(int)size];
        //        int count = f.Read(buffer, 0, (int)size);
        //        this._writer.Write(Encoding.Default.GetChars(buffer, 0, count));
        //    }
        //}


        //internal void TransmitFile(string filename, long offset, long size, bool isImpersonating, bool supportsLongTransmitFile)
        //{
        //    if (this._charBufferLength != this._charBufferFree)
        //    {
        //        this.FlushCharBuffer(true);
        //    }
        //    this._lastBuffer = null;
        //    this._buffers.Add(new HttpFileResponseElement(filename, offset, size, isImpersonating, supportsLongTransmitFile));
        //    if (!this._responseBufferingOn)
        //    {
        //        this._response.Flush();
        //    }
        //} 

    }
}
