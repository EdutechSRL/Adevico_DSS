using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Helpers
{
        [Serializable()]
        public static class TemplateCommonPlaceHolders
        {
            public static String BirthDateFormat = "dd/MM/yyyy";

            private readonly static String tagString = "{0}common.{1}{2}";
            public static String OpenTag { get { return "["; } }
            public static String CloseTag { get { return "]"; } }

            private static Dictionary<CommonPlaceHoldersType, String> _PlaceHolders = new Dictionary<CommonPlaceHoldersType, String>();
            public static Dictionary<CommonPlaceHoldersType, String> PlaceHolders()
            {
                if (_PlaceHolders.Count == 0)
                {
                    _PlaceHolders = (from e in Enum.GetValues(typeof(CommonPlaceHoldersType)).OfType<CommonPlaceHoldersType>()
                                     where e!= CommonPlaceHoldersType.None
                                     select e).ToDictionary(k=> k, v=> GetPlaceHolder(v));
                }
                return _PlaceHolders;
            }
            public static String GetPlaceHolder(CommonPlaceHoldersType type)
            {
                return string.Format(tagString, OpenTag,type.ToString(), CloseTag);
            }

            //ToDo: rivedere le altre puntando a QUESTA!!!
            public static String Translate(
                string content, 
                Person person, 
                Community community, 
                DateTime? subscriptionOn, 
                String organization, 
                String roleName, 
                String profileType, 
                String istanceName)
            {
                //String translation = content;

                content = Translatecommunity(content, community);
                content = TranslatePerson(content, person);
                content = TranslateOther(content, istanceName, organization, profileType, subscriptionOn, roleName);

                return content;
            }

            public static String Translate(
                string content, 
                Person person, 
                liteCommunity community, 
                DateTime? subscriptionOn, 
                String organization, 
                String roleName, 
                String profileType, 
                String istanceName)
            {
                //String translation = content;

                content = Translatecommunity(content, community);
                content = TranslatePerson(content, person);
                content = TranslateOther(content, istanceName, organization, profileType, subscriptionOn, roleName);

                return content;
            }

            //Tolto!
            //public static String Translate(
            //    string content, 
            //    litePerson person, 
            //    liteCommunity community, 
            //    DateTime? subscriptionOn, 
            //    String organization, 
            //    String roleName, 
            //    String profileName, 
            //    String istanceName)
            //{
            //    String translation = content;
            //    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CommunityName), (community == null) ? "" : ReplaceChars(community.Name));
            //    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserName), (person == null) ? "" : ReplaceChars(person.SurnameAndName));
            //    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.OrganizationName), ReplaceChars(organization));
            //    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.RoleName), ReplaceChars(roleName));
            //    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.ProfileType), ReplaceChars(profileName));
            //    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.SubscriptionOn), (subscriptionOn.HasValue) ? subscriptionOn.Value.ToShortDateString() : "");
            //    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.IstanceName), ReplaceChars(istanceName));

            //    return translation;
            //}

            public static string Translate(string content,
               CommonPlaceHolderData data)
            {
                DateTime? subscriptionOn = null;
                string roleName = "";

                if (data.Subscription != null)
                {
                    subscriptionOn = data.Subscription.SubscriptedOn;
                    if (data.Subscription.Role != null)
                    {
                        roleName = data.Subscription.Role.Name;
                    }
                }

                content = Translatecommunity(content, data.Community);
                content = TranslatePerson(content, data.Person);
                content = TranslateOther(content, data.InstanceName, data.OrganizationName, data.UserType, subscriptionOn, roleName);

                return content;
            }

            public static String ReplaceChars(String value)
            {
                if (String.IsNullOrEmpty(value))
                    return "";
                else {
                    string regex = string.Format("[{0}]", System.Text.RegularExpressions.Regex.Escape("<>"));
                    System.Text.RegularExpressions.Regex removeInvalidChars = new System.Text.RegularExpressions.Regex(regex, System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.CultureInvariant);
                    return removeInvalidChars.Replace(value, "");
                }
            }




            #region Translate elements

            private static string Translatecommunity(string translation, Community community)
            {
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CommunityName), (community == null) ? "" : ReplaceChars(community.Name));
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CommunityId), (community == null) ? "" : ReplaceChars(community.Id.ToString()));

                return translation;
            }
            private static string Translatecommunity(string translation, liteCommunity community)
            {
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CommunityName), (community == null) ? "" : ReplaceChars(community.Name));
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CommunityId), (community == null) ? "" : ReplaceChars(community.Id.ToString()));

                return translation;
            }
            private static string Translatecommunity(string translation, string communityName, int communityId)
            {
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CommunityName), ReplaceChars(communityName));
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CommunityId), communityId.ToString());

                return translation;
            }

            //private static string RemoveCommunity(string translation, string communityName)
            //{
            //    return translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CommunityName), "");
            //}
            
            private static string TranslatePerson(string translation, Person person)
            {
                if (person == null)
                    return RemovePerson(translation);

                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserName), ReplaceChars(person.SurnameAndName));
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserMail), ReplaceChars(person.Mail));
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.TaxCode), ReplaceChars(person.TaxCode));
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserJob), ReplaceChars(person.Job));
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserSector), ReplaceChars(person.Sector));

                CompanyUser company = null;
                try { company = (CompanyUser)person; }
                catch (Exception) { }

                if (company != null && company.CompanyInfo != null)
                {
                    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CompanyName), ReplaceChars(company.CompanyInfo.Name));
                    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CompanyTax), ReplaceChars(company.CompanyInfo.TaxCode));
                    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CompanyRea), ReplaceChars(company.CompanyInfo.ReaNumber));
                }
                else
                {
                    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CompanyName), "");
                    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CompanyTax), "");
                    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CompanyRea), "");
                }

                Employee dipendente = null;
                try { dipendente = (Employee)person; }
                catch (Exception) { }

                if (dipendente != null && dipendente.CurrentAffiliation != null &&
                    dipendente.CurrentAffiliation.Agency != null)
                {
                    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.AgencyName), dipendente.CurrentAffiliation.Agency.Name);
                    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.AgencyNationalCode), dipendente.CurrentAffiliation.Agency.NationalCode);
                    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.AgencyTaxCode), dipendente.CurrentAffiliation.Agency.TaxCode);
                    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.AgencyExternalCode), dipendente.CurrentAffiliation.Agency.ExternalCode);
                }
                else
                {
                    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.AgencyName), "");
                    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.AgencyNationalCode), "");
                    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.AgencyTaxCode), "");
                    translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.AgencyExternalCode), "");
                }

                translation = TranslatePersonInfo(translation, person.PersonInfo);

                return translation;
            }

            private static string RemovePerson(string translation)
            {
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserName), "");
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserMail), "");
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.TaxCode), "");
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserJob), "");
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserSector), "");

                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CompanyName), "");
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CompanyTax), "");
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CompanyRea), "");

                return translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.CommunityName), "");
            }

            private static string TranslatePersonInfo(string translation, PersonInfo personInfo)
            {
                if (personInfo == null)
                    return RemovePersonInfo(translation);

                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserBirthDate), ReplaceChars(personInfo.BirthDate.ToString(BirthDateFormat)));
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserBirthPlace), ReplaceChars(personInfo.BirthPlace));
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserAddress), ReplaceChars(personInfo.Address));
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserCap), ReplaceChars(personInfo.PostCode));
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserCity), ReplaceChars(personInfo.City));
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserPhone), ReplaceChars(personInfo.OfficePhone));

                return translation;
            }

            private static string RemovePersonInfo(string translation)
            {
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserBirthDate), "");
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserBirthPlace), "");
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserAddress), "");
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserCap), "");
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserCity), "");
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.UserPhone), "");

                return translation;
            }

            private static string TranslateOther(string translation,
                string istanceName, string organization, string profileType, DateTime? subscriptionOn, string roleName)
            {
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.IstanceName), ReplaceChars(istanceName));
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.OrganizationName), ReplaceChars(organization));
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.ProfileType), ReplaceChars(profileType));
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.SubscriptionOn), (subscriptionOn.HasValue) ? subscriptionOn.Value.ToShortDateString() : "");
                translation = translation.Replace(GetPlaceHolder(CommonPlaceHoldersType.RoleName), ReplaceChars(roleName));
                
                return translation;
            }



            #endregion
        }

       

        [Serializable()]
        public enum CommonPlaceHoldersType
        {
            None = 0,
            CommunityName = 1,
            UserName = 2,
            RoleName = 3,
            ProfileType = 4,
            SubscriptionOn = 5,
            //CommunityType = 6,
            OrganizationName = 7,
            IstanceName = 8,
            UserMail = 10,
            TaxCode = 11,
            CompanyName = 20,
            CompanyTax = 21,
            CompanyRea = 22,
            //Da aggiungere
            UserAddress = 30,
            UserCap = 31,
            UserCity = 32,
            UserPhone = 33,
            UserBirthDate = 34,
            UserBirthPlace = 35,
            UserJob = 36,
            UserSector = 37,

            AgencyName = 40,
            AgencyNationalCode = 41,
            AgencyTaxCode = 42,
            AgencyExternalCode = 43,

            CommunityId = 50,
        }

    }