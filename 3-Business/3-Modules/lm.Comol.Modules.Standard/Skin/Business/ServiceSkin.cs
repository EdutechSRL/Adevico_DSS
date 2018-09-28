// Funzioni di MANAGEMENT delle skin e dei suoi sottoggetti (CRUD)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;

using Entity = Comol.Entity;
using lm.Comol.Modules.Standard.Skin.Domain;

using DocTemplate = lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport; //lm.Comol.Core.DomainModel.DocTemplate;

namespace lm.Comol.Modules.Standard.Skin.Business
{
    public class ServiceSkin
    {
        public const String UniqueCode = "SRVskins";
        private BaseManager Manager { get; set; }
        private iUserContext UC { get; set; }

        #region initClass
        public ServiceSkin() { }
        public ServiceSkin(iApplicationContext oContext)
        {
            this.Manager = new BaseManager(oContext.DataContext);
            //DC = oContext.DataContext;
            this.UC = oContext.UserContext;
            
        }
        public ServiceSkin(iDataContext oDC)
        {
            //DC = oDC;
            this.Manager = new BaseManager(oDC);
            this.UC = null;
        }

        #endregion

        #region SKIN - Edit/Add
        // Funzioni di recupero e salvataggio Skin,
        // utilizzata in Management

        /// <summary>
        /// Aggiunge una nuova skin, senza associazioni e con tutti i parametri di default.
        /// </summary>
        /// <param name="name">
        /// Il nome della nuova skin
        /// </param>
        /// <returns>
        /// L'ID della skin appena creata
        /// </returns>
        public Int64 AddNew(String name, String path)
        {
            return AddNew(name, path, false);
            //Int64 IdOut;

            //Domain.Skin NewSkin = new Domain.Skin();
            
            //Person user = Manager.Get<Person>(UC.CurrentUserID);
            //NewSkin.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
            
            //NewSkin.Name = SkinName;
            
            //try
            //{
            //    Manager.SaveOrUpdate<Domain.Skin>(NewSkin);
            //    IdOut = NewSkin.Id;
            //}
            //catch (Exception ex)
            //{
            //    IdOut = -1;
            //}

            //if (IdOut > 0)
            //{
            //    try
            //    {
            //        SkinFileManagement.CreateDir(IdOut, BasePath);
            //    }
            //    catch {
            //        DELETE_Skin(IdOut, BasePath);
            //        IdOut = -2;
            //    }
            //}

            //return IdOut;
        }

        /// <summary>
        /// Aggiunge una nuova skin, senza associazioni e con tutti i parametri di default.
        /// </summary>
        /// <param name="name">
        /// Il nome della nuova skin
        /// </param>
        /// <returns>
        /// L'ID della skin appena creata
        /// </returns>
        public Int64 AddNew(String name, String path,Boolean forModule)
        {
            Int64 idSkin = -1;
            try
            {
                Manager.BeginTransaction();
                Domain.Skin skin = new Domain.Skin();
                skin.IsModule = forModule;
                skin.Name = name;
                Person user = Manager.Get<Person>(UC.CurrentUserID);
                skin.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                Manager.SaveOrUpdate<Domain.Skin>(skin);
                Manager.Commit();
                idSkin = skin.Id;
                SkinFileManagement.CreateDir(idSkin, path);
            }
            catch (Exception ex) {
                Manager.RollBack();
                if (idSkin > 0) {
                    DELETE_Skin(idSkin, path);
                    idSkin = -2;
                }
            }
            return idSkin;
        }

        /// <summary>
        /// Recupera i dati di una Skin
        /// </summary>
        /// <param name="SkinId">L'ID di una skin</param>
        /// <returns>ID e Nome di una skin</returns>
        public Domain.DTO.DtoSkin GetSkinData(Int64 SkinId)
        {
            return Manager.Get<Domain.DTO.DtoSkin>(SkinId);
        }
        /// <summary>
        /// Recupera i CSS di una skin
        /// </summary>
        /// <param name="SkinId">L'Id della skin</param>
        /// <returns></returns>
        public Domain.DTO.DtoSkinCss GetSkinCss(Int64 SkinId)
        {
            return Manager.Get<Domain.DTO.DtoSkinCss>(SkinId);
        }

        /// <summary>
        /// Aggiorna la Skin con i nuovi dati
        /// </summary>
        /// <param name="Skin">
        /// La skin con i dati aggiornati.
        /// </param>
        public void UpdateBaseData(Domain.DTO.DtoSkin SkinData)
        {
            Domain.Skin OldSkin = Manager.Get<Domain.Skin>(SkinData.Id);
            if (OldSkin == null)
            {
                throw new NullReferenceException("Skin Id not found.");
            }

            Person user = Manager.Get<Person>(UC.CurrentUserID);
            OldSkin.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

            OldSkin.Name = SkinData.Name;
            OldSkin.OverrideVoidFooterLogos = SkinData.OverrideFootLogos;

            try
            {
                Manager.SaveOrUpdate<Domain.Skin>(OldSkin);
            }
            catch (Exception ex) { }
        }

        #endregion

        #region CSS
        // Gestione dei CSS di una Skin
        /// <summary>
        /// Aggiorna la Skin con i nuovi dati
        /// </summary>
        /// <param name="Skin">
        /// La skin con i dati aggiornati.
        /// </param>
        public void UpdateCSS(Domain.DTO.DtoSkinCss SkinCss)
        {
            Domain.Skin OldSkin = Manager.Get<Domain.Skin>(SkinCss.Id);
            if (OldSkin == null)
            {
                throw new NullReferenceException("Skin Id not found.");
            }

            Person user = Manager.Get<Person>(UC.CurrentUserID);
            OldSkin.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

            OldSkin.MainCss = SkinCss.MainCss;
            OldSkin.AdminCss = SkinCss.AdminCss;
            OldSkin.IECss = SkinCss.IeCss;
            OldSkin.LoginCss = SkinCss.LoginCss;

            try
            {
                Manager.SaveOrUpdate<Domain.Skin>(OldSkin);
            }
            catch (Exception ex) { }
        }
        /// <summary>
        /// Aggiunge un CSS di tipo MAIN
        /// </summary>
        /// <param name="SkinId">L'ID della skin</param>
        /// <param name="MainCss">Il nome del CSS assegnato</param>
        public void UpdateCssMain(Int64 SkinId, String MainCss)
        {
            Domain.Skin skin = Manager.Get<Domain.Skin>(SkinId);
            skin.MainCss = MainCss;

            Person user = Manager.Get<Person>(UC.CurrentUserID);
            skin.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

            try
            {
                Manager.SaveOrUpdate<Domain.Skin>(skin);
            }
            catch { }
        }
        /// <summary>
        /// Aggiunge un CSS di tipo Admin
        /// </summary>
        /// <param name="SkinId">L'ID della skin</param>
        /// <param name="MainCss">Il nome del CSS assegnato</param>
        public void UpdateCssAdmin(Int64 SkinId, String AdminCss)
        {
            Domain.Skin skin = Manager.Get<Domain.Skin>(SkinId);
            skin.AdminCss = AdminCss;

            Person user = Manager.Get<Person>(UC.CurrentUserID);
            skin.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

            try
            {
                Manager.SaveOrUpdate<Domain.Skin>(skin);
            }
            catch { }
        }
        /// <summary>
        /// Aggiunge un CSS per Internet Explorer
        /// </summary>
        /// <param name="SkinId">L'ID della skin</param>
        /// <param name="MainCss">Il nome del CSS assegnato</param>
        public void UpdateCssIE(Int64 SkinId, String IECss)
        {
            Domain.Skin skin = Manager.Get<Domain.Skin>(SkinId);
            skin.IECss = IECss;

            Person user = Manager.Get<Person>(UC.CurrentUserID);
            skin.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

            try
            {
                Manager.SaveOrUpdate<Domain.Skin>(skin);
            }
            catch { }
        }
        /// <summary>
        /// Aggiunge un CSS per la Login (Utilizzato solo se Skin di Portale)
        /// </summary>
        /// <param name="SkinId">L'ID della skin</param>
        /// <param name="MainCss">Il nome del CSS assegnato</param>
        public void UpdateCssLogin(Int64 SkinId, String LoginCss)
        {
            Domain.Skin skin = Manager.Get<Domain.Skin>(SkinId);
            skin.LoginCss = LoginCss;

            Person user = Manager.Get<Person>(UC.CurrentUserID);
            skin.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

            try
            {
                Manager.SaveOrUpdate<Domain.Skin>(skin);
            }
            catch { }
        }
        #endregion
        
        #region Template
        /// <summary>
        /// Recupera i template disponibili
        /// </summary>
        /// <returns>
        /// Un oggetto con due liste contenenti i template per header e footer
        /// </returns>
        public Domain.DTO.DtoSkinTemplates GetDtoTemplates()
        {
            Domain.DTO.DtoSkinTemplates Templates = new Domain.DTO.DtoSkinTemplates();

            Templates.HeaderTemplates = 
                (from Domain.DTO.DtoSkinTemplate Ht in Manager.GetAll<Domain.DTO.DtoSkinTemplate>(t => t.IsHeader == true) orderby Ht.Name select Ht).ToList();

            Templates.FooterTemplates = 
                (from Domain.DTO.DtoSkinTemplate Ft in Manager.GetAll<Domain.DTO.DtoSkinTemplate>(t => t.IsHeader == false) orderby Ft.Name select Ft).ToList();

            Templates.CurrentFooterTemplareID = 0;
            Templates.CurrentHeaderTemplareID = 0;

            return Templates;
        }

        /// <summary>
        /// Recupera i template disponibili
        /// </summary>
        /// <returns>
        /// Un oggetto con due liste contenenti i template per header e footer
        /// </returns>
        public Domain.DTO.DtoSkinTemplates GetDtoTemplates(Int64 SkinId)
        {
            Domain.DTO.DtoSkinTemplates Templates = GetDtoTemplates();

            if (SkinId > 0)
            {
                Domain.Skin skin = Manager.Get<Domain.Skin>(SkinId);
                
                if (skin.HeaderTemplate != null && skin.HeaderTemplate.Id > 0) { Templates.CurrentHeaderTemplareID = skin.HeaderTemplate.Id; }
                if (skin.FooterTemplate != null && skin.FooterTemplate.Id > 0) { Templates.CurrentFooterTemplareID = skin.FooterTemplate.Id; }
            }

            return Templates;
        }

        #endregion

        #region Header Logo
        // Gestione del Logo nell'Header

        /// <summary>
        /// Recupera il Logo dell'Header
        /// </summary>
        /// <param name="SkinId">L'ID della Skin</param>
        /// <returns>Il logo dell'Header associato alla skin</returns>
        public IList<Domain.DTO.DtoHeadLogoLang> GetDtoHederLogos(Int64 SkinId)
        {
            return 
                (from Domain.DTO.DtoSkinLanguage Lang in Manager.GetAll<Domain.DTO.DtoSkinLanguage>() orderby Lang.IsDefault descending
                 select (new Domain.DTO.DtoHeadLogoLang { 
                     Language = Lang, 
                     Logo = Manager.GetAll<Domain.HeaderLogo>( hl => hl.Skin.Id == SkinId && hl.LangCode == Lang.LangCode).FirstOrDefault()
                 })).ToList();
        }

        /// <summary>
        /// Rimuove un logo dell'Header
        /// </summary>
        /// <param name="SkinId">ID della skin a cui appartiene</param>
        /// <param name="Logo">I dati relativi al logo</param>
        public void DelHeaderLogo(Int64 LogoId, String BasePath, Int64 SkinId)
        {
            Domain.HeaderLogo HdLogo = Manager.Get<Domain.HeaderLogo>(LogoId);

            if (HdLogo == null)
                return;

            try
            {
                Business.SkinFileManagement.DeleteLogo(SkinId, BasePath, LogoId, HdLogo.ImageUrl);
                Manager.DeletePhysical<Domain.HeaderLogo>(HdLogo);
            }
            catch {
            
            }
            
            
        }

        /// <summary>
        /// Salva(Update)/Aggiunge un Logo HEader
        /// </summary>
        /// <param name="LogoId">L'ID della Skin</param>
        /// <param name="ImageName">Il nome dell'immagine</param>
        /// <param name="Link">Il link associato</param>
        /// <param name="Alt">Il testo alternativo dell'immagine</param>
        /// <param name="SkinId">L'ID della Skin associata</param>
        /// <param name="LangCode">Il codice della lingua associata</param>
        /// <param name="BasePath">Il percorso base (fisico) del Logo</param>
        /// <returns>L'ID del logo</returns>
        public Int64 SaveHeadLogo(Int64 LogoId, String ImageName, String Link, String Alt, Int64 SkinId, String LangCode, String BasePath)
        {
            String OldImageName;
            Domain.HeaderLogo HeadLogo;
            Person user = Manager.Get<Person>(UC.CurrentUserID);

            if (LogoId > 0) // Aggiornamento
            {
                HeadLogo = Manager.Get<Domain.HeaderLogo>(LogoId);
                OldImageName = HeadLogo.ImageUrl;
                HeadLogo.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

                if (OldImageName != "" && ImageName !="")
                {
                    //Se necessario cancello la vecchia immagine
                    SkinFileManagement.DeleteLogo(SkinId, BasePath, LogoId, OldImageName);
                }

            }
            else {  // Creazione
                HeadLogo =  new Domain.HeaderLogo();
                HeadLogo.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                HeadLogo.Skin = Manager.Get<Domain.Skin>(SkinId);
                OldImageName = "";
            }

            if (ImageName != "")
            {
                HeadLogo.ImageUrl = ImageName;
            }

            HeadLogo.LangCode = LangCode;
            HeadLogo.Alt = Alt;
            HeadLogo.Link = Link;

            Manager.SaveOrUpdate<Domain.HeaderLogo>(HeadLogo);

            return HeadLogo.Id;

        }

        /// <summary>
        /// Crea una copia del logo Header
        /// </summary>
        /// <param name="SkinId">L'ID della skin</param>
        /// <param name="SourceLogoId">L'ID del logo sorgente</param>
        /// <param name="DestLogoId">L'ID del logo destinazione</param>
        /// <param name="BasePath">Il percorso fisico di base</param>
        /// <param name="DestLangCode">La lingua del logo Destinazione</param>
        public void CloneLogo(Int64 SkinId, Int64 SourceLogoId, Int64 DestLogoId, String BasePath, String DestLangCode)
        {
            String OldImageName;
            Domain.HeaderLogo SourceHeadLogo;
            Domain.HeaderLogo DestHeadLogo;

            Person user = Manager.Get<Person>(UC.CurrentUserID);

            SourceHeadLogo = Manager.Get<Domain.HeaderLogo>(SourceLogoId);

            if (DestLogoId > 0) // Se il logo di desitnazione esiste ne aggiorno i dati
            {
                DestHeadLogo = Manager.Get<Domain.HeaderLogo>(DestLogoId);
                OldImageName = DestHeadLogo.ImageUrl;
                DestHeadLogo.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

                if (OldImageName != "" && OldImageName != SourceHeadLogo.ImageUrl)
                {   //Se necessatio cancello i dati
                    SkinFileManagement.DeleteLogo(SkinId, BasePath, DestLogoId, OldImageName);
                }
            }
            else
            {   //Altrimenti lo creo nuovo
                DestHeadLogo = new Domain.HeaderLogo();
                DestHeadLogo.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                DestHeadLogo.Skin = Manager.Get<Domain.Skin>(SkinId);
            }

            DestHeadLogo.LangCode = DestLangCode;
            DestHeadLogo.Alt = SourceHeadLogo.Alt;
            DestHeadLogo.Link = SourceHeadLogo.Link;
            DestHeadLogo.ImageUrl = SourceHeadLogo.ImageUrl;

            Manager.SaveOrUpdate<Domain.HeaderLogo>(DestHeadLogo);

            SkinFileManagement.CopyLogo(SkinId, BasePath, SourceLogoId, DestHeadLogo.Id, SourceHeadLogo.ImageUrl, DestHeadLogo.ImageUrl);
        }

        #endregion

        #region Footer Logos
        //Management dei loghi Footer

        /// <summary>
        /// Recupera info base di un logo Footer
        /// </summary>
        /// <param name="SkinID">L'ID della skin</param>
        /// <returns>
        /// Un DTO con l'elenco deli loghi e di tutte le lingue disponibili
        /// </returns>
        public Domain.DTO.DtoFooterLogosList GetFooterLogosDto(Int64 SkinID)
        {
            Domain.DTO.DtoFooterLogosList FtLogos = new Domain.DTO.DtoFooterLogosList();
            FtLogos.Languages = Manager.GetAll<Domain.DTO.DtoSkinLanguage>();
            FtLogos.Logos = Manager.GetAll<Domain.FooterLogo>(ftl => ftl.Skin.Id == SkinID);

            return FtLogos;
        }

        /// <summary>
        /// Crea un nuovo logo Footer
        /// </summary>
        /// <param name="SkinId">L'ID della skin a cui appartiene</param>
        /// <param name="ImageName">Il nome dell'immagine</param>
        /// <param name="Link">Il link associato</param>
        /// <param name="Alt">Il testo alternativo dell'immagine</param>
        /// <param name="AssLang">Elenco delle lingue associate</param>
        /// <param name="DisplayOrder">NON USATO - L'ordine di visualizzazione</param>
        /// <returns>L'ID del nuovo logo appena creato</returns>
        public Int64 CreateNewFooterLogo(Int64 SkinId, String ImageName, String Link, String Alt, IList<String> AssLang, int DisplayOrder)
        {
            Domain.FooterLogo NewLogo = new Domain.FooterLogo();
            Person user = Manager.Get<Person>(UC.CurrentUserID);
            NewLogo.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
            NewLogo.Skin = Manager.Get<Domain.Skin>(SkinId);

            NewLogo.Alt = Alt;
            NewLogo.DisplayOrder = 0;
            NewLogo.ImageUrl = ImageName;
            NewLogo.Link = Link;

            NewLogo.Languages = new List<Domain.LogoToLang>();

            if (AssLang != null && AssLang.Count > 0)
            {
                foreach (String Lngcode in AssLang)
                {
                    Domain.LogoToLang LtL = new Domain.LogoToLang();
                    LtL.LangCode = Lngcode;
                    LtL.Logo = NewLogo;
                    
                    NewLogo.Languages.Add(LtL);
                }
            }

            Manager.SaveOrUpdate<Domain.FooterLogo>(NewLogo);

            return NewLogo.Id;
        }

        /// <summary>
        /// Elimina tutti i riferimenti alle lingue di un logo Footer
        /// </summary>
        /// <param name="LogoId">L'ID del logo</param>
        private void ClearLangLogo(Int64 LogoId)
        {
            IList<Domain.LogoToLang> LtL = Manager.GetAll<Domain.LogoToLang>(ll => ll.Logo.Id == LogoId);
            Manager.DeletePhysicalList<Domain.LogoToLang>(LtL);
            Manager.Commit();
        }

        /// <summary>
        /// Aggiorna il nome dell'immagine di un logo Footer
        /// </summary>
        /// <param name="LogoId">L'Id del logo</param>
        /// <param name="LogoName">Il nome della nuova immagine</param>
        public void UpdateFooterLogoName(Int64 LogoId, String LogoName)
        {
            Domain.FooterLogo Logo = Manager.Get<Domain.FooterLogo>(LogoId);
            Person user = Manager.Get<Person>(UC.CurrentUserID);

            Logo.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
            Logo.ImageUrl = LogoName;

            Manager.SaveOrUpdate<Domain.FooterLogo>(Logo);
        }

        /// <summary>
        /// Aggiorna tutti i campi di un determinato logo
        /// </summary>
        /// <param name="SkinId">ID della skin a cui appartiene</param>
        /// <param name="LogoId">ID del logo da modificare</param>
        /// <param name="ImageName">Nome originale immagine</param>
        /// <param name="Link">Link da applicare all'immagine</param>
        /// <param name="Alt">Alt dell'immagine</param>
        /// <param name="AssLang">Codici Lingue associate</param>
        /// <param name="DisplayOrder">Ordine display (not in use)</param>
        /// <param name="BasePath">Path base delle immagini delle skin</param>
        /// <remarks>
        /// Al momento, quando si cancellano tutte le lingue per ricrearle con quelle corrette,
        /// il logo viene cancellato e ricreato, modificandone così l'ID...
        /// Intanto viene rinominata l'immagine, eventualmente rivedere.
        /// </remarks>
        public void UpdateFooterLogo(Int64 SkinId, Int64 LogoId, String ImageName, String Link, String Alt, IList<String> AssLang, int DisplayOrder, String BasePath, bool OverrideVoidFooterLogos)
        {

            Manager.BeginTransaction();
            try
            {
                string OldImageName = "";
                Domain.FooterLogo Logo = Manager.Get<Domain.FooterLogo>(LogoId);
                Person user = Manager.Get<Person>(UC.CurrentUserID);
                Logo.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

                Logo.Alt = Alt;
                Logo.DisplayOrder = 0;

                OldImageName = Logo.ImageUrl;

                if (ImageName != "")
                {
                    Logo.ImageUrl = ImageName;
                }

                Logo.Link = Link;
                Logo.Languages.Clear();

                foreach (string code in AssLang)
                {
                    Domain.LogoToLang ltl = new Domain.LogoToLang();
                    ltl.LangCode = code;
                    ltl.Logo = Logo;
                    Logo.Languages.Add(ltl);
                }

                Logo.Skin.OverrideVoidFooterLogos = OverrideVoidFooterLogos;

                Manager.SaveOrUpdate<Domain.FooterLogo>(Logo);
                Manager.Commit();

                //Se è cambioto il nome dell'immagine, cancello quella precedente.
                if (OldImageName != "" && ImageName != "")
                {
                    SkinFileManagement.DeleteLogo(SkinId, BasePath, LogoId, OldImageName);
                }

            }
            catch {
                Manager.RollBack();
            }

        }

        /// <summary>
        /// Cancella un Logo Footer, tutte le sue immagini ed i collegamenti con le lingue
        /// </summary>
        /// <param name="LogoID">L'ID del logo da cancellare</param>
        public void DeleteFooterLogo(Int64 LogoID, Int64 SkinID, String BasePath)
        {
            Domain.FooterLogo FtLogo = Manager.Get<Domain.FooterLogo>(LogoID);
            String ImageName = FtLogo.ImageUrl;
            Manager.DeletePhysical<Domain.FooterLogo>(FtLogo);

            Business.SkinFileManagement.DeleteLogo(SkinID, BasePath, LogoID, ImageName);
        }
        #endregion

        #region Footer Text
        //Gestione dei testi del footer

        /// <summary>
        /// Recupera i Testi associati al Footer
        /// </summary>
        /// <param name="SkinId">L'ID della skin</param>
        /// <returns>Un elenco di DTO con i testi associati al Footer</returns>
        public IList<Domain.DTO.DtoSkinFooterText> GetFooterTexts(Int64 SkinId)
        {
            IList<Domain.DTO.DtoSkinFooterText> DtoFtText = new List<Domain.DTO.DtoSkinFooterText>();

            IList<Domain.DTO.DtoSkinLanguage> Langs = (from Domain.DTO.DtoSkinLanguage Lang in Manager.GetAll<Domain.DTO.DtoSkinLanguage>() orderby Lang.IsDefault descending select Lang).ToList();

            IList<Domain.FooterText> Text = Manager.GetAll<Domain.FooterText>(ft => ft.Skin.Id == SkinId).ToList();

            foreach (Domain.DTO.DtoSkinLanguage skl in Langs)
            {
                Domain.DTO.DtoSkinFooterText dtoSFT= new Domain.DTO.DtoSkinFooterText();
                dtoSFT.Id = -1;
                dtoSFT.LangCode = skl.LangCode;
                dtoSFT.LangName = skl.LangName;
                dtoSFT.IsDefault = skl.IsDefault;
                dtoSFT.SkinId = SkinId;
                dtoSFT.Text = "";

                foreach (Domain.FooterText txt in Text)
                {
                    if (txt.LangCode == skl.LangCode)
                    {
                        dtoSFT.Id = txt.Id;
                        dtoSFT.Text = txt.Value;
                        break;
                    }
                }
                DtoFtText.Add(dtoSFT);
            }
            return DtoFtText;
        }

        /// <summary>
        /// Aggiunge o modifica il testo del footer.
        /// Se il testo è vuoto, lo cancella.
        /// </summary>
        /// <param name="SkinId">ID della skin a cui appartiene</param>
        /// <param name="LangCode">Codice lingua associata</param>
        /// <param name="Text">Testo</param>
        public void UpdateText(Int64 SkinId, String LangCode, String Text)
        {
            Domain.FooterText FtText = Manager.GetAll<Domain.FooterText>(ft => ft.Skin.Id == SkinId && ft.LangCode == LangCode).FirstOrDefault();
            Person user = Manager.Get<Person>(UC.CurrentUserID);

            if (Text != "")
            {
                if (FtText == null)
                {
                    FtText = new Domain.FooterText();
                    FtText.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                    FtText.LangCode = LangCode;
                    FtText.Skin = Manager.Get<Domain.Skin>(SkinId);
                }
                else
                {
                    FtText.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                }

                FtText.Value = Text;

                Manager.SaveOrUpdate<Domain.FooterText>(FtText);
            }
            else
            {
                if (FtText != null)
                {
                    Manager.DeletePhysical<Domain.FooterText>(FtText);
                }
            }
        }

        public void UpdateTemplates(Int64 SkinId, Int64 HeaderTemplateId, Int64 FooterTemplateId)
        {
            Person user = Manager.Get<Person>(UC.CurrentUserID);

            Domain.Skin Skin = Manager.Get<Domain.Skin>(SkinId);
            Skin.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

            Skin.HeaderTemplate = Manager.Get<Domain.HeaderTemplate>(HeaderTemplateId);
            Skin.FooterTemplate = Manager.Get<Domain.FooterTemplate>(FooterTemplateId);

            Manager.SaveOrUpdate<Domain.Skin>(Skin);
        }

        #endregion

        #region Share
        /// <summary>
        /// Associa una Skin al Portale
        /// </summary>
        /// <param name="SkinId">L'ID della skin</param>
        /// <remarks>Verrà tolta l'associazione ad eventuali altre Skin associate al portale.</remarks>
        public void SetPortal(Int64 SkinId)
        {
            Person user = Manager.Get<Person>(UC.CurrentUserID);
            IList<Domain.Skin> Skins = Manager.GetAll<Domain.Skin>(s => s.IsPortal == true).ToList();

            if (Skins != null && Skins.Count > 0)
            {
                foreach (Domain.Skin Skin in Skins)
                {
                    Skin.IsPortal = false;
                    Manager.SaveOrUpdate<Domain.Skin>(Skin);
                }
            }

            Domain.Skin PortalSkin = Manager.Get<Domain.Skin>(SkinId);
            PortalSkin.IsPortal = true;
            PortalSkin.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
            Manager.SaveOrUpdate<Domain.Skin>(PortalSkin);
        }
        /// <summary>
        /// Rimuove l'associazione di una Skin al portale.
        /// </summary>
        /// <param name="SkinId">L'ID della skin</param>
        public void RemPortal(Int64 SkinId)
        {
            Person user = Manager.Get<Person>(UC.CurrentUserID);
            Domain.Skin PortalSkin = Manager.Get<Domain.Skin>(SkinId);
            PortalSkin.IsPortal = false;
            PortalSkin.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
            Manager.SaveOrUpdate<Domain.Skin>(PortalSkin);
        }
        /// <summary>
        /// Recupera l'elenco delle organizzazione associate ad una Skin
        /// </summary>
        /// <param name="SkinId">L'ID della skin</param>
        /// <returns>Un elenco di DTO con i dati delle Organizzazioni associate ad una Skin</returns>
        public IList<Domain.DTO.DtoSkinOrganization> GetOrganizationList(Int64 SkinId)
        {
            IList<Domain.DTO.DtoSkinOrganization> SkinOrgn = Manager.GetAll<Domain.DTO.DtoSkinOrganization>();

            if (SkinId > 0)
            {
                IList<Int32> SelectedOrgnId = (from Domain.Skin_ShareOrganization SSO in Manager.GetAll<Domain.Skin_ShareOrganization>(so => so.Skin.Id == SkinId) select SSO.OrganizationId).ToList();

                foreach (Domain.DTO.DtoSkinOrganization SSO in SkinOrgn)
                {
                    if (SelectedOrgnId.Contains(SSO.Id)) SSO.IsChecked = true;
                }

            }

            return SkinOrgn;
        }
        /// <summary>
        /// Recupera l'elenco di ID di Comunità associate ad una Skin
        /// </summary>
        /// <param name="SkinId">L'ID della Skin</param>
        /// <returns>Una lista di ID</returns>
        public IList<Int32> GetCommunitiesId(Int64 SkinId)
        {
            return (
                from Domain.Skin_ShareCommunity ssc in Manager.GetIQ<Domain.Skin_ShareCommunity>()
                where ssc.Skin.Id == SkinId
                select ssc.CommunityId).ToList();
        }
        /// <summary>
        /// Imposta l'elenco di Organizzazioni associate
        /// </summary>
        /// <param name="SkinId">L'ID della Skin</param>
        /// <param name="OrgnIds">L'elenco di ID da associare</param>
        /// <remarks>Le associazioni precedenti vengono eliminate</remarks>
        public void SetOrganizationAss(Int64 SkinId, IList<Int32> OrgnIds)
        {
            //Elimino le precedenti relazioni con altre skin
            RemOrgnAss(OrgnIds);


            Person user = Manager.Get<Person>(UC.CurrentUserID);

            IList<Domain.Skin_ShareOrganization> SSOrgs = (Manager.GetAll<Domain.Skin_ShareOrganization>(sso => sso.Skin.Id == SkinId)).ToList();

            Manager.DeletePhysicalList<Domain.Skin_ShareOrganization>(SSOrgs);

            Domain.Skin Skin = Manager.Get<Domain.Skin>(SkinId);

            IList<Domain.Skin_ShareOrganization> NewSsoList = new List<Domain.Skin_ShareOrganization>();
            foreach (Int32 OrgnId in OrgnIds)
            {
                Domain.Skin_ShareOrganization NewSso = new Domain.Skin_ShareOrganization();
                NewSso.OrganizationId = OrgnId;
                NewSso.Skin = Skin;
                NewSso.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                NewSsoList.Add(NewSso);
            }

            Manager.SaveOrUpdateList<Domain.Skin_ShareOrganization>(NewSsoList);

        }
        /// <summary>
        /// Imposta l'elenco di Comunità associate
        /// </summary>
        /// <param name="SkinId">L'ID della Skin</param>
        /// <param name="ComIds">Gli ID della comunità da associare</param>
        /// <remarks>Le associazioni precedenti vengono eliminate</remarks>
        public void SetCommunitiesAss(Int64 SkinId, IList<Int32> ComIds)
        {
            //Elimino le precedenti relazioni con altre skin
            //RemComAss(ComIds);

            Person user = Manager.Get<Person>(UC.CurrentUserID);

            //IList<Int32> PrevComsIds = (from Domain.Skin_ShareCommunity ssc
            //                           in Manager.GetIQ<Domain.Skin_ShareCommunity>()
            //                            where ssc.Skin.Id == SkinId && ComIds.Contains(ssc.CommunityId)
            //                            select ssc.CommunityId).Distinct().ToList();
                
            //(Manager.GetAll<Domain.Skin_ShareCommunity>(
            //ssc => ssc.Skin.Id == SkinId && ComIds.Contains(ssc.CommunityId))
            //);

            
            ComIds = ComIds.Except(
                    (from Domain.Skin_ShareCommunity ssc
                        in Manager.GetIQ<Domain.Skin_ShareCommunity>()
                        where ssc.Skin.Id == SkinId && ComIds.Contains(ssc.CommunityId)
                        select ssc.CommunityId).Distinct()                    
                ).ToList();

            //Manager.DeletePhysicalList<Domain.Skin_ShareCommunity>(SSComs);
            
            Domain.Skin Skin = Manager.Get<Domain.Skin>(SkinId);

            IList<Domain.Skin_ShareCommunity> NewSscList = new List<Domain.Skin_ShareCommunity>();

            foreach (Int32 ComId in ComIds)
            {
                Domain.Skin_ShareCommunity NewSsc = new Domain.Skin_ShareCommunity();
                NewSsc.CommunityId = ComId;
                NewSsc.Skin = Skin;
                NewSsc.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                NewSscList.Add(NewSsc);
            }

            Manager.SaveOrUpdateList<Domain.Skin_ShareCommunity>(NewSscList);
        }
        /// <summary>
        /// Rimuove una singola associazione con un'Organizzazione
        /// </summary>
        /// <param name="SkinId">L'ID della skin relativa</param>
        /// <param name="OrganizationId">L'ID dell'organizzazione relativa</param>
        public void RemOrgnAss(Int64 SkinId, Int32 OrganizationId)
        {
            Domain.DTO.DtoShareOrganization DSO = Manager.GetAll<Domain.DTO.DtoShareOrganization>(dso => dso.OrganizationId == OrganizationId && dso.SkinId == SkinId).FirstOrDefault();
            if (DSO != null)
            {
                Manager.DeletePhysical<Domain.DTO.DtoShareOrganization>(DSO);
            }

        }
        /// <summary>
        /// Rimuove una singola associazione con una Comunità
        /// </summary>
        /// <param name="SkinId">L'ID della skin relativa</param>
        /// <param name="CommunityId">L'ID della comunità relativa</param>
        public void RemComAss(Int64 SkinId, Int32 CommunityId)
        {
            Domain.DTO.DtoShareCommunity DSC = Manager.GetAll<Domain.DTO.DtoShareCommunity>(dsc => dsc.CommunityId == CommunityId && dsc.SkinId == SkinId).FirstOrDefault();
            if (DSC != null)
            {
                Manager.DeletePhysical<Domain.DTO.DtoShareCommunity>(DSC);
            }

        }

        /// <summary>
        /// Rimuove tutte le associazione con una lista di Organizzazioni
        /// </summary>
        /// <param name="SkinId">L'ID della skin relativa</param>
        /// <param name="OrganizationsId">Gli ID delle organizzazione di cui eliminare le relazioni</param>
        public void RemOrgnAss(IList<Int32> OrganizationsId)
        {
            IList<Domain.DTO.DtoShareOrganization> DSOs = Manager.GetAll<Domain.DTO.DtoShareOrganization>
                (dso => OrganizationsId.Contains(dso.OrganizationId))
                .ToList();
            if (DSOs != null && DSOs.Count() > 0)
            {
                Manager.DeletePhysicalList<Domain.DTO.DtoShareOrganization>(DSOs);
            }
        }

        /// <summary>
        /// Rimuove tutte le associazione con una lista di Comunità
        /// </summary>
        /// <param name="SkinId">L'ID della skin relativa</param>
        /// <param name="CommunitiesId">Gli ID delle comunità di cui eliminare le relazioni</param>
        public void RemComAss(IList<Int32> CommunitiesId)
        {
            IList<Domain.DTO.DtoShareCommunity> DSCs = Manager.GetAll<Domain.DTO.DtoShareCommunity>
                (dso => CommunitiesId.Contains(dso.CommunityId))
                .ToList();
            if (DSCs != null && DSCs.Count() > 0)
            {
                Manager.DeletePhysicalList<Domain.DTO.DtoShareCommunity>(DSCs);
            }
        }

        /// <summary>
        /// Recupera un DTO contenente TUTTE le associazioni di una Skin
        /// </summary>
        /// <param name="SkinId">L'ID della Skin</param>
        /// <returns>Un DTO con tutte le associazioni e relativi dati di una Skin</returns>
        public Domain.DTO.DtoSkinShares GetAllShares(Int64 SkinId)
        {
           
            Domain.DTO.DtoSkinShares DTOSkinShare = new Domain.DTO.DtoSkinShares();
           
            IList<Domain.Skin_ShareOrganization> Orgns = Manager.GetAll<Domain.Skin_ShareOrganization>(sso => sso.Skin.Id == SkinId);
            if (Orgns != null && Orgns.Count > 0)
            {           
                IList<Int32> OrganizationsId = (from Domain.Skin_ShareOrganization SSO in Orgns select SSO.OrganizationId).ToList();
                        DTOSkinShare.Organizations = (from Domain.DTO.DtoSkinOrganization SSO 
                                                        in Manager.GetIQ<Domain.DTO.DtoSkinOrganization>()
                                                      where OrganizationsId.Contains(SSO.Id)
                                                      select new Domain.DTO.DtoSkinShareItem { Id = SSO.Id, Name = SSO.Name }
                                                      ).ToList();
            }
            else
            {
                DTOSkinShare.Organizations = new List<Domain.DTO.DtoSkinShareItem>();
            }
           
            IList<Domain.Skin_ShareCommunity> Comm = Manager.GetAll<Domain.Skin_ShareCommunity>(ssc => ssc.Skin.Id == SkinId);
            if (Comm != null && Comm.Count > 0)
            {
                IList<Int32> CommunitiesId = (from Domain.Skin_ShareCommunity SSC in Comm select SSC.CommunityId).ToList();
                 
                DTOSkinShare.Communities = (from Domain.DTO.DtoSkinCommunity SSC 
                                                in Manager.GetIQ<Domain.DTO.DtoSkinCommunity>()
                                            where CommunitiesId.Contains(SSC.Id)
                                            select new Domain.DTO.DtoSkinShareItem { Id = SSC.Id, Name = SSC.Name }).ToList();
            }
            else
            {
                DTOSkinShare.Communities = new List<Domain.DTO.DtoSkinShareItem>();
            }

            Domain.Skin Skin = Manager.Get<Domain.Skin>(SkinId);
            DTOSkinShare.IsPortal = Skin.IsPortal;

            return DTOSkinShare;
        }

        #endregion

        #region Skin LIST
        /// <summary>
        /// Recupera un elenco di Skin a seconda del tipo
        /// </summary>
        /// <param name="Type">Tipo di skin richiesto</param>
        /// <returns>Elenco di Skin con tutti i relativi dati</returns>
        public IList<Domain.DTO.DtoSkinList> GetSkinsList(Domain.SkinShareType Type)
        {
            IList<Domain.Skin> Skins = new List<Domain.Skin>();

            switch (Type)
            {
                case Domain.SkinShareType.All:
                    Skins = Manager.GetAll<Domain.Skin>(sk => sk.IsModule == false);
                    break;

                case Domain.SkinShareType.NotAssociate:
                    Skins = Manager.GetAll<Domain.Skin>(sk => ((sk.IsModule == false) && (sk.IsPortal == false) && (sk.Organizations.Count() == 0) && (sk.Communities.Count() == 0)));
                    break;

                case Domain.SkinShareType.Portal:
                    Skins = Manager.GetAll<Domain.Skin>(sk => (sk.IsModule == false) && (sk.IsPortal == true));
                    break;

                case Domain.SkinShareType.Organization:
                    Skins = Manager.GetAll<Domain.Skin>(sk => (sk.IsModule == false) && (sk.Organizations.Count() > 0));
                    break;

                case Domain.SkinShareType.Community:
                    Skins = Manager.GetAll<Domain.Skin>(sk => (sk.IsModule == false) && (sk.Communities.Count() > 0));
                    break;
            }

            IList<Domain.DTO.DtoSkinList> SkinsDto = new List<Domain.DTO.DtoSkinList>();

            if (Skins != null && Skins.Count > 0)
            { 
                foreach (Domain.Skin oSkin in Skins)
                {
                    Domain.DTO.DtoSkinList SkinDto = new Domain.DTO.DtoSkinList();
                    SkinDto.Id = oSkin.Id;
                    SkinDto.IsPortal = oSkin.IsPortal;
                    SkinDto.Name = oSkin.Name;


                    SkinDto.Communities = new List<Domain.DTO.DtoSkinShareItem>();

                    if (oSkin.Communities != null && oSkin.Communities.Count > 0)
                    {
                        IList<int> ComIds = (from Domain.Skin_ShareCommunity ssc in oSkin.Communities select ssc.CommunityId).ToList();

                        SkinDto.Communities = (from Domain.DTO.DtoSkinCommunity DSC
                                                 in Manager.GetIQ<Domain.DTO.DtoSkinCommunity>()
                                                 where ComIds.Contains(DSC.Id)
                                                 select new Domain.DTO.DtoSkinShareItem { Name = DSC.Name, Id = DSC.Id }).ToList();
                    }




                    SkinDto.Organizations = new List<Domain.DTO.DtoSkinShareItem>();

                    if (oSkin.Organizations != null && oSkin.Organizations.Count > 0)
                    {
                        IList<int> OrgnIds = (from Domain.Skin_ShareOrganization sso in oSkin.Organizations select sso.OrganizationId).ToList();

                        SkinDto.Organizations = (from Domain.DTO.DtoSkinOrganization DSO
                                                 in Manager.GetIQ<Domain.DTO.DtoSkinOrganization>()
                                                 where OrgnIds.Contains(DSO.Id)
                                                 select new Domain.DTO.DtoSkinShareItem { Name = DSO.Name, Id = DSO.Id }).ToList();
                    }




                    SkinsDto.Add(SkinDto);
                }
            
            }


            return SkinsDto;
                   
        }

        /// <summary>
        /// Cancella una Skin
        /// </summary>
        /// <param name="SkinID">L'Id della Skin</param>
        /// <param name="BasePath">Il percorso base delle skin</param>
        /// <remarks>Vengono cancellati tutti i dati, i sotto-oggetti ed i file di una skin</remarks>
        public Boolean DELETE_Skin(Int64 SkinID, String BasePath)
        {
            Boolean result = false;
            try
            {
                SkinFileManagement.EraseDir(SkinID, BasePath);

                Domain.Skin Skin = Manager.Get<Domain.Skin>(SkinID);

                Manager.DeletePhysical<Domain.Skin>(Skin);
                result = true;
            }
            catch(Exception ex){
                result=false;
            }
           return result;
        }
        /// <summary>
        /// Crea una copia di una Skin
        /// </summary>
        /// <param name="SourceSkinID">L'ID della skin da copiare</param>
        /// <param name="BasePath">Il path base delle skin</param>
        /// <returns>L'ID della skin appena creata</returns>
        /// <remarks>La nuova skin avrà gli stessi dati ed una copia delle immagine e dei file della Skin sorgente.
        /// NON vengono copiate le associazioni!
        /// </remarks>
        public Int64 CopySkin(Int64 SourceSkinID, String BasePath)
        {
            Domain.Skin SourceSkin = Manager.Get<Domain.Skin>(SourceSkinID);
            if (SourceSkin == null)
            { return -1; }

            Domain.Skin NewSkin = new Domain.Skin();
            Person user = Manager.Get<Person>(UC.CurrentUserID);
            NewSkin.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

            //CSS
            NewSkin.MainCss = SourceSkin.MainCss;
            NewSkin.AdminCss = SourceSkin.AdminCss;
            NewSkin.IECss = SourceSkin.IECss;
            NewSkin.LoginCss = SourceSkin.LoginCss;

            NewSkin.Name = SourceSkin.Name + "_Copy";

            Manager.SaveOrUpdate<Domain.Skin>(NewSkin);
            SkinFileManagement.CreateDir(NewSkin.Id, BasePath);

            /* TUTTI I COLLEGAMENTE non saranno presenti nella nuova skin */
            NewSkin.Organizations = new List<Domain.Skin_ShareOrganization>();
            NewSkin.Communities = new List<Domain.Skin_ShareCommunity>();
            NewSkin.IsPortal = false;


            // FOOTER TEXT
            NewSkin.FooterText = new List<Domain.FooterText>();

            foreach(Domain.FooterText FtText in SourceSkin.FooterText)
            {
                Domain.FooterText NewFtText = new Domain.FooterText();
                NewFtText.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                NewFtText.LangCode = FtText.LangCode;
                NewFtText.Skin = NewSkin;
                NewFtText.Value = FtText.Value;

                NewSkin.FooterText.Add(NewFtText);
            }

            // Loghi Header
            NewSkin.HeaderLogos = new List<Domain.HeaderLogo>();
            foreach (Domain.HeaderLogo SourceHL in SourceSkin.HeaderLogos)
            {
                Domain.HeaderLogo HL = new Domain.HeaderLogo();
                HL.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

                HL.Alt = SourceHL.Alt;
                HL.ImageUrl = SourceHL.ImageUrl;
                HL.LangCode = SourceHL.LangCode;
                HL.Link = SourceHL.Link;
                HL.Skin = NewSkin;

                NewSkin.HeaderLogos.Add(HL);

                Manager.SaveOrUpdate<Domain.HeaderLogo>(HL);
                
                //FILE
                SkinFileManagement.CopyLogoSkin(SourceSkinID, BasePath, NewSkin.Id, SourceHL.Id, HL.Id, SourceHL.ImageUrl, HL.ImageUrl);
            }
            
            // Loghi Footer
            NewSkin.FooterLogos = new List<Domain.FooterLogo>();
            foreach (Domain.FooterLogo SourceFL in SourceSkin.FooterLogos)
            {
                Domain.FooterLogo FL = new Domain.FooterLogo();
                FL.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

                FL.Alt = SourceFL.Alt;
                FL.ImageUrl = SourceFL.ImageUrl;
                FL.Link = SourceFL.Link;
                FL.Skin = NewSkin;

                FL.Languages = new List<Domain.LogoToLang>();

                foreach (Domain.LogoToLang SourceLtL in SourceFL.Languages)
                {
                    Domain.LogoToLang Ltl = new Domain.LogoToLang();
                    Ltl.LangCode = SourceLtL.LangCode;
                    Ltl.Logo = FL;

                    FL.Languages.Add(Ltl);
                }

                Manager.SaveOrUpdate<Domain.FooterLogo>(FL);


                //FILE
                SkinFileManagement.CopyLogoSkin(SourceSkinID, BasePath, NewSkin.Id, SourceFL.Id, FL.Id, SourceFL.ImageUrl, FL.ImageUrl);

                NewSkin.FooterLogos.Add(FL);


            }

            NewSkin.OverrideVoidFooterLogos = SourceSkin.OverrideVoidFooterLogos;

            //ModuleSkin
            if (SourceSkin.FooterTemplate != null)
            {
                NewSkin.FooterTemplate = SourceSkin.FooterTemplate;
            }
            if (SourceSkin.HeaderTemplate != null)
            {
                NewSkin.HeaderTemplate = SourceSkin.HeaderTemplate;
            }

            Manager.SaveOrUpdate<Domain.Skin>(NewSkin);
            

            /* COPIA FISICA DEI FILE */
            /* CSS */
            if (SourceSkin.MainCss != "")
            {
                Business.SkinFileManagement.CopyCss(SourceSkinID, BasePath, NewSkin.Id, SkinFileManagement.CssType.Main);
            }
            if (SourceSkin.AdminCss != "")
            {
                Business.SkinFileManagement.CopyCss(SourceSkinID, BasePath, NewSkin.Id, SkinFileManagement.CssType.Admin);
            }
            if (SourceSkin.IECss != "")
            {
                Business.SkinFileManagement.CopyCss(SourceSkinID, BasePath, NewSkin.Id, SkinFileManagement.CssType.IE);
            }
            if (SourceSkin.LoginCss != "")
            {
                Business.SkinFileManagement.CopyCss(SourceSkinID, BasePath, NewSkin.Id, SkinFileManagement.CssType.Login);
            }

            /*Immagini */
            Business.SkinFileManagement.CopyImages(SourceSkin.Id, BasePath, NewSkin.Id);

            return NewSkin.Id;
        }
        #endregion

        #region HTML Management

        ///// <summary>
        ///// Ritorna i paramentri renderizzati, recuperati da cache, data una comunità e/o un organizzazione
        ///// </summary>
        ///// <param name="CommunityId">L'id della comunità in cui si trova l'utente</param>
        ///// <param name="UserOrganizationId">L'Id dell'organizzazione a cui è iscritto l'utente, nel caso in cui non sia in una comunità</param>
        ///// <param name="VirtualPath">Il path virtuale delle skin, nel caso debba recuperare oggetti renderizzati</param>
        ///// <param name="LangCode">Il codice della lingua corrente</param>
        ///// <param name="DefLangCode">Il codice della lingua di default</param>
        ///// <returns></returns>
        //public Domain.HTML.HTMLSkin GetSkinHTML_OLD(
        //    Int32 CommunityId,
        //    Int32 UserOrganizationId,
        //    string VirtualPath,
        //    string LangCode,
        //    string DefLangCode,
        //    Entity.Configuration.SkinSettings DEF_SkinSettings, String appBaseUrl)
        //{
        //    Domain.HTML.HTMLSkin CompleteHtmlSkin = new Domain.HTML.HTMLSkin();

        //    Domain.DTO.DtoSkinsID SkinsId = GetSkinIds(CommunityId, UserOrganizationId, VirtualPath, DefLangCode);
            
        //    Domain.HTML.HTMLSkin PortalHtmlSkin = GetPortalSkin(LangCode, VirtualPath, DefLangCode);

        //    Domain.HTML.HTMLSkin OrganizationHtmlSkin = new Domain.HTML.HTMLSkin();
        //    Domain.HTML.HTMLSkin CommunityHtmlSkin = new Domain.HTML.HTMLSkin();

        //    //Ottimizzo caricamento...
        //    if(CommunityId > 0 && DEF_SkinSettings.PortalSetting != Entity.Configuration.SkinSettings.PortalOrganizationElements.None)
        //    { 
        //        OrganizationHtmlSkin = GetHtmlSkin(SkinsId.OrganizationId, LangCode, VirtualPath, DefLangCode);
        //        CommunityHtmlSkin = GetHtmlSkin(SkinsId.CommunityId, LangCode, VirtualPath, DefLangCode);
        //    }
            
        //    if (PortalHtmlSkin == null) { PortalHtmlSkin = new Domain.HTML.HTMLSkin(); }
        //    if (OrganizationHtmlSkin == null) { OrganizationHtmlSkin = new Domain.HTML.HTMLSkin(); }
        //    if (CommunityHtmlSkin == null) { CommunityHtmlSkin = new Domain.HTML.HTMLSkin(); }


        //    //bool IsPortalLogo = CommunityId <= 0 && !(DEF_SkinSettings.PortalSetting == Entity.Configuration.SkinSettings.PortalOrganizationElements.MainLogo);
        //    //bool IsPortalFooter = CommunityId <= 0 && !(DEF_SkinSettings.PortalSetting == Entity.Configuration.SkinSettings.PortalOrganizationElements.MainLogo);
        //    //bool IsPortalCss = CommunityId <= 0 && !(DEF_SkinSettings.PortalSetting == Entity.Configuration.SkinSettings.PortalOrganizationElements.MainLogo);


        //    //Logo Header
        //    if (!IsPortalLogo && CommunityHtmlSkin.HtmlHeadLogo != "")
        //    {
        //        CompleteHtmlSkin.HtmlHeadLogo = CommunityHtmlSkin.HtmlHeadLogo;
        //    }
        //    else if (!IsPortalLogo && OrganizationHtmlSkin.HtmlHeadLogo != "")
        //    {
        //        CompleteHtmlSkin.HtmlHeadLogo = OrganizationHtmlSkin.HtmlHeadLogo;
        //    }
        //    else if (PortalHtmlSkin.HtmlHeadLogo != "")
        //    {
        //        CompleteHtmlSkin.HtmlHeadLogo = PortalHtmlSkin.HtmlHeadLogo;
        //    }
        //    else {
        //        CompleteHtmlSkin.HtmlHeadLogo = SkinHtmlHelper.RenderDefLogo(DEF_SkinSettings.HeadLogo, appBaseUrl);
        //    }

        //    // Header Template
        //    if (!IsPortalLogo && CommunityHtmlSkin.HeaderTemplate != null && CommunityHtmlSkin.HeaderTemplate != "")
        //    {
        //        CompleteHtmlSkin.HeaderTemplate = CommunityHtmlSkin.HeaderTemplate;
        //    }
        //    else if (!IsPortalLogo && OrganizationHtmlSkin.HeaderTemplate != null && OrganizationHtmlSkin.HeaderTemplate != "")
        //    {
        //        CompleteHtmlSkin.HeaderTemplate = OrganizationHtmlSkin.HeaderTemplate;
        //    }
        //    else if (PortalHtmlSkin.HeaderTemplate != null && PortalHtmlSkin.HeaderTemplate != "")
        //    {
        //        CompleteHtmlSkin.HeaderTemplate = PortalHtmlSkin.HeaderTemplate;
        //    }
        //    else //if (DEF_SkinSettings.FootLogos != null && DEF_SkinSettings.HeaderTemplate != "")
        //    {
        //        CompleteHtmlSkin.HeaderTemplate = "";
        //    }


        //    // Testo footer
        //    if (!IsPortalFooter && CommunityHtmlSkin.FooterText != "")
        //    {
        //        CompleteHtmlSkin.FooterText = CommunityHtmlSkin.FooterText;
        //    }
        //    else if (!IsPortalFooter && OrganizationHtmlSkin.FooterText != "")
        //    {
        //        CompleteHtmlSkin.FooterText = OrganizationHtmlSkin.FooterText;
        //    }
        //    else if (PortalHtmlSkin.FooterText != "")
        //    {
        //        CompleteHtmlSkin.FooterText = PortalHtmlSkin.FooterText;
        //    }
        //    else
        //    {
        //        CompleteHtmlSkin.FooterText = DEF_SkinSettings.FootText;
        //    }

        //    // Loghi Footer
        //    if (!IsPortalFooter && CommunityHtmlSkin.HtmlFooterLogos != null && CommunityHtmlSkin.HtmlFooterLogos.Count() > 0)
        //    {
        //        CompleteHtmlSkin.HtmlFooterLogos = CommunityHtmlSkin.HtmlFooterLogos;
        //    }
        //    else if (!IsPortalFooter && OrganizationHtmlSkin.HtmlFooterLogos != null && OrganizationHtmlSkin.HtmlFooterLogos.Count() > 0)
        //    {
        //        CompleteHtmlSkin.HtmlFooterLogos = OrganizationHtmlSkin.HtmlFooterLogos;
        //    }
        //    else if (PortalHtmlSkin.HtmlFooterLogos != null && PortalHtmlSkin.HtmlFooterLogos.Count() > 0)
        //    {
        //        CompleteHtmlSkin.HtmlFooterLogos = PortalHtmlSkin.HtmlFooterLogos;
        //    }
        //    else if (DEF_SkinSettings.FootLogos != null && DEF_SkinSettings.FootLogos.Count() > 0)
        //    {
        //        foreach (Entity.Configuration.SkinSettings.Logo logo in DEF_SkinSettings.FootLogos)
        //        {
        //            CompleteHtmlSkin.HtmlFooterLogos.Add(SkinHtmlHelper.RenderDefLogo(logo, appBaseUrl));
        //        }
        //    }
            
        //    //Footer Template
        //    if (!IsPortalFooter && CommunityHtmlSkin.FooterTemplate != null && CommunityHtmlSkin.FooterTemplate != "")
        //    {
        //        CompleteHtmlSkin.FooterTemplate = CommunityHtmlSkin.FooterTemplate;
        //    }
        //    else if (!IsPortalFooter && OrganizationHtmlSkin.FooterTemplate != null && OrganizationHtmlSkin.FooterTemplate != "")
        //    {
        //        CompleteHtmlSkin.FooterTemplate = OrganizationHtmlSkin.FooterTemplate;
        //    }
        //    else if (PortalHtmlSkin.FooterTemplate != null && PortalHtmlSkin.FooterTemplate != "")
        //    {
        //        CompleteHtmlSkin.FooterTemplate = PortalHtmlSkin.FooterTemplate;
        //    }
        //    else //if (DEF_SkinSettings.FootLogos != null && DEF_SkinSettings.HeaderTemplate != "")
        //    {
        //        CompleteHtmlSkin.FooterTemplate = "";
        //    }
            
        //    return CompleteHtmlSkin;
        //}

        /// <summary>
        /// Ritorna i paramentri renderizzati, recuperati da cache, data una comunità e/o un organizzazione
        /// </summary>
        /// <param name="idCommunity">L'id della comunità in cui si trova l'utente</param>
        /// <param name="UserOrganizationId">L'Id dell'organizzazione a cui è iscritto l'utente, nel caso in cui non sia in una comunità</param>
        /// <param name="VirtualPath">Il path virtuale delle skin, nel caso debba recuperare oggetti renderizzati</param>
        /// <param name="LangCode">Il codice della lingua corrente</param>
        /// <param name="DefLangCode">Il codice della lingua di default</param>
        /// <returns></returns>
        /// <remarks>Updated 16/02/2015: Config elementi Portal from Organization.</remarks>
        public Domain.HTML.HTMLSkin GetSkinHTML(
            Int32 idCommunity,
            Int32 UserOrganizationId,
            string VirtualPath,
            string LangCode,
            string DefLangCode,
            Entity.Configuration.SkinSettings DEF_SkinSettings,
            String appBaseUrl,
            Int64 SkinId = 0, Boolean forObject = false)
        {
            Domain.HTML.HTMLSkin CompleteHtmlSkin = new Domain.HTML.HTMLSkin();
            
            Domain.HTML.HTMLSkin OrganizationHtmlSkin = new Domain.HTML.HTMLSkin();
            Domain.HTML.HTMLSkin CommunityHtmlSkin = new Domain.HTML.HTMLSkin();

            if (UserOrganizationId <= 0 && idCommunity <= 0)
                UserOrganizationId = this.GetUserDefaultIdOrganization(UC.CurrentUserID);

            Domain.DTO.DtoSkinsID SkinsId = GetSkinIds(idCommunity, UserOrganizationId, VirtualPath, DefLangCode);

            //Ottimizzo caricamento...
            if (idCommunity > 0 || DEF_SkinSettings.PortalSetting > Entity.Configuration.SkinSettings.PortalOrganizationElements.None)
            {
                OrganizationHtmlSkin = GetHtmlSkin(SkinsId.OrganizationId, LangCode, VirtualPath, DefLangCode);
                CommunityHtmlSkin = GetHtmlSkin(SkinsId.CommunityId, LangCode, VirtualPath, DefLangCode);
            }
            else if (forObject)
            {
                OrganizationHtmlSkin = GetHtmlSkin(SkinsId.OrganizationId, LangCode, VirtualPath, DefLangCode);
                if (idCommunity > 0)
                    CommunityHtmlSkin = GetHtmlSkin(SkinsId.CommunityId, LangCode, VirtualPath, DefLangCode);
            }
            Domain.HTML.HTMLSkin PortalHtmlSkin = GetPortalSkin(LangCode, VirtualPath, DefLangCode);
            
            //OrganizationHtmlSkin = GetHtmlSkin(SkinsId.OrganizationId, LangCode, VirtualPath, DefLangCode);
            //CommunityHtmlSkin = GetHtmlSkin(SkinsId.CommunityId, LangCode, VirtualPath, DefLangCode);

            if (PortalHtmlSkin == null) { PortalHtmlSkin = new Domain.HTML.HTMLSkin(); }
            if (OrganizationHtmlSkin == null) { OrganizationHtmlSkin = new Domain.HTML.HTMLSkin(); }
            if (CommunityHtmlSkin == null) { CommunityHtmlSkin = new Domain.HTML.HTMLSkin(); }

            //Module
            Domain.HTML.HTMLSkin ModuleHtmlSkin = GetHtmlSkin(SkinId, LangCode, VirtualPath, DefLangCode);
            if (SkinId == null || SkinId <= 0) { ModuleHtmlSkin = new Domain.HTML.HTMLSkin(); }

            bool IsPortalLogo = ((idCommunity <= 0 && !forObject) &&
                !((DEF_SkinSettings.PortalSetting & Entity.Configuration.SkinSettings.PortalOrganizationElements.MainLogo) == Entity.Configuration.SkinSettings.PortalOrganizationElements.MainLogo));

            bool IsPortalFooter = ((idCommunity <= 0 && !forObject) &&
                !((DEF_SkinSettings.PortalSetting & Entity.Configuration.SkinSettings.PortalOrganizationElements.Footer) == Entity.Configuration.SkinSettings.PortalOrganizationElements.Footer));

            //bool IsPortalCss = CommunityId <= 0 && !(DEF_SkinSettings.PortalSetting == Entity.Configuration.SkinSettings.PortalOrganizationElements.MainLogo);


            //Logo Header
            if (ModuleHtmlSkin.HtmlHeadLogo != "")
            {
                CompleteHtmlSkin.HtmlHeadLogo = ModuleHtmlSkin.HtmlHeadLogo;
            }
            else if (!IsPortalLogo && CommunityHtmlSkin.HtmlHeadLogo != "")
            {
                CompleteHtmlSkin.HtmlHeadLogo = CommunityHtmlSkin.HtmlHeadLogo;
            }
            else if (!IsPortalLogo && OrganizationHtmlSkin.HtmlHeadLogo != "")
            {
                CompleteHtmlSkin.HtmlHeadLogo = OrganizationHtmlSkin.HtmlHeadLogo;
            }
            else if (PortalHtmlSkin.HtmlHeadLogo != "")
            {
                CompleteHtmlSkin.HtmlHeadLogo = PortalHtmlSkin.HtmlHeadLogo;
            }
            else
            {
                CompleteHtmlSkin.HtmlHeadLogo = SkinHtmlHelper.RenderDefLogo(DEF_SkinSettings.HeadLogo, appBaseUrl);
            }

            // Header Template
            if (ModuleHtmlSkin.HeaderTemplate != null && ModuleHtmlSkin.HeaderTemplate.Count() > 0)
            {
                CompleteHtmlSkin.HeaderTemplate = ModuleHtmlSkin.HeaderTemplate;
            }
            else if (!IsPortalLogo && CommunityHtmlSkin.HeaderTemplate != null && CommunityHtmlSkin.HeaderTemplate != "")
            {
                CompleteHtmlSkin.HeaderTemplate = CommunityHtmlSkin.HeaderTemplate;
            }
            else if (!IsPortalLogo && OrganizationHtmlSkin.HeaderTemplate != null && OrganizationHtmlSkin.HeaderTemplate != "")
            {
                CompleteHtmlSkin.HeaderTemplate = OrganizationHtmlSkin.HeaderTemplate;
            }
            else if (PortalHtmlSkin.HeaderTemplate != null && PortalHtmlSkin.HeaderTemplate != "")
            {
                CompleteHtmlSkin.HeaderTemplate = PortalHtmlSkin.HeaderTemplate;
            }
            else //if (DEF_SkinSettings.FootLogos != null && DEF_SkinSettings.HeaderTemplate != "")
            {
                CompleteHtmlSkin.HeaderTemplate = "";
            }


            // Testo footer
            if (ModuleHtmlSkin.FooterText != "")
            {
                CompleteHtmlSkin.FooterText = ModuleHtmlSkin.FooterText;
            }
            else if (!IsPortalFooter && CommunityHtmlSkin.FooterText != "")
            {
                CompleteHtmlSkin.FooterText = CommunityHtmlSkin.FooterText;
            }
            else if (!IsPortalFooter && OrganizationHtmlSkin.FooterText != "")
            {
                CompleteHtmlSkin.FooterText = OrganizationHtmlSkin.FooterText;
            }
            else if (PortalHtmlSkin.FooterText != "")
            {
                CompleteHtmlSkin.FooterText = PortalHtmlSkin.FooterText;
            }
            else
            {
                CompleteHtmlSkin.FooterText = DEF_SkinSettings.FootText;
            }

            // Loghi Footer
            if (ModuleHtmlSkin.HtmlFooterLogos != null && ModuleHtmlSkin.HtmlFooterLogos.Count() > 0)
            {
                CompleteHtmlSkin.HtmlFooterLogos = ModuleHtmlSkin.HtmlFooterLogos;
            }
            else if (!IsPortalFooter && CommunityHtmlSkin.HtmlFooterLogos != null && CommunityHtmlSkin.HtmlFooterLogos.Count() > 0)
            {
                CompleteHtmlSkin.HtmlFooterLogos = CommunityHtmlSkin.HtmlFooterLogos;
            }
            else if (!IsPortalFooter && OrganizationHtmlSkin.HtmlFooterLogos != null && OrganizationHtmlSkin.HtmlFooterLogos.Count() > 0)
            {
                CompleteHtmlSkin.HtmlFooterLogos = OrganizationHtmlSkin.HtmlFooterLogos;
            }
            else if (PortalHtmlSkin.HtmlFooterLogos != null && PortalHtmlSkin.HtmlFooterLogos.Count() > 0)
            {
                CompleteHtmlSkin.HtmlFooterLogos = PortalHtmlSkin.HtmlFooterLogos;
            }
            else if (DEF_SkinSettings.FootLogos != null && DEF_SkinSettings.FootLogos.Count() > 0)
            {
                foreach (Entity.Configuration.SkinSettings.Logo logo in DEF_SkinSettings.FootLogos)
                {
                    CompleteHtmlSkin.HtmlFooterLogos.Add(SkinHtmlHelper.RenderDefLogo(logo, appBaseUrl));
                }
            }

            //Footer Template
            if (ModuleHtmlSkin.FooterTemplate != null && ModuleHtmlSkin.FooterTemplate.Count() > 0)
            {
                CompleteHtmlSkin.FooterTemplate = ModuleHtmlSkin.FooterTemplate;
            }
            else if (!IsPortalFooter && CommunityHtmlSkin.FooterTemplate != null && CommunityHtmlSkin.FooterTemplate != "")
            {
                CompleteHtmlSkin.FooterTemplate = CommunityHtmlSkin.FooterTemplate;
            }
            else if (!IsPortalFooter && OrganizationHtmlSkin.FooterTemplate != null && OrganizationHtmlSkin.FooterTemplate != "")
            {
                CompleteHtmlSkin.FooterTemplate = OrganizationHtmlSkin.FooterTemplate;
            }
            else if (PortalHtmlSkin.FooterTemplate != null && PortalHtmlSkin.FooterTemplate != "")
            {
                CompleteHtmlSkin.FooterTemplate = PortalHtmlSkin.FooterTemplate;
            }
            else //if (DEF_SkinSettings.FootLogos != null && DEF_SkinSettings.HeaderTemplate != "")
            {
                CompleteHtmlSkin.FooterTemplate = "";
            }

            return CompleteHtmlSkin;
        }


        //public Domain.HTML.HTMLSkin GetSkinModuleHTML(
        //    lm.Comol.Core.DomainModel.Helpers.dtoItemSkin dtoSKin,
        //    string VirtualPath,
        //    string LangCode,
        //    string DefLangCode,
        //    Entity.Configuration.SkinSettings DEF_SkinSettings, String appBaseUrl)
        //    {
        //        Domain.HTML.HTMLSkin CompleteHtmlSkin = GetSkinHTML(dtoSKin.IdCommunity, dtoSKin.IdCommunity, VirtualPath, LangCode, DefLangCode, DEF_SkinSettings, appBaseUrl);

        //        Domain.HTML.HTMLSkin ModuleHtmlSkin = GetModuleSkin(dtoSKin.IdSkin, LangCode, VirtualPath, DefLangCode, LangCode);

        //        if (ModuleHtmlSkin.HtmlHeadLogo != "")
        //        {
        //            CompleteHtmlSkin.HtmlHeadLogo = ModuleHtmlSkin.HtmlHeadLogo;
        //        }

        //        if (ModuleHtmlSkin.HtmlFooterLogos != null || ModuleHtmlSkin.HtmlFooterLogos.Count != 0)
        //        {
        //            CompleteHtmlSkin.HtmlFooterLogos = ModuleHtmlSkin.HtmlFooterLogos;
        //        }

        //        if (ModuleHtmlSkin.FooterText != "")
        //        {
        //            CompleteHtmlSkin.FooterText = ModuleHtmlSkin.FooterText;
        //        }

        //        if (ModuleHtmlSkin.HeaderTemplate != "")
        //        {
        //            CompleteHtmlSkin.HeaderTemplate = ModuleHtmlSkin.HeaderTemplate;
        //        }

        //        if (ModuleHtmlSkin.FooterTemplate != "")
        //        {
        //            CompleteHtmlSkin.FooterTemplate = ModuleHtmlSkin.FooterTemplate;
        //        }

        //        return CompleteHtmlSkin;
        //    }
        #region Portal

        /// <summary>
        /// Inizializza (carica) la skin di portale
        /// </summary>
        /// <param name="BaseVirtualPath">Il path virtuale</param>
        /// <param name="DefaultLangCode">La lingua di default</param>
        /// <remarks>SE non vengono trovati parametri per una specifica lingua, vengono presi quelli della lingua di default</remarks>
        public void ResetSkinPortalHtml(string BaseVirtualPath, string DefaultLangCode)
        {
            Domain.Skin PortalSkin = Manager.GetAll<Domain.Skin>(sk => sk.IsPortal == true).FirstOrDefault();

            IList<Domain.DTO.DtoSkinLanguage> SkLanguages = Manager.GetAll<Domain.DTO.DtoSkinLanguage>();

            CacheHelper.PurgeCacheItems(getPortalKey(""));


            if (PortalSkin != null)
            {
                CacheHelper.AddToCache<Int64>(getPortalIdKey(), PortalSkin.Id, CacheExpiration.Week);
                foreach (Domain.DTO.DtoSkinLanguage Lang in SkLanguages)
                {
                    Domain.HTML.HTMLSkin HtmlSkin = SkinHtmlHelper.RenderSkin(BaseVirtualPath, PortalSkin, Lang.LangCode, DefaultLangCode);
                    CacheHelper.AddToCache<Domain.HTML.HTMLSkin>(getPortalKey(Lang.LangCode), HtmlSkin, CacheExp);
                }
            }

        }


        public Domain.HTML.HTMLSkin ResetSkinHtml(Int64 SkinId, string DefLangCode, string BaseVirtualPath, string CurrentLangcode)
        {
            Domain.HTML.HTMLSkin HtmlSkin = new Domain.HTML.HTMLSkin();


            Domain.Skin Skin = Manager.Get<Domain.Skin>(SkinId);

            if (Skin != null)
            {
                IList<Domain.DTO.DtoSkinLanguage> SkLanguages = Manager.GetAll<Domain.DTO.DtoSkinLanguage>();

                CacheHelper.PurgeCacheItems(getSkinKey(SkinId, "")); //cancello TUTTE le cache di una skin relative alle lingue
                //CacheHelper.PurgeCacheItems(getPortalIdKey());
                //CacheHelper.Find<Int64>(getPortalIdKey());

                foreach (Domain.DTO.DtoSkinLanguage Lang in SkLanguages)
                {

                    Domain.HTML.HTMLSkin HtmlLangSkin = SkinHtmlHelper.RenderSkin(BaseVirtualPath, Skin, Lang.LangCode, DefLangCode);

                    if (HtmlLangSkin != null)
                    {
                        CacheHelper.AddToCache<Domain.HTML.HTMLSkin>(getSkinKey(Skin.Id, Lang.LangCode), HtmlLangSkin, CacheExp);
                    }

                    if (Lang.LangCode == CurrentLangcode)
                    {
                        HtmlSkin = HtmlLangSkin;
                    }
                }
            }

            return HtmlSkin;
        }

        private Domain.HTML.HTMLSkin GetModuleSkin(Int64 SkinId, string LangCode, string BaseVirtualPath, string DefLangcode, string CurrentLangcode)
        {
            Domain.HTML.HTMLSkin Skin = CacheHelper.Find<Domain.HTML.HTMLSkin>(getModuleKey(SkinId, LangCode));

            if (Skin == null)
            {
                ResetSkinHtml(SkinId, DefLangcode, BaseVirtualPath, CurrentLangcode);
                Skin = CacheHelper.Find<Domain.HTML.HTMLSkin>(getModuleKey(SkinId, LangCode));
            }

            return Skin;
        }

       


        private Domain.HTML.HTMLSkin GetPortalSkin(string LangCode, string BaseVirtualPath, string DefLangcode)
        {
            Domain.HTML.HTMLSkin Skin = CacheHelper.Find<Domain.HTML.HTMLSkin>(getPortalKey(LangCode));

            if (Skin == null)
            {
                ResetSkinPortalHtml(BaseVirtualPath, DefLangcode);
                Skin = CacheHelper.Find<Domain.HTML.HTMLSkin>(getPortalKey(LangCode));
            }

            return Skin;
        }
        public Domain.DTO.DtoSkinsID GetSkinIds(Int32 CommunityId, Int32 OrganizationId, string BaseVirtualPath, string DefaultLangCode)
        {
            Domain.DTO.DtoSkinsID SkinsId = new Domain.DTO.DtoSkinsID();

            // Se ho una comunità, recupero l'Id di quella comunità e della sua organizzazione
            // e ne recupero le relative Skin.
            if (CommunityId > 0)
            {
                Domain.HTML.HTMLSkinComId ComId = CacheHelper.Find<Domain.HTML.HTMLSkinComId>(getCommunityKey(CommunityId));

                if (ComId == null)
                {
                    ComId = LoadComID(CommunityId);
                    CacheHelper.AddToCache<Domain.HTML.HTMLSkinComId>(getCommunityKey(CommunityId), ComId, CacheExp);
                }

                SkinsId.CommunityId = ComId.CommunitySkinID;
                SkinsId.OrganizationId = ComId.OrganizationSkinID;
            }
            else
            {
                // Altrimenti per la comunità imposto "nessuna skin"
                // e recupero la skin relativa all'organizzazione data

                SkinsId.CommunityId = -1;

                Int64 OrgnSkinId = CacheHelper.Find<Int64>(getOrganizationKey(OrganizationId));
                if (OrgnSkinId <= 0)
                {
                    OrgnSkinId = LoadOrgnID(OrganizationId);
                    CacheHelper.AddToCache<Int64>(getOrganizationKey(OrganizationId), OrgnSkinId, CacheExp);
                }

                SkinsId.OrganizationId = OrgnSkinId;
            }

            // Infine il portale
            Int64 PortalSkinId = CacheHelper.Find<Int64>(getPortalIdKey());
            // Se non me lo trova nella sua variabile in cache,
            // carico LA SKIN di portale.
            if (PortalSkinId <= 0)
            {
                ResetSkinPortalHtml(BaseVirtualPath, DefaultLangCode);
                PortalSkinId = CacheHelper.Find<Int64>(getPortalIdKey());
            }

            SkinsId.PortalId = PortalSkinId;

            return SkinsId;
        }
        private Domain.HTML.HTMLSkin GetHtmlSkin(Int64 SkinId, string CurrentLangCode, string BaseVirtualPath, string DefLangcode)
        {
            Domain.HTML.HTMLSkin HtmlSkin = new Domain.HTML.HTMLSkin();
            // SE ho un ID skin valido, carico quella skin, altrimenti restituisco un oggetto vuoto.
            if (SkinId > 0)
            {
                HtmlSkin = CacheHelper.Find<Domain.HTML.HTMLSkin>(getSkinKey(SkinId, CurrentLangCode));
                if (HtmlSkin == null)
                {
                    HtmlSkin = ResetSkinHtml(SkinId, DefLangcode, BaseVirtualPath, CurrentLangCode);
                }
            }

            return HtmlSkin;
        }



        private Domain.HTML.HTMLSkinComId LoadComID(Int32 CommunityId)
        {
            Domain.HTML.HTMLSkinComId ComId = new Domain.HTML.HTMLSkinComId();
            Domain.DTO.DtoSkinCommunity Com = Manager.Get<Domain.DTO.DtoSkinCommunity>(CommunityId);

            Domain.DTO.DtoShareCommunity SC = Manager.GetAll<Domain.DTO.DtoShareCommunity>(dsc => dsc.CommunityId == Com.Id).FirstOrDefault();
            Domain.DTO.DtoShareOrganization SO = Manager.GetAll<Domain.DTO.DtoShareOrganization>(dso => dso.OrganizationId == Com.OrganizationId).FirstOrDefault();

            if (SC != null)
            { ComId.CommunitySkinID = SC.SkinId; }
            else
            { ComId.CommunitySkinID = -1; }

            if (SO != null)
            { ComId.OrganizationSkinID = SO.SkinId; }
            else
            { ComId.OrganizationSkinID = -1; }

            return ComId;
        }
        private Int64 LoadOrgnID(Int32 idOrganization)
        {
            Domain.DTO.DtoShareOrganization SO = Manager.GetAll<Domain.DTO.DtoShareOrganization>(dso => dso.OrganizationId == idOrganization).FirstOrDefault();

            if (SO != null)
            {
                return SO.SkinId;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Carica l'HTML con i css opportuni.
        /// </summary>
        /// <param name="idCommunity">Id Comunità. 0 = portale.</param>
        /// <param name="idOrganization"></param>
        /// <param name="BaseSkinPath"></param>
        /// <param name="languageCode"></param>
        /// <param name="CssType"></param>
        /// <param name="DefaultCss"></param>
        /// <param name="BaseAppPath"></param>
        /// <param name="LoadOrgnForPortal">SE da configurazione caricare i CSS dell'organizzaizone:
        /// Se CommunityId > 0 viene ignorato.</param>
        /// <returns></returns>
        public string GetCSSHtml(
            Int32 idCommunity, 
            Int32 idOrganization, 
            string BaseSkinPath, 
            string languageCode, 
            SkinFileManagement.CssType CssType, 
            String BaseAppPath,
            Entity.Configuration.SkinSettings DEF_SkinSettings,
            Int64 idSkin = 0,Boolean forObject=false )
        {
            String DefaultCss = "";

            switch (CssType)
            {
                case SkinFileManagement.CssType.Main:
                    DefaultCss = DEF_SkinSettings.MainCss;
                    break;
                case SkinFileManagement.CssType.Admin:
                    DefaultCss = DEF_SkinSettings.AdminCss;
                    break;
                case SkinFileManagement.CssType.Login:
                    DefaultCss = DEF_SkinSettings.LoginCss;
                    break;
                case SkinFileManagement.CssType.IE:
                    DefaultCss = DEF_SkinSettings.IECss;
                    break;
            }

            if (idOrganization <= 0 && idCommunity <= 0)
                idOrganization = this.GetUserDefaultIdOrganization(UC.CurrentUserID);
            //CurrentManager.GetUserDefaultIdOrganization(UserContext.CurrentUserID);



            bool LoadOrgnForPortal = (!forObject || (forObject && idOrganization==0));

            if (idCommunity <= 0 && !forObject)
                LoadOrgnForPortal = ((DEF_SkinSettings.PortalSetting & Entity.Configuration.SkinSettings.PortalOrganizationElements.Css)== Entity.Configuration.SkinSettings.PortalOrganizationElements.Css);
            string OutCss = "";
            string SkPath = "";
            Domain.DTO.DtoSkinsID SkinIds = GetSkinIds(idCommunity, idOrganization, BaseSkinPath, languageCode);

            if (CssType == SkinFileManagement.CssType.Login)
            {
                //Login
                if (idSkin > 0)
                {
                    SkPath = BaseSkinPath + idSkin;
                    OutCss += GetCssHtml(SkinFileManagement.GetFullCssName(SkPath, CssType, true));
                }
                else if (SkinIds.PortalId > 0)
                {
                    SkPath = BaseSkinPath + SkinIds.PortalId;
                    OutCss += GetCssHtml(SkinFileManagement.GetFullCssName(SkPath, CssType, true));
                }
                else
                {
                    if (DefaultCss != "") { OutCss = GetCssHtml(BaseAppPath + DefaultCss); }
                }
            }
            else
            {

                //Organizzazione <- Carico Organizzazione se c'è organizzazione ad esso associata
                if ((LoadOrgnForPortal || forObject) && SkinIds.OrganizationId > 0)
                {
                    SkPath = BaseSkinPath + SkinIds.OrganizationId;
                    OutCss += GetCssHtml(SkinFileManagement.GetFullCssName(SkPath, CssType, true));
                }
                else if (SkinIds.PortalId > 0) //Portale <- Carico portale se c'è una skin ad esso associata
                {
                    SkPath = BaseSkinPath + SkinIds.PortalId;
                    OutCss += GetCssHtml(SkinFileManagement.GetFullCssName(SkPath, CssType, true));
                }

                // CONFIGURAZIONE <- SE NON HO ANCORA CSS "di defaul"
                if (OutCss == "" && DefaultCss != "") { OutCss = GetCssHtml(BaseAppPath + DefaultCss); }

                //Comunità <- Se ho una comunità carico la relativa skin (puo' essere vuota)
                if (SkinIds.CommunityId > 0)
                {
                    SkPath = BaseSkinPath + SkinIds.CommunityId;
                    OutCss += GetCssHtml(SkinFileManagement.GetFullCssName(SkPath, CssType, true));
                }

            }
            return OutCss;
        }

        private string GetCssHtml(string cssPath)
        {
            //if (cssPath != "-1")
            //{
            return "<link rel=\"stylesheet\" type=\"text/css\" href=\"" + cssPath + "\"> \r\n";
            //}
            //else {
            //    return "";
            //}
        }
        #endregion

        public void CleanCache()
        {
            CacheHelper.PurgeCacheItems(SkinBaseKey);
        }

        public void CleanModuleCache(Int64 SkinId)
        { 
                    CacheHelper.PurgeCacheItems(getModuleKey(SkinId, ""));
        }

        //  HtmlSkin = lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<Dictionary<String,String>>(CacheKeys.HtmlSkin(skin.Id, LanguageId.ToString()));
        #endregion

        #region Logo istituzione
        /// <summary>
        /// Ritorna i paramentri renderizzati, recuperati da cache, data una comunità e/o un organizzazione
        /// </summary>
        /// <param name="CommunityId">L'id della comunità in cui si trova l'utente</param>
        /// <param name="UserOrganizationId">L'Id dell'organizzazione a cui è iscritto l'utente, nel caso in cui non sia in una comunità</param>
        /// <param name="VirtualPath">Il path virtuale delle skin, nel caso debba recuperare oggetti renderizzati</param>
        /// <param name="LangCode">Il codice della lingua corrente</param>
        /// <param name="DefLangCode">Il codice della lingua di default</param>
        /// <returns></returns>
        public String GetlogoIstituzioneFullPath(Int32 CommunityId, Int32 UserOrganizationId, string BasePath, string LangCode, string DefLangCode, string ConfigLogo)
        {
            String path = "";
            Int64 SkinId = 0;

            Domain.HeaderLogo Hlogo = null;

            Domain.HTML.HTMLSkin CompleteHtmlSkin = new Domain.HTML.HTMLSkin();

            Domain.DTO.DtoSkinsID SkinsId = GetSkinIds(CommunityId, UserOrganizationId, BasePath, DefLangCode);

            //Community
            Skin.Domain.Skin skin = Manager.Get<Domain.Skin>(SkinsId.CommunityId);
            if (skin != null && skin.HeaderLogos != null)
            {
                SkinId = skin.Id;
                Hlogo = (from lg in skin.HeaderLogos where lg.LangCode == LangCode select lg).FirstOrDefault();
                if (Hlogo != null)
                {
                    (from lg in skin.HeaderLogos where lg.LangCode == DefLangCode select lg).FirstOrDefault();
                }
            }

            //Organization
            if (Hlogo == null)
            {
                skin = Manager.Get<Domain.Skin>(SkinsId.OrganizationId);
                if (skin != null && skin.HeaderLogos != null)
                {
                    SkinId = skin.Id;
                    Hlogo = (from lg in skin.HeaderLogos where lg.LangCode == DefLangCode select lg).FirstOrDefault();
                    if (Hlogo != null)
                    {
                        (from lg in skin.HeaderLogos where lg.LangCode == DefLangCode select lg).FirstOrDefault();
                    }
                }   
            }

            //Portal
            if (Hlogo == null)
            {
                skin = Manager.Get<Domain.Skin>(SkinsId.PortalId);
                if (skin != null && skin.HeaderLogos != null)
                {
                    SkinId = skin.Id;
                    Hlogo = (from lg in skin.HeaderLogos where lg.LangCode == DefLangCode select lg).FirstOrDefault();
                    if (Hlogo != null)
                    {
                        (from lg in skin.HeaderLogos where lg.LangCode == DefLangCode select lg).FirstOrDefault();
                    }
                }
            }


            if (Hlogo == null)
            {
                //Config
                path = ConfigLogo;
            }else
            {
                path = SkinFileManagement.GetLogoFullPath(BasePath, SkinId, Hlogo.Id, Hlogo.ImageUrl);
            }

            return path;
        }

        /// <summary>
        /// Restituisce il logo istituzione (Logo Header) per una data skin
        /// </summary>
        /// <param name="SkinId">Id della skin</param>
        /// <param name="BasePath">Da configurazione, es:  C:\inetput\wwwroot\Comol\ </param>
        /// <param name="LangCode">Codice lingua corrente (da sessione utente)</param>
        /// <param name="DefLangCode">lingua di default del sistema (da configurazione sistema)</param>
        /// <param name="ConfigLogo">Da configuarazione, es:  Me.BaseUrl & Me.SystemSettings.SkinSettings.HeadLogo.Url</param>
        /// <returns></returns>
        public String GetlogoIstituzionePathModule(Int64 SkinId, string BasePath, string LangCode, string DefLangCode, string ConfigLogo)
        {

            String path = "";

            Domain.HeaderLogo Hlogo = null;

            Domain.HTML.HTMLSkin CompleteHtmlSkin = new Domain.HTML.HTMLSkin();

            Skin.Domain.Skin skin = Manager.Get<Domain.Skin>(SkinId);
            if (skin != null && skin.HeaderLogos != null)
            {
                SkinId = skin.Id;
                Hlogo = (from lg in skin.HeaderLogos where lg.LangCode == LangCode select lg).FirstOrDefault();
                if (Hlogo != null)
                {
                    (from lg in skin.HeaderLogos where lg.LangCode == DefLangCode select lg).FirstOrDefault();
                }
            }

            if (Hlogo == null)
            {
                //Config
                path = ConfigLogo;
            }
            else
            {
                path = SkinFileManagement.GetLogoFullPath(BasePath, SkinId, Hlogo.Id, Hlogo.ImageUrl);
            }

            return path;
        }



        /// <summary>
        /// Ritorna i paramentri renderizzati, recuperati da cache, data una comunità e/o un organizzazione
        /// </summary>
        /// <param name="CommunityId">L'id della comunità in cui si trova l'utente</param>
        /// <param name="UserOrganizationId">L'Id dell'organizzazione a cui è iscritto l'utente, nel caso in cui non sia in una comunità</param>
        /// <param name="VirtualPath">Il path virtuale delle skin, nel caso debba recuperare oggetti renderizzati</param>
        /// <param name="LangCode">Il codice della lingua corrente</param>
        /// <param name="DefLangCode">Il codice della lingua di default</param>
        /// <returns></returns>
        public String GetlogoIstituzioneUrl(Int32 CommunityId, Int32 UserOrganizationId, string BaseUrl, string LangCode, string DefLangCode, string ConfigUrl)
        {
            String path = "";
            Int64 SkinId = 0;

            Domain.HeaderLogo Hlogo = null;

            Domain.HTML.HTMLSkin CompleteHtmlSkin = new Domain.HTML.HTMLSkin();

            Domain.DTO.DtoSkinsID SkinsId = GetSkinIds(CommunityId, UserOrganizationId, BaseUrl, DefLangCode);

            //Community
            Skin.Domain.Skin skin = Manager.Get<Domain.Skin>(SkinsId.CommunityId);
            if (skin != null && skin.HeaderLogos != null)
            {
                SkinId = skin.Id;
                Hlogo = (from lg in skin.HeaderLogos where lg.LangCode == LangCode select lg).FirstOrDefault();
                if (Hlogo != null)
                {
                    (from lg in skin.HeaderLogos where lg.LangCode == DefLangCode select lg).FirstOrDefault();
                }
            }

            //Organization
            if (Hlogo == null)
            {
                skin = Manager.Get<Domain.Skin>(SkinsId.OrganizationId);
                if (skin != null && skin.HeaderLogos != null)
                {
                    SkinId = skin.Id;
                    Hlogo = (from lg in skin.HeaderLogos where lg.LangCode == DefLangCode select lg).FirstOrDefault();
                    if (Hlogo != null)
                    {
                        (from lg in skin.HeaderLogos where lg.LangCode == DefLangCode select lg).FirstOrDefault();
                    }
                }
            }

            //Portal
            if (Hlogo == null)
            {
                skin = Manager.Get<Domain.Skin>(SkinsId.PortalId);
                if (skin != null && skin.HeaderLogos != null)
                {
                    SkinId = skin.Id;
                    Hlogo = (from lg in skin.HeaderLogos where lg.LangCode == DefLangCode select lg).FirstOrDefault();
                    if (Hlogo != null)
                    {
                        (from lg in skin.HeaderLogos where lg.LangCode == DefLangCode select lg).FirstOrDefault();
                    }
                }
            }

            if (Hlogo == null)
            {
                path = ConfigUrl;
            } else
            {
                path = SkinFileManagement.GetLogoVirtualFullPath(BaseUrl, SkinId, Hlogo.Id, Hlogo.ImageUrl);
            }

            return path;
        }

        #endregion

        #region CacheKey


            private string PortalKey
            {
                get { return SkinBaseKey + "Portal_"; }
            }
            private string OrganizationKey
            {
                get { return SkinBaseKey + "Organization_Keys"; }
            }
            private string CommunityKey
            {
                get { return SkinBaseKey + "Community_Keys"; }
            }
            private string ModuleKey
            {
                get { return SkinBaseKey + "Module_"; }
            }
            private string SkinKey
            {
                get { return SkinBaseKey + "SkinHtml_"; }
            }
            private string SkinBaseKey
            { get { return "Skin_"; } }

            private String getModuleKey(Int64 SkinId, String LangCode)
            {
                return ModuleKey + "_" + SkinId.ToString() + "_" + LangCode;
            }
            private String getPortalKey(String LangCode)
            {
                return PortalKey + LangCode;
            }
            private String getPortalIdKey()
            { return PortalKey + "ID"; }
            private String getOrganizationKey(Int32 OrgnId)
            { return OrganizationKey + OrgnId.ToString(); }
            private String getCommunityKey(Int32 ComId)
            { return CommunityKey + ComId.ToString(); }
            private String getSkinKey(Int64 SkinId, string LangCode)
            { return SkinKey + SkinId.ToString() + "_" + LangCode; }

            private TimeSpan CacheExp
            { get { return CacheExpiration.Week; } }
        #endregion

        #region "Service Module Skins"
            public T GetItem<T>(long idItem)
            {
                try
                {
                    Manager.BeginTransaction();
                    T obj = Manager.Get<T>(idItem);
                    Manager.Commit();
                    return obj;
                }
                catch (Exception ex) {
                    Manager.RollBack();
                    return default(T);
                }
            }
            public Domain.Skin AddSkin(String name, String path, lm.Comol.Core.DomainModel.ModuleObject owner)
            {
                Domain.Skin skin = null;
                try
                {
                    Manager.BeginTransaction();
                    skin = new Domain.Skin();
                    skin.IsModule = (owner!=null);
                    skin.Name = name;
                    Person user = Manager.Get<Person>(UC.CurrentUserID);
                    skin.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                    Manager.SaveOrUpdate<Domain.Skin>(skin);
                    if (owner!=null){
                        ModuleAssociation association = AddModuleAssociation(user, new DtoDisplaySkin() { Id = skin.Id, Type = SkinDisplayType.Module }, owner.CommunityID, owner);
                        AddModuleShare(user, owner.CommunityID, association, owner, SkinDisplayType.Module);
                    }

                    Manager.Commit();
                    SkinFileManagement.CreateDir(skin.Id, path);
                }
                catch (Exception ex)
                {
                    if(Manager.IsInTransaction())
                        Manager.RollBack();
                    if (skin.Id > 0)
                    {
                        DELETE_Skin(skin.Id, path);
                        skin = null;
                    }
                }
                return skin;
            }
            public Domain.Skin SaveSkin(long idSKin,String name, bool OverrideFooterLogos)
            {
                Domain.Skin skin = null;
                try
                {
                    Manager.BeginTransaction();
                    skin = Manager.Get<Domain.Skin>(idSKin);
                    if (skin != null)
                    {
                        skin.Name = name;
                        skin.OverrideVoidFooterLogos = OverrideFooterLogos;
                        Person user = Manager.Get<Person>(UC.CurrentUserID);
                        skin.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                        Manager.SaveOrUpdate<Domain.Skin>(skin);
                    }
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                    skin = null;
                }
                return skin;
            }
            public lm.Comol.Core.DomainModel.Helpers.dtoItemSkin GetModuleSkin(lm.Comol.Core.DomainModel.ModuleObject source, lm.Comol.Core.DomainModel.Helpers.dtoItemSkin starter ){
                try
                {
                    List<Skin.Domain.Skin> items = (from m in Manager.GetIQ<Skin.Domain.Skin_ShareModule>()
                                                    where m.IdModule == source.ServiceID && m.OwnerLongID == source.ObjectLongID && m.OwnerTypeID == source.ObjectTypeID
                                                    && m.Deleted == BaseStatusDeleted.None && m.Skin !=null 
                                                    select m.Skin).ToList();

                    Skin.Domain.Skin skin = items.OrderByDescending(s => s.ModifiedOn).Where(s => s.Deleted == BaseStatusDeleted.None).FirstOrDefault();
                    if (skin == null) {
                        ModuleAssociation association = (from ma in Manager.GetIQ<ModuleAssociation>()
                                                         where ma.IdModule == source.ServiceID && ma.OwnerLongID == source.ObjectLongID && ma.OwnerTypeID == source.ObjectTypeID && ma.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                                         select ma).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (association != null) {
                            starter.IsForPortal = association.IsPortal;
                            starter.IdOrganization = association.IdOrganization;
                            starter.IdCommunity = association.IdCommunity;
                        }
                    }
                    else
                        starter.IdSkin = skin.Id;
                }
                catch (Exception ex) { 
                
                }
                return starter;
            }

            public List<DtoDisplaySkin> GetAvailableSkins(Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, Boolean userAllowed, LoadItemsBy loadBy)
            {
                List<DtoDisplaySkin> items = new List<DtoDisplaySkin>();
                

                try
                {
                    Manager.BeginTransaction();
                    Person person = Manager.GetPerson(UC.CurrentUserID);
                    Community community = Manager.GetCommunity(idCommunity);
                    if (userAllowed)
                    {
                        // LOAD portal skins
                        items.AddRange((from s in Manager.GetIQ<Domain.Skin>() where s.Deleted == BaseStatusDeleted.None && !s.IsModule && s.IsPortal select new DtoDisplaySkin() { Id = s.Id, IsValid = true, Name = s.Name, Type = SkinDisplayType.Portal }).ToList());

                        // Load organizations
                        if (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator)
                            items.AddRange((from o in Manager.GetIQ<Organization>()
                                            select new DtoDisplaySkin() { Id = o.Id, Name = o.Name, Type = SkinDisplayType.Organization }).ToList().OrderBy(o=>o.Name).ToList());
                        else
                        {
                            List<Int32> idOrganizations = (from o in Manager.GetIQ<OrganizationProfiles>() where o.Profile == person select o.OrganizationID).ToList();
                            items.AddRange((from o in Manager.GetIQ<Organization>()
                                            where idOrganizations.Contains(o.Id)
                                            select new DtoDisplaySkin() { Id = o.Id, Name = o.Name, Type = SkinDisplayType.Organization }).ToList().OrderBy(o => o.Name).ToList());
                        }

                        // Load community
                        items.AddRange((from sc in Manager.GetIQ<Skin_ShareCommunity>()
                                        where sc.CommunityId == idCommunity && sc.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                        select new DtoDisplaySkin() { Id = idCommunity, Name = sc.Skin.Name, Type = SkinDisplayType.Community }).ToList());
                        //if (!items.Where(i=> i.Type== SkinDisplayType.Community).Any())
                        //    items.Add(new DtoDisplaySkin() { Id = idCommunity, IsValid = true, Name = community.Name, Type = SkinDisplayType.CurrentCommunity });
                    }
                    else {                        
                        items.Add(new DtoDisplaySkin() { Id = idCommunity, IsValid = true, Name = (community!=null) ? community.Name : "", Type = SkinDisplayType.CurrentCommunity });
                    }
                    // Find all real available skins !
                    items.Where(i => i.Type == SkinDisplayType.Organization).ToList().ForEach(o => o.IsValid = (from s in Manager.GetIQ<Skin_ShareOrganization>()
                                                                                                                where s.OrganizationId == o.Id && s.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                                                                                            select s.Id).Any());
                    items.Where(i => i.Type == SkinDisplayType.Community || i.Type == SkinDisplayType.CurrentCommunity).ToList().ForEach(o => o.IsValid = (from s in Manager.GetIQ<Skin_ShareCommunity>()
                                                                                                                where s.CommunityId == o.Id && s.Deleted == Core.DomainModel.BaseStatusDeleted.None
                                                                                                                select s.Id).Any());
                    items.AddRange(GetModuleSkins(person,idModule, idCommunity, idModuleItem, idItemType, loadBy));
                    items.Insert(0, new DtoDisplaySkin() { IsValid = true, Type = SkinDisplayType.Empty, Id = 0 });
                    Manager.Commit();
                }
                catch (Exception ex) {
                    Manager.RollBack();
                }

                return items;
            }
            private List<DtoDisplaySkin> GetModuleSkins(Person person ,Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType,LoadItemsBy loadBy)
            {
                List<DtoDisplaySkin> items = new List<DtoDisplaySkin>();
                try
                {
                    var query = (from sm in Manager.GetIQ<Skin_ShareModule>() 
                                 where sm.Deleted== Core.DomainModel.BaseStatusDeleted.None select sm);
                    if ((loadBy & LoadItemsBy.Creator)>0)
                        query = query.Where(m=>m.CreatedBy== person);
                    if ((loadBy & LoadItemsBy.Community)>0)
                        query = query.Where(m=>m.IdCommunity== idCommunity);
                    if ((loadBy & LoadItemsBy.Module)>0)
                        query = query.Where(m=>m.IdModule== idModule);
                    if ((loadBy & LoadItemsBy.Object)>0)
                        query = query.Where(m=>m.OwnerLongID== idModuleItem && m.OwnerTypeID==idItemType);
                    List<long> idSKins = query.Select(m=>m.Skin.Id).ToList();

                    items.AddRange((from s in Manager.GetIQ<Domain.Skin>() 
                                    where s.Deleted== Core.DomainModel.BaseStatusDeleted.None && (idSKins.Contains(s.Id) || s.IsModule )
                                    orderby s.Name 
                                    select new DtoDisplaySkin() { Id = s.Id , Name = s.Name, Type = SkinDisplayType.Module, IsValid=true  }).ToList());

                }
                catch (Exception ex)
                {

                }
                return items;
            }

            public DtoDisplaySkin GetDefaultSkinForModule(Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType)
            {
                DtoDisplaySkin result = null;
                try
                {
                    ModuleAssociation assigned = (from m in Manager.GetIQ<ModuleAssociation>()
                                                  where m.Deleted == Core.DomainModel.BaseStatusDeleted.None && m.OwnerTypeID == idItemType && m.OwnerLongID == idModuleItem
                                                  && m.IdModule == idModule
                                                  select m).Skip(0).Take(1).ToList().FirstOrDefault();
                    if (assigned != null) {
                        if (assigned.IdSkin > 0)
                            result = new DtoDisplaySkin() { Type = SkinDisplayType.Module, Id = assigned.IdSkin };
                        else if (assigned.IdOrganization>0 && assigned.IdCommunity>0)
                            result = new DtoDisplaySkin() { Type = SkinDisplayType.Community, Id = assigned.IdCommunity };
                        else if (assigned.IdOrganization>0)
                            result = new DtoDisplaySkin() { Type = SkinDisplayType.Organization, Id = assigned.IdOrganization };
                    }
                }
                catch (Exception es) { 
                
                }
                return result;
            }

            public Boolean SaveSkinAssociation(DtoDisplaySkin skin, Int32 idCommunity, lm.Comol.Core.DomainModel.ModuleObject source)
            {
                Boolean result = false;
                try
                {
                    Manager.BeginTransaction();
                    Person person = Manager.GetPerson(UC.CurrentUserID);
                    if (skin.Type == SkinDisplayType.Empty)
                    {
                        DeleteSkinAssociation(person, idCommunity, source, false);
                        DeleteModuleShare(person, idCommunity, source, false, false);
                    }
                    else
                    {
                        ModuleAssociation association = AddModuleAssociation(person, skin, idCommunity, source);
                        Domain.Skin_ShareModule share = AddModuleShare(person, idCommunity, association, source, skin.Type);
                        if (association != null)
                            Manager.SaveOrUpdate(association);
                        if (share != null)
                            Manager.SaveOrUpdate(share);
                    }
                    Manager.Commit();
                }
                catch (Exception ex) {
                    result = false;
                    Manager.RollBack();
                }
                return result;
            }
            private ModuleAssociation AddModuleAssociation(Person person, DtoDisplaySkin skin, Int32 idCommunity, lm.Comol.Core.DomainModel.ModuleObject source)
            {
                ModuleAssociation association = (from ma in Manager.GetIQ<ModuleAssociation>()
                                                 where ma.IdModule == source.ServiceID && ma.OwnerLongID == source.ObjectLongID && ma.OwnerTypeID == source.ObjectTypeID
                                        select ma).Skip(0).Take(1).ToList().FirstOrDefault();
                if (association == null) {
                    association = new ModuleAssociation();
                    association.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    association.IdCommunity = idCommunity;
                    association.IdModule = source.ServiceID;
                    association.OwnerLongID = source.ObjectLongID;
                    association.OwnerTypeID = source.ObjectTypeID;
                    association.OwnerFullyQualifiedName = source.FQN;
                }
                else{
                    association.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    association.Deleted= Core.DomainModel.BaseStatusDeleted.None;
                }
                switch (skin.Type)
                {
                    case SkinDisplayType.Portal:
                        association.IsPortal = true;
                        break;
                    case SkinDisplayType.Organization:
                        association.IdOrganization = Convert.ToInt32(skin.Id);
                        break;
                    case SkinDisplayType.CurrentCommunity:
                    case SkinDisplayType.Community:
                        association.IdCommunity = Convert.ToInt32(skin.Id);
                        break;
                    case SkinDisplayType.Module:
                    case SkinDisplayType.NotAssociated:
                        association.IdSkin = skin.Id;
                        break;
                }

                return association;
            }
            private Skin_ShareModule AddModuleShare(Person person, Int32 idCommunity,ModuleAssociation association, lm.Comol.Core.DomainModel.ModuleObject source, SkinDisplayType type)
            {
                Skin_ShareModule share = null;
                if (type == SkinDisplayType.Module) {
                    share = (from s in Manager.GetIQ<Skin_ShareModule>()
                             where s.IdModule == association.IdModule && s.OwnerLongID == association.OwnerLongID && s.OwnerTypeID == association.OwnerTypeID
                             && s.IdCommunity == idCommunity
                             select s).Skip(0).Take(1).ToList().FirstOrDefault();
                    if (share == null)
                    {
                        share = new Skin_ShareModule();
                        share.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        share.IdCommunity = idCommunity;
                        share.IdModule = association.IdModule;
                        share.OwnerLongID = association.OwnerLongID;
                        share.OwnerTypeID = association.OwnerTypeID;
                        share.OwnerFullyQualifiedName = association.OwnerFullyQualifiedName;
                        share.Skin = Manager.Get<Domain.Skin>(association.IdSkin);
                    }
                    else
                    {
                        share.Deleted = Core.DomainModel.BaseStatusDeleted.None;
                        share.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        share.Skin = Manager.Get<Domain.Skin>(association.IdSkin);
                    }
                }
                else{
                    DeleteModuleShare(person, idCommunity, source,false, false);
                }
                return share;
            }
            private Boolean DeleteSkinAssociation(Person person,Int32 idCommunity, lm.Comol.Core.DomainModel.ModuleObject source, Boolean physical)
            {
                Boolean result = false;
                var queryAssociation =(from ma in Manager.GetIQ<ModuleAssociation>()
                                       where ma.IdModule == source.ServiceID && ma.OwnerLongID == source.ObjectLongID && ma.OwnerTypeID == source.ObjectTypeID
                                                 select ma);
                
                if (physical)
                    Manager.DeletePhysicalList(queryAssociation.ToList());
                else{
                    List<ModuleAssociation> items = queryAssociation.Where(i => i.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList();
                    items.ForEach(i => i.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                    items.ForEach(i => i.Deleted |= Core.DomainModel.BaseStatusDeleted.Manual);
                    Manager.SaveOrUpdateList(items);
            
                }
                result = true;
                return result;
            }

            private Boolean DeleteModuleShare(Person person, Int32 idCommunity, lm.Comol.Core.DomainModel.ModuleObject source,Boolean deleteSkin, Boolean physical)
            {
                Boolean result = false;
                var queryShare = (from s in Manager.GetIQ<Skin_ShareModule>()
                                  where s.IdModule == source.ServiceID && s.OwnerLongID == source.ObjectLongID && s.OwnerTypeID == source.ObjectTypeID && s.IdCommunity == idCommunity
                                  select s);
                List<long> idSkins = queryShare.Where(s => s.Skin != null).Select(s => s.Skin.Id).ToList();


                if (physical)
                    Manager.DeletePhysicalList(queryShare.ToList());
                else
                {
                    List<Skin_ShareModule> shares = queryShare.Where(i => i.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList();
                    shares.ForEach(i => i.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                    shares.ForEach(i => i.Deleted |= Core.DomainModel.BaseStatusDeleted.Manual);
                    Manager.SaveOrUpdateList(shares);
                }
                if (deleteSkin)
                {
                    foreach (long idSkin in idSkins)
                    {
                        if (!queryShare.Where(s => s.Skin.Id == idSkin && s.Deleted == Core.DomainModel.BaseStatusDeleted.None).Any())
                        {
                            Domain.Skin skin = Manager.Get<Domain.Skin>(idSkin);
                            if (skin != null && physical)
                                Manager.DeletePhysical(skin);
                            else if (skin != null)
                            {
                                skin.Deleted = Core.DomainModel.BaseStatusDeleted.Manual;
                                skin.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(skin);
                            }
                        }
                    }
                }
                result = true;
                return result;
            }

            /// <summary>
            /// A seconda del tipo, recupera gli Id necessari alla master per il render delle skin
            /// </summary>
            /// <param name="idCommunity">Id comunità impostato</param>
            /// <param name="idItem">Id elemento (il significato del valore dipende dal tipo)</param>
            /// <param name="itemType">Il tipo di skin</param>
            /// <returns>Un dto con gli elementi necessari al render</returns>
            /// <remarks>
            ///     Significato di idItem a seconda di itemType:
            ///             Empty                   n.d.    (DEFAULT = Come Portal)
            ///             Portal                  Portale. Gli altri Id sono a zero.
            ///             Organization            Id Organizzazione
            ///             Community               Id Comunità
            ///             Module                  Id Skin di modulo
            ///             NotAssociated           n.d.    (DEFAULT = Come Portal)
            ///             CurrentCommunity        Id Comunità (Al momento come Community)
            /// </remarks>
            public lm.Comol.Core.DomainModel.Helpers.ExternalPageContext GetItemSkinSettings(Int32 idCommunity, long idItem, SkinDisplayType itemType)
            {
                lm.Comol.Core.DomainModel.Helpers.ExternalPageContext content = new lm.Comol.Core.DomainModel.Helpers.ExternalPageContext();
                content.Skin = new lm.Comol.Core.DomainModel.Helpers.dtoItemSkin();
                Community community = Manager.GetCommunity(idCommunity);
                Int32 idOrganization = (from p in Manager.GetIQ<OrganizationProfiles>() where p.Profile != null && p.Profile.Id== UC.CurrentUserID && p.isDefault select p.OrganizationID ).Skip(0).Take(1).ToList().FirstOrDefault();
                
                switch (itemType) { 
                    case SkinDisplayType.Portal:
                        content.Skin.IsForPortal = true;
                        content.Skin.IdCommunity = 0;
                        content.Skin.IdOrganization = 0;//idOrganization; -> modificato Mirco
                        break;
                        
                    case SkinDisplayType.Community:
                        //Agginto Mirco
                        content.Skin.IsForPortal = false;
                        content.Skin.IdCommunity = Convert.ToInt32(idItem);
                        community = Manager.GetCommunity(content.Skin.IdCommunity);
                        if (community != null)
                        {
                            content.Title = community.Name;
                            content.Skin.IdOrganization = community.IdOrganization;
                        }
                        else
                        {
                            content.Skin.IdOrganization = idOrganization;
                            content.Title = GetOrganizationName(idOrganization);
                        }
                        break;

                    case SkinDisplayType.CurrentCommunity:
                        content.Skin.IsForPortal = false;
                        content.Skin.IdCommunity = Convert.ToInt32(idItem);
                        community = Manager.GetCommunity(content.Skin.IdCommunity);
                        if (community != null)
                        {
                            content.Title = community.Name;
                            content.Skin.IdOrganization = community.IdOrganization;
                        }
                        else
                        {
                            content.Skin.IdOrganization = idOrganization;
                            content.Title = GetOrganizationName(idOrganization);
                        }
                        break;
                    case SkinDisplayType.Organization:
                        content.Skin.IsForPortal = false;
                        content.Skin.IdOrganization = Convert.ToInt32(idItem);
                        content.Title = GetOrganizationName(content.Skin.IdOrganization);
                        break;
                    case SkinDisplayType.Module:
                        content.Skin.IsForPortal = false;
                        content.Skin.IdCommunity = Convert.ToInt32(idCommunity);
                        if (community!=null)
                            content.Skin.IdOrganization = community.IdOrganization;
                        else
                            content.Skin.IdOrganization = idOrganization;
                            content.Skin.IdSkin = idItem;
                        break;
                    default:
                        //Aggiunto - Mirco (Come Portal. Se rimane così toglierei Portal)
                        content.Skin.IsForPortal = true;
                        content.Skin.IdCommunity = 0;
                        content.Skin.IdOrganization = 0;
                        break;
                }

                return content;
            }
            private String GetOrganizationName(Int32 idOrganization) {
                return (from o in Manager.GetIQ<Organization>() where o.Id == idOrganization select o.Name).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            public Boolean SkinHasMultipleAssociations(long idSkin, lm.Comol.Core.DomainModel.ModuleObject source)
            {
                Boolean multiple = false;
                try
                {
                    multiple = (from ma in Manager.GetIQ<ModuleAssociation>()
                             where ma.Deleted == Core.DomainModel.BaseStatusDeleted.None && ma.IdSkin == idSkin
                             && ma.OwnerLongID != source.ObjectLongID && ma.OwnerTypeID != source.ObjectTypeID && ma.IdModule != source.ServiceID && ma.OwnerFullyQualifiedName != source.FQN
                             select ma).Any();
                }
                catch (Exception ex)
                {

                }
                return multiple;
            }

            public Boolean DeleteModuleSkin(long idSkin, Boolean physical, String basePath)
            {
                Boolean result = false;
                try
                {
                    Manager.BeginTransaction();
                    Person person = Manager.GetPerson(UC.CurrentUserID);
                    var queryAssociation = (from ma in Manager.GetIQ<ModuleAssociation>() where ma.IdSkin == idSkin select ma);
                    if (physical)
                        Manager.DeletePhysicalList(queryAssociation.ToList());
                    else
                    {
                        List<ModuleAssociation> items = queryAssociation.Where(i => i.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList();
                        items.ForEach(i => i.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                        items.ForEach(i => i.Deleted |= Core.DomainModel.BaseStatusDeleted.Manual);
                        Manager.SaveOrUpdateList(items);

                    }
                    Domain.Skin skin = Manager.Get<Domain.Skin>(idSkin);
                    var queryShare = (from s in Manager.GetIQ<Skin_ShareModule>()
                                      where s.Skin==skin
                                      select s);

                    if (physical)
                        Manager.DeletePhysicalList(queryShare.ToList());
                    else
                    {
                        List<Skin_ShareModule> shares = queryShare.Where(i => i.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList();
                        shares.ForEach(i => i.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
                        shares.ForEach(i => i.Deleted |= Core.DomainModel.BaseStatusDeleted.Manual);
                        Manager.SaveOrUpdateList(shares);
                    }

                    
                    if(physical)
                        SkinFileManagement.EraseDir(idSkin, basePath);

                    if (skin != null && physical)
                        Manager.DeletePhysical(skin);
                    else if (skin != null)
                    {
                        skin.Deleted = Core.DomainModel.BaseStatusDeleted.Manual;
                        skin.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        Manager.SaveOrUpdate(skin);
                    }
                    Manager.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                    result = false;
                }
                return result;
            }

            //private Boolean DeleteSkin(DtoDisplaySkin skin, Boolean physical, String basePath)
            //{
            //    Boolean result = false;
            //    try
            //    {
            //        Manager.BeginTransaction();
            //        Person person = Manager.GetPerson(UC.CurrentUserID);
            //        var queryAssociation = (from ma in Manager.GetIQ<ModuleAssociation>() select ma);
            //        switch (skin.Type) { 
            //            case SkinDisplayType.Module:
            //                queryAssociation = queryAssociation.Where(ma => ma.IdSkin == skin.Id);
            //                break;
            //            case SkinDisplayType.Community:
            //                queryAssociation = queryAssociation.Where(ma => ma.IdCommunity == skin.Id);
            //                break;
            //            case SkinDisplayType.Organization:
            //                queryAssociation = queryAssociation.Where(ma => ma.IdOrganization == skin.Id);
            //                break;
            //            case SkinDisplayType.Portal:
            //                queryAssociation = queryAssociation.Where(ma => ma.IsPortal);
            //                break;
            //        }

            //        if (physical)
            //            Manager.DeletePhysicalList(queryAssociation.ToList());
            //        else
            //        {
            //            List<ModuleAssociation> items = queryAssociation.Where(i => i.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList();
            //            items.ForEach(i => i.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
            //            items.ForEach(i => i.Deleted |= Core.DomainModel.BaseStatusDeleted.Manual);
            //            Manager.SaveOrUpdateList(items);

            //        }
            //        Manager.Commit();
            //        result = true;
            //    }
            //    catch (Exception ex) {
            //        if (Manager.IsInTransaction())
            //            Manager.RollBack();
            //        result = false;
            //    }
            //    return result;
            //}

            //private Boolean DeleteSkinAssociation(Person person, DtoDisplaySkin skin, Boolean physical)
            //{
            //    Boolean result = false;
            //    var queryAssociation = (from ma in Manager.GetIQ<ModuleAssociation>() select ma);
            //    switch (skin.Type)
            //    {
            //        case SkinDisplayType.Module:
            //            queryAssociation = queryAssociation.Where(ma => ma.IdSkin == skin.Id);
            //            break;
            //        case SkinDisplayType.Community:
            //            queryAssociation = queryAssociation.Where(ma => ma.IdCommunity == skin.Id);
            //            break;
            //        case SkinDisplayType.Organization:
            //            queryAssociation = queryAssociation.Where(ma => ma.IdOrganization == skin.Id);
            //            break;
            //        case SkinDisplayType.Portal:
            //            queryAssociation = queryAssociation.Where(ma => ma.IsPortal);
            //            break;
            //    }

            //    if (physical)
            //        Manager.DeletePhysicalList(queryAssociation.ToList());
            //    else
            //    {
            //        List<ModuleAssociation> items = queryAssociation.Where(i => i.Deleted == Core.DomainModel.BaseStatusDeleted.None).ToList();
            //        items.ForEach(i => i.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress));
            //        items.ForEach(i => i.Deleted |= Core.DomainModel.BaseStatusDeleted.Manual);
            //        Manager.SaveOrUpdateList(items);

            //    }
            //    result = true;
            //    return result;
            //}

            public Boolean ObjectHasSkinAssociation(Int32 idCommunity, lm.Comol.Core.DomainModel.ModuleObject source)
            {
                Boolean found = false;
                try
                {
                    found = (from ma in Manager.GetIQ<ModuleAssociation>()
                             where ma.Deleted == Core.DomainModel.BaseStatusDeleted.None && ma.OwnerLongID == source.ObjectLongID && ma.OwnerTypeID == source.ObjectTypeID && ma.IdModule == source.ServiceID && ma.OwnerFullyQualifiedName== source.FQN 
                             select ma).Any();
                }
                catch (Exception ex) {
                    
                }
                return found;
            }

            public Boolean CloneSkinAssociation(int idUser,Int32 idModule, Int32 idCommunity, long idOldModuleItem, long idNewModuleItem, Int32 idItemType, String fullyQualifiedName) {
                Boolean cloned = false;
                try
                {
                    Manager.BeginTransaction();
                    Person person = Manager.GetPerson(idUser);
                    ModuleAssociation oldModuleAssociation = (from ma in Manager.GetIQ<ModuleAssociation>()
                                                              where ma.Deleted == Core.DomainModel.BaseStatusDeleted.None && ma.OwnerLongID == idOldModuleItem && ma.OwnerTypeID == idItemType && ma.IdModule == idModule && ma.OwnerFullyQualifiedName == fullyQualifiedName
                                                              select ma).Skip(0).Take(1).ToList().FirstOrDefault();
                    if (oldModuleAssociation != null)
                    {
                        ModuleAssociation association = new ModuleAssociation();
                        association.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        association.IdCommunity = idCommunity;
                        association.IdModule = idModule;
                        association.OwnerLongID = idNewModuleItem;
                        association.OwnerTypeID = idItemType;
                        association.OwnerFullyQualifiedName = fullyQualifiedName;
                        association.IdCommunity = oldModuleAssociation.IdCommunity;
                        association.IsPortal = oldModuleAssociation.IsPortal;
                        association.IdOrganization =oldModuleAssociation.IdOrganization;
                        association.IdSkin = oldModuleAssociation.IdSkin;
                        Manager.SaveOrUpdate(association);
                        Skin_ShareModule oldShare = (from s in Manager.GetIQ<Skin_ShareModule>()
                                                  where s.IdModule == association.IdModule && s.OwnerLongID == idOldModuleItem && s.OwnerTypeID == association.OwnerTypeID
                                 && s.IdCommunity == idCommunity && s.OwnerFullyQualifiedName == fullyQualifiedName
                                 select s).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (oldShare != null)
                        {
                            Skin_ShareModule s = new Skin_ShareModule();
                            s.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            s.IdCommunity = idCommunity;
                            s.IdModule = association.IdModule;
                            s.OwnerLongID = association.OwnerLongID;
                            s.OwnerTypeID = association.OwnerTypeID;
                            s.OwnerFullyQualifiedName = association.OwnerFullyQualifiedName;
                            s.Skin = oldShare.Skin;
                            Manager.SaveOrUpdate(s);
                        }
                    }
                    else
                        cloned = true;
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                    cloned = false;
                }
                return cloned;
            }
            public Boolean DeleteSkinAssociation( Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, String fullyQualifiedName)
            {
                Boolean deleted = false;
                try
                {
                    Manager.BeginTransaction();
                    List<ModuleAssociation> assignments = (from ma in Manager.GetIQ<ModuleAssociation>()
                                                           where ma.OwnerLongID == idModuleItem && ma.OwnerTypeID == idItemType && ma.IdModule == idModule && ma.OwnerFullyQualifiedName == fullyQualifiedName
                                                           select ma).ToList();
                    if (assignments.Any())
                        Manager.DeletePhysicalList(assignments);

                    List<Skin_ShareModule> skinShares = (from ma in Manager.GetIQ<Skin_ShareModule>()
                                                           where ma.OwnerLongID == idModuleItem && ma.OwnerTypeID == idItemType && ma.IdModule == idModule && ma.OwnerFullyQualifiedName == fullyQualifiedName
                                                           select ma).ToList();
                    if (skinShares.Any())
                        Manager.DeletePhysicalList(skinShares);
                   
                    Manager.Commit();
                    deleted = true;
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                    deleted = false;
                }
                return deleted;
            }


            public List<lm.Comol.Modules.Standard.Skin.Presentation.SkinView> GetAvailableViews(SkinType type) {
                List<lm.Comol.Modules.Standard.Skin.Presentation.SkinView> items = new List<lm.Comol.Modules.Standard.Skin.Presentation.SkinView>();
                if (type!= SkinType.Module){
                    items.Add(Presentation.SkinView.Css);
                    items.Add(Presentation.SkinView.Images);
                }
                items.Add(Presentation.SkinView.HeaderLogo);
                items.Add(Presentation.SkinView.FooterLogos);
                items.Add(Presentation.SkinView.FooterText);
                if (type== SkinType.Module)
                    items.Add(Presentation.SkinView.Templates);
                else
                    items.Add(Presentation.SkinView.Shares);
                return items;
            }
                //private DtoDisplaySkin GetDefaultSkinForUser(Person person, Int32 idCommunity)
            //{
               
            //    Int32 idDefOrganization = (from o in Manager.GetIQ<OrganizationProfiles>()
            //                               where o.Profile == person && o.isDefault select o.OrganizationID).Skip(0).Take(1).ToList().FirstOrDefault();
            //    Int32 idOrganization = idDefOrganization;
            //    Community community = Manager.GetCommunity(idCommunity);
            //    if (community != null)
            //        idOrganization = community.IdOrganization;

            //    var queryCommunity = (from sc in Manager.GetIQ<Skin_ShareCommunity>()
            //                          where sc.CommunityId == idCommunity && sc.Deleted == Core.DomainModel.BaseStatusDeleted.None
            //                          select sc);
            //    var queryOrganization = (from so in Manager.GetIQ<Skin_ShareOrganization>()
            //                          where so.Deleted == Core.DomainModel.BaseStatusDeleted.None
            //                          select so);

            //    if (queryCommunity.Any())
            //    {
            //        return (from sc in Manager.GetIQ<Skin_ShareCommunity>()
            //                where sc.CommunityId == idCommunity && sc.Deleted == Core.DomainModel.BaseStatusDeleted.None
            //                select new DtoDisplaySkin() { Id = idCommunity, IsValid = true, Name = sc.Skin.Name, Type = SkinDisplayType.Community | SkinDisplayType.CurrentCommunity }).Skip(0).Take(1).ToList().FirstOrDefault();
            //    }
            //    else if (queryOrganization.Where(o=> o.OrganizationId == idOrganization).Any())
            //    {
            //        return (from so in Manager.GetIQ<Skin_ShareOrganization>()
            //                where so.OrganizationId == idOrganization && so.Deleted == Core.DomainModel.BaseStatusDeleted.None
            //                select new DtoDisplaySkin() { Id = idOrganization, IsValid = true, Name = so.Skin.Name, Type = SkinDisplayType.Organization | SkinDisplayType.CurrentCommunity }).Skip(0).Take(1).ToList().FirstOrDefault();
                
            //    }
            //    else if (queryOrganization.Where(o => o.OrganizationId == idDefOrganization).Any())
            //    {
            //        return (from so in Manager.GetIQ<Skin_ShareOrganization>()
            //                where so.OrganizationId == idDefOrganization && so.Deleted == Core.DomainModel.BaseStatusDeleted.None
            //                select new DtoDisplaySkin() { Id = idOrganization, IsValid = true, Name = so.Skin.Name, Type = SkinDisplayType.Organization | SkinDisplayType.CurrentCommunity }).Skip(0).Take(1).ToList().FirstOrDefault();

            //    }
            //    else
            //        return (from s in Manager.GetIQ<Domain.Skin>()
            //                where s.Deleted == BaseStatusDeleted.None && !s.IsModule && s.IsPortal
            //                select new DtoDisplaySkin() { Id = s.Id, IsValid = true, Name = s.Name, Type = SkinDisplayType.Portal | SkinDisplayType.CurrentCommunity }).Skip(0).Take(1).ToList().FirstOrDefault();
            //}
        #endregion

            public String GetModuleCode(Int32 idModule) {
                String result = "";
                try
                {
                    result = (from m in Manager.GetIQ<ModuleDefinition>() where m.Id == idModule select m.Code).Skip(0).Take(1).ToList().FirstOrDefault();
                }
                catch (Exception ex) { 
                
                }
                return result;
            }

//#region "Skin To Template"

//            /// <summary>
//            /// A partire da ID Comunità ed Organizzazione,
//            /// recupera la skin corretta e la converte in DocTemplate.Template, impostando SOLO HEADER E FOOTER
//            /// </summary>
//            /// <param name="CommunityId"></param>
//            /// <param name="UserOrganizationId"></param>
//            /// <param name="DefLangCode">Lingua di default del sistema</param>
//            /// <param name="UsrLangCode">Lingua dell'utente</param>
//            /// <param name="Title">Il titolo nell'Header. SE non viene impostato e SE presente verrà usato l'ALT impostato per il logo Header</param>
//            /// <param name="BaseURL">L'url base, es: "http://demo.comunitaonline.unitn.it/"</param>
//            /// <returns>
//            ///     Un template con Header e Footer impostati correttamente.
//            /// </returns>
//            /// <remarks>
//            ///     Ad eccezione di Header e Footer, TUTTI gli altri parametri saranno NULL!!
//            /// </remarks>
//            public DocTemplate.Template GetCommunityTemplate(
//                int CommunityId, int UserOrganizationId, 
//                String DefLangCode, String UsrLangCode,
//                String Title, String BaseURL, String VirtualPath)
//            {
//                Int64 LogoSkinId_Header = 0;
//                Int64 LogoSkinId_Footer = 0;

//                Domain.DTO.DtoSkinsID SkinsId = GetSkinIds(CommunityId, UserOrganizationId, VirtualPath, DefLangCode);
//                Domain.Skin DefSkin = new Domain.Skin();

//                Domain.Skin CommunitySkin = Manager.Get<Domain.Skin>(SkinsId.CommunityId);
//                Domain.Skin OrganizationSkin = Manager.Get<Domain.Skin>(SkinsId.OrganizationId);
//                Domain.Skin PortalSkin = Manager.Get<Domain.Skin>(SkinsId.PortalId);

//                DefSkin.HeaderTemplate = null;
//                DefSkin.HeaderLogos = null;
//                DefSkin.FooterTemplate = null;
//                DefSkin.FooterText = null;
//                DefSkin.FooterLogos = null;

//                if (PortalSkin != null)
//                {
//                    if (PortalSkin.HeaderTemplate != null)
//                        DefSkin.HeaderTemplate = PortalSkin.HeaderTemplate;

//                    if (PortalSkin.HeaderLogos != null)
//                    {
//                        DefSkin.HeaderLogos = PortalSkin.HeaderLogos;
//                        LogoSkinId_Header = PortalSkin.Id;
//                    }
                        

//                    if (PortalSkin.FooterTemplate != null)
//                        DefSkin.FooterTemplate = PortalSkin.FooterTemplate;

//                    if (PortalSkin.FooterText != null)
//                        DefSkin.FooterText = PortalSkin.FooterText;

//                    if (PortalSkin.FooterLogos != null && PortalSkin.FooterLogos.Count() > 0)
//                    {
//                        DefSkin.FooterLogos = PortalSkin.FooterLogos;
//                        LogoSkinId_Footer = PortalSkin.Id;
//                    }
                        
//                }


//                if (OrganizationSkin != null)
//                {
//                    if (OrganizationSkin.HeaderTemplate != null)
//                        DefSkin.HeaderTemplate = OrganizationSkin.HeaderTemplate;

//                    if (OrganizationSkin.HeaderLogos != null)
//                    {
//                        DefSkin.HeaderLogos = OrganizationSkin.HeaderLogos;
//                        LogoSkinId_Header = OrganizationSkin.Id;
//                    }
                        

//                    if (OrganizationSkin.FooterTemplate != null)
//                        DefSkin.FooterTemplate = OrganizationSkin.FooterTemplate;

//                    if (OrganizationSkin.FooterText != null)
//                        DefSkin.FooterText = OrganizationSkin.FooterText;

//                    if (OrganizationSkin.FooterLogos != null && OrganizationSkin.FooterLogos.Count() > 0)
//                    {
//                        DefSkin.FooterLogos = OrganizationSkin.FooterLogos;
//                        LogoSkinId_Footer = OrganizationSkin.Id;
//                    }
                        
//                }

//                if(CommunitySkin != null)
//                {
//                    if(CommunitySkin.HeaderTemplate != null)
//                        DefSkin.HeaderTemplate = CommunitySkin.HeaderTemplate;

//                    if (CommunitySkin.HeaderLogos != null)
//                    {
//                        DefSkin.HeaderLogos = CommunitySkin.HeaderLogos;
//                        LogoSkinId_Header = CommunitySkin.Id;
//                    }
                        

//                    if(CommunitySkin.FooterTemplate != null)
//                        DefSkin.FooterTemplate = CommunitySkin.FooterTemplate;

//                    if(CommunitySkin.FooterText != null)
//                        DefSkin.FooterText = CommunitySkin.FooterText;

//                    if (CommunitySkin.FooterLogos != null && CommunitySkin.FooterLogos.Count() > 0)
//                    {
//                        DefSkin.FooterLogos = CommunitySkin.FooterLogos;
//                        LogoSkinId_Header = CommunitySkin.Id;
//                    }
                        
//                }

//                return SkinToTemplate(DefSkin, DefLangCode, UsrLangCode, Title, BaseURL + VirtualPath, LogoSkinId_Header, LogoSkinId_Footer);
//            }
    
//            /// <summary>
//            /// Dato l'ID di una skin,
//            /// la recupera e la converte in DocTemplate.Template, impostando SOLO HEADER E FOOTER
//            /// </summary>
//            /// <param name="SkinID">ID della skin</param>
//            /// <param name="DefLangCode">Lingua di default del sistema</param>
//            /// <param name="UsrLangCode">Lingua dell'utente</param>
//            /// <param name="Title">Il titolo nell'Header. SE non viene impostato e SE presente verrà usato l'ALT impostato per il logo Header</param>
//            /// <param name="BaseURL">L'url base, es: "http://demo.comunitaonline.unitn.it/"</param>
//            /// <returns>
//            ///     Un template con Header e Footer impostati correttamente.
//            /// </returns>
//            /// <remarks>
//            ///     Ad eccezione di Header e Footer, TUTTI gli altri parametri saranno NULL!!
//            /// </remarks>
//            public DocTemplate.Template GetSkinTemplate(
//                Int64 SkinID,
//                String DefLangCode, String UsrLangCode,
//        String Title, String BaseURL, String VirtualPath)
//            {
//                Int64 SkinHeaderId = 0;
//                Int64 SkinFooterId = 0;

//                Domain.DTO.DtoSkinsID SkinsId = GetSkinIds(0, 0, "", DefLangCode);
//                Domain.Skin DefSkin = new Domain.Skin();

//                Domain.Skin SrvSkin = Manager.Get<Domain.Skin>(SkinID);
//                Domain.Skin PortalSkin = Manager.Get<Domain.Skin>(SkinsId.PortalId);

//                if (PortalSkin != null)
//                {
//                    if (PortalSkin.HeaderTemplate != null)
//                        DefSkin.HeaderTemplate = PortalSkin.HeaderTemplate;
//                        //SkinHeaderId = PortalSkin.Id;

//                    if (PortalSkin.HeaderLogos != null)
//                    {
//                        DefSkin.HeaderLogos = PortalSkin.HeaderLogos;
//                        SkinHeaderId = PortalSkin.Id;
//                    }

//                    if (PortalSkin.FooterTemplate != null)
//                        DefSkin.FooterTemplate = PortalSkin.FooterTemplate;
//                        //SkinFooterId = PortalSkin.Id;

//                    if (PortalSkin.FooterText != null)
//                        DefSkin.FooterText = PortalSkin.FooterText;

//                    if (PortalSkin.FooterLogos != null && PortalSkin.FooterLogos.Count() > 0)
//                    {
//                        DefSkin.FooterLogos = PortalSkin.FooterLogos;
//                        SkinFooterId = PortalSkin.Id;
//                    }
//                }

//                if (SrvSkin != null)
//                {
//                    if (SrvSkin.HeaderTemplate != null)
//                        DefSkin.HeaderTemplate = SrvSkin.HeaderTemplate;

//                    if (SrvSkin.HeaderLogos != null)
//                    {
//                        DefSkin.HeaderLogos = SrvSkin.HeaderLogos;
//                        SkinHeaderId = DefSkin.Id;
//                    }

//                    if (SrvSkin.FooterTemplate != null)
//                        DefSkin.FooterTemplate = SrvSkin.FooterTemplate;

//                    if (SrvSkin.FooterText != null)
//                        DefSkin.FooterText = SrvSkin.FooterText;

//                    if (SrvSkin.FooterLogos != null && SrvSkin.FooterLogos.Count() > 0)
//                        DefSkin.FooterLogos = SrvSkin.FooterLogos;
//                        SkinFooterId = DefSkin.Id;
//                }

//                return SkinToTemplate(DefSkin, DefLangCode, UsrLangCode, Title, BaseURL + VirtualPath, SkinHeaderId, SkinFooterId);
//            }


    
    #region "DocTemplate - GET"

        //    /// <summary>
        ///// Restituisce il template definitivo
        ///// </summary>
        ///// <param name="BaseURL">BaseUrL: /Comol_Elle3/</param>
        ///// <param name="VirtualSkinPath">Virtual Skin Path: /File/Skins/</param>
        ///// <param name="DefaultLangCode">Lingua default sistema</param>
        ///// <param name="UserLangcode">Lingua dell'utente (se serve)</param>
        ///// <param name="Title">Eventual testo dell'Header</param>
        ///// <param name="CommunityId">Id Comunità da cui recuperare la skin</param>
        ///// <param name="OrganizationId">Id Organizzazione da vui recuperare la skin</param>
        ///// <returns>Il template vero e proprio</returns>
        //public DocTemplate.DTO_Template GetTemplateCommunity(
        //    String BaseURL, String VirtualSkinPath, String DefaultLangCode, String UserLangcode, String Title,
        //    Int32 CommunityId, Int32 OrganizationId)
        //{
        //    return GetTemplateCommunitySkin(
        //        BaseURL, VirtualSkinPath, DefaultLangCode, UserLangcode, Title,
        //        CommunityId, OrganizationId, 0, null);
        //}

        ///// <summary>
        ///// Restituisce il template definitivo
        ///// </summary>
        ///// <param name="BaseURL">BaseUrL: /Comol_Elle3/</param>
        ///// <param name="VirtualSkinPath">Virtual Skin Path: /File/Skins/</param>
        ///// <param name="DefaultLangCode">Lingua default sistema</param>
        ///// <param name="UserLangcode">Lingua dell'utente (se serve)</param>
        ///// <param name="Title">Eventual testo dell'Header</param>
        ///// <param name="CommunityId">Id Comunità da cui recuperare la skin</param>
        ///// <param name="OrganizationId">Id Organizzazione da vui recuperare la skin</param>
        ///// <param name="TemplateConfig">Template da configurazione (vedi funzioni in Master)</param>
        ///// <returns>Il template vero e proprio</returns>
        //public DocTemplate.DTO_Template GetTemplateCommunity(
        //    String BaseURL, String VirtualSkinPath, String DefaultLangCode, String UserLangcode, String Title,
        //    Int32 CommunityId, Int32 OrganizationId, DocTemplate.DTO_Template TemplateConfig)
        //{
        //    return GetTemplateCommunitySkin(
        //        BaseURL, VirtualSkinPath, DefaultLangCode, UserLangcode, Title,
        //        CommunityId, OrganizationId, 0, TemplateConfig);
        //}

        ///// <summary>
        ///// Restituisce il template definitivo
        ///// </summary>
        ///// <param name="BaseURL">BaseUrL: /Comol_Elle3/</param>
        ///// <param name="VirtualSkinPath">Virtual Skin Path: /File/Skins/</param>
        ///// <param name="DefaultLangCode">Lingua default sistema</param>
        ///// <param name="UserLangcode">Lingua dell'utente (se serve)</param>
        ///// <param name="Title">Eventual testo dell'Header</param>
        ///// <param name="SkinId">Id Skin (per servizi esterni o configurati con skin proprie)</param>
        ///// <returns>Il template vero e proprio</returns>
        //public DocTemplate.DTO_Template GetTemplateSkin(
        //    String BaseURL, String VirtualSkinPath, String DefaultLangCode, String UserLangcode, String Title,
        //    Int64 SkinId)
        //{
        //    return GetTemplateCommunitySkin(
        //        BaseURL, VirtualSkinPath, DefaultLangCode, UserLangcode, Title,
        //        0, 0, SkinId, null);
        //}

        ///// <summary>
        ///// Restituisce il template definitivo
        ///// </summary>
        ///// <param name="BaseURL">BaseUrL: /Comol_Elle3/</param>
        ///// <param name="VirtualSkinPath">Virtual Skin Path: /File/Skins/</param>
        ///// <param name="DefaultLangCode">Lingua default sistema</param>
        ///// <param name="UserLangcode">Lingua dell'utente (se serve)</param>
        ///// <param name="Title">Eventual testo dell'Header</param>
        ///// <param name="SkinId">Id Skin (per servizi esterni o configurati con skin proprie)</param>
        ///// <param name="TemplateConfig">Template da configurazione (vedi funzioni in Master)</param>
        ///// <returns>Il template vero e proprio</returns>
        //public DocTemplate.DTO_Template GetTemplateSkin(
        //    String BaseURL, String VirtualSkinPath, String DefaultLangCode, String UserLangcode, String Title,
        //    Int64 SkinId, DocTemplate.DTO_Template TemplateConfig)
        //{
        //    return GetTemplateCommunitySkin(
        //        BaseURL, VirtualSkinPath, DefaultLangCode, UserLangcode, Title,
        //        0, 0, SkinId, TemplateConfig);
        //}

        ///// <summary>
        ///// Restituisce il template definitivo
        ///// </summary>
        ///// <param name="BaseURL">BaseUrL: /Comol_Elle3/</param>
        ///// <param name="VirtualSkinPath">Virtual Skin Path: /File/Skins/</param>
        ///// <param name="DefaultLangCode">Lingua default sistema</param>
        ///// <param name="UserLangcode">Lingua dell'utente (se serve)</param>
        ///// <param name="Title">Eventual testo dell'Header</param>
        ///// <param name="CommunityId">Id Comunità da cui recuperare la skin</param>
        ///// <param name="OrganizationId">Id Organizzazione da vui recuperare la skin</param>
        ///// <param name="SkinId">Id Skin (per servizi esterni o configurati con skin proprie)</param>
        ///// <param name="TemplateConfig">Template da configurazione (vedi funzioni in Master)</param>
        ///// <returns></returns>
        //public DocTemplate.DTO_Template GetTemplateCommunitySkin(
        //    String BaseURL, String VirtualSkinPath, String DefaultLangCode, String UserLangcode, String Title,
        //    Int32 CommunityId, Int32 OrganizationId, 
        //    Int64 SkinId,
        //    DocTemplate.DTO_Template TemplateConfig)
        //{
        //    return GetTemplateCommunitySkin(
        //        BaseURL, VirtualSkinPath, DefaultLangCode, UserLangcode, Title,
        //        CommunityId, OrganizationId,
        //        SkinId,
        //        TemplateConfig, 0);
        //}

        public DocTemplate.DTO_Template GetTemplateCommunitySkin(
           String BaseURL, String VirtualSkinPath, String DefaultLangCode, String UserLangcode, String title,
           Int32 idCommunity, Int32 idOrganization,
           Int64 idSkin,
           DocTemplate.DTO_Template TemplateConfig,
            int Fontsize)
        {
            String BaseSkinUrl = BaseURL + VirtualSkinPath;

            IList<DocTemplate.DTO_Template> Templates = new List<DocTemplate.DTO_Template>();

            
            //DocTemplate.Template DefTemplate = new DocTemplate.Template();
            //DefTemplate.Footer = new DocTemplate.TemplateHeaderFooter();
            //DefTemplate.Header = new DocTemplate.TemplateHeaderFooter();


            //Get Community Template

            DocTemplate.DTO_Template TemplatePortal = null;
            DocTemplate.DTO_Template TemplateCommunity = null;
            DocTemplate.DTO_Template TemplateOrganization = null;
            
            Domain.DTO.DtoSkinsID SkinsId = GetSkinIds(idCommunity, idOrganization, VirtualSkinPath, DefaultLangCode);
            Domain.Skin PortalSkin = Manager.Get<Domain.Skin>(SkinsId.PortalId);
            Domain.Skin OrganizationSkin = Manager.Get<Domain.Skin>(SkinsId.OrganizationId);
            Domain.Skin CommunitySkin = Manager.Get<Domain.Skin>(SkinsId.CommunityId);

            if (PortalSkin != null)
                TemplatePortal = SkinToTemplate(PortalSkin, DefaultLangCode, UserLangcode, title, BaseSkinUrl, Fontsize);
            if (OrganizationSkin != null)
                TemplateOrganization = SkinToTemplate(OrganizationSkin, DefaultLangCode, UserLangcode, title, BaseSkinUrl, Fontsize);
            if (CommunitySkin != null)
                TemplateCommunity = SkinToTemplate(CommunitySkin, DefaultLangCode, UserLangcode, title, BaseSkinUrl, Fontsize);

            //Get Skin Template

            DocTemplate.DTO_Template TemplateSkin = null;
            
            Domain.Skin SkinSkin = Manager.Get<Domain.Skin>(idSkin);

            if (SkinSkin != null)
                TemplateSkin = SkinToTemplate(SkinSkin, DefaultLangCode, UserLangcode, title, BaseSkinUrl);

            //Create list

            if (TemplateConfig != null)
                Templates.Add(TemplateConfig);

            if (TemplatePortal != null)
                Templates.Add(TemplatePortal);

            if (TemplateOrganization != null)
                Templates.Add(TemplateOrganization);

            if (TemplateCommunity != null)
                Templates.Add(TemplateCommunity);

            if (TemplateSkin != null)
                Templates.Add(TemplateSkin);

            //Compile DefTempalte

            return CompileTemplate(Templates);
        }

        /// <summary>
        /// Data una lista di Template, compila un nuovo template con i pezzi definitivi.
        /// </summary>
        /// <param name="Templates">Lista di template sorgente.</param>
        /// <returns>Il template definitivo</returns>
        /// <remarks>NOTA: dev'essere ordinato correttamente, es:
        /// config, portal, organization, community, skin...</remarks>
        public static DocTemplate.DTO_Template CompileTemplate(IList<DocTemplate.DTO_Template> Templates)
        {
            DocTemplate.DTO_Template CompTemplate = new DocTemplate.DTO_Template();
            CompTemplate.Header = new DocTemplate.DTO_HeaderFooter();
            CompTemplate.Header.Center = null;
            CompTemplate.Footer = new DocTemplate.DTO_HeaderFooter();
            CompTemplate.Footer.Center = null;

            foreach (DocTemplate.DTO_Template template in Templates)
            {
                if (template != null)
                {
                    if (template.Header != null)
                    {
                        if (template.Header.Left != null)
                            CompTemplate.Header.Left = template.Header.Left;
                        if (template.Header.Right != null)
                            CompTemplate.Header.Right = template.Header.Right;
                    }
                    if (template.Footer != null)
                    {
                        if (template.Footer.Left != null)
                            CompTemplate.Footer.Left = template.Footer.Left;
                        if (template.Footer.Right != null)
                            CompTemplate.Footer.Right = template.Footer.Right;
                    }
                }
            }

            return CompTemplate;
        }

        public static DocTemplate.DTO_Template SkinToTemplate(
        Domain.Skin SourceSkin,
        String DefLangCode, String UsrLangCode,
        String Title, String BaseSkinURL)
        {
            return SkinToTemplate(SourceSkin, DefLangCode, UsrLangCode, Title, BaseSkinURL, 0);
        }

        /// <summary>
        /// Trasforma una Skin in un Template
        /// </summary>
        /// <param name="SourceSkin">La skin sorgente</param>
        /// <param name="DefLangCode">Lingua default sistema</param>
        /// <param name="UsrLangCode">Lingua dell'utente</param>
        /// <param name="Title">Titolo da impostare nell'Header</param>
        /// <param name="BaseSkinURL">Base Skin url = Base Url + Virtual Skin Path</param>
        /// <param name="FooterFontSize">Dimensione carattere in pixel del footer:
        ///     0 (default) reimpostato a 8px
        ///     -n  (negativo)  non viene impostato il font-size (Es: preview web)
        ///     n   (positivo)  viene impostato con quel valore  (Es: export pdf/rtf)
        /// </param>
        /// <returns>Tempalte</returns>
        /// <remarks>NON USARE se la skin non è una Skin, ma è stata creata da varie skin (vecchie versioni)</remarks>
        public static DocTemplate.DTO_Template SkinToTemplate(
        Domain.Skin SourceSkin,
        String DefLangCode, String UsrLangCode,
        String Title, String BaseSkinURL, int FooterFontSize)
        {
            if (FooterFontSize == 0)
                FooterFontSize = 8;

            DocTemplate.DTO_Template defTemplate = new DocTemplate.DTO_Template();
            defTemplate.Header = null;
            defTemplate.Footer = null;

            if (SourceSkin != null)
            {
                //HEADER
                DocTemplate.DTO_HeaderFooter THeader = new DocTemplate.DTO_HeaderFooter();

                //LOGO
                DocTemplate.DTO_ElementImage HLogo = null;

                if (SourceSkin.HeaderLogos != null && SourceSkin.HeaderLogos.Count > 0)
                {
                    HeaderLogo Logo = (from HeaderLogo lg in SourceSkin.HeaderLogos
                                       where lg.LangCode == UsrLangCode
                                       select lg).FirstOrDefault();

                    if (Logo == null)
                    {
                        Logo = (from HeaderLogo lg in SourceSkin.HeaderLogos
                                where lg.LangCode == DefLangCode
                                select lg).FirstOrDefault();
                    }

                    if (Logo != null)
                    {
                        HLogo = new DocTemplate.DTO_ElementImage();
                        HLogo.Height = 82;
                        HLogo.Width = 0;
                        HLogo.Alignment = Core.DomainModel.DocTemplateVers.ElementAlignment.MiddleCenter;
                        HLogo.Path = Skin.Business.SkinFileManagement.GetLogoVirtualFullPath(BaseSkinURL, SourceSkin.Id, Logo.Id, Logo.ImageUrl);
                        //HLogo.Path = BaseURL + Logo.ImageUrl;

                        if (String.IsNullOrEmpty(Title) && !String.IsNullOrEmpty(Logo.Alt))
                            Title = Logo.Alt;
                    }
                }
                if (HLogo != null)
                    THeader.Left = HLogo;

                // TITLE (Se viene passata stringa vuota E SE al logo è stato assegnato un ALT, verrà usato quello... (EVENTUALMENTE RIVEDERE!!!)

                if (!string.IsNullOrEmpty(Title))
                { 
                    DocTemplate.DTO_ElementText TTitle = new DocTemplate.DTO_ElementText();
                    TTitle.IsHTML = true;
                    TTitle.Alignment = Core.DomainModel.DocTemplateVers.ElementAlignment.MiddleCenter;// Core.DomainModel.Helpers.Export.ElementAlignment.MiddleCenter;
                    TTitle.Text = "<h1>" + Title + "</h1>";
                    THeader.Right = TTitle;
                } else {
                    THeader.Right = null;
                }

                // elemento vuoto
                THeader.Center = null;

                defTemplate.Header = THeader;




                //FOOTER
                DocTemplate.DTO_HeaderFooter TFooter = new DocTemplate.DTO_HeaderFooter();
                TFooter.Left = null;
                TFooter.Center = null;
                TFooter.Right = null;

                //Images


                if (SourceSkin.FooterLogos != null && SourceSkin.FooterLogos.Count() > 0)
                {
                    DocTemplate.DTO_ElementImageMulti mIElement = new DocTemplate.DTO_ElementImageMulti();
                    mIElement.ImgElements = new List<DocTemplate.DTO_ElementImage>();

                    foreach (FooterLogo logo in SourceSkin.FooterLogos)
                    {
                        DocTemplate.DTO_ElementImage FLogo = new DocTemplate.DTO_ElementImage();

                        Boolean found = false;
                        foreach (LogoToLang lc in logo.Languages)
                        {
                            if (lc.LangCode == UsrLangCode || lc.LangCode == DefLangCode)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found)
                        {
                            FLogo = new DocTemplate.DTO_ElementImage();
                            FLogo.Height = 0;
                            FLogo.Width = 0;
                            FLogo.Alignment = Core.DomainModel.DocTemplateVers.ElementAlignment.MiddleLeft;// Core.DomainModel.Helpers.Export.ElementAlignment.MiddleCenter;
                            FLogo.Path = Skin.Business.SkinFileManagement.GetLogoVirtualFullPath(BaseSkinURL, SourceSkin.Id, logo.Id, logo.ImageUrl);

                            //FLogo.Path = BaseURL + logo.ImageUrl;

                            mIElement.ImgElements.Add(FLogo);
                        }
                    }

                    TFooter.Left = mIElement;
                }

                //Footer text
                if (SourceSkin.FooterText != null && SourceSkin.FooterText.Count() > 0)
                {
                    FooterText ftxt = (from FooterText ft in SourceSkin.FooterText
                                       where ft.LangCode == UsrLangCode
                                       select ft).FirstOrDefault();
                    if (ftxt == null)
                        ftxt = (from FooterText ft in SourceSkin.FooterText
                                where ft.LangCode == DefLangCode
                                select ft).FirstOrDefault();

                    if (ftxt != null && !String.IsNullOrEmpty(ftxt.Value))
                    {
                        DocTemplate.DTO_ElementText txtel = new DocTemplate.DTO_ElementText();// DocTemplate.TextElement();
                        txtel.IsHTML = true;
                        txtel.Alignment = Core.DomainModel.DocTemplateVers.ElementAlignment.MiddleCenter;// Core.DomainModel.Helpers.Export.ElementAlignment.MiddleCenter;
                        if (FooterFontSize > 0)
                        {
                            txtel.Text = "<font style=\"font-size: " + FooterFontSize.ToString() + "px;\">" + ftxt.Value + "</font>";
                        }
                        else
                        {
                            txtel.Text =  ftxt.Value;
                        }
                        

                        TFooter.Right = txtel;
                    }
                }

                defTemplate.Footer = TFooter;

            }

            return defTemplate;

        }

        public Int32 GetUserDefaultIdOrganization(Int32 idPerson)
        {
            Int32 idOrganization = 0;
            try
            {
                liteOrganizationProfile dOrganization = (from o in Manager.GetIQ<liteOrganizationProfile>() where o.IdPerson == idPerson && o.isDefault select o).Skip(0).Take(1).ToList().FirstOrDefault();


                //liteOrganizationProfile dOrganization = (from o in DC.GetCurrentSession().Linq<liteOrganizationProfile>()
                //                                         where o.IdPerson == idPerson && o.isDefault
                //                                         select o).Skip(0).Take(1).ToList().FirstOrDefault();

                idOrganization = (dOrganization == null) ? 0 : dOrganization.IdOrganization;
            }
            catch (Exception ex) { }
            return idOrganization;
        }

        // SE HO UNA SKIN che ha elementi di skin diverse (DEPRECATA!)
        //            /// <summary>
        //            /// Converte una Skin in DocTemplate.Template, impostando SOLO HEADER E FOOTER
        //            /// </summary>
        //            /// <param name="DefSkin">Skin di partenza</param>
        //            /// <param name="DefLangCode">Lingua di default del sistema</param>
        //            /// <param name="UsrLangCode">Lingua dell'utente</param>
        //            /// <param name="Title">Il titolo nell'Header. SE non viene impostato e SE presente verrà usato l'ALT impostato per il logo Header</param>
        //            /// <param name="BaseURL">L'url base, es: "http://demo.comunitaonline.unitn.it/"</param>
        //            /// <returns>
        //            ///     Un template con Header e Footer impostati correttamente.
        //            /// </returns>
        //            /// <remarks>
        //            ///     Ad eccezione di Header e Footer, TUTTI gli altri parametri saranno NULL!!
        //            /// </remarks>
        //            public DocTemplate.Template SkinToTemplate(
        //        Domain.Skin DefSkin,
        //        String DefLangCode, String UsrLangCode,
        //        String Title, String BaseSkinURL, Int64 SkinHeaderId, Int64 SkinFooterId)
        //            {
        //                DocTemplate.Template defTemplate = new DocTemplate.Template();
        //                defTemplate.Header = null;
        //                defTemplate.Footer = null;

        //                if (DefSkin != null)
        //                {
        //                    //HEADER
        //                    DocTemplate.TemplateHeaderFooter THeader = new DocTemplate.TemplateHeaderFooter();

        //                    //LOGO
        //                    DocTemplate.ImageElement HLogo = null;

        //                    if (DefSkin.HeaderLogos != null && DefSkin.HeaderLogos.Count > 0)
        //                    {
        //                        HeaderLogo Logo = (from HeaderLogo lg in DefSkin.HeaderLogos
        //                                           where lg.LangCode == UsrLangCode
        //                                           select lg).FirstOrDefault();

        //                        if (Logo == null)
        //                        {
        //                            Logo = (from HeaderLogo lg in DefSkin.HeaderLogos
        //                                    where lg.LangCode == DefLangCode
        //                                    select lg).FirstOrDefault();
        //                        }

        //                        if (Logo != null)
        //                        {
        //                            HLogo = new DocTemplate.ImageElement();
        //                            HLogo.Height = 0;
        //                            HLogo.Width = 0;
        //                            HLogo.Alignment = Core.DomainModel.Helpers.Export.ElementAlignment.MiddleCenter;
        //                            HLogo.Path = Skin.Business.SkinFileManagement.GetLogoVirtualFullPath(BaseSkinURL, SkinHeaderId, Logo.Id, Logo.ImageUrl);
        //                            //HLogo.Path = BaseURL + Logo.ImageUrl;

        //                            if (String.IsNullOrEmpty(Title) && !String.IsNullOrEmpty(Logo.Alt))
        //                                Title = Logo.Alt;
        //                        }
        //                    }
        //                    if (HLogo != null)
        //                        THeader.LeftElement = HLogo;

        //                    // TITLE (Se viene passata stringa vuota E SE al logo è stato assegnato un ALT, verrà usato quello... (EVENTUALMENTE RIVEDERE!!!)

        //                    DocTemplate.TextElement TTitle = new DocTemplate.TextElement();
        //                    TTitle.IsHTML = true;
        //                    TTitle.Alignment = Core.DomainModel.Helpers.Export.ElementAlignment.MiddleCenter;
        //                    TTitle.Text = "<h1>" + Title + "</h1>";
        //                    THeader.RightElement = TTitle;

        //                    // elemento vuoto
        //                    THeader.CenterElement = null;

        //                    defTemplate.Header = THeader;




        //                    //FOOTER
        //                    DocTemplate.TemplateHeaderFooter TFooter = new DocTemplate.TemplateHeaderFooter();
        //                    TFooter.LeftElement = null;
        //                    TFooter.CenterElement = null;
        //                    TFooter.RightElement = null;

        //                    //Images


        //                    if (DefSkin.FooterLogos != null && DefSkin.FooterLogos.Count() > 0)
        //                    {
        //                        DocTemplate.MultiImageElement mIElement = new DocTemplate.MultiImageElement();
        //                        mIElement.ImgElements = new List<DocTemplate.ImageElement>();

        //                        foreach (FooterLogo logo in DefSkin.FooterLogos)
        //                        {
        //                            DocTemplate.ImageElement FLogo = new DocTemplate.ImageElement();

        //                            Boolean found = false;
        //                            foreach (LogoToLang lc in logo.Languages)
        //                            {
        //                                if (lc.LangCode == UsrLangCode || lc.LangCode == DefLangCode)
        //                                {
        //                                    found = true;
        //                                    break;
        //                                }
        //                            }
        //                            if (found)
        //                            {
        //                                FLogo = new DocTemplate.ImageElement();
        //                                FLogo.Height = 0;
        //                                FLogo.Width = 0;
        //                                FLogo.Alignment = Core.DomainModel.Helpers.Export.ElementAlignment.MiddleCenter;
        //                                FLogo.Path = Skin.Business.SkinFileManagement.GetLogoVirtualFullPath(BaseSkinURL, SkinFooterId, logo.Id, logo.ImageUrl);

        //                                //FLogo.Path = BaseURL + logo.ImageUrl;

        //                                mIElement.ImgElements.Add(FLogo);
        //                            }
        //                        }

        //                        TFooter.LeftElement = mIElement;
        //                    }

        //                    //Footer text
        //                    if (DefSkin.FooterText != null && DefSkin.FooterText.Count() > 0)
        //                    {
        //                        FooterText ftxt = (from FooterText ft in DefSkin.FooterText
        //                                           where ft.LangCode == UsrLangCode
        //                                           select ft).FirstOrDefault();
        //                        if (ftxt == null)
        //                            ftxt = (from FooterText ft in DefSkin.FooterText
        //                                    where ft.LangCode == DefLangCode
        //                                    select ft).FirstOrDefault();

        //                        if (ftxt != null && !String.IsNullOrEmpty(ftxt.Value))
        //                        {
        //                            DocTemplate.TextElement txtel = new DocTemplate.TextElement();
        //                            txtel.IsHTML = true;
        //                            txtel.Alignment = Core.DomainModel.Helpers.Export.ElementAlignment.MiddleCenter;
        //                            txtel.Text = ftxt.Value;

        //                            TFooter.RightElement = txtel;
        //                        }
        //                    }

        //                    defTemplate.Footer = TFooter;

        //                }

        //                return defTemplate;

        //            }
        //    #endregion
#endregion


    }
}
