using UnityEngine;
 

// Player class which handles gets input for paddle movement 
public class Player : MonoBehaviour
{
    public KeyCode upKey;
    public KeyCode downKey;

    private Paddle paddle;

    // init paddle component
    void Start()
    {
        paddle = GetComponent<Paddle>();
    }
    
    // Check for user up or down input
    void Update()
    {
        float direction = 0;
        if (Input.GetKey(upKey))
        {
            direction = 1;
        }
        else if (Input.GetKey(downKey))
        {
            direction = -1;
        }
        
        paddle.MovePaddle(direction);
    }
}