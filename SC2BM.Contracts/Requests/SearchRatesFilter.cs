using System.Runtime.Serialization;

namespace SC2BM.ServiceModel.Requests
{
    [DataContract]
    public class SearchRatesFilter
    {
        [DataMember]
        public int? RateID { get; set; }

        [DataMember]
        public bool IsStrictSearch { get; set; }

        [DataMember]
        public int? OwnerUserID { get; set; }

        [DataMember]
        public string EntityType { get; set; }

        [DataMember]
        public int? EntityID { get; set; }
    }
}