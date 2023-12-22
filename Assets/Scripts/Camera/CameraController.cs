using UnityEngine;


public class CameraController : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField] private Transform target = null;
    #endregion

    #region Private Fields
    private Vector3 offset = Vector3.zero;
    #endregion


    #region Unity Methods
    void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        transform.position = target.position + offset;
    }
    #endregion
}
