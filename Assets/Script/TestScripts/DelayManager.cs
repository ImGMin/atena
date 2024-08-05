using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayManager : MonoBehaviour
{
    private static DelayManager instance;

    private void Awake()
    {
        // 싱글톤 패턴을 사용하여 DelayManager의 인스턴스를 하나로 유지합니다.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 코루틴을 시작하는 static 메서드
    public static void ExecuteAfterDelay(MonoBehaviour monoBehaviour, float delay, System.Action action)
    {
        instance.StartCoroutine(instance.DelayedAction(delay, action));
    }

    // 코루틴으로 딜레이를 처리하는 메서드
    private IEnumerator DelayedAction(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
