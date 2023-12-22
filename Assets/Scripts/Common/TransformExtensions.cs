using UnityEngine;


public static class TransformExtensions
{
  public static Vector3 setX( this Vector3 data, float x )
  {
    return new Vector3( x, data.y, data.z );
  }

  public static Vector3 setY( this Vector3 data, float y )
  {
    return new Vector3( data.x, y, data.z );
  }

  public static Vector3 setZ( this Vector3 data, float z )
  {
    return new Vector3( data.x, data.y, z );
  }


  public static Vector2 setX( this Vector2 data, float x )
  {
    return new Vector2( x, data.y );
  }

  public static Vector2 setY( this Vector2 data, float y )
  {
    return new Vector2( data.x, y );
  }

  public static Vector3 setZ( this Vector2 data, float z )
  {
    return new Vector3( data.x, data.y, z );
  }


  public static Vector3 plusX( this Vector3 data, float x )
  {
    return new Vector3( data.x + x, data.y, data.z );
  }

  public static Vector3 plusY( this Vector3 data, float y )
  {
    return new Vector3( data.x, data.y + y, data.z );
  }

  public static Vector3 plusZ( this Vector3 data, float z )
  {
    return new Vector3( data.x, data.y, data.z + z );
  }

  public static Vector3 multX( this Vector2 data, float x )
  {
    return new Vector3( data.x * x, data.y, 0.0f );
  }

  public static Vector3 multY( this Vector2 data, float y )
  {
    return new Vector3( data.x, data.y * y, 0.0f );
  }
  
  public static Vector3 multX( this Vector3 data, float x )
  {
    return new Vector3( data.x * x, data.y, data.z );
  }

  public static Vector3 multY( this Vector3 data, float y )
  {
    return new Vector3( data.x, data.y * y, data.z );
  }

  public static Vector3 multZ( this Vector3 data, float z )
  {
    return new Vector3( data.x, data.y, data.z * z );
  }

  public static Vector3 plusX( this Vector2 data, float x )
  {
    return new Vector3( data.x + x, data.y, 0.0f );
  }

  public static Vector3 plusY( this Vector2 data, float y )
  {
    return new Vector3( data.x, data.y + y, 0.0f );
  }

  public static float distanceTo( this Vector3 from, Vector3 to )
  {
    return Vector3.Distance( from, to );
  }

  public static float distanceTo( this Vector2 from, Vector2 to )
  {
    return Vector2.Distance( from, to );
  }
}