using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SC2BM.DomainModel
{
    [DataContract]
    public class BuildOrderItemInfo
    {
        public BuildOrderItemInfo()
        {
            AdditionalValues = new Dictionary<string, int>();
        }

        [DataMember]
        public int Limit { get; set; }

        [DataMember]
        public int MaxLimit { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public int StartedSecond { get; set; }

        [DataMember]
        public int FinishedSecond { get; set; }

        [DataMember]
        public string ItemType { get; set; }

        [DataMember]
        public string StartTime 
        {
            get { return CalculateTimeFromSeconds(StartedSecond); }
        }

        [DataMember]
        public string EndTime 
        {
            get { return CalculateTimeFromSeconds(FinishedSecond); } 
        }

        [DataMember]
        public Dictionary<string, int> AdditionalValues { get; set; }

        private static string CalculateTimeFromSeconds(int second)
        {
            int min = second / 60;
            string strMin = CalculatePartTimeView(min);
            string strSec = CalculatePartTimeView(second - min * 60);

            return string.Format("{0}:{1}", strMin, strSec);
        }

        private static string CalculatePartTimeView(int second)
        {
            if (second < 10)
            {
                return string.Format("0{0}", second);
            }

            if (second < 60)
            {
                return string.Format("{0}", second);
            }

            return "--";
        }
    }
}