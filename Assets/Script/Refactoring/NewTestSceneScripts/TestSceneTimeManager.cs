using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneTimeManager : BaseTimeManager
{
    // Start is called before the first frame update
    protected override void Start()
    {
        SetTimeFlag(true);
        SetTimeScale(1f);
    }

    protected override void Update()
    {
        base.Update();
    }
}
