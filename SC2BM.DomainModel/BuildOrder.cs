using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SC2BM.DomainModel
{
    [DataContract]
    public class BuildOrder
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string SC2VersionID { get; set; }

        [DataMember]
        public string Race { get; set; }

        [DataMember]
        public string VsRace { get; set; }

        [DataMember]
        public List<string> BuildItems { get; set; }

        [DataMember]
        public int OwnerUserID { get; set; }

        [DataMember]
        public string OwnerUserName { get; set; }

        [DataMember]
        public DateTime AddedDate { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public int MicroRate { get; set; }

        [DataMember]
        public int MacroRate { get; set; }

        [DataMember]
        public int AggressionRate { get; set; }

        [DataMember]
        public int DefenceRate { get; set; }
    }
}