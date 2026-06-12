using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Dogs.Presentation
{
    public class BreedButton : MonoBehaviour, IPoolable<IMemoryPool>
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _breedText;

        public Button Button => _button;
        public string Id { get; private set; }

        public void Initialize(string breed, string id)
        {
            _breedText.text = breed;
            Id = id;
        }
        
        public void OnDespawned()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void OnSpawned(IMemoryPool pool)
        {
            
        }

        public class Pool : MonoMemoryPool<BreedButton>
        {
        }
    }
}