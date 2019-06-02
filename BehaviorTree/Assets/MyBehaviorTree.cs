using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class MyBehaviorTree : MonoBehaviour
{
    public GameObject BeginningPoint;
    public GameObject door;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    //public Animator door_Animator;
    public GameObject scroll;
    public float speed = 2.3f;

    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
        //door_Animator = gameObject.GetComponent<Animator>();
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*protected Node ST_ApproachAndWait(Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
	}

	
}*/
    
    protected Node Shoot()
    {
        return new Sequence(
            player3.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("KATANA45DEGSWING", 1000),
            player2.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("DYING", 1000),
            //player2.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("DEAD", 1000),
            new LeafWait(1000),
            //player3.GetComponent<BehaviorMecanim>().Node_OrientTowards(player1.transform.position),
            player3.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("KATANA45DEGSWING", 1000),
            player1.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("STARTDUCK", 1000)
            );
    }

    protected Node ST_ApproachAndWait(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(player1.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }

    /*protected Node BuildTreeRoot()
    {
        /*Val<float> pp = Val.V (() => police.transform.position.z);
		Func<bool> act = () => (police.transform.position.z > 10);
		Node roaming = new DecoratorLoop (
						new Sequence(
						this.ST_ApproachAndWait(this.wander1),
						this.ST_ApproachAndWait(this.wander2),
						this.ST_ApproachAndWait(this.wander3)));
		Node trigger = new DecoratorLoop (new LeafAssert (act));
		Node root = new DecoratorLoop (new DecoratorForceStatus (RunStatus.Success, new SequenceParallel(trigger, roaming)));
		return root;
    }*/
    protected Node DoorOpens()
    {
        return new LeafInvoke(() => door.transform.Translate(0, speed, 0));
    }
    protected Node OpenDoor()
    {
        return
            new Sequence(
        player1.GetComponent<BehaviorMecanim>().Node_GoTo(BeginningPoint.transform.position),
        player1.GetComponent<BehaviorMecanim>().Node_OrientTowards(door.transform.position),
        player1.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CLAP", 1000),
        new LeafWait(1000), DoorOpens()
        
        );
        
    }

    protected Node Findsplayer()
    {
        return new Sequence(
            player1.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(player2.transform.position, 0.5f)
        );
    }

    protected Node Dance()
    {
        return new SequenceParallel
        (
            player1.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("BREAKDANCE", 5000),
            player2.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("BREAKDANCE", 5000)
        );
    }

    protected Node pickUpScrolls()
    {
        return
            new Sequence(
                new SequenceParallel(
                    player1.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(scroll.transform.position, 1.7f),
                    player2.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(scroll.transform.position, 2.7f),
                    player3.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(scroll.transform.position, 2.7f)),

                player1.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("IDLE", 20),
                player1.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("GROUND_PICKUP_RIGHT", 30)

                );
    }

    protected Node BuildTreeRoot()
    {
        return
            new Sequence(
                OpenDoor(),
               /* new Sequence(
                    Findsplayer(),
                    new LeafWait(1000),
                    Dance(),
                    new LeafWait(1000)),*/
                pickUpScrolls(),
                Shoot()

                
                );
    }
}
