using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowChanger : MonoBehaviour
{
    [SerializeField] private GameObject titleFlow;
    [SerializeField] private GameObject titleFlowSpawnPoint;

    [SerializeField] private GameObject gameFlow;
    [SerializeField] private GameObject gameFlowSpawnPoint;

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject liquidDeleter;

    private Flow currentFlow;
    public Flow CurrentFlow
    {
        get
        {
            return currentFlow;
        }
        set
        {
            if (currentFlow == value) return;

            FlowChange(value);
            currentFlow = value;
        }
    }

    private void Start()
    {
        currentFlow = Flow.None;
        CurrentFlow = Flow.Title;
    }

    private void FlowChange(Flow changedFlow)
    {
        //LiquidDelete();
        switch (changedFlow)
        {
            case Flow.Title:
                titleFlow.SetActive(true);
                //gameFlow.SetActive(false);
                //player.transform.position = titleFlowSpawnPoint.transform.position;
                //player.transform.rotation = titleFlowSpawnPoint.transform.rotation;
                break;
            case Flow.Game:
                titleFlow.SetActive(false);
                //gameFlow.SetActive(true);
                //player.transform.position = gameFlowSpawnPoint.transform.position;
                //player.transform.rotation = gameFlowSpawnPoint.transform.rotation;
                //player.transform.localScale = Vector3.one;
                break;
            default:
                Debug.Assert(true, "FlowChanger에서 지정되지않은 Flow를 입력하였습니다.");
                break;
        }
    }

    public void LiquidDelete()
    {
        StartCoroutine(LiquidDeleter());
    }

    IEnumerator LiquidDeleter()
    {
        liquidDeleter.SetActive(true);
        yield return new WaitForSecondsRealtime(0.1f);
        liquidDeleter.SetActive(false);
    }
}

public enum Flow
{
    Title,Game,None
}