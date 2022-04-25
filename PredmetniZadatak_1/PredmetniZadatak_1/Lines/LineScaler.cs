using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PredmetniZadatak_1.Lines
{
    public class LineScaler
    {
        private List<Tuple<string, long, long>> lineIDs = new List<Tuple<string, long, long>>();
        private List<long> lineFirstList = new List<long>();
        private List<long> lineSecondList = new List<long>();
        private XmlLoader xmlLoader = new XmlLoader("Geographic.xml");
        public List<Tuple<string, long, long>> LineIDs { get => lineIDs; set => lineIDs = value; }

        public LineScaler()
        {
            LoadLines();
        }

        private void LoadLines()
        {
            XmlNodeList nodes = null;

            nodes = xmlLoader.ReadXml("/NetworkModel/Lines/LineEntity");

            foreach (XmlNode node in nodes)
            {
                string firstEndStr = node.SelectSingleNode("FirstEnd").InnerText;
                string secondEndStr = node.SelectSingleNode("SecondEnd").InnerText;
                string id = node.SelectSingleNode("Id").InnerText;
                string name = node.SelectSingleNode("Name").InnerText;
                string IsUnderground = node.SelectSingleNode("IsUnderground").InnerText;
                string r = node.SelectSingleNode("R").InnerText;
                string conductorMaterial = node.SelectSingleNode("ConductorMaterial").InnerText;
                string lineType = node.SelectSingleNode("LineType").InnerText;
                string thermalConstantHeat = node.SelectSingleNode("ThermalConstantHeat").InnerText;

                string toolTip = string.Format("\t\tLine\n\tID {0}\n\tName " +
                    "{1}\n\tIsUnderground {2}\n\tR {3}\n\tConductor material {4}\n\tLine type {5}\n\tThermal Constant Heat {6}" +
                    "\n\tStartEnd {7}\n\tSecondEnd {8}"
                    , id, name, IsUnderground, r, conductorMaterial, lineType, thermalConstantHeat, firstEndStr, secondEndStr);

                long secondEnd = Int64.Parse(firstEndStr);
                long firstEnd = Int64.Parse(secondEndStr);

                if ((lineFirstList.Contains(firstEnd) && lineSecondList[lineFirstList.IndexOf(firstEnd)] == secondEnd)
                    || lineSecondList.Contains(firstEnd) && lineFirstList[lineSecondList.IndexOf(firstEnd)] == secondEnd)
                    continue;

                lineIDs.Add(new Tuple<string, long, long>(toolTip, firstEnd, secondEnd));
            }
        }
    }
}
