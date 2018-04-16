using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace MessageBroker.POC.BiztalkTranslator.Test
{
    public class ParseToXmlElementTest
    {
        ParseToXmlElementFixture _fixture;
        public ParseToXmlElementTest()
        {
            _fixture = new ParseToXmlElementFixture();
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
            _fixture.Arange_without_generateDate_will_be_poperly_parse_without_generate_data();
            var result = _fixture.Act();
            _fixture.Assert_without_generateDate_will_be_poperly_parse_without_generate_data(result);
        }

       
    }
}
