using System.Xml.Serialization;

namespace ParseXML {
    [XmlRoot("way")]
    public class Way {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("nd")]
        public WayNd[] Nds { get; set; }

        [XmlElement("tag")]
        public WayTag[] Tags { get; set; }
    }

    public class WayNd {
        [XmlAttribute("ref")]
        public string Ref { get; set; }
    }

    public class WayTag {

        [XmlAttribute("k")]
        public string K { get; set; }

        [XmlAttribute("v")]
        public string V { get; set; }
    }
}
