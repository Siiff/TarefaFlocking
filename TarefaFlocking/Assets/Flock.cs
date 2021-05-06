using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    //Instanciando variaveis//
    public FlockManager myManager;
    public float speed;
    bool turning = false;
    private void Start()
    {
        //Atribuindo um valor aleatório a speed entre o minimo e o maximo//
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }
    void Update()
    {
        //Definindo as Boundaries//
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);

        //Raycast para enxergar colisores//
        RaycastHit hit = new RaycastHit(); 
        Vector3 direction = myManager.transform.position - transform.position;

        if (!b.Contains(transform.position))
        {
            //Se for true, ele vira e vaga em direção ao centro do grupo//
            turning = true;
            direction = myManager.transform.position - transform.position;
        }
        //caso vá bater, vira//
        else if(Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else
        {
            turning = false;
        }
        if (turning)
        {
            //fazendo o sistema de rotação//
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10) speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            if (Random.Range(0, 100) < 20) ApplyRules();
        }
            transform.Translate(0, 0, Time.deltaTime * speed);
    }
    //Metodo para movimentação "Realista" do cardume, evitar colisão, velocidade, formação do grupo e etc//
    void ApplyRules()
    {
        GameObject[] gos;
        gos = myManager.allFish;
        //Controle de velocidade e movimentação//
        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;
        //Para cada peixe, atribui as informações//
        //Fazendo a movimentação de entrada no cardume//
        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if (nDistance <= myManager.neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;
                    if (nDistance < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }
                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        //Se o grupo for maior q 0, começa a formar a movimentação circular do cardume//
        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
            speed = gSpeed / groupSize;
            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
            }

        }
    }
}
