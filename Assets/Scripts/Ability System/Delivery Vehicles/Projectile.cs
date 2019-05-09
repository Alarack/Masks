using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public bool testMode;
    public bool rotateTowardDireciton;
    public bool selfPropelled;

    [Header("VFX")]
    public GameObject particleTrail;
    public string projectileCorpse;
    public SpriteRenderer spriteRenderer;

    public StatCollectionData statTemplate;
    public StatCollection ProjectileStats { get; protected set; }

    public float RotateSpeed { get { return ProjectileStats.GetStatModifiedValue(BaseStat.StatType.RotateSpeed); } }

    public LayerMask Mask; /*{ get; protected set; }*/

    protected Effect parentEffect;
    protected ZoneInfo payloadZoneInfo;
    protected EffectZone activeZone;

    protected int penetrationCount = 0;

    //Movement
    protected Rigidbody2D myBody;
    protected float maxSpeed = 10f;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (testMode == false)
            return;

        payloadZoneInfo = new ZoneInfo();
        payloadZoneInfo.durationType = EffectZone.EffectZoneDuration.Instant;
        payloadZoneInfo.size = VisualEffectLoader.VisualEffectSize.Medium;
        payloadZoneInfo.shape = VisualEffectLoader.VisualEffectShape.Sphere;
        SetUpStats();
    }

    public void Initialize(Effect parent)
    {
        payloadZoneInfo = parent.effectZoneInfo;
        parentEffect = parent;
        Mask = parent.layerMask;
        SetUpStats();
    }

    protected void SetUpStats()
    {
        ProjectileStats = new StatCollection(gameObject, OnStatChanged, statTemplate);
        maxSpeed = ProjectileStats.GetStatModifiedValue(BaseStat.StatType.MoveSpeed);

        float life = ProjectileStats.GetStatModifiedValue(BaseStat.StatType.Lifetime);

        if (life > 0)
            Invoke("CleanUp", life);
    }



    protected virtual void OnTriggerEnter(Collider other)
    {
        DeployAfterLayerCheck(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DeployAfterLayerCheck(other.gameObject);
    }

    private void DeployAfterLayerCheck(GameObject other)
    {
        //if (LayerTools.IsLayerInMask(Mask, other.gameObject.layer) == false)
        //    return;

        Collider2D otherCollider = other.GetComponent<Collider2D>();
        if (otherCollider != null && otherCollider.isTrigger == true)
            return;

        //Debug.Log("Hit " + other.name);

        DeployEffectZone();

        HandlePenetration();
    }

    private void HandlePenetration()
    {
        float penetrationCount = ProjectileStats.GetStatModifiedValue(BaseStat.StatType.ProjectilePenetration);

        if (penetrationCount <= -1f)
            return;

        if(penetrationCount <= 0)
        {
            CleanUp();
        }
        else
        {
            StatAdjustmentManager.ApplyUntrackedStatMod(ProjectileStats, ProjectileStats, BaseStat.StatType.ProjectilePenetration, -1f);
        }
    }

    private void DeployEffectZone()
    {
        activeZone = EffectZoneFactory.CreateEffect(payloadZoneInfo, transform.position, Quaternion.identity);

        Transform originPoint = null;
        if (payloadZoneInfo.parentEffectToOrigin == true)
            originPoint = transform;

        activeZone.Initialize(parentEffect, Mask, originPoint);
    }


    public virtual void CleanUp()
    {
        CreateCorpse();
        Destroy(gameObject);
    }

    protected void CreateCorpse()
    {
        if (string.IsNullOrEmpty(projectileCorpse) == true)
            return;


        GameObject loadedPrefab = VisualEffectLoader.LoadVisualEffect("HitEffects", projectileCorpse);
        
        if(loadedPrefab == null)
        {
            Debug.LogError("Couldn't load a projectile impact");
            return;
        }

        Instantiate(loadedPrefab, transform.position, transform.rotation);
    }


    private void OnStatChanged(BaseStat.StatType stat, GameObject source)
    {
        if (stat == BaseStat.StatType.MoveSpeed)
            maxSpeed = ProjectileStats.GetStatModifiedValue(BaseStat.StatType.MoveSpeed);
    }

    private void FixedUpdate()
    {
        if(selfPropelled)
            myBody.velocity = transform.up * maxSpeed * Time.deltaTime;
    }

    private void Update()
    {
        if (rotateTowardDireciton && spriteRenderer != null)
        {
           transform.transform.up = Vector3.Slerp(transform.transform.up, myBody.velocity.normalized, Time.deltaTime * RotateSpeed);
        }
    }


}
