using UnityEditor;
using UnityEngine;


[CustomEditor( typeof( MineController ) )]
public class MineControllerEditor : EditorBase<MineController>
{
  private int virtualLayerNum = 0;


  public override void OnInspectorGUI()
  {
    base.OnInspectorGUI();

    if ( !Application.isPlaying )
      return;

    EditorGUILayout.BeginVertical();
    if ( GUILayout.Button( "Next layer" ) )
    {
      castedTarget.CurLayerNum++;
    }

    EditorGUILayout.BeginHorizontal();
    virtualLayerNum = EditorGUILayout.IntField( "Layer", virtualLayerNum );
    if ( GUILayout.Button( "Set layer" ) )
    {
      castedTarget.CurLayerNum = virtualLayerNum;
    }
    EditorGUILayout.EndHorizontal();
    EditorGUILayout.EndVertical();
  }
}