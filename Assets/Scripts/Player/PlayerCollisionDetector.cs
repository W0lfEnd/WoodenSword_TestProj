using System;
using UnityEngine;


public class PlayerCollisionDetector : MonoBehaviour
{
    #region Events
    public event Action<MineCell> onCollisionMineCell = delegate {};
    public event Action<MineGem>  onCollisionMineGem  = delegate {};
    #endregion


    #region Unity Methods
    private void OnTriggerEnter( Collider other )
    {
        TryHandleCollisionWithMineCell( other );
        TryHandleCollisionWithMineGem( other );
    }
    #endregion

    #region Private Methods
    private void TryHandleCollisionWithMineCell( Collider other )
    {
        if ( !other.TryGetComponent( out MineCell cell ) )
            return;

        onCollisionMineCell( cell );
    }

    private void TryHandleCollisionWithMineGem( Collider other )
    {
        if ( !other.TryGetComponent( out MineGem gem ) )
            return;

        onCollisionMineGem(gem);
    }
    #endregion
}
