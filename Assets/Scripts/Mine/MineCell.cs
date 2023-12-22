using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;


public class MineCell : MonoBehaviour
{
  #region Events
  public event Action onBreak = delegate {};
  #endregion

  #region Serialize Fields
  [SerializeField] private MineGem          gemPrefab = null;
  [SerializeField] private MineCellAnimator animator  = null;
  [Space]
  [SerializeField] private Vector3 halfSize;
  #endregion

  #region Private Fields
  private List<MineGem> gems     = new List<MineGem>();
  private bool          isBroken = false;

  #endregion

  #region Properties
  public         Vector3             HalfSize => halfSize;
  private static ObjectPool<MineGem> gemsPool => StaticPool<MineGem>.Pool;

  public bool IsBroken
  {
    get => isBroken;
    private set
    {
      isBroken = value;
      OnBreak( isBroken );
    }
  }
  #endregion


  #region Public Methods
  public void Break()
  {
    IsBroken = true;
  }

  public void Init( int gemsAmount )
  {
    IsBroken = false;

    InitGems( gemsAmount );
  }
  #endregion

  #region Private Methods
  private void InitGems( int gemsAmount )
  {
    if ( StaticPool<MineGem>.IsNotExists )
      StaticPool<MineGem>.Init( gemPrefab );

    int oldGemsCount = gems.Count;
    for ( int i = oldGemsCount; i < gemsAmount + oldGemsCount; i++ )
    {
      MineGem gem = gemsPool.Get();
      gems.Add( gem );

      gem.Init( () => ReleaseGem( gem ) );

      Vector2 randomPosInsideOfSize = new Vector2( Random.Range( -halfSize.x, halfSize.x ), Random.Range( -halfSize.z, halfSize.z ) );
      Vector3 randomGemPos          = new Vector3( randomPosInsideOfSize.x, halfSize.y, randomPosInsideOfSize.y );

      gem.transform.parent = transform;
      gem.transform.localPosition = randomGemPos;
    }
  }

  private void OnBreak( bool isBroke )
  {
    animator.Init( isBroke );
    bool isRepaired = !isBroke;
    if ( isRepaired )
      return;

    foreach ( MineGem gem in gems )
    {
      gem.IsInteractable = true;
      gem.transform.localPosition = gem.transform.localPosition.setY( 0.0f ); //gem.transform.localPosition.setY( -halfSize.y );
    }

    Debug.Log( $"Broke {name}" );
    onBreak();
  }

  private void ReleaseGem( MineGem gem )
  {
    gemsPool.Release( gem );
    gems.Remove( gem );
  }
  #endregion
}