using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TreeSharpPlus;

public class MyBehaviorTree : MonoBehaviour
{
    public GameObject BeginningPoint;
    public Text guiTexts;
    //public Text timerText;
    public GameObject door;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject playerBoy;
    //public Animator door_Animator;
    public GameObject scroll1;
    public GameObject scroll2;
    public GameObject scroll3;
    public GameObject scroll4;
    public UnityEngine.Random rand = new UnityEngine.Random();
    public float speed = 10.0f;
    //public Rigidbody rb;
    public float startTime;
    private BehaviorAgent behaviorAgent;
    float setTime = 120.0f;
    int objNum = 8;
    int scrollNum = 4;
    public int play = 0;
    // Use this for initialization
    void Start()
    {
        //door_Animator = gameObject.GetComponent<Animator>();
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
        startTime = Time.time;
        StartCoroutine(ShowMessage("", 10));
        playerBoy = player1;
       
        
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
        return new Sequence(p1.GetComponent<BehaviorMecanim>().Node_HandAnimation(gestureName, startBool), new LeafWait(100) );
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
            //player3.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(player2.transform.position, 2.7f),
            ST_Shoot(player3),
            ST_Duck(player2),
            //new LeafWait(1000),
            //player3.GetComponent<BehaviorMecanim>().Node_OrientTowards(player1.transform.position),
            //player3.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("KATANA45DEGSWING", 1000),
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
                ST_Greetings(player1),
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
                player3.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(player2.transform.position, 2.7f),
            player1.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("BREAKDANCE", 5000),
            player2.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("BREAKDANCE", 5000)
            
        ));
    }

    protected Node ST_Greetings(GameObject p1)
    {
        Val<string> gestureName = Val.V(() => "WAVE");
        Val<bool> startBool = Val.V(() => true);
        return new Sequence(p1.GetComponent<BehaviorMecanim>().Node_HandAnimation(gestureName, startBool), new LeafWait(100));
    }


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
                    player2.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(sc.transform.position, 1.7f)),
                //player3.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(scroll.transform.position, 2.7f)),

                ST_PickUp(player1),
                new LeafWait(1000),
                Vanish(sc)
                );
    }

    /* protected Node randomizeScroll()
     {
         /*int i = UnityEngine.Random.Range(0, 4);
         if (i == 0)
         {
             return new Sequence(pickUpScrolls(scroll1), new LeafWait(100));

         }else if(i == 1)
         {
             return new Sequence(pickUpScrolls(scroll2), new LeafWait(100));
         }else if(i == 2)
         {
             return new Sequence(pickUpScrolls(scroll3), new LeafWait(100));
         }
         else
         {
             return new Sequence(pickUpScrolls(scroll4), new LeafWait(100));
         }*/
    /*return new SequenceShuffle(
        pickUpScrolls(scroll1),
        pickUpScrolls(scroll2),
        pickUpScrolls(scroll3),
        pickUpScrolls(scroll4)
        );*/

    //}

    IEnumerator ShowMessage(string message, float delay)
    {
        guiTexts.text = message;
        guiTexts.enabled = true;
        yield return new WaitForSeconds(delay);
        guiTexts.enabled = false;
    }

    GameObject NearestObject(string tag, Vector3 CurrentPosition)
    {
        GameObject[] ObjList;
        ObjList = GameObject.FindGameObjectsWithTag(tag);
        float nearestDist = float.PositiveInfinity;
        GameObject nearestObject = null;
        int temp = 0;
        foreach(GameObject SingleObj in ObjList)
        {
            temp = temp + 1;
            Vector3 posdiff = SingleObj.transform.position - CurrentPosition;
            float distancediff = posdiff.sqrMagnitude;
            if (distancediff < nearestDist)
            {
                nearestDist = distancediff;
                nearestObject = SingleObj;
            }
        }
        return nearestObject;
    }

    int NumberOfObjects(string tag)
    {
        GameObject[] ObjLst;
        ObjLst = GameObject.FindGameObjectsWithTag(tag);
        int temp = 0;
        foreach(GameObject SnglObj in ObjLst)
        {
            temp = temp + 1;
        }
        return temp;
    }

    void gameMoves(GameObject player)
    {
        GameObject collectObject;
        GameObject collectScroll;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal*speed, 0.0f, moveVertical*speed);
            player.transform.Translate(moveHorizontal*0.1f, 0.0f, moveVertical*0.1f);
        }
        float currentTime = Time.time;
        float timeSpent = currentTime - startTime;
        //float setTime = 120.0f;
        //timerText.text = (setTime - timeSpent).ToString("f0") + "seconds";
        if(timeSpent > setTime)
        {
            StartCoroutine(ShowMessage("Player Loses!", 50));
            //player.SetActive(false);
        }
        if(objNum >  0)
        {
            collectObject = NearestObject("Collect", player.transform.position);
            Vector3 posdist = collectObject.transform.position - player.transform.position;
            float distnum = posdist.sqrMagnitude;
            if (distnum < 3)
            {
                collectObject.SetActive(false);
                objNum = objNum - 1;
            }
        }
        if(scrollNum > 0)
        {
            collectScroll = NearestObject("Scroll", player.transform.position);
            Vector3 posdist2 = collectScroll.transform.position - player.transform.position;
            float distnum2 = posdist2.sqrMagnitude;
            if (distnum2 < 3)
            {
                collectScroll.SetActive(false);
                scrollNum = scrollNum - 1;
                //timerText.text = scrollNum.ToString();
                if(player == player1)
                {
                    setTime = setTime + 20.0f;
                }
                if(player == player3)
                {
                    StartCoroutine(ShowMessage("Shooter Loses", 50));
                    //player.SetActive(false);

                }
                
            }
        }
        if(player == player3)
        {
            if(NumberOfObjects("Collect") == 1)
            {
                StartCoroutine(ShowMessage("Shooter Wins", 50));
                //player.SetActive(false);
            }
        }
        if (NumberOfObjects("Scroll") == 1 && NumberOfObjects("Collect") == 1)
        {
            if(player == player1)
            {
                StartCoroutine(ShowMessage("Daniel Wins!", 50));
                //player.SetActive(false);
            }
            else
            {
                StartCoroutine(ShowMessage("Kevin Wins!", 50));
                //player.SetActive(false);
            }
        }
        
    }

    protected Node UserControlLeaf(GameObject player)
    {
        return new LeafInvoke(() => gameMoves(player));
    }
    void selectPlayer()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            play = 1;
            playerBoy = player1;
        }else if (Input.GetKey(KeyCode.Alpha2))
        {

            play = 2;
            playerBoy = player2;
        }else if (Input.GetKey(KeyCode.Alpha3))
        {
            play = 3;
            playerBoy = player3;
        }
    }
    void chooseAPlayer()
    {
        if (play == 1)
        {
            gameMoves(player1);
        }
        else if (play == 2)
        {
            gameMoves(player2);
        }else if (play == 3)
        {
            gameMoves(player3);
        }
        else
        {
            gameMoves(player1);
        }
    }

    protected Node choosePlayer()
    {
        return new LeafInvoke(() => chooseAPlayer());
    }

    protected Node selectPlayerNode()
    {
        return new LeafInvoke(() => selectPlayer());
    }

    protected Node UserControl()
    {
        
        return new DecoratorLoop(choosePlayer());
        
    }
    protected Node switchSituation()
    {
        if(play == 1)
        {
            return new Sequence(ST_Greetings(player1));
        }
        else if(play == 2)
        {
            return new Sequence(ST_NotDuck(player2), ST_Greetings(player2));
        }
        else if(play == 3){
            return new Sequence(ST_Duck(player1), new LeafWait(100));
        }
        else
        {
            return new Sequence(ST_Greetings(player1));
        }
    }
    protected Node BuildTreeRoot()
    {
        return
            new Sequence(
                OpenDoor(),
                new Sequence(
                    Findsplayer(),
                    new LeafWait(1000),
                    Dance()),

                //randomizeScroll(),
                Shoot(),
                selectPlayerNode(),
                //switchSituation(),
                UserControl()
                
                );
    }
}