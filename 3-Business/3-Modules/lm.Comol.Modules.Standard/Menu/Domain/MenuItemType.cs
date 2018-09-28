using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    /// <summary>
    /// Tipi di elemento per il menu.
    /// 1 Text: elemento di solo testo. Non puo' contenere ulteriori elementi
    /// 2 Link: link. Puo' contenere fino a 3 elementi, che sono le 3 immagini: New-Stat-Admin
    /// 3 Separator: di default senza link. Divide in blocchi varie voci del menu. Puo' contenere ulteriori voci.
    /// 
    /// 11 TextContainer: come Text, ma puo' contenere sotto elementi (testo che raggruppa più elementi)
    /// 12 LinkConteiner: come Link, ma puo' contenere più sottoelementi (link che raggruppa più elementi)
    /// 
    /// 21 ImgNew: link per l'immagine "NEW"
    /// 22 ImgNew: link per l'immagine "STAT"
    /// 23 ImgNew: link per l'immagine "ADMIN"
    /// </summary>
    /// 
    [Serializable]
    public enum MenuItemType
    {
        None = 0,
        Text = 1,
        Link = 2,
        Separator = 3,

        TextContainer = 11,
        LinkContainer = 12,

        IconNewItem = 21,
        IconStatistic = 22,
        IconManage = 23,
        ItemColumn = 40,
        TopItemMenu = 41,
        Menubar =42,

    }
}