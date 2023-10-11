using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

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

    private void Start()
    {

        oldPosition = transform.position;
        count = 0;
        winText.text = "";
        setCountText();
    }



    void OnMove(InputValue value)
    {
        print("E");
        moveValue = value.Get<Vector2>();

    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);
        newPosition = transform.position;
        playerVelocity.text = "Velocity: " + ((newPosition - oldPosition) / Time.deltaTime).magnitude.ToString("0.00");
        oldPosition = newPosition;
        playerPosition.text = "Position: "  + newPosition.ToString("0.00");
        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);
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

}
