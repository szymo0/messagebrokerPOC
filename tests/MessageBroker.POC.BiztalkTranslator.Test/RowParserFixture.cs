using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MessageBroker.POC.Messages.Messages;
using NSubstitute;
using Shouldly;
using Xunit;

namespace MessageBroker.POC.BiztalkTranslator.Test
{
    public class RowParserFixture
    {
        private IRowParser _rowParser;
        private XElement _document;
        private int _rows = 10;
        private string _destination;
        private DateTime _generateDate;

        public RowParserFixture()
        {
            IMetadataParser metadataParser = Substitute.For<IMetadataParser>();
            metadataParser.ParseToXmlElement(Arg.Any<Metadata>()).Returns(c =>
            {
                var root = new XElement("MetaData");
                root.Add(new XElement("Dest", _destination));
                root.Add(new XElement("DateFrom", DateTime.Now.ToString("s")));
                root.Add(new XElement("DateTo", DateTime.Now.ToString("s")));
                root.Add(new XElement("GenerateDate", _generateDate));
                root.Add(new XElement("RowCount", 2));
                root.Add(new XElement("Source", "0003NPIK"));
                root.Add(new XElement("PackageId", _id));
                return root;
            });
            metadataParser.ParseToXmlElement(Arg.Any<DestinationMetadata>()).Returns(c =>
            {
                var root = new XElement("MetaData");
                root.Add(new XElement("Dest", _destination));
                root.Add(new XElement("GenerateDate", _generateDate.ToString("s")));
                root.Add(new XElement("Source", "0003NPIK"));
                root.Add(new XElement("PackageId", _id));
                return root;
            });
            metadataParser.ParseFromXElementToMetadata(Arg.Any<XElement>()).Returns(c =>
            {
                if (c == null || !c.Args().Any() || (c.ArgAt<XElement>(0) == null))
                    throw new EmptyMetdaDataXmlProvieded();
                Metadata metadata = new Metadata();
                metadata.Destinations = new[] { _destination };
                metadata.CorrelationId = _id;
                metadata.RowsCount = 1;
                metadata.Source = "0003NPIK";
                return metadata;
            });
            metadataParser.ParseFromXElementToDestinationMetadata(Arg.Any<XElement>()).Returns(c =>
            {
                DestinationMetadata metadata = new DestinationMetadata();
                metadata.CorrelationId = _id;
                metadata.Source = "0003NPIK";
                return metadata;
            });
            _rowParser = new BiztalkRowParser(metadataParser);
        }
        public IEnumerable<BiztalkMessage> Act()
        {
            return _rowParser.ParseXmlForRows(_document);
        }

        public bool ActWithThrow()
        {
            bool isException = false;
            Should.Throw<EmptyMetdaDataXmlProvieded>(() =>
           {
               isException = true;
               Act();
           });
            return isException;
        }

        private Guid _id;
        public XElement GenerateMetadata()
        {
            _id = Guid.NewGuid();
            _destination = "0003NPIK";
            _generateDate = DateTime.Now;
            var root = new XElement("MetaData");
            root.Add(new XElement("Dest", _destination));
            root.Add(new XElement("DateFrom", DateTime.Now.ToString("s")));
            root.Add(new XElement("DateTo", DateTime.Now.ToString("s")));
            root.Add(new XElement("GenerateDate", _generateDate));
            root.Add(new XElement("RowCount", 2));
            root.Add(new XElement("Source", "0003NPIK"));
            root.Add(new XElement("PackageId", _id));
            return root;
        }

        public List<XElement> GenerateRows()
        {
            List<XElement> elements = new List<XElement>();
            for (int i = 0; i < _rows; i++)
            {
                elements.Add(new XElement("Row", $"Some data {i}"));
            }

            return elements;
        }

        public void Arrange_should_return_as_many_messages_as_rows_in_xml()
        {
            _document = new XElement("Root");
            _document.Add(GenerateMetadata());
            foreach (var row in GenerateRows())
            {
                _document.Add(row);
            }
        }

        public void Assert_should_return_as_many_messages_as_rows_in_xml(IEnumerable<BiztalkMessage> messages)
        {
            messages.ShouldNotBeNull();
            var count = messages.Count();
            count.ShouldBe(_rows);
        }

        public void Arrange_throw_exception_if_no_metadata_in_xml()
        {
            _document = new XElement("Root");
            _document.Add(new XElement("NoMetadata"));

        }

        public void Assert_throw_exception_if_no_metadata_in_xml(bool wasThrow)
        {
            wasThrow.ShouldBe(true);
        }

        public void Arrange_should_return_messages_be_correct()
        {
            _document = new XElement("Root");
            _document.Add(GenerateMetadata());
            foreach (var row in GenerateRows())
            {
                _document.Add(row);
            }
        }

        public void Assert_should_return_messages_be_correct(IEnumerable<BiztalkMessage> messages)
        {
            messages.ShouldNotBeNull();
            foreach (var biztalkMessage in messages)
            {
                biztalkMessage.CorrelationId.ShouldBe(_id);
                biztalkMessage.Data.ShouldNotBeNull();
                biztalkMessage.Destination.ShouldBe(_destination);
                biztalkMessage.GenerateTime.ShouldNotBeNull();
            }

        }

        public void Arrange_should_return_no_rows_if_no_in_xml()
        {
            _document = new XElement("Root");
            _document.Add(GenerateMetadata());
        }

        public void Assert_should_return_no_rows_if_no_in_xml(IEnumerable<BiztalkMessage> messages)
        {
            messages.ShouldNotBeNull();
            messages.Any().ShouldBeFalse();
        }
    }

    public class RowParserTest
    {

        RowParserFixture _fixture = new RowParserFixture();

        public IEnumerable<BiztalkMessage> Act()
        {
            return _fixture.Act();
        }
        [Fact]
        public void Should_return_as_many_messages_as_rows_in_xml()
        {
            _fixture.Arrange_should_return_as_many_messages_as_rows_in_xml();
            var result = Act();
            _fixture.Assert_should_return_as_many_messages_as_rows_in_xml(result);
        }
        [Fact]
        public void Should_return_messages_be_correct()
        {
            _fixture.Arrange_should_return_messages_be_correct();
            var result = Act();
            _fixture.Assert_should_return_messages_be_correct(result);
        }

        [Fact]
        public void Should_not_return_any_rows_if_no_rows_in_xlm()
        {
            _fixture.Arrange_should_return_no_rows_if_no_in_xml();
            var result = Act();
            _fixture.Assert_should_return_no_rows_if_no_in_xml(result);
        }
        [Fact]
        public void Should_throw_exception_if_no_metadata_in_xml()
        {
            _fixture.Arrange_throw_exception_if_no_metadata_in_xml();
            Should.Throw<EmptyMetdaDataXmlProvieded>(() => Act());
        }

        private bool ActWithThrow()
        {
            return _fixture.ActWithThrow();
        }
    }
}
