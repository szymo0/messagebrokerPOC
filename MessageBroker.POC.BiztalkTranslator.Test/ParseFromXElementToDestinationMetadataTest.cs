using Shouldly;
using Xunit;

namespace MessageBroker.POC.BiztalkTranslator.Test
{
    public class ParseFromXElementToDestinationMetadataTest
    {

        private ParseFromXElementToDestinationMetadataFixture _fixture=new ParseFromXElementToDestinationMetadataFixture();
        [Fact]
        public void Should_have_all_properties_set_like_in_xml()
        {
            _fixture.Arrange_should_have_all_properties_set_like_in_xml();
            var result=_fixture.Act();
            _fixture.Assert_should_have_all_properties_set_like_in_xml(result);
        }
        [Fact]
        public void Should_have_all_properties_without_generate_date()
        {
            _fixture.Arrange_should_have_all_properties_without_generate_date();
            var result = _fixture.Act();
            _fixture.Assert_should_have_all_properties_without_generate_date(result);
        }
        [Fact]
        public void Should_throw_EmptyMetdaDataXmlProvieded_when_xml_is_empty()
        {
            _fixture.Arrange_should_throw_EmptyMetdaDataXmlProvieded_when_xml_is_empty();
            Should.Throw<EmptyMetdaDataXmlProvieded>(() => _fixture.Act());
        }
        [Fact]
        public void Should_throw_DiffrentThenMetadataNodeWasProvidedFoundwhen_xml_has_not_destmetadata_row()
        {
            _fixture.Arrange_should_throw_DiffrentThenMetadataNodeWasProvidedFoundwhen_xml_has_not_destmetadata_row();
            Should.Throw<DiffrentThenMetadataNodeWasProvidedFound>(() => _fixture.Act());
        }
    }
}