using UnityEngine;


public class MyInput : MonoBehaviourSingleton<MyInput>
{
  [SerializeField] private Joystick joystick = null;


  public float   HorizontalInput => joystick.Horizontal;
  public float   VerticalInput   => joystick.Vertical;
  public Vector2 DirectionInput  => joystick.Direction;
}