using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public float horizontalScrollSpeed = 0.25f;
    public float verticalScrollSpeed = 0.25f;
 
    public Renderer renderer;
    int stepX = 0;
    int stepY = 0;
    public int maxStepX = 5;
    public int maxStepY = 3;

    private void Start() {
        Application.targetFrameRate = 60;
    }
 
    public void FixedUpdate()
    {
        // float verticalOffset = Time.time * verticalScrollSpeed;
        // float horizontalOffset = Time.time * horizontalScrollSpeed;
        
        float verticalOffset = verticalScrollSpeed*stepX;
        float horizontalOffset = horizontalScrollSpeed*stepY;
        renderer.material.mainTextureOffset = new Vector2(horizontalOffset, verticalOffset);

        stepX++;
        stepY++;

        if(stepX>maxStepX)
            stepX = 0;

        if(stepY>maxStepY)
            stepY = 0;
    }
}
