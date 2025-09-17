using ChainCube.Scripts.Handlers;
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
        private XMovementSwipeHandler _xMovementHandler;
        private Coroutine _spawnRoutine;
        private bool _canSpawn = true;

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
            _xMovementHandler = FindObjectOfType<XMovementSwipeHandler>();

            // Спавним первый куб сразу
            SpawnCube();
        }

        private void Subscribe()
        {
            if (_swipeDetector != null)
            {
                _swipeDetector.onSwipeEnd += OnSwipeEnd;
            }
        }

        private void OnSwipeEnd(Vector2 delta)
        {
            if (_canSpawn && _spawnRoutine == null)
            {
                _spawnRoutine = StartCoroutine(SpawnWithDelay());
            }
        }

        private IEnumerator SpawnWithDelay()
        {
            _canSpawn = false;
            yield return new WaitForSeconds(_spawnDelay);

            SpawnCube();

            _spawnRoutine = null;
            _canSpawn = true;
        }

        private void SpawnCube()
        {
            if (_cubePrefab == null)
                return;

            // Разблокируем управление для нового куба
            if (_xMovementHandler != null)
            {
                _xMovementHandler.UnlockControl();
            }

            InstantiateAndInject();
        }

        private void InstantiateAndInject()
        {
            var instance = Instantiate(_cubePrefab, transform.position, Quaternion.identity);
            InjectCube(instance.gameObject);
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
            if (_spawnRoutine != null)
            {
                StopCoroutine(_spawnRoutine);
            }
        }

        private void Unsubscribe()
        {
            if (_swipeDetector != null)
            {
                _swipeDetector.onSwipeEnd -= OnSwipeEnd;
            }
        }
    }
}