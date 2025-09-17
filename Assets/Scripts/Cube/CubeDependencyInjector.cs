using ChainCube.Scripts.Utils;
using UnityEngine;

public class CubeDependencyInjector : MonoBehaviour
{
    [SerializeField] private GameObject _cube;

    public GameObject Cube
    {
        get => _cube;
        set
        {
            if (_cube == value)
                return;

            _cube = value;
            Inject();
        }
    }
    private IDependency<GameObject>[] _dependencies;

    private void Start()
    {
        _dependencies = GetComponents<IDependency<GameObject>>();

        if (_cube != null)
            Inject();
    }

    private void Inject()
    {
        foreach (var dependency in _dependencies)
        {
            dependency.Inject(Cube);
        }
    }
}

