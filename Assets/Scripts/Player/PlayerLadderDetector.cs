using System;
using UnityEngine;


public class PlayerLadderDetector : MonoBehaviour
{
    #region Events
    public event Action<Ladder> onLadderChanged = delegate {};

    #endregion

    #region Serialize Fields
    [SerializeField] private float detectionDistance       = 1.5f;
    [SerializeField] private float verticalRayOriginOffset = 0.5f;
    #endregion

    #region Private Fields
    private const float  COOLDOWN_DELAY = 0.4f;
    private       float  lastTimeUsed   = 0.0f;
    private       Ladder curLadder      = null;
    #endregion

    #region Properties
    private Vector3 raycastOrigin => transform.position + Vector3.up * verticalRayOriginOffset;

    public Ladder CurLadder
    {
        get => curLadder;
        set
        {
            if ( curLadder == value )
                return;

            if ( curLadder == null && value != null && Time.fixedTime - lastTimeUsed <= COOLDOWN_DELAY )
                return;

            lastTimeUsed = Time.time;
            curLadder = value;
            onLadderChanged( curLadder );
        }
    }
    #endregion


    #region Public Methods
    public void Unclimb()
    {
        CurLadder = null;
    }
    #endregion

    #region Private Methods
    private void FixedUpdate()
    {
        Ladder newLadder = null;
        if ( Physics.Raycast( raycastOrigin, transform.forward, out RaycastHit hit, detectionDistance ) )
            newLadder = hit.transform.GetComponent<Ladder>();

        CurLadder = newLadder;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = curLadder == null ? Color.green : Color.red;
        Gizmos.DrawLine( raycastOrigin, raycastOrigin + transform.forward * detectionDistance );
    }
    #endregion
}
