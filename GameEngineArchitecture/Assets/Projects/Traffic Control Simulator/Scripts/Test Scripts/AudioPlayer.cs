using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GetComponent<AudioSource>().Play();
    }
}
