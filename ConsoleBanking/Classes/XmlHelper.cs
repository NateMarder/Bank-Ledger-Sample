

using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ConsoleBanking.Classes
{

    public class XmlHelper
    {

        public FileStream stream => new FileStream("../../Shared/SharedResources.xml", FileMode.CreateNew);

        private XDocument _document;
        public XDocument XDocument 
            => _document ?? ( _document = XDocument.Load( "../../Shared/SharedResources.xml" ) );
        

        public string GetSignInUrl()
        {
            return  XDocument.Descendants().Select( des => des.FirstNode ).FirstOrDefault().ToString();
        }

    } 

}

