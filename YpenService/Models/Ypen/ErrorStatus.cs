using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace YpenService.Models.Ypen
{

    [XmlRoot(ElementName = "Exception")]
    public class Error
    {

        [XmlElement(ElementName = "ExceptionText")]
        public string ExceptionText { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "exceptionCode")]
        public string ExceptionCode { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "locator")]
        public string Locator { get; set; } = string.Empty;

        [XmlText]
        public string Text { get; set; } = string.Empty;
    }

    [XmlRoot(ElementName = "ExceptionReport")]
    public class ErrorStatus
    {

        [XmlElement(ElementName = "Exception")]
        public Error Error { get; set; } = new Error();

        [XmlAttribute(AttributeName = "xs")]
        public string Xs { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "ows")]
        public string Ows { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "xsi")]
        public string Xsi { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "schemaLocation")]
        public string SchemaLocation { get; set; } = string.Empty;

        [XmlText]
        public string Text { get; set; } = string.Empty;
    } 

    
}
