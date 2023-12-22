using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;


public static class StaticPool<T> where T : MonoBehaviour
{
  private static Dictionary<Type, T> prefabStorage = new Dictionary<Type, T>();

  public static readonly ObjectPool<T> Pool = new ObjectPool<T>( Create, OnGetAction, OnReleaseAction, OnDestroyAction, defaultCapacity: 1000 );


  public static void Init( T prefab )
  {
    prefabStorage[typeof( T )] = prefab;
  }

  [RuntimeInitializeOnLoadMethod]
  public static void clear()
  {
    prefabStorage.Clear();
    Pool.Clear();
  }

  public static bool IsExists => prefabStorage.ContainsKey( typeof( T ) );

  public static bool IsNotExists => !IsExists;

  private static T Create()
  {
    if ( !prefabStorage.TryGetValue( typeof( T ), out T value ) || value == null )
    {
      Debug.LogError( $"Key in {nameof( prefabStorage )} does not exists. You need to use {nameof( Init )} method before" );
      return null;
    }

    return Object.Instantiate( value, Vector3.zero, Quaternion.identity );
  }

  private static void OnGetAction( T obj )
  {
    obj.gameObject.SetActive( true );
  }

  private static void OnReleaseAction( T obj )
  {
    obj.gameObject.SetActive( false );
  }

  private static void OnDestroyAction( T obj )
  {
    Object.Destroy( obj.gameObject );
  }
}
