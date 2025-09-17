using System;
using ChainCube.Scripts.Utils;
using UnityEngine;

namespace ChainCube.Scripts.Handlers
{
    public class ForceYMovementSwipeHandler : MonoBehaviour, IMovableObjectHandler
    {
        [SerializeField]
        private float _forwardForce = 15.0f;

        [SerializeField]
        private float _upwardForce = 5.0f;

        [SerializeField]
        private XMovementSwipeHandler _xMovementHandler;

        private Rigidbody _movableRigidBody;
        private ISwipeDetector _swipeDetector;

        public void Inject(GameObject dependency)
        {
            _movableRigidBody = dependency.GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _swipeDetector = GetComponent<ISwipeDetector>();

            if (_xMovementHandler == null)
            {
                _xMovementHandler = FindObjectOfType<XMovementSwipeHandler>();
            }

            Subscribe();
        }

        private void Subscribe()
        {
            if (_swipeDetector == null)
                return;

            _swipeDetector.onSwipeEnd += OnSwipeEnd;
        }

        private void OnSwipeEnd(Vector2 delta)
        {
            if (_movableRigidBody == null)
                return;

            // Простой бросок вперед
            ThrowCubeForward();

            // Блокируем управление
            if (_xMovementHandler != null)
            {
                _xMovementHandler.LockControl();
            }

            _movableRigidBody = null;
        }

        private void ThrowCubeForward()
        {
            // Убедимся, что rigidbody не кинематический
            _movableRigidBody.isKinematic = false;

            Vector3 force = (Vector3.forward * _forwardForce) + (Vector3.up * _upwardForce);
            _movableRigidBody.AddForce(force, ForceMode.Impulse);
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            if (_swipeDetector == null)
                return;

            _swipeDetector.onSwipeEnd -= OnSwipeEnd;
        }
    }
}