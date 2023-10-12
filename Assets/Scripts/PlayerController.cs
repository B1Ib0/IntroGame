using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerController : MonoBehaviour

{

    public Vector2 moveValue;
    public Vector3 oldPosition;
    private Vector3 newPosition;
    public float speed;
    private int count;
    private int numPickups = 4;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI playerPosition;
    public TextMeshProUGUI playerVelocity;


    private GameObject[] pickUps;
    private int closest = 0;
    private float compareClosest;
    private LineRenderer lineRenderer;
    public TextMeshProUGUI closePickUp;


    private Mode mode;

    private enum Mode
    {
        normal,
        debug,
        vision

    }

    private void Start()
    {

        oldPosition = transform.position;
        count = 0;
        winText.text = "";
        setCountText();

        mode = Mode.normal;
        pickUps = GameObject.FindGameObjectsWithTag("PickUp");
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.enabled = false;
        playerPosition.enabled = false;
        playerVelocity.enabled = false;
        closePickUp.enabled = false;
    }


    void OnSwitchMode()
    {
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
            for (int i = 0; i < pickUps.Length; i++)
            {
                pickUps[i].GetComponent<Renderer>().material.color = Color.white;
            }
            lineRenderer.enabled = false;
            playerPosition.enabled = false;
            playerVelocity.enabled = false;
            closePickUp.enabled = false;
        }

    }

    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();

    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);
        newPosition = transform.position;
        playerVelocity.text = "Velocity: " + ((newPosition - oldPosition) / Time.deltaTime).magnitude.ToString("0.00");
        playerPosition.text = "Position: "  + newPosition.ToString("0.00");
        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);

        if (mode == Mode.debug)
        {
            DebugMode();
        }
        else if (mode == Mode.vision)
        {
            VisionMode();
        }

        oldPosition = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            count++;
            other.gameObject.SetActive(false);
            setCountText();
        }
    }

    private void setCountText()
    {
        scoreText.text = "Score: " + count.ToString();
        if (count >= numPickups)
        {
            winText.text = "You Win!";
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
                compareClosest = Vector3.Distance(transform.position, pickUps[i].transform.position);
                if (compareClosest <= closestDistance)
                {
                    closest = i;
                    closestDistance = compareClosest;
                }
            }
            lineRenderer.SetPosition(0, pickUps[closest].transform.position);
            lineRenderer.SetPosition(1, transform.position);

            closePickUp.text = "Pick Up Distance: " + closestDistance.ToString("0.00");
            pickUps[closest].GetComponent<Renderer>().material.color = Color.blue;

        }
        catch (System.Exception e)
        {
            lineRenderer.enabled = false;

        }

    }

    private float CountDistance(Vector3 a, Vector3 b)
    {
        float distance = 0;
        distance = (float)(Math.Pow(Math.Abs(a.x * b.z - b.x * a.z), 0.5) / Math.Pow(Math.Pow(a.x, 2) + Math.Pow(a.z, 2), 0.5));
        return distance;
    }

    private void VisionMode()
    {
        Vector3 towards = (transform.position - oldPosition) * 50;
        try
        {
            pickUps = GameObject.FindGameObjectsWithTag("PickUp");


            float minimum = Mathf.Infinity;
            closest = 0;
            for (int i = 0; i < pickUps.Length; i++)
            {
                pickUps[i].GetComponent<Renderer>().material.color = Color.white;
                if (CountDistance(towards, pickUps[i].transform.position - transform.position) <= minimum && (towards.x * (pickUps[i].transform.position - transform.position).x + towards.z * (pickUps[i].transform.position - transform.position).z) >= 0)
                {
                    minimum = CountDistance(towards, pickUps[i].transform.position - transform.position);
                    closest = i;
                }
            }

            pickUps[closest].GetComponent<Renderer>().material.color = Color.green;
            pickUps[closest].transform.LookAt(transform.position);




        }
        catch (Exception e) { }
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + towards);
    }


}

