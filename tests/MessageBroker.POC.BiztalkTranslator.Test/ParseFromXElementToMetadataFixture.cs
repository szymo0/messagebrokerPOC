using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Shouldly;

namespace MessageBroker.POC.BiztalkTranslator.Test
{
    public class ParseFromXElementToMetadataFixture
    {
        MetadataParser _metadataParser = new MetadataParser();
        private Metadata _metadata = new Metadata();
        private XElement _element = null;

        private DateTime _dateFrom = new DateTime(2017, 1, 1, 1, 1, 1);
        private DateTime _dateTo = new DateTime(2017, 2, 1, 1, 1, 1);
        private DateTime _generateDateTime = new DateTime(2018, 1, 1, 1, 1, 1);
        private int _rowCounts = 2;
        private string _source = "0001NPIK";
        private Guid _id = Guid.Parse("{727BD862-C1EB-46E9-B6ED-CEA9B17FC774}");
        private string _destination = "0002NPIK;0003NPIK;0004NPIK;0005NPIK";

        public ParseFromXElementToMetadataFixture()
        {

        }

        public Metadata Act()
        {
            return _metadataParser.ParseFromXElementToMetadata(_element);
        }

        public void Arrange_create_full_set_metada_when_xml_was_created()
        {
            _element = new XElement("Metadata");
            _element.Add(new XElement("Dest", "0002NPIK;0003NPIK;0004NPIK;0005NPIK"));
            _element.Add(new XElement("DateFrom", _dateFrom.ToString("s")));
            _element.Add(new XElement("DateTo", _dateTo.ToString("s")));
            _element.Add(new XElement("GenerateDate", _generateDateTime.ToString("s")));
            _element.Add(new XElement("RowCount", _rowCounts));
            _element.Add(new XElement("Source", _source));
            _element.Add(new XElement("PackageId", _id));
        }
        public void Assert_create_full_set_metada_when_xml_was_created(Metadata metadata)
        {
            metadata.Destinations.ShouldNotBeNull();
            metadata.Destinations.Count().ShouldBe(_destination.Split(new []{';'},StringSplitOptions.RemoveEmptyEntries).Length);
            metadata.GenerateDate.ShouldBe(_generateDateTime);
            metadata.RowsCount.ShouldBe(_rowCounts);
            metadata.Source.ShouldBe(_source);
            metadata.CorrelationId.ShouldBe(_id);
        }

        public void Arrange_create_metadata_set_without_dateFrom_and_dateTo_when_xml_was_created()
        {
            _element = new XElement("Metadata");
            _element.Add(new XElement("Dest", "0002NPIK;0003NPIK;0004NPIK;0005NPIK"));
            _element.Add(new XElement("DateFrom", _dateFrom.ToString("s")));
            _element.Add(new XElement("DateTo", _dateTo.ToString("s")));
            _element.Add(new XElement("GenerateDate", _generateDateTime.ToString("s")));
            _element.Add(new XElement("RowCount", _rowCounts));
            _element.Add(new XElement("Source", _source));
            _element.Add(new XElement("PackageId", _id));
        }
        public void Assert_create_metadata_set_without_dateFrom_and_dateTo_when_xml_was_created(Metadata metadata)
        {
            metadata.Destinations.ShouldNotBeNull();
            metadata.Destinations.Count().ShouldBe(_destination.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Length);
            metadata.GenerateDate.ShouldBe(_generateDateTime);
            metadata.RowsCount.ShouldBe(_rowCounts);
            metadata.Source.ShouldBe(_source);
            metadata.CorrelationId.ShouldBe(_id);
        }

        public void Arrange_create_metadata_set_without_row_when_xml_was_created()
        {
            _element = new XElement("Metadata");
            _element.Add(new XElement("Dest", "0002NPIK;0003NPIK;0004NPIK;0005NPIK"));
            _element.Add(new XElement("DateFrom", _dateFrom.ToString("s")));
            _element.Add(new XElement("DateTo", _dateTo.ToString("s")));
            _element.Add(new XElement("GenerateDate", _generateDateTime.ToString("s")));
            _element.Add(new XElement("Source", _source));
            _element.Add(new XElement("PackageId", _id));
        }
        public void Assert_create_metadata_set_without_row_when_xml_was_created(Metadata metadata)
        {
            metadata.Destinations.ShouldNotBeNull();
            metadata.Destinations.Count().ShouldBe(_destination.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Length);
            metadata.GenerateDate.ShouldBe(_generateDateTime);
            metadata.RowsCount.ShouldBe(0);
            metadata.Source.ShouldBe(_source);
            metadata.CorrelationId.ShouldBe(_id);
        }

        public void Arrange_create_metadata_set_without_id_when_xml_was_created()
        {
            _element = new XElement("Metadata");
            _element.Add(new XElement("Dest", "0002NPIK;0003NPIK;0004NPIK;0005NPIK"));
            _element.Add(new XElement("DateFrom", _dateFrom.ToString("s")));
            _element.Add(new XElement("DateTo", _dateTo.ToString("s")));
            _element.Add(new XElement("GenerateDate", _generateDateTime.ToString("s")));
            _element.Add(new XElement("RowCount", _rowCounts));
            _element.Add(new XElement("Source", _source));
        }
        public void Assert_create_metadata_set_without_id_when_xml_was_created(Metadata metadata)
        {
            metadata.Destinations.ShouldNotBeNull();
            metadata.Destinations.Count().ShouldBe(_destination.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Length);
            metadata.GenerateDate.ShouldBe(_generateDateTime);
            metadata.RowsCount.ShouldBe(_rowCounts);
            metadata.Source.ShouldBe(_source);
            metadata.CorrelationId.ShouldNotBeNull();
            metadata.CorrelationId.ShouldNotBe(_id);
        }
    }
}