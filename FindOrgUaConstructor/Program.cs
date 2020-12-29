using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using System.Xml.XPath;
using System.Xml.Xsl;

namespace FindOrgUaConstructor
{
    class Program
    {
        static void Main(string[] args)
        {
            string RootFolder = @"E:\Project\FindOrgUaCloneVS\FindOrgUa\constructor\";

            string WwwFolder = RootFolder + @"www\";
            string HtmlFolder = RootFolder + @"html\";
            string XmlDataFile = RootFolder + @"xml\data.xml";
            string XsltFile = RootFolder + @"xslt\template.xslt";

            XslCompiledTransform xsltGenerator = new XslCompiledTransform();
            xsltGenerator.Load(XsltFile, new XsltSettings(true, true), null);

            XPathDocument xPathDoc = new XPathDocument(XmlDataFile);
            XPathNavigator xPathDocNavigator = xPathDoc.CreateNavigator();

            XPathNodeIterator PageNodes = xPathDocNavigator.Select("/root/page");
            while (PageNodes.MoveNext())
            {
                XPathNavigator currentNode = PageNodes.Current;

                string titleNode = currentNode.SelectSingleNode("title").Value;
                string fileNode = currentNode.SelectSingleNode("file").Value;
                string menuactiveNode = currentNode.SelectSingleNode("menuactive").Value;

                Console.WriteLine(fileNode);

                XsltArgumentList xsltArgumentList = new XsltArgumentList();
                xsltArgumentList.AddParam("Title", "", titleNode);
                xsltArgumentList.AddParam("MenuActive", "", menuactiveNode);
                xsltArgumentList.AddParam("Body", "", File.ReadAllText(HtmlFolder + fileNode));

                FileStream fileStream = new FileStream(WwwFolder + fileNode, FileMode.Create);
                
                xsltGenerator.Transform(XmlDataFile, xsltArgumentList, fileStream);
            }
        }
    }
}
