using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.OpenMeetings
{
    /// <summary>
    /// Parametri stanza OpenMeetings
    /// </summary>
    [Serializable]
    public class oMRoomParameters : WbRoomParameter
    {
        /// <summary>
        /// costruttore vuoto
        /// </summary>
        public oMRoomParameters()
        {
            allowUserQuestions = true;
            isModeratedRoom = false;
            isPasswordProtected = false;
            ispublic = false;
            numberOfPartizipants = 10;
            password = "";
            redirectURL = "";
            validFromDate = DateTime.Now;
            validToDate = DateTime.Now.AddYears(1);
            
            //this.allowUserQuestions = true;
            //this.hideActionsMenu = false;
            //this.hideActivitiesAndActions = false;
            //this.hideChat = false;
            //this.hideFilesExplorer = false;
            //this.hideScreenSharing = false;
            //this.hideTopBar = false;
            //this.hideWhiteboard = false;
            //this.isAudioOnly = false;
            //this.reminderTypeId = 1;
            
        }
        /// <summary>
        /// enable or disable the button to allow users to apply for moderation
        /// </summary>
        public Boolean allowUserQuestions { get; set; }
        /// <summary>
        /// If room is moderated
        /// </summary>
        public Boolean isModeratedRoom { get; set; }      //  <- modify
        /// <summary>
        /// If the links send via EMail to invited people is password protected
        /// </summary>
        public Boolean isPasswordProtected { get; set; }      //  <- modify
        /// <summary>
        /// is public
        /// </summary>
        public Boolean ispublic { get; set; }
        /// <summary>
        /// number of participants
        /// </summary>
        public long numberOfPartizipants { get; set; }      //  <- modify
        /// <summary>
        /// Password for Invitations send via Mail
        /// </summary>
        public String password { get; set; }      //  <- modify
        /// <summary>
        /// URL Users will be lead to if the Conference Time is elapsed
        /// </summary>
        public String redirectURL { get; set; }
        /// <summary>
        /// Data inizio
        /// </summary>
        public DateTime validFromDate { get; set; }
        /// <summary>
        /// Data fine
        /// </summary>
        public DateTime validToDate { get; set; }

        /////  ALTRI PARAMETRI "IGNOTI"

        ///// <summary>
        ///// The SID of the User. This SID must be marked as Loggedin
        ///// </summary>
        //public String SID { get; set; }
        ///// <summary>
        ///// the room id to update
        ///// </summary>
        //public long room_id { get; set; }
        ///// <summary>
        ///// new name of the room
        ///// </summary>
        //public String name { get; set; }
        ///// <summary>
        ///// new type of room (1 = Conference, 2 = Audience, 3 = Restricted, 4 = Interview)
        ///// </summary>
        //public long roomtypes_id { get; set; }
        ///// <summary>
        ///// new comment
        ///// </summary>
        //public String comment { get; set; }
        ///// <summary>
        ///// if the room is an appointment (use false if not sure what that means)
        ///// </summary>
        //public Boolean appointment { get; set; }
        ///// <summary>
        ///// is it a Demo Room with limited time? (use false if not sure what that means)
        ///// </summary>
        //public Boolean isDemoRoom { get; set; }
        ///// <summary>
        ///// time in seconds after the user will be logged out (only enabled if isDemoRoom is true)
        ///// </summary>
        //public int demoTime { get; set; }
        ///// <summary>
        ///// Users have to wait until a Moderator arrives. Use the becomeModerator parameter in setUserObjectAndGenerateRoomHash to set a user as default Moderator
        ///// </summary>
        ///// <summary>
        ///// enable or disable the video / or audio-only
        ///// </summary>
        //public Boolean isAudioOnly { get; set; }
        ///// <summary>
        ///// hide or show TopBar
        ///// </summary>
        //public Boolean hideTopBar { get; set; }
        ///// <summary>
        ///// hide or show Chat
        ///// </summary>
        //public Boolean hideChat { get; set; }
        ///// <summary>
        ///// hide or show Activities And Actions
        ///// </summary>
        //public Boolean hideActivitiesAndActions { get; set; }
        ///// <summary>
        ///// hide or show Files Explorer
        ///// </summary>
        //public Boolean hideFilesExplorer { get; set; }
        ///// <summary>
        ///// hide or show Actions Menu
        ///// </summary>
        //public Boolean hideActionsMenu { get; set; }
        ///// <summary>
        ///// hide or show Screen Sharing
        ///// </summary>
        //public Boolean hideScreenSharing { get; set; }
        ///// <summary>
        ///// hide or show Whiteboard. If whitboard is hidden, video pods and scrollbar appear instead.
        ///// </summary>
        //public Boolean hideWhiteboard { get; set; }
        ///// <summary>
        ///// 1=none, 2=simple mail, 3=ICAL
        ///// </summary>
        //public long reminderTypeId { get; set; }
    }
}
