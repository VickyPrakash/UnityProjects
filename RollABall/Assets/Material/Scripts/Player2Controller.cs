using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player2Controller : MonoBehaviour
{
    public float speed;
    private Rigidbody rb2;
    public GameObject player2;
    public int count2;
    public int hit2;
    public Text countText2;
    public Text guiTexts2;
    public Text winText2;
    // Start is called before the first frame update
    void Start()
    {
        rb2 = GetComponent<Rigidbody>();
        count2 = 0;
        hit2 = 0;
        DisplayCountText();
        winText2.text = "";
        player2.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            rb2.AddForce(movement * speed);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count2 = count2 + 3;
            hit2 = hit2 + 1;
            DisplayCountText();
            StartCoroutine(ShowMessage("Player B Picked a cube", 2));
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            //other.gameObject.SetActive(true);
            count2 = count2 - 1;
            DisplayCountText();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            count2 = count2 - 1;
            DisplayCountText();
            StartCoroutine(ShowMessage("Player B Wall Collision", 2));
        }

    }
    private void DisplayCountText()
    {
        countText2.text = "Player 2: " + count2.ToString();
        if (hit2 + FindObjectOfType<PlayerController>().hit >= 12)
        {
            player2.SetActive(false);
            FindObjectOfType<PlayerController>().player.SetActive(false);
            if (count2 > FindObjectOfType<PlayerController>().count)
            {
                winText2.text = "Player B Wins!!!";
            }
            else if (FindObjectOfType<PlayerController>().count > count2)
            {
                winText2.text = "Player A Wins!!!";
            }
            else
            {
                winText2.text = "Its a Draw!!!";
            }

        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Play()
    {
        SceneManager.LoadScene("MiniGame");
    }
    IEnumerator ShowMessage(string message, float delay)
    {
        guiTexts2.text = message;
        guiTexts2.enabled = true;
        yield return new WaitForSeconds(delay);
        guiTexts2.enabled = false;
    }
}
