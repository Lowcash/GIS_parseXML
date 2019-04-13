using System.Xml.Serialization;

namespace ParseXML {
    [XmlRoot("node")]
    public class Station {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("lat")]
        public string Lat { get; set; }

        [XmlAttribute("lon")]
        public string Lon { get; set; }

        [XmlElement("tag")]
        public StationTag[] Tags { get; set; }
    }

    public class StationTag {

        [XmlAttribute("k")]
        public string K { get; set; }

        [XmlAttribute("v")]
        public string V { get; set; }
    }
}
