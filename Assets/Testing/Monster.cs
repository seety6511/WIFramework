using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WIFramework;

public class Monster : WIBehaviour
{
    [SerializeField] Player target;
    public float speed = 5f;

    private void Update()
    {
       var dir = target.transform.position - transform.position;
       transform.position += dir.normalized * speed * Time.deltaTime;
    }
}
