using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    public class DashboardException : Exception
    {
        public DashboardErrorType ErrorType { get; set; }
        public DashboardException()
        {
        }
        public DashboardException(DashboardErrorType errorType)
        {
            ErrorType = errorType;
        }
        public DashboardException(DashboardErrorType errorType, Exception inner, String message = "")
            : base(message, inner)
        {
            ErrorType = errorType;
        }
    }

    public enum DashboardErrorType
    {
        None = 0,
        DefaultSettingsUnavailable = 1,
        DefaultSettingsUndeletable = 2,
        NoAssignmentsForEnable = 3,
        MultipleAssignmentForProfileType = 4,
        MultipleAssignmentForRole = 5,
        MultipleAssignmentForPerson = 6,
        GenericError = 7,
        NoAssignmentsForActiveSettings =8,
        NotActivableSettings = 9,
        AlreadyActiveSettings = 10,
        NoAssignmentsForUndelete = 11
    }
}