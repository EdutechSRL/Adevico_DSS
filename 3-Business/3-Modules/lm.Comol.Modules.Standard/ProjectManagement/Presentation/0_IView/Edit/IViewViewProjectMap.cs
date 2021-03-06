﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewViewProjectMap : IViewBaseProjectMap
    {
        void LoadActivities(List<dtoMapActivity> activities);
        void DisplayNoActivities();
    }
}