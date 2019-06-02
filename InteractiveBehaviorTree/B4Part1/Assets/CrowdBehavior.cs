using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;

public class CrowdBehavior : MonoBehaviour
{
    public GameObject crowd1;
    public GameObject crowd2;
    public GameObject crowd3;
    public GameObject crowd4;
    public GameObject crowd5;
    public GameObject crowd6;
    public GameObject daniel;
    public GameObject kevin;


    private BehaviorAgent behaviorAgent;
    // Start is called before the first frame update
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected Node ST_Greeting(GameObject p1)
    {
        Val<string> gestureName = Val.V(() => "PICKUPRIGHT");
        Val<bool> startBool = Val.V(() => true);
        return new Sequence(p1.GetComponent<BehaviorMecanim>().Node_BodyAnimation(gestureName, startBool), new LeafWait(100));
    }


    protected Node ST_FocusOn(GameObject p1, GameObject p2, GameObject p3)
    {
        return new Sequence(
            p2.GetComponent<BehaviorMecanim>().ST_TurnToFace(p1.transform.position),
            p3.GetComponent<BehaviorMecanim>().ST_TurnToFace(p1.transform.position),
            p1.GetComponent<BehaviorMecanim>().ST_TurnToFace(p2.transform.position),
            ST_Greeting(p1),
            ST_Greeting(p2),
            ST_Greeting(p3)
            );
    }

    protected Node ST_Conversation(GameObject p1, GameObject p2, GameObject p3)
    {
        return new CreateRandom(ST_FocusOn(p1, p2, p3),
            ST_FocusOn(p1, p3, p2),
            ST_FocusOn(p2, p3, p1),
            ST_FocusOn(p3, p2, p1),
            ST_FocusOn(p2, p1, p3)
            );
    }

    protected Node BuildTreeRoot()
    {
        Node root = new DecoratorLoop(new SequenceParallel(ST_Conversation(crowd1, crowd2, crowd3), ST_Conversation(crowd4, crowd5, crowd6)));
        return root;
        
    }
}
