using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTools : MonoBehaviour
{
    [Header("Renderers")]
    public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    [Header("Sprites")]
    public List<Sprite> spriteVariants = new List<Sprite>();
    public List<Color> tints = new List<Color>();



    public void RandomizeSprites()
    {

        int count = spriteRenderers.Count;
        for (int i = 0; i < count; i++)
        {
            int randomSpriteIndex = Random.Range(0, spriteVariants.Count);
            int randomColorIndex = Random.Range(0, tints.Count);

            spriteRenderers[i].sprite = spriteVariants[randomSpriteIndex];
            spriteRenderers[i].color = tints[randomColorIndex];
        }
    }

}
