
using System;
using System.Runtime.Serialization;

namespace lm.Comol.Core.DomainModel
{
    [Serializable(), CLSCompliant(true), DataContract]
    public class ModuleObject : iModuleObject, IEquatable<ModuleObject>
    {
        [DataMember]
        public virtual string FQN { get; set; }
        [DataMember]
        public virtual int ObjectTypeID { get; set; }
        [DataMember]
        public virtual long ObjectLongID { get; set; }
        [DataMember]
        public virtual long ObjectIdVersion { get; set; }
        [DataMember]
        public virtual Guid ObjectGuidID { get; set; }
        public virtual object ObjectOwner { get; set; }
        [DataMember]
        public virtual int CommunityID { get; set; }
        public virtual int ServiceID { get; set; }
        [DataMember]
        public virtual string ServiceCode { get; set; }

        public bool Equals(ModuleObject other)
        {
            return ObjectGuidID.Equals(other.ObjectGuidID) && ObjectLongID.Equals(other.ObjectLongID) && ObjectTypeID.Equals(other.ObjectTypeID) && ServiceCode.Equals(other.ServiceCode);
        }

        public static ModuleObject CreateLongObject(long objID, object obj, int objIdType, int objIdCommunity, string moduleCode, int idModule)
        {
            ModuleObject oObject = new ModuleObject();
            oObject.FQN = obj.GetType().FullName;
            oObject.ObjectLongID = objID;
            oObject.ObjectOwner = obj;
            oObject.ObjectTypeID = objIdType;
            oObject.ServiceCode = moduleCode;
            oObject.ServiceID = idModule;
            oObject.CommunityID = objIdCommunity;

            return oObject;
        }
        public static ModuleObject CreateLongObject(long objID, object obj, int objIdType, int objIdCommunity, string moduleCode)
        {
            ModuleObject oObject = new ModuleObject();
            oObject.FQN = obj.GetType().FullName;
            oObject.ObjectLongID = objID;
            oObject.ObjectOwner = obj;
            oObject.ObjectTypeID = objIdType;
            oObject.ServiceCode = moduleCode;
            oObject.CommunityID = objIdCommunity;

            return oObject;
        }

        public static ModuleObject CreateLongObject(long objID, long idVersion, object obj, int objIdType, int objIdCommunity, string moduleCode, Int32 idModule = 0)
        {
            ModuleObject oObject = new ModuleObject();
            oObject.FQN = obj.GetType().FullName;
            oObject.ObjectLongID = objID;
            oObject.ObjectOwner = obj;
            oObject.ObjectTypeID = objIdType;
            oObject.ServiceCode = moduleCode;
            oObject.CommunityID = objIdCommunity;
            oObject.ObjectIdVersion = idVersion;
            if (idModule > 0)
                oObject.ServiceID = idModule;
            return oObject;
        }

        public static ModuleObject CreateGuidObject(Guid objID, object obj, int objIdType, int objIdCommunity, string moduleCode, int idModule)
        {
            ModuleObject oObject = new ModuleObject();
            oObject.FQN = obj.GetType().FullName;
            oObject.ObjectGuidID = objID;
            oObject.ObjectOwner = obj;
            oObject.ObjectTypeID = objIdType;
            oObject.ServiceCode = moduleCode;
            oObject.ServiceID = idModule;
            oObject.CommunityID = objIdCommunity;

            return oObject;
        }
        public static ModuleObject CreateLongObject(long objID, int objIdType, int objIdCommunity, string moduleCode, Int32 idModule = 0)
        {
            ModuleObject oObject = new ModuleObject();
            oObject.ObjectLongID = objID;
            oObject.ObjectTypeID = objIdType;
            oObject.ServiceCode = moduleCode;
            oObject.CommunityID = objIdCommunity;
            oObject.ServiceID = idModule;
            return oObject;
        }
        public static ModuleObject CreateGuidObject(Guid objID, int objIdType, int objIdCommunity, string moduleCode)
        {
            ModuleObject oObject = new ModuleObject();
            oObject.ObjectGuidID = objID;
            oObject.ObjectTypeID = objIdType;
            oObject.ServiceCode = moduleCode;
            oObject.CommunityID = objIdCommunity;
            return oObject;
        }

        public static ModuleObject CloneObject(ModuleObject obj)
        {
            return new ModuleObject() { CommunityID = obj.CommunityID, FQN = obj.FQN, ObjectGuidID = obj.ObjectGuidID, ObjectIdVersion = obj.ObjectIdVersion, ObjectLongID = obj.ObjectLongID, ObjectOwner = obj, ObjectTypeID = obj.ObjectTypeID, ServiceCode = obj.ServiceCode, ServiceID = obj.ServiceID };
        }

        public virtual String ToString()
        {
            return "oType:" + FQN + " type:" + ObjectTypeID.ToString() + " Id:" + ObjectLongID.ToString() + " IdVersion:" + ObjectIdVersion.ToString() + " idCommunity:" + CommunityID.ToString() + " module:" + ServiceCode;
        }
    }
}