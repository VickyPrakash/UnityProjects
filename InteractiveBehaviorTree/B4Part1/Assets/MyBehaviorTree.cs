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
    public GameObject scroll1;
    public GameObject scroll2;
    public GameObject scroll3;
    public GameObject scroll4;
    public float speed = 2.3f;
    private BehaviorAgent behaviorAgent;
    private System.Random random1 = new System.Random();
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
    protected Node ST_Shoot(GameObject p1)
    {
        Val<string> gestureName = Val.V(() => "KATANA");
        Val<bool> startBool = Val.V(() => true);
        return new Sequence(p1.GetComponent<BehaviorMecanim>().Node_HandAnimation(gestureName, startBool), new LeafWait(100));
    }


    protected Node ST_Duck(GameObject p1)
    {
        Val<string> gestureName = Val.V(() => "DUCK");
        Val<bool> startBool = Val.V(() => true);
        return new Sequence(p1.GetComponent<BehaviorMecanim>().Node_BodyAnimation(gestureName, startBool), new LeafWait(100));
    }
    protected Node ST_NotDuck(GameObject p1)
    {
        Val<string> gestureName = Val.V(() => "DUCK");
        Val<bool> startBool = Val.V(() => false);
        return new Sequence(p1.GetComponent<BehaviorMecanim>().Node_BodyAnimation(gestureName, startBool), new LeafWait(100));
    }
    protected Node Shoot()
    {
        return new Sequence(
            player3.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(scroll1.transform.position, 2.7f),
            ST_Shoot(player3),
            player2.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("DYING", 1000),
            new LeafWait(1000),
            //player3.GetComponent<BehaviorMecanim>().Node_OrientTowards(player1.transform.position),
            //player3.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("KATANA45DEGSWING", 1000),
            //player1.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("STARTDUCK", 1000)
            ST_Duck(player1),
            ST_NotDuck(player1)
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
        return new LeafInvoke(() => door.SetActive(false));
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

    

    protected Node Vanish(GameObject vanishObject)
    {
        return new LeafInvoke(() => vanishObject.gameObject.SetActive(false));
    }

    protected Node Findsplayer()
    {
        return new Sequence(
            player1.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(player2.transform.position, 1.5f)
        );
    }

    protected Node Dance()
    {
        return new Sequence(new SequenceParallel(
            player1.GetComponent<BehaviorMecanim>().ST_TurnToFace(player2.transform.position),
            player2.GetComponent<BehaviorMecanim>().ST_TurnToFace(player1.transform.position)),
            /*new SequenceParallel(ST_Greetings(player1),
            ST_Greetings(player2)),*/
            new SequenceParallel(
            player1.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("BREAKDANCE", 5000),
            player2.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("BREAKDANCE", 5000)
        ));
    }

    /*protected Node ST_Greetings(GameObject p1)
    {
        Val<string> gestureName = Val.V(() => "WAVE");
        Val<bool> startBool = Val.V(() => true);
        return new Sequence(p1.GetComponent<BehaviorMecanim>().Node_HandAnimation(gestureName, startBool), new LeafWait(100));
    }*/


    protected Node ST_PickUp(GameObject p1)
    {
        Val<string> gestureName = Val.V(() => "PICKUPRIGHT");
        Val<bool> startBool = Val.V(() => true);
        return new Sequence(p1.GetComponent<BehaviorMecanim>().Node_BodyAnimation(gestureName, startBool), new LeafWait(100));
    }

    

    protected Node pickUpScrolls(GameObject sc)
    {
        return
            new Sequence(
                new SequenceParallel(
                    player1.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(sc.transform.position, 1.7f),
                    player2.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(sc.transform.position, 2.7f)),
                //player3.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(scroll.transform.position, 2.7f)),

                //player1.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("IDLE", 20),
                ST_PickUp(player1),
                new LeafWait(1000),
                Vanish(sc)
                );
    }
    Vector3 giveRandom()
    {
        Vector3 position = scroll1.transform.position;
        int rand = random1.Next(0, 4);
        if(rand == 0)
        {
            position = scroll1.transform.position;
        }else if (rand == 1)
        {
            position = scroll2.transform.position;
        }else if (rand == 2)
        {
            position = scroll3.transform.position;
        }
        else
        {
            position = scroll4.transform.position;
        }
        return position;

    }
    protected Node randomizeScroll()
    {
        /*return
            new Sequence(
                new SequenceParallel(
                    player1.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(giveRandom(), 1.7f),
                    player2.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(giveRandom(), 2.7f)),
                //player3.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(scroll.transform.position, 2.7f)),

                //player1.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("IDLE", 20),
                ST_PickUp(player1),
                new LeafWait(1000),
                Vanish(scroll1)
                );*/
        return new SequenceShuffle(
            pickUpScrolls(scroll1),
            pickUpScrolls(scroll2),
            pickUpScrolls(scroll3),
            pickUpScrolls(scroll4)
            );

    }
    protected Node BuildTreeRoot()
    {
        return
            new Sequence(
                OpenDoor(),
                new Sequence(
                    Findsplayer(),
                    new LeafWait(1000),
                    Dance(),
                    new LeafWait(1000)),
               //pickUpScrolls(scroll1),
                randomizeScroll(),
                
                Shoot()

                
                );
    }
}
