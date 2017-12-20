using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace lm.Comol.Core.File
{
    public class Serialize<T>
    {
        public static FileMessage SaveToFile(T oItem, String path, String fileName)
        {
            if (String.IsNullOrWhiteSpace(fileName))
                return FileMessage.InvalidFileName;
            if (String.IsNullOrWhiteSpace(path))
                return FileMessage.InvalidPath;
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                    return FileMessage.ImpersonationFailed;
                else
                {
                    Boolean pathExists = Exists.Directory(path);
                    if (!pathExists)
                        pathExists = Create.Directory(path);
                    if (pathExists)
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(T));
                        StreamWriter sw = new StreamWriter(path + "\\" + fileName);

                        xs.Serialize(sw, oItem);

                        sw.Flush();
                        sw.Close();
                        return FileMessage.ChangeSaved;
                    }
                    else
                        return FileMessage.DirectoryDoesntExist;
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
        public static T Load(String path, String fileName)
        {
            T sObject = default(T);

            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                    throw new Exception();
                else
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    StreamReader sr = new StreamReader(path + "\\" + fileName);

                    sObject = (T)xs.Deserialize(sr);

                    sr.Close();

                    return sObject;
                }
            }
            catch (Exception ex)
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
                throw ex;
            }
            finally
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
            }
        }
        public static T LoadOrCreate(String path, String fileName)
        {

            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                    throw new Exception();
                else
                    return Load(path, fileName);
            }
            catch (Exception ex)
            {
                if (!wasImpersonated)
                {
                    oImpersonate.UndoImpersonation();
                    throw ex;
                }
                else
                {
                    T result = default(T);
                    SaveToFile(result, path, fileName);
                    return result;
                }
            }
            finally
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
            }
        }
    }
}