using UnityEngine;
using System.Collections;

public class RuntimeVariableText : MonoBehaviour {
    public string VariableName;
    public UnityEngine.UI.Text TextVariableName;
    public UnityEngine.UI.Text TextValue;

	// Use this for initialization
	void Start () {
        TextVariableName.text = VariableName;
      
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void RefreshVariable(string newValue)
    {
        TextVariableName.text = VariableName;
        TextValue.text = newValue;

    }
}
