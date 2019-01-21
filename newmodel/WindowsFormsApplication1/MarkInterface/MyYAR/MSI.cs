using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HG.MSICS
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ELM_OPTIC_SETTING
    {
        //打标范围
        // The size of the marking area
        [FieldOffset(0)]
        public float fAreaSize;

        [FieldOffset(4)]
        public int bXYFlip;	// Whether to flip the X, Y axis.

        [FieldOffset(8)]
        public int bXInvert;	// Whether to inver the x axis.

        [FieldOffset(12)]
        public int bYInvert;	// Whether to inver the y axis.

        //中点
        // The center of the marking area.
        [FieldOffset(16)]
        public float fCenterX;

        [FieldOffset(20)]
        public float fCenterY;

        //原点
        // The position of the laser light point when standby
        [FieldOffset(24)]
        public float fHomePositionX;

        [FieldOffset(28)]
        public float fHomePositionY;

        //偏移
        // Offset
        [FieldOffset(32)]
        public float fOffsetX;

        [FieldOffset(36)]
        public float fOffsetY;

        //增益
        // Gain
        [FieldOffset(40)]
        public float fGainX;

        [FieldOffset(44)]
        public float fGainY;

        //旋转
        // Rotate
        [FieldOffset(48)]
        public float fRotateX;

        [FieldOffset(52)]
        public float fRotateY;

        [FieldOffset(56)]
        public float fRotateAngle;

        [FieldOffset(64)]
        public double dFocus;		// 镜头的焦距, 单位毫米

        // The focus of the lens
        [FieldOffset(72)]
        public double dGapX;		// X轴鼓型，枕型调节，（有正负）

        // X axis pillow correction (it can be positive or negative)
        [FieldOffset(80)]
        public double dGapY;		// Y轴鼓型，枕型调节，（有正负）

        // Y axis pillow correction (it can be positive or negative)
        [FieldOffset(88)]
        public double dTrapeX;		// X轴梯形调节，（有正负）

        // X axis trape correction (it can be positive or negative)
        [FieldOffset(96)]
        public double dTrapeY;		// Y轴梯形调节，（有正负）

        // Y axis trape correction (it can be positive or negative)
        [FieldOffset(104)]
        public double dParalX;		// X轴平行四边形调节（有正负）

        // X axis parallel correction (it can be positive or negative)
        [FieldOffset(112)]
        public double dParalY;		// Y轴平行四边形调节（有正负）

        // Y axis parallel correction (it can be positive or negative)
    };

    [StructLayout(LayoutKind.Explicit)]
    public struct ELM_MARK_SETTING
    {
        [FieldOffset(0)]
        public int iLaserType;						// 激光类型, 0: CO2; 1: FIBER; 2: YAG

        // Laser Type, 0: CO2; 1: FIBER; 2: YAG

        [FieldOffset(4)]
        public int iYAGMode;						// YAG激光模式，三种模式，详细见MarkStudio的设置项

        // The mode of YAG laser.

        [FieldOffset(8)]
        public int bVariablePolygonDelay;			// 是否优化折线延时

        // The polygon delay is variable based on the angle of the two lines.

        [FieldOffset(12)]
        public int iPowerPort;						// 8Bits : 1, DA1 : 2, or DA2 : 3

        [FieldOffset(16)]
        public int bLaserStartupDelay;				// 是否激光启动延时

        // Whether to add startup delay when open laser, this is for fiber laser.

        [FieldOffset(20)]
        public int usLaserStartupDelay;			// 激光启动延时

        // The startup delay

        [FieldOffset(24)]
        public int bStandBy;						// 是否支持Standby

        // Whether to support standby function to keep the frequency of the laser.

        [FieldOffset(28)]
        public int bStandByAlwaysActive;			// StandBy 是否一直有效

        // Whether to set the standby function always active.

        [FieldOffset(32)]
        public float fStandByFreq;					// Standby频率,单位:KHz

        // The standby frequency(KHz)

        [FieldOffset(36)]
        public int usStandByPulseWidth;			// Standby脉宽,Unit: us;

        // The pulse width of standby(us)

        [FieldOffset(40)]
        public float fFirstPulseKillerOffVoltage;	// 关光时的FirstPulseKiller电压

        [FieldOffset(44)]
        public float fFirstPulseKillerOnVoltage;		// 出光时的FirstPulseKiller电压

        [FieldOffset(48)]
        public int iFirstPulseKillerTime;			// FirstPulseKiller的时间

        [FieldOffset(52)]
        public int iFirstPulseKillerInterval;		// 重新启动FirstPulseKiller的时间

        [FieldOffset(56)]
        public int iFirstPulseKillerLaserOnTime;	// FirstPulseKiller相对LaserOn的时间

        //For Fly
        [FieldOffset(60)]
        public int bFly;							// 是否飞行模式

        // Whether to set flying mark
        [FieldOffset(64)]
        public int bFlyXY;							// 飞行坐标，TRUE:X轴; FALS:Y轴

        // The flying axis, 1: x axis, 0: y axis.
        [FieldOffset(68)]
        public int bFlySimulation;					// 是否模拟

        // Whether to simulate the multiplier of the coder.
        [FieldOffset(72)]
        public double dFlyMultiplier;					// 飞行乘积数

        // The multiplier of the coder.

        // External Triger
        [FieldOffset(80)]
        public int usExternalTriggerDelay;			// 外触发延时

        // The delay for the external trigger.

        // 高级设置
        [FieldOffset(84)]
        public int bEnableDirectLight;

        [FieldOffset(88)]
        public int bDirectLightRect;

        [FieldOffset(92)]
        public float fDirectLightOffsetX;

        [FieldOffset(96)]
        public float fDirectLightOffsetY;

        [FieldOffset(100)]
        public float fDirectLightScaleX;

        [FieldOffset(104)]
        public float fDirectLightScaleY;

        [FieldOffset(108)]
        public float fDirectLightSpeed;

        [FieldOffset(112)]
        public int iDirectLightDelay;

        [FieldOffset(116)]
        public int bCacheMode;

        [FieldOffset(120)]
        public int bNoTriggerWithinTime;

        [FieldOffset(124)]
        public int iNoTriggerWithinTime;
    };

    [StructLayout(LayoutKind.Explicit)]
    public struct ELM_SYSTEM_LIMITATION
    {
        //打标最大最小速度范围
        // The min and max marking speed.
        [FieldOffset(0)]
        public float fSpeedMin;

        [FieldOffset(4)]
        public float fSpeedMax;

        //频率范围
        // The min and max marking frequency
        [FieldOffset(8)]
        public float fFreqMin;

        [FieldOffset(12)]
        public float fFreqMax;

        // 最大功率
        // The max power(percent)
        [FieldOffset(16)]
        public float fPowerMax;	// 百分比

        [FieldOffset(20)]
        public float fPowerMin;
    };

    [StructLayout(LayoutKind.Explicit)]
    public struct DPOINT
    {
        [FieldOffset(0)]
        public double x;

        [FieldOffset(8)]
        public double y;

        [FieldOffset(16)]
        public int iFlag;
    };

    [StructLayout(LayoutKind.Explicit)]
    public struct ELM_FILL_PARAMETERS
    {
        [FieldOffset(0)]
        public int bEnable; // 使能该填充

        [FieldOffset(4)]
        public int iFillType;//填充类型, 对应MarkStudio里面的选项. 0: FoldNoBoardLine; 1: LeftRetraceLine; 2: RigtRetracehLine

        [FieldOffset(8)]
        public float fInterval; //间距

        [FieldOffset(12)]
        public float fAngle; //旋转角度，相对于对象的，或者绝对角度

        [FieldOffset(16)]
        public float fStartOffset;//起始偏移

        [FieldOffset(20)]
        public float fEndOffset;//末尾偏移

        [FieldOffset(24)]
        public float fIndent;//线缩进

        [FieldOffset(28)]
        public float bKeepAngle;//是否相对于对象的角度变化

        [FieldOffset(32)]
        public int iLoopCount;//这个参数的意思是：当除了LoopLine的情况下，可以设置从外到里面的圈数，

        [FieldOffset(36)]
        public int bInsideHatch;//如果设置了iLoopCount，是否中间还进行填充

        [FieldOffset(40)]
        public int bMarkOutline;// 这个是为轮廓填充的

        [FieldOffset(44)]
        public int lMarkCount;// 单独打标次数

        [FieldOffset(48)]
        public int bCalcWhole;// 整体计算

        [FieldOffset(52)]
        public int bWholeScan;// 整体扫描

        [FieldOffset(56)]
        public float fLoopInterval;// 环间距

        [FieldOffset(60)]
        public int iMarkPenIndex;// 打标笔

        [FieldOffset(64)]
        public float fAutoRotateAngle;	// 自动旋转角度

        [FieldOffset(68)]
        public int bMarkFillContour;	// 打一遍填充轮廓

        [FieldOffset(72)]
        public int lContourMarkCount;	// 填充轮廓的打标次数
    };

    [StructLayout(LayoutKind.Explicit)]
    public struct ELM_MARK_PEN
    {
        // 前面的都是基本参数
        [FieldOffset(0)]
        public float fMarkSpeed;					// 打标速率: ms/s

        // Mark speed(ms/s)
        [FieldOffset(4)]
        public float fJumpSpeed;					// 跳转速率Unit: ms/s

        // Jump speed(ms/s)
        [FieldOffset(8)]
        public float fMarkFreq;					// 频率: KHz

        // Frequency(KHz)

        //Scanner Delays
        [FieldOffset(12)]
        public int usJumpDelay;				// 跳转延时, unit: us

        // Jump delay(us)
        [FieldOffset(16)]
        public int usMarkDelay;				// 打标延时, unit: us

        // Mark Delay(us)
        [FieldOffset(20)]
        public int usPolygonDelay;				// 多边形延时, unit: us

        // polygon delay(us)
        [FieldOffset(24)]
        public int sLaserOnDelay;				// 激光开延时

        // Laser on delay(us)
        [FieldOffset(28)]
        public int sLaserOffDelay;				// 激光关延时

        // Laser off delay(us)
        [FieldOffset(32)]
        public int usMarkPulseWidth;			// 脉冲长度us

        // the width of the pulse.
        [FieldOffset(36)]
        public float fMarkPower;					// 功率, 最大功率的百分比

        //  Power(percent)
        // 以上为基本参数

        //Only For YAG Pen
        [FieldOffset(40)]
        public int usFirstPulseKillerWidth;	// 首脉冲抑制功能

        // The first pulse killer width.
        [FieldOffset(44)]
        public int iPointMarkTime;				// 点打标时间

        // The mark time for a point.
        [FieldOffset(48)]
        public int lPenType;					// 笔的类型, 用Bit来表示不同的功能: bit0: 1为支持连续打点功能

        // Pen type. direct the different functions by bits. Bit0: dotted line mode.

        // 线形, 多支持虚线，而且能设置空白和实线的长度
        // Line Type.
        [FieldOffset(52)]
        public int iLineType;					// 0, 实线;

        // 0: Solid line; 1: dashed line.
        [FieldOffset(56)]
        public double dDashedLineBlank;			// 实线部分

        // the blank part of the line if iLineType is 1;
        [FieldOffset(64)]
        public double dDashedLineSolid;			// 空白部分

        // the solid part of the line if iLineType is 1;

        // For SPI laser
        [FieldOffset(72)]
        public int lState;						// 状态

        [FieldOffset(76)]
        public float fSimmer;					// 电流

        [FieldOffset(80)]
        public int lMarkkLoopCount;					// 打标循环次数
    };

    internal static class NativeMethods
    {
        //初始化 卸载
        [DllImport("MSI.dll")]
        internal static extern int ELM_GetVersion();

        [DllImport("MSI.dll")]
        internal static extern int ELM_Init(int iCardMode, int iCardNumber);

        [DllImport("MSI.dll")]
        internal static extern int ELM_Uninit();

        [DllImport("MSI.dll")]
        internal static extern int ELM_Unload();

        //打标 停止
        [DllImport("MSI.dll")]
        internal static extern int ELM_MarkMksFile(int iCardNumber, int iIndex, int iType);
    
        //iType,打标类型，0: 激光打标;1:引导光(红光)

        [DllImport("MSI.dll")]
        internal static extern int ELM_MarkMksFile2(int iCardNumber, int iIndex, int iType);

        [DllImport("MSI.dll")]
        internal static extern int ELM_MarkMksFileInRect(int iDocIndex, int iType, int iCardNumber, double dLeft, double dRight, double dTop, double dBottom, double dOffsetX, double dOffsetY);

        [DllImport("MSI.dll")]
        internal static extern int ELM_StopMarkMksFile(int iCardNumber);

        [DllImport("MSI.dll")]
        internal static extern int ELM_WaitForMarkEnd(int iCardNumber, int dwTimeout);

        [DllImport("MSI.dll")]
        internal static extern int ELM_WaitForMarkEnd2(int iCardNumber, int dwTimeout);

        //打标一个对象
        [DllImport("MSI.dll")]
        internal static extern int ELM_MarkPolyline(int iCardNumber, int iType, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DPOINT[] pPoints, int iPointCount, ref ELM_MARK_PEN pMarkPen);

        //预览
        [DllImport("MSI.dll")]
        internal static extern int ELM_DrawMksFile(int iDocIndex, IntPtr HWnd);

        //触发等待与停止
        [DllImport("MSI.dll")]
        internal static extern int ELM_WaitForExternalTrigger(int iCardNumber, Int32 lTimeout, ref long piSource);
        //internal static extern int ELM_WaitForExternalTrigger(int iCardNumber, Int32 lTimeout, ref Int32 piSource);

        [DllImport("MSI.dll")]
        internal static extern int ELM_StopWaitForExternalTrigger(int iCardNumber);

        //MKS文件 读写保存
        [DllImport("MSI.dll")]
        internal static extern int ELM_OpenMksFile([In, MarshalAs(UnmanagedType.LPWStr)]string pstrPath);

        [DllImport("MSI.dll")]
        internal static extern int ELM_CloseMksFile(int iIndex);

        [DllImport("MSI.dll")]
        internal static extern int ELM_SaveMksFile(int iIndex, [MarshalAs(UnmanagedType.LPWStr)] string strPath);

        //图档对象 获取 修改
        [DllImport("MSI.dll")]
        internal static extern int ELM_EditTextObj(int iDocIndex, [In, MarshalAs(UnmanagedType.LPWStr)]string strObjName, [In, MarshalAs(UnmanagedType.LPWStr)]string pstrText);

        [DllImport("MSI.dll")]
        internal static extern int ELM_EditTextObj2(int iDocIndex, [In, MarshalAs(UnmanagedType.LPWStr)]string strObjName, [In, MarshalAs(UnmanagedType.LPWStr)]string pstrText);

        [DllImport("MSI.dll")]
        internal static extern int ELM_GetObjectName(int iDocIndex, int iObjIndex, [Out, MarshalAs(UnmanagedType.LPWStr)]StringBuilder pstrName);

        [DllImport("MSI.dll")]
        internal static extern int ELM_GetObjectMarkPenIndex(int iDocIndex, int iObjIndex);

        [DllImport("MSI.dll")]
        internal static extern int ELM_GetObjectsCount(int iDocIndex);

        [DllImport("MSI.dll")]
        internal static extern int ELM_RotateObject(int iDocIndex, int iObjIndex, float fCenterX, float fCenterY, float fAngle);

        [DllImport("MSI.dll")]
        internal static extern int ELM_AddPolylineToMksDoc(int iDocIndex, int iPenIndex, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DPOINT[] pPoints, int iPointCount, int bFill, [MarshalAs(UnmanagedType.LPWStr)] string strObjName);

        [DllImport("MSI.dll")]
        internal static extern int ELM_AddArcToMksDoc(int iDocIndex, int iPenIndex, [In, MarshalAs(UnmanagedType.LPWStr)]string strObjName, double dLeft, double dRight, double dTop, double dBottom, double dStartAngle, double dSweepAngle, int bFill);

        //参数设置
        [DllImport("MSI.dll")]
        internal static extern int ELM_LoadSystemSettingXmlFile(int iCardMode, int iCardNumber, [In, MarshalAs(UnmanagedType.LPWStr)]string pstrXmlPath);

        [DllImport("MSI.dll")]
        internal static extern int ELM_GetMarkSetting(int iCardNumber, ref ELM_MARK_SETTING pMarkSetting);

        [DllImport("MSI.dll")]
        internal static extern int ELM_SetMarkSetting(int iCardNumber, ref ELM_MARK_SETTING pMarkSetting);


        [DllImport("MSI.dll")]
        internal static extern int ELM_PrepareMark(int iCardNumber,int pMarkSetting);


        [DllImport("MSI.dll")]
        internal static extern int ELM_SetOpticSetting(ref  ELM_OPTIC_SETTING OpticSetting);

        [DllImport("MSI.dll")]
        internal static extern int ELM_SetLimitationSetting(ref ELM_SYSTEM_LIMITATION AdjustSetting);

        [DllImport("MSI.dll")]
        internal static extern int ELM_GetMarkPen(int iDocIndex, int iPenIndex, ref ELM_MARK_PEN pMarkPen);

        [DllImport("MSI.dll")]
        internal static extern int ELM_SetMarkPen(int iDocIndex, int iPenIndex, ref ELM_MARK_PEN pMarkPen);

        [DllImport("MSI.dll")]
        internal static extern int ELM_SetLedCtrl(int iRed, int iGreen, int iYellow, int iAlarm);

        //扩展轴相关
        [DllImport("MSI.dll")]
        internal static extern int ELM_SwitchExtendAxisMoveMode(int iMode);

        [DllImport("MSI.dll")]
        internal static extern int ELM_ResetExtendAxis(int iCardNumber, int iAxis);

        [DllImport("MSI.dll")]
        internal static extern int ELM_MoveExtendAxis(int iCardNumber, int iAxis, int iPulse);

        [DllImport("MSI.dll")]
        internal static extern int ELM_StopExtendAxisMove(int iCardNumber);

        [DllImport("MSI.dll")]
        internal static extern int ELM_GetExtendAxisState(int iCardNumber, ref int iAxis1, ref int iAxis2, ref int iAxis3, ref int iAxis4);

        [DllImport("MSI.dll")]
        internal static extern int ELM_GetExtendAxis(int iCardNumber, int iAxis, ref int piAxis);

        //其他
        [DllImport("MSI.dll")]
        internal static extern int ELM_SetExtendOutputCtrl(int iCardNumber, int iOutput, int iValue);

        [DllImport("MSI.dll")]
        internal static extern int ELM_GetMarkProcess(int iTimeout);

        [DllImport("MSI.dll")]
        internal static extern int ELM_SetFill(int iIndex, ref ELM_FILL_PARAMETERS fill);

        [DllImport("MSI.dll")]
        internal static extern int ELM_ClearAllObjects(int iIndex);

        [DllImport("MSI.dll")]
        internal static extern int ELM_ClearMarkObjectList();

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("MSI.dll")]
        internal static extern int ELM_GetCtrlInput(int iCard);

        [DllImport("MSI.dll")]
        internal static extern int ELM_GetObjectType(int iDocIndex, int iObjIndex);

        [DllImport("MSI.dll")]
        internal static extern int ELM_SetMachineVision(int iCardNumber, int iEnable, double dOffsetX, double dOffsetY, double dAngle);

        [DllImport("MSI.dll")]
        internal static extern int ELM_IsObjectBarcode(int iDocIndex, int iObjIndex);

        [DllImport("MSI.dll")]
        internal static extern int ELM_GetTextObjectText(int iDocIndex, int iObjIndex, [Out, MarshalAs(UnmanagedType.LPWStr)]StringBuilder pstrText);

        [DllImport("MSI.dll")]
        internal static extern  int ELM_SetCtrlOutput(int iCardNumber, int iOutput, int iMode, int iValue);

        //新增
        [DllImport("MSI.dll")]
        internal static extern int ELM_GetOpticSetting(int iCardNumber, ref ELM_OPTIC_SETTING pOpticSetting);
        //[DllImport("MSI.dll", CallingConvention = CallingConvention.StdCall)]
        [DllImport("MSI.dll")]
        internal static extern int ELM_GetObjectRect(int iDocIndex, int iObjIndex, ref double dLeft, ref double dRight, ref double dTop, ref double dBottom);
        //MCILIB_API int MCILIB_DLL ELM_SetCtrlOutput(int iCardNumber, int iOutput, int iMode, int iValue);
        
    };

    public class MSI
    {
        public static bool m_bCardOK = false;

        public static string m_strCrossMksPath = System.Windows.Forms.Application.StartupPath + "\\Cross.mks";
        public static string m_strCrossNpcPath = System.Windows.Forms.Application.StartupPath + "\\NPC.mks";

        public static int m_iCrossMks = -1;
        public static int m_iNPCalibMks = -1;

        public static int TopCard = 2;
        public static int BtmCard = 1;

        public static int m_iMoveAxisTimeout = 20000;
        public const int MOVE_CARD = 1;
        public static double[] m_dAxis = new double[4];

        public const int MSI_ERR_NOT_INIT = -1;
        public const int MSI_ERR_CROSS_MKS = -101;
        public const int MSI_ERR_NPCALIB_MKS = -102;
        public static int m_iMarkEndPulseWidth = 20;

        public static double[] m_dAxisUnit = new double[4];
        public static double[] m_dAxisMin = new double[4];
        public static double[] m_dAxisMax = new double[4];

        public static double m_dJogStep = 5;
        public static double m_dJogSlowStep = 1;

        public static bool m_bHome = false;

        public static double[] m_dAxisDir = new double[4];

        public static int m_iMarkEndOutput = 2;
        public static int m_iWidthOutput = 3;

        public static void InitVariables()
        {
            m_MarkMutex[0] = new Mutex();
            m_MarkMutex[1] = new Mutex();

            for (int i = 0; i < 4; i++)
            {
                m_dAxisUnit[i] = 170.0 / 40000.0;

                m_dAxisDir[i] = 1;
            }
            m_dAxisMin[0] = 0;
            m_dAxisMin[1] = -460;
            m_dAxisMin[2] = 0;
            m_dAxisMin[3] = -460;

            m_dAxisMax[0] = 550;
            m_dAxisMax[1] = 0;
            m_dAxisMax[2] = 550;
            m_dAxisMax[3] = 0;
        }

        public static bool Initialize()
        {
            try
            {
                int iR = 0;
                string strSetting = System.AppDomain.CurrentDomain.BaseDirectory + "Settings.xml";
                string str = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                str += "\\Earain\\MarkStudio\\Settings.xml";
                iR = NativeMethods.ELM_LoadSystemSettingXmlFile(0, 0, str);
               if (iR < 0)
               {
                   return false;
               }
               else
               {
                   iR = NativeMethods.ELM_Init(0, 0);
                   if (iR < 0)
                   {
                       return  false;
                   }
                   else
                   {
                    m_bCardOK = true;
                    return true;
                   }
                   // ELM_OPTIC_SETTING optic = new ELM_OPTIC_SETTING();
                 //   NativeMethods.ELM_GetOpticSetting(0, ref optic);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

      //  private static int m_mksIndex = -1;
        public static int Loadmodelfile(ref string path)
        {
            int   m_mksIndex = NativeMethods.ELM_OpenMksFile(path);
            return m_mksIndex;
        }

        public static void UnLoadmodelfile(int m_mksIndex)
        {
            if (m_mksIndex >= 0)
            {
                NativeMethods.ELM_CloseMksFile(m_mksIndex);
            }       
        }
        public static bool SetPresaveMode(int m_mksIndexSetPresaveMode)
        {
            if (m_mksIndexSetPresaveMode >= 0)
            {
                ELM_MARK_SETTING MarkSetting = new ELM_MARK_SETTING();
                NativeMethods.ELM_GetMarkSetting(0, ref MarkSetting);
                MarkSetting.bCacheMode = 1;
                NativeMethods.ELM_SetMarkSetting(0, ref MarkSetting);
                NativeMethods.ELM_PrepareMark(0, m_mksIndexSetPresaveMode);
                return true;
            }else
            {
                return false;
            }
        }

       private static bool   isfirstmark = true;
       public static int MarkFlyByInIOSignal(bool withtrriger,int m_mksIndex)
       {
           if (withtrriger)
           {
               long piSource = 0;
               int trrigerresult = NativeMethods.ELM_WaitForExternalTrigger(0, 1000000, ref piSource);
               if (trrigerresult == 1)
               {
                    int markre = -1;
                   if (isfirstmark)
                   {
                        markre = NativeMethods.ELM_MarkMksFile(0, m_mksIndex, 0);
                       isfirstmark = false;
                   }
                   else
                   {
                        markre = NativeMethods.ELM_MarkMksFile(0, m_mksIndex, 2);
                   }

                    if (markre > 0)
                    {
                        return 11;
                    }
                    else
                    {
                        return 10;
                    }

                }
               else
               {
                   return trrigerresult;
               }
           }
           else
           {
               int ir = -1;
               if (isfirstmark)
               {
                   ir =   NativeMethods.ELM_MarkMksFile(0, m_mksIndex, 0);
                   isfirstmark = false;
               }
               else
               {
                   ir =  NativeMethods.ELM_MarkMksFile(0, m_mksIndex, 2);
               }

               if (ir > 0)
               {
                    int waitre = NativeMethods.ELM_WaitForMarkEnd(0, 100000);
                    if (waitre > 0)
                    {
                        return 21;
                    }
                    else
                    {
                        return 20;
                    }                 
                }
               else
               {
                   return 201;
               }

           }



     }
            
        public static bool ChangeTextByName(ref string Name,ref string txt,int m_mksIndex)
        {
            if (NativeMethods.ELM_EditTextObj(m_mksIndex, Name, txt) < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool Mark(int m_mksIndex)
        {
            if (NativeMethods.ELM_MarkMksFile(0, m_mksIndex, 0) < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void Close(int m_mksIndex)
        { 
            NativeMethods.ELM_CloseMksFile(m_mksIndex);

            NativeMethods.ELM_Unload();
            if (m_bCardOK)
            {
                int i = NativeMethods.ELM_Uninit();
                m_bCardOK = false;
            }
                    
        }

  

        public static string GetErrorString(int iErr)
        {
            string strErr = null;
            switch (iErr)
            {
                case MSI_ERR_NOT_INIT:
                    strErr = "LaserMark Err: Can't Initialized";
                    break;

                case MSI_ERR_CROSS_MKS:
                    strErr = "LaserMark Err: Can't Open Cross File, Please Check cross.mks File";
                    break;

                case MSI_ERR_NPCALIB_MKS:
                    strErr = "LaserMark Err: Can't Open NPCalib File, Please Check NPC.mks File";
                    break;

                default:
                    break;
            }
            return strErr;
        }

        private static Mutex[] m_MarkMutex = new Mutex[2];

        public static void WaitMarkMutex(int iCard)
        {
            if (iCard > 0)
                iCard--;
            m_MarkMutex[iCard].WaitOne();
        }

        public static void ReleaseMarkMutex(int iCard)
        {
            if (iCard > 0)
                iCard--;
            m_MarkMutex[iCard].ReleaseMutex();
        }

        public static int SetExtendAxisHome()
        {
            MSI.WaitMarkMutex(MOVE_CARD);

            int i = NativeMethods.ELM_ResetExtendAxis(MOVE_CARD, 1);
            if (i < 0)
            {
                MSI.ReleaseMarkMutex(MOVE_CARD);
                return -1;
            }
            m_dAxis[0] = 0;
            m_dAxis[1] = 0;
            m_dAxis[2] = 0;
            m_dAxis[3] = 0;
            MSI.ReleaseMarkMutex(MOVE_CARD);
            return 0;
        }

        public static int CheckExtendAxisMoveDone()
        {
            int iR = 0;
            int iAxis1 = 0, iAxis2 = 0, iAxis3 = 0, iAxis4 = 0;
            MSI.WaitMarkMutex(MOVE_CARD);
            int i = NativeMethods.ELM_GetExtendAxisState(MOVE_CARD, ref iAxis1, ref iAxis2, ref iAxis3, ref iAxis4);
            if (i < 0)  // 错误
            {
                MSI.ReleaseMarkMutex(MOVE_CARD);
                return -1;
            }
            //System.Diagnostics.Debug.WriteLine("{0}, {1}, {2}, {3}", icx, icy, icz, icr);
            if ((iAxis1 == 0) && (iAxis2 == 0) && (iAxis3 == 0) && (iAxis4 == 0))
            {
                iR = 1;
            }
            MSI.ReleaseMarkMutex(MOVE_CARD);
            return iR;
        }

        public static int JogMoveAxis(int iAxis, bool bDir, bool bSlow)
        {
            int iR = 0;
            double d = 0;

            int i = iAxis - 1;

            double dStep = m_dJogStep;
            if (bSlow)
                dStep = m_dJogSlowStep;

            if (bDir == false)
                d = m_dAxis[i] - dStep;
            else
                d = m_dAxis[i] + dStep;

            iR = SetMoveExtendAxis(iAxis, d);

            return 0;
        }

        public static int JogMoveAxisFullRange(int iAxis, bool bDir)
        {
            int iR = 0;
            double d = 0;

            int i = iAxis - 1;

            if (bDir == false)
                d = m_dAxisMin[i];
            else
                d = m_dAxisMax[i];

            iR = SetMoveExtendAxis(iAxis, d);

            return 0;
        }

        public static int SetMoveExtendAxis(int iAxis, double dPosition)
        {
            int iPulse = 0;
            switch (iAxis)
            {
                case 1:
                    iPulse = (int)(m_dAxisDir[0] * dPosition / m_dAxisUnit[0]);
                    break;

                case 2:
                    iPulse = (int)(m_dAxisDir[1] * dPosition / m_dAxisUnit[1]);
                    break;

                case 3:
                    iPulse = (int)(m_dAxisDir[2] * dPosition / m_dAxisUnit[2]);
                    break;

                case 4:
                    iPulse = (int)(m_dAxisDir[3] * dPosition / m_dAxisUnit[3]);
                    break;

                default:
                    return -1;
            }
            WaitMarkMutex(MOVE_CARD);
            int iM = NativeMethods.ELM_MoveExtendAxis(MOVE_CARD, iAxis, iPulse);
            if (iM < 0)
            {
                System.Diagnostics.Debug.WriteLine("ELM_MoveExtendAxis Err");
                MSI.ReleaseMarkMutex(MOVE_CARD);
                return -1;
            }
            ReleaseMarkMutex(MOVE_CARD);
            return 0;
        }

        public static int SetMarkEndOutput()
        {
            int iR = 0;
            MSI.WaitMarkMutex(MSI.MOVE_CARD);
            iR = NativeMethods.ELM_SetExtendOutputCtrl(MSI.MOVE_CARD, m_iMarkEndOutput, 1);
            Thread.Sleep(m_iMarkEndPulseWidth);
            iR = NativeMethods.ELM_SetExtendOutputCtrl(MSI.MOVE_CARD, m_iMarkEndOutput, 0);
            MSI.ReleaseMarkMutex(MSI.MOVE_CARD);
            return iR;
        }

        public static int SetWidthRightOutput(int i)
        {
            int iR = 0;
            MSI.WaitMarkMutex(MSI.MOVE_CARD);
            iR = NativeMethods.ELM_SetExtendOutputCtrl(MSI.MOVE_CARD, m_iWidthOutput, i);
            MSI.ReleaseMarkMutex(MSI.MOVE_CARD);
            return iR;
        }

        public static int GetExtendAxis(int iAxis, ref double dAxis)
        {
            int iPulse = 0;
            int iR = NativeMethods.ELM_GetExtendAxis(MSI.MOVE_CARD, iAxis, ref iPulse);
            switch (iAxis)
            {
                case 1:
                    dAxis = (m_dAxisDir[0] * iPulse * m_dAxisUnit[0]);
                    break;

                case 2:
                    dAxis = (m_dAxisDir[1] * iPulse * m_dAxisUnit[1]);
                    break;

                case 3:
                    dAxis = (m_dAxisDir[2] * iPulse * m_dAxisUnit[2]);
                    break;

                case 4:
                    dAxis = (m_dAxisDir[3] * iPulse * m_dAxisUnit[3]);
                    break;

                default:
                    return -1;
            }
            m_dAxis[iAxis - 1] = dAxis;
            return 0;
        }

        public static int OpenMksFile(string pstrPath)
        {
            int i = NativeMethods.ELM_OpenMksFile(pstrPath);
            return i;
        }

        public static bool DrawMksFile(int m_mksIndex, IntPtr HWnd)
        {
            NativeMethods.ELM_DrawMksFile(m_mksIndex, HWnd);
            return true;
        }

        //////////////////////////////////////////////////////////////////////////
        private static Thread threadInitLaser = null;

        private static string mMarkStudioFile = null;
        //////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////////
        public static bool EditMarkStudioForm(string strFilePath, string strMksName/*strEzdCadName*/)
        {
            if (threadInitLaser != null)
            {
                //判断当前线程是否结束
                if (threadInitLaser.IsAlive)
                {
                    return false;
                }
            }
            if (!File.Exists(Application.StartupPath + "\\" + strMksName))
            {
                MessageBox.Show(strMksName.ToString() + ":文件不存在");
                return false;
            }

            NativeMethods.ELM_OpenMksFile(strFilePath);
            mMarkStudioFile = strFilePath;
            threadInitLaser = new Thread(threadInitHglaser);
            threadInitLaser.Start(strMksName);
            return true;
        }

   

        private static void threadInitHglaser(object strCadName)
        {
            int mksindex = 0;
            Close(mksindex);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.CreateNoWindow = true;//设不显示窗口
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = Application.StartupPath + @"\" + strCadName.ToString();
            process.StartInfo.Arguments = mMarkStudioFile;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            try
            {
                process.Start();
            }
            catch (System.ComponentModel.Win32Exception we)
            {
                process.Dispose();
                MessageBox.Show(we.Message);
                return;
            }
            process.WaitForExit();
            process.Dispose();
            //if (!InitLaser(m_Ptr))
            //{
            //    MessageBox.Show("初始化激光器失败!");
            //    return;
            //}
            return;
        }
    }
}