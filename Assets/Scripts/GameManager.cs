using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
    
    public void Start() {
        Application.targetFrameRate = 60;
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
        EnemyManager.EffectManager = EffectManager;
        Debug.Log("SetComponents");
        yield return new WaitForSecondsRealtime(0.03f);
        //PlayerController.LoadDataFromFile = true;
        PlayerController.Position = CurrentLevelInfo.PlayerPosition;

        SceneCamera.IsLocked = CurrentLevelInfo.CameraLocked;
        SceneCamera.LockedCameraPos = CurrentLevelInfo.CameraStartPos;
        SceneCamera.SetCameraSize(CurrentLevelInfo.CameraSize);
        Debug.Log("SetData");
        yield return new WaitForSecondsRealtime(0.03f);
        Time.timeScale = 1f;
        Debug.Log("StartGame");
    }

}
