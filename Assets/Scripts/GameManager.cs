using Assets.Scripts.Items;
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {
    [HideInInspector]
    public PlayerController PlayerController;
    [HideInInspector]
    public GameObject PlayerControllerObject;
    [HideInInspector]
    public PlayerSpriteRenderer PlayerSpriteRenderer;
    [HideInInspector]
    public SceneCamera SceneCamera;
    [HideInInspector]
    public GameObject SceneCameraObject;
    [HideInInspector]
    public GameObject EnemyManagerObject;
    [HideInInspector]
    public EnemyManager EnemyManager;
    [HideInInspector]
    public GameObject EffectManagerObject;
    [HideInInspector]
    public EffectManager EffectManager;

    public LevelInfo CurrentLevelInfo;
    public GameObject PlayerPrefab;
    public GameObject SceneCameraPrefab;
    public GameObject EnemyManagerPrefab;
    public GameObject EffectManagerPrefab;

    public GameObject LoadingScreenRighthalf;
    public GameObject LoadingScreenLefthalf;

    public ItemDataObject ItemDataObject;
    public ItemFactory ItemFactory;
    
    public void Start() {
        Application.targetFrameRate = 60;
        ItemDataObject.Initialize();
        ItemFactory = new ItemFactory(ItemDataObject);
        GameInput.Init();
        Time.timeScale = 0f;
        StartCoroutine(StartGame());
    }
    public IEnumerator StartGame() {
        PlayerControllerObject = Instantiate(PlayerPrefab, CurrentLevelInfo.PlayerPosition, Quaternion.identity);
        SceneCameraObject = Instantiate(SceneCameraPrefab, CurrentLevelInfo.CameraStartPos, Quaternion.identity);
        EnemyManagerObject = Instantiate(EnemyManagerPrefab);
        EffectManagerObject = Instantiate(EffectManagerPrefab);
        Debug.Log("CreatePrefabs");
        yield return new WaitForSecondsRealtime(0.03f);
        PlayerController = PlayerControllerObject.GetComponent<PlayerController>();
        SceneCamera = SceneCameraObject.GetComponent<SceneCamera>();
        EnemyManager = EnemyManagerObject.GetComponent<EnemyManager>();
        EffectManager = EffectManagerObject.GetComponent<EffectManager>();
        PlayerSpriteRenderer = PlayerControllerObject.GetComponentInChildren<PlayerSpriteRenderer>();
        Debug.Log("GetComponents");
        yield return new WaitForSecondsRealtime(0.03f);
        SceneCamera.PlayerSpriteRenderer = PlayerSpriteRenderer;
        EffectManager.gameCamera = SceneCamera;
        PlayerController.EffectManager = EffectManager;
        PlayerController.ItemFactory = ItemFactory;
        EnemyManager.EffectManager = EffectManager;
        EnemyManager.PlayerController = PlayerController;
        Debug.Log("SetComponents");
        yield return new WaitForSecondsRealtime(0.03f);
        //PlayerController.LoadDataFromFile = true;
        PlayerController.Position = CurrentLevelInfo.PlayerPosition;
        PlayerController.Initialize();
        SceneCamera.IsLocked = CurrentLevelInfo.CameraLocked;
        SceneCamera.LockedCameraPos = CurrentLevelInfo.CameraStartPos;
        SceneCamera.SetCameraSize(CurrentLevelInfo.CameraSize);

        // EnemyManager.GenerateBoss1(new Vector3(10f,-1f,0));
        Debug.Log("SetData");
        yield return new WaitForSecondsRealtime(0.03f);
        StartCoroutine(LoadingScreenSlideOpen(0.5f));
        
        Debug.Log("StartGame");
    }

    public IEnumerator LoadingScreenSlideClose(float time) { 
        //Left half from 1600 to 310, right half from -1600 to -310
        LoadingScreenRighthalf.transform.localPosition = new Vector3(1600, 0, 0);
        LoadingScreenRighthalf.GetComponent<Image>().enabled = true;
        LoadingScreenLefthalf.transform.localPosition = new Vector3(-1600, 0, 0);
        LoadingScreenLefthalf.GetComponent<Image>().enabled = true;
        float timer = 0f;
        while (timer < time) {
            timer += 0.03f;
            float t = timer / time;
            float x = Mathf.Lerp(1600, 310, t);
            float y = Mathf.Lerp(-1600, -310, t);
            LoadingScreenRighthalf.transform.localPosition = new Vector3(x, 0, 0);
            LoadingScreenLefthalf.transform.localPosition = new Vector3(y, 0, 0);
            yield return new WaitForSecondsRealtime(0.03f);
        }
        //Debug
        StartCoroutine(StartGame());
    }
    public IEnumerator LoadingScreenSlideOpen(float time) {
        //Left half from 310 to 1600, right half from -310 to -1600
        float timer = 0f;
        while (timer < time) {
            timer += 0.03f;
            float t = timer / time;
            float x = Mathf.Lerp(310, 1600, t);
            float y = Mathf.Lerp(-310, -1600, t);
            LoadingScreenRighthalf.transform.localPosition = new Vector3(x, 0, 0);
            LoadingScreenLefthalf.transform.localPosition = new Vector3(y, 0, 0);
            yield return new WaitForSecondsRealtime(0.03f);
        }
        LoadingScreenRighthalf.GetComponent<Image>().enabled = false;
        LoadingScreenLefthalf.GetComponent<Image>().enabled = false;
        Time.timeScale = 1f;
    }

}
