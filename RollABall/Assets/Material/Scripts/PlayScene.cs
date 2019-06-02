using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown (KeyCode.Space))
        {
            SceneManager.LoadScene("MiniGame");
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("MiniGame");
    }
}
