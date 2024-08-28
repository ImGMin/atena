using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineSceneTimeManager : BaseTimeManager
{
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
