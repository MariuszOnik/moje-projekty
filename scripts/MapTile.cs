using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public GameObject znacznik;
    public bool isEmpty = true;
    public bool isVisble;
    public Material Empty;
    public Material Zajety;
    public Renderer renderZnacznika;
    public MeshRenderer znacznikMeshRender;
    public GameObject ArrowPref;
    public MeshRenderer arrow;
  
    RaycastHit statusHit;
    Animator animator;
    public Transform startReyPosition;
    public Actor curentActor;
    public GameObject menager;
    public bool isCurentTarget = false;
    public bool isCurentDirection = false; 
    public GameObject curentTileSelect; 
    
    
    // Start is called before the first frame update
    void Start()
    {
        renderZnacznika = znacznik.GetComponent<Renderer>();
        znacznikMeshRender = znacznik.GetComponent<MeshRenderer>();
        animator = znacznik.GetComponent<Animator>();
        arrow = ArrowPref.GetComponent<MeshRenderer>();
        znacznikMeshRender.enabled = false;
        animator.SetBool("playAnimation", false);

         menager = GameObject.FindGameObjectWithTag("gameMenager");
        


    }

    // Update is called once per frame
    void Update()
    {

        //isWalkableNow();

    }
    private void OnMouseEnter()
    {
        //znacznikMeshRender.enabled = isWalkableNow();
        //animator.SetBool("playAnimation", true);
        //Debug.Log(curentActor.transform.position.ToString());
    }

    private void OnMouseExit()
    {
       // znacznikMeshRender.enabled = false;
        //animator.SetBool("playAnimation", false);

    }

    public void OnMouseDown()
    {
       
       menager.GetComponent<GameMenageraScript>().WhatIClikNow[GameEntity.MapTile] = true;
        curentActor = menager.GetComponent<GameMenageraScript>().curentActor;//znajdz biezacego actora, tego ktory jest aktualnie zaznaczony 
        //Debug.Log(curentActor.transform.position.ToString());
        
        if(curentActor!= null && curentActor.GetComponent<Actor>().actionPoint>0){//jezeli ma jeszcze punkty akcji
            if (isWalkableNow() == false)//ale właśnie idzie sobie do celu
        {
            return;//wychodzimy
        }
        else// jezeli akurat nie idzie i stoi w miejscu
        {
            if (isCurentTarget)// a na dodatek kafelek na ktory wlasnie klikasz jest aktualnie oznaczony jako cel podrozy 
            {


                //Debug.Log("I m curent Target");
                isCurentTarget = false;// odznacz  ten kafelek jako cel podrozy
                DeactivArrow();// wyłącz wszystkie strzałki na ekranie
                Layout layot = menager.GetComponent<GameMenageraScript>().layot;// pobierz referencje do layotu gry, potrzebujesz go do konwersji pozycji Vectorowej na pozycje w Hexach
                Hex thisHex2 = layot.Vector3ToHex(this.transform.position);// robimy pomocniczy Hex , bedacy pozycja tego kafelka wyrazona w hexach
                curentTileSelect.GetComponent<MeshRenderer>().enabled = false;// wyłącz znacznik zaznaczenia
                curentActor.GetComponent<Actor>().findPath(thisHex2);// znajdz sciezke dla biezacego actora do celu ktorym jest ten kafelek
                return;//wychodzimy
            }


            if (isCurentTarget == false && curentActor.GetComponent<Actor>().actionPoint > 0)
            {
                
                Layout layot = menager.GetComponent<GameMenageraScript>().layot;
                Hex thisHex = layot.Vector3ToHex(this.transform.position);
                Dictionary<string, Hex> curenPlayerRange = curentActor.GetComponent<Actor>().hexRangeMove;
                Dictionary<Hex, GameObject> allMapTile = menager.GetComponent<GameMenageraScript>().AllHexGameObject;
                foreach (var item in allMapTile)
                {
                    GameObject temp = item.Value;

                    item.Value.GetComponent<MapTile>().DeactivArrow();
                    item.Value.GetComponent<MapTile>().isCurentTarget = false;
                }
                if (curenPlayerRange.ContainsKey(thisHex.ToString()) && curentActor.isWalkNow == false)
                {
                    activeArrow();
                    curentActor.target = layot.HexToVector3(thisHex);
                    isCurentTarget = true;

                }
            }
        }
        }
        
        if(curentActor != null && curentActor.GetComponent<Actor>().actionPoint==0 && isCurentDirection == false && curentActor.GetComponent<Actor>().isDoneAllInThisTurn == false ){
            Dictionary<Hex, GameObject> allMapTile = menager.GetComponent<GameMenageraScript>().AllHexGameObject;
            foreach (var item in allMapTile)
                {
                    GameObject temp = item.Value;

                    item.Value.GetComponent<MapTile>().DeactivArrow();
                    item.Value.GetComponent<MapTile>().isCurentDirection = false;
                }
            activeArrow();
            curentActor.GetComponent<Transform>().LookAt(this.transform);
            isCurentDirection = true; 
            return;
        }
        if(curentActor != null && curentActor.GetComponent<Actor>().actionPoint==0 && isCurentDirection == true &&  curentActor.GetComponent<Actor>().isDoneAllInThisTurn == false){
            Dictionary<Hex, GameObject> allMapTile = menager.GetComponent<GameMenageraScript>().AllHexGameObject;
            foreach (var item in allMapTile)
                {
                    GameObject temp = item.Value;

                    item.Value.GetComponent<MapTile>().DeactivArrow();
                }
            curentActor.GetComponent<Actor>().isDoneAllInThisTurn = true; 
            curentActor.GetComponent<Actor>().isChosenDirection = true; 
            curentActor.GetComponent<Actor>().updateAfterSelect();
            
        }

        



    }



    public void activeCurentRange()
    {
        znacznikMeshRender.enabled = isWalkableNow();
        animator.SetBool("playAnimation", true);
    }

    public void DeactivRange()
    {
        znacznikMeshRender.enabled = false;
        //curentTileSelect.GetComponent<MeshRenderer>().enabled = false;
        animator.SetBool("playAnimation", false);
    }

    public void activeArrow()
    {
        arrow.enabled = true;
         
        
    }

    public void DeactivArrow()
    {
        arrow.enabled = false;
        //curentTileSelect.GetComponent<MeshRenderer>().enabled = false;

    }

    

    public bool isWalkableNow()
    {

        
        if (Physics.Raycast(startReyPosition.position, transform.TransformDirection(Vector3.forward), out statusHit, 4f)) 
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.red);
            return false;
        }else
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.green);
            return true;
        }
        
    }
}
