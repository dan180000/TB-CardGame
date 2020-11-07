using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckTester : MonoBehaviour
{
    [SerializeField] List<AbilityCardData> abilityDeckConfig = new List<AbilityCardData>();
    [SerializeField] AbilityCard CurCard;
    [SerializeField] AbilityCardView abilityCardView = null;
    Deck<AbilityCard> abilityDeck = new Deck<AbilityCard>();
    Deck<AbilityCard> abilityDiscard = new Deck<AbilityCard>();

    Deck<AbilityCard> playerHand = new Deck<AbilityCard>();

    public GameObject[] Cardsets;
    public GameObject SelectedCard;
    public string SelectedCardName;
    public GameObject Enemy;
    public int CardNumber;
    public float EnemyHealth = 100;
    public float PlayerHealth = 100;
    public enum State
    {
        NoOne, Player, Enemy
    }
    public State WhoWon;
    
    public GameObject WinText;
    public GameObject LoseText;
    public Text EnemyHealthText;
    public Text PlayerHealthText;

    private void Start()
    {
        SetupAbilityDeck();
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
        }

        if(WhoWon == State.Player)
        {
            WinMatch();
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
    public void Draw()
    {
        if (CardNumber < Cardsets.Length)
        {
            AbilityCard newCard = abilityDeck.Draw(DeckPosition.Middle);
            Debug.Log("Drew card: " + newCard.Name);
            abilityCardView.Display(newCard);
            CardNumber += 1;
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
        CardSet CS = SelectedCard.GetComponent(typeof(CardSet)) as CardSet;
        Debug.Log("Card added to discard: " + SelectedCard);
        Debug.Log("Played card: " + CS.Name.text);
        PlaySelectedCard();

            if(SelectedCardName == "Punch")
            {
                EnemyHealth -= 25;
            }

            if (SelectedCardName == "Kick")
            {
                EnemyHealth -= 50;
            }

        foreach (AbilityCardData abilityData in abilityDeckConfig)
        {
            AbilityCard newAbilityCard = new AbilityCard(abilityData);
            abilityDeck.Add(newAbilityCard);
        }

        abilityDeck.Shuffle();
    }

    public void SelectCard(GameObject card)
    {
        SelectedCard = card;
    }

    private void PlaySelectedCard()
    {
            SelectedCard.SetActive(false);
            SelectedCard = null;
    }

    public void KillPlayer()
    {
        PlayerHealth = 0;
    }

    private void WinMatch()
    {
        WinText.SetActive(true);
    }

    private void LoseMatch()
    {
        LoseText.SetActive(true);
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
}
