using System.Runtime.Serialization;

namespace SC2BM.ServiceModel.Requests
{
    [DataContract]
    public class SearchBuildOrdersFilter
    {
        [DataMember]
        public int? BuildOrderID { get; set; }

        [DataMember]
        public bool IsStrictSearch { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string SC2VersionID { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Race { get; set; }

        [DataMember]
        public string VsRace { get; set; }

        [DataMember]
        public int? OwnerUserID { get; set; }

        [DataMember]
        public bool? IsDeleted { get; set; }
    }
}
