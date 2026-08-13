using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public EconomyManager economyManager;
    public UIManager UIManager;
    public SettlementListGenerator settlementListGenerator;
    public Timber Timber;
    public StoneQuarry StoneQuarry;
    public Mine Mine;
    public CastleButton Castle;
    public float SettlementProduction;
    public float PlayerStrenght;
    public int WorkerSpeed;

    public float addedStrength;
    
    private void Start()
    {
        InvokeRepeating(nameof(StartWorkerFlow), 0f, 1.5f);
        InvokeRepeating(nameof(StrenghtCalculation), 0f, 1f);
    }
    public void StrenghtCalculation() 
    {
        Castle.Strength = Castle.Level + addedStrength;
    }
    public float CheckStrength(float clonestrength) 
    {
        float Ratio = 0.5f + (Castle.Strength - clonestrength) / (2 * (Castle.Strength + clonestrength));
        return MathF.Round(Ratio * 100f, 2);
    }
    public float Roll() 
    {
        float roll = UnityEngine.Random.Range(0, 100);
        return roll;
    }
    public void StartWorkerFlow() 
    {
        if (Timber.Level != 0) 
        {
            GameObject Workerclone = Instantiate(Timber.Worker);
            Workerclone.transform.position = Timber.transform.position;
            Vector3 pointB = new Vector3(-0.04f, 0, 0);
            Vector3 pointC = new Vector3(0, 0, 5);
            StartCoroutine(MoveWorker(Workerclone, pointB, pointC, Workerclone.transform.position, Vector3.left));
        }
        if (StoneQuarry.Level != 0)
        {
            GameObject Workerclone = Instantiate(StoneQuarry.Worker);
            Workerclone.transform.position = StoneQuarry.transform.position;
            Vector3 pointB = new Vector3(-0.04f, 0, 0);
            Vector3 pointC = new Vector3(0, 0, 5);
            StartCoroutine(MoveWorker(Workerclone, pointB, pointC, Workerclone.transform.position, Vector3.right));
        }
        if (Mine.Level != 0)
        {
            GameObject Workerclone = Instantiate(Mine.Worker);
            Workerclone.transform.position = Mine.transform.position;
            Vector3 pointB = new Vector3(-0.04f, 0, 0);
            Vector3 pointC = new Vector3(0, 0, 5);
            StartCoroutine(MoveWorker(Workerclone, pointB, pointC, Workerclone.transform.position, Vector3.right));
        }
    }
    private IEnumerator MoveWorker(GameObject worker, Vector3 pointB, Vector3 pointC, Vector3 startPosition, Vector3 rotationDirection)
    {
        Vector3 intermediatePosition = new Vector3(pointB.x, startPosition.y, startPosition.z);
        while (Vector3.Distance(worker.transform.position, intermediatePosition) > 0.01f)
        {
            worker.transform.position = Vector3.MoveTowards(worker.transform.position, intermediatePosition, WorkerSpeed * Time.deltaTime);
            worker.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            yield return null;
        }
        if (Mathf.Abs(pointC.z - startPosition.z) > 0.01f)
        {
            Vector3 finalPosition = new Vector3(pointC.x, startPosition.y, pointC.z);
            while (Vector3.Distance(worker.transform.position, finalPosition) > 0.01f)
            {
                worker.transform.position = Vector3.MoveTowards(worker.transform.position, finalPosition, WorkerSpeed * Time.deltaTime);
                worker.transform.rotation = Quaternion.LookRotation(rotationDirection);
                yield return null;
            }
        }
        Destroy(worker);
    }
}