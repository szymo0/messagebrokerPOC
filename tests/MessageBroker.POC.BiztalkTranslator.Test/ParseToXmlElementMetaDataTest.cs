using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace MessageBroker.POC.BiztalkTranslator.Test
{
    public class ParseToXmlElementMetaDataTest
    {
        ParseToXmlElementMetaDataFixture _fixture;
        public ParseToXmlElementMetaDataTest()
        {
            _fixture = new ParseToXmlElementMetaDataFixture();
        }

        public XElement Act()
        {
            return _fixture.Act();
        }

        [Fact]
        public void Set_every_property_will_be_in_xelement()
        {
            _fixture.Arange_set_every_property_will_be_in_xelement();
            var result = _fixture.Act();
            _fixture.Assert_set_every_property_will_be_in_xelement(result);
        }

        [Fact]
        public void Metadata_has_not_dateFrom_and_dateTo_every_property_will_be_in_xelement()
        {
            _fixture.Arange_metadata_has_not_dateFrom_and_dateTo_every_property_will_be_in_xelement();
            var result = _fixture.Act();
            _fixture.Assert_metadata_has_not_dateFrom_and_dateTo_every_property_will_be_in_xelement(result);
        }

        [Fact]
        public void Metadata_has_not_id_every_property_will_be_in_xelementt()
        {
            _fixture.Arange_metadata_has_not_id_every_property_will_be_in_xelement();
            var result = _fixture.Act();
            _fixture.Assert_metadata_has_not_id_every_property_will_be_in_xelement(result);
        }
    }
}
