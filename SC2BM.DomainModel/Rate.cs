using System;
using System.Runtime.Serialization;

namespace SC2BM.DomainModel
{
    [DataContract]
    public class Rate
    {
        [DataMember]
        public int ID { get; set; }
        
        [DataMember]
        public int OwnerUserID { get; set; }

        [DataMember]
        public string OwnerUserName { get; set; }

        [DataMember]
        public int EntityID { get; set; }

        [DataMember]
        public int Value { get; set; }

        [DataMember]
        public DateTime AddedDate { get; set; }

        [DataMember]
        public string EntityType { get; set; }
    }
}