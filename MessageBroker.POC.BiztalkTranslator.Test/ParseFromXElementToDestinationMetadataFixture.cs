using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Shouldly;

namespace MessageBroker.POC.BiztalkTranslator.Test
{
    public class ParseFromXElementToDestinationMetadataFixture
    {
        IMetadataParser _metadataParser=new MetadataParser();
        private XElement _xElement; 
        public DestinationMetadata Act()
        {
            return _metadataParser.ParseFromXElementToDestinationMetadata(_xElement);
        }

        private string _destination = "0004NPIK";
        private DateTime? _generateDate= new DateTime?(new DateTime(2015,1,1,1,1,1));
        private string _source = "0005NPIK";
        private Guid _id = Guid.NewGuid();
        public void Arrange_should_have_all_properties_set_like_in_xml()
        {
            _xElement=new XElement("DestMetadata");
            _xElement.Add(new XElement("Dest",_destination));
            _xElement.Add(new XElement("GenerateDate", _generateDate ));
            _xElement.Add(new XElement("Source",_source));
            _xElement.Add(new XElement("PackageId",_id));
        }

        public void Assert_should_have_all_properties_set_like_in_xml(DestinationMetadata metadata)
        {
            metadata.ShouldNotBeNull();
            metadata.CorrelationId.ShouldBe(_id);
            metadata.Destination.ShouldBe(_destination);
            metadata.GenerateDate.ShouldBe(_generateDate);
            metadata.Source.ShouldBe(_source);
        }

        public void Arrange_should_have_all_properties_without_generate_date()
        {
            _xElement = new XElement("DestMetadata");
            _xElement.Add(new XElement("Dest", _destination));
            _xElement.Add(new XElement("Source", _source));
            _xElement.Add(new XElement("PackageId", _id));
        }
        public void Assert_should_have_all_properties_without_generate_date(DestinationMetadata metadata)
        {
            metadata.ShouldNotBeNull();
            metadata.CorrelationId.ShouldBe(_id);
            metadata.Destination.ShouldBe(_destination);
            metadata.GenerateDate.ShouldBeNull();
            metadata.Source.ShouldBe(_source);
        }

        public void Arrange_should_throw_EmptyMetdaDataXmlProvieded_when_xml_is_empty()
        {
            _xElement = new XElement("DestMetadata");
        }

        public void Arrange_should_throw_DiffrentThenMetadataNodeWasProvidedFoundwhen_xml_has_not_destmetadata_row()
        {
            _xElement = new XElement("ErrorRoor");
            _xElement.Add(new XElement("dadas","dsada"));
        }
    }
}
