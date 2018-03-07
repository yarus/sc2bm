using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SC2BM.DomainModel
{
    [DataContract]
    public class KnowladgeBaseInfo
    {
        public KnowladgeBaseInfo(string sc2VersionId, string faction)
        {
            SC2VersionId = sc2VersionId;
            Faction = faction;

            Units = new List<BuildItemInfo>();
            Buildings = new List<BuildItemInfo>();
            Upgrades = new List<BuildItemInfo>();
        }

        [DataMember]
        public string SC2VersionId { get; set; }

        [DataMember]
        public string Faction { get; set; }

        [DataMember]
        public List<BuildItemInfo> Units { get; set; }
        
        [DataMember]
        public List<BuildItemInfo> Buildings { get; set; }

        [DataMember]
        public List<BuildItemInfo> Upgrades { get; set; } 
    }
}