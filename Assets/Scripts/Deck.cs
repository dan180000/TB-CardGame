using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Deck <T> where T: Card
{
    List<T> cards = new List<T>();

    public event Action Emptied = delegate { };
    public event Action<T> CardAdded = delegate { };
    public event Action<T> CardRemoved = delegate { };

    public int Count => cards.Count;
    public T TopItem => cards[cards.Count - 1];
    public T BottomItem => cards[0];
    public bool IsEmpty => cards.Count == 0;
    public int LastIndex
    {
        get
        {
            if(cards.Count == 0)
            {
                return 0;
            }
            else
            {
                return cards.Count - 1;
            }
        }
    }

    //Deck Position Code
    private int GetIndexFromPosition(DeckPosition position)
    {
        int newPositionIndex = 0;
        if(cards.Count == 0)
        {
            newPositionIndex = 0;
        }
        if(position == DeckPosition.Top)
        {
            newPositionIndex = LastIndex;
        }
        else if (position == DeckPosition.Middle)
        {
            newPositionIndex = UnityEngine.Random.Range(0, LastIndex);
        }
        else if(position == DeckPosition.Bottom)
        {
            newPositionIndex = 0;
        }

        return newPositionIndex;
    }

    public void Add(T card, DeckPosition position = DeckPosition.Top)
    {
        if(card == null) { return; }
        int targetIndex = GetIndexFromPosition(position);
        if(targetIndex == LastIndex)
        {
            cards.Add(card);
        }
        else
        {
            cards.Insert(targetIndex, card);
        }
        CardAdded?.Invoke(card);
    }

    public void Add(List<T> cards, DeckPosition position = DeckPosition.Top)
    {
        int itemCount = cards.Count;
        for(int i = 0; i < itemCount; i++)
        {
            Add(cards[i], position);
        }
    }

    //Draw Code
    public T Draw(DeckPosition position = DeckPosition.Top)
    {
        if(IsEmpty)
        {
            Debug.LogWarning("Deck is empty!");
            return default;
        }
        int targetIndex = GetIndexFromPosition(position);

        T cardToRemove = cards[targetIndex];
        Remove(targetIndex);

        return cardToRemove;
    }

    public void Remove(int index)
    {
        if(IsEmpty)
        {
            Debug.LogWarning("Deck: Nothing to remove; deck is already empty");
        }
        else if(!IsIndexWithinListRange(index))
        {
            Debug.LogWarning("Deck: Nothing to remove; index out of range");
            return;
        }

        T removedItem = cards[index];
        cards.RemoveAt(index);

        CardRemoved?.Invoke(removedItem);

        if(cards.Count == 0)
        {
            Emptied?.Invoke();
        }
    }

    bool IsIndexWithinListRange(int index)
    {
        if(index >= 0 && index <= cards.Count - 1)
        {
            return true;
        }

        Debug.LogWarning("Deck: index outside of range;" + "index: " + index);
        return false;
    }

    //View Card
    public T GetCard(int index)
    {
        if(cards[index] != null)
        {
            return cards[index];
        }
        else
        {
            return default;
        }
    }

    //Random Shuffle Code
    public void Shuffle()
    {
        for(int currentIndex = Count - 1; currentIndex > 0; --currentIndex)
        {
            int randomIndex = UnityEngine.Random.Range(0, currentIndex + 1);
            T randomCard = cards[randomIndex];
            cards[randomIndex] = cards[currentIndex];
            cards[currentIndex] = randomCard;
        }
    }
}
