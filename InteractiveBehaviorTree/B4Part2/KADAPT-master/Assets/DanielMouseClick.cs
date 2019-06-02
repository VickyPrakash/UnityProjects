using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentDirector : MonoBehaviour
{

    public Camera cam;

    private int selectCount;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            
            RaycastHit hit;

            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                foreach (var agent in FindObjectsOfType<GameObject>())
                {
                   
                    if(agent.CompareTag("Daniel")==true){
                    NavMeshAgent agentNav = agent.GetComponent<NavMeshAgent>();
                    agentNav.destination = hit.point;

                        
                    }
                }
             }
        }

        selectCount = 0;
    }
}
