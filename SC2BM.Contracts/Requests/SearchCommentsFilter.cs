using System.Runtime.Serialization;

namespace SC2BM.ServiceModel.Requests
{
    [DataContract]
    public class SearchCommentsFilter
    {
        [DataMember]
        public int? CommentID { get; set; }

        [DataMember]
        public bool IsStrictSearch { get; set; }

        [DataMember]
        public int? OwnerUserID { get; set; }

        [DataMember]
        public bool? IsDeleted { get; set; }

        [DataMember]
        public string EntityType { get; set; }

        [DataMember]
        public int? EntityID { get; set; }
    }
}