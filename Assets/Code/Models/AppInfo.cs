using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Models
{
    /// <summary>
    /// Represents Param.Sfo Info
    /// </summary>
    public class AppInfo
    {
        public string titleId { get; set; }
        public string key { get; set; }
        public string val { get; set; }
    }

    /// <summary>
    /// This info is stored per userid 
    /// e.g. user 1000000 will be tbl_appbrowse_0268435456
    /// </summary>
    public class AppBrowse
    {
        public string titleId { get; set; }
        public string contentId { get; set; }

        public string titleName { get; set; }

        public string metaDataPath { get; set; }

        public string lastAccessTime { get; set; }

        public int ContentStatus { get; set; }

        public int OnDisc { get; set; }

        public int parentalLevel { get; set; }

        public bool visible { get; set; }

        public int sortPriority { get; set; }

        public long pathInfo { get; set; }

        public int lastAccessIndex { get; set; }

        public int dispLocation { get; set; }

        public bool canRemove { get; set; }

        public string category { get; set; }

        public int contentType { get; set; }

        public int pathInfo2 { get; set; }

        public int presentBoxStatus { get; set; }

        public int entitlement { get; set; }

        public string thumbnailUrl { get; set; }

        public string lastUpdateTime { get; set; }

        public DateTime playableDate { get; set; }

        public long contentSize { get; set; }

        public DateTime installDate { get; set; }

        public int platform { get; set; }

        public string uiCategory { get; set; }

        public string skuId { get; set; }

        public int disableLiveDetail { get; set; }

        public int linkType { get; set; }

        public string linkUri { get; set; }

        public string serviceIdAddCont1 { get; set; }
        public string serviceIdAddCont2 { get; set; }
        public string serviceIdAddCont3 { get; set; }
        public string serviceIdAddCont4 { get; set; }
        public string serviceIdAddCont5 { get; set; }
        public string serviceIdAddCont6 { get; set; }
        public string serviceIdAddCont7 { get; set; }

        public int folderType { get; set; }

        public string folderInfo { get; set; }

        public string parentFolderId { get; set; }
        public string positionInFolder { get; set; }
        public DateTime activeDate { get; set; }
        public string entitlementTitleName { get; set; }

        public int hddLocation { get; set; }
        public int externalHddAppStatus { get; set; }
        public string entitlementIdKamaji { get; set; }
        public DateTime mTime { get; set; }
    }

}
