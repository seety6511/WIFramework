using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WIFramework;

public class Monster : MonoBehaviour
{
    public Player target;
    public PlayerChild target2;
    public float speed = 5f;

    private void Update()
    {
       var dir = target.transform.position - transform.position;
       transform.position += dir.normalized * speed * Time.deltaTime;
    }
}
