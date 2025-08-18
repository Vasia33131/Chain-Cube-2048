using System.Collections;
using UnityEngine;

namespace ChainCube.Scripts.Utils
{
    [RequireComponent(typeof(ISwipeDetector))]
    public class CubeSpawner : MonoBehaviour
    {
        [SerializeField] private float _spawnDelay = 0.3f;

        [SerializeField] private GameObject _cubePrefab;

        [SerializeField] private GameObject _swipeDetectorObject;

        private CubeDependencyInjector[] _cubeDependencies;
        
        private ISwipeDetector _swipeDetector;

        private Coroutine _spawnRoutine;
        
        private void Start()
        {
            _swipeDetector = _swipeDetectorObject.GetComponent<ISwipeDetector>();
            _cubeDependencies = FindObjectsOfType<CubeDependencyInjector>();
            Subscribe();
        }

        private void Subscribe()
        {
            _swipeDetector.onSwipeEnd += OnSwipeEnd;
        }

        private void Unsubscribe()
        {
            _swipeDetector.onSwipeEnd -= OnSwipeEnd;
        }

        private void OnSwipeEnd(Vector2 delta)
        {
            if (_spawnRoutine == null)
                _spawnRoutine = StartCoroutine(SpawnWithDelay());
        }

        private IEnumerator SpawnWithDelay()
        {
            yield return null;
            yield return new WaitForSeconds(_spawnDelay);
            var instance = Instantiate(_cubePrefab, transform.position, Quaternion.identity);
            InjectCube(instance.gameObject);
            _spawnRoutine = null;
        }

        private void InjectCube(GameObject cube)
        {
            foreach (var dependency in _cubeDependencies)
            {
                dependency.Cube = cube;
            }
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }
    }
}
