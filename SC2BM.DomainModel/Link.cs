using System;
using System.Runtime.Serialization;

namespace SC2BM.DomainModel
{
    [DataContract]
    public class Link
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string MainLink { get; set; }

        [DataMember]
        public string SecondaryLink { get; set; }

        [DataMember]
        public string DisplayText { get; set; }

        [DataMember]
        public string EntityType { get; set; }

        [DataMember]
        public int EntityID { get; set; }

        [DataMember]
        public int OwnerUserID { get; set; }

        [DataMember]
        public string OwnerUserName { get; set; }

        [DataMember]
        public DateTime AddedDate { get; set; }
    }
}