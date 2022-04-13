using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PredmetniZadatak_1.Dots
{
    public class DotScaler
    {
        private int minUtmX;
        private int minUtmY;
        private int maxUtmX;
        private int maxUtmY;
        private List<int> listOfX = new List<int>();
        private List<int> listOfY = new List<int>();
        private XmlLoader xmlLoader = new XmlLoader("Geographic.xml");

        public int MinUtmX { get => minUtmX; set => minUtmX = value; }
        public int MinUtmY { get => minUtmY; set => minUtmY = value; }
        public int MaxUtmX { get => maxUtmX; set => maxUtmX = value; }
        public int MaxUtmY { get => maxUtmY; set => maxUtmY = value; }

        public DotScaler()
        {
            LoadLists();
        }

        private void LoadLists()
        {
            List<string> enititisAddrs = new List<string>(3)
                { "/NetworkModel/Substations/SubstationEntity", "/NetworkModel/Nodes/NodeEntity", "/NetworkModel/Switches/SwitchEntity" };

            XmlNodeList nodes = null;

            foreach (string entity in enititisAddrs)
            {
                nodes = xmlLoader.ReadXml(entity);

                foreach (XmlNode node in nodes)
                {
                    int utmX = Int32.Parse(node.SelectSingleNode("X").InnerText.Split('.')[0]);
                    int utmY = Int32.Parse(node.SelectSingleNode("Y").InnerText.Split('.')[0]);

                    listOfX.Add(utmX);
                    listOfY.Add(utmY);
                }
            }

            minUtmX = listOfX.Min();
            minUtmY = listOfY.Min();
            maxUtmX = listOfX.Max();
            maxUtmY = listOfY.Max();
        }

        public void Scale(int utmX, int utmY, out int x, out int y)
        {
            int b = 900; //canvas size

            x = ((utmX - minUtmX) * b) / (maxUtmX - minUtmX) + 20;
            y = ((utmY - minUtmY) * b) / (maxUtmY - minUtmY) + 20;
        }
    }
}
