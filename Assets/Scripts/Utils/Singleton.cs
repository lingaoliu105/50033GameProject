using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Singleton<T> : MonoBehaviour  where  T : MonoBehaviour
{
    private  static  T _instance;
    public  static  T Instance
    {
        get
        {
            return  _instance;
        }
    }

    public  virtual  void  Awake ()
    {
        //Debug.Log("Singleton Awake called");

        if (_instance  ==  null) {
            _instance  =  this  as T;
            DontDestroyOnLoad (this.gameObject);
        } else {
            Destroy (gameObject);
        }
    }
}