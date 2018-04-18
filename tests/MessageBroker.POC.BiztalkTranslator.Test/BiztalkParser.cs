using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NSubstitute;
using Shouldly;
using Xunit;

namespace MessageBroker.POC.BiztalkTranslator.Test
{
    public class BiztalkParserFixture
    {
        private bool _errorWhenProcessing;
        private int _rowsCount = 10;

        private BiztalkParser GetBiztalkParser()
        {
            IRowParser rowParser = Substitute.For<IRowParser>();
            rowParser
                .ParseXmlForRows(Arg.Any<XElement>())
                .Returns(c =>
                {
                    if(_errorWhenProcessing) throw new Exception("Test needed exception");
                    var xElement = c.ArgAt<XElement>(0);
                    var count = int.Parse(xElement.Element("RowsCount").Value);
                    
                    var returns = new List<BiztalkMessage>();
                    
                    for (int i = 0; i < count; i++)
                    {
                        returns.Add(new BiztalkMessage());
                    }

                    return returns;
                });
            var parser = new BiztalkParser(rowParser);
            return parser;
        }

        private string _xmlMessage = null;

        public IEnumerable<BiztalkMessage> Act()
        {
            return GetBiztalkParser().ParseMessage(_xmlMessage);
        }

        public void Arrange_throw_InvalidBiztalkMessageXml_if_xml_provided_to_the_method_is_null_or_empty()
        {
            _xmlMessage = string.Empty;
        }

        public void Arrange_throw_InvalidBiztalkMessageXml_if_xml_has_no_document_root_or_document_root_is_empty()
        {
            _xmlMessage = "<someRoot></someRoot>";
        }

        public void Arrange_throw_ErrorOccuredWhenProccessTranposrtXml_if_there_was_error_through_parse_message()
        {
            _errorWhenProcessing = true;
            _xmlMessage = "<someRoot><metaData></metaData><dane></dane></someRoot>";
        }

        public void Arrange_return_parsed_message_if_xml_is_correct()
        {
            _xmlMessage = $"<someRoot><metaData></metaData><dane></dane><RowsCount>{_rowsCount}</RowsCount></someRoot>";
        }
        public void Assert_return_parsed_message_if_xml_is_correct(IEnumerable<BiztalkMessage> biztalkMessages)
        {
            biztalkMessages.ShouldNotBeNull();
            var count=biztalkMessages.Count();
            count.ShouldBe(_rowsCount);

        }

    }
    public class BiztalkParserTest
    {
        public BiztalkParserFixture _fixture = new BiztalkParserFixture();
        [Fact]
        public void Throw_InvalidBiztalkMessageXml_if_xml_provided_to_the_method_is_null_or_empty()
        {
            _fixture.Arrange_throw_InvalidBiztalkMessageXml_if_xml_provided_to_the_method_is_null_or_empty();
            Should.Throw<InvalidBiztalkMessageXml>(() => { _fixture.Act(); });
        }

        [Fact]
        public void Throw_InvalidBiztalkMessageXml_if_xml_has_no_document_root_or_document_root_is_empty()
        {
            _fixture.Arrange_throw_InvalidBiztalkMessageXml_if_xml_has_no_document_root_or_document_root_is_empty();
            Should.Throw<InvalidBiztalkMessageXml>(() => { _fixture.Act(); });
        }

        [Fact]
        public void Throw_ErrorOccuredWhenProccessTranposrtXml_if_there_was_error_through_parse_message()
        {
            _fixture.Arrange_throw_ErrorOccuredWhenProccessTranposrtXml_if_there_was_error_through_parse_message();
            Should.Throw<ErrorOccuredWhenProccessTranposrtXml>(() => { _fixture.Act(); });
        }

        [Fact]
        public void Return_parsed_message_if_xml_is_correct()
        {
            _fixture.Arrange_return_parsed_message_if_xml_is_correct();
            var result = _fixture.Act();
            _fixture.Assert_return_parsed_message_if_xml_is_correct(result);
        }
    }
}
