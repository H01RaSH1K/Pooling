using UnityEngine;
using UnityEngine.Pool;

namespace Pool
{
    public class Pool<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private T _objectPrefab;
        [SerializeField] private int _poolCapacity = 10;
        [SerializeField] private int _poolMaxSize = 50;

        private ObjectPool<T> _pool;

        protected virtual void Awake()
        {
            _pool = new ObjectPool<T>(
                createFunc: CreateObject,
                actionOnGet: OnGetObject,
                actionOnRelease: OnReleaseObject,
                actionOnDestroy: OnDestroyObject,
                collectionCheck: true,
                defaultCapacity: _poolCapacity,
                maxSize: _poolMaxSize
            );
        }

        public virtual T GetObject()
        {
            T result = _pool.Get();
            result.transform.SetParent(transform);
            return result;
        }

        public virtual void Release(T poolObject)
        {
            _pool.Release(poolObject);
        }

        protected virtual void OnGetObject(T poolObject)
        {
            poolObject.gameObject.SetActive(true);
        }

        protected virtual T CreateObject()
        {
            return Instantiate(_objectPrefab);
        }

        protected virtual void OnDestroyObject(T poolObject)
        {
            Destroy(poolObject);
        }

        protected virtual void OnReleaseObject(T poolObject)
        {
            poolObject.gameObject.SetActive(false);
        }
    }
}