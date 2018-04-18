using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace MessageBroker.POC.BiztalkTranslator.Test
{
    public class ParseFromXElementToMetadataTest
    {
        ParseFromXElementToMetadataFixture _fixture;
        public ParseFromXElementToMetadataTest()
        {
            _fixture = new ParseFromXElementToMetadataFixture();
        }

        public Metadata Act()
        {
            return _fixture.Act();
        }

        [Fact]
        public void Create_full_set_metada_when_xml_was_created()
        {
            _fixture.Arrange_create_full_set_metada_when_xml_was_created();
            var result = _fixture.Act();
            _fixture.Assert_create_full_set_metada_when_xml_was_created(result);
        }

        [Fact]
        public void Create_metadata_set_without_dateFrom_and_dateTo_when_xml_was_created()
        {
            _fixture.Arrange_create_metadata_set_without_dateFrom_and_dateTo_when_xml_was_created();
            var result = _fixture.Act();
            _fixture.Assert_create_metadata_set_without_dateFrom_and_dateTo_when_xml_was_created(result);
        }

        [Fact]
        public void Create_metadata_set_without_row_when_xml_was_created()
        {
            _fixture.Arrange_create_metadata_set_without_row_when_xml_was_created();
            var result = _fixture.Act();
            _fixture.Assert_create_metadata_set_without_row_when_xml_was_created(result);
        }
        [Fact]
        public void Create_metadata_set_without_id_when_xml_was_created()
        {
            _fixture.Arrange_create_metadata_set_without_id_when_xml_was_created();
            var result = _fixture.Act();
            _fixture.Assert_create_metadata_set_without_id_when_xml_was_created(result);
        }
    }
}
