using Leap;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct Gesture
{
    public string name;
    public Vector3[] fingerPositions;
}

public class gestureDetector : MonoBehaviour
{
    public string currentGestureName;

    public float tolerance;
    public Vector3[] leftHandPos = new Vector3[15];
    public Vector3[] rightHandPos = new Vector3[15];
    public bool saveActive = false;

    public List<Gesture> gestureList;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && saveActive)
        {
            Save();
        }

        //left hand
        try
        {
            var leftHand = GameObject.Find("RigidRoundHand_L");
            LeftHandPos(leftHand);
            SetCurrentGesture(leftHand);
            DrawGestureName();
        }
        catch
        {}

    }

    void DrawGestureName()
    {
        TextMeshPro leftHand = GameObject.Find("LeftGestureName").GetComponent<TextMeshPro>();
        leftHand.text = currentGestureName;
    }

    void SetCurrentGesture(GameObject leftHand)
    {
        foreach (Gesture gesture in gestureList)
        {
            bool tooFar = false;
            float totalDiff = 0;

            for (int i = 0; i < 15; i++)
            {
                Vector3 thisBone = leftHand.transform.InverseTransformPoint(leftHandPos[i]);
                float difference = Vector3.Distance(thisBone, gesture.fingerPositions[i]);

                if (difference > tolerance)
                {
                    tooFar = true;
                    break;
                }

                totalDiff += difference;
            }

            if (!tooFar && totalDiff < tolerance)
            {
                currentGestureName = gesture.name;
            }
            else
            {
                currentGestureName = "Not Sure";
            }
        }
    }

    void LeftHandPos(GameObject leftHand)
    {
        int counter = 0;
        for (int i = 0; i < 5; i++)
        {
            Transform finger = leftHand.transform.GetChild(i);

            for (int j = 0; j < 3; j++)
            {
                leftHandPos[counter] = finger.GetChild(j).transform.position;
                counter++;
            }
        }
    }

    void Save()
    {
        var tempGesture = new Gesture();
        tempGesture.name = "Temp name";

       
        try
        {
            var leftHand = GameObject.Find("RigidRoundHand_L");
            Vector3[] tempPos = new Vector3[15];
            int counter = 0;

            for (int i = 0; i < 5; i++)
            {
                Transform finger = leftHand.transform.GetChild(i);

                for (int j = 0; j < 3; j++)
                {
                    tempPos[counter] = finger.GetChild(j).transform.position;
                    counter++;
                }
            }

            tempGesture.fingerPositions = tempPos;

            gestureList.Add(tempGesture);
        }
        catch (System.Exception)
        {

        }
       
    }
}
