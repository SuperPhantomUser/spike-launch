using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RotateSpike : MonoBehaviour
{

    public LoadingData LoadingData;
    public string scene;

    // Start is called before the first frame update
    void Start()
    {
        scene = LoadingData.sceneToLoad;
        StartCoroutine(LoadingTime());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, -360f * Time.deltaTime);
    }

    IEnumerator LoadingTime() {
        yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        SceneManager.LoadSceneAsync(scene);
    }
}
