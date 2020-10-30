using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckTester : MonoBehaviour
{
    [SerializeField] List<AbilityCardData> abilityDeckConfig = new List<AbilityCardData>();
    [SerializeField] AbilityCardView abilityCardView = null;
    Deck<AbilityCard> abilityDeck = new Deck<AbilityCard>();
    Deck<AbilityCard> abilityDiscard = new Deck<AbilityCard>();

    Deck<AbilityCard> playerHand = new Deck<AbilityCard>();

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
        if(Input.GetKeyDown(KeyCode.Q))
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

    private void Draw()
    {
        AbilityCard newCard = abilityDeck.Draw(DeckPosition.Top);
        Debug.Log("Drew card: " + newCard.Name);
        playerHand.Add(newCard, DeckPosition.Top);

        abilityCardView.Display(newCard);
    }

    private void PrintPlayerHand()
    {
        for(int i = 0; i < playerHand.Count; i++)
        {
            Debug.Log("Player Hand Card: " + playerHand.GetCard(i).Name);
        }
    }

    void PlayTopCard()
    {
        AbilityCard targetCard = playerHand.TopItem;
        targetCard.Play();
        playerHand.Remove(playerHand.LastIndex);
        abilityDiscard.Add(targetCard);
        Debug.Log("Card added to discard: " + targetCard.Name);
    }
}
