using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerDataManager : MonoSingleton<PlayerDataManager>
{
    public delegate void PlayerHandAddedHandler(int handId);
    public static event PlayerHandAddedHandler OnPlayerHandAdded;

    public delegate void PlayerHandChangedHandler(int handId);
    public static event PlayerHandChangedHandler OnPlayerHandChanged;

    public Hand GetHandById(int handId)
    {
        Hand hand = handData.hands.Find(x => x.saveId == handId);
        return hand;
    }

    public List<Hand> GetHandData()
    {
        return handData.hands;
    }

    public void SaveHandData(Hand hand)
    {
        int index = handData.hands.FindIndex(x => x.saveId == hand.saveId);
        if (index == -1)
        {
            handData.hands.Add(hand);
        }
        else
        {
            handData.hands[index] = hand;
        }

        isHandSave = true;
        SaveData();

        OnPlayerHandChanged?.Invoke(hand.saveId);
    }

    public bool IsHaveHand(int handId)
    {
        Hand hand = GetHandById(handId);
        if (hand == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void AddHand(Hand handData)
    {
        if (IsHaveHand(handData.saveId))
        {
            return;
        }

        SaveHandData(handData);

        OnPlayerHandAdded?.Invoke(handData.saveId);
    }

    public void DeleteHand(int id)
    {
        if (!IsHaveHand(id))
        {
            return;
        }
        else
        {
            int index = handData.hands.FindIndex(x => x.saveId == id);
            handData.hands.RemoveAt(index);
            isHandSave = true;
            SaveData();
        }
    }
}