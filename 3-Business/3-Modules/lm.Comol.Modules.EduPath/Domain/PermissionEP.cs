using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [CLSCompliant(true)]
    [FlagsAttribute]
    public enum PermissionEP_Enum
    {

        None = 0,
        Create = 1,
        Read = 2,
        Update = 4,
        Delete = 8,
        Evaluate = 16,
        ViewUserStat=32,
        ViewOwnStat=64,
        ManageRule=128,

        Participant = 66,
        Evaluator = 50,
        Manager = 191,
        StatViewer = 64
    }
    [Serializable]
    public class PermissionEP
    {
        private PermissionEP_Enum Permission { get; set; }

        public bool ManageRule { get { return (Permission & PermissionEP_Enum.ManageRule) == PermissionEP_Enum.ManageRule; } } 
        public bool Create { get { return (Permission & PermissionEP_Enum.Create) == PermissionEP_Enum.Create; } }
        public bool Read { get { return (Permission & PermissionEP_Enum.Read) == PermissionEP_Enum.Read; } }
        public bool Evaluate { get { return (Permission & PermissionEP_Enum.Evaluate) == PermissionEP_Enum.Evaluate; } }
        public bool Update { get { return (Permission & PermissionEP_Enum.Update) == PermissionEP_Enum.Update; } }
        public bool Delete { get { return (Permission & PermissionEP_Enum.Delete) == PermissionEP_Enum.Delete; } }
        public bool ViewUserStat { get { return (Permission & PermissionEP_Enum.ViewUserStat) == PermissionEP_Enum.ViewUserStat; } }
        public bool ViewOwnStat { get { return (Permission & PermissionEP_Enum.ViewOwnStat) == PermissionEP_Enum.ViewOwnStat; } }

        public PermissionEP(int role)
        {
            Permission = (PermissionEP_Enum)role;
        }
        public PermissionEP(RoleEP Role)
        {
            if ((Role & RoleEP.Manager) == RoleEP.Manager)
                Permission |= PermissionEP_Enum.Manager;
            if ((Role & RoleEP.Participant) == RoleEP.Participant)
                Permission |= PermissionEP_Enum.Participant;
            if ((Role & RoleEP.Evaluator) == RoleEP.Evaluator)
                Permission |= PermissionEP_Enum.Evaluator;
            if ((Role & RoleEP.StatViewer) == RoleEP.StatViewer)
                Permission |= PermissionEP_Enum.StatViewer;
        }
    }
}
