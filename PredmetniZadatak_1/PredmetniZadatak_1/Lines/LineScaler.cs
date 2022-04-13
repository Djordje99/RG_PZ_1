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
        private Tuple<List<long>, List<long>> lineIDs;
        private List<long> lineFirstList = new List<long>();
        private List<long> lineSecondList = new List<long>();
        private XmlLoader xmlLoader = new XmlLoader("Geographic.xml");

        public Tuple<List<long>, List<long>> LineIDs { get => lineIDs; set => lineIDs = value; }

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
                long secondEnd = Int64.Parse(firstEndStr);
                long firstEnd = Int64.Parse(secondEndStr);

                if ((lineFirstList.Contains(firstEnd) && lineSecondList[lineFirstList.IndexOf(firstEnd)] == secondEnd)
                    || lineSecondList.Contains(firstEnd) && lineFirstList[lineSecondList.IndexOf(firstEnd)] == secondEnd)
                    continue;

                lineFirstList.Add(secondEnd);
                lineSecondList.Add(firstEnd);
            }

            lineIDs = new Tuple<List<long>, List<long>>(lineFirstList, lineSecondList);
        }
    }
}
