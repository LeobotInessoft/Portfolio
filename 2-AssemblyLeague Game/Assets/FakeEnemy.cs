using UnityEngine;
using System.Collections;

public class FakeEnemy : MonoBehaviour {
    bool hasBeenAdded = false;
    public string ID;
	// Use this for initialization
	void Start () {
        ID = "";
	}
	
	// Update is called once per frame
	void Update () {
        if (ID == "") ID = System.Guid.NewGuid().ToString();
        if (hasBeenAdded==false)
        {
            if (FakeEnemyList.PublicAccess != null)
            {
                if (FakeEnemyList.PublicAccess.AllFakeEnemies.Contains(this) == false)
                {
                    FakeEnemyList.PublicAccess.AllFakeEnemies.Add(this);
                }
                hasBeenAdded = true;

            }
        }
	}
}
