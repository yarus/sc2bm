using System;
using System.Runtime.Serialization;

namespace SC2BM.DomainModel
{
    [DataContract]
    public class Comment
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int OwnerUserID { get; set; }

        [DataMember]
        public string OwnerUserName { get; set; }

        [DataMember]
        public string EntityType { get; set; }

        [DataMember]
        public DateTime AddedDate { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public int EntityID { get; set; }
    }
}