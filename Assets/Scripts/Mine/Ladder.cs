using UnityEngine;


public class Ladder : MonoBehaviour
{
  public Vector3 GetStartClimbPosition( Vector3 climber )
  {
    return (transform.position + transform.forward).setY( climber.y );
  }

  public Quaternion GetRotationToLadder()
  {
    return Quaternion.LookRotation( -transform.forward, transform.up );
  }
}

