using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;
    public GameObject player;
    public KeyCode jump;
    public int count;
    public int hit;
    public Text countText;
    public Text guiTexts;
    public Text winText;
    public Text timerText;
    public float startTime;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        hit = 0;
        DisplayCountText();
        winText.text = "";
        startTime = Time.time;
        player.SetActive(true);
    }
    void FixedUpdate()
    {
        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            rb.AddForce(movement * speed);
        }
        
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f0");

        timerText.text = minutes + ":" + seconds;
        if (timerText.text.Equals("2:0"))
        {
            FindObjectOfType<Player2Controller>().player2.SetActive(false);
            player.SetActive(false);
            if (count > FindObjectOfType<Player2Controller>().count2)
            {
                StartCoroutine(ShowMessage("Player A Wins!!!", 10)); 
            }
            else if (FindObjectOfType<Player2Controller>().count2 > count)
            {
                StartCoroutine(ShowMessage("Player B Wins!!!", 10));
            }
            else
            {
                StartCoroutine(ShowMessage("Its a Draw!!!", 10));
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 3;
            hit = hit + 1;
            DisplayCountText();
            StartCoroutine(ShowMessage("Player A picked a cube", 2));
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            //other.gameObject.SetActive(true);
            count = count - 1;
            DisplayCountText();
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            count = count - 1;
            DisplayCountText();
            StartCoroutine(ShowMessage("Player A Wall Collision", 2));
        }

    }
    public void DisplayCountText()
    {
        countText.text = "Player 1: " + count.ToString();
        if (hit + FindObjectOfType<Player2Controller>().hit2 >= 12)
        {
            player.SetActive(false);
            FindObjectOfType<Player2Controller>().player2.SetActive(false);
            if (count > FindObjectOfType<Player2Controller>().count2)
            {
                StartCoroutine(ShowMessage("Player A Wins!!!", 10));
            }
            else if (FindObjectOfType<Player2Controller>().count2 > count)
            {
                StartCoroutine(ShowMessage("Player B Wins!!!", 10));
            }
            else
            {
                StartCoroutine(ShowMessage("Its a Draw!!!", 10));
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
        guiTexts.text = message;
        guiTexts.enabled = true;
        yield return new WaitForSeconds(delay);
        guiTexts.enabled = false;
    }
}