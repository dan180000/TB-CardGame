using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DeckTester : MonoBehaviour
{
    [SerializeField] List<AbilityCardData> abilityDeckConfig = new List<AbilityCardData>();
    [SerializeField] AbilityCard CurCard;
    [SerializeField] AbilityCardView abilityCardView = null;
    Deck<AbilityCard> abilityDeck = new Deck<AbilityCard>();
    Deck<AbilityCard> abilityDiscard = new Deck<AbilityCard>();

    Deck<AbilityCard> playerHand = new Deck<AbilityCard>();
    public int RoundNumber;

    public GameObject[] Cardsets;
    public GameObject SelectedCard;
    public int CardNumber;
    public int TotalCards;
    public bool[] UsedCardSets;
    public int UsedCardSetsNumber;
    public AudioClip PunchSound;
    public AudioClip BlockSound;

    public string SelectedCardName;

    public GameObject Enemy;
    public GameObject Player;

    public float EnemyHealth = 100;
    public float PlayerHealth = 100;
    public bool CanBeDamaged;
    public enum State
    {
        NoOne, Player, Enemy
    }
    public State WhoWon;

    public enum State2
    {
        Player, Enemy, Noone
    }
    public State2 WhoseTurnisIt;
    
    public GameObject WinText;
    public GameObject LoseText;

    public Text EnemyHealthText;
    public Text PlayerHealthText;

    public Text RoundText;

    public Text EnemyIsThinkingText;

    public Text PlayerTurnText;

    public Text PlayerUsedText;

    public ParticleSystem PlayerHurtPS;

    private float PlayerUsedTextDelay = 2;
    private bool CountdownPlayerUsedTextDelay = false;

    public bool IsDoingPlayerHurtPS;

    private float PlayerHurtPSTime = 1;

    private bool AlreadyDidPlayerHurtPS = false;
    private float RestartTimer = 5;

    private AIDeck AID;
    private AudioSource AS;
    private TargetController TC;

    private void Start()
    {
        AID = GetComponent<AIDeck>();
        PlayerUsedText.gameObject.SetActive(false);
        SetupAbilityDeck();
        AS = GetComponent<AudioSource>();
    }

    private void SetupAbilityDeck()
    {
        foreach(AbilityCardData abilityData in abilityDeckConfig)
        {
            AbilityCard newAbilityCard = new AbilityCard(abilityData);
            abilityDeck.Add(newAbilityCard);
        }

        abilityDeck.Shuffle();
    }

    private void Update()
    {
        SelectCard(SelectedCard);
        SelectedCardNameChange();
        UpdateHealthTexts();
        ChangeRoundNumberText();
        PlayPlayerParticleSystems();
        IfAllCardsAreOut();

        if (CountdownPlayerUsedTextDelay == true)
        {
            TriggerCountdownPlayerUseTextDelay();
        }

        if (WhoseTurnisIt == State2.Player)
        {
            IfPlayerTurn();
        }

        if (WhoseTurnisIt == State2.Enemy)
        {
            IfEnemyTurn();
        }

        if (EnemyHealth <= 0)
        {
            WhoWon = State.Player;
        }

        if (PlayerHealth <= 0)
        {
            WhoWon = State.Enemy;
        }

        if(WhoWon == State.Enemy)
        {
            LoseMatch();
            RestartTimer -= Time.deltaTime;
        }

        if(WhoWon == State.Player)
        {
            WinMatch();
            RestartTimer -= Time.deltaTime;
        }

        if(RestartTimer <= 0)
        {
            RestartScene();
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Draw();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            PrintPlayerHand();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayTopCard();
        }
    }

    void PlayPlayerParticleSystems()
    {
        ParticleSystem PHPS = PlayerHurtPS.GetComponent(typeof(ParticleSystem)) as ParticleSystem;

        if (IsDoingPlayerHurtPS == true)
        {
            PlayerHurtPSTime -= Time.deltaTime;
            PlayerHurtPSTime -= Time.deltaTime;
            if (AlreadyDidPlayerHurtPS == false)
            {
                AlreadyDidPlayerHurtPS = true;
                PHPS.Play();
            }
        }


        if (PlayerHurtPSTime <= 0)
        {
            if (AlreadyDidPlayerHurtPS == true)
            {
                IsDoingPlayerHurtPS = false;
            }
        }


        if (IsDoingPlayerHurtPS == false)
        {
            PHPS.Stop();
            PlayerHurtPSTime = 1;
            AlreadyDidPlayerHurtPS = false;
        }
    }

    void IfAllCardsAreOut()
    {
        if(TotalCards == 0)
        if(CardNumber > 0)
        {
            TotalCards = -1;
        }

        if(TotalCards <= -1)
        {
            CardNumber = TotalCards;
        }
    }

    void TriggerCountdownPlayerUseTextDelay()
    {
        PlayerUsedTextDelay -= Time.deltaTime;
        if(PlayerUsedTextDelay <= 0)
        {
            CountdownPlayerUsedTextDelay = false;
            PlayerUsedText.gameObject.SetActive(false);
        }
    }
    void ChangeRoundNumberText()
    {
        Text roundText = RoundText.GetComponent(typeof(Text)) as Text;

        roundText.text = ("Round:" + "" + RoundNumber.ToString());
    }

    public void ChangeToEnemyRound()
    {
        WhoseTurnisIt = State2.Enemy;
        AID.CanBeDamaged = true;
    }

    public void ChangeToPlayerRound()
    {
        RoundNumber += 1;
        WhoseTurnisIt = State2.Player;
        CanBeDamaged = true;
    }

    void IfPlayerTurn()
    {
        EnemyIsThinkingText.gameObject.SetActive(false);
        PlayerTurnText.gameObject.SetActive(true);
    }

    void IfEnemyTurn()
    {
        EnemyIsThinkingText.gameObject.SetActive(true);
        PlayerTurnText.gameObject.SetActive(false);
    }
    public void Draw()
    {
        if(TotalCards < Cardsets.Length)
        {
            AbilityCard newCard = abilityDeck.Draw(DeckPosition.Middle);
            Debug.Log("Drew card: " + newCard.Name);
            abilityCardView.Display(newCard);
                CardNumber += 1;
                TotalCards += 1;
        }
        for (int i = 0; i < Cardsets.Length; i++)
        {
            if(CardNumber < Cardsets.Length)
            {
                CardSet CS = Cardsets[CardNumber].gameObject.GetComponent(typeof(CardSet)) as CardSet;

                abilityCardView.costTextUI = CS.Cost;
                abilityCardView.graphicUI = CS.image;
                abilityCardView.nameTextUI = CS.Name;

                Cardsets[CardNumber].SetActive(true);
            }
        }
    }

    private void PrintPlayerHand()
    {
        for(int i = 0; i < playerHand.Count; i++)
        {
            Debug.Log("Player Hand Card: " + playerHand.GetCard(i).Name);
        }
    }
 
    public void PlayTopCard()
    {
        if(WhoseTurnisIt == State2.Player)
        if (Enemy != null)
            {
            CardSet CS = SelectedCard.GetComponent(typeof(CardSet)) as CardSet;
            Debug.Log("Card added to discard: " + SelectedCard);
            Debug.Log("Played card: " + CS.Name.text);
                PlaySelectedCard();

                if (SelectedCardName == "Punch")
                {
                    UsePunchCard();
                }

                if (SelectedCardName == "Kick")
                {
                    UseKickCard();
                }

                if(SelectedCardName == "Block")
                {
                    UseBlockCard();
                }
            ChangeToEnemyRound();

            foreach (AbilityCardData abilityData in abilityDeckConfig)
            {
                AbilityCard newAbilityCard = new AbilityCard(abilityData);
                abilityDeck.Add(newAbilityCard);
            }

            abilityDeck.Shuffle();
        }
    }

    void UsePunchCard()
    {
        Text PUT = PlayerUsedText.GetComponent(typeof(Text)) as Text;
        PUT.text = "Player used Punch card!";
        CountdownPlayerUsedTextDelay = true;
        PlayerUsedTextDelay = 2;
        PlayerUsedText.gameObject.SetActive(true);
        AID.IsDoingEnemyHurtPS = true;
        TotalCards -= 1;
        AS.PlayOneShot(PunchSound);
        AS.pitch = 1;
        if (AID.CanBeDamaged == true)
        if(Enemy != null)
        {
            EnemyHealth -= 25;
        }
    }

    void UseKickCard()
    {
        Text PUT = PlayerUsedText.GetComponent(typeof(Text)) as Text;
        PUT.text = "Player used Kick card!";
        CountdownPlayerUsedTextDelay = true;
        PlayerUsedTextDelay = 2;
        PlayerUsedText.gameObject.SetActive(true);
        AID.IsDoingEnemyHurtPS = true;
        TotalCards -= 1;
        AS.PlayOneShot(PunchSound);
        AS.pitch = 0.5f;
        if (AID.CanBeDamaged == true)
            if (Enemy != null)
            {
            EnemyHealth -= 50;
        }
    }

    void UseBlockCard()
    {
        Text PUT = PlayerUsedText.GetComponent(typeof(Text)) as Text;
        PUT.text = "Player used Block card!";
        PlayerUsedTextDelay = 2;
        CountdownPlayerUsedTextDelay = true;
        TotalCards -= 1;
        AS.PlayOneShot(BlockSound);
        AS.pitch = 1;
        PlayerUsedText.gameObject.SetActive(true);
        CanBeDamaged = false;
    }

    public void SelectCard(GameObject card)
    {
        SelectedCard = card;
        for (int i = 0; i < Cardsets.Length; i++)
        {
            if(SelectedCard == Cardsets[i])
            {
                Debug.Log(SelectedCard);
            }
        }
    }

    private void PlaySelectedCard()
    {
        CardSet CS = SelectedCard.GetComponent(typeof(CardSet)) as CardSet;

        SelectedCard.SetActive(false);
        SelectedCard = null;
        CS.ResetSettings();
    }

    public void KillPlayer()
    {
        PlayerHealth = 0;
    }

    private void WinMatch()
    {
        Rigidbody ERB = Enemy.GetComponent(typeof(Rigidbody)) as Rigidbody;
        WinText.SetActive(true);
        WhoseTurnisIt = State2.Noone;

        Enemy.transform.rotation = Quaternion.Euler(0, 0, 90);
        ERB.isKinematic = false;
    }

    private void LoseMatch()
    {
        Rigidbody PRB = Player.GetComponent(typeof(Rigidbody)) as Rigidbody;
        LoseText.SetActive(true);
        WhoseTurnisIt = State2.Noone;

        Player.transform.rotation = Quaternion.Euler(0, 0, 90);
        PRB.isKinematic = false;
    }

    private void SelectedCardNameChange()
    {
        if (SelectedCard != null)
        {
            CardSet CS = SelectedCard.GetComponent(typeof(CardSet)) as CardSet;
            if (CS.Name.text == "Punch")
            {
                SelectedCardName = "Punch";
            }

            if (CS.Name.text == "Kick")
            {
                SelectedCardName = "Kick";
            }

            if (CS.Name.text == "Block")
            {
                SelectedCardName = "Block";
            }

            if (SelectedCard == null)
            {
                SelectedCardName = "";
            }
        }
    }

    private void UpdateHealthTexts()
    {
        Text EHealthText = EnemyHealthText.GetComponent(typeof(Text)) as Text;
        Text PHealthText = PlayerHealthText.GetComponent(typeof(Text)) as Text;

        EHealthText.text = EnemyHealth.ToString();
        PHealthText.text = PlayerHealth.ToString();
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(Application.loadedLevel);
    }
}
