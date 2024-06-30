using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManualCoding : MonoBehaviour {
    public UnityEngine.UI.Text TextManual;
	// Use this for initialization
	void Start () {
        TextManual.text = generateManual();
	}
    string generateManual()
    {
        string ret = "<size=25>Coding Manual</size>\n";

        ret += "<size=23>Getting started Coding</size>\n";
        ret += "Assembly League robots use a language very similar to a real programming language known as \"Assembly Language\", often abbreviated as \"ASM\".\n";
        ret += "Assembly language is extremely easy and only has a view basic commands such as add, multiply, etc. which you can learn from the tutorials available at the Assembly League website if you want. Alternatively, simply use the Code Generator to generate the code for you!\n\n";

        ret += "<size=23>Basic Commands</size>\n";
        IEnumerable<Computer.CodeLine.EnumCommand> ints2 = Computer.InterrupHandler.GetValues<Computer.CodeLine.EnumCommand>();
        foreach (Computer.CodeLine.EnumCommand an in ints2)
        {
            ret += an + ": " + Computer.CodeLine.GetDisplayText(an) + "\n";
        }
        ret += "";
        ret += "<size=23>Special Commands</size>\n";
        ret += "There are two 'special' commands that are essential to assembly:\n";
        ret += "<size=20>IO Command</size>\n";
        ret += "The IO command is an abbreviation of \"Input\\Output\" and allows you to send or retrieve values from an external device such as a Laser Gun, Mine-layer and Hull components. To shoot a weapon, you need to provide the right IO command along with the data needed for external IO Device to use. Thereby allowing you to easily say, provide the command to change the direction of a weapon and then provide the command to fire the weapon.\n ";
        ret += "The IO commands you have available depend on what devices are attached to your robot. Read the IO manual of a device to see what it is capable of.\n\n";
        ret += "<size=20>INT Command</size>\n";
        ret += "The INT command has a wide variety of usages. It essentially provides you with the building blocks of code needed to perform useful actions such as determine which enemy has the lowest health, or is the closest to your robot or is the furthest way from your robot. There are many INT commands and once you know the basics of Assembly Coding you will mainly want a quick reference to see all the INT commands at your disposal for usage. Below is a list of all the INT commands supported:\n";
        IEnumerable<Computer.InterrupHandler.Interrupt> ints = Computer.InterrupHandler.GetValues<Computer.InterrupHandler.Interrupt>();
        foreach (Computer.InterrupHandler.Interrupt an in ints)
        {
            ret +="INT "+ (int)an+": "+ Computer.InterrupHandler.GetDisplayText(an) + "\n";
        }
        ret += "";
        return ret;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
