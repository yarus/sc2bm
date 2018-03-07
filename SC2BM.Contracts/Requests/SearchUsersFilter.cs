using System.Runtime.Serialization;

namespace SC2BM.ServiceModel.Requests
{
    [DataContract]
    public class SearchUsersFilter
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public bool IsStrictSearch { get; set; }
    }
}