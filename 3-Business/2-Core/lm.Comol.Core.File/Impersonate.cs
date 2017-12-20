using System;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Security;
using System.Security.Principal;
using System.Security.Permissions;
using System.Configuration;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.File
{
    public class Impersonate : IDisposable
    {
        private IntPtr _Token;
        private static Boolean _isImpersonated;
        public static Boolean isImpersonated()
        { return _isImpersonated; }
        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_PROVIDER_DEFAULT = 0;
        private string user = System.Configuration.ConfigurationManager.AppSettings["ImpersonateUser"];
        private string domain = System.Configuration.ConfigurationManager.AppSettings["ImpersonateDomain"];
        private string password = System.Configuration.ConfigurationManager.AppSettings["ImpersonatePassword"];

        private string _isImpersonate = System.Configuration.ConfigurationManager.AppSettings["isImpersonate"];
        private bool isImpersonate()
        {
            return (_isImpersonate == null ? false : (_isImpersonate.ToLower() == "true" ? true : false));
        }


        System.Security.Principal.WindowsImpersonationContext impersonationContext;
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int LogonUserA(String lpszUserName, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int LogonUserW(String lpszUserName, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int LogonUser(String lpszUserName, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);
        public FileMessage ImpersonateValidUser()
        {
            if (isImpersonate() && !_isImpersonated)
            {
                _isImpersonated = true;
                return (ImpersonateValidUser(user, domain, password) ? FileMessage.Impersonated : FileMessage.ImpersonationFailed);
            }
            else return FileMessage.NoImpersonationRequired;
        }
        public bool ImpersonateValidUser(String userName, String domain, String password)
        {

            System.Security.Principal.WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUser(userName, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new System.Security.Principal.WindowsIdentity(tokenDuplicate);
                        impersonationContext = tempWindowsIdentity.Impersonate();
                        if (impersonationContext != null)
                        {
                            CloseHandle(token);
                            CloseHandle(tokenDuplicate);
                            return true;
                        }
                    }
                }
            }
            if (token != IntPtr.Zero)
                CloseHandle(token);
            if (tokenDuplicate != IntPtr.Zero)
                CloseHandle(tokenDuplicate);
            _Token = token;
            return false;

        }

        public void UndoImpersonation()
        {
            if (impersonationContext != null) // _isImpersonated deve comunque essere fuori dall'if.
            {
                impersonationContext.Undo();
            }
            _isImpersonated = false; 
        }
        public static Boolean isImpersonated(System.Security.Principal.WindowsImpersonationContext Context)
        {
            return (Context != null);
        }
        //public Boolean isImpersonated()
        //{ return (impersonationContext != null); }
        private void Leave()
        {
            _isImpersonated = false;
            if (impersonationContext != null)
                impersonationContext.Undo();

            if (_Token != IntPtr.Zero) CloseHandle(_Token);
            impersonationContext = null;
        }

        public void Dispose()
        {
            Leave();
        }

    }
}
