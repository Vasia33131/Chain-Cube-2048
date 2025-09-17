using System;
using ChainCube.Scripts.Utils;
using UnityEngine;

namespace ChainCube.Scripts.Handlers
{
    public class XMovementSwipeHandler : MonoBehaviour, IMovableObjectHandler
    {
        [SerializeField]
        private Transform _leftBorder;

        [SerializeField]
        private Transform _rightBorder;

        [SerializeField, Range(1f, 10f)]
        private float _moveSpeed = 8.0f; // Увеличено в 26 раз!

        private GameObject _movableObject;
        private ISwipeDetector _swipeDetector;
        private bool _isControlling = false;
        private Vector2 _touchStartPosition;
        private float _objectStartX;

        public void Inject(GameObject dependency)
        {
            _movableObject = dependency;
        }

        private void Start()
        {
            _swipeDetector = GetComponent<ISwipeDetector>();
            Subscribe();
        }

        private void Subscribe()
        {
            if (_swipeDetector == null)
                return;

            _swipeDetector.onSwipeStart += OnSwipeStart;
            _swipeDetector.onSwipe += OnSwipe;
            _swipeDetector.onSwipeEnd += OnSwipeEnd;
        }

        private void OnSwipeStart(Vector2 delta)
        {
            if (_movableObject == null) return;

            _isControlling = true;
            _touchStartPosition = Input.mousePosition;
            _objectStartX = _movableObject.transform.position.x;
        }

        private void OnSwipe(Vector2 delta)
        {
            if (!_isControlling || _movableObject == null) return;

            // Очень высокая чувствительность!
            float screenWidth = Screen.width;
            float moveAmount = delta.x / screenWidth * _moveSpeed * 5f; // Дополнительный множитель

            float newX = _movableObject.transform.position.x + moveAmount;
            newX = Mathf.Clamp(newX, _leftBorder.position.x, _rightBorder.position.x);

            _movableObject.transform.position = new Vector3(
                newX,
                _movableObject.transform.position.y,
                _movableObject.transform.position.z
            );
        }

        private void OnSwipeEnd(Vector2 delta)
        {
            _isControlling = false;
        }

        public void LockControl()
        {
            _isControlling = false;
            _movableObject = null;
        }

        public void UnlockControl()
        {
            // Разблокировка происходит через Inject
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            if (_swipeDetector == null)
                return;

            _swipeDetector.onSwipeStart -= OnSwipeStart;
            _swipeDetector.onSwipe -= OnSwipe;
            _swipeDetector.onSwipeEnd -= OnSwipeEnd;
        }
    }
}