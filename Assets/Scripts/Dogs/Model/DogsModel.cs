using System;
using System.Collections.Generic;

namespace Dogs.Model
{
    public class DogsModel
    {
        private readonly Dictionary<string, BreedModel> _cachedBreeds = new Dictionary<string, BreedModel>();
        
        public IEnumerable<BreedModel> Breeds => _cachedBreeds.Values;

        public void AddBreeds(List<BreedDto> breedsDto)
        {
            foreach (var dto in breedsDto)
            {
                _cachedBreeds.TryAdd(dto.Id, new BreedModel(dto.Id, dto.Name));
            }
        }

        public bool TryGetBreed(string id, out BreedModel breed)
        {
            return _cachedBreeds.TryGetValue(id, out breed);
        }
    }
}