using UnityEngine;
using UnityEngine.Pool;

public abstract class ObstacleBase : MonoBehaviour
{
    private ObjectPool<ObstacleBase> _pool;

    public void SetPool(ObjectPool<ObstacleBase> pool)
    {
        _pool = pool;
    }

    public virtual void Release()
    {
        _pool.Release(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Mouse>(out Mouse mouse))
            OnMouseCollided(mouse);
    }

    protected abstract void OnMouseCollided(Mouse mouse);
}