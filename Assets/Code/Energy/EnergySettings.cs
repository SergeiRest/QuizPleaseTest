using UnityEngine;

namespace QuizPlease.Energy
{
    [CreateAssetMenu(fileName = "EnergySettings", menuName = "Energy Settings")]
    public class EnergySettings : ScriptableObject
    {
        [SerializeField] private int _maxEnergy = 100;

        [SerializeField]
        private float _regenSeconds = 1f;

        public int MaxEnergy => _maxEnergy;

        public float RegenSeconds => _regenSeconds;
    }
}
