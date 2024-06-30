using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentMassUploader : MonoBehaviour {
    public bool MustDoUpload = false;
    public WwwLeagueInterface wwwLeague;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (MustDoUpload)
        {
            MustDoUpload = false;
            Upload();
        }
	}
    private void Upload()
    {
        List<ComponentType> allTypes = new List<ComponentType>();
        allTypes.AddRange(gameObject.transform.GetComponentsInChildren<ComponentType>());
        print("Found "+allTypes.Count);
        wwwLeague.UploadComponent(allTypes);
    }
}
