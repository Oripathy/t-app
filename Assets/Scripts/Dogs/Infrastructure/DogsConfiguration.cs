using UnityEngine;

namespace Dogs.Infrastructure
{
    [CreateAssetMenu(fileName = "DogsConfiguration", menuName = "Configurations/DogsConfiguration")]
    public class DogsConfiguration : ScriptableObject
    {
        [SerializeField] private string _uri;
        [SerializeField] private int _pageSize;
        [SerializeField] private int _timeout;

        public string Uri => _uri;
        public int PageSize => _pageSize;
        public int Timeout => _timeout;
    }
}