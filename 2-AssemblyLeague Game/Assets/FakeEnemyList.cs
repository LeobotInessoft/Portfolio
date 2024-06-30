using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FakeEnemyList : MonoBehaviour {
    public static FakeEnemyList PublicAccess;
    public List<FakeEnemy> AllFakeEnemies= new List<FakeEnemy>();
	// Use this for initialization
	void Start () {
        AllFakeEnemies = new List<FakeEnemy>();
        PublicAccess = this;
	}
	
	// Update is called once per frame
	void Update () {
        PublicAccess = this;
	}
}
