using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    //Variaveis e Headers//
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish; 
    public Vector3 swinLimits = new Vector3(5, 5, 5); //Variavel das Boundaries//
    public Vector3 goalPos;
    [Header("Configurações do Cardume")] 
    [Range(0.0f, 5.0f)] 
    public float minSpeed;
    [Range(0.0f, 5.0f)] 
    public float maxSpeed;
    [Range(1.0f, 10.0f)] 
    public float neighbourDistance;
    [Range(0.0f, 5.0f)] 
    public float rotationSpeed;

    void Start() 
    { 
        //Colocando os peixes no array//
        allFish = new GameObject[numFish];
        //Para cada peixe no array, Instancia um peixe dentro dos limites do grupo//
        for (int i = 0; i < numFish; i++) 
        { 
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z)); 
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity); 
            allFish[i].GetComponent<Flock>().myManager = this;
            goalPos = this.transform.position;
        }
    }
    void Update() 
    {
        //criando a posição alvo do grupo dentro das cordenadas estabelecidas//
        goalPos= this.transform.position;
        if(Random.Range(0,100)<10)
        {
            goalPos= this.transform.position+ new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                Random.Range(-swinLimits.y, swinLimits.y),
                Random.Range(-swinLimits.z, swinLimits.z));
        }
    }
}
