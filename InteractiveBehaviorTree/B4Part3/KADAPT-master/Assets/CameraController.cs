﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<MyBehaviorTree>().playerBoy;
        offset = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate()
    {
        player = FindObjectOfType<MyBehaviorTree>().playerBoy;
        Vector3 newPosition;
        float angle = player.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        newPosition = player.transform.position - (rotation * offset);
        StartCoroutine(TransitionCamera(player, newPosition));
    }

    IEnumerator TransitionCamera(GameObject player1, Vector3 endPosition)
    {
        float TransitionTime = 1f;
        float t = 0.0f;
        Vector3 StatrtingPosition = transform.position;
        while(t<1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / TransitionTime);

            transform.position = Vector3.Lerp(StatrtingPosition, endPosition, t);
            transform.LookAt(player1.transform);
            yield return 0;


        }
    }
}
