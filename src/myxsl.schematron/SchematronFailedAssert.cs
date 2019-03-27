using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myxsl.schematron
{
    /*
     <svrl:failed-assert test="book-title-group/book-title" id="a_book-title"
        location="/*:Asset[namespace-uri()='urn:degruyter.com:repo:indexcontainer'][1]/*:AssetFileInfo[namespace-uri()='urn:degruyter.com:repo:indexcontainer'][1]/*:ZipFileStructure[namespace-uri()='urn:degruyter.com:repo:indexcontainer'][1]/*:ZipEntry[namespace-uri()='urn:degruyter.com:repo:indexcontainer'][29]/*:ZipEntryFileInfo[namespace-uri()='urn:degruyter.com:repo:indexcontainer'][1]/*:XmlFileContainer[namespace-uri()='urn:degruyter.com:repo:indexcontainer'][1]/*:XmlPayload[namespace-uri()='urn:degruyter.com:repo:indexcontainer'][1]/book[1]/book-meta[1]">
        <svrl:text>there must be /book/book-meta/book-title-group/book-title</svrl:text>
    </svrl:failed-assert>
     * */
    public class SchematronFailedAssert
    {
        public string Id { get; set; }
        public string Test { get; set; }
        public string Location { get; set; }
        public string Role { get; set; }
        public string Flag { get; set; }
        public List<DiagnosticReference> DiagnosticReferences { get; set; }
        public string Text { get; set; }
        
}


    /*
     <svrl:diagnostic-reference diagnostic="NMTOKEN">
   <svrl:text>string</svrl:text>
</svrl:diagnostic-reference>

     */

    public class DiagnosticReference
    {
        /// <summary>
        /// Local, Unqualified
        /// </summary>
        public string Diagnostic { get; set; }
        public string Text { get; set; }

    }

}
