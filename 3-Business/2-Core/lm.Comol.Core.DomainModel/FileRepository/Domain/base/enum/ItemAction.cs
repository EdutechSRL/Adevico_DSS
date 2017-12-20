using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public enum ItemAction : int 
    {
        none = 0,
        preview = 1,
        play = 2,
        download = 3,
        details= 4,
        viewMyStatistics = 5,
        viewOtherStatistics = 6,
        edit = 7,
        editPermission = 8,
        viewPermission = 9,
        editSettings = 10,
        addVersion = 11,
        removeVersion = 12,
        manageVersions = 13,
        link = 14,
        addlink = 15,
        gotofolderfather = 16,
        hide = 17,
        show = 18,
        move = 19,
        virtualdelete = 20,
        undelete = 21,
        delete = 22,
        setCurrentVersion = 23,
        allowSelection = 24,
        upload = 25,
        addfolder = 26
    } 
}