//using System;
//using System.Runtime.InteropServices;
//using System.Web;
//using System.Web.Security;
//using System.Security.Principal;
//using System.Security.Permissions;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;

//namespace lm.Comol.Core.DomainModel.Helpers
//{
//    public class Impersonate
//    {

//        public const int LOGON32_LOGON_INTERACTIVE = 2;
//        public const int LOGON32_PROVIDER_DEFAULT = 0;
        
//        System.Security.Principal.WindowsImpersonationContext impersonationContext;
//        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        public static extern int LogonUserA(String lpszUserName, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
//        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        public static extern int LogonUserW(String lpszUserName, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
//        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        public static extern int LogonUser(String lpszUserName, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

//        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        public static extern int DuplicateToken(IntPtr hToken,
//            int impersonationLevel,
//            ref IntPtr hNewToken);
//        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        public static extern bool RevertToSelf();
//        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        public static extern bool CloseHandle(IntPtr handle);

//        public bool ImpersonateValidUser(String userName, String domain, String password)
//        {
//            System.Security.Principal.WindowsIdentity tempWindowsIdentity;
//            IntPtr token = IntPtr.Zero;
//            IntPtr tokenDuplicate = IntPtr.Zero;

//            if (RevertToSelf())
//            {
//                if (LogonUserW(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
//                    LOGON32_PROVIDER_DEFAULT, ref token) != 0)
//                {
//                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
//                    {
//                        tempWindowsIdentity = new System.Security.Principal.WindowsIdentity(tokenDuplicate);
//                        impersonationContext = tempWindowsIdentity.Impersonate();
//                        if (impersonationContext != null)
//                        {
//                            CloseHandle(token);
//                            CloseHandle(tokenDuplicate);
//                            return true;
//                        }
//                    }
//                }
//            }
//            if (token != IntPtr.Zero)
//                CloseHandle(token);
//            if (tokenDuplicate != IntPtr.Zero)
//                CloseHandle(tokenDuplicate);
//            return false;
//        }

//        public void UndoImpersonation()
//        {
//            impersonationContext.Undo();
//        }
//    }
//}
