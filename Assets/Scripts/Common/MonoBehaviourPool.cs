using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;


public class MonoBehaviourPool<T> where T : Component
{
  private ObjectPool<T> pool;

  private T prefab;
  private Transform rootToSpawn;


  public MonoBehaviourPool( T prefab, Transform rootToSpawn = null, int minCapacity = 10, int maxCapacity = 1000 )
  {
    this.prefab = prefab;
    this.rootToSpawn = rootToSpawn;
    pool = new ObjectPool<T>( Create, OnGetAction, OnReleaseAction, OnDestroyAction, defaultCapacity: minCapacity, maxSize: maxCapacity );
  }

  public T Get()
  {
    return pool.Get();
  }

  public void Release( T obj )
  {
    pool.Release( obj );
  }
  
  private T Create()
  {
    return Object.Instantiate( prefab, rootToSpawn );
  }

  private void OnGetAction( T obj )
  {
    obj.gameObject.SetActive( true );
  }

  private void OnReleaseAction( T obj )
  {
    obj.gameObject.SetActive( false );
  }

  private void OnDestroyAction( T obj )
  {
    Object.Destroy( obj.gameObject );
  }
}
