﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MessageBroker.POC.BiztalkTranslator
{
    public class MetadataParser : IMetadataParser
    {
        public Metadata ParseFromXElementToMetadata(XElement message)
        {
            if (message == null || message.IsEmpty)
                throw new EmptyMetdaDataXmlProvieded();
            if (message.Name.LocalName.ToUpper() != "METADATA")
                throw new DiffrentThenMetadataNodeWasProvidedFound(message);

            Metadata metaData = new Metadata();
            metaData.Destinations = GetDestionations(message.Element("Dest"));
            metaData.GenerateDate = ParseDate(message.Element("GenerateDate"));
            metaData.AttachmentId = ParseString(message.Element("Attachment"));
            metaData.BussinesId = ParseString(message.Element("BussinesId"));
            metaData.ConfirmMethodName = ParseString(message.Element("ConfirmStoredProcedure"));
            metaData.InsertMethodName = ParseString(message.Element("InsertStoredProcedure"));
            metaData.Source = message.Element("Source")?.Value ?? "0666NPIK";
            metaData.CorrelationId = Guid.NewGuid();

            return metaData;

        }

        private string ParseString(XElement element)
        {
            if (element == null || element.IsEmpty)
                return null;

            return element.Value;
        }

        //public XElement ParseToXmlElement(Metadata metadata)
        //{
        //    XElement element = new XElement("MetaData");
        //    element.Add(new XElement("Dest", string.Join(";", metadata.Destinations)));
        //    if(metadata.GenerateDate.HasValue)
        //        element.Add(new XElement("GenerateDate", metadata.GenerateDate));
        //    if(metadata.CorrelationId!=Guid.Empty)
        //        element.Add(new XElement("CorrelationId"));
        //    if(metadata.AttachmentId)
        //    if(!string.IsNullOrEmpty(metadata.Source))
        //        element.Add(new XElement("Source", metadata.Source));
        //    if(!Guid.Empty.Equals(metadata.CorrelationId))
        //        element.Add(new XElement("PackageId", metadata.CorrelationId));

        //    return element;
        //}

        public DestinationMetadata ParseFromXElementToDestinationMetadata(XElement message)
        {
            if (message == null || message.IsEmpty)
                throw new EmptyMetdaDataXmlProvieded();
            if (message.Name.LocalName.ToUpper() != "DESTMETADATA")
                throw new DiffrentThenMetadataNodeWasProvidedFound(message);

            DestinationMetadata metaData = new DestinationMetadata();
            metaData.Destination = message.Element("Dest").Value;
            metaData.GenerateDate = ParseDate(message.Element("GenerateDate"));
            metaData.InsertMethodName = ParseString(message.Element("InsertMethodName"));
            metaData.Source = message.Element("Source")?.Value;
            metaData.CorrelationId = Guid.Parse(message.Element("PackageId").Value);
            metaData.BussinesId = message.Element("BussinesId").Value;

            return metaData;
        }

        public XElement ParseToXmlElement(DestinationMetadata metadata)
        {
            XElement element = new XElement("MetaData");
            element.Add(new XElement( "Dest", metadata.Destination));
            if(metadata.GenerateDate.HasValue)
                element.Add(new XElement("GenerateDate", metadata.GenerateDate));
            if(!string.IsNullOrEmpty(metadata.InsertMethodName))
                element.Add(new XElement("InsertMethodName",metadata.InsertMethodName));
            element.Add(new XElement("Source", metadata.Source));
            element.Add(new XElement("PackageId", metadata.CorrelationId));
            element.Add(new XElement("BussinesId", metadata.BussinesId));

            return element;
        }

        private DateTime? ParseDate(XElement element)
        {
            if (element == null || element.IsEmpty)
                return new DateTime?();
            DateTime dt;
            if (DateTime.TryParse(element.Value, out dt))
                return dt;
            return new DateTime?();
        }

        private Guid ParseCorellationId(XElement element)
        {
            if (element == null || element.IsEmpty)
                return Guid.NewGuid();
            Guid id;
            if (Guid.TryParse(element.Value, out id))
                return id;
            return Guid.NewGuid();
        }

        private int ParseInt(XElement element)
        {
            if (element == null || element.IsEmpty)
                return 0;
            int tmp;
            if (int.TryParse(element.Value, out tmp))
                return tmp;
            return 0;
        }


        public IEnumerable<string> GetDestionations(XElement nodeValue)
        {
            if (nodeValue == null || string.IsNullOrEmpty(nodeValue.Value) ||
                "ALLNPIK".Equals(nodeValue.Value.ToUpper()))
                return GenerateForAll();

            List<string> destinations = new List<string>();
            foreach (var destination in nodeValue.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                destinations.Add(destination);
            }

            return destinations;

        }

        private static List<string> _allNpikDest;
        public IEnumerable<string> GenerateForAll()
        {
            if (_allNpikDest == null)
            {
                _allNpikDest = new List<string>();
                _allNpikDest.AddRange(new []{"0002NPIK", "0004NPIK" , "0005NPIK"});
                //for (int i = 0; i < 400; i++)
                //{
                //    _allNpikDest.Add($"{i:0000}NPIK");
                //}
            }

            return _allNpikDest;
        }

    }

    public interface IMetadataParser
    {

        Metadata ParseFromXElementToMetadata(XElement parsedMessage);
        DestinationMetadata ParseFromXElementToDestinationMetadata(XElement parsedMessage);
        //XElement ParseToXmlElement(Metadata metadata);
        XElement ParseToXmlElement(DestinationMetadata metadata);
    }


    public class NoMetadataFound : Exception
    {
        public XElement ParsedMessage { get; }

        public NoMetadataFound(XElement parsedMessage)
            : base("No metada in message found")
        {
            ParsedMessage = parsedMessage;
        }
    }

    public class DiffrentThenMetadataNodeWasProvidedFound : Exception
    {
        public XElement ParsedMessage { get; }

        public DiffrentThenMetadataNodeWasProvidedFound(XElement parsedMessage)
            : base($"proiveded xml element is not metadata xml : {parsedMessage}")
        {
            ParsedMessage = parsedMessage;
        }
    }

    public class EmptyMetdaDataXmlProvieded : Exception
    {
        public EmptyMetdaDataXmlProvieded()
            : base("Provided metadata xml was empty")
        {

        }
    }
}