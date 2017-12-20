using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace lm.Comol.Core.Authentication{
    [DataContract]
    public enum ProfilerError
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
        itemOwnerNotFound = 9,
        [EnumMember]
        providerUnavailable = 10,
        [EnumMember]
        itemUpdated = 11,
        [EnumMember]
        loginduplicate = 12,
        [EnumMember]
        taxCodeDuplicate = 13,
        [EnumMember]
        mailDuplicate = 14,
        [EnumMember]
        invalidPassword = 15,
        [EnumMember]
        invalidUsername = 16,
        [EnumMember]
        uniqueIDduplicate = 17,
        [EnumMember]
        externalUniqueIDduplicate = 18
    }
}