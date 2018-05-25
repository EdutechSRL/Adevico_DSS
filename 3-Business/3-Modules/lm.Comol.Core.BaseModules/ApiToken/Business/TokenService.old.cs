using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
//using lm.Comol.Modules.Standard.Business;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;

//using lm.Comol.Modules.Standard;

namespace lm.Comol.Core.BaseModules.ApiToken
{
    /// <summary>
    /// Token service
    /// </summary>
    public class TokenService
    {
        public iDataContext ODataContext;
        public iApplicationContext OAppContext;

        public iApplicationContext ApplicationContextGetFake()
        {
            ApplicationContext fakeContext = new ApplicationContext();
            fakeContext.DataContext = ODataContext;

            //ToDo: get di uno Usercontext appropriato
            fakeContext.UserContext = new UserContext();

            return fakeContext;
        }


       




        #region "Init"

        public BaseAPIService()
        {}

        public BaseAPIService(iDataContext oDC)
        {
            ODataContext = oDC;
            
        }

        public BaseAPIService(iApplicationContext oContext)
        {
            ODataContext = oContext.DataContext;
            OAppContext = oContext;
        }

#endregion


        private lm.Comol.Modules.Standard.Business.BaseManager _manager;
        /// <summary>
        /// Manager Servizi (senza funzioni core)
        /// </summary>
        public lm.Comol.Modules.Standard.Business.BaseManager Manager
        {
            get
            {
                if (ODataContext == null)
                    return null;

                if (_manager == null)
                {
                    _manager = new BaseManager(ODataContext);
                }

                return _manager;
            }
        }

        private lm.Comol.Core.Business.BaseModuleManager _coreManager;

        /// <summary>
        /// Manager CON funzioni CORE
        /// </summary>
        public lm.Comol.Core.Business.BaseModuleManager CoreManager
        {
            get
            {
                if (ODataContext == null)
                    return null;

                if (_coreManager == null)
                {
                    _coreManager = new lm.Comol.Core.Business.BaseModuleManager(ODataContext);
                }

                return _coreManager;
            }
        }

        #region Role

        private ProfileManagementService _PmService;

        public ProfileManagementService PMService
        {
            get
            {
                if (_PmService == null)
                    _PmService = new ProfileManagementService(ODataContext);
                return _PmService;
            }
        }
        #endregion
    }
}
