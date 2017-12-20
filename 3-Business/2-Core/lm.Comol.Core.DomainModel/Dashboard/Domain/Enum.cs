using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    /// <summary>
    /// Available views
    /// </summary>
    [Serializable]
    public enum DashboardViewType
    {
        List = 0,
        Combined = 1,
        Tile = 2,
        Search = 3,
        Subscribe =4
    }
    /// <summary>
    /// Where noticeboard must be display
    /// </summary>
    [Serializable]
    public enum DisplayNoticeboard
    {
        Hide = 0,
        OnLeft = 1,
        OnRight = 2,
        InheritsFromDefault = 3,
        DefinedOnAllPages = 4
    }
    /// <summary>
    /// Tell if search filed must be hided or showed on view
    /// </summary>
    [Serializable]
    public enum DisplaySearchItems
    {
        Hide = 0,
        Simple = 1,
        Advanced = 2,
    }
    /// <summary>
    /// Available options to load settings
    /// </summary>
    [Serializable]
    public enum OnLoadSettings
    {
        AlwaysDefault = 0,
        UserDefault = 1,
        UserLastClick = 2,
    }
    [Serializable]
    public enum OrderItemsBy
    {
        LastAccess = 0,
        CreatedOn = 1,
        ClosedOn = 2,
        Name = 3,
        ActivatedOn = 4
    }
    /// <summary>
    /// Types of grouping available
    /// </summary>
    [Serializable]
    public enum GroupItemsBy
    {
        None = 0,
        Tile = 1,
        CommunityType = 2,
        Tag = 3,
        Service = 4
    }

    /// <summary>
    /// How link "More iotems" must be showed on page
    /// </summary>
    [Serializable]
    public enum DisplayMoreItems
    {
        AsLink = 0,
        AsTile = 1,
        Hide = 2,
    }
    /// <summary>
    /// All dashboard types
    /// </summary>
    [Serializable]
    public enum DashboardType
    {
        Portal = 0,
        AllCommunities = 1,
        Community = 2,
    }
    [Serializable]
    public enum DashboardAssignmentType
    {
        ProfileType = 0,
        RoleType = 1,
        User = 2,
    }

    [Serializable]
    public enum TileType
    {
        None = -2,
        CommunityTag = 0,
        Module = 1,
        CombinedTags = 2,
        UserDefined = 3,
        CommunityType = 4,
        DashboardUserDefined = 5
    }
    [Serializable]
    public enum TileItemType
    {
        UserDefinedUrl = 0,
        NewUrl = 1,
        StatisticUrl = 2,
        SettingsUrl = 3,
    }

    [Serializable]
    public enum PlainLayout
    {
        ignore = 0,
        box7box5 = 7,
        box8box4 = 8,
        full = 12
    }
    [Serializable]
    public enum TileLayout: int 
    {
        grid_12 = 12,
        grid_6 = 6,
        grid_4 = 4,
        grid_3 = 3,
        grid_2 = 2,
        grid_1 = 1,
        grid_0 = 0
    }

    [Serializable]
    public enum AvailableStatus
    {
        Any = -1,
        Draft = 0,
        Available = 1,
        Unavailable = 2,
    }

    public enum OrderSettingsBy
    {
        None = 0,
        Default = 1,
        Name = 2,
        ModifiedOn = 3,
        ModifiedBy = 4,
        Status = 5,
    }
    /// <summary>
    /// Step wizard
    /// </summary>
    public enum WizardDashboardStep
    {
        None = 0,
        Settings = 1,
        HomepageSettings = 2,
        Tiles = 3,
        CommunityTypes = 4,
        Modules = 5,
    }
    /// <summary>
    /// Order communities for subscription
    /// </summary>
    [Serializable]
    public enum OrderItemsToSubscribeBy
    {
        Name = 0,
        SubscriptionClosedOn = 1,
        SubscriptionOpenOn = 2,
        MaxUsers = 3,
        Responsible = 4,
        Year = 5,
        Timespan = 6,
        DegreeType = 7
    }

}