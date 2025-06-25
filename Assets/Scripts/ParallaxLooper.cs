using UnityEngine;

public class ParallaxLooper : MonoBehaviour
{
    [System.Serializable] // Allows custom class to be visible and editable in Unity Inspector
    public class ParallaxLayer
    {
        public Transform part0;
        public Transform part1;
        public float speed = 1f;
    }

    public ParallaxLayer[] layers; // Array of background layers
    public float globalSpeed = 1f; // Base speed, modified by each layer’s multiplier

    public float layerWidth = 80f;

    void Update()
    {
        foreach(var layer in layers)
        {
            float moveStep = globalSpeed * layer.speed * Time.deltaTime;

            layer.part0.position += Vector3.left * moveStep;
            layer.part1.position += Vector3.left * moveStep;

            if(layer.part0.position.x <= -layerWidth)
            {
                layer.part0.position += Vector3.right * layerWidth * 2f;
                SwapParts(layer);
            }

            if(layer.part1.position.x <= -layerWidth)
            {
                layer.part1.position += Vector3.right * layerWidth * 2f;
                SwapParts(layer);
            }
        }
    }

    static void SwapParts(ParallaxLayer layer)
    {
        var temp = layer.part0;
        layer.part0 = layer.part1;
        layer.part1 = temp;
    }
}