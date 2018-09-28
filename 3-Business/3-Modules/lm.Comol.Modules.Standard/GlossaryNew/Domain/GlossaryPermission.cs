using System;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    [Serializable]
    public class GlossaryPermission
    {
        public GlossaryPermission()
        {
            ViewTerm = false;
            EditGlossary = false;
            DeleteGlossary = false;
            AddTerm = false;
            EditTerm = false;
            DeleteTerm = false;
            ViewStat = false;
        }

        public GlossaryPermission(ModuleGlossaryNew module) : this()
        {
            ViewTerm = module.ViewTerm;
            EditGlossary = module.EditGlossary;
            DeleteGlossary = module.DeleteGlossary;

            AddTerm = module.AddTerm;
            EditTerm = module.EditTerm;
            DeleteTerm = module.DeleteTerm;

            ViewStat = module.ViewStat;
        }

        public Boolean ViewTerm { get; set; }
        public Boolean EditGlossary { get; set; }
        public Boolean DeleteGlossary { get; set; }
        public Boolean AddTerm { get; set; }
        public Boolean EditTerm { get; set; }
        public Boolean DeleteTerm { get; set; }
        public Boolean ViewStat { get; set; }

        public void AddPermissions(ModuleGlossaryNew module)
        {
            if (module.ViewTerm)
                ViewTerm = module.ViewTerm;
            if (module.EditGlossary)
                EditGlossary = module.EditGlossary;
            if (module.DeleteGlossary)
                DeleteGlossary = module.DeleteGlossary;

            if (module.AddTerm)
                AddTerm = module.AddTerm;
            if (module.EditTerm)
                EditTerm = module.EditTerm;
            if (module.DeleteTerm)
                DeleteTerm = module.DeleteTerm;

            if (module.ViewStat)
                ViewStat = module.ViewStat;
        }

        public void AddPermissions(SharePermissionEnum sharePermission)
        {
            if ((sharePermission & SharePermissionEnum.EditTerm) == SharePermissionEnum.EditTerm)
                EditTerm = true;
            if ((sharePermission & SharePermissionEnum.AddTerm) == SharePermissionEnum.AddTerm)
                AddTerm = true;
            if ((sharePermission & SharePermissionEnum.DeleteTerm) == SharePermissionEnum.DeleteTerm)
                DeleteTerm = true;
        }
    }
}