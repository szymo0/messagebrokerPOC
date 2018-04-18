using System;
using System.Xml.Linq;
using Shouldly;

namespace MessageBroker.POC.BiztalkTranslator.Test
{
    public class ParseToXmlElementFixture
    {
        MetadataParser _metadataParser = new MetadataParser();
        private DestinationMetadata _metadata = new DestinationMetadata();
        private DateTime _dateFrom = new DateTime(2017, 1, 1, 1, 1, 1);
        private DateTime _dateTo = new DateTime(2017, 2, 1, 1, 1, 1);
        private DateTime _generateDateTime = new DateTime(2018, 1, 1, 1, 1, 1);
        private string _destination = "0005NPIK";
        private int _rowCounts = 2;
        private string _source = "0001NPIK";
        private Guid _id = Guid.Parse("{727BD862-C1EB-46E9-B6ED-CEA9B17FC774}");
        public ParseToXmlElementFixture()
        {
    
        }

        public XElement Act()
        {
            return _metadataParser.ParseToXmlElement(_metadata);
        }

        #region Builder
        public ParseToXmlElementFixture SetGenerateDate(DateTime? date)
        {
            _metadata.GenerateDate = date;
            return this;
        }
        public ParseToXmlElementFixture SetCorrelationId(Guid id)
        {
            _metadata.CorrelationId = id;
            return this;
        }
       public ParseToXmlElementFixture SetSource(string source)
        {
            _metadata.Source = source;
            return this;
        }
        public ParseToXmlElementFixture SetDestination(string destination)
        {
            _metadata.Destination = destination;
            return this;
        }
        private ParseToXmlElementFixture Clear()
        {
            _metadata = new DestinationMetadata();
            return this;
        }

        #endregion



        public void Arange_set_every_property_will_be_in_xelement()
        {
            Clear()
                .SetCorrelationId(_id)
                .SetDestination(_destination)
                .SetGenerateDate(_generateDateTime)
                .SetSource(_source);
        }
        public void Assert_set_every_property_will_be_in_xelement(XElement element)
        {
            element.ShouldNotBeNull();
            element.Name.LocalName.ShouldBe("MetaData");
            element.Element("Dest").ShouldNotBeNull();
            element.Element("Dest").Value.ShouldBe(_destination);
            element.Element("GenerateDate").ShouldNotBeNull();
            element.Element("GenerateDate").Value.ShouldBe(_generateDateTime.ToString("s"));
            element.Element("Source").ShouldNotBeNull();
            element.Element("Source").Value.ShouldBe(_source);
            element.Element("PackageId").ShouldNotBeNull();
            element.Element("PackageId").Value.ShouldBe(_id.ToString());
        }

        public void Arange_without_generateDate_will_be_poperly_parse_without_generate_data()
        {
            Clear()
                .SetCorrelationId(_id)
                .SetDestination(_destination)
                .SetSource(_source);
        }
        public void Assert_without_generateDate_will_be_poperly_parse_without_generate_data(XElement element)
        {
            element.ShouldNotBeNull();
            element.Name.LocalName.ShouldBe("MetaData");
            element.Element("Dest").ShouldNotBeNull();
            element.Element("Dest").Value.ShouldBe(_destination);
            element.Element("GenerateDate").ShouldBeNull();
            element.Element("Source").ShouldNotBeNull();
            element.Element("Source").Value.ShouldBe(_source);
            element.Element("PackageId").ShouldNotBeNull();
            element.Element("PackageId").Value.ShouldBe(_id.ToString());
        }


    }
}
