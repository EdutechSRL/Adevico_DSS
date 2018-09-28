using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using System.IO;        // USARE: lm.Comol.core.File
using lm.Comol.Core.File;

namespace lm.Comol.Modules.Standard.Skin.Business
{
    /// <summary>
    /// La funzioni "base" per lavorare sui file relativi alle skin.
    /// SONO ESCLUSE le funzioni di upload, poichè viene usato direttamente il controllo asp.net direttamente nel code-behind.
    /// </summary>
    /// <remarks>
    ///     BasePath    Path assoluto della cartella contenente TUTTE le skin (contenuto in Config_*.config)
    ///         Es: E:\Inetpub\ProgettiWeb\LM_ComolElle3\Src\Comunita_OnLine\File\Skins\
    ///         
    ///     SkinPath    Path assoluto della cartella contente TUTTI i file di una singola skin. In pratica BasePath + Skin_ID
    ///         Es: E:\Inetput\...\File\Skin\45\
    ///         
    ///     ImagePage   Path assoluto della cartella contente TUTTI i file di una singola skin. In pratica BasePath + Skin_ID + Images
    ///         Es: E:\Inetput\...\File\Skin\45\Images
    /// </remarks>
    public class SkinFileManagement
    {
        #region Stringhe e costanti

        /// <summary>
        /// Tipi si css
        /// </summary>
        public enum CssType {None,Main, Admin, IE, Login};

        /// <summary>
        /// Cartella immagini
        /// </summary>
        private static String ImageDir = "Images";

        /// <summary>
        /// Nome del CSS principale
        /// </summary>
        private static String MainCss = "Main.css";

        /// <summary>
        /// Nome del CSS specifico per IE
        /// </summary>
        private static String IECss = "IE.css";

        /// <summary>
        /// Nome del CSS per l'amministrazione
        /// </summary>
        private static String AdminCss = "Admin.css";

        /// <summary>
        /// Nome del CSS per la login (SOLO se PORTALE)
        /// </summary>
        private static String LoginCss = "Login.css";

        #endregion

        #region Get path e dintorni
        // BasePath         =>              C:\inetput\wwwroot\Comol\Skin\
        // SkinPath = BasePath + SkinIs =>  C:\inetput\wwwroot\Comol\Skin\45\
        // BaseVirtualPath  =>              /Comol_Elle3/File/Skin/
        /// <summary>
        /// Nome completo del file
        /// </summary>
        /// <param name="SkinPath">Path base della skin</param>
        /// <param name="Type">Tipo di css</param>
        /// <returns>es: C:\inetput\wwwroot\Comol\Skin\45\Main.css</returns>
        public static String GetFullCssName(String SkinPath, CssType Type, Boolean IsWeb)
        {
            if (SkinPath == "") return "";

            String FullName = SkinPath;// +"\\" + SkinId.ToString();
            // return BasePath + "\\" + SkinId.ToString() + "\\" + ImageDir;

            if (IsWeb)
            {
                FullName = FullName.Replace("\\", "/");
                if (!FullName.EndsWith("/"))
                {
                    FullName += "/";
                }
                //FullName.Replace("//", "/");  // Tanto non lo fa...
            }
            else
            {
                FullName = FullName.Replace("/", "\\");
                if (!FullName.EndsWith("\\"))
                {
                    FullName += "\\";

                }
                //FullName.Replace("\\\\", "\\");   // Tanto non lo fa...
            } 

            switch (Type)
            {
                case CssType.Main:
                    FullName += MainCss;
                    break;

                case CssType.IE:
                    FullName += IECss;
                    break;

                case CssType.Admin:
                    FullName += AdminCss;
                    break;

                case CssType.Login:
                    FullName += LoginCss;
                    break;
            }


            return FullName;
        }

        /// <summary>
        /// Il percorso completo delle immagini
        /// </summary>
        /// <param name="SkinPath">Path base della skin</param>
        /// <returns>es: C:\inetput\wwwroot\Comol\Skin\45\Images\ </returns>
        public static String GetImagePath(String SkinPath)
        {
            if (SkinPath == "") return "";

            SkinPath = SkinPath.Replace("/", "\\");

            if (!SkinPath.EndsWith("\\"))
            {
                SkinPath += "\\";
            }

            return SkinPath + ImageDir;
        }
        /// <summary>
        /// Il percorso completo delle immagini
        /// </summary>
        /// <param name="BasePath">Path base</param>
        /// <param name="SkinId">Id della skin</param>
        /// <returns>es: C:\inetput\wwwroot\Comol\Skin\45\Images\ </returns>
        public static String GetImagePath(String BasePath, Int64 SkinId)
        {
            if (BasePath == "") return "";

            BasePath = BasePath.Replace("/", "\\");

            if (!BasePath.EndsWith("\\"))
            {
                BasePath += "\\";
            }

            return BasePath + "\\" + SkinId.ToString() + "\\" + ImageDir;
        }
        /// <summary>
        /// Path virtuale per le immagini
        /// </summary>
        /// <param name="BaseVirtualPath">Path base</param>
        /// <param name="SkinId">Id della skin</param>
        /// <param name="ImageName">Nome immagine</param>
        /// <returns>/Comol_Elle3/File/Skin/45/Images/</returns>
        public static String GetImageVirtualPath(String BaseVirtualPath, Int64 SkinId, String ImageName)
        {
            if (BaseVirtualPath == "") return "";

            BaseVirtualPath = BaseVirtualPath.Replace("\\", "/");

            if (!BaseVirtualPath.EndsWith("/")) { BaseVirtualPath += "/"; }

            return (BaseVirtualPath + SkinId + "/" + ImageDir + "/" + ImageName).Replace("//", "/");
        }
        /// <summary>
        /// Path completo dei un logo
        /// </summary>
        /// <param name="BasePath">Path base</param>
        /// <param name="SkinId">Id Skin</param>
        /// <param name="LogoId">Id Logo</param>
        /// <param name="ImageName">Nome Immagine Logo</param>
        /// <returns>es: C:\inetput\wwwroot\Comol\Skin\45\7_Logo.png</returns>
        public static String GetLogoFullPath(String BasePath, Int64 SkinId, Int64 LogoId, String ImageName)
        {
            if (BasePath == "") return "";

            BasePath = BasePath.Replace("/", "\\");

            if (!BasePath.EndsWith("\\"))
            {
                BasePath += "\\";
            }

            return BasePath + SkinId.ToString() + "\\" + LogoId.ToString() + "_" + ImageName;
        }
        /// <summary>
        /// Path virtuale di un logo
        /// </summary>
        /// <param name="SkinId">Id della skin</param>
        /// <param name="LogoId">Id del logo</param>
        /// <param name="ImageName">Nome immagine</param>
        /// <returns>Es: /45/7_Logo.png</returns>
        public static String GetLogoVirtualPath(Int64 SkinId, Int64 LogoId, String ImageName)
        {
            String Path = "/" + SkinId.ToString() + "/" + LogoId.ToString() + "_" + System.Web.HttpUtility.UrlEncode(ImageName);

            Path = Path.Replace("\\", "/").Replace("//", "/");

            return Path;
        }
        /// <summary>
        /// Path virtuale completo di un logo
        /// </summary>
        /// <param name="SkinId">Id della skin</param>
        /// <param name="LogoId">Id del logo</param>
        /// <param name="ImageName">Nome immagine</param>
        /// <returns>Es: /Comol_Elle3/File/Skin/45/7_Logo.png</returns>
        public static String GetLogoVirtualFullPath(String baseUrl, Int64 SkinId, Int64 LogoId, String ImageName)
        {
            String path = GetLogoVirtualPath(SkinId, LogoId, ImageName);
            path = path.Replace("//", "/").Replace("\\", "/");
            if (baseUrl.EndsWith("/"))
                return ((path.StartsWith("/")) ? baseUrl + path.Remove(0,1).ToString() : baseUrl + path);
            else
                return  ((path.StartsWith("/")) ? baseUrl + path : baseUrl + "/" + baseUrl);
        }
        
        #endregion
        
        #region Dir Skin
        //IO:3  <- OK
        /// <summary>
        /// Crea la directory per una nuova skin
        /// </summary>
        /// <param name="SkinId">L'ID della skin appena creata</param>
        /// <param name="BasePath">Il percorso base con i file della skin (da file di configurazione)</param>
        /// <remarks>
        /// !ATTENZIONE! Se esiste già una cartella relativa all'ID corrente, questa viene completamente svuotata.
        /// Questo perchè viene usata per creare la struttura per una nuova SKIN. L'ID quindi sarà quello di una nuova skin.
        /// </remarks>
        public static void CreateDir(Int64 SkinId, string BasePath)
        {
            if (BasePath == "") return;

            BasePath = BasePath.Replace("/", "\\");
            if (!BasePath.EndsWith("\\"))
            {
                BasePath += "\\";
            }

            string path = BasePath + SkinId.ToString();

            //if (Directory.Exists(path))
            //{
            EraseDir(path); 
            //}

            Create.Directory(path);
            Create.Directory(GetImagePath(path));
            
            Create.TextFile(path + "\\" + MainCss, "", true, false);
            Create.TextFile(path + "\\" + AdminCss, "", true, false);
            Create.TextFile(path + "\\" + IECss, "", true, false);
            Create.TextFile(path + "\\" + LoginCss, "", true, false);

            //Directory.CreateDirectory(path);
            //Directory.CreateDirectory(GetImagePath(path));
        }
        /// <summary>
        /// Elimina TUTTI i file di una skin
        /// </summary>
        /// <param name="SkinId">L'ID della skin di cui eliminare TUTTI i file</param>
        /// <param name="BasePath">Il Path originario della Skin</param>
        /// <remarks>USARE quando si cancella una SKIN</remarks>
        public static void EraseDir(Int64 SkinId, string BasePath)
        {
            if (BasePath == "") return;


            BasePath = BasePath.Replace("/", "\\");
            if (!BasePath.EndsWith("\\"))
            {
                BasePath += "\\";
            }

            string path = BasePath + SkinId.ToString();

            EraseDir(path);
        }

        //IO:4 <- OK
        /// <summary>
        /// Cancella TUTTA la cartella ed il relativo contenuto.
        /// </summary>
        /// <param name="Path">Il percorso fisico.</param>
        /// <remarks>
        /// SALVO permessi, potenzialmente cancella TUTTO!
        /// USARE con PRUDENZA.
        /// </remarks>
        private static void EraseDir(String Path)
        {
            if (Path == "") return;

            Delete.Directory(Path, true);

            //// Cancello TUTTI i file nella directory
            //try
            //{
            //    String[] Files = Directory.GetFiles(Path);
            //    foreach (string file in Files)
            //    {

            //            File.Delete(file);
            //    }
            //} catch { }
            
            //// Ricorsivamente cancello anche le cartelle contenute
            //String[] Directories = Directory.GetDirectories(Path);
            //foreach (string dir in Directories)
            //{
            //    try
            //    {
            //        EraseDir(dir);
            //    }
            //    catch { }
            //}

            //try
            //{
            //    Directory.Delete(Path);
            //}
            //catch { }
            

        }
        #endregion

        #region CSS, Image, Loghi
        //IO:2 <- OK
        /// <summary>
        /// Cancella un file CSS
        /// </summary>
        /// <param name="SkinPath">Il path base di una skin</param>
        /// <param name="Type">Il tipo di css</param>
        public static void DelCssFile(String SkinPath, CssType Type)
        { 
            String FileName = GetFullCssName(SkinPath, Type, false);
            Delete.File(FileName);

            Create.TextFile(FileName, "", true, false);
            
        }

        //IO:2  <- OK
        /// <summary>
        /// Elenco di immagini di una skin (percorso fisico)
        /// </summary>
        /// <param name="BasePath">Indirizzo base</param>
        /// <param name="SkinId">Id skin</param>
        /// <returns></returns>
        public static IList<Domain.DTO.DtoSkinImage> GetImages(String BasePath, Int64 SkinId)
        {   
            String ImagePath = GetImagePath(BasePath, SkinId);
            if (ImagePath == "") return null;           


            List<Domain.DTO.DtoSkinImage> Images = new List<Domain.DTO.DtoSkinImage>();
            //String[] files = Directory.GetFiles(ImagePath);

            String[] files = new String[] { };
            lm.Comol.Core.File.ContentOf.Directory(ImagePath, ref files, true);


            foreach (String FileName in files)
            {
                System.IO.FileInfo File = ContentOf.File_Info(FileName);//new FileInfo(FileName);
                Domain.DTO.DtoSkinImage Dtoimg = new Domain.DTO.DtoSkinImage();
                Dtoimg.Name = File.Name;
                Dtoimg.SizeByte = File.Length;

                Images.Add(Dtoimg);
            }

            return Images;
        }

        //IO:1  <- OK
        /// <summary>
        /// Cancella una singola immagine di una skin
        /// </summary>
        /// <param name="BasePath">Percorso base</param>
        /// <param name="SkinId">Id skin</param>
        /// <param name="ImageName">Nome immagine</param>
        public static void DeleteImage(String BasePath, Int64 SkinId, String ImageName)
        {
            if (ImageName.StartsWith("\\"))
            {
                ImageName.Remove(0,1);
            }

            //File.Delete(GetImagePath(BasePath, SkinId) + "\\" + ImageName);
            Delete.File(GetImagePath(BasePath, SkinId) + "\\" + ImageName);
        }

        //IO:1  <- OK
        /// <summary>
        /// Cancella un logo
        /// </summary>
        /// <param name="SkinId">Id della skin</param>
        /// <param name="BasePath">Percorso base</param>
        /// <param name="LogoId">Id del logo</param>
        /// <param name="ImageName">Nome del logo</param>
        public static void DeleteLogo(Int64 SkinId, String BasePath, Int64 LogoId, String ImageName)
        {
            Delete.File(GetLogoFullPath(BasePath, SkinId, LogoId, ImageName));
            //File.Delete(GetLogoFullPath(BasePath, SkinId, LogoId, ImageName));
        }

        /// <summary>
        /// Copia un logo all'interno di una skin
        /// </summary>
        /// <param name="SkinId">ID skin</param>
        /// <param name="BasePath">Percorso base</param>
        /// <param name="SourceLogoId">ID logo sorgente</param>
        /// <param name="DestLogoId">ID logo destinazione</param>
        /// <param name="SourceImageName">Nome immagine sorgente</param>
        /// <param name="DestImageName">Nome immagine destinazione</param>
        public static void CopyLogo(Int64 SkinId, String BasePath, Int64 SourceLogoId, Int64 DestLogoId, String SourceImageName, String DestImageName)
        { 
            String Source = GetLogoFullPath(BasePath, SkinId, SourceLogoId, SourceImageName);
            String Destination = GetLogoFullPath(BasePath, SkinId, DestLogoId, DestImageName);

            CopyAndReplace(Source, Destination);
        }
        /// <summary>
        /// Copia un logo da una skin all'altra
        /// </summary>
        /// <param name="SourceSkinId">ID Skin sorgente</param>
        /// <param name="BasePath">Percorso base</param>
        /// <param name="DestSkinID">ID Skin destinazione</param>
        /// <param name="SourceLogoId">ID Logo sorgente</param>
        /// <param name="DestLogoId">ID Logo destinazione</param>
        /// <param name="SourceImageName">Nome immagine sorgente</param>
        /// <param name="DestImageName">Nome immagine destinazione</param>
        public static void CopyLogoSkin(Int64 SourceSkinId, String BasePath, Int64 DestSkinID, Int64 SourceLogoId, Int64 DestLogoId, String SourceImageName, String DestImageName)
        {
            String Source = GetLogoFullPath(BasePath, SourceSkinId, SourceLogoId, SourceImageName);
            String Destination = GetLogoFullPath(BasePath, DestSkinID, DestLogoId, DestImageName);

            CopyAndReplace(Source, Destination);
        }
        // IO:1 <- OK
        /// <summary>
        /// Modifica l'ID di un logo
        /// </summary>
        /// <param name="SkinId">Id skin</param>
        /// <param name="BasePath">Percorso base</param>
        /// <param name="SourceLogoId">Id logo sorgente</param>
        /// <param name="DestLogoId">Id logo destinazione</param>
        /// <param name="ImageName">Nome immagine</param>
        /// <remarks>Teoricamente inutile, serve nel caso nHibernate cambi l'ID di un logo...</remarks>
        public static void UpdateLogoId(Int64 SkinId, String BasePath, Int64 SourceLogoId, Int64 DestLogoId, String ImageName)
        {
            String Source = GetLogoFullPath(BasePath, SkinId, SourceLogoId, ImageName);
            String Destination = GetLogoFullPath(BasePath, SkinId, DestLogoId, ImageName);

            try {
                Create.CopyFile(Source, Destination);
                Delete.File(Source);
                //File.Move(Source, Destination);
            }
            catch { }

        }
        #endregion

        #region Skin - COPY
        // E' necessario copiare man mano tutti i file,
        // visto che si basano sull'id di sottoggetti che vengono ricreati

        /// <summary>
        /// Crea una copia di un css di una skin
        /// </summary>
        /// <param name="SourceSkinID">ID Skin sorgente</param>
        /// <param name="BasePath">Percorso base</param>
        /// <param name="DestSkinId">ID Skin destinazione</param>
        /// <param name="Type">Tipo di CSS</param>
        public static void CopyCss(Int64 SourceSkinID, String BasePath, Int64 DestSkinId, CssType Type)
        {
            String Source = GetFullCssName(BasePath + "/" + SourceSkinID, Type, false);
            String Destination = GetFullCssName(BasePath + "/" + DestSkinId, Type, false);

            CopyAndReplace(Source, Destination);
        }
        /// <summary>
        /// Copia tutte le immagini di una skin
        /// </summary>
        /// <param name="SourceSkinId">ID Skin sorgente</param>
        /// <param name="BasePath">Percorso base</param>
        /// <param name="DestSkinId">ID Skin destinazione</param>
        /// <remarks>Viene copiata direttamente la directory e tutto il suo contenuto, visto che le varie immagini non sono salvate su bd, ma gestite direttamente su FileSystem.</remarks>
        public static void CopyImages(Int64 SourceSkinId, String BasePath, Int64 DestSkinId)
        {
            String SourcePath = GetImagePath(BasePath, SourceSkinId);
            String DestPath = GetImagePath(BasePath, DestSkinId);

            CopyAndReplaceDir(SourcePath, DestPath);
        }

        // IO: 3 <- OK
        /// <summary>
        /// Copia e sostituisce un file
        /// </summary>
        /// <param name="Source">Path completo sorgente</param>
        /// <param name="Destination">Path completo destinazione</param>
        /// <remarks>Puo' essere sostituita da una funzione analoga di LM.COMOL.FILE</remarks>
        private static void CopyAndReplace(String Source, String Destination)
        {
            Delete.File(Destination);
            Create.CopyFile(Source, Destination);
            //if (File.Exists(Destination))
            //{
            //    File.Delete(Destination);
            //}

            //try { File.Copy(Source, Destination); }
            //catch { }
        }

        // IO: 1 <- OK
        /// <summary>
        /// Copia e sostituisce una cartella
        /// </summary>
        /// <param name="SourceDir">Path cartella sorgente</param>
        /// <param name="DestDir">Path cartella destinazione</param>
        /// <remarks>Puo' essere sostituita da una funzione analoga di LM.COMOL.FILE</remarks>
        private static void CopyAndReplaceDir(String SourceDir, String DestDir)
        {
            String[] Files = new String[] {};

            lm.Comol.Core.File.ContentOf.Directory(SourceDir, ref Files, true);
            

            int DirChar = SourceDir.Count();

            foreach (string file in Files)
            {
                String FileName = file.Remove(0, DirChar);
                CopyAndReplace(SourceDir + FileName, DestDir + FileName);
            }
        }
        #endregion
    }
}