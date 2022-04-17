using PredmetniZadatak_1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;

namespace PredmetniZadatak_1.Dots
{
    public class DotFormater : DotScaler
    {
        private XmlLoader xmlLoader = new XmlLoader("Geographic.xml");
        private bool[,] dotsMatrix = new bool[320, 320];
        private List<DotModel> dotModels = new List<DotModel>();

        public List<DotModel> DotModels { get => dotModels; set => dotModels = value; }

        public DotFormater() : base() { }

        public List<Ellipse> AddDots(out List<int> placeXList, out List<int> placeYList)
        {
            placeXList = new List<int>();
            placeYList = new List<int>();
            List<Ellipse> ellipses = new List<Ellipse>();

            List<string> enititisAddrs = new List<string>(3)
                { "/NetworkModel/Substations/SubstationEntity", "/NetworkModel/Nodes/NodeEntity", "/NetworkModel/Switches/SwitchEntity" };

            bool isSwitch = false;
            XmlNodeList nodes = null;

            foreach (string entity in enititisAddrs)
            {
                if (entity.Contains("Switch"))
                {
                    nodes = xmlLoader.ReadXml(entity, true);
                    isSwitch = true;
                }
                else
                {
                    nodes = xmlLoader.ReadXml(entity);
                }

                foreach (XmlNode node in nodes)
                {
                    Console.WriteLine(node);

                    string id = node.SelectSingleNode("Id").InnerText;
                    string name = node.SelectSingleNode("Name").InnerText;
                    string status = "";

                    if (isSwitch)
                        status = node.SelectSingleNode("Status").InnerText;

                    ToolTip toolTipText = new ToolTip();
                    if (entity.Contains("Sub"))
                        toolTipText.Content = string.Format("\t\tSubstation\n\tID: {0}\n\tName: {1}", id, name);
                    else if (entity.Contains("Nodes"))
                        toolTipText.Content = string.Format("\t\tNode\n\tID: {0}\n\tName: {1}", id, name);
                    else if (isSwitch)
                        toolTipText.Content = string.Format("\t\tSwitch\n\tID: {0}\n\tName: {1}\n\tStatus: {2}", id, name, status);


                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = 2;
                    ellipse.Height = 2;
                    ellipse.Fill = Brushes.Black;
                    ellipse.ToolTip = toolTipText;
                    ellipse.Name = "id_" + id;


                    int utmX = Int32.Parse(node.SelectSingleNode("X").InnerText.Split('.')[0]);
                    int utmY = Int32.Parse(node.SelectSingleNode("Y").InnerText.Split('.')[0]);
                    int x, y = 0;

                    this.Scale(utmX, utmY, out x, out y);

                    FindFreeSpot(x, y, out int placeX, out int placeY);
                    placeX *= 3;
                    placeY *= 3;

                    ellipses.Add(ellipse);
                    placeXList.Add(placeX);
                    placeYList.Add(placeY);

                    dotModels.Add(new DotModel(Int64.Parse(id), placeX, placeY));
                }
            }
            return ellipses;
        }

        private void FindFreeSpot(int decimalX, int decimalY, out int placeX, out int placeY)
        {
            while (true)
            {
                placeX = (decimalX / 3);
                placeY = (decimalY / 3);

                if (dotsMatrix[placeX, placeY] == false) //trazi se kordinata da ima 3x3 slobodan prostor za smestanje tacke
                {
                    dotsMatrix[placeX, placeY] = true;
                    break;
                }

                while (true)//i >= 8 * siprlCount)
                {
                    if (placeY + 1 < 300)
                        placeY++;
                    if (dotsMatrix[placeX, placeY] == false)
                    {
                        dotsMatrix[placeX, placeY] = true;
                        break;
                    }
                    if (placeX + 1 < 300)
                        placeX++;
                    if (dotsMatrix[placeX, placeY] == false)
                    {
                        dotsMatrix[placeX, placeY] = true;
                        break;
                    }
                    decimalY--;
                    if (dotsMatrix[placeX, placeY] == false)
                    {
                        dotsMatrix[placeX, placeY] = true;
                        break;
                    }
                    decimalY--;
                    if (dotsMatrix[placeX, placeY] == false)
                    {
                        dotsMatrix[placeX, placeY] = true;
                        break;
                    }
                    decimalX--;
                    if (dotsMatrix[placeX, placeY] == false)
                    {
                        dotsMatrix[placeX, placeY] = true;
                        break;
                    }
                    decimalX--;
                    if (dotsMatrix[placeX, placeY] == false)
                    {
                        dotsMatrix[placeX, placeY] = true;
                        break;
                    }
                    if (placeY + 1 < 300)
                        placeY++;
                    if (dotsMatrix[placeX, placeY] == false)
                    {
                        dotsMatrix[placeX, placeY] = true;
                        break;
                    }
                    if (placeY + 1 < 300)
                        placeY++;
                    if (dotsMatrix[placeX, placeY] == false)
                    {
                        dotsMatrix[placeX, placeY] = true;
                        break;
                    }
                    if (placeX + 1 < 300)
                        placeX++;

                }
            }
        }
    }
}
