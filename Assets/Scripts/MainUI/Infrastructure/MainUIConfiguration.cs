using UnityEngine;

namespace MainUI.Infrastructure
{
    [CreateAssetMenu(fileName = "MainUIConfiguration", menuName = "Configurations/MainUIConfiguration")]
    public class MainUIConfiguration : ScriptableObject
    {
        [SerializeField] private TabType _initialTab;
        [SerializeField] private Color _inactiveButtonColor;
        [SerializeField] private Color _activeButtonColor;

        public TabType InitialTab => _initialTab;
        public Color InactiveButtonColor => _inactiveButtonColor;
        public Color ActiveButtonColor => _activeButtonColor;
    }
}