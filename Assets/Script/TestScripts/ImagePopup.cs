using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagePopup : MonoBehaviour
{
    public float time = 0f;
    public float speed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + 0.5f,
            transform.position.z
        );

        if (time >= 2.0)
        {
            Destroy(gameObject);
            Debug.Log("asdfasdfasdf");
        }
    }
}
