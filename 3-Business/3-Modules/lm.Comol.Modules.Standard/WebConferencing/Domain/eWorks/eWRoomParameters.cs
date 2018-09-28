using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks
{
    [Serializable]
	public class eWRoomParameters : WbRoomParameter
	{
		/*Non presenti in "Get": (compresa 6.3) */
		public string audiocodec { get; set; } // -> (string, , 100) 

		/*Invariati*/
		//public bool crypt { get; set; } // -> (integer, /^(0|1)$/)        Cifra le comunicazioni
		//public bool dashboard { get; set; } // -> (integer, /^(0|1)$/)      Pubblica il meeting sulla home eWorks
		public string description { get; set; } // -> (string) 
		public eWLanguages language { get; set; } // -> (string, (it|en|de|fr|es), 2) 
		public int meetingduration { get; set; } // -> (integer, /[0-9]/) 
		public bool meetinglog { get; set; } // -> (integer, /^(0|1)$/), attiva il log esteso di un meeting 
		public DateTime? meetingstart { get; set; } // -> (string, /^[0-9]{14}$/)  yyyyMMddHHmmss
		public string meetingtitle { get; set; } // -> (string, , 200) 
		public string meetingpassword { get; set; } // -> (string, , 50) 
		public eWLanguages part_language { get; set; } // -> (string, (it|en|de|fr|es), 2) 
		public string part_properties { get; set; } // -> (string, /^(A|V|T){0, 3}$/) 
		public string properties { get; set; } // -> (string, /^(A|V|T){0, 3}$/) 
		public bool recording { get; set; } // -> (integer, /^(0|1)$/) 
		public eWsharingType sharingtype { get; set; } // -> (string, /^(none|whiteboard|screen|browser|dicom)$/) 
		public int timezone { get; set; } // -> (integer)  
		//public bool udpenabled { get; set; } // -> (integer, /^(0|1)$/)       Abilita UDP
		public bool usedatetime { get; set; } // -> (integer, /^(0|1)$/) 
		//public bool needaccount { get; set; } // -> (integer, /^(0|1)$/)
		//public bool forcehost { get; set; } // -> (integer, /^(0|1)$/)          Il primo che entra ha l'host
		//public bool forcecontroller { get; set; } // -> (integer, /^(0|1)$/)    Il primo che entra ha il controller
		//public bool controller { get; set; } // -> (integer, /^(0|1)$/)
		//public bool host { get; set; } //-> (integer, /^(0|1)$/)

		/*Nuovi (da 6.2)*/
		public string clientname { get; set; } //???
		public string email { get; set; } //???
		/// <summary>
		/// Video Framerate
		/// </summary>
		public int framerate { get; set; }
		/// <summary>
		/// Voice Switched Video
		/// </summary>
		public bool vav { get; set; }   //Voice Switched Video
		/// <summary>
		/// SVC (Auto tuning bitrate)
		/// </summary>
		public bool svc { get; set; }   //SVC ???
/// <summary>
		/// ???
		/// </summary>
		public string endsessionurl { get; set; }

		/* Nuovi 6.2, tolti 6.3 */
		/// <summary>
		/// ???
		/// </summary>
		public int audiopayload { get; set; }

		/*Solo in documentaizone, get*/
		//public object collaborationtype { get; set; }   //=-> SharingType?

		/* Tolti in 6.3 */
		public List<int> bitrate { get; set; } // -> (integer, elenco bitrate valide per l'utente separato da "|")  
		public string videocodec { get; set; } // -> (string, , 100) 
		public int videoheight { get; set; } // -> (integer) 
		public int videowidth { get; set; } // -> (integer) 
	}

	public enum eWLanguages
	{
		it,en,de,fr,es
	}

	public enum eWsharingType //Rivedere, se necessari più parametri...!
	{
		none,whiteboard,screen,browser,dicom
	}
}

/*              Riepilogo riunione parametri  */

/*Valori di default per:

- Framerate	    Default: 15
- AudioCodec	(Fisso: ISAC 16K)
- VoiceSvitched video: a template (VAV)
- bitrate (a seconda dei template)
- Abilita il Gateway PIN 63575 <- set/get
- AVT: in base al template!
- vav: Voice Switched Video

Prametri "intoccabili"

- SVC = downscaling bitrate	(svc=1, default (>2 persone))
- needaccount	(vincola alla presenza) -> FALSE
- forcehost		False
- forcecontroller	False

http://www.e-works.it/resources/beta/e-works_setup_beta.exe

SEGARE:
- crypt		VIA! (FALSE)
- Videocodec	(SEGARE)
- UDPEnabled	(VIA!)
- dashboard=1	(VIA!)


SU CREATE KEY:
- controller		Permessi admin		"Vedi Create Key"	

TEST su "multi partecipanti"
- host			"Centro Stella"	(Automatico)
 
 */


/*              Riepilogo parametri e versioni
  
						5.0		6.2		6.3
		audiocodec 		 X		 X		 X
		crypt 			 V		 V		 V
		dashboard 		 V		 V		 V
		description 	 V		 V		 V
		language 		 V		 V		 V
		meetingduration	 V		 V		 V
		meetinglog		 V		 V		 V
		meetingstart	 V		 V		 V
		meetingtitle	 V		 V		 V
		meetingpassword	 V		 V		 V
		part_language	 V		 V		 V
		part_properties	 V		 V		 V
		properties		 V		 V		 V
		recording		 V		 V		 V
		sharingtype		 V		 V		 V
		timezone		 V		 V		 V
		udpenabled		 V		 V		 V
		usedatetime		 V		 V		 V
		needaccount		 V		 V		 V
		forcehost		 V		 V		 V
		forcecontroller	 V		 V		 V
		controller		 V		 V		 V
		host			 V		 V		 V
		clientname		 X		 V		 V
		email			 X		 V		 V
		framerate		 X		 V		 V
		vav				 X		 V		 V
		svc				 X		 V		 V
		endsessionurl	 X		 V		 V
		audiopayload	 X		 V		 X
		collaborationtype X		 X		 X
		bitrate			 V		 V		 X
		videocodec		 V		 V		 X
		videoheight		 V		 V		 X
		videowidth		 V		 V		 X
 
 */