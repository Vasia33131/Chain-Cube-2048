using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChainCube.Scripts.Utils
{
    public class MouseSwipeDetector : MonoBehaviour, ISwipeDetector
    {
        public event Action<Vector2> onSwipeStart;
        public event Action<Vector2> onSwipe;
        public event Action<Vector2> onSwipeEnd;

        private bool _isSwipe;
        private Vector3 _lastPosition = new Vector2();

        private void Update()
        {
            if (!Input.GetMouseButton(0))
            {
                if (_isSwipe)
                {
                    _isSwipe = false;
                    onSwipeEnd?.Invoke(_lastPosition);
                }

                _lastPosition = Input.mousePosition;
                return;
            }

            if (!_isSwipe)
            {
                _isSwipe = true;
                onSwipeStart?.Invoke(Input.mousePosition - _lastPosition);
            }

            onSwipe?.Invoke(Input.mousePosition - _lastPosition);
            _lastPosition = Input.mousePosition;
        }
    }
}

