using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    II II;

    int coordinateX;
    int coordinateY;

    public GameObject creature;
    GameObject asv;
    // Start is called before the first frame update
    void Start()
    {
        coordinateX = 10;
        coordinateY = 10;
        II = new RandomII(coordinateX, coordinateY, 0, 10, 10);
        asv = Instantiate(creature, new Vector3(coordinateX, coordinateY, 0), Quaternion.identity);
        Debug.Log(II.condition);
    }

    // Update is called once per frame
    void Update()
    {
        II.ChooseMove(GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>().world);
        asv.transform.position = new Vector3(II.coordinateX, II.coordinateY, 0);
    }
    public void ShowCreature()
    {

    }
    public void RandomPlace(int maxX, int maxY)
    {

    }
}
