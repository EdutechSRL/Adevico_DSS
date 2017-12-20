using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adevico.Helpers
{
    /// <summary>
    /// Gestione dell'output nella consolle
    /// </summary>
    public class OutputManager
    {
        private String TraceString = "";

        //~OutputManager()
        //{
        //    if(this.TraceString != "")
        //    {
        //        try
        //        {
        //            this.SaveTrace("ERROR.log", 0);
        //        }
        //        catch { }
        //    }
        //}
        /// <summary>
        /// Aggiunge il testo indicato nella posizione corrente della consolle
        /// </summary>
        /// <param name="Text">Testo da scrivere in consolle</param>
        /// <param name="Color">Il colore con cui visualizzare il testo</param>
        /// <param name="Trace">SE aggiungere il testo ai LOG</param>
        public void Write(String Text, ConsoleColor Color = ConsoleColor.Gray, Boolean Trace = false)
        {
            Console.ForegroundColor = Color;
            Console.Write(Text);

            if (Trace)
            {
                AddTrace(Text);
                
            }
        }


        public void Wait(String text = "")
        {
            if (String.IsNullOrEmpty(text))
            {
                text = " - Press a Key to continue. - ";
            }

            WriteLine(text, ConsoleColor.Gray, false);
            Console.ReadKey();

        }

        /// <summary>
        /// Aggiunge il testo indicato in una nuova riga della consolle
        /// </summary>
        /// <param name="Text">Testo da scrivere in consolle</param>
        /// <param name="Color">Il colore con cui visualizzarlo</param>
        /// <param name="Trace">SE aggiungere il testo ai LOG</param>
        public void WriteLine(String Text, ConsoleColor Color = ConsoleColor.Gray, Boolean Trace = false)
        {

            Console.ForegroundColor = Color;
            Console.WriteLine(Text);
            if (Trace)
            {
                AddTrace(Text);
            }
        }

        public void WriteLineDatetime(String Text, ConsoleColor Color = ConsoleColor.Gray, Boolean Trace = false)
        {
            Console.ForegroundColor = Color;
            Console.WriteLine(
                String.Format("{0} : {1}", DateTime.Now, Text)
                );

            if (Trace)
            {
                AddTrace(Text);
            }
        }

#region TRACE: ToDo - SOSTITUIRE con nLog

        /// <summary>
        /// Aggiungere il testo indicato ai log
        /// </summary>
        /// <param name="text">Il testo da aggiungere</param>
        private void AddTrace(String text)
        {
            text = text.Trim();
            if (text.EndsWith(":")) { text = text.Remove(text.LastIndexOf(":"), 1);  }
            TraceString += DateTime.Now.ToString() + " - " + text.Replace("\r\n", "") + "\r\n";
        }

        /// <summary>
        /// Scrive definitivamente il testo nei file di log.
        /// </summary>
        /// <param name="filepath">Il percorso del file di log</param>
        /// <param name="SizeKb">Dimensioni massime del file di log</param>
        public void SaveTrace(string filepath, int SizeKb)
        {
            String filename;
            int dotpos = filepath.LastIndexOf(".");
            string extension = ".log";

            if (dotpos < 0)
            {
                filename = filepath;
            }
            else
            {
                extension = filepath.Remove(0, dotpos);
                filename = filepath.Remove(dotpos, extension.Length);
            }
            
            Boolean found = false;
            int ver = 0;

            string currentfilename;

            if (SizeKb > 0)
            {
                do
                {
                    currentfilename = filename + "_" + ver.ToString() + extension;
                    if (System.IO.File.Exists(currentfilename))
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(currentfilename);
                        if (fi.Length < SizeKb * 1024)
                        {
                            found = true;
                        }
                        else
                        {
                            ver++;
                        }
                    }
                    else
                    {
                        found = true;
                    }

                } while (found != true && ver <= 1000);

            }
            else
            {
                currentfilename = filename + extension;
            }
            
            if (ver > 1000) 
            { 
                filepath = filename + "_0" + extension;
                System.IO.File.Delete(filepath);
            }
            else { filepath = currentfilename; }

            //Save trace...
            this.WriteLine("\r\n\r\n - Trace data - \r\n", ConsoleColor.Yellow, false);
            this.WriteLine(TraceString, ConsoleColor.DarkYellow, false);
            
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
                {
                    file.WriteLine(TraceString);
                }
            }
            catch
            {
                this.WriteLine("Log file error! No log was saved.", ConsoleColor.Red, false);
            }

            TraceString = "";
        }

        
#endregion

        //public void SaveBLOCKTrace(string filepath, int SizeKb)
        //{
        //    if (TraceString != "")
        //    {
        //        TraceString += "\r\n";
        //        String filename;
        //        int dotpos = filepath.LastIndexOf(".");
        //        string extension = ".log";

        //        if (dotpos < 0)
        //        {
        //            filename = filepath;
        //        }
        //        else
        //        {
        //            extension = filepath.Remove(0, dotpos);
        //            filename = filepath.Remove(dotpos, extension.Length);
        //        }

        //        Boolean found = false;
        //        int ver = 0;

        //        string currentfilename;

        //        do
        //        {
        //            currentfilename = filename + "_" + ver.ToString() + extension;
        //            if (System.IO.File.Exists(currentfilename))
        //            {
        //                System.IO.FileInfo fi = new System.IO.FileInfo(currentfilename);
        //                if (fi.Length < SizeKb * 1024)
        //                {
        //                    found = true;
        //                }
        //                else
        //                {
        //                    ver++;
        //                }
        //            }
        //            else
        //            {
        //                found = true;
        //            }

        //        } while (found != true && ver <= 1000);

        //        if (ver > 1000)
        //        {
        //            filepath = filename + "_0" + extension;
        //            System.IO.File.Delete(filepath);
        //        }
        //        else { filepath = currentfilename; }

        //        //Save trace...
        //        //this.WriteLine("\r\n\r\n - Trace data - \r\n", ConsoleColor.Yellow, false);
        //        //this.WriteLine(TraceString, ConsoleColor.DarkYellow, false);

        //        try
        //        {
        //            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
        //            {
        //                file.WriteLine(TraceString);
        //            }
        //        }
        //        catch
        //        {
        //            this.WriteLine("Log file error! No log was saved.", ConsoleColor.Red, false);
        //        }

        //    }
        //}
    }
}
