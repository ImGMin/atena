using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePopup : MonoBehaviour
{
    public float time = 0f;
    public float speed = 0.1f;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + 0.5f,
            transform.position.z
        );

        Color color = image.color;
        color.a = Mathf.Clamp01(1 - time/2);
        image.color = color;

        if (time >= 2.0f)
        {
            Destroy(gameObject);
            Debug.Log("asdfasdfasdf");
        }
    }
}
