using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace lm.Comol.Core.Authentication{
    [DataContract]
    public enum AgencyError
    {
        [EnumMember]
        none = 0,
        [EnumMember]
        internalError = 1,
        [EnumMember]
        itemUnknow = 2,
        [EnumMember]
        itemDisabled = 3,
        [EnumMember]
        itemAlreadyExist = 4,
        [EnumMember]
        itemDeleted = 5,
        [EnumMember]
        itemNotDeleted = 6,
        [EnumMember]
        itemNotCreated = 7,
        [EnumMember]
        itemNotModified = 8,
        [EnumMember]
        itemUpdated = 9,
        [EnumMember]
        nameDuplicate = 10,
        [EnumMember]
        taxCodeDuplicate = 11,
        [EnumMember]
        externalCodeDuplicate = 12,
        [EnumMember]
        nationalCodeDuplicate = 13,
        [EnumMember]
        emptyValues = 14,
        [EnumMember]
        defaultNotAvailable = 15
    }
}