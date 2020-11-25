using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIDeck : MonoBehaviour
{
    public bool CanBeDamaged;
    public enum State
    {
        Player, Enemy, Noone
    }
    public State WhoseTurnisIt;

    public int BlockCards = 2;
    public int KickCards;
    public int PunchCards;
    public int RandomNumber;
    public Text EnemyUsedText;
    public AudioClip PunchSound;
    public AudioClip BlockSound;

    public ParticleSystem EnemyHurtPS;

    public bool IsDoingEnemyHurtPS;

    private float EnemyHurtPSTime = 1;

    private bool AlreadyDidEnemyHurtPS = false;

    private DeckTester DT;
    private float Delay = 1;
    private float RandomNumberDelay = 1;
    private bool IsDelaying;
    public bool AlreadyChoseRandomNumber;
    private float EnemyUsedTextDelay = 2;
    private bool CountdownEnemyUsedTextDelay = false;
    private float RandomGenerateNumber = 0;
    private bool AlreadyRegeneratedCards = false;
    private AudioSource AS;
    void Start()
    {
        DT = GetComponent<DeckTester>();
        EnemyUsedText.gameObject.SetActive(false);
        AS = GetComponent<AudioSource>();
    }

    void Update()
    {
        ChangeWhoseTurns();
        IfAllOutOfCards();
        PlayEnemyParticleSystems();
        if(WhoseTurnisIt == State.Player)
        {
            IfPlayersTurn();
        }

        if(WhoseTurnisIt == State.Enemy)
        {
            Delay -= Time.deltaTime;
        }

        if(Delay <= 0)
        {
            PlayCard();
        }

        if(IsDelaying == true)
        {
            RandomNumberDelay -= Time.deltaTime;
            if(RandomNumberDelay <= 0)
            {
                RandomNumber = Random.Range(0, 3);
            }
        }

        if(CountdownEnemyUsedTextDelay == true)
        {
            TriggerEnemyUsedTextDelay();
            if(EnemyUsedTextDelay <= 0)
            {
                CountdownEnemyUsedTextDelay = false;
                EnemyUsedText.gameObject.SetActive(false);
            }
        }
    }

    void PlayEnemyParticleSystems()
    {
        ParticleSystem EHPS = EnemyHurtPS.GetComponent(typeof(ParticleSystem)) as ParticleSystem;

        if (IsDoingEnemyHurtPS == true)
        {
            EnemyHurtPSTime -= Time.deltaTime;
            EnemyHurtPSTime -= Time.deltaTime;
            if (AlreadyDidEnemyHurtPS == false)
            {
                AlreadyDidEnemyHurtPS = true;
                EHPS.Play();
            }
        }

        if (EnemyHurtPSTime <= 0)
        {
            if (AlreadyDidEnemyHurtPS == true)
            {
                IsDoingEnemyHurtPS = false;
            }
        }

        if (IsDoingEnemyHurtPS == false)
        {
            EHPS.Stop();
            EnemyHurtPSTime = 1;
            AlreadyDidEnemyHurtPS = false;
        }
    }

    void IfAllOutOfCards()
    {
        if (BlockCards <= 0)
            if (KickCards <= 0)
                if (PunchCards <= 0)
                {
                    AlreadyChoseRandomNumber = false;

                    if (AlreadyChoseRandomNumber == false)
                    {
                        AlreadyChoseRandomNumber = true;
                        RegenerateCards();
                    }
                }
    }
    void TriggerEnemyUsedTextDelay()
    {
        EnemyUsedTextDelay -= Time.deltaTime;
    }

    void ChangeWhoseTurns()
    {
        if(DT.WhoseTurnisIt == DeckTester.State2.Player)
        {
            WhoseTurnisIt = State.Player;
        }

        if (DT.WhoseTurnisIt == DeckTester.State2.Enemy)
        {
            WhoseTurnisIt = State.Enemy;
        }

        if (DT.WhoseTurnisIt == DeckTester.State2.Noone)
        {
            WhoseTurnisIt = State.Noone;
        }
    }

    void PlayCard()
    {
        RandomNumber = Random.Range(0, 3);
        Delay = 1;

        if (RandomNumber == 0)
        {
            if(BlockCards > 0)
            {
                UseBlockCard();
                BlockCards -= 1;
                DT.ChangeToPlayerRound();
            }

            if (BlockCards < 1)
            {
                if (AlreadyChoseRandomNumber == false)
                {
                    AlreadyChoseRandomNumber = true;
                    RandomNumber = Random.Range(0, 3);
                    IsDelaying = true;
                }
            }
        }

        if (RandomNumber == 1)
        {
            if (PunchCards > 0)
            {
                UsePunchCard();
                PunchCards -= 1;
                DT.ChangeToPlayerRound();
            }

            if (PunchCards < 1)
            {
                if (AlreadyChoseRandomNumber == false)
                {
                    AlreadyChoseRandomNumber = true;
                    RandomNumber = Random.Range(0, 3);
                    IsDelaying = true;
                }
            }
        }

        if (RandomNumber == 2)
        {
            if (KickCards > 0)
            {
                UseKickCard();
                KickCards -= 1;
                DT.ChangeToPlayerRound();
            }

            if (KickCards < 1)
            {
                if (AlreadyChoseRandomNumber == false)
                {
                    AlreadyChoseRandomNumber = true;
                    RandomNumber = Random.Range(0, 3);
                    IsDelaying = true;
                }
            }
        }
    }

    void IfPlayersTurn()
    {
        
    }

    void UsePunchCard()
    {
        Text EUT = EnemyUsedText.GetComponent(typeof(Text)) as Text;
        EUT.text = "Enemy used Punch card!";
        EnemyUsedText.gameObject.SetActive(true);
        CountdownEnemyUsedTextDelay = true;
        EnemyUsedTextDelay = 2;
        AS.PlayOneShot(PunchSound);
        AS.pitch = 1;
        if (DT.CanBeDamaged == true)
        {
            DT.PlayerHealth -= 25;
            DT.IsDoingPlayerHurtPS = true;
        }
    }

    void UseKickCard()
    {
        Text EUT = EnemyUsedText.GetComponent(typeof(Text)) as Text;
        EUT.text = "Enemy used Kick card!";
        EnemyUsedText.gameObject.SetActive(true);
        CountdownEnemyUsedTextDelay = true;
        EnemyUsedTextDelay = 2;
        AS.PlayOneShot(PunchSound);
        AS.pitch = 0.5f;
        if (DT.CanBeDamaged == true)
        {
            DT.PlayerHealth -= 50;
            DT.IsDoingPlayerHurtPS = true;
        }
    }

    void UseBlockCard()
    {
        Text EUT = EnemyUsedText.GetComponent(typeof(Text)) as Text;
        CanBeDamaged = false;
        EUT.text = "Enemy used Block card!";
        EnemyUsedText.gameObject.SetActive(true);
        AS.PlayOneShot(BlockSound);
        AS.pitch = 1;
        CountdownEnemyUsedTextDelay = true;
        EnemyUsedTextDelay = 2;
    }

    void RegenerateCards()
    {
        BlockCards = 2;
        RandomGenerateNumber = Random.Range(0, 1);
        if(RandomGenerateNumber == 0)
        {
            KickCards = 2;
            PunchCards = 1;
        }

        if(RandomGenerateNumber == 1)
        {
            KickCards = 1;
            PunchCards = 2;
        }
    }
}
