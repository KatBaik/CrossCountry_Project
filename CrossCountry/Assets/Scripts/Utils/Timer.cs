using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
	float totalSeconds = 0;
	float elapsedSeconds = 0;
	bool running = false;
	bool started = false;
	bool check = false;

	public float Duration
	{
		set
		{
			if (!running)
			{
				this.totalSeconds = value;
			}
		}
	}

	public bool Finished
	{
		get { return started && !running; }
	}

	public bool Running
	{
		get { return running; }
	}

	public float ElapsedSeconds
	{
		get { return elapsedSeconds; }
	}

	void Update()
	{
		if (running)
		{
			elapsedSeconds += Time.deltaTime;
			if (elapsedSeconds >= totalSeconds)
			{
				running = false;
			}
		}
	}

	public void Run()
	{
		if (totalSeconds > 0)
		{
			started = true;
			running = true;
			check = false;
			elapsedSeconds = 0;
		}
	}

	public void Abort()
    {
		running = false;
		started = false;
    }

	public bool Check()
    {
		if (!check)
        {
			check = true;
			return false;
        }
		return check;
    }
}
