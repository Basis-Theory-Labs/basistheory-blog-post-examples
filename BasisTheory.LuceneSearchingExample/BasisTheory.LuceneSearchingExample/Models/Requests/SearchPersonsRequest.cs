using Newtonsoft.Json;

namespace BasisTheory.LuceneSearchingExample.Models.Requests;

public class SearchPersonsRequest
{
    [JsonProperty("query")]
    public string Query { get; set; }
}