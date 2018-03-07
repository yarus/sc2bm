using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SC2BM.DomainModel
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        [JsonIgnore]
        public string Password { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public List<string> Roles { get; set; }

        [DataMember]
        public DateTime BirthDate { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        [JsonIgnore]
        public string ActivationSalt { get; set; }

        [DataMember]
        public DateTime RegisteredDate { get; set; }
    }
}