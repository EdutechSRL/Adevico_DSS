using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Helpers.Export
{
    /// <summary>
    /// Tipo di file esportato
    /// </summary>
    [Serializable, Flags]
    public enum ExportMode
    {
        none = 0,
        web = 1,
        file = 2
    }
    /// <summary>
    /// Tipo di file esportato
    /// </summary>
    [Serializable]
    public enum ExportFileType
    {
        none = 0,
        zip = 1,
        pdf = 2,
        xls = 4,
        xml = 5,
        csv = 6
    }
    //rtf = 3,

    /// <summary>
    /// Formato immagine di sfondo. Eventualmente calcolare il posizionamento...
    /// </summary>
    [Serializable]
    public enum BackgrounImageFormat
    {
        Center = 0,
        Tiled = 1,
        Stretch = 2
    }

    /// <summary>
    /// Allineamento elementi
    /// </summary>
    [Serializable]
    public enum ElementAlignment
    {
        TopLeft = 0, TopCenter = 1, TopRight = 2,
        MiddleLeft = 3, MiddleCenter = 4, MiddleRight = 5,
        BottomLeft = 6, BottomCenter = 7, BottomRight = 8
    }

    [Serializable]
    public enum TemplateType
    {
        standard = 0
    }
}
