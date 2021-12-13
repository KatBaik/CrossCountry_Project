using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rider : MonoBehaviour
{
    float timeToCommand = 1f;
    float holdForJump = 2f;
    int commandDone = 0;
    Timer gaitTimer;
    Timer jumpTimer;
    public GameObject horseObject;
    Horse horse;

    // Start is called before the first frame update
    void Start()
    {
        gaitTimer = gameObject.AddComponent<Timer>();
        jumpTimer = gameObject.AddComponent<Timer>();
        horse = horseObject.GetComponent<Horse>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gaitTimer.Finished)
        {
            if (!gaitTimer.Check() && commandDone > 0)
            {
                switch (commandDone)
                {
                    case 1:
                        horse.Walk();
                        break;
                    case 2:
                        horse.Trot();
                        break;
                    default:
                        horse.Canter();
                        break;
                }
            }
            commandDone = 0;
        }

        bool gaitCommand = false;
        //bool gaitCommandStart = false;
        bool gaitCommandFinish = false;
        bool turnRight = false;
        bool turnLeft = false;
        bool speedUp = false;
        bool slowDown = false;
        if (Input.touches.Length < 3)
        {
            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    //case TouchPhase.Began:
                        //break;
                    case TouchPhase.Ended:
                        gaitCommandFinish = true;
                        break;
                    case TouchPhase.Stationary:
                        gaitCommand = true;
                        break;
                    case TouchPhase.Moved:
                        if (touch.deltaPosition.x > 0 && Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y))
                        {
                            speedUp = true;
                        }
                        else if (touch.deltaPosition.x < 0 && Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y))
                        {
                            slowDown = true;
                        }
                        else if (touch.deltaPosition.y > 0 && Mathf.Abs(touch.deltaPosition.x) < Mathf.Abs(touch.deltaPosition.y))
                        {
                            turnRight = true;
                        }
                        else if (touch.deltaPosition.y < 0 && Mathf.Abs(touch.deltaPosition.x) < Mathf.Abs(touch.deltaPosition.y))
                        {
                            turnLeft = true;
                        }
                        break;
                    default:
                        break;
                }
            }
        }        

        if (Input.GetKeyDown(KeyCode.Space) || gaitCommand)
        {
            if (!gaitTimer.Running)
            {
                commandDone = 0;
                gaitTimer.Duration = timeToCommand;
                gaitTimer.Run();
            }
            if (!jumpTimer.Running)
            {
                jumpTimer.Duration = holdForJump;
                jumpTimer.Run();
            }
        }
        if (Input.GetKey(KeyCode.Space) || gaitCommand)
        {
            if (jumpTimer.Finished)
            {
                if (!jumpTimer.Check())
                {
                    horse.Jump();
                    jumpTimer.Duration = holdForJump;
                    jumpTimer.Run();
                }                
            }
        }
        else if (jumpTimer.Running)
        {
            jumpTimer.Abort();
        }
        if (Input.GetKeyUp(KeyCode.Space) || gaitCommandFinish)
        {
            if (gaitTimer.Running)
            {
                commandDone++;
            }
        }

        if (Input.GetKeyDown(KeyCode.W) || speedUp)
        {
            horse.SpeedUp();
        }
        if (Input.GetKeyDown(KeyCode.S) || slowDown)
        {
            horse.SlowDown();           
        }
        if (Input.GetKeyDown(KeyCode.D) || turnRight)
        {
            horse.TurnRight();
        }
        if (Input.GetKeyDown(KeyCode.A) || turnLeft)
        {
            horse.TurnLeft();
        }        
    }
}
