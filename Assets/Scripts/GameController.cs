using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private GameObject[] pickUps;
    private int closest = 0;
    public GameObject player;
    private float compareClosest;
    private LineRenderer lineRenderer;
    public TextMeshProUGUI closePickUp;
    public TextMeshProUGUI playerPosition;
    public TextMeshProUGUI playerVelocity;
    private Mode mode;

    private enum Mode
    {
        normal,
        debug,
        vision

    }

    // Start is called before the first frame update
    void Start()
    {
        mode = Mode.normal;
        pickUps = GameObject.FindGameObjectsWithTag("PickUp");
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.enabled = false;
        playerPosition.enabled = false;
        playerVelocity.enabled = false;
        closePickUp.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == Mode.debug)
        {
            DebugMode();
        }
        else
        {
            VisionMode();
        }


    }
    void OnSwitchMode()
    {
        print("E");
        if (mode == Mode.normal)
        {
            mode = Mode.debug;
            playerPosition.enabled = true;
            playerVelocity.enabled = true;
            closePickUp.enabled = true;
            lineRenderer.enabled = true;   
        }
        else if (mode == Mode.debug)
        {
            mode = Mode.vision;
        }
        else if (mode == Mode.vision)
        {
            mode = Mode.normal;
            lineRenderer.enabled = false;
            playerPosition.enabled = false;
            playerVelocity.enabled = false;
            closePickUp.enabled = false;
        }

    }



    private void DebugMode()
    {
        try
        {
            pickUps = GameObject.FindGameObjectsWithTag("PickUp");
            closest = 0;
            float closestDistance = Mathf.Infinity;
            for (int i = 0; i < pickUps.Length; i++)
            {
                pickUps[i].GetComponent<Renderer>().material.color = Color.white;
                compareClosest = Vector3.Distance(player.transform.position, pickUps[i].transform.position);
                if (compareClosest <= closestDistance)
                {
                    closest = i;
                    closestDistance = compareClosest;
                }
            }
            lineRenderer.SetPosition(0, pickUps[closest].transform.position);
            lineRenderer.SetPosition(1, player.transform.position);
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            closePickUp.text = "Pick Up Distance: " + closestDistance.ToString("0.00");
            pickUps[closest].GetComponent<Renderer>().material.color = Color.blue;

        }
        catch (System.Exception e) { }

    }

    private void VisionMode()
    {

    }
}
