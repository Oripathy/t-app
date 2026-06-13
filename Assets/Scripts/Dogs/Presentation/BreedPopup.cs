using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dogs.Presentation
{
    public class BreedPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _header;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Button _okButton;
        [SerializeField] private LayoutElement _descriptionLayout;
        [SerializeField] private RectTransform _container;
        [SerializeField] private int _maxHeight = 500;

        public void Show(string header, string description)
        { 
            gameObject.SetActive(true);
            _header.text = header;
            _description.text = description;
            _header.ForceMeshUpdate();
            var height = Math.Min(_description.preferredHeight, _maxHeight);
            _descriptionLayout.preferredHeight = height;
            _okButton.onClick.AddListener(Hide);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_container);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _okButton.onClick.RemoveAllListeners();
        }
    }
}