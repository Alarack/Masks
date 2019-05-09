using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{

    public static InputBuffer Instance { get; protected set; }

    public List<Ability> bufferedAbilities = new List<Ability>();
    public float bufferDuration = 0.5f;

    private Timer bufferTimer;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }


    private void Start()
    {
        bufferTimer = new Timer(bufferDuration, PopBufferTimer, true);
    }

    private void Update()
    {
        if (bufferTimer != null && bufferedAbilities.Count > 0)
            bufferTimer.UpdateClock();
    }

    private void PopBufferTimer()
    {
        int count = bufferedAbilities.Count;
        for (int i = 0; i < count; i++)
        {
            bufferedAbilities[i].Activate();
        }

        bufferedAbilities.Clear();
    }

    public static void BufferAbility(Ability ability)
    {
        Instance.bufferedAbilities.AddUnique(ability);
    }
}
