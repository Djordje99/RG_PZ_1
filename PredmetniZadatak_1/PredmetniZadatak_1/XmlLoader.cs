using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PredmetniZadatak_1
{
    public class XmlLoader
    {
        private XmlElement root = null;
        private XmlDocument xmlDoc = null;

        public XmlLoader(string filename)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            root = xmlDoc.DocumentElement;
        }

		public XmlNodeList ReadXml(string nodeAddress, bool haveStatus = false)
        {
			XmlNodeList nodes = root.SelectNodes(nodeAddress);

			return nodes;
		}
    }
}
