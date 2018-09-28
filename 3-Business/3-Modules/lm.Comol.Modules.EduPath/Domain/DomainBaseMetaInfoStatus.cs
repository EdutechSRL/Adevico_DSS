using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class DomainBaseMetaInfoStatus: lm.Comol.Core.DomainModel.DomainBaseObjectIdLiteMetaInfo<long>
    {
        private const Status DefaultStatus =   (Status.NotLocked | Status.NotMandatory);
        private  Status _status = DefaultStatus;
        const  Status errorLocked = (Status.Locked | Status.NotLocked);
        const  Status errorMandatory = (Status.Mandatory  | Status.NotMandatory );
        public virtual Status Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                if ((value & errorMandatory) == errorMandatory)
                {
                    setNotMandatory();
                }
                if ((value & errorLocked) == errorLocked)
                {
                    setNotLocked();
                }
            }
        }
        public virtual void setMandatory()
        {
            ForceStatus(Status.Mandatory, Status.NotMandatory);
        }
        public virtual void setNotMandatory()
        {
            ForceStatus(Status.NotMandatory, Status.Mandatory);
        }
        public virtual void setNotLocked()
        {
            ForceStatus(Status.NotLocked, Status.Locked);
        }
        public virtual void setLocked()
        {
            ForceStatus(Status.Locked, Status.NotLocked);
        }

        //public virtual void ForceStatus(out Status current, Status set, Status remove)
        //{
        //    current |= set;
        //    current = ((_status & remove) == remove) ? (Status)((int)_status - (int)remove) : current;            
        //}

        public virtual void ForceStatus(Status set, Status remove)
        {            
            _status |= set;
            _status = ((_status & remove) == remove) ? (Status)((int)_status - (int)remove) : _status;  
        }

        public virtual void ForceStatus(Status set)
        {
            Status remove = Status.None;

            remove = GetOtherSideOrPrevious(remove, set, errorLocked);
            remove = GetOtherSideOrPrevious(remove, set, errorMandatory);

            ForceStatus(set, remove);
        }

        private  Status GetOtherSideOrPrevious(Status previous, Status set, Status error)
        {
            int temp = 0;
            temp = (int)error - (int)set;
            return (temp > 0) ? (Status)temp : previous;
        }

        public virtual bool CheckStatus(Status Expected)
        {
            return (Status & Expected) == Expected;
        }
    }
}
