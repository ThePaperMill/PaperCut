/*********************************
 * HighlightController.cs
 * Troy
 * Created 9/11/2015
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using UnityEngine;
using System.Collections;
//(not) using ActionSystem; (I guess)

public class HighlightController : MonoBehaviour
{
	public float Height = 1.0f;
	public float EasePercent = 1f;
	public float RotateSpeedDegrees = 40.0f;
    public float fullSize = 20.0f;
    public Vector3 GoToPos = new Vector3(0,0,0);
    public GameObject CurrObj;

    Vector3 growToPos;
    Vector3 shrinkToPos;
    Vector3 fullSizeVec;
    Vector3 goingTo = new Vector3(1,1,1);
	Vector3 lerpSpeed = new Vector3(0,0,0);
    float initSpeed = 0.0f;
    float speed = 0.0f;
	float lastDist = float.MaxValue;
	bool updating = false;
	bool nearApocynthion = false;
    bool newBorn = true;
    bool newBornMoved = false;
    bool dying = false;

    void Start()
	{
		// Sanity check:  EasePercent can't be above 100!
		if(EasePercent > 100.0f)
		{
			EasePercent = 100.0f;
		}

        fullSizeVec = new Vector3(fullSize, fullSize, fullSize);
        growToPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + Height, gameObject.transform.position.z);
        shrinkToPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }

	void Update()
	{
		// Always be rotating
		gameObject.transform.Rotate(new Vector3(0, RotateSpeedDegrees * Time.smoothDeltaTime, 0));

        // If we've matured to full size then we aren't newBorn
        if (gameObject.transform.localScale.x >= 19.9f)
        {
            gameObject.transform.localScale = new Vector3(20.0f, 20.0f, 20.0f);
            newBorn = false;
            newBornMoved = false;
        }

        // Else grow to maturity
        else if(newBorn && !dying)
        {
            Birth();
        }

        // Or kill if we're done with this Highlight
        if (dying)
        {
            Death();
        }

        // Move to a new locale if necessary
        else if (updating)
		{
			UpdatePos();
		}
	}

	void UpdatePos()
	{
		// Determine our destination if it has changed
		if(goingTo != GoToPos)
		{        
			goingTo = GoToPos;
            shrinkToPos = GoToPos;
            shrinkToPos.y -= Height;
            speed = EasePercent * Time.smoothDeltaTime;

			// Sanity check:  Don't go over 100% of the distance at any time!
			if(speed > 1.0f)
			{
				speed = 1.0f;
			}

			lerpSpeed = Vector3.Lerp(gameObject.transform.position, GoToPos, speed) - gameObject.transform.position;
		}

		// Always move to the requested location
		gameObject.transform.position += lerpSpeed;
		lastDist = Vector3.Distance(gameObject.transform.position, GoToPos) - speed;

		// If we get close, prepare to stop (necessary for if the game is running at a lower FPS)
		if(lastDist < 0.2f)
		{
			nearApocynthion = true;
		}

		// Stupidity Counter:  At a low FPS the Highlight can overshoot its mark, so reverse & lessen its speed
		if(lastDist > 0.2f && nearApocynthion)
		{
			lerpSpeed = Vector3.Lerp(gameObject.transform.position, GoToPos, speed) - gameObject.transform.position;
			nearApocynthion = false;
		}

		// Lock into place if we're close enough (we are NOT using Epsilon because it's too small of a distance)
		if(Vector3.Distance(gameObject.transform.position, GoToPos) < speed * 1.1f)
		{
			gameObject.transform.position = GoToPos;
			updating = false;
			lastDist = float.MaxValue;
			nearApocynthion = false;
		}
	}

    // I.E. grow the new Highlight to fullSize
    void Birth()
    {
        speed = EasePercent * Time.smoothDeltaTime;

        Vector3 lerpGrow = Vector3.Lerp(gameObject.transform.localScale, fullSizeVec, speed * 4) - gameObject.transform.localScale;
        gameObject.transform.localScale += lerpGrow;

        Vector3 lerpRise = Vector3.Lerp(gameObject.transform.position, growToPos, speed * 4) - gameObject.transform.position;
        gameObject.transform.position += lerpRise;
    }

    // I.E. shrink the old Highlight to nothing
    void Death()
    {
        speed = EasePercent * Time.smoothDeltaTime;

        Vector3 lerpGrow = Vector3.Lerp(gameObject.transform.localScale, Vector3.zero, speed * 4) - gameObject.transform.localScale;
        gameObject.transform.localScale += lerpGrow;

        // Kill when it gets small enough
        if (gameObject.transform.localScale.x <= 0.5f)
        {
            Destroy(gameObject);
        }

        Vector3 lerpRise = Vector3.Lerp(gameObject.transform.position, shrinkToPos, speed * 4) - gameObject.transform.position;
        gameObject.transform.position += lerpRise;
    }

    Vector3 QLerp(Vector3 initVec, Vector3 endVec, float speed)
    {
        return Vector3.Lerp(initVec, endVec, Mathf.SmoothStep(0.0f, 1.0f, speed));
    }

    public void setUp(bool IsUp)
	{
		updating = IsUp;
	}
	
	public bool getUp()
	{
		return updating;
    }
    public void setDeath(bool toSet)
    {
        dying = toSet;
    }
}