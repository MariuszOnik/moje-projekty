using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UiButton : MonoBehaviour
{

    public GameObject selectIcon; 
    public bool isActivNow = false; 
    public GameObject gameManger;
    public GameMenageraScript scriptMenager; 

    public Action thisAction ;


    public   GameObject[] allbuuton = new GameObject[4]; 


    public void PressMee(){
        var Actor = gameManger.GetComponent<GameMenageraScript>().curentActor;
        scriptMenager.WhatIClikNow[GameEntity.UiButton] = true;
        //scriptMenager.TestMouseDown();
        
        if(Actor != null){
            UnactivateAll();
            //Activate();
            Actor.GetComponent<Actor>().lastAction = Actor.GetComponent<Actor>().curentAction;
            Actor.GetComponent<Actor>().curentAction = thisAction;
            Activate();
           
        }
        
    }

    public void UnactivateAll(){
        foreach (var item in allbuuton)
        {
            item.GetComponent<UiButton>().Unactivate();
        }
    }
    // Start is called before the first frame update

    public void Unactivate(){
        isActivNow = false; 
        selectIcon.SetActive(false);
    }
    public void Activate(){
        isActivNow = true;
        selectIcon.SetActive(true);
    }


    void Start()
    {
       scriptMenager = gameManger.GetComponent<GameMenageraScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
