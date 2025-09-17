namespace ChainCube.Scripts.Utils
{
    public interface IDependency<T> where T : class
    {
        void Inject(T dependency);
    }
}
