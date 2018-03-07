using System.Runtime.Serialization;

namespace SC2BM.ServiceModel.Requests
{
    [DataContract]
    public class SearchLinksFilter
    {
        [DataMember]
        public int? LinkID { get; set; }

        [DataMember]
        public bool IsStrictSearch { get; set; }

        [DataMember]
        public int? OwnerUserID { get; set; }

        [DataMember]
        public string EntityType { get; set; }

        [DataMember]
        public int? EntityID { get; set; }

        [DataMember]
        public string Type { get; set; }
    }
}