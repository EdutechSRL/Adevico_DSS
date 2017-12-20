using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using System.Web;

namespace WCF_CoreServices
{
    public class NHSessionHelper
    {
        //Encoding encode = Encoding.GetEncoding(737);
        //public static ISessionFactory SessionFactory = CreateFactory( "E:\\inetpub\\progettiWeb\\LM_Comol\\Services\\WS_CoreServices\\hibernate.cfg.xml.config" );

        

        public static ISessionFactory SessionFactory = CreateFactory(System.Web.Hosting.HostingEnvironment.MapPath("~/hibernate.cfg.xml.config"));
        private static ISessionFactory IcodeonFactory = CreateFactory(System.Web.Hosting.HostingEnvironment.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["IcodeonFactory"]));

        public static ISessionFactory CreateFactory()
        {
            try
            {

                Configuration configuration = new Configuration();
                return configuration.Configure().BuildSessionFactory();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ISessionFactory CreateFactory(string filename)
        {
            try
            {

                Configuration configuration = new Configuration();
                return configuration.Configure(filename).BuildSessionFactory();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ISession GetComolSession()
        {
            try
            {
                return SessionFactory.OpenSession();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ISession GetIcodeonSession()
        {
            try
            {
                return IcodeonFactory.OpenSession();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
