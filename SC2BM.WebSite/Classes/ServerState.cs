using System.Runtime.Serialization;
using SC2BM.DomainModel;

namespace SC2BM.WebSite.Classes
{
    [DataContract]
    public class ServerState
    {
        [DataMember]
        public User User { get; set; }

        [DataMember]
        public string Ip { get; set; }
    }
}