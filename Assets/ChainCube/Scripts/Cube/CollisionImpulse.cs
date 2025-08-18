using UnityEngine;

namespace ChainCube.Scripts.Cube
{
    [RequireComponent(typeof(PointsContainer), typeof(Rigidbody), typeof(PointsContainerCollisionDetector))]
    public class CollisionImpulse : MonoBehaviour
    {
        private PointsContainer _pointsContainer;
        private Rigidbody _rigidbody;
        private PointsContainerCollisionDetector _detector;

        private void Start()
        {
            _pointsContainer = GetComponent<PointsContainer>();
            _rigidbody = GetComponent<Rigidbody>();
            _detector = GetComponent<PointsContainerCollisionDetector>();
            Subscribe();
        }
        
        private void OnCollisionStart(PointsContainer col)
        {
            if (col.points == _pointsContainer.points)
                _rigidbody.AddForce(Vector3.up * 10, ForceMode.Impulse);
            
        }

        private void Subscribe()
        {
            _detector.onCollisionStart += OnCollisionStart;
        }

        private void Unsubscribe()
        {
            _detector.onCollisionStart -= OnCollisionStart;
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }
    }
}
