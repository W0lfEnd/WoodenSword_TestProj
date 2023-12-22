using System;
using UnityEngine;


public class MineController : MonoBehaviour
{
  #region Events
  public event Action<int> onLayerChanged = delegate {};
  #endregion

  #region Serialize Fields
  [Header( "Ladder")]
  [SerializeField] private RepeatableObject ladder = null;
  [SerializeField] private float            ladderMaxHeight  = 100.0f;
  [SerializeField] private PlayerController playerController = null;
  [Range( 1, 20 )]
  [SerializeField] private int updateLadderEveryNFrame = 1;

  [Header( "Ground")]
  [SerializeField] private Transform mineGroundRoot = null;

  [Header( "Mine layers" )]
  [SerializeField] private MineLayer curLayer = null;
  [SerializeField] private float      layerHeight   = 1.0f;
  [SerializeField] private Vector2Int mineSize      = new Vector2Int( 6, 4 );
  [SerializeField] private int        startLayerNum = 0;
  #endregion

  #region Private Fields
  private const int LAYERS_ABOVE_GROUND = 1;
  private       int curLayerNum         = 0;
  #endregion
  
  #region Properties
  public int CurLayerNum
  {
    get => curLayerNum;
    set
    {
      curLayerNum = value;
      Init( curLayerNum );

      onLayerChanged( curLayerNum );
    }
  }
  #endregion


  #region Unity Methods
  private void Awake()
  {
    InitSubscriptions();
  }

  private void Start()
  {
    CurLayerNum = startLayerNum;
    ReinitLadder();
  }

  private void Update()
  {
    TryToReinitLadder();
  }
  #endregion

  #region Private Methods
  private void Init( int layer )
  {
    Debug.Log( $"Init Mine with layer: {curLayerNum}" );
    SetGroundPosition( layer );
    curLayer.Init( layer, mineSize.x, mineSize.y );
  }

  private void InitSubscriptions()
  {
    curLayer.onLayerFinished -= OnLayerFinished;
    curLayer.onLayerFinished += OnLayerFinished;
  }

  private void TryToReinitLadder()
  {
    if ( Time.frameCount % updateLadderEveryNFrame != 0 )
      return;

    ReinitLadder();
  }

  private void ReinitLadder()
  {
    InitLadder( playerController.transform.position );
  }

  private void InitLadder( Vector3 playerPos )
  {
    Vector3 playerPosInLocalSpace = ladder.transform.InverseTransformPoint( playerPos );
    float   halfHeight            = ladderMaxHeight / 2.0f;

    //ladder.SetRangeByPosition( playerPosInLocalSpace.plusY( halfHeight ), playerPosInLocalSpace.plusY( -halfHeight ) );
    ladder.SetRangeByPosition( playerPosInLocalSpace.plusY( -halfHeight ), playerPosInLocalSpace.plusY( halfHeight ) );
  }

  private void SetGroundPosition( int layer )
  {
    float layerGroundY = -( layer + LAYERS_ABOVE_GROUND ) * layerHeight;
    mineGroundRoot.transform.localPosition = mineGroundRoot.transform.localPosition.setY( layerGroundY );
  }

  private void OnLayerFinished()
  {
    CurLayerNum++;
  }
  #endregion
}