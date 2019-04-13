using System.Xml.Serialization;

namespace ParseXML {
    [XmlRoot("node")]
    public class WayNode {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("lat")]
        public string Lat { get; set; }

        [XmlAttribute("lon")]
        public string Lon { get; set; }
    }
}
