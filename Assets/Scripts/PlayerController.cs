using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{

    public Vector2 moveValue;

    public float speed;
    private int count;
    private float startPos;
    private float newPos;
    private int numPickups = 8;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;


    void onMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);
        
        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);

    }

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        winText.text = "";
        SetCountText ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    private void SetCountText()
    {
        scoreText.text = "Score: " + count.ToString();
        if(count >= numPickups)
        {
            winText.text = "You win!";
        }
    }
}
