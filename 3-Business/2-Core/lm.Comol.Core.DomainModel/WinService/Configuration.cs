using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace lm.Comol.Core.WinService.Configurations
{
    public class Configuration<T>
    {
        public const String FileName="ServiceConfiguration.xml";
        public static void Save(T config, String path, String fileName="")
        {
            try
            {

                XmlSerializer xs = new XmlSerializer(typeof(T));
                StreamWriter sw = new StreamWriter(path + "\\" + (String.IsNullOrEmpty(fileName) ? FileName : fileName));

                xs.Serialize(sw, config);

                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static T Load(String path, String fileName = "")
        {
            T conf = default(T);

            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                StreamReader sr = new StreamReader(path + "\\" + (String.IsNullOrEmpty(fileName) ? FileName : fileName));

                conf = (T)xs.Deserialize(sr);

                sr.Close();

                return conf;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static T LoadOrCreate(String path, String fileName = "")
        {
            try
            {
                return Load(path, fileName);
            }
            catch (Exception ex)
            {
                T result = default(T);
                Save(result, path, fileName);
                return result;
            }
        }
    }
}