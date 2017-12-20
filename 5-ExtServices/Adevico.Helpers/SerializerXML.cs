using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
using System.IO;

namespace Adevico.Helpers
{
    /// <summary>
    /// Serializzatore generico
    /// </summary>
    public static class SerializerXML
    {
        /// <summary>
        /// Salva/crea l'oggetto di tipo T serializzandolo (XML) nel file indicato
        /// </summary>
        /// <typeparam name="T">Tipo dell'oggetto</typeparam>
        /// <param name="myObject">Oggetto da serializzare</param>
        /// <param name="path">Percorso completo in cui salvare il file</param>
        public static void Save<T>(T myObject, String path)
        {
            try
            {

                XmlSerializer xs = new XmlSerializer(typeof(T));
                StreamWriter sw = new StreamWriter(path);

                xs.Serialize(sw, myObject);

                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Carica un oggetto di tipo T dall'XML indicato
        /// </summary>
        /// <typeparam name="T">Tipo di oggetto da recuperare</typeparam>
        /// <param name="path">Percorso con il file .xml</param>
        /// <returns>L'oggetto T con i dati salvati nell'xml</returns>
        public static T Load<T>(String path)
        {
            T conf = default(T);

            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                StreamReader sr = new StreamReader(path);

                conf = (T)xs.Deserialize(sr);

                sr.Close();

                return conf;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Carica l'oggetto T. SE non ci riesce, restituisce un oggetto nuovo
        /// e cerca di salvarlo nel path indicato.
        /// </summary>
        /// <typeparam name="T">Tipo dell'oggetto da recuperare</typeparam>
        /// <param name="path">Percorso completo del file sorgente</param>
        /// <returns>L'oggetto T.</returns>
        public static T LoadOrCreate<T>(String path) where T : new()
        {
            
            FileInfo fi = new FileInfo(path);
            T result = new T();

            bool save = false;

            if (fi == null || !fi.Exists)
            {
                result = new T();//default(T);
                save = true;
                //return result;
            }
            else
            {
                try
                {
                    result = Load<T>(path);    
                }
                catch (Exception)
                {
                    
                }


                if (result == null)
                {
                    result = new T();
                    save = true;
                }
            }



            if (save)
            {
                Save(result, path);    
            }

            return result; //Load<T>(path);
        }
    }
}