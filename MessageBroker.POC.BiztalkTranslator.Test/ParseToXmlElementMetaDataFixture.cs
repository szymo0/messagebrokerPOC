using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Shouldly;

namespace MessageBroker.POC.BiztalkTranslator.Test
{

    public class ParseToXmlElementMetaDataFixture
    {
        MetadataParser _metadataParser = new MetadataParser();
        private Metadata _metadata = new Metadata();
        private DateTime _dateFrom = new DateTime(2017, 1, 1, 1, 1, 1);
        private DateTime _dateTo = new DateTime(2017, 2, 1, 1, 1, 1);
        private DateTime _generateDateTime = new DateTime(2018, 1, 1, 1, 1, 1);
        private IEnumerable<string> _defaultDestinations;
        private int _rowCounts = 2;
        private string _source = "0001NPIK";
        private Guid _id = Guid.Parse("{727BD862-C1EB-46E9-B6ED-CEA9B17FC774}");
        public ParseToXmlElementMetaDataFixture()
        {
            List<string> defaultDestinations = new List<string>();
            defaultDestinations.Add("0002NPIK");
            defaultDestinations.Add("0003NPIK");
            defaultDestinations.Add("0004NPIK");
            defaultDestinations.Add("0005NPIK");
            _defaultDestinations = defaultDestinations;
        }

        public XElement Act()
        {
            return _metadataParser.ParseToXmlElement(_metadata);
        }

        #region Builder
        public ParseToXmlElementMetaDataFixture SetGenerateDate(DateTime? date)
        {
            _metadata.GenerateDate = date;
            return this;
        }
        public ParseToXmlElementMetaDataFixture SetCorrelationId(Guid id)
        {
            _metadata.CorrelationId = id;
            return this;
        }
        public ParseToXmlElementMetaDataFixture SetDateFrom(DateTime? date)
        {
            _metadata.DateFrom = date;
            return this;
        }
        public ParseToXmlElementMetaDataFixture SetDateTo(DateTime? date)
        {
            _metadata.DateTo = date;
            return this;
        }
        public ParseToXmlElementMetaDataFixture SetDestinations(IEnumerable<string> destinations)
        {
            _metadata.Destinations = destinations;
            return this;
        }
        public ParseToXmlElementMetaDataFixture SetRowsCount(int count)
        {
            _metadata.RowsCount = count;
            return this;
        }
        public ParseToXmlElementMetaDataFixture SetSource(string source)
        {
            _metadata.Source = source;
            return this;
        }
        private ParseToXmlElementMetaDataFixture Clear()
        {
            _metadata = new Metadata();
            return this;
        }

        #endregion



        public void Arange_set_every_property_will_be_in_xelement()
        {
            Clear()
                .SetCorrelationId(_id)
                .SetDateFrom(_dateFrom)
                .SetDateTo(_dateTo)
                .SetDestinations(_defaultDestinations)
                .SetRowsCount(_rowCounts)
                .SetGenerateDate(_generateDateTime)
                .SetSource(_source);
        }
        public void Assert_set_every_property_will_be_in_xelement(XElement element)
        {
            element.ShouldNotBeNull();
            element.Name.LocalName.ShouldBe("MetaData");
            element.Element("Dest").ShouldNotBeNull();
            element.Element("Dest").Value.ShouldBe(string.Join(";", _defaultDestinations));
            element.Element("DateFrom").ShouldNotBeNull();
            element.Element("DateFrom").Value.ShouldBe(_dateFrom.ToString("s"));
            element.Element("DateTo").ShouldNotBeNull();
            element.Element("DateTo").Value.ShouldBe(_dateTo.ToString("s"));
            element.Element("GenerateDate").ShouldNotBeNull();
            element.Element("GenerateDate").Value.ShouldBe(_generateDateTime.ToString("s"));
            element.Element("RowCount").ShouldNotBeNull();
            element.Element("RowCount").Value.ShouldBe(_rowCounts.ToString());
            element.Element("Source").ShouldNotBeNull();
            element.Element("Source").Value.ShouldBe(_source);
            element.Element("PackageId").ShouldNotBeNull();
            element.Element("PackageId").Value.ShouldBe(_id.ToString());
        }

        public void Arange_metadata_has_not_dateFrom_and_dateTo_every_property_will_be_in_xelement()
        {
            Clear()
                .SetCorrelationId(_id)
                .SetDestinations(_defaultDestinations)
                .SetRowsCount(_rowCounts)
                .SetGenerateDate(_generateDateTime)
                .SetSource(_source);
        }
        public void Assert_metadata_has_not_dateFrom_and_dateTo_every_property_will_be_in_xelement(XElement element)
        {
            element.ShouldNotBeNull();
            element.Name.LocalName.ShouldBe("MetaData");
            element.Element("Dest").ShouldNotBeNull();
            element.Element("Dest").Value.ShouldBe(string.Join(";", _defaultDestinations));
            element.Element("DateFrom").ShouldBeNull();
            element.Element("DataTo").ShouldBeNull();
            element.Element("GenerateDate").ShouldNotBeNull();
            element.Element("GenerateDate").Value.ShouldBe(_generateDateTime.ToString("s"));
            element.Element("RowCount").ShouldNotBeNull();
            element.Element("RowCount").Value.ShouldBe(_rowCounts.ToString());
            element.Element("Source").ShouldNotBeNull();
            element.Element("Source").Value.ShouldBe(_source);
            element.Element("PackageId").ShouldNotBeNull();
            element.Element("PackageId").Value.ShouldBe(_id.ToString());
        }

        public void Arange_metadata_has_not_id_every_property_will_be_in_xelement()
        {
            Clear()
                .SetDateFrom(_dateFrom)
                .SetDateTo(_dateTo)
                .SetDestinations(_defaultDestinations)
                .SetRowsCount(_rowCounts)
                .SetGenerateDate(_generateDateTime)
                .SetSource(_source);
        }
        public void Assert_metadata_has_not_id_every_property_will_be_in_xelement(XElement element)
        {
            element.ShouldNotBeNull();
            element.Name.LocalName.ShouldBe("MetaData");
            element.Element("Dest").ShouldNotBeNull();
            element.Element("Dest").Value.ShouldBe(string.Join(";", _defaultDestinations));
            element.Element("DateFrom").ShouldNotBeNull();
            element.Element("DateFrom").Value.ShouldBe(_dateFrom.ToString("s"));
            element.Element("DateTo").ShouldNotBeNull();
            element.Element("DateTo").Value.ShouldBe(_dateTo.ToString("s"));
            element.Element("GenerateDate").ShouldNotBeNull();
            element.Element("GenerateDate").Value.ShouldBe(_generateDateTime.ToString("s"));
            element.Element("RowCount").ShouldNotBeNull();
            element.Element("RowCount").Value.ShouldBe(_rowCounts.ToString());
            element.Element("Source").ShouldNotBeNull();
            element.Element("Source").Value.ShouldBe(_source);
            element.Element("PackageId").ShouldBeNull();
        }

       
    }
}
