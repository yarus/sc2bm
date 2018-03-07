using System;
using System.Runtime.Serialization;

namespace SC2BM.ServiceModel.Requests
{
    [DataContract]
    public class SearchBlogPostFilter
    {
        [DataMember]
        public int? BlogPostID { get; set; }

        [DataMember]
        public int? BlogID { get; set; }

        [DataMember]
        public bool IsStrictSearch { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public int? OwnerUserID { get; set; }

        [DataMember]
        public DateTime? FromDate { get; set; }

        [DataMember]
        public DateTime? ToDate { get; set; }

        [DataMember]
        public bool? IsDeleted { get; set; }
    }
}