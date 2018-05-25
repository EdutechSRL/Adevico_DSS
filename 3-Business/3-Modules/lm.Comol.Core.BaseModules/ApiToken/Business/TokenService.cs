using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ApiToken.Business
{
    public class TokenService : CoreServices
    {
        public TokenService(iDataContext oContext)
                : base(oContext)    //, string xmlPath
        {

        }

        public TokenService(iApplicationContext oAppContext)
            : base(oAppContext)    //, string xmlPath
        {

        }

        public const string DefaultDeviceId = "Unknow";

        #region ToDo: gestione token


        //private TokenCheckPrevious


        private string TokenGenerate(InternalLoginInfo user, string deviceId, Domain.TokenType type)
        {
            if (user == null || user.Person == null)
                return "";

            if (String.IsNullOrEmpty(deviceId))
                deviceId = DefaultDeviceId;

            Domain.ApiToken token = Manager.GetAll<Domain.ApiToken>(mt => mt.PersonId == user.Person.Id && mt.DeviceId == deviceId)
                .Skip(0)
                .Take(1)
                .FirstOrDefault();

            if (token == null)
            {
                token = new Domain.ApiToken();
                token.Type = type;
            }
            else
            {
                //token.Type = type;    
            }

            token.CreateOn = DateTime.Now;
            token.PersonId = user.Person.Id;
            token.Token = System.Guid.NewGuid().ToString();
            token.DeviceId = deviceId;


            try
            {
                if (!Manager.IsInTransaction())
                    Manager.BeginTransaction();

                Manager.SaveOrUpdate<Domain.ApiToken>(token);

                Manager.Commit();

            }
            catch (Exception)
            {
                if (Manager.IsInTransaction())
                    Manager.RollBack();
                return "";
            }

            ClearOldToken(user.Person.Id, type, token.Id, deviceId);

            return token.Token;
        }


        /// <summary>
        /// Aggiorna il Token: per ApiWrapper!
        /// </summary>
        /// <param name="personId">Id Person corrente</param>
        /// <param name="deviceId">WorkingSessionId</param>
        /// <param name="type">Tipo di token</param>
        /// <param name="forceUpdate">Forza la generazione di un nuovo token</param>
        /// <returns>Token</returns>
        public string TokenRefresh(int personId, string deviceId, Domain.TokenType type, bool forceUpdate)
        {
            litePerson user = Manager.Get<litePerson>(personId);

            if (user == null)
                return "";

            if (String.IsNullOrEmpty(deviceId))
                return "";
            //deviceId = DefaultDeviceId;

            Domain.ApiToken token = Manager.GetAll<Domain.ApiToken>(
                    mt => mt.PersonId == user.Id
                    && mt.DeviceId == deviceId)// && mt.Type == type)
                .Skip(0)
                .Take(1)
                .FirstOrDefault();

            if (forceUpdate || token == null || String.IsNullOrEmpty(token.Token))
            {
                token = new Domain.ApiToken();
                token.PersonId = user.Id;
                token.Token = System.Guid.NewGuid().ToString();
                token.DeviceId = deviceId;
                token.CreateOn = DateTime.Now;
                token.Type = type;

                try
                {
                    if (!Manager.IsInTransaction())
                        Manager.BeginTransaction();

                    Manager.SaveOrUpdate<Domain.ApiToken>(token);

                    Manager.Commit();

                }
                catch (Exception)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                    return "";
                }
            }

            ClearOldToken(user.Id, type, token.Id, deviceId);

            return token.Token;
        }

        private void ClearOldToken(int UserId, Domain.TokenType type, long TokenId, string DeviceId)
        {
            DateTime reference = DateTime.Now.AddHours(-24);

            IList<Domain.ApiToken> ExpiredTokens = Manager.GetAll<Domain.ApiToken>(tk =>
                tk.Type == type &&
                tk.PersonId == UserId && tk.Id != TokenId
                && (
                  (tk.Type == Domain.TokenType.Mobile && tk.DeviceId == DeviceId)
                  || (tk.Type != Domain.TokenType.Mobile && (tk.CreateOn < reference)) //&& tk.PersonId == user.Person.Id && tk.DeviceId == deviceId)
                  )
              );

            //&& tk.PersonId == user.Person.Id 
            //&& token.Type == type 

            if (ExpiredTokens.Any())
            {
                try
                {
                    if (!Manager.IsInTransaction())
                        Manager.BeginTransaction();

                    Manager.DeletePhysicalList<Domain.ApiToken>(ExpiredTokens);

                    Manager.Commit();
                }
                catch
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();

                }
            }
        }

        public int PersonGetIdFromToken(String token, string deviceId, Domain.TokenType type)
        {
            if (String.IsNullOrEmpty(deviceId))
                deviceId = DefaultDeviceId;

            Domain.ApiToken objToken = Manager.GetAll<Domain.ApiToken>(mt => mt.Token == token && mt.DeviceId == deviceId)// && mt.Type == type)
               .Skip(0)
               .Take(1)
               .FirstOrDefault();

            //return objToken;

            if (objToken == null)
            {
                return -1;
            }
            else if (objToken.IsExpired)
            {

            }
            //return -1;

            return objToken.PersonId;

        }

        public litePerson PersonGetLiteFromToken(String token, string deviceId, Domain.TokenType type)
        {
            //if (String.IsNullOrEmpty(deviceId))
            //    deviceId = DefaultDeviceId;

            int prsId = PersonGetIdFromToken(token, deviceId, type);

            if (prsId <= 0)
                return null;

            return Manager.Get<litePerson>(prsId);
        }

        public Person PersonGetFromToken(String token, string deviceId, Domain.TokenType type)
        {
            //if (String.IsNullOrEmpty(deviceId))
            //    deviceId = DefaultDeviceId;

            int prsId = PersonGetIdFromToken(token, deviceId, type);

            if (prsId <= 0)
                return new Person();

            return Manager.GetPerson(prsId);
        }

        #endregion

    }
}
