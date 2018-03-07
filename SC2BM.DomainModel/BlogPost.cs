using System;
using System.Runtime.Serialization;

namespace SC2BM.DomainModel
{
    [DataContract]
    public class BlogPost
    {
        [DataMember]
        public int? ID { get; set; }

        [DataMember]
        public int? BlogID { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public int OwnerUserID { get; set; }

        [DataMember]
        public string OwnerUserName { get; set; }

        [DataMember]
        public DateTime AddedDate { get; set; }

        [DataMember]
        public string LogoPath { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }
    }
}