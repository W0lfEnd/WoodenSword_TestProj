using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class MineLayer : MonoBehaviour
{
  #region Events
  public event Action onLayerFinished = delegate {};
  #endregion

  #region Serialize Fields
  [SerializeField] private MineCell cellPrefab = null;
  #endregion

  #region Private Fields
  private MineCell[][] cells    = null;
  private int          curLayer = 0;
  #endregion


  #region Public Methods
  public void Init( int layer, int xCount, int zCount )
  {
    if ( layer < 0 || xCount == 0 || zCount == 0 )
      return;

    curLayer = layer;

    if ( cells == null || xCount != cells.Length || zCount != cells[0].Length )
      SpawnCells( xCount, zCount );

    Vector3 cellSize = cells[0][0].HalfSize * 2;
    InitCells( xCount, zCount, cellSize );
  }

  public static int GemsAmountAtLayer( int layer )
  {
    return Random.Range( 20, 40 ) + 2 * layer;
  }
  #endregion

  #region Private Methods
  private void InitCells( int xCount, int zCount, Vector3 cellSize )
  {
    float xPosMax = xCount * cellSize.x;
    float zPosMax = zCount * cellSize.z;

    Dictionary<Vector2Int, int> gemsInCells = new Dictionary<Vector2Int, int>();
    int                         gemsAtLayer = GemsAmountAtLayer( curLayer );
    for ( int i = 0; i < gemsAtLayer; i++ )
    {
      int x = Random.Range( 0, xCount );
      int z = Random.Range( 0, zCount );
      gemsInCells.SetOrIncrement( new Vector2Int( x, z ) );
    }

    EnumerateCellsAndDo( InitCell );
    return;

    void InitCell( MineCell cell, int i, int j )
    {
      float   x   = i * cellSize.x - ( ( xPosMax - cellSize.x ) * 0.5f );
      float   z   = j * cellSize.z - ( ( zPosMax - cellSize.z ) * 0.5f );
      Vector3 pos = new Vector3( x, cellSize.y * 0.5f, z );
      cell.transform.localPosition = pos;

      int gemsCount = gemsInCells.GetValueOrDefault( new Vector2Int( i, j ) );
      cell.Init( gemsCount );

      cell.onBreak -= OnMineCellBroke;
      cell.onBreak += OnMineCellBroke;
    }
  }

  private void SpawnCells( int xCount, int zCount )
  {
    ClearCells();

    cells = new MineCell[xCount][];
    for ( int i = 0; i < xCount; i++ )
    {
      cells[i] = new MineCell[zCount];
      for ( int j = 0; j < zCount; j++ )
      {
        cells[i][j] = Instantiate( cellPrefab, transform );
      }
    }
  }

  private void ClearCells()
  {
    EnumerateCellsAndDo(
      ( it, _, _ ) =>
      {
        it.onBreak -= OnMineCellBroke;
        Destroy( it );
      }
    );

    cells = null;
  }

  private void EnumerateCellsAndDo( Action<MineCell, int, int> action )
  {
    if ( cells == null )
      return;

    int iMax = cells.Length;
    int jMax = cells[0].Length;
    for ( int i = 0; i < iMax; i++ )
    {
      for ( int j = 0; j < jMax; j++ )
      {
        action?.Invoke( cells[i][j], i, j );
      }
    }
  }

  private void OnMineCellBroke()
  {
    bool isAllBroken = cells.SelectMany( it => it ).All( it => it.IsBroken );
    if ( isAllBroken )
    {
      onLayerFinished();
    }
  }
  #endregion
}