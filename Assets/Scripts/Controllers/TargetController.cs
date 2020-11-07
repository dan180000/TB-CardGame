using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public static ITargetable CurrentTarget;
    [SerializeField] Creature objectToTarget = null;
    public DeckTester deckTesterObject;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            TargetEntity();
        }
    }

    public void TargetEntity()
    {
        ITargetable possibleTarget = objectToTarget.GetComponent<ITargetable>();
        DeckTester DT = deckTesterObject.GetComponent<DeckTester>();
        if (possibleTarget != null)
        {
            Debug.Log("New target acquired!");
            CurrentTarget = possibleTarget;
            DT.Enemy = GameObject.FindGameObjectWithTag("Enemy");
            objectToTarget.Target();
        }
    }
}
