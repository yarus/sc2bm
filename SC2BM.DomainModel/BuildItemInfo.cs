using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SC2BM.DomainModel
{
    [DataContract]
    public class BuildItemInfo
    {
        public BuildItemInfo()
        {
            Requirements = new List<BuildItemInfo>();
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public int CostMinerals { get; set; }

        [DataMember]
        public int CostGas { get; set; }

        [DataMember]
        public int CostSupply { get; set; }

        [DataMember]
        public int BuildTime { get; set; }

        [DataMember]
        public List<BuildItemInfo> Requirements { get; set; } 
    }
}