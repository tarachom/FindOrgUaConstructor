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
            string RootFolder = @"D:\VS\Project\FindOrgUa\constructor\";

            string WwwFolder = RootFolder + @"www\";
            string HtmlFolder = RootFolder + @"html\";
            string XmlDataFile = RootFolder + @"xml\data.xml";
            string XsltFile = RootFolder + @"xslt\template.xslt";
            string XsltFileSitemap = RootFolder + @"xslt\sitemap.xslt";

            //Sitemap
            Console.WriteLine("Create Sitemap");

            XslCompiledTransform xsltSitemap = new XslCompiledTransform();
            xsltSitemap.Load(XsltFileSitemap, new XsltSettings(true, true), null);

            XsltArgumentList xsltArgumentListSitemap = new XsltArgumentList();
            xsltArgumentListSitemap.AddParam("DateTime", "", DateTime.Now.ToString("yyyy-MM-dd"));

            FileStream fileStreamSitemap = new FileStream(WwwFolder + "sitemap.xml", FileMode.Create);

            xsltSitemap.Transform(XmlDataFile, xsltArgumentListSitemap, fileStreamSitemap);

            xsltArgumentListSitemap = null;
            xsltSitemap = null;
            fileStreamSitemap = null;

            Console.WriteLine("OK");


            //
            Console.WriteLine("Create pages");

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

                Console.Write(fileNode);

                XsltArgumentList xsltArgumentList = new XsltArgumentList();
                xsltArgumentList.AddParam("Title", "", titleNode);
                xsltArgumentList.AddParam("MenuActive", "", menuactiveNode);
                xsltArgumentList.AddParam("Body", "", File.ReadAllText(HtmlFolder + fileNode));

                FileStream fileStream = new FileStream(WwwFolder + fileNode, FileMode.Create);
                
                xsltGenerator.Transform(XmlDataFile, xsltArgumentList, fileStream);

                fileStream = null;
                xsltArgumentList = null;

                Console.Write(" -> ok\n");
            }

            Console.WriteLine("OK");

        }
    }
}
