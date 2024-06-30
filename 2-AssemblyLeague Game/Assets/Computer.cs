using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Computer : MonoBehaviour
{
    public RobotMeta myRobotMeta;
    public StandardStack runtimeStack = new StandardStack();
    public InterrupHandler TheInterruptHandler;
    public Program DeviceProgram;
    public RobotPart Part;
    public string CodeText;
    public string OutText;
    public bool IsEnabled = false;
    float MilliSecondsDelayPerCode = 0;
    System.DateTime nextExecutationTime = System.DateTime.Now;

    public Match CurrentMatch;
    float InstructrionsPerSecond = 25.50f;
    float InstructionsAllowed = 0;
    public void Start()
    {
        myRobotMeta = gameObject.GetComponent<RobotMeta>();
        MilliSecondsDelayPerCode = Random.Range(1, 20);
        //  SecondsDelayPerCode = 0;
        runtimeStack = new StandardStack();

        DeviceProgram = new Program();
        DeviceProgram.ProgramName = "Test Program";
        DeviceProgram.Functions = new List<Function>();
        DeviceProgram.ID = Random.Range(0, 1000);
        DeviceProgram.TheThread = new ThreadHandler();
        TheInterruptHandler = new InterrupHandler();
         TheInterruptHandler.part = Part;

     }
    public IOFinder anIO;
    public void StepExecute()
    {
        ////load stack for each step stop
        Function main = new Function();
        main.FunctionName = "main";
        main.ID =
        main.ParentID = DeviceProgram.ID;
        main.CodeLines = new List<CodeLine>();
        anIO = new IOFinder();
        anIO.AllIOHandlers = new List<IOHandler>();
        anIO.AllIOHandlers.AddRange(gameObject.GetComponentsInChildren<IOHandler>());

        string[] linesText = CodeText.Split(new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        List<string> lastLabels = new List<string>();
        for (int c = 0; c < linesText.Length; c++)
        {
            CodeLine aLine = CodeLine.ParseFromText(linesText[c]);
            aLine.ExtractMetaFirstPass();
            if (aLine.IsSkipable == false)
            {
                if (lastLabels.Count > 0)
                {
                    aLine.LabelName = lastLabels;
                       lastLabels = new List<string>();
                }
                main.CodeLines.Add(aLine);
            }
            else
            {
                 if (aLine.LabelName != null)
                {
                    lastLabels.AddRange(aLine.LabelName);
                }
            }
        }

        DeviceProgram.Functions = new List<Function>();
        DeviceProgram.Functions.Add(main);
        {
            Function test = new Function();
            test.FunctionName = "test";
            test.ID = Random.Range(0, 1000);
            test.ParentID = DeviceProgram.ID;
            test.CodeLines = new List<CodeLine>();
            test.CodeLines.Add(CodeLine.ParseFromText("add ax 1000"));
            test.CodeLines.Add(CodeLine.ParseFromText("call test2"));
            test.CodeLines.Add(CodeLine.ParseFromText("add ax -5000"));
            test.CodeLines.Add(CodeLine.ParseFromText("call test2"));

            DeviceProgram.Functions.Add(test);
        }

        {
            Function test = new Function();
            test.FunctionName = "test2";
            test.ID = Random.Range(0, 1000);
            test.ParentID = DeviceProgram.ID;
            test.CodeLines = new List<CodeLine>();
            test.CodeLines.Add(CodeLine.ParseFromText("add ax 5000"));
            test.CodeLines.Add(CodeLine.ParseFromText("add ax 5000"));
            test.CodeLines.Add(CodeLine.ParseFromText("add ax 5000"));

            DeviceProgram.Functions.Add(test);
        }

        DeviceProgram.TheThread.ContinueCallStack(DeviceProgram, ref runtimeStack, TheInterruptHandler, anIO);
       
        string outp = "";
        outp += "Ax: " + runtimeStack.Ax.Val + "\n";
        outp += "Bx: " + runtimeStack.Bx.Val + "\n";
        outp += "Cx: " + runtimeStack.Cx.Val + "\n";
        outp += "Dx: " + runtimeStack.Dx.Val + "\n";
        outp += "Ex: " + runtimeStack.Ex.Val + "\n";
        outp += "Fx: " + runtimeStack.Fx.Val + "\n";
        outp += "Gx: " + runtimeStack.Gx.Val + "\n";
        outp += "Ip: " + runtimeStack.IP.Val + "\n\n";
        OutText = outp;

    }

    public class CodeGeneration
    {
        public List<CodeGenCondition> Coditions;
        public List<CodeGenAction> Actions;
        public static InterrupHandler.Interrupt GetInterruptNumberForTarget(CodeGenCondition.EnumTarget aTarget)
        {
            InterrupHandler.Interrupt ret = InterrupHandler.Interrupt.I_101_ScanClosestLocation;
            ret = ConvertEnumTextToInterrupt(aTarget.ToString());
            return ret;
        }
        public static InterrupHandler.Interrupt GetInterruptNumberForTarget(CodeGenAction.EnumActionTarget aTarget)
        {
            InterrupHandler.Interrupt ret = InterrupHandler.Interrupt.I_101_ScanClosestLocation;


            ret = ConvertEnumTextToInterrupt(aTarget.ToString());
            return ret;
        }
        public static InterrupHandler.Interrupt ConvertEnumTextToInterrupt(string inText)
        {
            InterrupHandler.Interrupt ret = InterrupHandler.Interrupt.I_904_ClearCom;
            ret = (InterrupHandler.Interrupt)System.Enum.Parse(typeof(InterrupHandler.Interrupt), inText);
            return ret;

        }
        private List<string> GenerateIfLineOfCode(CodeGenCondition aCond, string labelEnd)
        {
            print("aCond TARGET IS " + aCond.Target);
            List<string> linesOfCode = new List<string>();
            {
                {
                    string codeLine = CodeLine.EnumCommand.INT + " " + (int)GetInterruptNumberForTarget(aCond.Target);
                    linesOfCode.Add(codeLine);
                }


                InterrupHandler.Interrupt anInt = ConvertEnumTextToInterrupt(aCond.Attribute.ToString());// (InterrupHandler.Interrupt)val;
                #region Generated ASM Code
                //moce id into ax
                {
                    string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Ax + " " + StandardStack.EnumRegisters.Ex;
                    linesOfCode.Add(codeLine);
                }
                if(aCond.NumberOfValues>1)
                {
                    {
                        string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Bx + " " + StandardStack.EnumRegisters.Fx;
                        linesOfCode.Add(codeLine);
                    }
                }
                if (aCond.NumberOfValues > 2)
                {
                    {
                        string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Cx + " " + StandardStack.EnumRegisters.Gx;
                        linesOfCode.Add(codeLine);
                    }
                }
                // get info on id
                {
                    string codeLine = CodeLine.EnumCommand.INT + " " + (int)anInt;
                    linesOfCode.Add(codeLine);
                }

                //store value in ax
                {
                    string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Ax + " " + StandardStack.EnumRegisters.Ex;
                    linesOfCode.Add(codeLine);
                }

                //Do Any Extras



                //at this point the ID of the closest/healtheist/etc id is in EX
                switch (aCond.Attribute)
                {

                    default:
                        {
                            break;
                        }

                }

                //do comparison
                {
                    string valToCompareTo = aCond.SingleValue;
                    string codeLine = CodeLine.EnumCommand.CMP + " " + StandardStack.EnumRegisters.Ax + " " + valToCompareTo;
                    linesOfCode.Add(codeLine);
                }
                #endregion






                CodeLine.EnumCommand commandToUse = DetermineCheckCommandIf(aCond.ConditionCheck);



                {
                    string codeLine = commandToUse + " " + labelEnd;
                    linesOfCode.Add(codeLine);
                }

            }
            return linesOfCode;
        }
        private List<string> GenerateWhileLineOfCode(CodeGenCondition aCond, string labelStart)
        {
             List<string> linesOfCode = new List<string>();
            {
                {
                    string codeLine = CodeLine.EnumCommand.INT + " " + (int)GetInterruptNumberForTarget(aCond.Target);
                    linesOfCode.Add(codeLine);
                }


                // int val = (int)aCond.Attribute;
                InterrupHandler.Interrupt anInt = ConvertEnumTextToInterrupt(aCond.Attribute.ToString());// (InterrupHandler.Interrupt)val;
                #region Generated ASM Code
                //moce id into ax
                {
                    string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Ax + " " + StandardStack.EnumRegisters.Ex;
                    linesOfCode.Add(codeLine);
                }
                // get info on id
                {
                    string codeLine = CodeLine.EnumCommand.INT + " " + (int)anInt;
                    linesOfCode.Add(codeLine);
                }

                //store value in ax
                {
                    string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Ax + " " + StandardStack.EnumRegisters.Ex;
                    linesOfCode.Add(codeLine);
                }

                //Do Any Extras



                //at this point the ID of the closest/healtheist/etc id is in EX
                switch (aCond.Attribute)
                {

                    default:
                        {
                            break;
                        }

                }

                //do comparison
                {
                    string valToCompareTo = aCond.SingleValue;
                    string codeLine = CodeLine.EnumCommand.CMP + " " + StandardStack.EnumRegisters.Ax + " " + valToCompareTo;
                    linesOfCode.Add(codeLine);
                }
                #endregion






                CodeLine.EnumCommand commandToUse = DetermineCheckCommandWhile(aCond.ConditionCheck);



                {
                    string codeLine = commandToUse + " " + labelStart;
                    linesOfCode.Add(codeLine);
                }

            }
            return linesOfCode;
        }
        public string GenerateCode(string codeName)
        {
            string ret = "";
            List<string> linesOfCode = new List<string>();

            string labelStart = codeName + "_START";
            string labelEnd = codeName + "_END";
            string labelAction = codeName + "_ACTION";
            //start
            {
                string codeLine = CodeLine.EnumCommand.LBL + " " + labelStart;
                linesOfCode.Add(codeLine);
            }


            for (int c = 0; c < Coditions.Count; c++)
            {
                if (Coditions[c].ConditionType == CodeGenCondition.EnumConditionType.If)
                {

                    linesOfCode.AddRange(GenerateIfLineOfCode(Coditions[c], labelEnd));

                }
            }
            {
                string codeLine = CodeLine.EnumCommand.LBL + " " + labelAction;
                linesOfCode.Add(codeLine);
            }
            #region Code ACTION

            for (int c = 0; c < Actions.Count; c++)
            {

                //select target

                switch (Actions[c].Target)
                {
                    case CodeGenAction.EnumActionTarget.None:
                        {
                            break;
                        }
                    case CodeGenAction.EnumActionTarget.SpecificAngle:
                        {
                            if (Actions[c].SingleValue.Length > 0)
                            {
                                {
                                    string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Ex + " " + Actions[c].SingleValue;
                                    linesOfCode.Add(codeLine);
                                }
                            }
                            else
                            {
                               

                                if(Actions[c].ThreeValue1.Length == 0 && Actions[c].ThreeValue2.Length == 0)
                                {
                                    if (Actions[c].DropdownValue >= 0)
                                    {
                                        {
                                            string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Ex + " " + Actions[c].DropdownValue;
                                            linesOfCode.Add(codeLine);
                                        }
                                    }

                                }else
                                {

                                    if (Actions[c].ThreeValue1.Length > 0)
                                    {
                                        {
                                            string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Ex + " " + Actions[c].ThreeValue1;
                                            linesOfCode.Add(codeLine);
                                        }
                                    }else
                                    {

                                         {
                                            string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Ex + " " + "0";
                                            linesOfCode.Add(codeLine);
                                        }
                                    }
                                    if (Actions[c].ThreeValue2.Length > 0)
                                    {
                                        {
                                            string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Fx + " " + Actions[c].ThreeValue2;
                                            linesOfCode.Add(codeLine);
                                        }
                                    }else
                                    {
                                        {
                                            string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Fx + " " +"0";
                                            linesOfCode.Add(codeLine);
                                        }
                                    }
                                }

                            }


                            break;
                        }
                    case CodeGenAction.EnumActionTarget.SpecificPosition:
                        {
                            {
                                if (Actions[c].ThreeValue1.Length == 0) Actions[c].ThreeValue1 = "0";
                                string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Ex + " " + Actions[c].ThreeValue1;
                                linesOfCode.Add(codeLine);
                            }
                            {
                                if (Actions[c].ThreeValue2.Length == 0) Actions[c].ThreeValue2 = "0";
                                string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Fx + " " + Actions[c].ThreeValue2;
                                linesOfCode.Add(codeLine);
                            }
                            {
                                if (Actions[c].ThreeValue3.Length == 0) Actions[c].ThreeValue3 = "0";
                                string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Gx + " " + Actions[c].ThreeValue3;
                                linesOfCode.Add(codeLine);
                            }
                            break;
                        }
                    case CodeGenAction.EnumActionTarget.SpecificValue:
                        {
                            if (Actions[c].SingleValue.Length > 0)
                            {
                                {
                                    string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Ex + " " + Actions[c].SingleValue;
                                    linesOfCode.Add(codeLine);
                                }
                            }
                            else
                            {
                                if (Actions[c].DropdownValue >= 0)
                                {
                                    {
                                        string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Ex + " " + Actions[c].DropdownValue;
                                        linesOfCode.Add(codeLine);
                                    }
                                }

                            }
                            break;
                        }
                    case CodeGenAction.EnumActionTarget.DropDown:
                        {
                             {
                                if (Actions[c].DropdownValue >= 0)
                                {
                                    {
                                        string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Ex + " " + Actions[c].DropdownValue;
                                        linesOfCode.Add(codeLine);
                                    }
                                }

                            }
                            break;
                        }
                    default:
                        {
                            {
                                string codeLine = CodeLine.EnumCommand.INT + " " + (int)GetInterruptNumberForTarget(Actions[c].Target);
                                linesOfCode.Add(codeLine);
                            }
                            {
                                string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Ax + " " + StandardStack.EnumRegisters.Ex;
                                linesOfCode.Add(codeLine);
                            }
                            {
                                string codeLine = CodeLine.EnumCommand.INT + " " + (int)InterrupHandler.Interrupt.I_201_TargetInfo_Location;
                                linesOfCode.Add(codeLine);
                            }
                            break;
                        }
                }


                //select values
                switch (Actions[c].TheFunction.InputVariables.DataSetType)
                {
                    case ComponentType.IoFunctionMeta.EnumIODataSetType.WorldPositionBxCxDx:
                        {

                            {
                                string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Bx + " " + StandardStack.EnumRegisters.Ex;
                                linesOfCode.Add(codeLine);
                            }
                            {
                                string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Cx + " " + StandardStack.EnumRegisters.Fx;
                                linesOfCode.Add(codeLine);
                            }
                            {
                                string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Dx + " " + StandardStack.EnumRegisters.Gx;
                                linesOfCode.Add(codeLine);
                            }
                            break;
                        }
                    case ComponentType.IoFunctionMeta.EnumIODataSetType.AngleBx:
                        {

                            {
                                string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Bx + " " + StandardStack.EnumRegisters.Ex;
                                linesOfCode.Add(codeLine);
                            }
                            {
                                string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Cx + " " + StandardStack.EnumRegisters.Fx;
                                linesOfCode.Add(codeLine);
                            }
                            break;
                        }
                    case ComponentType.IoFunctionMeta.EnumIODataSetType.DirectionBxCxDx:
                        {

                            {
                                string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Bx + " " + StandardStack.EnumRegisters.Ex;
                                linesOfCode.Add(codeLine);
                            }
                            {
                                string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Cx + " " + StandardStack.EnumRegisters.Fx;
                                linesOfCode.Add(codeLine);
                            }
                            {
                                string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Dx + " " + StandardStack.EnumRegisters.Gx;
                                linesOfCode.Add(codeLine);
                            }

                            break;
                        }
                    case ComponentType.IoFunctionMeta.EnumIODataSetType.DropDownBx:
                        {

                            {
                                string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Bx + " " + StandardStack.EnumRegisters.Ex;
                                linesOfCode.Add(codeLine);
                            }

                            break;
                        }
                    case ComponentType.IoFunctionMeta.EnumIODataSetType.NumberBx:
                        {

                            {
                                string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Bx + " " + StandardStack.EnumRegisters.Ex;
                                linesOfCode.Add(codeLine);
                            }

                            break;
                        }
                    case ComponentType.IoFunctionMeta.EnumIODataSetType.NoExtra:
                        {

                            {
                                //string codeLine = CodeLine.EnumCommand.MOV + " " + StandardStack.EnumRegisters.Bx + " " + StandardStack.EnumRegisters.Ex;
                                // linesOfCode.Add(codeLine);
                            }

                            break;
                        }
                    default:
                        {
                            throw new System.NotImplementedException();
                            break;
                        }

                }
                //pass values to correct function
                {
                    string codeLine = CodeLine.EnumCommand.MOV + " " + Actions[c].TheFunction.InputVariables.InputVariableMain.Register + " " + Actions[c].TheFunction.InputVariables.InputVariableMainValue;
                    linesOfCode.Add(codeLine);
                }
                {
                    string codeLine = CodeLine.EnumCommand.IO + " " + Actions[c].TheIONumber;
                    linesOfCode.Add(codeLine);
                }
                // pass values to correct IO device
            }

            #endregion


            for (int c = 0; c < Coditions.Count; c++)
            {


                if (Coditions[c].ConditionType == CodeGenCondition.EnumConditionType.While)
                {

                    linesOfCode.AddRange(GenerateWhileLineOfCode(Coditions[c], labelStart));


                }
            }




            //end
            {
                string codeLine = CodeLine.EnumCommand.LBL + " " + labelEnd;
                linesOfCode.Add(codeLine);
            }

            for (int c = 0; c < linesOfCode.Count; c++)
            {
                if (c > 0) ret += "\n";
                ret += linesOfCode[c];

            }
            ret += "\n";

            return ret;
        }

        CodeLine.EnumCommand DetermineCheckCommandIf(CodeGenCondition.EnumConditionCheck aCom)
        {
            CodeLine.EnumCommand commandToUse = CodeLine.EnumCommand.JMP;

            switch (aCom)
            {
                case CodeGenCondition.EnumConditionCheck.EqualTo:
                    {
                        commandToUse = CodeLine.EnumCommand.JNE;
                        break;
                    }
                case CodeGenCondition.EnumConditionCheck.GreaterThan:
                    {
                        commandToUse = CodeLine.EnumCommand.JLE;
                        break;
                    }
                case CodeGenCondition.EnumConditionCheck.GreaterThanOrEqualTo:
                    {
                        commandToUse = CodeLine.EnumCommand.JLS;
                        break;
                    }
                case CodeGenCondition.EnumConditionCheck.LessThan:
                    {
                        commandToUse = CodeLine.EnumCommand.JGE;
                        break;
                    }
                case CodeGenCondition.EnumConditionCheck.LessThanOrEqualTo:
                    {
                        commandToUse = CodeLine.EnumCommand.JGR;
                        break;
                    }
                case CodeGenCondition.EnumConditionCheck.NotEqualTo:
                    {
                        commandToUse = CodeLine.EnumCommand.JE;
                        break;
                    }

            }
            return commandToUse;
        }
        CodeLine.EnumCommand DetermineCheckCommandWhile(CodeGenCondition.EnumConditionCheck aCom)
        {
            CodeLine.EnumCommand commandToUse = CodeLine.EnumCommand.JMP;

            switch (aCom)
            {
                case CodeGenCondition.EnumConditionCheck.EqualTo:
                    {
                        commandToUse = CodeLine.EnumCommand.JE;
                        break;
                    }
                case CodeGenCondition.EnumConditionCheck.GreaterThan:
                    {
                        commandToUse = CodeLine.EnumCommand.JGR;
                        break;
                    }
                case CodeGenCondition.EnumConditionCheck.GreaterThanOrEqualTo:
                    {
                        commandToUse = CodeLine.EnumCommand.JGE;
                        break;
                    }
                case CodeGenCondition.EnumConditionCheck.LessThan:
                    {
                        commandToUse = CodeLine.EnumCommand.JLS;
                        break;
                    }
                case CodeGenCondition.EnumConditionCheck.LessThanOrEqualTo:
                    {
                        commandToUse = CodeLine.EnumCommand.JLE;
                        break;
                    }
                case CodeGenCondition.EnumConditionCheck.NotEqualTo:
                    {
                        commandToUse = CodeLine.EnumCommand.JNE;
                        break;
                    }

            }
            return commandToUse;
        }
    }
    public class CodeGenCondition
    {
        public EnumConditionType ConditionType;
        public EnumAttribute Attribute;
        public EnumTarget Target;
        public EnumConditionCheck ConditionCheck;
        public int NumberOfValues = 1;
        public string SingleValue;
        public string SingleValue2;
        public string SingleValue3;
        public string GetHumanText()
        {
            string ret = "";          

            ret += Computer.CodeGenCondition.GetDisplayText(ConditionType)
            + Computer.CodeGenCondition.GetDisplayText(Attribute)
            + Computer.CodeGenCondition.GetDisplayText(Target)
            + Computer.CodeGenCondition.GetDisplayText(ConditionCheck)
             + " " + SingleValue
                 + " ";

            return ret;
        }



        public enum EnumConditionType
        {
            If,
            While
        }
        public enum EnumAttribute
        {

            //   I_201_TargetInfo_Location = 201,
            I_202_TargetInfo_TotalHealth,
            I_203_TargetInfo_TotalArmour,
            I_204_TargetInfo_TotalHeat,
            I_205_TargetInfo_TotalLinesOfCode,
            I_206_TargetInfo_TotalLinesOfCodePerSecond,
            I_207_TargetInfo_TotalPower,
            I_208_TargetInfo_TotalWeight,
            I_209_TargetInfo_MaxSpeed,
            I_210_TargetInfo_MaxAcceleration,
            I_211_TargetInfo_RotationSpeed,
            I_212_TargetInfo_CurrentSpeedPercentage,
            I_213_TargetInfo_CurrentSpeedVelocity,
            I_214_TargetInfo_DistanceTo,
            //  I_215_TargetInfo_LookAtLocation,
            I_216_TargetInfo_AngleBetweenOnYAxis,
            I_217_TargetInfo_AngleBetweenOnZAxis,
            I_218_TargetInfo_AverageAccuracy,
            I_219_TargetInfo_PhyicalSize,
            I_220_TargetInfo_TotalNumberOfCockpits,
            I_221_TargetInfo_TotalNumberOfShoulders,
            I_222_TargetInfo_TotalNumberOfBackpacks,
            I_223_TargetInfo_TotalNumberOfWeapons,
            I_224_TargetInfo_TotalNumberOfEMPWeapons,
            I_225_TargetInfo_TotalNumberOfBallisticWeapons,
            I_226_TargetInfo_TotalNumberOfRockets,
            I_227_TargetInfo_TotalNumberOfMeleeWeapons,
            I_228_TargetInfo_TotalNumberOfLaserWeapons,
            I_229_TargetInfo_TotalNumberOfFireWeapons,
            I_230_TargetInfo_TotalNumberOfMineLayers,
           // I_231_OwnInfo_Position,



        }
        public enum EnumTarget
        {
            I_02_OwnID,
            I_101_ScanClosestLocation,
            I_102_ScanFurthestLocation,
            I_103_ScanShotsFired,
            I_104_ScanLeastShotsFired,
            I_105_ScanMostHealth,
            I_106_ScanLeastHealth,
            I_107_ScanMostAccurate,
            I_108_ScanLeastAccurate,
            I_109_ScanMostShotsHit,
            I_110_ScanLeastShotsHit,
            I_111_ScanHighestLocation,
            I_112_ScanLowestLocation,
            I_113_ScanFastest,
            I_114_ScanSlowest,
            I_115_ScanFastestMaxSpeed,
            I_116_ScanSlowestMaxSpeed,
            I_117_ScanMostModules,
            I_118_ScanLeastModules,
            I_119_ScanHeaviest,
            I_120_ScanLightest,
            I_121_ScanMostLinesOfCode,
            I_122_ScanLeastLinesOfCode,
            I_123_ScanBestScore,
            I_124_ScanLowestScore,
            I_125_ScanMostKills,
            I_126_ScanLeastKills,
            I_127_ScanLastRobotThatHitMe,
            I_128_ScanLastRobotThatIHit,
            I_129_ScanWarmestHeatLevel,
            I_130_ScanColdestHeatLevel,
            I_131_ScanClosestEnemyBehindMe,
            I_132_ScanFurthestEnemyBehindMe,
            I_133_ScanClosestEnemyInfrontOfMe,
            I_134_ScanFurthestEnemyInfrontOfMe,
            I_135_ScanClosestEnemyToMyRight,
            I_136_ScanFurthestEnemyToMyRight,
            I_137_ScanClosestEnemeyToMyLeft,
            I_138_ScanFurthestEnemyToMyLeft,
            I_139_ScanClosestEnemyWithAMeleeWeapon,
            I_140_ScanFurthestEnemyWithAMeleeWeapon,
            I_141_ScanClosestEnemyTargettingMe,
            I_142_ScanFurthestEnemyTargettingMe,
            I_143_ScanClosestEnemyWithWheels,
            I_144_ScanFurthestEnemyWithWheels,
            I_145_ScanClosestEnemyWithWalkerLegs,
            I_146_ScanFurthestEnemyWithWalkerLegs,
            I_147_ScanClosestEnemyWithSpiderLegs,
            I_148_ScanFurthestEnemyWithSpiderLegs,
            I_149_ScanClosestEnemyWithJet,
            I_150_ScanFurthestEnemyWithJet,
            I_151_ScanClosestOtherOwnRobot,
            I_152_ScanFurthestOtherOwnRobot,
            I_153_ScanSpecificPosition,

        }

        public enum EnumConditionCheck
        {
            EqualTo,
            NotEqualTo,
            LessThan,
            LessThanOrEqualTo,
            GreaterThan,
            GreaterThanOrEqualTo,

        }
        public static string GetDisplayText(EnumConditionType aVal)
        {
            string ret = "";
            switch (aVal)
            {
                case EnumConditionType.If:
                    {
                        ret = "If";
                        break;
                    }
                case EnumConditionType.While:
                    {
                        ret = "While";
                        break;
                    }
                default:
                    {
                        ret = "[Not Set]";
                        break;
                    }
            }
            return ret;
        }
        public static string GetDisplayText(EnumAttribute aVal)
        {
            string ret = "";
          
           
                        InterrupHandler.Interrupt anInt = CodeGeneration.ConvertEnumTextToInterrupt(aVal.ToString());
                        ret = InterrupHandler.GetDisplayText(anInt);
         
            return ret;
        }
        public static string GetDisplayText(EnumTarget aVal)
        {
            string ret = "";
            InterrupHandler.Interrupt anInt = CodeGeneration.GetInterruptNumberForTarget(aVal);// (InterrupHandler.Interrupt)val;
             ret = InterrupHandler.GetDisplayText(anInt) + "";
           
            return ret;
        }

        public static string GetDisplayText(EnumConditionCheck aVal)
        {
            string ret = "";
            switch (aVal)
            {
                case EnumConditionCheck.EqualTo:
                    {
                        ret = "Exactly Equal To";
                        break;
                    }
                case EnumConditionCheck.GreaterThan:
                    {
                        ret = "Greater Than ";
                        break;
                    }
                case EnumConditionCheck.GreaterThanOrEqualTo:
                    {
                        ret = "Greater Than Or Equal To";
                        break;
                    }
                case EnumConditionCheck.LessThan:
                    {
                        ret = "Less Than";
                        break;
                    }
                case EnumConditionCheck.LessThanOrEqualTo:
                    {
                        ret = "Less Than Or Equal To";
                        break;
                    }
                case EnumConditionCheck.NotEqualTo:
                    {
                        ret = "Not Equal To";
                        break;
                    }

                default:
                    {
                        ret = "[Not Set]";
                        break;
                    }
            }
            return ret;
        }
    }

    public class CodeGenAction
    {
        public string TheIONumber;
        public ComponentType TheComponent;
        public ComponentType.IoFunctionMeta TheFunction;

        public EnumActionTarget Target;
        public int DropdownValue;
        public string SingleValue;
        public string ThreeValue1;
        public string ThreeValue2;
        public string ThreeValue3;

        public string GetHumanText()
        {
            string ret = "";
            string valText = "";
            if (TheFunction != null && TheFunction.InputVariables.DataSetType == ComponentType.IoFunctionMeta.EnumIODataSetType.DropDownBx)
            {
                if (DropdownValue >= 0)
                {

                     valText += TheFunction.InputVariables.InputVariables[0].DropDownOptions[DropdownValue].DisplayText + " ";
                }
            }
            else
            {

                if (SingleValue.Length > 0)
                {
                    valText += " " + SingleValue + " ";
                }
                if (ThreeValue1.Length > 0)
                {
                    valText += ", " + ThreeValue1 + " ";
                }
                if (ThreeValue2.Length > 0)
                {
                    valText += ", " + ThreeValue2 + " ";
                }
                if (ThreeValue3.Length > 0)
                {
                    valText += ", " + ThreeValue3 + " ";
                }
            }
            if (TheComponent != null)
            {
                ret += TheComponent.DeviceName + " ";
                if (TheFunction != null)
                {
                    ret += TheFunction.FunctionName + " ";
                    if (Target != null && Target != EnumActionTarget.None)
                    {
                        string targName = Computer.CodeGenAction.GetDisplayText(Target);

                        {
                            ret += "@ " + targName;//+ Target + "";
                        }
                    }
                    if (valText != null && valText.Length > 0)
                    {
                        ret += ": " + valText + " ";
                    }

                }
            }


            return ret;
        }



        public enum EnumActionTarget
        {
            None,
            I_02_OwnID,

            I_101_ScanClosestLocation,
            I_102_ScanFurthestLocation,
            I_103_ScanShotsFired,
            I_104_ScanLeastShotsFired,
            I_105_ScanMostHealth,
            I_106_ScanLeastHealth,
            I_107_ScanMostAccurate,
            I_108_ScanLeastAccurate,
            I_109_ScanMostShotsHit,
            I_110_ScanLeastShotsHit,
            I_111_ScanHighestLocation,
            I_112_ScanLowestLocation,
            I_113_ScanFastest,
            I_114_ScanSlowest,
            I_115_ScanFastestMaxSpeed,
            I_116_ScanSlowestMaxSpeed,
            I_117_ScanMostModules,
            I_118_ScanLeastModules,
            I_119_ScanHeaviest,
            I_120_ScanLightest,
            I_121_ScanMostLinesOfCode,
            I_122_ScanLeastLinesOfCode,
            I_123_ScanBestScore,
            I_124_ScanLowestScore,
            I_125_ScanMostKills,
            I_126_ScanLeastKills,
            I_127_ScanLastRobotThatHitMe,
            I_128_ScanLastRobotThatIHit,
            I_129_ScanWarmestHeatLevel,
            I_130_ScanColdestHeatLevel,
            I_131_ScanClosestEnemyBehindMe,
            I_132_ScanFurthestEnemyBehindMe,
            I_133_ScanClosestEnemyInfrontOfMe,
            I_134_ScanFurthestEnemyInfrontOfMe,
            I_135_ScanClosestEnemyToMyRight,
            I_136_ScanFurthestEnemyToMyRight,
            I_137_ScanClosestEnemeyToMyLeft,
            I_138_ScanFurthestEnemyToMyLeft,
            I_139_ScanClosestEnemyWithAMeleeWeapon,
            I_140_ScanFurthestEnemyWithAMeleeWeapon,
            I_141_ScanClosestEnemyTargettingMe,
            I_142_ScanFurthestEnemyTargettingMe,
            I_143_ScanClosestEnemyWithWheels,
            I_144_ScanFurthestEnemyWithWheels,
            I_145_ScanClosestEnemyWithWalkerLegs,
            I_146_ScanFurthestEnemyWithWalkerLegs,
            I_147_ScanClosestEnemyWithSpiderLegs,
            I_148_ScanFurthestEnemyWithSpiderLegs,
            I_149_ScanClosestEnemyWithJet,
            I_150_ScanFurthestEnemyWithJet,
            I_151_ScanClosestOtherOwnRobot,
            I_152_ScanFurthestOtherOwnRobot,
            I_153_ScanSpecificPosition,

            SpecificPosition = 100001,
            SpecificAngle,
            SpecificValue,
            DropDown,
        }

        public static string GetDisplayText(EnumActionTarget aVal)
        {
            string ret = "";
            switch (aVal)
            {

                case EnumActionTarget.SpecificPosition:
                    {
                        ret = "Specific Position";
                        break;
                    }
                case EnumActionTarget.SpecificAngle:
                    {
                        ret = "Specific Angle";
                        break;
                    }
                case EnumActionTarget.SpecificValue:
                    {
                        ret = "Specific Value";
                        break;
                    }
                case EnumActionTarget.DropDown:
                    {
                        ret = "Multi-choice";
                        break;
                    }
                default:
                    {

                        InterrupHandler.Interrupt anInt = CodeGeneration.GetInterruptNumberForTarget(aVal);
                        ret = InterrupHandler.GetDisplayText(anInt);
                        break;
                    }
            }
            return ret;
        }
    }

    public class IOFinder
    {
        public List<IOHandler> AllIOHandlers;
        public void HandleIO(int IONumber, Program parentProgram, ref StandardStack runtimeStack, RobotPart part)
        {
            if (AllIOHandlers != null)
            {
                for (int c = 0; c < AllIOHandlers.Count; c++)
                {
                    if (AllIOHandlers[c].IONumber == IONumber)
                    {
                        if (AllIOHandlers[c].enabled)
                        {
                            AllIOHandlers[c].HandleIO(parentProgram, ref runtimeStack, part);
                        }
                    }
                }
            }
        }
    }
   

    public class InterrupHandler
    {
        public RobotPart part;
        public static IEnumerable<T> GetValues<T>()
        {
            return Interrupt.GetValues(typeof(T)).Cast<T>();
        }
        public enum Interrupt
        {
            I_0_Destruct = 0,
            I_01_Reset = 01,
            I_02_OwnID = 02,
            I_03_LocateSelf = 03,
            I_04_MatchTime = 04,

            I_101_ScanClosestLocation = 101,
            I_102_ScanFurthestLocation,
            I_103_ScanShotsFired,
            I_104_ScanLeastShotsFired,
            I_105_ScanMostHealth,
            I_106_ScanLeastHealth,
            I_107_ScanMostAccurate,
            I_108_ScanLeastAccurate,
            I_109_ScanMostShotsHit,
            I_110_ScanLeastShotsHit,
            I_111_ScanHighestLocation,
            I_112_ScanLowestLocation,
            I_113_ScanFastest,
            I_114_ScanSlowest,
            I_115_ScanFastestMaxSpeed,
            I_116_ScanSlowestMaxSpeed,
            I_117_ScanMostModules,
            I_118_ScanLeastModules,
            I_119_ScanHeaviest,
            I_120_ScanLightest,
            I_121_ScanMostLinesOfCode,
            I_122_ScanLeastLinesOfCode,
            I_123_ScanBestScore,
            I_124_ScanLowestScore,
            I_125_ScanMostKills,
            I_126_ScanLeastKills,
            I_127_ScanLastRobotThatHitMe,
            I_128_ScanLastRobotThatIHit,
            I_129_ScanWarmestHeatLevel,
            I_130_ScanColdestHeatLevel,
            I_131_ScanClosestEnemyBehindMe,
            I_132_ScanFurthestEnemyBehindMe,
            I_133_ScanClosestEnemyInfrontOfMe,
            I_134_ScanFurthestEnemyInfrontOfMe,
            I_135_ScanClosestEnemyToMyRight,
            I_136_ScanFurthestEnemyToMyRight,
            I_137_ScanClosestEnemeyToMyLeft,
            I_138_ScanFurthestEnemyToMyLeft,
            I_139_ScanClosestEnemyWithAMeleeWeapon,
            I_140_ScanFurthestEnemyWithAMeleeWeapon,
            I_141_ScanClosestEnemyTargettingMe,
            I_142_ScanFurthestEnemyTargettingMe,
            I_143_ScanClosestEnemyWithWheels,
            I_144_ScanFurthestEnemyWithWheels,
            I_145_ScanClosestEnemyWithWalkerLegs,
            I_146_ScanFurthestEnemyWithWalkerLegs,
            I_147_ScanClosestEnemyWithSpiderLegs,
            I_148_ScanFurthestEnemyWithSpiderLegs,
            I_149_ScanClosestEnemyWithJet,
            I_150_ScanFurthestEnemyWithJet,
            I_151_ScanClosestOtherOwnRobot,
            I_152_ScanFurthestOtherOwnRobot,
            I_153_ScanSpecificPosition,






            I_201_TargetInfo_Location = 201,
            I_202_TargetInfo_TotalHealth,
            I_203_TargetInfo_TotalArmour,
            I_204_TargetInfo_TotalHeat,
            I_205_TargetInfo_TotalLinesOfCode,
            I_206_TargetInfo_TotalLinesOfCodePerSecond,
            I_207_TargetInfo_TotalPower,
            I_208_TargetInfo_TotalWeight,
            I_209_TargetInfo_MaxSpeed,
            I_210_TargetInfo_MaxAcceleration,
            I_211_TargetInfo_RotationSpeed,
            I_212_TargetInfo_CurrentSpeedPercentage,
            I_213_TargetInfo_CurrentSpeedVelocity,
            I_214_TargetInfo_DistanceTo,
            I_215_TargetInfo_LookAtLocation,
            I_216_TargetInfo_AngleBetweenOnYAxis,
            I_217_TargetInfo_AngleBetweenOnZAxis,
            I_218_TargetInfo_AverageAccuracy,
            I_219_TargetInfo_PhyicalSize,
            I_220_TargetInfo_TotalNumberOfCockpits,
            I_221_TargetInfo_TotalNumberOfShoulders,
            I_222_TargetInfo_TotalNumberOfBackpacks,
            I_223_TargetInfo_TotalNumberOfWeapons,
            I_224_TargetInfo_TotalNumberOfEMPWeapons,
            I_225_TargetInfo_TotalNumberOfBallisticWeapons,
            I_226_TargetInfo_TotalNumberOfRockets,
            I_227_TargetInfo_TotalNumberOfMeleeWeapons,
            I_228_TargetInfo_TotalNumberOfLaserWeapons,
            I_229_TargetInfo_TotalNumberOfFireWeapons,
            I_230_TargetInfo_TotalNumberOfMineLayers,
            I_231_TargetInfo_Direction,
            I_232_TargetInfo_Position,


            
            //todo
            I_901_Transmit = 901,
            I_902_Receive = 902,
            I_903_DataReady = 903,
            I_904_ClearCom = 904,
            // I_3_Overburn = 3,
            //I_6_FindAngle = 6,
            //I_7_TargetID = 7,
            //  I_9_GameInfo = 9,
            //  I_10_RobotInfo = 10,
            //I_11_Collisions = 11,
            //I_12_ResetCollisionCount = 12,

            // I_17_KillsDeaths = 17,




        }
        public static string GetDisplayText(Interrupt aVal)
        {
            string ret = "";


            switch (aVal)
            {

                case Interrupt.I_0_Destruct:
                    {
                        ret = "Self Destruct";
                        break;
                    }
                case Interrupt.I_01_Reset:
                    {
                        ret = "Reset";
                        break;
                    }
                case Interrupt.I_02_OwnID:
                    {
                        ret = "Self";
                        break;
                    }
                case Interrupt.I_03_LocateSelf:
                    {
                        ret = "Location of Self";
                        break;
                    }
                case Interrupt.I_04_MatchTime:
                    {
                        ret = "Match Time";
                        break;
                    }
                case Interrupt.I_101_ScanClosestLocation:
                    {
                        ret = "Closest Enemy";
                        break;
                    }
                case Interrupt.I_102_ScanFurthestLocation:
                    {
                        ret = "Furthest Enemy";
                        break;
                    }
                case Interrupt.I_103_ScanShotsFired:
                    {
                        ret = "Enemy Who Fired Most Bullets";
                        break;
                    }
                case Interrupt.I_104_ScanLeastShotsFired:
                    {
                        ret = "Enemy Who Fired Least Bullets";
                        break;
                    }
                case Interrupt.I_105_ScanMostHealth:
                    {
                        ret = "Enemy with Highest Health";
                        break;
                    }
                case Interrupt.I_106_ScanLeastHealth:
                    {
                        ret = "Enemy with Lowest Health";
                        break;
                    }
                case Interrupt.I_107_ScanMostAccurate:
                    {
                        ret = "Most Accurate";
                        break;
                    }
                case Interrupt.I_108_ScanLeastAccurate:
                    {
                        ret = "Least Accurate";
                        break;
                    }
                case Interrupt.I_109_ScanMostShotsHit:
                    {
                        ret = "Most Shots Hit";
                        break;
                    }
                case Interrupt.I_110_ScanLeastShotsHit:
                    {
                        ret = "Least Shots Hit";
                        break;
                    }
                case Interrupt.I_111_ScanHighestLocation:
                    {
                        ret = "Highest Altitude";
                        break;
                    }
                case Interrupt.I_112_ScanLowestLocation:
                    {
                        ret = "Lowest Altitude";
                        break;
                    }
                case Interrupt.I_113_ScanFastest:
                    {
                        ret = "Fastest";
                        break;
                    }
                case Interrupt.I_114_ScanSlowest:
                    {
                        ret = "Slowest";
                        break;
                    }
                case Interrupt.I_115_ScanFastestMaxSpeed:
                    {
                        ret = "Highest Max Speed";
                        break;
                    }
                case Interrupt.I_116_ScanSlowestMaxSpeed:
                    {
                        ret = "Lowest Max Speed";
                        break;
                    }
                case Interrupt.I_117_ScanMostModules:
                    {
                        ret = "Most Modules";
                        break;
                    }
                case Interrupt.I_118_ScanLeastModules:
                    {
                        ret = "Least Modules";
                        break;
                    }
                case Interrupt.I_119_ScanHeaviest:
                    {
                        ret = "Heaviest";
                        break;
                    }
                case Interrupt.I_120_ScanLightest:
                    {
                        ret = "Lightest";
                        break;
                    }
                case Interrupt.I_121_ScanMostLinesOfCode:
                    {
                        ret = "Most Lines of Code";
                        break;
                    }
                case Interrupt.I_122_ScanLeastLinesOfCode:
                    {
                        ret = "Least Lines of Code";
                        break;
                    }
                case Interrupt.I_123_ScanBestScore:
                    {
                        ret = "Highest Score";
                        break;
                    }
                case Interrupt.I_124_ScanLowestScore:
                    {
                        ret = "Lowest Score";
                        break;
                    }
                case Interrupt.I_125_ScanMostKills:
                    {
                        ret = "Most Kills";
                        break;
                    }
                case Interrupt.I_126_ScanLeastKills:
                    {
                        ret = "Least Kills";
                        break;
                    }
                case Interrupt.I_127_ScanLastRobotThatHitMe:
                    {
                        ret = "Last Robot That Hit Me";
                        break;
                    }
                case Interrupt.I_128_ScanLastRobotThatIHit:
                    {
                        ret = "Last Robot That I Hit";
                        break;
                    }
                case Interrupt.I_129_ScanWarmestHeatLevel:
                    {
                        ret = "Warmest";
                        break;
                    }
                case Interrupt.I_130_ScanColdestHeatLevel:
                    {
                        ret = "Coolest";
                        break;
                    }
                case Interrupt.I_131_ScanClosestEnemyBehindMe:
                    {
                        ret = "Closest Enemy Behind Me";
                        break;
                    }
                case Interrupt.I_132_ScanFurthestEnemyBehindMe:
                    {
                        ret = "Furthest Enemy Behind Me";
                        break;
                    }
                case Interrupt.I_133_ScanClosestEnemyInfrontOfMe:
                    {
                        ret = "Closest Enemy in Front Of Me";
                        break;
                    }
                case Interrupt.I_134_ScanFurthestEnemyInfrontOfMe:
                    {
                        ret = "Furthest Enemy in Front of Me";
                        break;
                    }
                case Interrupt.I_135_ScanClosestEnemyToMyRight:
                    {
                        ret = "Closest Enemy to My Right";
                        break;
                    }
                case Interrupt.I_136_ScanFurthestEnemyToMyRight:
                    {
                        ret = "Furthest Enemy to My Right";
                        break;
                    }
                case Interrupt.I_137_ScanClosestEnemeyToMyLeft:
                    {
                        ret = "Closest Enemy to My Left";
                        break;
                    }
                case Interrupt.I_138_ScanFurthestEnemyToMyLeft:
                    {
                        ret = "Furthest Enemy to My Left";
                        break;
                    }
                case Interrupt.I_139_ScanClosestEnemyWithAMeleeWeapon:
                    {
                        ret = "Closest Enemy with a Melee Weapon";
                        break;
                    }
                case Interrupt.I_140_ScanFurthestEnemyWithAMeleeWeapon:
                    {
                        ret = "Furthest Enemy with a Melee Weapon";
                        break;
                    }
                case Interrupt.I_141_ScanClosestEnemyTargettingMe:
                    {
                        ret = "Closest Enemy Targeting Me";
                        break;
                    }
                case Interrupt.I_142_ScanFurthestEnemyTargettingMe:
                    {
                        ret = "Furthest Enemy Targeting Me";
                        break;
                    }
                case Interrupt.I_143_ScanClosestEnemyWithWheels:
                    {
                        ret = "Closest Enemy with Wheels";
                        break;
                    }
                case Interrupt.I_144_ScanFurthestEnemyWithWheels:
                    {
                        ret = "Furthest Enemy with Wheels";
                        break;
                    }
                case Interrupt.I_145_ScanClosestEnemyWithWalkerLegs:
                    {
                        ret = "Closest Enemy with Walker Legs";
                        break;
                    }
                case Interrupt.I_146_ScanFurthestEnemyWithWalkerLegs:
                    {
                        ret = "Furthest Enemy with Walker Legs";
                        break;
                    }
                case Interrupt.I_147_ScanClosestEnemyWithSpiderLegs:
                    {
                        ret = "Closest Enemy with Spider Legs";
                        break;
                    }
                case Interrupt.I_148_ScanFurthestEnemyWithSpiderLegs:
                    {
                        ret = "Furthest Enemy with Spider Legs";
                        break;
                    }
                case Interrupt.I_149_ScanClosestEnemyWithJet:
                    {
                        ret = "Closest Enemy with Jet Flight";
                        break;
                    }
                case Interrupt.I_150_ScanFurthestEnemyWithJet:
                    {
                        ret = "Furthest Enemy with Jet Flight";
                        break;
                    }
                case Interrupt.I_151_ScanClosestOtherOwnRobot:
                    {
                        ret = "Closest Other Own Robot ";
                        break;
                    }
                case Interrupt.I_152_ScanFurthestOtherOwnRobot:
                    {
                        ret = "Furthest Other Own Robot";
                        break;
                    }
                    
                         case Interrupt.I_153_ScanSpecificPosition:
                    {
                        ret = "Specific Position on Map";
                        break;
                    }
                case Interrupt.I_201_TargetInfo_Location:
                    {
                        ret = "Location in Arena";
                        break;
                    }
                case Interrupt.I_202_TargetInfo_TotalHealth:
                    {
                        ret = "Total Health";
                        break;
                    }
                case Interrupt.I_203_TargetInfo_TotalArmour:
                    {
                        ret = "Total Armour";
                        break;
                    }
                case Interrupt.I_204_TargetInfo_TotalHeat:
                    {
                        ret = "Total Heat";
                        break;
                    }
                case Interrupt.I_205_TargetInfo_TotalLinesOfCode:
                    {
                        ret = "Total Lines of Code";
                        break;
                    }
                case Interrupt.I_206_TargetInfo_TotalLinesOfCodePerSecond:
                    {
                        ret = "Lines of Code Processed per Second";
                        break;
                    }
                case Interrupt.I_207_TargetInfo_TotalPower:
                    {
                        ret = "Total KW Power";
                        break;
                    }
                case Interrupt.I_208_TargetInfo_TotalWeight:
                    {
                        ret = "Total KG Weight";
                        break;
                    }
                case Interrupt.I_209_TargetInfo_MaxSpeed:
                    {
                        ret = "Maximum KM/pH Speed";
                        break;
                    }
                case Interrupt.I_210_TargetInfo_MaxAcceleration:
                    {
                        ret = "Maximum KM/pS Acceleration";
                        break;
                    }
                case Interrupt.I_211_TargetInfo_RotationSpeed:
                    {
                        ret = "Rotation Speed Rating";
                        break;
                    }
                case Interrupt.I_212_TargetInfo_CurrentSpeedPercentage:
                    {
                        ret = "Current Speed Percentage";
                        break;
                    }
                case Interrupt.I_213_TargetInfo_CurrentSpeedVelocity:
                    {
                        ret = "Current Speed Velocity KM/pH";
                        break;
                    }
                case Interrupt.I_214_TargetInfo_DistanceTo:
                    {
                        ret = "My Distance To";
                        break;
                    }
                case Interrupt.I_215_TargetInfo_LookAtLocation:
                    {
                        ret = "Enemy's Look-At Location";
                        break;
                    }
                case Interrupt.I_216_TargetInfo_AngleBetweenOnYAxis:
                    {
                        ret = "My Angle to Target around Y-Axis";
                        break;
                    }
                case Interrupt.I_217_TargetInfo_AngleBetweenOnZAxis:
                    {
                        ret = "My Angle to Target around Z-Axis";
                        break;
                    }
                case Interrupt.I_218_TargetInfo_AverageAccuracy:
                    {
                        ret = "Robot's Average Accuracy";
                        break;
                    }
                case Interrupt.I_219_TargetInfo_PhyicalSize:
                    {
                        ret = "Robot's physical size in SqrMeters";
                        break;
                    }
                case Interrupt.I_220_TargetInfo_TotalNumberOfCockpits:
                    {
                        ret = "Total Number of Cockpit Modules";
                        break;
                    }
                case Interrupt.I_221_TargetInfo_TotalNumberOfShoulders:
                    {
                        ret = "Total Number of Shoulder Modules";
                        break;
                    }
                case Interrupt.I_222_TargetInfo_TotalNumberOfBackpacks:
                    {
                        ret = "Total Number of Backpack Modules";
                        break;
                    }
                case Interrupt.I_223_TargetInfo_TotalNumberOfWeapons:
                    {
                        ret = "Total Number of Weapons";
                        break;
                    }
                case Interrupt.I_224_TargetInfo_TotalNumberOfEMPWeapons:
                    {
                        ret = "Total Number of EMP Weapons";
                        break;
                    }
                case Interrupt.I_225_TargetInfo_TotalNumberOfBallisticWeapons:
                    {
                        ret = "Total Number of Ballistic Weapons";
                        break;
                    }
                case Interrupt.I_226_TargetInfo_TotalNumberOfRockets:
                    {
                        ret = "Total Number of Rocketry Weapons";
                        break;
                    }
                case Interrupt.I_227_TargetInfo_TotalNumberOfMeleeWeapons:
                    {
                        ret = "Total Number of Melee Weapons";
                        break;
                    }
                case Interrupt.I_228_TargetInfo_TotalNumberOfLaserWeapons:
                    {
                        ret = "Total Number of Laser Weapons";
                        break;
                    }
                case Interrupt.I_229_TargetInfo_TotalNumberOfFireWeapons:
                    {
                        ret = "Total Number of Fire-based Weapons";
                        break;
                    }
                case Interrupt.I_230_TargetInfo_TotalNumberOfMineLayers:
                    {
                        ret = "Total Number of Minelaying Modules";
                        break;
                    }

                case Interrupt.I_231_TargetInfo_Direction:
                    {
                        ret = "Forward Direction";
                        break;
                    }
                case Interrupt.I_232_TargetInfo_Position:
                    {
                        ret = "Position on Map";
                        break;
                    }
                case Interrupt.I_901_Transmit:
                    {
                        ret = "Transmit Data";
                        break;
                    }
                case Interrupt.I_902_Receive:
                    {
                        ret = "Receive Data";
                        break;
                    }
                case Interrupt.I_903_DataReady:
                    {
                        ret = "Set DataCom Ready";
                        break;
                    }
                case Interrupt.I_904_ClearCom:
                    {
                        ret = "Clear DataCom";
                        break;
                    }
               
                default:
                    {
                        ret = aVal + "";
                       // throw new System.NotImplementedException();
                        break;
                    }
            }
            return ret;
        }
        public void HandleInterrupt(int interrupId, Program parentProgram, ref StandardStack runtimeStack)
        {
            RobotMeta theMeta = part.gameObject.GetComponent<RobotMeta>();
            List<RobotMeta> allMetas = Match.PublicAccess.GetAllRobotMetas();
            string scanText = "Scanning for Enemy - ";
            Interrupt inter = (Interrupt)interrupId;
            switch (inter)
            {
                case Interrupt.I_0_Destruct:
                    {
                        theMeta.Health = 0;
                        // part.SelfDesctruct();
                        break;
                    }
                case Interrupt.I_01_Reset:
                    {
                        runtimeStack.Ax.Val = "0";
                        runtimeStack.Bx.Val = "0";
                        runtimeStack.CMPL.Val = "0";
                        runtimeStack.CMPR.Val = "0";
                        runtimeStack.Cx.Val = "0";
                        runtimeStack.Dx.Val = "0";
                        runtimeStack.Ex.Val = "0";
                        runtimeStack.Fx.Val = "0";
                        runtimeStack.Gx.Val = "0";
                        runtimeStack.IP.Val = "-1";
                        runtimeStack.N.Val = "0";
                        parentProgram.TheThread.CallStack.Clear();

                        break;
                    }
                case Interrupt.I_02_OwnID:
                    {
                        runtimeStack.Ex.Val = theMeta.RuntimeRobotID + "";
                        break;
                    }

                case Interrupt.I_03_LocateSelf:
                    {
                        runtimeStack.Ex.Val = theMeta.gameObject.transform.position.x + "";
                        runtimeStack.Fx.Val = theMeta.gameObject.transform.position.y + "";
                        runtimeStack.Gx.Val = theMeta.gameObject.transform.position.z + "";
                        break;
                    }


                case Interrupt.I_04_MatchTime:
                    {
                        if (Match.PublicAccess != null)
                        {
                            runtimeStack.Ex.Val = ((System.DateTime.UtcNow - Match.PublicAccess.MatchStartTime).TotalSeconds + Match.PublicAccess.IntroductionSecondsLength) + "";
                        }
                        else
                        {
                            runtimeStack.Ex.Val = "0";
                        }



                        break;
                    }


                case Interrupt.I_101_ScanClosestLocation:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Closest";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = theMeta.ClosestRobot; ;

                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_102_ScanFurthestLocation:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Furthest";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = theMeta.FurthestRobot; ;

                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_103_ScanShotsFired:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Most Shots Fired";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderByDescending(x => x.ShotsFired).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_104_ScanLeastShotsFired:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Least Shots Fired";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderBy(x => x.ShotsFired).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_105_ScanMostHealth:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Most Health";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderByDescending(x => x.Health).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_106_ScanLeastHealth:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Least Health";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderBy(x => x.Health).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_107_ScanMostAccurate:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Most Accurate";
                            theMeta.AddEventLog(scanText + iName);
                        }
                      
                        RobotMeta aRobot = allMetas.OrderByDescending(x => (x.ShotsHit + 1) / (x.ShotsFired + 1)).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }

                        }
                        break;
                    }
                case Interrupt.I_108_ScanLeastAccurate:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Least Accurate";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderBy(x => (x.ShotsHit + 1) / (x.ShotsFired + 1)).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_109_ScanMostShotsHit:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Least Shots Hit";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderBy(x => x.ShotsHit).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_110_ScanLeastShotsHit:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Least Shots Hit";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderBy(x => x.ShotsHit).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_111_ScanHighestLocation:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Highest Altitude";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderByDescending(x => x.gameObject.transform.position.y).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_112_ScanLowestLocation:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Lowest Altitude";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderBy(x => x.gameObject.transform.position.y).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_113_ScanFastest:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Fastest";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderByDescending(x => x.EffectiveSpeed).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_114_ScanSlowest:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Slowest";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderBy(x => x.EffectiveSpeed).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_115_ScanFastestMaxSpeed:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Fastest Max Speed";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderByDescending(x => x.EffectiveSpeedMax).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_116_ScanSlowestMaxSpeed:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Slowest Max Speed";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderBy(x => x.EffectiveSpeedMax).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_117_ScanMostModules:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Most Modules";
                            theMeta.AddEventLog(scanText + iName);
                        }
                         RobotMeta aRobot = allMetas.OrderByDescending(x => x.NumberOfModules).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_118_ScanLeastModules:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Least Modules";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderBy(x => x.NumberOfModules).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_119_ScanHeaviest:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Heaviest";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderByDescending(x => x.EffectiveWeight).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_120_ScanLightest:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Lightest";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderBy(x => x.EffectiveWeight).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_121_ScanMostLinesOfCode:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Most Lines of Code";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderByDescending(x => x.LinesOfCode).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_122_ScanLeastLinesOfCode:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Least Lines of Code";
                            theMeta.AddEventLog(scanText + iName);
                        }
                         RobotMeta aRobot = allMetas.OrderBy(x => x.LinesOfCode).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                         if (theMeta.IsEventLoggingOn)
                        {   theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                         }
                        }
                        break;
                    }
                case Interrupt.I_123_ScanBestScore:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Best Score";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderByDescending(x => x.RuntimeScore).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_124_ScanLowestScore:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Worst Score";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderBy(x => x.RuntimeScore).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_125_ScanMostKills:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Most Kills";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderByDescending(x => x.Kills).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_126_ScanLeastKills:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Least Kills";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderBy(x => x.Kills).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_127_ScanLastRobotThatHitMe:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Last Robot That Hit Me";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = theMeta.LastRobotThatHitMe;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_128_ScanLastRobotThatIHit:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Last Robot That I Hit";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.Where(x => x.LastRobotThatHitMe != null && x.LastRobotThatHitMe.RuntimeRobotID == theMeta.RuntimeRobotID).OrderBy(x => Vector3.Distance(theMeta.gameObject.transform.position, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_129_ScanWarmestHeatLevel:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Warmest Robot";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderByDescending(x => x.Heat).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_130_ScanColdestHeatLevel:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Coldest";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = allMetas.OrderBy(x => x.Heat).Where(x => x.RuntimeRobotID != theMeta.RuntimeRobotID && x.IsDead == false).FirstOrDefault();
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_131_ScanClosestEnemyBehindMe:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Closest Enemy Behind Me";
                            theMeta.AddEventLog(scanText + iName);
                        }
                         RobotMeta aRobot = theMeta.ClosestRobotBehindMe; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_132_ScanFurthestEnemyBehindMe:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Furthest Enemy Behind Me";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = theMeta.FurthestRobotBehindMe; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_133_ScanClosestEnemyInfrontOfMe:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Closest In Front of Me";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = theMeta.ClosestRobotInFrontOfMe; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_134_ScanFurthestEnemyInfrontOfMe:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Furthest In Front of Me";
                            theMeta.AddEventLog(scanText + iName);
                        }
                         RobotMeta aRobot = theMeta.FurthestRobotInFrontOfMe; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_135_ScanClosestEnemyToMyRight:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Closest Enemy To My Right";
                            theMeta.AddEventLog(scanText + iName);
                        }
                         RobotMeta aRobot = theMeta.ClosestRobotRightOfMe; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_136_ScanFurthestEnemyToMyRight:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Furthest Enemy To My Right";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = theMeta.FurthestRobotightOfMe; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_137_ScanClosestEnemeyToMyLeft:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Closest Enemy To My Left";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = theMeta.ClosestRobotLeftOfMe; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_138_ScanFurthestEnemyToMyLeft:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Furthest Enemy To My Left";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = theMeta.FurthestRobotLeftOfMe; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_139_ScanClosestEnemyWithAMeleeWeapon:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Closest Enemy With Melee Weapons";
                            theMeta.AddEventLog(scanText + iName);
                        }
                       RobotMeta aRobot = theMeta.ClosestRobotWithMelee; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_140_ScanFurthestEnemyWithAMeleeWeapon:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Furthest Enemy With Melee Weapons";
                            theMeta.AddEventLog(scanText + iName);
                        }
                         RobotMeta aRobot = theMeta.FurthestRobotWithMelee; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_141_ScanClosestEnemyTargettingMe:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Closest Enemy Targeting Me";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = theMeta.ClosestRobotTargettingMe; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            { theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val); }
                        }
                        break;
                    }
                case Interrupt.I_142_ScanFurthestEnemyTargettingMe:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Furthest Enemy Targeting Me";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = theMeta.FurthestRobotTargettingMe; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_143_ScanClosestEnemyWithWheels:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Closest Enemy With Wheels";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = theMeta.ClosestRobotWithWheels; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_144_ScanFurthestEnemyWithWheels:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Furthest Enemy With Wheels";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = theMeta.FurthestRobotWithWheels; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_145_ScanClosestEnemyWithWalkerLegs:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Closest Enemy With Walker Legs";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = theMeta.ClosestRobotWithWalkerLegs; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_146_ScanFurthestEnemyWithWalkerLegs:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Furthest Enemy with Walker Legs";
                            theMeta.AddEventLog(scanText + iName);
                        }
                         RobotMeta aRobot = theMeta.FurthestRobotWithWalkerLegs; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                   
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_147_ScanClosestEnemyWithSpiderLegs:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Closest Enemy With Spider Legs";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = theMeta.ClosestRobotWithSpiderLegs; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_148_ScanFurthestEnemyWithSpiderLegs:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Furthest Enemy with Spider Legs";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = theMeta.FurthestRobotWithSpiderLegs; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_149_ScanClosestEnemyWithJet:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Closest Enemy With Jet";
                            theMeta.AddEventLog(scanText + iName);
                        }
                        RobotMeta aRobot = theMeta.ClosestRobotWithJet; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                        }
                        break;
                    }

                case Interrupt.I_150_ScanFurthestEnemyWithJet:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Furthest Enemy With Jet";
                            theMeta.AddEventLog(scanText + iName);

                        }
                        RobotMeta aRobot = theMeta.FurthestRobotWithJet; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_151_ScanClosestOtherOwnRobot:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Closest Other Owned Robot";
                            theMeta.AddEventLog(scanText + iName);

                        }
                        RobotMeta aRobot = theMeta.ClosestRobotOwnOtherRobot; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_152_ScanFurthestOtherOwnRobot:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            string iName = "Furthest Other Owned Robot";
                            theMeta.AddEventLog(scanText + iName);
                        }
                         RobotMeta aRobot = theMeta.FurthestRobotOwnOtherRobot; ;
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeRobotID.ToString();
                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Found: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                    


                  case Interrupt.I_153_ScanSpecificPosition:
                    {
                        //todo
                        break;
                    }






                case Interrupt.I_201_TargetInfo_Location:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("Finding Target Location");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.gameObject.transform.position.x.ToString("f2");
                            runtimeStack.Fx.Val = aRobot.gameObject.transform.position.y.ToString("f2");
                            runtimeStack.Gx.Val = aRobot.gameObject.transform.position.z.ToString("f2");

                        }

                        break;
                    }

                case Interrupt.I_202_TargetInfo_TotalHealth:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_202_TargetInfo_TotalHealth");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = Match.PublicAccess.GetAllRobotMetas().FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.Health.ToString("f2");
                        }                     
                        break;
                    }
                case Interrupt.I_203_TargetInfo_TotalArmour:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_203_TargetInfo_TotalArmour");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.Armour.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_204_TargetInfo_TotalHeat:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_204_TargetInfo_TotalHeat");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.RuntimeOverheat.ToString("f2");

                        }


                        break;
                    }
                case Interrupt.I_205_TargetInfo_TotalLinesOfCode:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_202_TargetInfo_TotalHealth");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.Health.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_206_TargetInfo_TotalLinesOfCodePerSecond:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_202_TargetInfo_TotalHealth");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.Health.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_207_TargetInfo_TotalPower:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_202_TargetInfo_TotalHealth");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.Health.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_208_TargetInfo_TotalWeight:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_202_TargetInfo_TotalHealth");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.Health.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_209_TargetInfo_MaxSpeed:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_202_TargetInfo_TotalHealth");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.Health.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_210_TargetInfo_MaxAcceleration:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_202_TargetInfo_TotalHealth");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.Health.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_211_TargetInfo_RotationSpeed:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_202_TargetInfo_TotalHealth");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.Health.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_212_TargetInfo_CurrentSpeedPercentage:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_212_TargetInfo_CurrentSpeedPercentage");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            IoLegsMech leg = aRobot.GetComponent<IoLegsMech>();
                            if (leg == null) leg = aRobot.GetComponentInChildren<IoLegsMech>();

                            if (leg != null)
                            {
                                runtimeStack.Ex.Val = leg.ActualSpeedOutOf100.ToString("f2");
                            }
                        }


                        break;
                    }
                case Interrupt.I_213_TargetInfo_CurrentSpeedVelocity:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_213_TargetInfo_CurrentSpeedVelocity");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            IoLegsMech leg = aRobot.GetComponent<IoLegsMech>();
                            if (leg == null) leg = aRobot.GetComponentInChildren<IoLegsMech>();

                            if (leg != null)
                            {

                                runtimeStack.Ex.Val = leg.MyRigidbody.velocity.magnitude.ToString("f2");
                            }
                        }


                        break;
                    }
                case Interrupt.I_214_TargetInfo_DistanceTo:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_214_TargetInfo_DistanceTo");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        RobotMeta aRobotMe = allMetas.FirstOrDefault(x => x.RuntimeRobotID == theMeta.RuntimeRobotID);
                        
                        if (aRobot != null)
                        {
                        float dist=     Vector3.Distance(aRobotMe.gameObject.transform.position, aRobot.gameObject.transform.position);
                            runtimeStack.Ex.Val =dist.ToString("f0");

                            if (theMeta.IsEventLoggingOn)
                            {
                                theMeta.AddEventLog("Distance: " + runtimeStack.Ex.Val);
                            }
                        }
                        break;
                    }
                case Interrupt.I_215_TargetInfo_LookAtLocation:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_215_TargetInfo_LookAtLocation");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            IoLegsMech leg = aRobot.GetComponent<IoLegsMech>();
                            if (leg == null) leg = aRobot.GetComponentInChildren<IoLegsMech>();

                            if (leg != null)
                            {
                                runtimeStack.Ex.Val = leg.ActualLookDestination.x.ToString("f2");
                                runtimeStack.Fx.Val = leg.ActualLookDestination.y.ToString("f2");
                                runtimeStack.Gx.Val = leg.ActualLookDestination.z.ToString("f2");
                            }
                        }


                        break;
                    }
                case Interrupt.I_216_TargetInfo_AngleBetweenOnYAxis:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_216_TargetInfo_AngleBetweenOnYAxis");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.DetermineAngleBetweenObjectOnYAxis().ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_217_TargetInfo_AngleBetweenOnZAxis:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_217_TargetInfo_AngleBetweenOnZAxis");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.DetermineAngleBetweenObjectOnZAxis().ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_218_TargetInfo_AverageAccuracy:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_218_TargetInfo_AverageAccuracy");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = (((aRobot.ShotsHit+1f)/(aRobot.ShotsFired+1f))*100f).ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_219_TargetInfo_PhyicalSize:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_219_TargetInfo_PhyicalSize");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.PhysicalSize.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_220_TargetInfo_TotalNumberOfCockpits:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_220_TargetInfo_TotalNumberOfCockpits");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.NumberOfCockpits.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_221_TargetInfo_TotalNumberOfShoulders:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_221_TargetInfo_TotalNumberOfShoulders");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.NumberOfShoulders.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_222_TargetInfo_TotalNumberOfBackpacks:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_222_TargetInfo_TotalNumberOfBackpacks");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.NumberOfBackPacks.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_223_TargetInfo_TotalNumberOfWeapons:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_223_TargetInfo_TotalNumberOfWeapons");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.NumberOfWeapons.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_224_TargetInfo_TotalNumberOfEMPWeapons:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_224_TargetInfo_TotalNumberOfEMPWeapons");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.NumberOfEMPWeapons.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_225_TargetInfo_TotalNumberOfBallisticWeapons:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_225_TargetInfo_TotalNumberOfBallisticWeapons");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.NumberOfBallisticWeapons.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_226_TargetInfo_TotalNumberOfRockets:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_226_TargetInfo_TotalNumberOfRockets");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.NumberOfRocketWeapons.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_227_TargetInfo_TotalNumberOfMeleeWeapons:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_227_TargetInfo_TotalNumberOfMeleeWeapons");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.NumberOfMeleeWeapons.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_228_TargetInfo_TotalNumberOfLaserWeapons:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_228_TargetInfo_TotalNumberOfLaserWeapons");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.NumberOfLaserWeapons.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_229_TargetInfo_TotalNumberOfFireWeapons:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_229_TargetInfo_TotalNumberOfFireWeapons");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.NumberOfFireWeapons.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_230_TargetInfo_TotalNumberOfMineLayers:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("I_230_TargetInfo_TotalNumberOfMineLayers");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.NumberOfMineLayerWeapons.ToString("f2");
                        }
                        break;
                    }
                case Interrupt.I_231_TargetInfo_Direction:
                    {
                        if (theMeta.IsEventLoggingOn)
                        {
                            theMeta.AddEventLog("Finding Target Forward Direction");
                        }
                        int axVal = int.Parse(runtimeStack.Ax.Val);
                        RobotMeta aRobot = allMetas.FirstOrDefault(x => x.RuntimeRobotID == axVal);
                        if (aRobot != null)
                        {
                            runtimeStack.Ex.Val = aRobot.gameObject.transform.forward.x.ToString("f2");
                            runtimeStack.Fx.Val = aRobot.gameObject.transform.forward.y.ToString("f2");
                            runtimeStack.Gx.Val = aRobot.gameObject.transform.forward.z.ToString("f2");

                        }

                        break;
                    }
                case Interrupt.I_232_TargetInfo_Position:
                    {
                        //todo

                        break;
                    }
                case Interrupt.I_901_Transmit:
                    {
                        //todo
                        break;
                    }
                case Interrupt.I_902_Receive:
                    {
                        //todo

                        break;
                    }
                case Interrupt.I_903_DataReady:
                    {
                        //todo

                        break;
                    }
                case Interrupt.I_904_ClearCom:
                    {
                        //todo

                        break;
                    }


            }
        }
    }


    public class ThreadHandler
    {
        public List<CallStackEntry> CallStack = new List<CallStackEntry>();
        public void ContinueCallStack(Program parentProgram, ref StandardStack runtimeStack, InterrupHandler anInterrupt, IOFinder anIO)
        {
            if (CallStack.Count == 0)
            {
                SetCallStack(parentProgram.Functions[0].FunctionName, 0);

            }
            runtimeStack.IP.Val = CallStack[CallStack.Count - 1].IPOfFunction.ToString();
            parentProgram.CallFunction(CallStack[CallStack.Count - 1].FunctionNameToExecute, ref runtimeStack, anInterrupt, anIO);
          
        }
        public Function GetFunction(Program parentProgram, CallStackEntry aStack)
        {

            return null;
        }
        public void SetCallStack(string functionName, int IP)
        {
            if (CallStack == null) CallStack = new List<CallStackEntry>();
            if (CallStack.Count == 0)
            {
                CallStackEntry aCall = new CallStackEntry();
                aCall.FunctionNameToExecute = functionName;
                aCall.IPOfFunction = IP;
                CallStack.Add(aCall);
            }


            CallStackEntry lastEntry = CallStack[CallStack.Count - 1];

            for (int c = CallStack.Count - 1; c >= 0; c--)
            {
                if (CallStack[c].FunctionNameToExecute == functionName)
                {
                    lastEntry = CallStack[c];
                    break;
                }
            }


            if (lastEntry.FunctionNameToExecute == functionName)
            {
                lastEntry.IPOfFunction = IP;
            }
            else
            {
                CallStackEntry newEntry = new CallStackEntry();
                newEntry.FunctionNameToExecute = functionName;
                newEntry.IPOfFunction = IP;
                CallStack.Add(newEntry);
            }
        }


    }

    public class CallStackEntry
    {
        public string FunctionNameToExecute;
        public int IPOfFunction;
    }
    public class Program
    {
        public int ID;
        public string ProgramName;
        public List<Function> Functions = new List<Function>();
        public ThreadHandler TheThread;
        public void CallFunction(string functionName, ref StandardStack runtimeStack, InterrupHandler anInterrupt, IOFinder anIO)
        {
            for (int c = 0; c < Functions.Count; c++)
            {
                if (Functions[c].FunctionName.ToLower().Trim() == functionName.ToLower().Trim())
                {


                    Functions[c].ExecuteFunction(this, ref runtimeStack, anInterrupt, anIO);

                    break;
                }
            }
        }


    }
    public class Function
    {
        public int ID;
        public int ParentID;
       
        public List<Variable> LocalVariables = new List<Variable>();

        public string FunctionName;
        public List<CodeLine> CodeLines;
        public StandardStack TagRuntimeStack;
        public bool hasEnded = false;
        public void ExecuteFunction(Program parentProgram, ref StandardStack runtimeStack, InterrupHandler anInterrupt, IOFinder anIO)
        {

            if (parentProgram.TheThread == null) parentProgram.TheThread = new ThreadHandler();

              {
                int indexPointer = int.Parse(runtimeStack.IP.Val);
                if (indexPointer < 0) indexPointer = 0;
                try
                {
                    CodeLines[indexPointer].ExecuteCommand(parentProgram, ref runtimeStack, this, anInterrupt, anIO);
                }
                catch
                {
                }
                indexPointer = int.Parse(runtimeStack.IP.Val);
                indexPointer++;
                if (indexPointer < 0) indexPointer = 0;

                runtimeStack.IP.Val = indexPointer.ToString();
                CallStackEntry anEntry = new CallStackEntry();
                anEntry.FunctionNameToExecute = FunctionName;
                anEntry.IPOfFunction = int.Parse(runtimeStack.IP.Val);
                parentProgram.TheThread.SetCallStack(anEntry.FunctionNameToExecute, anEntry.IPOfFunction);


                if (indexPointer >= CodeLines.Count)
                {
                    hasEnded = true;
                    runtimeStack.IP.Val = "0";
                    parentProgram.TheThread.SetCallStack(anEntry.FunctionNameToExecute, 0);

                    int indexToRemove = -1;
                    for (int c = parentProgram.TheThread.CallStack.Count - 1; c >= 0; c--)
                    {
                        if (parentProgram.TheThread.CallStack[c].FunctionNameToExecute == FunctionName)
                        {
                            indexToRemove = c;
                            break;
                        }
                    }
                    if (indexToRemove >= 0)
                    {
                        parentProgram.TheThread.CallStack.RemoveAt(indexToRemove);

                    }
                  
                }
                else
                {
                    hasEnded = false;


                }

            }
          }

        public int GetIpOfLabel(string labelName)
        {
            int ip = -1;
            for (int c = 0; c < CodeLines.Count; c++)
            {
                  if (CodeLines[c].LabelName != null && CodeLines[c].LabelName.Contains(labelName.Trim()))
                {
                     ip = c - 1;
                    break;
                }
            
            }
           
            return ip;
        }
    }
    public class StandardStack
    {
        public enum EnumRegisters
        {
            Ax,
            Bx,
            Cx,
            Dx,
            Ex,
            Fx,
            Gx,
        }
        public Variable Ax;
        public Variable Bx;
        public Variable Cx;
        public Variable Dx;
        public Variable Ex;
        public Variable Fx;
        public Variable Gx;
        public Variable IP;
        public Variable CMPL;
        public Variable CMPR;

        public Variable N;
        public StandardStack()
        {
            Ax = new Variable();
            Ax.VariableName = "ax";
            Ax.Description = "";
            Ax.Val = "0";

            Bx = new Variable();
            Bx.VariableName = "bx";
            Bx.Description = "";
            Bx.Val = "0";

            Cx = new Variable();
            Cx.VariableName = "cx";
            Cx.Description = "";
            Cx.Val = "0";

            Dx = new Variable();
            Dx.VariableName = "dx";
            Dx.Description = "";
            Dx.Val = "0";


            Ex = new Variable();
            Ex.VariableName = "ex";
            Ex.Description = "";
            Ex.Val = "0";


            Fx = new Variable();
            Fx.VariableName = "fx";
            Fx.Description = "";
            Fx.Val = "0";


            Gx = new Variable();
            Gx.VariableName = "gx";
            Gx.Description = "";
            Gx.Val = "0";

            IP = new Variable();
            IP.VariableName = "ip";
            IP.Description = "";
            IP.Val = "0";

            CMPL = new Variable();
            CMPL.VariableName = "cmpl";
            CMPL.Description = "";
            CMPL.Val = "0";

            CMPR = new Variable();
            CMPR.VariableName = "cmpr";
            CMPR.Description = "";
            CMPR.Val = "0";
           

        }
        public Variable FindVariableFromName(string variableName)
        {
            Variable ret = null;

            switch (variableName.ToLower())
            {

                case "ax":
                    {
                        ret = Ax;
                        break;
                    }
                case "bx":
                    {
                        ret = Bx;
                        break;
                    }
                case "cx":
                    {
                        ret = Cx;
                        break;
                    }
                case "dx":
                    {
                        ret = Dx;
                        break;
                    }
                case "ex":
                    {
                        ret = Ex;
                        break;
                    }
                case "fx":
                    {
                        ret = Fx;
                        break;
                    }
                case "gx":
                    {
                        ret = Gx;
                        break;
                    }
                case "ip":
                    {
                        ret = IP;
                        break;
                    }
                case "cmpl":
                    {
                        ret = CMPL;
                        break;
                    }
                case "cmpr":
                    {
                        ret = CMPR;
                        break;
                    }
                default:
                    {
                        N = new Variable();
                        N.VariableName = "n";
                        N.Val = variableName;
                        ret = N;
                        break;
                    }
            }
            return ret;

        }

    }
    public class Variable
    {
        public string VariableName;
        public string Val;
        public string Description;
    }
    public class CodeLine
    {
        public int LineNumber;
        public List<string> LabelName;
        public bool IsSkipable;
        public string Comments;
        public CompiledCommand Command;
        public void ExtractMetaFirstPass()
        {
            switch (Command.Command)
            {
                case EnumCommand.LBL:
                    {
                        LabelName = new List<string>();
                        LabelName.Add(Command.Parameter1.Trim());
                        //  LabelName = Command.Parameter1;
                        IsSkipable = true;
                        break;
                    }
            }
        }
        public void ExecuteCommand(Program parentProgram, ref StandardStack runtimeStack, Function parentFunction, InterrupHandler anInterrupt, IOFinder anIO)
        {
            int newIndexPointer = int.Parse(runtimeStack.IP.Val);
            switch (Command.Command)
            {
                case EnumCommand.ADD:
                    {
                        Variable var1 = runtimeStack.FindVariableFromName(Command.Parameter1);
                        Variable var2 = runtimeStack.FindVariableFromName(Command.Parameter2);
                        var1.Val = (int.Parse(var1.Val) + int.Parse(var2.Val)).ToString();

                        break;
                    }
                case EnumCommand.CALL:
                    {
                        Variable var1 = runtimeStack.FindVariableFromName(Command.Parameter1);
                        string tempIP = runtimeStack.IP.Val;
                        runtimeStack.IP.Val = "0";
                        parentProgram.CallFunction(var1.Val, ref runtimeStack, anInterrupt, anIO);
                        runtimeStack.IP.Val = tempIP;
                        break;
                    }
                case EnumCommand.CMP:
                    {
                        Variable var1 = runtimeStack.FindVariableFromName(Command.Parameter1);
                        Variable var2 = runtimeStack.FindVariableFromName(Command.Parameter2);
                        Variable cmpl = runtimeStack.FindVariableFromName("cmpl");
                        Variable cmpr = runtimeStack.FindVariableFromName("cmpr");
                        cmpl.Val = var1.Val;
                        cmpr.Val = var2.Val;

                        
                        break;
                    }
               
                case EnumCommand.DIV:
                    {
                        Variable var1 = runtimeStack.FindVariableFromName(Command.Parameter1);
                        Variable var2 = runtimeStack.FindVariableFromName(Command.Parameter2);
                        var1.Val = ((int)(int.Parse(var1.Val) / int.Parse(var2.Val))).ToString();

                        break;
                    }
                
                case EnumCommand.INT:
                    {
                        Variable var1 = runtimeStack.FindVariableFromName(Command.Parameter1);
                         anInterrupt.HandleInterrupt(int.Parse(var1.Val), parentProgram, ref runtimeStack);
                        break;
                    }
                case EnumCommand.IO:
                    {
                        Variable var1 = runtimeStack.FindVariableFromName(Command.Parameter1);
                          anIO.HandleIO(int.Parse(var1.Val), parentProgram, ref runtimeStack, anInterrupt.part);

                        break;
                    }
                case EnumCommand.JE:
                    {
                       
                        if (System.Convert.ToDouble(runtimeStack.CMPL.Val) == System.Convert.ToDouble(runtimeStack.CMPR.Val))
                        {
                           Variable var1 = runtimeStack.FindVariableFromName(Command.Parameter1);
                            int ipOfLabel = parentFunction.GetIpOfLabel(var1.Val);
                            runtimeStack.IP.Val = ipOfLabel.ToString();

                        }
                        break;
                    }
                case EnumCommand.JGE:
                    {
                        if (System.Convert.ToDouble(runtimeStack.CMPL.Val) >= System.Convert.ToDouble(runtimeStack.CMPR.Val))
                        {
                          
                            Variable var1 = runtimeStack.FindVariableFromName(Command.Parameter1);
                            int ipOfLabel = parentFunction.GetIpOfLabel(var1.Val);
                            runtimeStack.IP.Val = ipOfLabel.ToString();

                        }
                        break;
                    }
                case EnumCommand.JGR:
                    {
                         if (System.Convert.ToDouble(runtimeStack.CMPL.Val) > System.Convert.ToDouble(runtimeStack.CMPR.Val))
                        {
                          
                            Variable var1 = runtimeStack.FindVariableFromName(Command.Parameter1);
                            int ipOfLabel = parentFunction.GetIpOfLabel(var1.Val);
                            runtimeStack.IP.Val = ipOfLabel.ToString();

                        }
                        break;
                    }
                case EnumCommand.JLE:
                    {
                        if (System.Convert.ToDouble(runtimeStack.CMPL.Val) <= System.Convert.ToDouble(runtimeStack.CMPR.Val))
                        {

                            Variable var1 = runtimeStack.FindVariableFromName(Command.Parameter1);
                            int ipOfLabel = parentFunction.GetIpOfLabel(var1.Val);
                            runtimeStack.IP.Val = ipOfLabel.ToString();

                        }
                        break;
                    }
                case EnumCommand.JLS:
                    {
                        if (System.Convert.ToDouble(runtimeStack.CMPL.Val) < System.Convert.ToDouble(runtimeStack.CMPR.Val))
                        {

                            Variable var1 = runtimeStack.FindVariableFromName(Command.Parameter1);
                            int ipOfLabel = parentFunction.GetIpOfLabel(var1.Val);
                            runtimeStack.IP.Val = ipOfLabel.ToString();

                        }
                        break;
                    }
                case EnumCommand.JMP:
                    {


                        Variable var1 = runtimeStack.FindVariableFromName(Command.Parameter1);
                        int ipOfLabel = parentFunction.GetIpOfLabel(var1.Val);
                        runtimeStack.IP.Val = ipOfLabel.ToString();


                        break;
                    }
                case EnumCommand.JNE:
                    {
                        if (System.Convert.ToDouble(runtimeStack.CMPL.Val) != System.Convert.ToDouble(runtimeStack.CMPR.Val))
                        {
                         
                            Variable var1 = runtimeStack.FindVariableFromName(Command.Parameter1);
                            int ipOfLabel = parentFunction.GetIpOfLabel(var1.Val);
                            runtimeStack.IP.Val = ipOfLabel.ToString();

                        }
                        break;
                    }
                
                case EnumCommand.MOD:
                    {
                        Variable var1 = runtimeStack.FindVariableFromName(Command.Parameter1);
                        Variable var2 = runtimeStack.FindVariableFromName(Command.Parameter2);
                        var1.Val = (int.Parse(var1.Val) % int.Parse(var2.Val)).ToString();

                        break;
                    }
                case EnumCommand.MOV:
                    {
                        Variable var1 = runtimeStack.FindVariableFromName(Command.Parameter1);
                        Variable var2 = runtimeStack.FindVariableFromName(Command.Parameter2);
                        var1.Val = var2.Val;
                        break;
                    }
                case EnumCommand.MPY:
                    {
                        Variable var1 = runtimeStack.FindVariableFromName(Command.Parameter1);
                        Variable var2 = runtimeStack.FindVariableFromName(Command.Parameter2);
                        var1.Val = (int.Parse(var1.Val) * int.Parse(var2.Val)).ToString();

                        break;
                    }
                case EnumCommand.NOP:
                    {
                      
                        break;
                    }
               
                case EnumCommand.SUB:
                    {
                        Variable var1 = runtimeStack.FindVariableFromName(Command.Parameter1);
                        Variable var2 = runtimeStack.FindVariableFromName(Command.Parameter2);
                        var1.Val = (int.Parse(var1.Val) - int.Parse(var2.Val)).ToString();

                        break;
                    }



            }
            //   bool is

        }

        public static CodeLine ParseFromText(string text)
        {
            CodeLine ret = new CodeLine();
            string[] parts = text.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries);
            EnumCommand aCommand = ParseCommandEnumPart(parts[0]);


            ret.Command = new CompiledCommand();
            ret.Command.Command = aCommand;
            if (parts.Length > 1)
            {
                ret.Command.Parameter1 = parts[1];
            }
            if (parts.Length > 2)
            {
                ret.Command.Parameter2 = parts[2];
            }

            return ret;
        }
        public static EnumCommand ParseCommandEnumPart(string commandText)
        {
            EnumCommand ret = EnumCommand.NOP;
            try
            {
                ret = (EnumCommand)System.Enum.Parse(typeof(EnumCommand), commandText.ToUpper());
            }
            catch
            {

            }
            return ret;
        }

        public static int GetCycleCountForCommand(EnumCommand aCommand)
        {
            int ret = 0;
            switch (aCommand)
            {
                default:
                    {
                        ret = 0; ;
                        break;
                    }
            }
            return ret;
        }

        public static int GetMemoryCountForCommand(EnumCommand aCommand)
        {
            int ret = 0;
            switch (aCommand)
            {
                default:
                    {
                        ret = 0; ;
                        break;
                    }
            }
            return ret;
        }

        public static int GetDescriptionForCommand(EnumCommand aCommand)
        {
            int ret = 0;
            switch (aCommand)
            {
                default:
                    {
                        ret = 0; ;
                        break;
                    }
            }
            return ret;
        }

        public static int DoSyntaxCheckForCommand(EnumCommand aCommand)
        {
            int ret = 0;
            switch (aCommand)
            {
                default:
                    {
                        ret = 0; ;
                        break;
                    }
            }
            return ret;
        }
        public class CompiledCommand
        {
            public EnumCommand Command;
            public string Parameter1;
            public string Parameter2;
            public string Comments;
        }

        public enum EnumCommand
        {
            NOP = 0, //Waste Clock Cycle
            ADD = 1, //Add V+N, store in  V
            SUB = 2, //Subracts V-N
            MPY = 3, //Multiply
            DIV = 4, //Divides V by N
            MOD = 5, //Mod V by N store in V
            //RET = 6, //RETURNS FUNCTION
            CALL = 7, //CALL a function
            CMP = 8, //Compares V and N
            JMP = 9, //Jump to label
            JLS = 10, //Jump if less than <
            JGR = 11, //Jump if greater than >
            JNE = 12, //Jump not equal to
            JE = 13, //Jump equal to
            JGE = 14, //Jump if greater and equal >=
            JLE = 15, //Jump if less than or equal to <=
            //  JZ,     //Jump if zero
            //  JNZ, //Jump if not zero
            //  JTL, //JUMP TO LINE
            MOV, //MOVE variable N to V
            INT, //CALL INTERRUPT
            IO, //CALL an IO Device
            //DEL, // Delay for N time
            //ERR, //Generate Error
            LBL
        }
        public static string GetDisplayText(EnumCommand aVal)
        {
            string ret = "";


            switch (aVal)
            {

                case EnumCommand.ADD:
                    {
                        ret = "Adds 2 numbers.";
                        break;
                    }
                case EnumCommand.CALL:
                    {
                        ret = "Calls a function.";
                        break;
                    }
                case EnumCommand.CMP:
                    {
                        ret = "Compares 2 numbers.";
                        break;
                    }
                case EnumCommand.DIV:
                    {
                        ret = "Divides 2 numbers";
                        break;
                    }
                case EnumCommand.INT:
                    {
                        ret = "Interrupt Request";
                        break;
                    }
                case EnumCommand.IO:
                    {
                        ret = "IO Request";
                        break;
                    }
                case EnumCommand.JE:
                    {
                        ret = "Jump if Equal";
                        break;
                    }
                case EnumCommand.JGE:
                    {
                        ret = "Jump if Greater or Equal ";
                        break;
                    }
                case EnumCommand.JGR:
                    {
                        ret = "Jump if Greater";
                        break;
                    }
                case EnumCommand.JLE:
                    {
                        ret = "Jump if Less Than or Equal";
                        break;
                    }
                case EnumCommand.JLS:
                    {
                        ret = "Jump if Less Than";
                        break;
                    }
                case EnumCommand.JMP:
                    {
                        ret = "Jump";
                        break;
                    }
                case EnumCommand.JNE:
                    {
                        ret = "Jump if Not Equal";
                        break;
                    }
                case EnumCommand.LBL:
                    {
                        ret = "Labeled Position";
                        break;
                    }
                case EnumCommand.MOD:
                    {
                        ret = "Modulus";
                        break;
                    }
                case EnumCommand.MOV:
                    {
                        ret = "Move value into register";
                        break;
                    }
                case EnumCommand.MPY:
                    {
                        ret = "Multiply 2 numbers";
                        break;
                    }
                case EnumCommand.NOP:
                    {
                        ret = "No Operation; Do Nothing";
                        break;
                    }
                case EnumCommand.SUB:
                    {
                        ret = "Subtract 2 numbers";
                        break;
                    }
               
                
                default:
                    {
                        ret = aVal + "";
                      //  throw new System.NotImplementedException();
                        break;
                    }
            }
            return ret;
        }
   
      
    }
    // Use this for initialization


    
    void Update()
    {
        InstructionsAllowed += Time.deltaTime * InstructrionsPerSecond;


        if (IsEnabled)
        {
            //if (nextExecutationTime <= System.DateTime.Now)
            if (InstructionsAllowed > 1)
            {


                nextExecutationTime = System.DateTime.Now.AddMilliseconds(MilliSecondsDelayPerCode);
                bool IsOverHeated = false;
                for (int c = 0; c < InstructionsAllowed; c++)
                {

                    if (myRobotMeta != null)
                    {
                        IsOverHeated = myRobotMeta.IsOverheated;
                    }
                    if (IsOverHeated == false)
                    {
                        StepExecute();
                    }
                    InstructionsAllowed--;
                    if (InstructionsAllowed < 0)
                    {
                        InstructionsAllowed = 0;
                    }
                }

            }
        }

    }
}
