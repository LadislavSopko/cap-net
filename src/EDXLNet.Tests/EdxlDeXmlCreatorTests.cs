using System;
using CAPNet.Models;
using EDXLNet.Models;
using Shouldly;

namespace EDXLNet.Tests
{
    public class EdxlDeXmlCreatorTests
    {
        public void CanCreateXElementFromEdxlDistribution()
        {
            var alert1 = new Alert { Identifier = "Alert1" };
            var alert2 = new Alert { Identifier = "Alert2" };
            alert2.Codes.Add("Secret");

            var edxlDistribution = new EdxlDistribution
            {
                SenderId = "victorg@teamnet.ro",
                DistributionId = "ea6fa603-a967-4387-a79a-86cbd9cb5227",
                DateTimeSent = new DateTimeOffset(2015, 3, 28, 12, 23, 0, TimeSpan.FromHours(-2)),
                DateTimeExpires = new DateTimeOffset(2015, 3, 29, 12, 23, 0, TimeSpan.FromHours(-2)),
                DistributionStatus = new DistributionStatus
                {
                    StatusKindDefault = new StatusKindDefault
                    {
                        Value = StatusKindDefaultValues.Test
                    }
                },
                DistributionKind = new DistributionKind
                {
                    DistributionKindDefault = new DistributionKindDefault
                    {
                        Value = DistTypeDefaultValues.Report
                    }
                },
                Content = new Content()
            };
            edxlDistribution.Content.ContentObjects.Add(new ContentObject
            {
                ContentXml = new ContentXml
                {
                    EmbeddedXml = CAPNet.XmlCreator.Create(alert1)
                }
            });
            edxlDistribution.Content.ContentObjects.Add(new ContentObject
            {
                ContentXml = new ContentXml
                {
                    EmbeddedXml = CAPNet.XmlCreator.Create(alert2)
                }
            });
            var element = edxlDistribution.ToXElement();

            element.ToString()
                .ShouldBe(
@"<EDXLDistribution xmlns:ct=""urn:oasis:names:tc:emergency:edxl:ct:1.0"" xmlns=""urn:oasis:names:tc:emergency:EDXL:DE:2.0"">
  <DistributionID>ea6fa603-a967-4387-a79a-86cbd9cb5227</DistributionID>
  <SenderID>victorg@teamnet.ro</SenderID>
  <DateTimeSent>2015-03-28T12:23:00-02:00</DateTimeSent>
  <DateTimeExpires>2015-03-29T12:23:00-02:00</DateTimeExpires>
  <DistributionStatus>
    <StatusKindDefault>
      <ct:ValueListURI>urn:oasis:names:tc:emergency:EDXL:DE:2.0:Defaults:StatusKind</ct:ValueListURI>
      <ct:Value>Test</ct:Value>
    </StatusKindDefault>
  </DistributionStatus>
  <DistributionKind>
    <DistributionKindDefault>
      <ct:ValueListURI>urn:oasis:names:tc:emergency:EDXL:DE:2.0:Defaults:DistributionType</ct:ValueListURI>
      <ct:Value>Report</ct:Value>
    </DistributionKindDefault>
  </DistributionKind>
  <Content>
    <ContentObject>
      <ContentXML>
        <EmbeddedXMLContent>
          <alert xmlns=""urn:oasis:names:tc:emergency:cap:1.2"">
            <identifier>Alert1</identifier>
            <status>Actual</status>
            <msgType>Alert</msgType>
            <scope>Public</scope>
          </alert>
        </EmbeddedXMLContent>
      </ContentXML>
    </ContentObject>
    <ContentObject>
      <ContentXML>
        <EmbeddedXMLContent>
          <alert xmlns=""urn:oasis:names:tc:emergency:cap:1.2"">
            <identifier>Alert2</identifier>
            <status>Actual</status>
            <msgType>Alert</msgType>
            <scope>Public</scope>
            <code>Secret</code>
          </alert>
        </EmbeddedXMLContent>
      </ContentXML>
    </ContentObject>
  </Content>
</EDXLDistribution>");
        }
    }
}
