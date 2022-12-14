using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WIFramework;

public class Player : MonoBehaviour, IGetKey, ISingle
{
    public float speed = 5f;

    public Transform hand;
    public RectTransform hand_1;
    [SerializeField] Transform hand_2;
    [SerializeField] RectTransform hand_3;
    protected Transform hand_4;
    protected RectTransform hand_5;
    Transform hand_6;
    RectTransform hand_7;

    public void GetKey(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W:
                transform.position += Vector3.up * speed * Time.deltaTime;
                break;
            case KeyCode.A:
                transform.position += Vector3.left * speed * Time.deltaTime;
                break;
            case KeyCode.S:
                transform.position += Vector3.down * speed * Time.deltaTime;
                break;
            case KeyCode.D:
                transform.position += Vector3.right * speed * Time.deltaTime;
                break;
        }
    }
}
