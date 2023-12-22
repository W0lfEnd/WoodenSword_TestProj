using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor;
using UnityEngine;

//TODO refactor
public class RepeatableObject : MonoBehaviour
{
  #region Enum SpawnDirection
  public enum SpawnDirection
  {
    UP,
    DOWN
  }
  #endregion

  #region Serialize Fields
  [SerializeField] private GameObject     prefab              = null;
  [SerializeField] private Transform      root                = null;
  [SerializeField] private Vector3        size                = Vector3.one;
  [SerializeField] private Transform      upperLimitTransform = null;
  [SerializeField] private Transform      lowerLimitTransform = null;
  [SerializeField] private SpawnDirection directionToSpawn    = SpawnDirection.UP;
  #endregion

  #region Private Fields
  private MonoBehaviourPool<Transform> pool;
  private Dictionary<int, Transform>   chunks = new Dictionary<int, Transform>();
  #endregion


  #region Unity Methods
  private void Awake()
  {
    pool = new MonoBehaviourPool<Transform>( prefab.transform, root );
  }
  #endregion

  #region Public Methods
  public void SetRangeByIdx( int startInclusive, int finishExclusive )
  {
    // Debug.Log( $"{nameof( SetRangeByIdx )}: startInclusive = {startInclusive}, finishExclusive = {finishExclusive}" );
    if ( startInclusive > finishExclusive )
      ( startInclusive, finishExclusive ) = ( finishExclusive, startInclusive );//swap values

    ReleaseExtraObjects( startInclusive, finishExclusive );

    for ( int i = startInclusive; i < finishExclusive; i++ )
    {
      if( chunks.ContainsKey( i ))
        continue;

      Vector3 localPos = IdxToLocalPosition( i );
      if ( !IsLocalPosInBounds( localPos ) )
        continue;

      Transform obj = pool.Get();
      chunks[i] = obj;
      obj.transform.localPosition = localPos;
    }
  }

  public void SetRangeByPosition( Vector3 start, Vector3 finish )
  {
    // Debug.Log( $"{nameof( SetRangeByPosition )}: start = {start}, finish = {finish}" );
    int startIdx  = LocalPositionToIdx( start );
    int finishIdx = LocalPositionToIdx( finish );

    SetRangeByIdx( startIdx, finishIdx );
  }

  public void Clear()
  {
    foreach ( int key in chunks.Keys.ToArray() )
      RemoveObjWithKey( key );
  }
  #endregion

  #region Private Methods
  private void ReleaseExtraObjects( int startInclusive, int finishExclusive )
  {
    int[] keys = chunks.Keys.ToArray();
    for ( int i = 0; i < keys.Length; i++ )
    {
      int dictKey = keys[i];
      if ( !IsIdxInBounds( dictKey ) || dictKey < startInclusive || dictKey >= finishExclusive )
      {
        pool.Release( chunks[dictKey] );
        chunks.Remove( dictKey );
      }
    }
  }

  private void RemoveObjWithKey( int key )
  {
    Transform obj = chunks[key];
    pool.Release( obj );
    chunks.Remove( key );
  }

  private Vector3 SpawnDirectionToVector3( SpawnDirection spawnDirection )
  {
    return spawnDirection switch
    {
      SpawnDirection.UP      => transform.up
    , SpawnDirection.DOWN    => -transform.up

    , _ => throw new InvalidEnumArgumentException( spawnDirection.ToString() )
    };
  }

  private bool IsLocalPosInBounds( Vector3 localPos )
  {
    Vector3 pos = root.InverseTransformPoint( localPos );
    if ( upperLimitTransform != null && pos.y > upperLimitTransform.position.y )
      return false;

    if ( lowerLimitTransform != null && pos.y < lowerLimitTransform.position.y )
      return false;

    return true;
  }

  private bool IsIdxInBounds( int idx )
  {
    return IsLocalPosInBounds( IdxToLocalPosition( idx ) );
  }

  private Vector3 IdxToLocalPosition( int idx )
  {
    return ( idx * 2 + 1 ) * TrimPosBySpawnDirection( size );
  }

  private int LocalPositionToIdx( Vector3 localPosition )
  {
    Vector3 trimmedSize     = TrimPosBySpawnDirection( size );
    Vector3 trimmedLocalPos = TrimPosBySpawnDirection( localPosition );
    float   idx             = GetValueBySpawnDirection( trimmedLocalPos ) / ( 2 * GetValueBySpawnDirection( trimmedSize ) ) - 1;
    return Mathf.RoundToInt( idx );
  }

  private Vector3 TrimPosBySpawnDirection( Vector3 vector )
  {
    Vector3 mask = SpawnDirectionToVector3( directionToSpawn );

    return new Vector3( vector.x * mask.x, vector.y * mask.y, vector.z * mask.z );
  }

  private float GetValueBySpawnDirection( Vector3 localPosition )
  {
    Vector3 trimmedLocalPos = TrimPosBySpawnDirection( localPosition );

    if ( trimmedLocalPos.x != 0 )
      return trimmedLocalPos.x;
    
    if ( trimmedLocalPos.y != 0 )
      return trimmedLocalPos.y;
    
    if ( trimmedLocalPos.z != 0 )
      return trimmedLocalPos.z;

    return 0.0f;
  }

  #if UNITY_EDITOR
  private void OnDrawGizmos()
  {
    foreach ( KeyValuePair<int, Transform> chunk in chunks )
    {
      Vector3 localPos = IdxToLocalPosition( chunk.Key );
      Vector3 pos      = root.InverseTransformPoint( localPos );
      Handles.Label( pos, $"{chunk.Key}: {localPos}" );
    }
  }
  #endif
  #endregion
}
