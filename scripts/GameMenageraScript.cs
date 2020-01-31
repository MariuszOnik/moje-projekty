using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameEntity
{
    Actor, MapTile, UiButton, EmptyClik
}
public class GameMenageraScript : MonoBehaviour
{
   

    public GameObject hexPrefab;
    public Layout layot;
    public Transform mapHolder;
    public Vector2 MapOrgin;
    public Vector2 hexPrefabSize;
    public int mapRadius = 3;
    public Dictionary<string, Hex> AllHex = new Dictionary<string, Hex>();
    public Dictionary<Hex, GameObject> AllHexGameObject = new Dictionary<Hex, GameObject>();
    public Vector3[] PlayerTeamStartPositions = new Vector3[4];
    public Vector3[] EnemyTeamStartPositions = new Vector3[4];
    public int HowManyPlayer;
    public GameObject[] PlayerTeam;
    public GameObject PlayerPref;
    public Actor curentActor;
    public Team curentTurn;
    public int LicznikPunktówAkcjiTemu;
    public GameObject UiButonEnd;

    public GameObject Uishotgun;
    public GameObject UiFire; 
    public GameObject UiWalk;
    public GameObject UiAmo; 

    public UiButton curentButon;

    public Dictionary<Action, GameObject> actionDictionary = new Dictionary<Action, GameObject>();
    
    public Dictionary<GameEntity, bool> WhatIClikNow = new Dictionary<GameEntity, bool>();

    
    
    public void Awake()
    {
        layot = new Layout(Layout.pointy, hexPrefabSize, MapOrgin);
        //hexPrefabSize = new Vector2(hexPrefab.GetComponent<Collider>().bounds.size.x, hexPrefab.GetComponent<Collider>().bounds.size.z);
        setStartPosition(Directions.West, Directions.SouthEast);
        MapOrgin = new Vector2(0, 0);
        hexPrefabSize = new Vector2(1, 1);
        
        PlayerTeam = new GameObject[HowManyPlayer];
        curentTurn = Team.player;
        UiButonEnd.SetActive(false);
        actionDictionary.Add(Action.Shotgan,Uishotgun.GetComponent<UiButton>().transform.gameObject);
        actionDictionary.Add(Action.Target,UiFire.GetComponent<UiButton>().transform.gameObject);
        actionDictionary.Add(Action.Walk,UiWalk.GetComponent<UiButton>().transform.gameObject);
        actionDictionary.Add(Action.Amo,UiAmo.GetComponent<UiButton>().transform.gameObject);

        WhatIClikNow.Add(GameEntity.Actor,false);
        WhatIClikNow.Add(GameEntity.MapTile,false);
        WhatIClikNow.Add(GameEntity.UiButton,false);
        WhatIClikNow.Add(GameEntity.EmptyClik,false);


    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateHexagonShapeMap(mapRadius);
       DrawHexMap();
        setStartPosition(Directions.West, Directions.SouthEast);
        initializeTeam(PlayerTeam);
        LicznikPunktówAkcjiTemu = PlayerTeam.Length * 2;
    }

    public void ResetWhatIClicNowDictionary(){
        WhatIClikNow[GameEntity.Actor] = false;
        WhatIClikNow[GameEntity.EmptyClik] = false;
        WhatIClikNow[GameEntity.MapTile] = false;
        WhatIClikNow[GameEntity.UiButton] = false;
    }
    public void TestMouseDown(){
        
          //  Debug.Log("Mouse Test");

        WhatIClikUpdate();
       

        foreach (var item in WhatIClikNow)
        {
            Debug.Log(item.Key.ToString() + " " + item.Value.ToString());
            //if(item.Value == true){
               // WhatIClikNow[GameEntity.EmptyClik] = false;
            //}
        }
        //Debug.Log(WhatIClikNow.ToString());
        
                
    }

    public void WhatIClikUpdate(){
        if(WhatIClikNow[GameEntity.Actor] == false && WhatIClikNow[GameEntity.UiButton] == false && WhatIClikNow[GameEntity.MapTile] == false ){
            WhatIClikNow[GameEntity.EmptyClik] = true;
        }
    }
    // Update is called once per frame
    void Update()
    {

        
       /* if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; 
            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log(layot.Vector3ToHex(new Vector3(hit.point.x, 0, hit.point.z)));
            }
        }*/
    }
    private void LateUpdate() {
        
        //TestMouseDown();
        if(Input.GetMouseButtonUp(0)){
            TestMouseDown();
            if(WhatIClikNow[GameEntity.EmptyClik] == true && curentActor != null){
                foreach (var item in actionDictionary)
                {
                    item.Value.SetActive(false);
                }
                
            }
            ResetWhatIClicNowDictionary();
        }
    }

    public void endTurn()
    {
        if(curentTurn == Team.player)
        {
            curentTurn = Team.enemy;
            return;
        }
        if (curentTurn == Team.enemy)
        {
            curentTurn = Team.player;
            foreach (var item in PlayerTeam)
            {
                var script = item.GetComponent<Actor>();

                script.updateActionPoint(2);
                script.checkActionPoint();

            }
            return;
        }
        
    }

    public void Shotgun(){
        Animator curenanimator = curentActor.GetComponent<Animator>();
        bool curent = curenanimator.GetBool("Equipshotgun");

        if(curent == false){
            curenanimator.SetBool("Equipshotgun", true);
            curentActor.GetComponent<Actor>().Shotgun.enabled = true; 
            return;
        }
        if(curent == true){
            curenanimator.SetBool("Equipshotgun", false);
            curentActor.GetComponent<Actor>().Shotgun.enabled = false; 
            return;
        }
        
    }
    void GenerateHexagonShapeMap(int map_radius)
    {
        for (int q = -map_radius; q <= map_radius; q++)
        {
            int r1 = Mathf.Max(-map_radius, -q - map_radius);
            int r2 = Mathf.Min(map_radius, -q + map_radius);
            for (int r = r1; r <= r2; r++)
            {
                Hex temp = new Hex(q, r, -q - r);
                AllHex.Add(temp.ToString(), temp);

            }
        }
    }

    public void Fire(){
        //curentActor.animator.ResetTrigger("ShotTriger");
        curentActor.animator.SetTrigger("ShotTriger");
        curentActor.playShotgun();
       // curentActor.animator.ResetTrigger("ShotTriger");
        
    }

   
    public void RestoreTeamPlayerActionPoint(){
        foreach (var item in PlayerTeam)
        {
            item.GetComponent<Actor>().RestoreActionPoint();
            item.GetComponent<Actor>().updateActionPoint(0);
            

        }
        UiButonEnd.SetActive(false);
       
    }

    public void updateCurentActor(Actor actor){
        curentActor = actor;
        ActorStateUpdate(Action.Walk);

    }

    public void ActorStateUpdate(Action action){
        curentActor.curentAction = action;
        switch (curentActor.curentAction)
        {   
            case Action.Walk:
                curentActor.DeactivAllArrow();
                curentActor.start = layot.Vector3ToHex(curentActor.transform.position);
                curentActor.hexRangeMove = curentActor.start.GetHexInRange(curentActor.movrRangeInHex, curentActor.start);//wyswietl zasieg ruchu dla miejsa w ktorym stoi
                curentActor.hexRangeMove= curentActor.CorectRangeToWalkableAndIsInMap();// 
                curentActor.DisplayCurentRange();
            break;
            
            default:
            break;
        }
    }
    public void updateCurentActor2(Actor actor)
    {
        
        curentActor = actor;
        Hex curentselectHex = layot.Vector3ToHex(curentActor.transform.position);

        if(actor.curentAction == actor.lastAction){
            return;
        }else
        {
            switch (actor.curentAction)
        {
            case Action.Walk:
                if(actor.isDoneAllInThisTurn == false){//jesli ma punkty akcji i został zaznaczony
                    if(actor.isWalkNow == false){//jeszeli stoi w miejscu
                        foreach (var item in AllHexGameObject) // odznacz poprzednie zaznaczenia
                        {
                            var m = item.Value.GetComponent<MapTile>();
                            m.curentTileSelect.GetComponent<MeshRenderer>().enabled = false;
                        }
                        GameObject curenTile = FindMapTileFromHex(curentselectHex);// znajdz kafelek na ktorym stoi
                        GameObject curent = curenTile.GetComponent<MapTile>().curentTileSelect;//znajdz zaznaczenie tego kafelka
                        curent.GetComponent<MeshRenderer>().enabled = true;//aktywj to zaznaczenie
                        curent.GetComponent<MeshRenderer>().material = curenTile.GetComponent<MapTile>().Zajety;//nadaj mu kolor zólty
                        int Licznik = 0; // licznik wszystki punktow akcji druzyny
                        foreach (var item in PlayerTeam) // dla każdego actora nalezacego do gracza
                        {
                        Licznik += item.GetComponent<Actor>().actionPoint; //dodaj do licznika punkty kazdego gracza 
                        }
                        if (Licznik == 0)// jezel zaden z graczy nie ma juz punktow akcji
                        {
                            UiButonEnd.SetActive(true);//aktywuj przycisk koniec tury
                        }
                    }
                }else{ //jezeli aktualny actor nie ma juz punktow akcji a został zaznaczony 
                        GameObject curenTile = FindMapTileFromHex(curentselectHex); //znajdz kafelek na ktorym stoi
                        GameObject curent = curenTile.GetComponent<MapTile>().curentTileSelect; //znajdz zaznaczenie tego kafelka
                        curent.GetComponent<MeshRenderer>().enabled = false; //wyłącz zaznaczenie
                        //curent.GetComponent<MeshRenderer>().material = curenTile.GetComponent<MapTile>().Empty;
                }     
            break;

            case Action.Target:
            actor.DisableCurentDirection();
            actor.updateAfterSelect();
            
            
            break;

            case Action.Shotgan:
            actor.DisableCurentDirection();
           actor.updateAfterSelect();
            //actor.updateAfterSelect();
            break;

            case Action.Amo:
            actor.DisableCurentDirection();
            actor.updateAfterSelect();
            //actor.updateAfterSelect();
            break;

            default:
            break;
        }
        }

        

        

           
        
        
    }

    public void DrawHexMap()
    {
        foreach (var item in AllHex)
        {
            
            GameObject temp = Instantiate(hexPrefab);
            temp.transform.position = layot.HexToVector3(item.Value);
            temp.transform.parent = mapHolder;
            AllHexGameObject.Add(item.Value, temp);
        }
    }

    public void initializeTeam(GameObject[] team)
    {

        for (int i = HowManyPlayer-1; i >= 0; i--)
        {
            team[i] =Instantiate(PlayerPref);
            team[i].GetComponent<Transform>().position = PlayerTeamStartPositions[i];
            team[i].GetComponent<Transform>().Rotate(0, 30, 0);

        }
              
    }

    public GameObject FindMapTileFromHex(Hex hex)
    {
       
        foreach (var item in AllHexGameObject)
        {
            if (item.Key.ToString() == hex.ToString())
            {
                return item.Value;
            }
        }
        return null;
    }

    public void setStartPosition(Directions dirFirst, Directions dirNext)
    {
        Hex center = new Hex(0, 0, 0);
        int radius = mapRadius;
        for (int i = 1; i <= radius; i++)
        {
            center = center.Add(Hex.directionsArray[(int)dirFirst]);
        };

        PlayerTeamStartPositions[0] = layot.HexToVector3(center);
      

        Hex next = new Hex(center.q, center.r, center.s);
        for (int i = 1; i <= radius; i++)
        {

            next = next.Add(Hex.directionsArray[(int)dirNext]);
            PlayerTeamStartPositions[i] = layot.HexToVector3(next);
           
        }

    }
}

public enum Team
{
    player, enemy
}
