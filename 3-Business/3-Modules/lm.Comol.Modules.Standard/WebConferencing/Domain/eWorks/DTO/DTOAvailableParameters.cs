using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks.DTO
{
    public class DTOAvailableParameters
    {
        public IList<eWLanguages> Languages { get; set; }
        public IList<eWsharingType> SharingTypes { get; set; }
        public IList<DTOVideoFormat> VideoSizes { get; set; }
        public IList<String> VideoCodec { get; set; }
        public IList<int> Framerates { get; set; }
        public IList<int> Bitrate { get; set; }

        public IList<string> AudioCodec { get; set; }
    }

}
