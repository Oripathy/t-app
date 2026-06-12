using Newtonsoft.Json;

namespace Dogs.Infrastructure
{
    public class DogsResponse<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
    
    public class Breed
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("attributes")]
        public BreedAttributes Attributes { get; set; }
    }
    
    public class BreedAttributes
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("hypoallergenic")]
        public bool Hypoallergenic { get; set; }

        [JsonProperty("life")]
        public WeightRange Life { get; set; }

        [JsonProperty("male_weight")]
        public WeightRange MaleWeight { get; set; }

        [JsonProperty("female_weight")]
        public WeightRange FemaleWeight { get; set; }
    }
    
    public class WeightRange
    {
        [JsonProperty("min")]
        public int Min { get; set; }

        [JsonProperty("max")]
        public int Max { get; set; }
    }
}