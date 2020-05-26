using UnityEngine;

public class Camera : MonoBehaviour
{
    #region preset
    /// <summary>
    /// 玩家變形元件
    /// </summary>
    private Transform player;

    [Header("鏡頭追蹤速度"), Range(0.1f, 50.5f)]
    public float CameraSpeed = 1.5f;
    #endregion

    
    #region method
    private void Track()
    {
        Vector3 posTrack = player.position;
        posTrack.y += 2f;
        posTrack.z += 4f;

        Vector3 posCam = transform.position;
        posCam = Vector3.Lerp(posCam, posTrack, 0.5f * Time.deltaTime * CameraSpeed);
        transform.position = posCam;
    }
    #endregion

    #region events
    private void Start()
    {
        player = GameObject.Find("unitychan").transform;
    }

    // do after Update
    private void LateUpdate()
    {
        Track();
    }
    #endregion
}
