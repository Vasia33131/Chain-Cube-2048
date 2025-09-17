using System;
using UnityEngine;

namespace ChainCube.Scripts.Cube
{
    public class PointsContainer : MonoBehaviour
    {
        [SerializeField] protected long _points;

        public long points
        {
            get => _points;
            set
            {
                if (_points == value)
                    return;

                _points = value;
                onPointsChanged?.Invoke(_points);
            }
        }

        public event Action<long> onPointsChanged;
    }
}
