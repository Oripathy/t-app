using UnityEngine;

namespace MainUI.Infrastructure
{
    [CreateAssetMenu(fileName = "MainUIConfiguration", menuName = "Configurations/MainUIConfiguration")]
    public class MainUIConfiguration : ScriptableObject
    {
        [SerializeField] private TabType _initialTab;

        public TabType InitialTab => _initialTab;
    }
}