using System;
using System.Collections;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static IEnumerator WaitThenFireAction(float seconds, Action callback = null)
    {
        yield return new WaitForSeconds(seconds);
        callback?.Invoke();
    }
}
