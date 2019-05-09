using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splat : PooledObject {
    public enum SplatLoacation {
        Foreground,
        Background,
    }

    public Color backgroundTint;
    public float minSizeMod = 0.3f;
    public float maxSizeMod = 0.7f;
    public float lifeTime = 15f;

    public Sprite[] sprites;
    public Color[] startColors;

    private SplatLoacation splatLocation;
    private SpriteRenderer spriteRenderer;
    private AnimHelper animHelper;
    private Timer lifeTimer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animHelper = GetComponent<AnimHelper>();
    }

    private void Start()
    {
        lifeTimer = new Timer(lifeTime, PlayFade, true);
        animHelper.SetAnimEventAction(CleanUp);
    }

    public override void OnGet()
    {
        SetSprite();
        SetSize();
        SetRotation();
        SetStartColor();
        SetLocationProperties();
        base.OnGet();
    }

    public override void OnReturn()
    {
        base.OnReturn();
        SetStartColor();
    }

    private void Update()
    {
        if (lifeTimer != null && gameObject.activeSelf == true)
        {
            lifeTimer.UpdateClock();
        }

    }

    public void SetSplatVisualLayer(SplatLoacation splatLocation)
    {
        this.splatLocation = splatLocation;
    }

    private void SetSprite()
    {
        int randomIndex = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[randomIndex];
    }

    private void SetSize()
    {
        float sizeMod = Random.Range(minSizeMod, maxSizeMod);
        transform.localScale = Vector3.one * sizeMod;
    }

    private void SetRotation()
    {
        float randomRotation = Random.Range(-360f, 360f);
        transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);
    }

    private void SetStartColor()
    {
        int index = Random.Range(0, startColors.Length);
        spriteRenderer.color = startColors[index];
    }

    private void SetLocationProperties()
    {
        switch (splatLocation)
        {
            case SplatLoacation.Background:
                spriteRenderer.color = backgroundTint;
                spriteRenderer.sortingOrder = -1;
                break;

            case SplatLoacation.Foreground:
                spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                spriteRenderer.sortingOrder = 1;
                break;
        }
    }

    private void PlayFade()
    {
        animHelper.PlayAnimTrigger("Fade");
    }

    public void CleanUp()
    {
        GameManager.ReturnPooledObject(this);
    }
}
