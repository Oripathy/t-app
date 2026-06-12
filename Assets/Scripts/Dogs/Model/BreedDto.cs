namespace Dogs.Model
{
    public readonly struct BreedDto
    {
        public string Id { get; }
        public string Name { get; }

        public BreedDto(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}