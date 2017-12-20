using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace lm.Comol.Core.DomainModel.Helpers
{
    public class XmlRepository<T> where T : new()
    {
        public void Serialize(String filename, T item)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            StreamWriter sw = new StreamWriter(filename);

            xs.Serialize(sw, item);

            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        public T Deserialize(String filename)
        {
            XmlSerializer xs = null;
            StreamReader sr = null;
            try
            {
                xs = new XmlSerializer(typeof(T));
                sr = new StreamReader(filename);
                T item = (T)xs.Deserialize(sr);
                sr.Close();
                sr.Dispose();
                return item;
            }
            catch (Exception ex)
            {
                if (sr != null) {
                    sr.Close();
                    sr.Dispose();
                }
                return default(T);
                //return Create();
            }
        }
    }
}