using UnityEditor;
using UnityEngine;


public abstract class EditorBase<T> : Editor
  where T : MonoBehaviour
{
  private T castedTargetCache = null;
  protected T castedTarget => castedTargetCache ??= (T)target;
}