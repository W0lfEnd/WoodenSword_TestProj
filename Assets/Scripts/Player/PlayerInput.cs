using System;
using UnityEngine;


public class PlayerInput : MonoBehaviour
{
  public event Action<float>   onHorizontalInput = delegate {};
  public event Action<float>   onVerticalInput   = delegate {};
  public event Action<Vector2> onDirectionInput  = delegate {};


  private void Update()
  {
    onHorizontalInput( MyInput.Instance.HorizontalInput );
    onVerticalInput( MyInput.Instance.VerticalInput );
    onDirectionInput( MyInput.Instance.DirectionInput );
  }
}