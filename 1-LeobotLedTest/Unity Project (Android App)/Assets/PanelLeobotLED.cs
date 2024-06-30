using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class PanelLeobotLED : MonoBehaviour
{
    int NumberOfLeds = 192;
    public InputField InputServer;
    public InputField InputTestData;
    public InputField InputText;
    public Text TextResult;
    public Dropdown DropdownDevice;
    List<DeviceType> devices;
    List<LedColour> ledColours;
    List<LedScroll> ledScrollOptions;

    public Slider SliderBrightness;

    public Toggle ToggleWrap;
    public Dropdown DropDownColor;
    public Dropdown DropDownBorderColor1;
    public Dropdown DropDownBorderColor2;
    public Dropdown DropDownScroll;
    public Slider SliderScrollSpeed;
    // Start is called before the first frame update
    void Start()



    {
        StartCoroutine(LateStart(1));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Your Function You Want to Call
        ShowPanel();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void ShowPanel()
    {
        DropdownDevice.ClearOptions();
        devices = LedPanelHelper.GetAllDeviceTypes();

        for (int c = 0; c < devices.Count; c++)
        {
            DropdownDevice.options.Add(new Dropdown.OptionData() { text = devices[c].Name });
        }
        DropdownDevice.value = 0;
        DropdownDevice.value = 1;
        // DropdownDevice.value = 0;

        DropDownColor.ClearOptions();
        DropDownBorderColor1.ClearOptions();
        DropDownBorderColor2.ClearOptions();
        ledColours = LedPanelHelper.GetAllLedColours();

        for (int c = 0; c < ledColours.Count; c++)
        {
            DropDownColor.options.Add(new Dropdown.OptionData() { text = ledColours[c].ColourName });
            DropDownBorderColor1.options.Add(new Dropdown.OptionData() { text = ledColours[c].ColourName });
            DropDownBorderColor2.options.Add(new Dropdown.OptionData() { text = ledColours[c].ColourName });
        }
        DropDownColor.value = 0;
        DropDownColor.value = 4;
        DropDownBorderColor1.value = 0;
        DropDownBorderColor1.value = 1;
        DropDownBorderColor2.value = 0;
        DropDownBorderColor2.value = 2;


        DropDownScroll.ClearOptions();
        ledScrollOptions = LedPanelHelper.GetAllScrollOptions();

        for (int c = 0; c < ledScrollOptions.Count; c++)
        {
            DropDownScroll.options.Add(new Dropdown.OptionData() { text = ledScrollOptions[c].DirectionName });
        }
        DropDownScroll.value = 0;
        DropDownScroll.value = 2;
        //DropDownScroll.value = 0;


        WWWInterface.PublicAccess.OnDataDoneReceiving += PublicAccess_OnDataDoneReceiving;
        WWWInterface.PublicAccess.OnErrorDoneReceiving += PublicAccess_OnDataDoneReceiving;

        // sd.ServiceDiscovered += (s, serviceName) =>
        //{
        //  Console.WriteLine($"service '{serviceName}'");

        // Ask for the name of instances of the service.
        //   mdns.SendQuery(serviceName, type: DnsType.PTR);
        //  };
    }
    DeviceType GetSelectedDevice()
    {
        return devices[DropdownDevice.value];
    }
    LedColour GetSelectedColour()
    {
        return ledColours[DropDownColor.value];
    }
    LedColour GetSelectedBorderColour1()
    {
        return ledColours[DropDownBorderColor1.value];
    }
    LedColour GetSelectedBorderColour2()
    {
        return ledColours[DropDownBorderColor2.value];
    }
    LedScroll GetSelectedScrollOption()
    {
        return ledScrollOptions[DropDownScroll.value];
    }
    bool GetMustUseWrap()
    {
        return ToggleWrap.isOn;
    }
    int GetSelectedBrightness()
    {
        return (int)SliderBrightness.value;
    }
    int GetSelectedDelaySpeed()
    {
        return (int)SliderScrollSpeed.value;
    }
    public void ClosePanel()
    {

        WWWInterface.PublicAccess.OnDataDoneReceiving -= PublicAccess_OnDataDoneReceiving;
        WWWInterface.PublicAccess.OnErrorDoneReceiving -= PublicAccess_OnDataDoneReceiving;

    }

    private void PublicAccess_OnDataDoneReceiving(string result)
    {
        TextResult.text = result;
    }

    public void ButtonUploadClick()
    {

        WWWInterface.PublicAccess.UploadData(InputServer.text, InputTestData.text);
    }

    public void ButtonLeobotClick()
    {
        InputTestData.text = GetDisplay_LB();

        WWWInterface.PublicAccess.UploadData(InputServer.text, InputTestData.text);
    }

    public void ButtonTurnOffClick()
    {
        InputTestData.text = GetDisplay_TurnOff(GetSelectedDevice());
        WWWInterface.PublicAccess.UploadData(InputServer.text, InputTestData.text);
    }

    public void ButtonAsTextClick()
    {

        string text = InputText.text;


        int brightness = GetSelectedBrightness();

        LedColour mainColour = GetSelectedColour();
        LedColour borderColor1 = GetSelectedBorderColour1();
        LedColour borderColor2 = GetSelectedBorderColour2();
        bool wrap = GetMustUseWrap();

        LedScroll scrollOption = GetSelectedScrollOption();
        int delayTime = GetSelectedDelaySpeed();

        InputTestData.text = GenerateEncodedStringCommand(text, brightness, mainColour.EnumColor, borderColor1.EnumColor, borderColor2.EnumColor, wrap, scrollOption.TheDirection, delayTime);
        WWWInterface.PublicAccess.UploadData(InputServer.text, InputTestData.text);

    }
    public void ButtonOpenClick()
    {

        string text = "OPEN";

        int brightness = GetSelectedBrightness();
        LedColour mainColour = GetSelectedColour();
        LedColour borderColor1 = GetSelectedBorderColour1();
        LedColour borderColor2 = GetSelectedBorderColour2();
        bool wrap = GetMustUseWrap();
        LedScroll scrollOption = GetSelectedScrollOption();

        int delayTime = GetSelectedDelaySpeed();
        InputTestData.text = GenerateEncodedStringCommand(text, brightness, mainColour.EnumColor, borderColor1.EnumColor, borderColor2.EnumColor, wrap, scrollOption.TheDirection, delayTime);
        WWWInterface.PublicAccess.UploadData(InputServer.text, InputTestData.text);

    }
    public void ButtonClosedClick()
    {

        string text = "CLOSED";

        int brightness = GetSelectedBrightness();
        LedColour mainColour = GetSelectedColour();
        LedColour borderColor1 = GetSelectedBorderColour1();
        LedColour borderColor2 = GetSelectedBorderColour2();
        bool wrap = GetMustUseWrap();
        LedScroll scrollOption = GetSelectedScrollOption();

        int delayTime = GetSelectedDelaySpeed();
        InputTestData.text = GenerateEncodedStringCommand(text, brightness, mainColour.EnumColor, borderColor1.EnumColor, borderColor2.EnumColor, wrap, scrollOption.TheDirection, delayTime);
        WWWInterface.PublicAccess.UploadData(InputServer.text, InputTestData.text);

    }
    public void ButtonLunchClick()
    {

        string text = "OUT ON LUNCH";

        int brightness = GetSelectedBrightness();
        LedColour mainColour = GetSelectedColour();
        LedColour borderColor1 = GetSelectedBorderColour1();
        LedColour borderColor2 = GetSelectedBorderColour2();
        bool wrap = GetMustUseWrap();
        LedScroll scrollOption = GetSelectedScrollOption();
        int delayTime = GetSelectedDelaySpeed();

        InputTestData.text = GenerateEncodedStringCommand(text, brightness, mainColour.EnumColor, borderColor1.EnumColor, borderColor2.EnumColor, wrap, scrollOption.TheDirection, delayTime);
        WWWInterface.PublicAccess.UploadData(InputServer.text, InputTestData.text);

    }
    public string GenerateEncodedStringCommand(string input, int brightness, EnumLedColour mainColour, EnumLedColour aColourBorder1, EnumLedColour aColourBorder2, bool wrap, LedPanelHelper.ScrollDirection aScrollDirection, int scrollDelayTime)
    {
        string ret = "0^bright=" + brightness + "^ time=" + scrollDelayTime + "^phase=0^total=0^data=";
        int startPos = -1;
        List<ByteLetter> theLetters = LedPanelHelper.GetByteLetters(input);
        DeviceType aDevice = GetSelectedDevice();

        List<int> theLettersAsInt = new List<int>();

        {
            //scroll
            theLettersAsInt = LedPanelHelper.GenerateByteLettersCodeForDevice(input.Length * 8, 8, theLetters);
            LedPanelHelper.ScrollDirection theDirection = aScrollDirection;// LedPanelHelper.ScrollDirection.Down;
            List<LedPanelHelper.ScrollPhase> phases = LedPanelHelper.GenerateScroll(aDevice, input.Length * 8, 8, theLettersAsInt, 4, theDirection, wrap, mainColour, aColourBorder1, aColourBorder2, startPos);
            ret = "0^bright=" + brightness + "^time=" + (scrollDelayTime + 3) + "^phase=0^total=" + (phases.Count - 1) + "^data=";
            for (int z = 0; z < phases[0].TheData.Count; z++)
            {
                ret += phases[0].TheData[z] + "";
            }
            ret += ">";
            for (int c = 1; c < phases.Count; c++)
            {
                ret += "id=0^bright=" + brightness + "^time=" + scrollDelayTime + "^phase=" + (c) + "^total=" + (phases.Count - 1) + "^data=";


                for (int z = 0; z < phases[c].TheData.Count; z++)
                {
                    ret += phases[c].TheData[z] + "";
                }
                ret += ">";

            }

        }


        int totalLettersThatCanBeShownInRow = aDevice.LedsPerRow / 8;
        return ret;
    }
    public void ButtonDoneClick()
    {
        Application.Quit();
    }
    private string GenerateAllOffData()
    {
        string ret = "0^bright=15^time=50^phase=0^total=0^data=";
        for (int c = 0; c < NumberOfLeds; c++)
        {
            ret += "0";

        }
        ret += ">";
        return ret;
    }
    private string GenerateAllOnData()
    {
        string ret = "0^bright=15^time=50^phase=0^total=0^data=";
        for (int c = 0; c < NumberOfLeds; c++)
        {
            ret += (c % 2) + "";

        }
        ret += ">";
        return ret;
    }

    public string GetDisplay_TurnOff(DeviceType aDevice)
    {
        string ret = "";
        int totalLeds = aDevice.LedsPerRow * aDevice.TotalRows;

        // ret = "0^bright=15^time=5^phase=0^total=0^data=414442121212144444441212104440000000044444444001204440000000044000044002104440000000044444440001204440000000044444440002104444444000044000044001204444444000044444444002124444444121244444442121>";
        //   ret = "0^bright=15^time=5^phase=0^total=0^data=000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000>";


        ret = "0^bright=15^time=5^phase=0^total=0^data=";
        for (int c = 0; c < totalLeds; c++)
        {
            ret += "0";

        }
        ret += ">";
        return ret;
    }
    public string GetDisplay_LB()
    {
        string ret = "";

        ret = "0^bright=15^time=5^phase=0^total=7^data=414442121212144444441212104440000000044444444001204440000000044000044002104440000000044444440001204440000000044444440002104444444000044000044001204444444000044444444002124444444121244444442121>id=0^bright=15^time=5^phase=1^total=7^data=213332121212133333311212103330000000033333333001203330000000033000033002103330000000033333330001203330000000033333330002103333333000033000033001203333333000033333333002123333333121233333312121>id=0^bright=15^time=5^phase=2^total=7^data=212121212121212112121212144444444444444444444441244444444444444444444442144444444444444444444441244444444444444444444442144444444444444444444441244444444444444444444442121212121212121212121212>id=0^bright=15^time=5^phase=3^total=7^data=121212121212121212121212244444444444444444444441144444444444444444444442244444444444444444444441144444444444444444444442244444444444444444444441144444444444444444444442212121212121212121212121>id=0^bright=15^time=5^phase=4^total=7^data=213332121212133333311212103334444444433333333401203334444444433444433402103334444444433333334401203334444444433333334402103333333444433444433401203333333444433333333402123333333121233333312121>id=0^bright=15^time=5^phase=5^total=7^data=214442121212144444441212104440000000044444444001204440000000044000044002104440000000044444440001204440000000044444440002104444444000044000044001204444444000044444444002124444444121244444442121>id=0^bright=15^time=5^phase=6^total=7^data=214442121212144444441212104440000000044444444001204440000000044000044002104440000000044444440001204440000000044444440002104444444000044000044001204444444000044444444002124444444121244444442121>id=0^bright=15^time=5^phase=7^total=7^data=213332121212133333311212103330000000033333333001203330000000033000033002103330000000033333330001203330000000033333330002103333333000033000033001203333333000033333333002123333333121233333312121>";
        return ret;
    }


    public class ByteLetter

    {
        public char TheLetter;
        public int[] Bits8x8;
    }
    public class LedPanelHelper
    {
        public enum ScrollDirection
        {
            None,
            Left,
            Right,
            Up,
            Down
        }
        public static List<ScrollPhase> GenerateScroll(DeviceType aDevice, int inLedsPerRow, int inTotalRows, List<int> inData, int inPhases, ScrollDirection scrollDirection, bool wrap, EnumLedColour aColour, EnumLedColour aColourBorder1, EnumLedColour aColourBorder2, int startRow)
        {
            EnumLedColour color1 = aColourBorder1;
            EnumLedColour color2 = aColourBorder2;

            EnumLedColour color1Used = color1;
            EnumLedColour color2Used = color2;

            List<ScrollPhase> scrollPhases = new List<ScrollPhase>();
            inPhases = Mathf.Max(1, inPhases);
            int totalPixelToShiftPerPhase;
            int shiftAmount;

            switch (scrollDirection)
            {
                case ScrollDirection.Left:
                case ScrollDirection.Right:
                    totalPixelToShiftPerPhase = inLedsPerRow / inPhases;
                    shiftAmount = scrollDirection == ScrollDirection.Left ? -totalPixelToShiftPerPhase : totalPixelToShiftPerPhase;
                    break;

                case ScrollDirection.Up:
                case ScrollDirection.Down:
                    totalPixelToShiftPerPhase = inTotalRows / inPhases;
                    shiftAmount = scrollDirection == ScrollDirection.Up ? -totalPixelToShiftPerPhase : totalPixelToShiftPerPhase;
                    break;

                default:
                    totalPixelToShiftPerPhase = 0;
                    shiftAmount = 0;
                    break;
            }

            for (int phase = 0; phase < inPhases; phase++)
            {
                ScrollPhase scrollPhase = new ScrollPhase();
                scrollPhase.phaseNumber = phase;
                scrollPhase.TheData = new List<int>();

                int xPos = 0;
                int yPos = startRow;

                switch (scrollDirection)
                {
                    case ScrollDirection.Left:
                    case ScrollDirection.Right:
                        xPos = phase * shiftAmount;
                        break;

                    case ScrollDirection.Up:
                    case ScrollDirection.Down:
                        yPos = phase * shiftAmount;
                        break;
                }

                scrollPhase.TheData = CreateWindowViewOnData(xPos, yPos, aDevice.LedsPerRow, aDevice.TotalRows, inLedsPerRow, inTotalRows, inData, wrap);

                if ((phase) % 2 == 0)
                {
                    color1Used = color1;
                    color2Used = color2;
                }
                else
                {
                    color1Used = color2;
                    color2Used = color1;

                }
                scrollPhase.TheData = ChangeColours(scrollPhase.TheData, '1', char.Parse((int)aColour + ""));
                scrollPhase.TheData = SetBorderColour(scrollPhase.TheData, aDevice.LedsPerRow, aDevice.TotalRows, '0', char.Parse((int)color1Used + ""), char.Parse((int)color2Used + ""));

                scrollPhases.Add(scrollPhase);
            }

            return scrollPhases;
        }
        public static List<int> CreateWindowViewOnData(int xPos, int yPos, int length, int height, int inLength, int inHeight, List<int> inData, bool wrap)
        {
            List<int> ret = new List<int>();

            for (int row = 0; row < height; row++)
            {
                for (int cell = 0; cell < length; cell++)
                {
                    int x = xPos + cell;
                    int y = yPos + row;

                    if (wrap)
                    {
                        x = (x + inLength) % inLength;
                        y = (y + inHeight) % inHeight;
                    }

                    if (x >= 0 && x < inLength && y >= 0 && y < inHeight)
                    {
                        int index = y * inLength + x;
                        ret.Add(inData[index]);
                    }
                    else
                    {
                        ret.Add(0);
                    }
                }
            }

            return ret;
        }

        public static List<int> CreateWindowViewOnDataOldxx(int xPos, int yPos, int length, int height, int inLength, int inHeight, List<int> inData, bool wrap)
        {
            List<int> ret = new List<int>();
            int curColumn = 0;

            for (int row = 0; row < height; row++)
            {
                for (int cell = 0; cell < length; cell++)
                {
                    int startPixel;
                    if (wrap)
                    {
                        startPixel = (((yPos + row) % inHeight) * inLength) + ((xPos + cell) % inLength);
                    }
                    else
                    {
                        startPixel = (((yPos + row) * inLength) + xPos + cell);
                        if (startPixel >= inData.Count)
                        {
                            ret.Add(0);
                            continue;
                        }
                    }

                    ret.Add(inData[startPixel]);
                }
            }

            return ret;
        }
        public static List<ScrollPhase> GenerateScrollOld(DeviceType aDevice, int inLedsPerRow, int inTotalRows, List<int> inData, int inPhases, ScrollDirection scrollDirection, bool wrap)
        {
            List<ScrollPhase> scrollPhases = new List<ScrollPhase>();
            inPhases = Mathf.Max(1, inPhases);
            int totalPixelToShiftPerPhase;
            int shiftAmount;

            switch (scrollDirection)
            {
                case ScrollDirection.Left:
                case ScrollDirection.Right:
                    totalPixelToShiftPerPhase = inLedsPerRow / inPhases;
                    shiftAmount = scrollDirection == ScrollDirection.Left ? -totalPixelToShiftPerPhase : totalPixelToShiftPerPhase;
                    break;

                case ScrollDirection.Up:
                case ScrollDirection.Down:
                    totalPixelToShiftPerPhase = inTotalRows / inPhases;
                    shiftAmount = scrollDirection == ScrollDirection.Up ? -totalPixelToShiftPerPhase : totalPixelToShiftPerPhase;
                    break;

                default:
                    totalPixelToShiftPerPhase = 0;
                    shiftAmount = 0;
                    break;
            }

            for (int phase = 0; phase < inPhases; phase++)
            {
                ScrollPhase scrollPhase = new ScrollPhase();
                scrollPhase.phaseNumber = phase;
                scrollPhase.TheData = new List<int>();

                int xPos = 0;
                int yPos = 0;

                switch (scrollDirection)
                {
                    case ScrollDirection.Left:
                    case ScrollDirection.Right:
                        xPos = phase * totalPixelToShiftPerPhase;
                        break;

                    case ScrollDirection.Up:
                    case ScrollDirection.Down:
                        yPos = phase * totalPixelToShiftPerPhase;
                        break;
                }

                scrollPhase.TheData = CreateWindowViewOnData(xPos, yPos, aDevice.LedsPerRow, aDevice.TotalRows, inLedsPerRow, inTotalRows, inData, wrap);

                scrollPhases.Add(scrollPhase);
            }

            return scrollPhases;
        }

        public class ScrollPhase
        {
            public int phaseNumber;
            public List<int> TheData;

        }
        public static List<int> GenerateByteLettersCodeForDevice(int NumberOfCells, int NumberOfRows, List<ByteLetter> theLetters)
        {
            List<int> ret = new List<int>();
            int currentLetterNumberRow = -1;

            for (int r = 0; r < NumberOfRows; r++)
            {
                if (r % 8 == 0)
                    currentLetterNumberRow++;

                int currentLetterNumberCell = 0;
                int letterCount = 0;
                for (int c = 0; c < NumberOfCells; c++)
                {
                    if ((c + 1) % 8 == 0)
                    {
                        currentLetterNumberCell++;
                        letterCount = 0;
                    }
                    if (currentLetterNumberCell < theLetters.Count)
                    {
                        int val = theLetters[currentLetterNumberCell].Bits8x8[letterCount + r * 8];
                        letterCount++;
                        ret.Add(val);
                    }
                    else
                    {
                        ret.Add(0);
                    }


                }
            }

            return ret;
        }

        public static List<int> GenerateByteLettersCodeForDevice(DeviceType aDevice, List<ByteLetter> theLetters)
        {
            List<int> ret = new List<int>();
            int currentLetterNumberRow = -1;

            for (int r = 0; r < aDevice.TotalRows; r++)
            {
                if (r % 8 == 0)
                    currentLetterNumberRow++;

                int currentLetterNumberCell = 0;
                int letterCount = 0;
                for (int c = 0; c < aDevice.LedsPerRow; c++)
                {
                    if ((c + 1) % 8 == 0)
                    {
                        currentLetterNumberCell++;
                        letterCount = 0;
                    }
                    if (currentLetterNumberCell < theLetters.Count)
                    {
                        int val = theLetters[currentLetterNumberCell].Bits8x8[letterCount + r * 8];
                        letterCount++;
                        ret.Add(val);
                    }
                    else
                    {
                        ret.Add(0);
                    }


                }
            }

            return ret;
        }
        public static ByteLetter GetByteLetter(char letter)
        {
            ByteLetter ret = new ByteLetter();
            ret.TheLetter = letter;
            ret.Bits8x8 = GetTextBitArray(letter);
            return ret;

        }
        public static List<ByteLetter> GetByteLetters(string letters)
        {
            List<ByteLetter> ret = new List<ByteLetter>();
            for (int c = 0; c < letters.Length; c++)
            {
                ByteLetter tmp = new ByteLetter();
                tmp.TheLetter = letters[c];
                tmp.Bits8x8 = GetTextBitArray(tmp.TheLetter);
                ret.Add(tmp);
            }
            return ret;
        }
        public static int[] GetTextBitArray(char letter)
        {
            int[] ret = new int[64];
            switch (letter)
            {
                case 'A':
                    {

                        ret = new int[]
                        {
                            0,0,0,1,1,0,0,0,
                            0,0,1,1,1,1,0,0,
                            0,1,1,0,0,1,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,1,1,1,1,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,

                        };
                        break;
                    }
                case 'a':
                    {

                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,1,1,1,1,1,1,0,
                            0,0,0,0,0,0,1,0,
                            0,0,1,1,1,1,1,0,
                            0,1,1,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,1,0,0,0,1,0,
                            0,0,1,1,1,1,1,0,

                        };
                        break;
                    }
                case 'B':
                    {
                        ret = new int[]
                        {
                            0,1,1,1,1,1,0,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,1,1,1,1,0,0,
                            0,1,1,1,1,1,0,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,1,1,1,1,0,0,

                        };
                        break;
                    }
                case 'b':
                    {
                        ret = new int[]
                        {
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,1,1,1,1,0,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,1,1,1,1,0,0,

                        };
                        break;
                    }
                case 'C':
                    {
                        ret = new int[]
                        {
                            0,0,1,1,1,1,0,0,
                            0,1,1,0,0,0,1,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,1,0,0,0,1,0,
                            0,0,1,1,1,1,0,0,

                        };
                        break;
                    }
                case 'c':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,1,1,1,1,0,0,
                            0,1,1,0,0,0,1,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,1,0,0,0,1,0,
                            0,0,1,1,1,1,0,0,

                        };
                        break;
                    }
                case 'D':
                    {
                        ret = new int[]
                        {
                            0,1,1,1,1,1,0,0,
                            0,1,0,0,0,1,0,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,1,0,0,
                            0,1,1,1,1,0,0,0,

                        };
                        break;
                    }
                case 'd':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,1,0,
                            0,0,0,0,0,0,1,0,
                            0,0,1,1,1,1,1,0,
                            0,1,1,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,1,0,0,0,1,0,
                            0,0,1,1,1,1,0,0,

                        };
                        break;
                    }
                case 'E':
                    {
                        ret = new int[]
                        {
                            0,1,1,1,1,1,1,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,1,1,1,1,0,0,
                            0,1,1,1,1,1,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,1,1,1,1,1,0,

                        };
                        break;
                    }
                case 'e':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,1,1,1,1,0,0,
                            0,1,1,0,0,1,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,1,1,1,1,1,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,1,0,
                            0,0,1,1,1,1,0,0,

                        };
                        break;
                    }
                case 'F':
                    {
                        ret = new int[]
                        {
                            0,1,1,1,1,1,1,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,1,1,1,1,0,0,
                            0,1,1,1,1,1,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,

                        };
                        break;
                    }
                case 'f':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,1,1,1,1,0,0,
                            0,1,1,0,0,1,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,1,1,1,1,1,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,1,0,
                            0,0,1,1,1,1,0,0,

                        };
                        break;
                    }
                case 'G':
                    {
                        ret = new int[]
                        {
                            0,0,1,1,1,1,1,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,1,1,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,0,1,1,1,1,1,0,

                        };
                        break;
                    }
                case 'g':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,1,1,1,1,1,0,
                            0,1,1,0,0,1,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,1,1,1,1,1,0,
                            0,0,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,0,1,1,1,1,0,0,

                        };
                        break;
                    }
                case 'H':
                    {
                        ret = new int[]
                        {
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,1,1,1,1,1,0,
                            0,1,1,1,1,1,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,

                        };
                        break;
                    }
                case 'h':
                    {
                        ret = new int[]
                        {
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,1,1,1,1,0,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,

                        };
                        break;
                    }
                case 'I':
                    {
                        ret = new int[]
                        {
                            0,1,1,1,1,1,1,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,1,1,1,1,1,1,0,

                        };
                        break;
                    }
                case 'i':
                    {
                        ret = new int[]
                        {
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,

                        };
                        break;
                    }
                case 'J':
                    {
                        ret = new int[]
                        {
                            0,1,1,1,1,1,1,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            1,0,0,1,1,0,0,0,
                            1,1,1,1,1,0,0,0,

                        };
                        break;
                    }
                case 'j':
                    {
                        ret = new int[]
                        {
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            1,1,1,1,1,0,0,0,
                            1,1,1,1,1,0,0,0,

                        };
                        break;
                    }
                case 'K':
                    {
                        ret = new int[]
                        {
                            0,1,0,0,0,1,0,0,
                            0,1,0,0,1,0,0,0,
                            0,1,0,1,0,0,0,0,
                            0,1,1,0,0,0,0,0,
                            0,1,1,0,0,0,0,0,
                            0,1,0,1,0,0,0,0,
                            0,1,0,0,1,0,0,0,
                            0,1,0,0,0,1,0,0,

                        };
                        break;
                    }
                case 'k':
                    {
                        ret = new int[]
                        {
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,1,0,0,
                            0,1,0,0,1,0,0,0,
                            0,1,0,1,0,0,0,0,
                            0,1,1,0,0,0,0,0,
                            0,1,1,1,0,0,0,0,
                            0,1,0,0,1,0,0,0,
                            0,1,0,0,0,1,0,0,

                        };
                        break;
                    }
                case 'L':
                    {
                        ret = new int[]
                        {
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,1,1,1,1,0,0,

                        };
                        break;
                    }
                case 'l':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,1,0,0,0,0,0,

                        };
                        break;
                    }
                case 'M':
                    {
                        ret = new int[]
                        {
                            1,1,0,0,0,0,1,1,
                            1,0,1,0,0,1,0,1,
                            1,0,0,1,1,0,0,1,
                            1,0,0,1,1,0,0,1,
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,

                        };
                        break;
                    }
                case 'm':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,1,1,0,0,1,1,0,
                            1,0,0,1,1,0,0,1,
                            1,0,0,1,1,0,0,1,
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,

                        };
                        break;
                    }
                case 'N':
                    {
                        ret = new int[]
                        {
                            1,0,0,0,0,0,0,1,
                            1,1,0,0,0,0,0,1,
                            1,0,1,0,0,0,0,1,
                            1,0,0,1,0,0,0,1,
                            1,0,0,0,1,0,0,1,
                            1,0,0,0,0,1,0,1,
                            1,0,0,0,0,0,1,1,
                            1,0,0,0,0,0,0,1,

                        };
                        break;
                    }
                case 'n':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            1,0,1,1,1,1,1,0,
                            1,1,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,

                        };
                        break;
                    }
                case 'O':
                    {
                        ret = new int[]
                        {
                            0,0,0,1,1,0,0,0,
                            0,0,1,0,0,1,0,0,
                            0,1,0,0,0,0,1,0,
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,
                            0,1,0,0,0,0,1,0,
                            0,0,1,0,0,1,0,0,
                            0,0,0,1,1,0,0,0,

                        };
                        break;
                    }
                case 'o':
                    {
                        ret = new int[]
                        {
                            0,0,0,1,1,0,0,0,
                            0,0,1,0,0,1,0,0,
                            0,1,0,0,0,0,1,0,
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,
                            0,1,0,0,0,0,1,0,
                            0,0,1,0,0,1,0,0,
                            0,0,0,1,1,0,0,0,

                        };
                        break;
                    }
                case 'P':
                    {
                        ret = new int[]
                         {
                            0,1,1,1,1,1,0,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,0,1,
                            0,1,0,0,0,0,1,0,
                            0,1,1,1,1,1,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,

                        };
                        break;
                    }
                case 'p':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,1,1,1,1,0,0,0,
                            0,1,0,0,0,1,0,0,
                            0,1,0,0,0,1,0,0,
                            0,1,1,1,1,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,

                        };
                        break;
                    }
                case 'R':
                    {
                        ret = new int[]
                         {
                            0,1,1,1,1,1,0,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,0,1,
                            0,1,0,0,0,0,1,0,
                            0,1,1,1,1,1,0,0,
                            0,1,0,0,0,1,0,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,0,1,

                        };
                        break;
                    }
                case 'r':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,1,1,1,1,0,0,0,
                            0,1,0,0,0,1,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,

                        };
                        break;
                    }
                case 'Q':
                    {
                        ret = new int[]
                         {
                            0,0,0,1,1,0,0,0,
                            0,0,1,0,0,1,0,0,
                            0,1,0,0,0,0,1,0,
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,1,0,0,1,
                            0,1,0,0,0,1,1,0,
                            0,0,1,0,0,1,1,0,
                            0,0,0,1,1,0,0,1,

                        };
                        break;
                    }
                case 'q':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,1,0,
                            0,0,1,1,1,1,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,0,1,1,1,1,1,0,
                            0,0,0,0,0,0,1,0,
                            0,0,0,0,0,0,1,0,

                        };
                        break;
                    }
                case 'S':
                    {
                        ret = new int[]
                         {
                            0,0,0,1,1,0,0,0,
                            0,0,1,0,0,1,0,0,
                            0,1,0,0,0,0,1,0,
                            0,0,1,0,0,0,0,0,
                            0,0,0,1,1,1,0,0,
                            0,0,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,0,1,1,1,1,0,0,

                        };
                        break;
                    }
                case 's':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,0,1,1,1,0,0,
                            0,0,1,0,0,0,0,0,
                            0,0,1,0,0,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,0,0,1,0,0,
                            0,0,0,0,0,1,0,0,
                            0,0,1,1,1,0,0,0,

                        };
                        break;
                    }
                case 'T':
                    {
                        ret = new int[]
                        {
                            1,1,1,1,1,1,1,1,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,

                        };
                        break;
                    }
                case 't':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,1,0,0,0,0,
                            0,0,1,1,1,1,0,0,
                            0,0,0,1,0,0,0,0,
                            0,0,0,1,0,0,0,0,
                            0,0,0,1,0,0,0,0,
                            0,0,0,1,1,1,0,0,

                        };
                        break;
                    }
                case 'V':
                    {
                        ret = new int[]
                        {
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,
                            1,1,0,0,0,0,1,1,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,1,0,0,1,1,0,
                            0,0,1,1,1,1,0,0,
                            0,0,0,1,1,0,0,0,

                        };
                        break;
                    }
                case 'v':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,0,1,0,0,1,0,0,
                            0,0,0,1,1,0,0,0,

                        };
                        break;
                    }
                case 'W':
                    {
                        ret = new int[]
                        {
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,
                            1,0,0,1,1,0,0,1,
                            1,0,0,1,1,0,0,1,
                            1,0,1,0,0,1,0,1,
                            0,1,0,0,0,0,1,0,

                        };
                        break;
                    }
                case 'w':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            1,0,0,0,0,0,0,1,
                            1,0,0,1,1,0,0,1,
                            1,0,1,0,0,1,0,1,
                            0,1,0,0,0,0,1,0,

                        };
                        break;
                    }
                case 'U':
                    {
                        ret = new int[]
                        {
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,
                            1,0,0,0,0,0,0,1,
                            0,1,1,0,0,1,1,0,
                            0,0,0,1,1,0,0,0,

                        };
                        break;
                    }
                case 'u':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,0,1,0,0,1,1,0,
                            0,0,0,1,1,0,1,0,

                        };
                        break;
                    }
                case 'X':
                    {
                        ret = new int[]
                        {
                            1,0,0,0,0,0,0,1,
                            0,1,0,0,0,0,1,0,
                            0,0,1,0,0,1,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,1,0,0,1,0,0,
                            0,1,0,0,0,0,1,0,
                            1,0,0,0,0,0,0,1,

                        };
                        break;
                    }
                case 'x':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,1,0,0,0,0,1,0,
                            0,0,1,0,0,1,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,1,0,0,1,0,0,
                            0,1,0,0,0,0,1,0,

                        };
                        break;
                    }
                case 'Y':
                    {
                        ret = new int[]
                        {
                            1,0,0,0,0,0,0,1,
                            0,1,0,0,0,0,1,0,
                            0,0,1,0,0,1,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,

                        };
                        break;
                    }
                case 'y':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,1,0,0,0,0,1,
                            0,0,0,1,0,0,1,0,
                            0,0,0,0,1,1,0,0,
                            0,0,0,0,1,1,0,0,
                            0,0,0,0,1,0,0,0,
                            0,0,0,1,0,0,0,0,
                            0,0,1,0,0,0,0,0,

                        };
                        break;
                    }
                case 'Z':
                    {
                        ret = new int[]
                        {
                            1,1,1,1,1,1,1,1,
                            0,0,0,0,0,0,1,0,
                            0,0,0,0,0,1,0,0,
                            0,0,0,0,1,0,0,0,
                            0,0,0,1,0,0,0,0,
                            0,0,1,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            1,1,1,1,1,1,1,1,

                        };
                        break;
                    }
                case 'z':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,1,1,1,1,1,1,0,
                            0,0,0,0,0,1,0,0,
                            0,0,0,1,0,0,0,0,
                            0,0,1,0,0,0,0,0,
                            0,1,1,1,1,1,1,0,

                        };
                        break;
                    }
                case '0':
                    {
                        ret = new int[]
                        {
                            0,0,0,1,1,0,0,0,
                            0,0,1,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            1,0,0,1,1,0,0,1,
                            1,0,0,1,1,0,0,1,
                            0,1,0,0,0,0,1,0,
                            0,0,1,0,0,1,0,0,
                            0,0,0,1,1,0,0,0,

                        };
                        break;
                    }
                case '1':
                    {
                        ret = new int[]
                        {
                            0,1,1,1,1,0,0,0,
                            0,1,1,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,1,1,1,1,1,1,0,

                        };
                        break;
                    }
                case '2':
                    {
                        ret = new int[]
                        {
                            0,0,1,1,1,0,0,0,
                            0,1,0,0,0,1,0,0,
                            1,0,0,0,0,0,1,0,
                            1,0,0,0,0,0,1,0,
                            0,0,1,1,1,1,0,0,
                            0,1,0,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,
                            0,0,1,1,1,1,1,0,

                        };
                        break;
                    }
                case '3':
                    {
                        ret = new int[]
                        {
                            0,1,1,1,1,1,1,0,
                            0,0,0,0,0,0,0,1,
                            0,0,0,0,0,0,0,1,
                            0,0,0,0,1,1,1,0,
                            0,0,0,0,1,1,1,0,
                            0,0,0,0,0,0,0,1,
                            0,0,0,0,0,0,0,1,
                            0,1,1,1,1,1,1,0,

                        };
                        break;
                    }
                case '4':
                    {
                        ret = new int[]
                        {
                            0,0,0,0,0,1,1,0,
                            0,0,0,0,1,0,1,0,
                            0,0,0,1,0,0,1,0,
                            0,0,1,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,1,1,1,1,1,1,1,
                            0,0,0,0,0,0,1,0,
                            0,0,0,0,0,0,1,0,

                        };
                        break;
                    }
                case '5':
                    {
                        ret = new int[]
                        {
                            0,1,1,1,1,1,1,0,
                            0,1,0,0,0,0,0,0,
                            0,1,1,0,0,0,0,0,
                            0,0,0,1,1,0,0,0,
                            0,0,0,0,0,1,1,0,
                            0,0,0,0,0,0,1,0,
                            0,0,0,0,0,0,1,0,
                            0,1,1,1,1,1,0,0,

                        };
                        break;
                    }
                case '6':
                    {
                        ret = new int[]
                        {
                            0,0,1,1,1,1,0,0,
                            0,0,1,0,0,0,0,0,
                            0,0,1,0,0,0,0,0,
                            0,0,1,1,1,1,0,0,
                            0,0,1,0,0,0,1,0,
                            0,0,1,0,0,0,1,0,
                            0,0,1,0,0,0,1,0,
                            0,0,0,1,1,1,0,0,

                        };
                        break;
                    }
                case '7':
                    {
                        ret = new int[]
                        {
                            0,1,1,1,1,1,1,1,
                            0,0,0,0,0,0,0,1,
                            0,0,0,0,0,0,1,0,
                            0,0,0,0,0,1,0,0,
                            0,0,0,0,1,0,0,0,
                            0,0,0,1,0,0,0,0,
                            0,0,1,0,0,0,0,0,
                            0,1,0,0,0,0,0,0,

                        };
                        break;
                    }
                case '8':
                    {
                        ret = new int[]
                        {
                            0,0,1,1,1,1,0,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,0,1,1,1,1,0,0,
                            0,0,1,1,1,1,0,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,0,1,1,1,1,0,0,

                        };
                        break;
                    }
                case '9':
                    {
                        ret = new int[]
                        {
                            0,0,1,1,1,1,0,0,
                            0,1,0,0,0,0,1,0,
                            0,1,0,0,0,0,1,0,
                            0,0,1,0,0,0,1,0,
                            0,0,0,1,1,1,0,0,
                            0,0,0,0,1,0,0,0,
                            0,0,0,1,0,0,0,0,
                            0,0,1,0,0,0,0,0,

                        };
                        break;
                    }
                case ':':
                    {
                        //todo
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,

                        };
                        break;
                    }
                case '/':
                    {
                        //todo
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,

                        };
                        break;
                    }
                case '\\':
                    {
                        //todo
                        ret = new int[]
                        {
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,

                        };
                        break;
                    }
            }

            return ret;
        }
        public static int[] RandomiseColours(int[] inp)
        {

            List<int> outp = new List<int>();
            for (int c = 0; c < inp.Length; c++)
            {
                if (inp[c] != 0)
                {
                    outp.Add(Random.Range(1, 4));
                }
                else
                {
                    outp.Add(0);

                }
            }

            return outp.ToArray();
        }
        public static List<DeviceType> GetAllDeviceTypes()
        {
            List<DeviceType> ret = new List<DeviceType>();
            {
                DeviceType dv = new DeviceType();
                dv.EnumDeviceType = EnumDeviceTypes.LB_24x8;
                dv.Name = "LB 24x8";
                dv.LedsPerRow = 24;
                dv.TotalRows = 8;
                ret.Add(dv);
            }
            {
                DeviceType dv = new DeviceType();
                dv.EnumDeviceType = EnumDeviceTypes.LB_50x10;
                dv.Name = "LB 50x10";
                dv.LedsPerRow = 50;
                dv.TotalRows = 10;
                ret.Add(dv);
            }
            return ret;
        }
        public static List<LedColour> GetAllLedColours()
        {
            List<LedColour> ret = new List<LedColour>();
            {
                LedColour dv = new LedColour();
                dv.EnumColor = EnumLedColour.Off;
                dv.ColourName = "OFF";
                ret.Add(dv);
            }
            {
                LedColour dv = new LedColour();
                dv.EnumColor = EnumLedColour.Blue;
                dv.ColourName = "BLUE";
                ret.Add(dv);
            }
            {
                LedColour dv = new LedColour();
                dv.EnumColor = EnumLedColour.Red;
                dv.ColourName = "RED";
                ret.Add(dv);
            }
            {
                LedColour dv = new LedColour();
                dv.EnumColor = EnumLedColour.Purple;
                dv.ColourName = "PURPLE";
                ret.Add(dv);
            }
            {
                LedColour dv = new LedColour();
                dv.EnumColor = EnumLedColour.Green;
                dv.ColourName = "GREEN";
                ret.Add(dv);
            }
            {
                LedColour dv = new LedColour();
                dv.EnumColor = EnumLedColour.Yellow;
                dv.ColourName = "Yellow";
                ret.Add(dv);
            }
            {
                LedColour dv = new LedColour();
                dv.EnumColor = EnumLedColour.Orange;
                dv.ColourName = "Orange";
                ret.Add(dv);
            }
            {
                LedColour dv = new LedColour();
                dv.EnumColor = EnumLedColour.Cyan;
                dv.ColourName = "Cyan";
                ret.Add(dv);
            }
            {
                LedColour dv = new LedColour();
                dv.EnumColor = EnumLedColour.Magenta;
                dv.ColourName = "Magenta";
                ret.Add(dv);
            }
            {
                LedColour dv = new LedColour();
                dv.EnumColor = EnumLedColour.Turquoise;
                dv.ColourName = "Turquoise";
                ret.Add(dv);
            }
            {
                LedColour dv = new LedColour();
                dv.EnumColor = EnumLedColour.White;
                dv.ColourName = "White";
                ret.Add(dv);
            }


            return ret;
        }
        public static List<LedScroll> GetAllScrollOptions()
        {
            List<LedScroll> ret = new List<LedScroll>();
            {
                LedScroll dv = new LedScroll();
                dv.DirectionName = "None";
                dv.TheDirection = ScrollDirection.None;
                ret.Add(dv);
            }
            {
                LedScroll dv = new LedScroll();
                dv.DirectionName = "Left";
                dv.TheDirection = ScrollDirection.Left;
                ret.Add(dv);
            }
            {
                LedScroll dv = new LedScroll();
                dv.DirectionName = "Right";
                dv.TheDirection = ScrollDirection.Right;
                ret.Add(dv);
            }
            {
                LedScroll dv = new LedScroll();
                dv.DirectionName = "Up";
                dv.TheDirection = ScrollDirection.Up;
                ret.Add(dv);
            }
            {
                LedScroll dv = new LedScroll();
                dv.DirectionName = "Down";
                dv.TheDirection = ScrollDirection.Down;
                ret.Add(dv);
            }


            return ret;
        }

        public static List<int> ChangeColours(List<int> inData, char fromColour, char toColour)
        {
            List<int> ret = new List<int>();
            for (int c = 0; c < inData.Count; c++)
            {
                if (inData[c] == int.Parse(fromColour + "")) ret.Add(int.Parse(toColour + ""));
                else
                {
                    ret.Add(inData[c]);

                }
            }

            return ret;
        }
        public static List<int> SetBorderColour(List<int> inData, int ledsPerRow, int totalRows, char fromColour, char toColour, char toColour2)
        {
            List<int> ret = new List<int>();
            int width = ledsPerRow; // Width excluding the border
            int height = totalRows; // Height excluding the border

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    // Check if it's a border pixel
                    if (row == 0 || row == height - 1 || col == 0 || col == width - 1)
                    {
                        // Alternate between the two border colors

                        int dataIndex = row * ledsPerRow + col;
                        int color = inData[dataIndex];
                        if (color == int.Parse(fromColour + ""))
                        {
                            char borderColour = (row + col) % 2 == 0 ? toColour : toColour2;
                            ret.Add(int.Parse(borderColour.ToString()));
                        }
                        else
                        {
                            ret.Add(color);
                        }
                    }
                    else
                    {
                        // Not a border pixel, copy the corresponding input data
                        int dataIndex = row * ledsPerRow + col;
                        if (dataIndex < inData.Count)
                        {
                            int color = inData[dataIndex];
                            ret.Add(color);
                        }
                        else
                        {
                            // Handle the case when there is no corresponding input data
                            ret.Add(0); // Assuming the default value is 0 for empty pixels
                        }
                    }
                }
            }

            return ret;
        }
        public static List<int> SetBorderColourOld(List<int> inData, int ledsPerRow, int totalRows, char fromColour, char toColour, char toColour2)
        {
            List<int> ret = new List<int>();
            for (int c = 0; c < inData.Count; c++)
            {
                if (inData[c] == int.Parse(fromColour + "")) ret.Add(int.Parse(toColour + ""));
                else
                {
                    ret.Add(inData[c]);

                }
            }

            return ret;
        }
    }

    public enum EnumLedColour
    {
        Off = 0,
        Blue = 1,
        Red = 2,
        Purple = 3,
        Green = 4,
        Yellow = 5,
        Orange = 6,
        Cyan = 7,
        Magenta = 8,
        Turquoise = 9,
        White = 10

    }
    public enum EnumDeviceTypes
    {
        LB_24x8,
        LB_50x10

    }
    public class DeviceType
    {
        public string Name;
        public EnumDeviceTypes EnumDeviceType;
        public int LedsPerRow;
        public int TotalRows;


    }
    public class LedColour
    {
        public string ColourName;
        public EnumLedColour EnumColor;


    }
    public class LedScroll
    {
        public string DirectionName;
        public LedPanelHelper.ScrollDirection TheDirection;


    }
}
