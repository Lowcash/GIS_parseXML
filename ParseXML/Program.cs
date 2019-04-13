using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ParseXML {
    class Program {
        static void Main( string[] args ) {
            var pathToFile = "poruba.osm";

            var data = File.ReadAllLines(pathToFile);

            var stations = ProcessXML(data, "<node", "</node>").Where(s => s.Contains("public_transport")).ToList();

            var objectStations = new List<Station>();

            var stationSerializer = new XmlSerializer(typeof(Station));
            foreach (var station in stations) {
                using (var reader = new StringReader(station)) {
                    objectStations.Add((Station)stationSerializer.Deserialize(reader));
                }
            }

            var stationSB = new StringBuilder();
            foreach (var station in objectStations) {
                stationSB.AppendFormat("{0},{1},", station.Lon, station.Lat);

                foreach (var tag in station.Tags) {
                    if (tag.K == "name") {
                        stationSB.Append(tag.V);

                        break;
                    }
                }

                stationSB.Append("\n");
            }

            File.WriteAllText("porubaStations.dat", stationSB.ToString(), Encoding.UTF8);

            var ways = ProcessXML(data, "<way", "</way>").Where(s => s.Contains("primary") || s.Contains("residential")).ToList();

            var objectWays = new List<Way>();

            var waySerializer = new XmlSerializer(typeof(Way));
            foreach (var way in ways) {
                using (var reader = new StringReader(way)) {
                    objectWays.Add((Way)waySerializer.Deserialize(reader));
                }
            }

            var waysNodes = ProcessXML(data, "<node", "/>", isOnSameLine:true).Where(w => w.Any()).ToList();

            var objectWaysNodes = new Dictionary<string, WayNode>();

            var wayNodeSerializer = new XmlSerializer(typeof(WayNode));
            foreach (var wayNode in waysNodes) {
                using (var reader = new StringReader(wayNode)) {
                    var node = (WayNode)wayNodeSerializer.Deserialize(reader);
                    objectWaysNodes.Add(node.Id, node);
                }
            }

            var waySB = new StringBuilder();
            foreach (var way in objectWays) {
                foreach (var wayNode in way.Nds) {
                    if (objectWaysNodes.ContainsKey(wayNode.Ref)) {
                        waySB.AppendFormat("{0},{1}\n", objectWaysNodes[wayNode.Ref].Lon, objectWaysNodes[wayNode.Ref].Lat);
                    } 
                }

                waySB.Append("\n");
            }

            File.WriteAllText("porubaWays.dat", waySB.ToString(), Encoding.UTF8);
        }

        static List<string> ProcessXML( string[] lineData, string start, string end, bool isOnSameLine = false ) {
            var xmlNodes = new List<string>();

            var sb = new StringBuilder();

            bool isAppendingData = false;

            for (int i = 0; i < lineData.Length; i++) {
                if (lineData[i].IndexOf(start) != -1) {
                    sb.Clear();

                    isAppendingData = true;
                }

                if (isAppendingData)
                    sb.Append(lineData[i]);

                if (!isOnSameLine) {
                    if (lineData[i].IndexOf(end) != -1) {
                        xmlNodes.Add(sb.ToString());

                        isAppendingData = false;
                    }
                } else {
                    if (lineData[i].IndexOf(end) != -1) {
                        xmlNodes.Add(sb.ToString());
                    }

                    sb.Clear();

                    isAppendingData = false;
                }
                
            }

            return xmlNodes;
        }
    }
}
