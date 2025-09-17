using System;
using UnityEngine;

namespace ChainCube.Scripts.Utils
{
    public class TouchSwipeDetector : MonoBehaviour, ISwipeDetector
    {
        public event Action<Vector2> onSwipeStart;
        public event Action<Vector2> onSwipe;
        public event Action<Vector2> onSwipeEnd;

        private bool _isSwipe;
        private Vector2 _lastPosition;

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            // Мышь для тестирования в редакторе
            if (Input.GetMouseButtonDown(0))
            {
                _isSwipe = true;
                _lastPosition = Input.mousePosition;
                onSwipeStart?.Invoke(Vector2.zero);
            }
            else if (Input.GetMouseButton(0) && _isSwipe)
            {
                Vector2 delta = (Vector2)Input.mousePosition - _lastPosition;
                onSwipe?.Invoke(delta * 3f); // Увеличено в 3 раза!
                _lastPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0) && _isSwipe)
            {
                _isSwipe = false;
                Vector2 finalDelta = (Vector2)Input.mousePosition - _lastPosition;
                onSwipeEnd?.Invoke(finalDelta);
            }

            // Тач для мобильных устройств
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _isSwipe = true;
                        _lastPosition = touch.position;
                        onSwipeStart?.Invoke(Vector2.zero);
                        break;

                    case TouchPhase.Moved:
                        if (_isSwipe)
                        {
                            Vector2 delta = touch.position - _lastPosition;
                            onSwipe?.Invoke(delta * 4f); // Увеличено в 4 раза для мобильных!
                            _lastPosition = touch.position;
                        }
                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        if (_isSwipe)
                        {
                            _isSwipe = false;
                            Vector2 finalDelta = touch.position - _lastPosition;
                            onSwipeEnd?.Invoke(finalDelta);
                        }
                        break;
                }
            }
        }
    }
}