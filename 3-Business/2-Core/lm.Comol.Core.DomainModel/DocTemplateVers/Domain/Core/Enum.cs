using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers
{
    /// <summary>
    /// Template Type:
    /// Standard = template Standard
    /// System = template di Systema, necessita di permessi particolari per essere modificato
    /// Skin = utilizza le skin e necessita quindi di alcuni parametri particolari
    /// External = utilizza un pdf base che andrà mappato opportunamente (sviluppi futuri)
    /// </summary>
    [Serializable]
    public enum TemplateType
    {
        Standard = 0,
        System = 1,
        Skin = 2,
        External = 3
    }

    [Serializable]
    public enum ElementPosition
    {
        HeaderLeft = 10,
        HeaderCenter = 11,
        HeaderRight = 12,
        Body = 21,
        FooterLeft = 30,
        FooterCenter = 31,
        FooterRight = 32
    }

    /// <summary>
    /// Formato immagine di sfondo. Eventualmente calcolare il posizionamento...
    /// </summary>
    [Serializable]
    public enum BackgrounImagePosition
    {
        Center = 0,
        Tiled = 1,
        Stretch = 2
    }

    [Serializable]
    public enum ElementAlignment
    {
        TopLeft = 10, TopCenter = 11, TopRight = 12,
        MiddleLeft = 20, MiddleCenter = 21, MiddleRight = 22,
        BottomLeft = 30, BottomCenter = 31, BottomRight = 32
    }

    /// <summary>
    /// PageSize
    /// L sta per LANDSCAPE (Orizzontale)
    /// Tutti gli "L" sono DISPARI. In questo modo è più facile riconoscerli.
    /// </summary>
    [Serializable]
    public enum PageSize
    {
        custom = 0,
        //none = 0, 
        A0 = 100, A0_L = 101,
        A1 = 110, A1_L = 111,
        A2 = 120, A2_L = 121,
        A3 = 130, A3_L = 131,
        A4 = 140, A4_L = 141,
        A5 = 150, A5_L = 151,
        A6 = 160, A6_L = 161,
        A7 = 170, A7_L = 171,
        A8 = 180, A8_L = 181,
        A9 = 190, A9_L = 191,
        A10 = 194, A10_L = 195,

        B0 = 200, B0_L = 201,
        B1 = 210, B1_L = 211,
        B2 = 220, B2_L = 221,
        B3 = 230, B3_L = 231,
        B4 = 240, B4_L = 241,
        B5 = 250, B5_L = 251,
        B6 = 260, B6_L = 261,
        B7 = 270, B7_L = 271,
        B8 = 280, B8_L = 281,
        B9 = 290, B9_L = 291,
        B10 = 294, B10_L = 295,
        
        LEGAL = 300, LEGAL_L = 301,
        LETTER = 304, LETTER_L = 305,
        TABLOID = 310, TABLOID_L = 311,
        POSTCARD = 320, POSTCARD_L = 321,
        NOTE = 330, NOTE_L = 331,
        HALFLETTER = 340, HALFLETTER_L = 341,
        _11X17 = 350, _11X17_L = 351
    }

    [Serializable]
    public enum SignaturePosition
    {
        left = 1,
        center = 2,
        right = 3
    }

    [Serializable]
    public enum ItemUpdating
    {
        Added, Updated, NotNecessary, Error, NoPermission
    }
}
