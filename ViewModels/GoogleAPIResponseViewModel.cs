using Newtonsoft.Json;

namespace _AbsoPickUp.ViewModels
{
    public class GoogleAPIResponseViewModel
    {
        public string Status { get; set; }

        [JsonProperty(PropertyName = "destination_addresses")]
        public string[] DestinationAddresses { get; set; }

        [JsonProperty(PropertyName = "origin_addresses")]
        public string[] OriginAddresses { get; set; }

        public Row[] Rows { get; set; }
    }

    public class Data
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

    public class Element
    {
        public string Status { get; set; }
        public Data Duration { get; set; }
        public Data Distance { get; set; }
    }

    public class Row
    {
        public Element[] Elements { get; set; }
    }

    public class DistanceMatrixResponse
    {
        public string Distance { get; set; }
        public string Duration { get; set; }
        public int Price { get; set; }
        public string DeliveryType { get; set; }
        public string Description { get; set; }
    }

    public class DistanceMatrixAPIResponse
    {
        public string SourceAddress { get; set; }
        public string DestinationAddress { get; set; }
        public Element element { get; set; }
        
    }
}
