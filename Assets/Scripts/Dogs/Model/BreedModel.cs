using System;

namespace Dogs.Model
{
    public class BreedModel
    {
        public string Id { get; }
        public string Name { get; }
        public BreedInfo Info { get; private set; }

        public event Action<BreedModel> BreedInfoAdded;

        public BreedModel(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddInfo(string description)
        {
            if (Info != null)
            {
                return;
            }
            
            Info = new BreedInfo(description);
            BreedInfoAdded?.Invoke(this);
        }
    }
}