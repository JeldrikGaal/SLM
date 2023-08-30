using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RESETER : MonoBehaviour
{
    private float _startTime;
    private float _waitTime;
    // Start is called before the first frame update
    void Start()
    {
        _waitTime = 300;
        _startTime = Time.time;   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            _startTime = Time.time;
        }
        if (Time.time - _startTime > _waitTime)
        {
            Scene scene = SceneManager.GetActiveScene();

            foreach (GameObject gameObject in FindObjectsOfType<GameObject>()) {
                if (gameObject.scene.buildIndex == -1) {
                    Destroy(gameObject);
                }
            }
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}
