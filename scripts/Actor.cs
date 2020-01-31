using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  enum Action
    {
        Walk, Shotgan, Amo, Target
    }

public class Actor : MonoBehaviour
{

    
    AudioSource audioSource;
    public AudioClip soundShotgan; 
    public Team team = Team.player;
    public int startDirection = 30;
    public float speed = 0.1f;
    public Dictionary<string, Hex> hexRangeMove;

    public Dictionary<string,Hex> hexRangeDirection; 
    public int movrRangeInHex = 2;
    public Vector3 target;
    public int actionPoint = 2;
    public GameObject actionPointSpher1;
    public GameObject actionPointSpher2;
    public Hex start;
    public Layout layot;
    public Transform mapHolder;
    public Dictionary<string, Hex> all;
    public Dictionary<Hex, GameObject> allHexGameObject;
    public GameObject gameMenager;
    public List<Hex> curentPath;
    public bool isWalkNow = false;
    public Animator animator;
    public  GameMenageraScript menagerScript; 
    public Renderer Shotgun;

    public bool isChosenDirection = false; 
    public bool isDoneAllInThisTurn = false; 

    public Action curentAction = Action.Walk;
    public Action lastAction = Action.Walk;

    
    




    // Start is called before the first frame update
    public void Start()
    {
        gameMenager = GameObject.FindGameObjectWithTag("gameMenager");
         menagerScript=gameMenager.GetComponent<GameMenageraScript>();
        layot = gameMenager.GetComponent<GameMenageraScript>().layot;
        mapHolder = gameMenager.GetComponent<GameMenageraScript>().mapHolder;
        all = gameMenager.GetComponent<GameMenageraScript>().AllHex;
        hexRangeMove = new Dictionary<string, Hex>();
        allHexGameObject = gameMenager.GetComponent<GameMenageraScript>().AllHexGameObject;
        animator.SetBool("isWalkNow", false);
        audioSource = GetComponent<AudioSource>();
    }
        // Update is called once per frame
    void Update()
    {
       
    }

    public void playShotgun(){
        audioSource.PlayOneShot(soundShotgan);
    }

    public void RestoreActionPoint(){
        actionPoint = 2; 
        isDoneAllInThisTurn = false; 
        isChosenDirection = false;
    }
    public void DisplayCurentRange()
    {
        foreach (var item in allHexGameObject)
        {
            item.Value.GetComponent<MapTile>().DeactivRange();
        }

        foreach (var item in hexRangeMove)
        {
            //Debug.Log(item.Key.ToString());
            if (all.ContainsKey(item.Key.ToString()))
            {
                Hex curent = all[item.Key.ToString()];
                //Debug.Log("curent " + curent);
                allHexGameObject[curent].GetComponent<MapTile>().activeCurentRange();
            }
        }
    }

    public void checkActionPoint()
    {
        Debug.Log(actionPoint);
        Debug.Log("sprawdzam : ");
        if (actionPoint <= 0)
        {
            DeactivAllArrow();
            foreach (var item in allHexGameObject)
            {
                item.Value.GetComponent<MapTile>().DeactivRange();
            }
            curentPath.Clear();
            return;
        }
        if (actionPoint > 0)
        {
            DeactivAllArrow();
            updateAfterSelect();
        }


    }
    private void OnMouseDown()
    {
        //checkActionPoint();
        //updateAfterSelect();
        setMeAsCurentActor();
        menagerScript.WhatIClikNow[GameEntity.Actor] = true;
        //gameMenager.GetComponent<GameMenageraScript>().updateCurentActor(this);
    }

    public void setMeAsCurentActor(){
         gameMenager.GetComponent<GameMenageraScript>().updateCurentActor(this);
    }



    public void DeactivAllArrow()
    {
        foreach (var item in allHexGameObject)
        {
            item.Value.GetComponent<MapTile>().DeactivArrow();
            item.Value.GetComponent<MapTile>().DeactivRange();

        }
    }

    IEnumerator TravelWithCurentPath(List<Hex> path)
    {
        Hex[] pathArray = path.ToArray();
        Vector3 target = layot.HexToVector3(path[pathArray.Length - 1]);
        Vector3 curentTarget = layot.HexToVector3(pathArray[0]);
        Vector3 start = transform.position;
        int licznik = 0;
        isWalkNow = true;
        float curentDistans;
        float distanceToTarget;

        //Debug.Log("UWAGA !!!!!!!");
        distanceToTarget = Vector3.Distance(start, target);
        while (distanceToTarget >= 0.1)
        {

            start = transform.position;
            curentDistans = Vector3.Distance(start, curentTarget);
            distanceToTarget = Vector3.Distance(start, target);
            if (curentDistans >= 0.1)
            {
                transform.LookAt(curentTarget);
                transform.position = Vector3.MoveTowards(transform.position, curentTarget, speed * Time.deltaTime);

            }

            if (curentDistans <= 0.1 && licznik < pathArray.Length - 1)
            {
                //Debug.Log("sart == curent and licznik = " + licznik);
                licznik += 1;
                //start = curentTarget;
                curentTarget = layot.HexToVector3(pathArray[licznik]);
                //transform.position = Vector3.MoveTowards(transform.position, curentTarget, speed * Time.deltaTime);
            }


            yield return null;
        }


        //Debug.Log("end");
        transform.position = target;
        bool isHaveShotgunEqip = animator.GetBool("Equipshotgun");
            if(isHaveShotgunEqip == true){
                animator.SetBool("shotgunWalk", false);
            }else{
                 animator.SetBool("isWalkNow", false);
            }
        updateActionPoint(-1);


        isWalkNow = false;

        checkActionPoint();
        updateAfterSelect();
        //gameMenager.GetComponent<GameMenageraScript>().updateCurentActor(this);



    }

    public void updateActionPoint(int ile)
    {
        actionPoint = actionPoint + ile;

        if (actionPoint == 2)
        {
            actionPointSpher1.GetComponent<MeshRenderer>().enabled = true;
            actionPointSpher2.GetComponent<MeshRenderer>().enabled = true;
        }
        if (actionPoint == 1)
        {
            actionPointSpher1.GetComponent<MeshRenderer>().enabled = false;
            actionPointSpher2.GetComponent<MeshRenderer>().enabled = true;
        }
        if (actionPoint == 0)
        {
            actionPointSpher1.GetComponent<MeshRenderer>().enabled = false;
            actionPointSpher2.GetComponent<MeshRenderer>().enabled = false;
        }
        if (actionPoint < 0)
        {
            actionPoint = 0;
            //Debug.Log("zla ilosc punktow akcji akcji"); 
        }

        if (actionPoint > 2)
        {
            actionPoint = 2;
            //Debug.Log("zla ilosc punktow akcji akcji");
        }
    }

    public Dictionary<string, Hex> CorectRangeToWalkableAndIsInMap()
    {

        Dictionary<string, Hex> corectRange = new Dictionary<string, Hex>();
        List<Hex> gameo = new List<Hex>();




        foreach (var item in allHexGameObject)
        {
            if (item.Value.GetComponent<MapTile>().isWalkableNow())
            {
                gameo.Add(item.Key);


                if (hexRangeMove.ContainsKey(item.Key.ToString()))
                {
                    corectRange.Add(item.Key.ToString(), item.Key);

                }
            }
        }

        


        /*if (hexRangeMove.Count > 0)
        {
            foreach (var item in hexRangeMove)
            {
                Hex tem = item.Value;
                var all = gameMenager.GetComponent<GameMenageraScript>().AllHex;

                    if (allHexGameObject.ContainsKey(tem))
                    {
                    Debug.Log(" ha ha ");
                        GameObject temp2 = allHexGameObject[tem];
                        if (temp2.GetComponent<MapTile>().isWalkableNow())
                        {
                            corectRange.Add(tem.ToString(), tem);
                        }
                    }






            }
        }

        Debug.Log(corectRange.Count + "  a stara ma:  " + hexRangeMove.Count);*/
        return corectRange;
    }
    public void findPath(Hex start2)
    {
        curentPath = new List<Hex>();



        curentPath = FindPath.FindPathInHex(start, start2, hexRangeMove);
        curentPath.Reverse();
        if (curentPath.Count > 0 && isWalkNow == false)
        {
            Hex curentselectHex = layot.Vector3ToHex(transform.position);
            GameObject curenTile = menagerScript.FindMapTileFromHex(curentselectHex);
            GameObject curent = curenTile.GetComponent<MapTile>().curentTileSelect;
            curent.GetComponent<MeshRenderer>().enabled = false;
            DeactivAllArrow();

            bool isHaveShotgunEqip = animator.GetBool("Equipshotgun");
            if(isHaveShotgunEqip == true){
                animator.SetBool("shotgunWalk", true);
            }else{
                 animator.SetBool("isWalkNow", true);
            }
           
            StartCoroutine(TravelWithCurentPath(curentPath));

            /*foreach (var item in curentPath)
            {
                GameObject to = GameObject.CreatePrimitive(PrimitiveType.Cube);
                to.transform.position = layot.HexToVector3(item);

            }*/
            // GameObject to = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // to.transform.position = layot.HexToVector3(curentPath[1]);
        }
    }

    public void updateAfterSelect()
    {


            DeactivAllArrow();// wyłącz wszystkie strzałki 
            foreach (var item in menagerScript.actionDictionary)//actywuj ikone dla actywnej akcji actora
            {
                Debug.Log("szukam Akcji");
                if(item.Key == curentAction){
                    
                    item.Value.GetComponent<UiButton>().UnactivateAll();
                    item.Value.GetComponent<UiButton>().Activate();
                
                }
            }
            start = layot.Vector3ToHex(this.transform.position);//znajdz hexa na ktorym stoi actor, przypisz go do zmiennej start
            if(isDoneAllInThisTurn && isChosenDirection ){// jezeli wszyskie punkty wykorzystane i wybrany kierunek
                
                //gameMenager.GetComponent<GameMenageraScript>().updateCurentActor(null);
                DisableCurentDirection();//wyłącz hexsy dla kierunku
                return; 
            }

            if(actionPoint>0 && isDoneAllInThisTurn == false){//jezeli ma punkty akcji i nie zrobił wszystkiego co mozliwe w tej turze
                hexRangeMove = start.GetHexInRange(movrRangeInHex, start);//wyswietl zasieg ruchu dla miejsa w ktorym stoi
            hexRangeMove = CorectRangeToWalkableAndIsInMap();//popraw zasieg uwzgledniajac hexsy ktorer sa zajete
            DisplayCurentRange();//wyswietl zasieg danego gracza
            gameMenager.GetComponent<GameMenageraScript>().updateCurentActor(this);// oznacz danego gracza jako biezacego
            }
            if(actionPoint == 0 && isChosenDirection == false && isDoneAllInThisTurn == false){//jezeli nie ma punktow akcji ale nie wybral kierunku i nie zrobił wszysiego co mozliwe
                hexRangeDirection = start.GetHexInRange(1, start);// znajdz heksy w odleglosci 1 od tego w ktorym stoi
                DisplayCurentDirection();//wyswietl heksy kierunkowe
                gameMenager.GetComponent<GameMenageraScript>().updateCurentActor(this);//oznacz danego gracza jako biezacego
            }
            
            
    
        
        
    }
    /*private void OnMouseDown()
    {
    
        
    }*/
    public void DisplayCurentDirection(){
        foreach (var item in allHexGameObject)
        {
            item.Value.GetComponent<MapTile>().DeactivRange();
        }

        foreach (var item in hexRangeDirection)
        {
            //Debug.Log(item.Key.ToString());
            if (all.ContainsKey(item.Key.ToString()))
            {
                Hex curent = all[item.Key.ToString()];
                //Debug.Log("curent " + curent);
                allHexGameObject[curent].GetComponent<MapTile>().activeCurentRange();
            }
        }
    }

    public void DisableCurentDirection(){
        foreach (var item in allHexGameObject)
        {
            item.Value.GetComponent<MapTile>().DeactivRange();
        }
    }
}
