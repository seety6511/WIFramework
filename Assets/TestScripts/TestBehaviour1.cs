using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WIFramework;

public class TestBehaviour1 : WIBehaviour
{
    public Test_RuntimeWIMaker maker;
    public override void Initialize()
    {
        base.Initialize();
        Debug.Log($"{name} is Initialize");
    }
}
