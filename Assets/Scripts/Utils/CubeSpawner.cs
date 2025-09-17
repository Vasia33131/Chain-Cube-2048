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
            if (_swipeDetectorObject != null)
            {
                _swipeDetector = _swipeDetectorObject.GetComponent<ISwipeDetector>();
                if (_swipeDetector != null)
                {
                    Subscribe();
                }
            }

            _cubeDependencies = FindObjectsOfType<CubeDependencyInjector>();
        }

        private void Subscribe()
        {
            if (_swipeDetector != null)
            {
                _swipeDetector.onSwipeEnd += OnSwipeEnd;
            }
        }

        private void Unsubscribe()
        {
            if (_swipeDetector != null)
            {
                _swipeDetector.onSwipeEnd -= OnSwipeEnd;
            }
        }

        private void OnSwipeEnd(Vector2 delta)
        {
            if (_spawnRoutine == null && _cubePrefab != null)
            {
                _spawnRoutine = StartCoroutine(SpawnWithDelay());
            }
        }

        private IEnumerator SpawnWithDelay()
        {
            yield return null;
            yield return new WaitForSeconds(_spawnDelay);

            if (_cubePrefab == null)
            {
                _spawnRoutine = null;
                yield break;
            }

            var instance = Instantiate(_cubePrefab, transform.position, Quaternion.identity);

            if (instance != null)
            {
                InjectCube(instance.gameObject);
            }

            _spawnRoutine = null;
        }

        private void InjectCube(GameObject cube)
        {
            if (cube == null || _cubeDependencies == null)
                return;

            foreach (var dependency in _cubeDependencies)
            {
                if (dependency != null)
                {
                    dependency.Cube = cube;
                }
            }
        }

        private void OnDestroy()
        {
            Unsubscribe();

            // Stop any running coroutines when the object is destroyed
            if (_spawnRoutine != null)
            {
                StopCoroutine(_spawnRoutine);
                _spawnRoutine = null;
            }
        }
    }
}