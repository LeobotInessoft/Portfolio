using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class PanelFiancialRow : MonoBehaviour
{
    public Image ImageRobot;
    public Text TextDate;
    public Text TextReason;
    public Text TextAmount;
    public PanelFinancialsContent MyPanelFinancialsContent;
    // Use this for initialization
    Texture2D texture2D;
    void Start()
    {
        texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, false);

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetRow(xFinanceRow aRow)
    {
        if (texture2D == null)
            texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, false);
        TextDate.text = aRow.EventDate.ToString("HH:mm\ndd MMM");
        TextReason.text = aRow.Reason;
        TextAmount.text = "$" + aRow.MoneyAmount;

        RobotConstructor.RobotTemplate aTemp = MyPanelFinancialsContent.TheLookUp.GetAllTemplatesForCurrentPlayer().FirstOrDefault(x => x.LeagueID == aRow.RobotID);
        byte[] image = new byte[0];
        if (aTemp != null)
        {
            image = aTemp.ScreenShot;
        }
        else
        {

        }
        if (image != null && image.Length > 0)
        {
            texture2D.LoadImage(image);
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            ImageRobot.sprite = sprite;
            ImageRobot.gameObject.SetActive(true);

        }
        else
        {
            ImageRobot.gameObject.SetActive(false);
        }
    }
}
