using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WIFramework;

public class Player : WIBehaviour, IGetKey, ISingle
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

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log($"{name}");
        Debug.Log($"{hand}");
        Debug.Log($"{hand_1}");
        Debug.Log($"{hand_2}");
        Debug.Log($"{hand_3}");
        Debug.Log($"{hand_4}");
        Debug.Log($"{hand_5}");
        Debug.Log($"{hand_6}");
        Debug.Log($"{hand_7}");
    }

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
