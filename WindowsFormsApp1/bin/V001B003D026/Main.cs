/*
 *版本号：uppermasterV001B001D004
 * 日期：22年07月13日09时41分
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO.Ports;
using Microsoft.Office.Interop;
using System.Messaging;

namespace WindowsFormsApp1
{
  public partial class Main : Form
  {
    private static AutoResetEvent ackRcvd = new AutoResetEvent(false);
    int[] CheckArray = new int[8];      //存放哪些数据显示哪些不显示
    private static byte lastSentOpcode = 0;
    public static Main frm1;
    public int Signal_Send_Data = 0;
    public int count_ACK = 1;
    public int signle = 0;
    byte[] Null = new byte[8];
    const byte ID_RACK1 = 0x01;
    const byte ID_RACK2 = 0x02;
    const byte ID_RACK3 = 0x03;
    const byte ID_Shelf1 = 0x01;
    const byte ID_Shelf2 = 0x02;
    const byte ID_Shelf3 = 0x03;
    const byte ID_Shelf4 = 0x04;
    const byte ID_Shelf5 = 0x05;
    const byte ID_Shelf6 = 0x06;
    const byte CMD_Summary = 0x01;
    const byte CMD_Cell_Temperature = 0x02;
    const byte CMD_Cell_Voltage = 0x03;
    const byte CMD_All_Parameter_Read = 0x11;
    const byte ParameterSet = 0x12;
    public byte RACK_ID_Number_Sent; //生成要发送的RACKID
    public byte Shelf_ID_Number_Sent; //生成要发送的Shelf ID
    public byte Timer_Count;//时间定时器
    public byte CMD_CalibrationValue = 0x51;//阈值设置
    public byte CMD_ThreshholdValue = 0x50;//校准命令
    public byte CMD_FactoryVaule = 0x52;

    public Byte[] Set_Imation_Reiving_Data = new byte[(255)]; //设置信息接收数据
    public static byte[] Flag_VoltageData = { 0x0A, 0x1D };
    byte[] Header_data = { 0x11, 0x22 };  //接收帧头
    byte[] End_data = { 0x33, 0x44 };     //接收帧尾
    static readonly UInt16[] CRC16Table = new UInt16[256] {
    0x0000,0x1021,0x2042,0x3063,0x4084,0x50a5,0x60c6,0x70e7,
    0x8108,0x9129,0xa14a,0xb16b,0xc18c,0xd1ad,0xe1ce,0xf1ef,
    0x1231,0x0210,0x3273,0x2252,0x52b5,0x4294,0x72f7,0x62d6,
    0x9339,0x8318,0xb37b,0xa35a,0xd3bd,0xc39c,0xf3ff,0xe3de,
    0x2462,0x3443,0x0420,0x1401,0x64e6,0x74c7,0x44a4,0x5485,
    0xa56a,0xb54b,0x8528,0x9509,0xe5ee,0xf5cf,0xc5ac,0xd58d,
    0x3653,0x2672,0x1611,0x0630,0x76d7,0x66f6,0x5695,0x46b4,
    0xb75b,0xa77a,0x9719,0x8738,0xf7df,0xe7fe,0xd79d,0xc7bc,
    0x48c4,0x58e5,0x6886,0x78a7,0x0840,0x1861,0x2802,0x3823,
    0xc9cc,0xd9ed,0xe98e,0xf9af,0x8948,0x9969,0xa90a,0xb92b,
    0x5af5,0x4ad4,0x7ab7,0x6a96,0x1a71,0x0a50,0x3a33,0x2a12,
    0xdbfd,0xcbdc,0xfbbf,0xeb9e,0x9b79,0x8b58,0xbb3b,0xab1a,
    0x6ca6,0x7c87,0x4ce4,0x5cc5,0x2c22,0x3c03,0x0c60,0x1c41,
    0xedae,0xfd8f,0xcdec,0xddcd,0xad2a,0xbd0b,0x8d68,0x9d49,
    0x7e97,0x6eb6,0x5ed5,0x4ef4,0x3e13,0x2e32,0x1e51,0x0e70,
    0xff9f,0xefbe,0xdfdd,0xcffc,0xbf1b,0xaf3a,0x9f59,0x8f78,
    0x9188,0x81a9,0xb1ca,0xa1eb,0xd10c,0xc12d,0xf14e,0xe16f,
    0x1080,0x00a1,0x30c2,0x20e3,0x5004,0x4025,0x7046,0x6067,
    0x83b9,0x9398,0xa3fb,0xb3da,0xc33d,0xd31c,0xe37f,0xf35e,
    0x02b1,0x1290,0x22f3,0x32d2,0x4235,0x5214,0x6277,0x7256,
    0xb5ea,0xa5cb,0x95a8,0x8589,0xf56e,0xe54f,0xd52c,0xc50d,
    0x34e2,0x24c3,0x14a0,0x0481,0x7466,0x6447,0x5424,0x4405,
    0xa7db,0xb7fa,0x8799,0x97b8,0xe75f,0xf77e,0xc71d,0xd73c,
    0x26d3,0x36f2,0x0691,0x16b0,0x6657,0x7676,0x4615,0x5634,
    0xd94c,0xc96d,0xf90e,0xe92f,0x99c8,0x89e9,0xb98a,0xa9ab,
    0x5844,0x4865,0x7806,0x6827,0x18c0,0x08e1,0x3882,0x28a3,
    0xcb7d,0xdb5c,0xeb3f,0xfb1e,0x8bf9,0x9bd8,0xabbb,0xbb9a,
    0x4a75,0x5a54,0x6a37,0x7a16,0x0af1,0x1ad0,0x2ab3,0x3a92,
    0xfd2e,0xed0f,0xdd6c,0xcd4d,0xbdaa,0xad8b,0x9de8,0x8dc9,
    0x7c26,0x6c07,0x5c64,0x4c45,0x3ca2,0x2c83,0x1ce0,0x0cc1,
    0xef1f,0xff3e,0xcf5d,0xdf7c,0xaf9b,0xbfba,0x8fd9,0x9ff8,
    0x6e17,0x7e36,0x4e55,0x5e74,0x2e93,0x3eb2,0x0ed1,0x1ef0
   };
    System.DateTime currentTime = new System.DateTime();

    public struct SHELF
    {
      public Moudle_Temp moudle_Temp;  //温度
      public Summary summary;    //概要
      public FactorySetting FactorySetting_Read;//出厂设置信息读取
      public CalibrationValue CalibrationValue_Read; //校准值读取
      public ThreshholdValue ThreshholdValue_Read; //阈值读取
      public Moudle_Voltage voltage;//电压
    };
    public string[] SerialNuber_data = System.IO.Ports.SerialPort.GetPortNames();

    public Byte[] Rack1Shelf1Summary = new byte[136];      //摘要信息的创建
    public Byte[] Rack1Shelf1Voltage = new byte[136];   //电压信息
    public Byte[] Rack1Shelf1Temperature = new byte[136];//温度信息
    public Byte[]  Rack1Shelf1FactoryValue = new byte[150];   //设置信息
    public Byte[] Rack1Shelf1ThreshholdValue = new byte[150];   //设置信息
    public Byte[] Rack1Shelf1CalibrationValue = new byte[150];   //设置信息
    public struct SetStructure
    {
      public UInt16 ID;
      public byte reseve;
      public byte reseve1;
      public Int32 Value;
    };
    //调整
    enum temp1
    {
      a = 1,
      b,
    };
    SetStructure setStructure;
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    //概要信息
    public struct Summary
    {
      public UInt16 TotalVol_1;
      public UInt16 TotalVol_2;
      public UInt16 TotalVol_3;
      public UInt16 CurCurrent;
      public UInt16 MaxVol;
      public byte MaxVolID;
      public ushort MinVol;
      public byte MinVolID;
      public byte MaxTemp;
      public byte MaxTempID;
      public byte MinTemp;
      public byte MinTempID;
      public uint warning_pre;
      public uint warning;
      public uint fault;
      public ushort Drive_Status;
      public byte Input_Status;
      public byte Bat_Status;
      public byte SOC;
      public byte SOH;
      public ushort BalanceVol;
      public short MBB1_Balance_Stas;
      public short MBB2_Balance_Stas;
      public short MBB3_Balance_Stas;
      public short MBB4_Balance_Stas;
      public short MBB5_Balance_Stas;
      public short MBB6_Balance_Stas;
      public short FW_version;
      public short HW_version;
      public byte RTC_Y;
      public byte RTC_MON;
      public byte RTC_D;
      public byte RTC_H;
      public byte RTC_MIN;
      public byte RTC_S;
    };
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct FactorySetting
    {
      public byte Cell_Type;
      public byte Cell_Parallel_Config;
      public byte Shelf_Serial_Config;
      public byte Cell_Temperature_Config;
      public UInt16 Rated_Capacity;
      public UInt16 Rated_Voltage;
      public UInt32 BMS_SN;
      public UInt32 Project_Code;
    }
    //阈值信息
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct CalibrationValue
    {
      //参数校准参数
      public Int16 TotalVol_1_Slope;
      public Int16 TotalVol_1_Offset;
      public Int16 TotalVol_2_Slope;
      public Int16 TotalVol_2_Offset;
      public Int16 TotalVol_3_Slope;
      public Int16 TotalVol_3_Offset;
      public Int16 Current_Slope;
      public Int16 Current_Offset;
      public UInt16 SOC;
    };
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct FactoryVaule
    {
      public byte Cell_Type;
      public byte Cell_Parallel_Config;
      public byte Shelf_Serial_Config;
      public byte Cell_Temperature_Config;
      public UInt16 Rated_Capacity;
      public UInt16 Rated_Voltage;
      public UInt32 BMS_SN;
      public UInt32 Project_Code;
      public byte Voltage_Platform;
    }
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ThreshholdValue
    {
      public UInt16 Cell_OV_Warning_Pre;
      public UInt16 Cell_OV_Warning;
      public UInt16 Cell_OV_Fault;
      public UInt16 Battery_OV_Warning_Pre;
      public UInt16 Battery_OV_Warning;
      public UInt16 Battery_OV_Fault;
      public UInt16 Chg_OCur_Warning_Pre;
      public UInt16 Chg_OCur_Warning;
      public UInt16 Chg_OCur_Fault;
      public UInt16 Chg_Utemp_Warning_Pre;
      public UInt16 Chg_Utemp_Warning;
      public UInt16 Chg_Utemp_Fault;
      public UInt16 Chg_Otemp_Warning_Pre;
      public UInt16 Chg_Otemp_Warning;
      public UInt16 Chg_Otemp_Fault;
      public UInt16 Cell_UV_Warning_Pre;
      public UInt16 Cell_UV_Warning;
      public UInt16 Cell_UV_Fault;
      public UInt16 Battery_UV_Warning_Pre;
      public UInt16 Battery_UV_Warning;
      public UInt16 Battery_UV_Fault;
      public UInt16 DisChg_OCur_Warning_Pre;
      public UInt16 DisChg_OCur_Warning;
      public UInt16 DisChg_OCur_Fault;
      public UInt16 DisChg_UTemp_Warning_Pre;
      public UInt16 DisChg_UTemp_Warning;
      public UInt16 DisChg_UTemp_Fault;
      public UInt16 DisChg_Otemp_Warning_Pre;
      public UInt16 DisChg_Otemp_Warning;
      public UInt16 DisChg_Otemp_Fault;
      public UInt16 SOC_Low_Warning_Pre;
      public UInt16 SOC_Low_Warning;
      public UInt16 SOC_Low_Fault;
      public UInt16 V_Diff_Warning_Pre;
      public UInt16 V_Diff_Warning;
      public UInt16 V_Diff_Fault;
      public UInt16 T_Diff_Warning_Pre;
      public UInt16 T_Diff_Warning;
      public UInt16 T_Diff_Fault;
    }
    //电压信息定义
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Moudle_Voltage
    {
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
      public UInt16[] Cell_Vol;
    };
    //温度定义
    public struct Moudle_Temp
    {
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
      public byte[] Cell_Temp;
    };
    public SHELF RACK1SHLE1;
    public SHELF RACK1SHLE2;
    public SHELF RACK1SHLF3;

    public byte[] SendSetCMDID = {0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,
                                0x0D,0x0E,0x0F,0x10,0x11,0x12,0x13,0x14,0x15,0x16,0x17,0x18,0x19,
                                0x1A,0x1B,0x1C,0x1D,0x1E,0x1F,0x20,0x21,0x22,0x23,0x24,0x25,0x26,0x27
    };
    public byte[] SendSetCMDID1 = { 0xA0, 0xA1, 0xA2, 0xA3, 0xA4, 0xA5, 0xA6, 0xA7, 0xA8, 0xA9 };
    public delegate void printString(SHELF a);
    public Main()
    {
      InitializeComponent();
      x = this.Width;
      y = this.Height;
      setTag(this);
      RS485_select();
      frm1 = this;
      LoadInitialization();
      Timer();
      Control.CheckForIllegalCrossThreadCalls = false;
      Factory_setting f = new Factory_setting(); // 首次带参数打开的Bug
    }
    /************************************************************************		
    *name: 所有初始化选项
    *************************************************************************/
    #region

    /************************************************************************		
    *name:  LoadInitialization
    *describe: 初始化子界面关闭
    * *data : 2022.3.29 Alex
    *************************************************************************/
    private void LoadInitialization()
    {
      Panel_ChildSerialPort.Visible = false;
      panel2_RACK1.Visible = false;
      panel3_RACK2.Visible = false;
      panel4_RACK3.Visible = false;
      Button6_Download.Enabled = false;
      uiLight6.State = Sunny.UI.UILightState.Off;
      Panel_ChildSerialPort.Visible = false;

    }
    #region 控件大小随窗体大小等比例缩放
    private float x;//定义当前窗体的宽度
    private float y;//定义当前窗体的高度

    /// <summary>
    /// 将控件的宽，高，左边距，顶边距和字体大小暂存到tag属性中
    /// </summary>
    /// <param name="cons">递归控件中的控件</param>
    private void setTag(Control cons)
    {
      foreach (Control con in cons.Controls)
      {
        con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
        if (con.Controls.Count > 0)
        {
          setTag(con);
        }
      }
    }


    /// <summary>
    /// 重新设置控件的属性
    /// </summary>
    /// <param name="newx">设置后新的x坐标</param>
    /// <param name="newy">设置后新的y坐标</param>
    /// <param name="cons">递归控件中的控件</param>
    private void setControls(float newx, float newy, Control cons)
    {
      //遍历窗体中的控件，重新设置控件的值
      foreach (Control con in cons.Controls)
      {
        //获取控件的Tag属性值，并分割后存储字符串数组
        if (con.Tag != null)
        {
          string[] mytag = con.Tag.ToString().Split(new char[] { ';' });
          //根据窗体缩放的比例确定控件的值
          con.Width = Convert.ToInt32(System.Convert.ToSingle(mytag[0]) * newx);//宽度
          con.Height = Convert.ToInt32(System.Convert.ToSingle(mytag[1]) * newy);//高度
          con.Left = Convert.ToInt32(System.Convert.ToSingle(mytag[2]) * newx);//左边距
          con.Top = Convert.ToInt32(System.Convert.ToSingle(mytag[3]) * newy);//顶边距
          Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
          con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
          if (con.Controls.Count > 0)
          {
            setControls(newx, newy, con);
          }
        }
      }
    }


    /// <summary>
    /// 窗体变化大小方法
    /// </summary>
    private void Form1_Resize(object sender, EventArgs e)
    {
      float newx = (this.Width) / x;
      float newy = (this.Height) / y;
      setControls(newx, newy, this);
    }

    #endregion
    /************************************************************************		
    *name:  RS485_select
    *describe: RS485配置文件
    * *data : 2022.3.29 Alex
    *************************************************************************/

    private void RS485_select()
    {
      string[] SerialNuber_data = System.IO.Ports.SerialPort.GetPortNames();
      string[] Baud_data = {"110","300","600","1200","2400","4800","9600","14400","19200",
                        "38400","56000","57600","115200","128000","256000"};
      string[] CheckDigit_data = { "NONE", "ODD", "EVEN", "MARK", "SPACE" };
      string[] DataBits_data = { "5", "6", "7", "8" };
      string[] StopBits_data = { "1", "1.5", "2" };
      //加载所有预选数据
      comboBox_SerialNumber.Items.AddRange(SerialNuber_data);
      comboBox_BaudRate.Items.AddRange(Baud_data);
      comboBox_CheckDigit.Items.AddRange(CheckDigit_data);
      comboBox_DataBits.Items.AddRange(DataBits_data);
      comboBox_StopBit.Items.AddRange(StopBits_data);
      //初始显示预选数据
      comboBox_SerialNumber.Text = "SerialPort";
      comboBox_BaudRate.Text = "115200";
      comboBox_CheckDigit.Text = "NONE";
      comboBox_DataBits.Text = "8";
      comboBox_StopBit.Text = "1";
    }
    private void Timer()
    {
      System.Timers.Timer t = new System.Timers.Timer(200);//实例化Timer类，设置间隔时间为500毫秒；
      t.Elapsed += new System.Timers.ElapsedEventHandler(Poll_Query);//到达时间的时候执行事件；
      t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
      t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
    }

    #endregion
    /************************************************************************		
    *name:  所有底层接口函数
    *************************************************************************/
    #region
    /************************************************************************		
    *name:  OpenOrCloseSubmenu
    *describe: 打开或者关闭子界面接口程序
    * *data : 2022.3.29 Alex
    *************************************************************************/
    private void OpenOrCloseSubmenu(Panel childPanle)
    {
      if (childPanle.Visible == true)
      {
        childPanle.Visible = false;
      }
      else if (childPanle.Visible == false)
      {
        childPanle.Visible = true;
      }
      else
      {
        //None
      }
    }
    /************************************************************************ 	
      *name:	CRC
      *describe: CRC校验
      *data : 2022.5.5 Alex
      *************************************************************************/

    public static UInt16 calCRC16(byte[] dataIn, int length)
    {
      UInt16 i;
      UInt16 nAccum = 0;

      for (i = 0; i < length; i++)
        nAccum = (UInt16)((nAccum << 8) ^ (UInt16)CRC16Table[(nAccum >> 8) ^ dataIn[i]]);
      return nAccum;
    }
    /************************************************************************		
      *name:  getCheckSum
      *describe: 
      * *data : 2022.3.29 Alex
      *************************************************************************/
    public static byte getCheckSum(byte[] packBytes, int length)
    {
      UInt64 checkSum = 0x0024;
      for (int i = 0; i < length; i++)
      {
        checkSum += packBytes[i];//计算和校验
      }
      checkSum &= 0x00ff; //取低八位  
      return (byte)checkSum;
    }
    /************************************************************************		
    *name:  OpenChildFrom
    *describe: 打开别的别的子Form
    * *data : 2022.3.29 Alex
    *************************************************************************/
    private Form activeForm = null;
    private void OpenChildFrom(Form childForm)
    {
      if (activeForm != null)
      activeForm.Close();
      childForm.TopLevel = false;
      childForm.FormBorderStyle = FormBorderStyle.None;
      childForm.Dock = DockStyle.Fill;
      panel3.Controls.Add(childForm);
      panel3.Tag = childForm;
      childForm.BringToFront();
      childForm.Show();
    }
    /************************************************************************		
      *name:  Reverse
      *describe: 所有大小端转化
      *data : 2022.5.5 Alex
      *************************************************************************/
    public static long Reverse(long value)
    {
      return (((long)Reverse((int)value) & 0xFFFFFFFF) << 32)
              | ((long)Reverse((int)(value >> 32)) & 0xFFFFFFFF);
    }

    public static ulong Reverse(ulong value)
    {
      return (((ulong)Reverse((uint)value) & 0xFFFFFFFF) << 32)
              | ((ulong)Reverse((uint)(value >> 32)) & 0xFFFFFFFF);
    }

    public static int Reverse(int value)
    {
      return (((int)Reverse((short)value) & 0xFFFF) << 16)
              | ((int)Reverse((short)(value >> 16)) & 0xFFFF);
    }

    public static uint Reverse(uint value)
    {
      return (((uint)Reverse((ushort)value) & 0xFFFF) << 16)
              | ((uint)Reverse((ushort)(value >> 16)) & 0xFFFF);
    }

    public static short Reverse(short value)
    {
      return (short)((((int)value & 0xFF) << 8) | (int)((value >> 8) & 0xFF));
    }

    public static ushort Reverse(ushort value)
    {
      return (ushort)((((int)value & 0xFF) << 8) | (int)((value >> 8) & 0xFF));
    }
    /************************************************************************		
    *name:  SerialPort_Send
    *describe: 传口发送数据接口函数
    *data : 2022.3.29 Alex
    *************************************************************************/
    public void SerialPort_Send(string Text_M)
    {
      try
      {
        if (serialPort1.IsOpen)
        {
          //串口处于开启状态，将发送区文本发送
          serialPort1.Write(Text_M);
          textBox1.AppendText("Send:" + Text_M + "\r\n");
        }
        else
        {
          //None;
        }
      }
      catch (Exception ex)
      {
        //响铃并显示异常给用户
        System.Media.SystemSounds.Beep.Play();
        MessageBox.Show(ex.Message);
      }
    }
    /************************************************************************		
      *name: data_processing
      *describe: 转换成浮点
      *data : 2022.4.18 Alex
      *************************************************************************/
    private float data_processing(short temp)
    {
      float data = (float)temp;
      return data;
    }
    /************************************************************************		
    *name:  ShortGetBit
    *describe: short拆成位的函数
    *data : 2022.4.18 Alex
    *************************************************************************/
    public static int ShortGetBit(ushort data, short index)
    {
      short x = 1;
      switch (index)
      {
        case 0: { x = 0x0001; } break;
        case 1: { x = 0x0002; } break;
        case 2: { x = 0x0004; } break;
        case 3: { x = 0x0008; } break;
        case 4: { x = 0x0010; } break;
        case 5: { x = 0x0020; } break;
        case 6: { x = 0x0040; } break;
        case 7: { x = 0x0080; } break;
        case 8: { x = 0x0100; } break;
        case 9: { x = 0x0200; } break;
        case 10: { x = 0x0400; } break;
        case 11: { x = 0x0800; } break;
        case 12: { x = 0x1000; } break;
        case 13: { x = 0x2000; } break;
        case 14: { x = 0x4000; } break;

        default: { return 0; }
      }
      return (data & x) == x ? 1 : 0;
    }
    /************************************************************************		
      *name:  UintGetBit
      *describe: Uint拆成位的函数
      *data : 2022.4.18 Alex
      * 
      *************************************************************************/
    public static int UintGetBit(uint data, short index)
    {
      uint x = 1;
      switch (index)
      {
        case 0: { x = 0x0001; } break;
        case 1: { x = 0x0002; } break;
        case 2: { x = 0x0004; } break;
        case 3: { x = 0x0008; } break;
        case 4: { x = 0x0010; } break;
        case 5: { x = 0x0020; } break;
        case 6: { x = 0x0040; } break;
        case 7: { x = 0x0080; } break;
        case 8: { x = 0x0100; } break;
        case 9: { x = 0x0200; } break;
        case 10: { x = 0x0400; } break;
        case 11: { x = 0x0800; } break;
        case 12: { x = 0x1000; } break;
        case 13: { x = 0x2000; } break;
        case 14: { x = 0x4000; } break;
        case 15: { x = 0x8000; } break;
        case 16: { x = 0x10000; } break;
        case 17: { x = 0x20000; } break;
        case 18: { x = 0x40000; } break;
        case 19: { x = 0x80000; } break;
        case 20: { x = 0x100000; } break;
        case 21: { x = 0x200000; } break;
        case 22: { x = 0x400000; } break;
        case 23: { x = 0x800000; } break;
        case 24: { x = 0x1000000; } break;

        default: { return 0; }
      }
      return (data & x) == x ? 1 : 0;
    }
    /************************************************************************		
      *name:  StructToBytes
      *describe: 结构体转数组
      *data : 2022.3.29 Alex
      *************************************************************************/
    public byte[] StructToBytes(object anyStruct)
    {
      int size = Marshal.SizeOf(anyStruct);
      IntPtr bytesPtr = Marshal.AllocHGlobal(size);
      Marshal.StructureToPtr(anyStruct, bytesPtr, false);
      byte[] bytes = new byte[size];
      Marshal.Copy(bytesPtr, bytes, 0, size);
      Marshal.FreeHGlobal(bytesPtr);
      return bytes;
    }
    /************************************************************************		
      *name:  BytesToDataStruct
      *describe:数组转结构体 A = (object)BytesToDataStruct(byte[] bytes, Type type);
      *data : 2022.5.5 Alex
      *************************************************************************/
    private object BytesToDataStruct(byte[] bytes, Type type)
    {
      int size = Marshal.SizeOf(type);
      if (size > bytes.Length)
      {
        return null;
      }
      IntPtr structPtr = Marshal.AllocHGlobal(size);
      Marshal.Copy(bytes, 0, structPtr, size);
      object obj = Marshal.PtrToStructure(structPtr, type);
      Marshal.FreeHGlobal(structPtr);
      return obj;
    }




    public bool SendPacketAck(byte[] buffer, int len)
    {
      if (!serialPort1.IsOpen)
      {
        MessageBox.Show("Please open the serial port." + "\r\n");
      }
      lastSentOpcode = buffer[1];
      serialPort1.Write(buffer, 0, len);
      Thread.Sleep(50);
      if (ackRcvd.WaitOne(50))
      {
        return true;
      }
      return false;
    }
    /************************************************************************		
    *name:  SerialPort1_DataRecive
    *describe: 串口接受数据接口函数
    *data : 2022.3.29 Alex
    *************************************************************************/
    private void SerialPort1_DataRecive(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
    {
      Thread.Sleep(40);
      try
      {
        Byte[] receivedData = new Byte[serialPort1.BytesToRead];        //创建接收字节数组
        serialPort1.Read(receivedData, 0, receivedData.Length);         //读取数据    
                                                                        //帧头帧尾判断
        if ((receivedData[0] == Header_data[0])
            && (receivedData[1] == Header_data[1])
            && (receivedData[receivedData.Length - 2] == End_data[0])
            && (receivedData[receivedData.Length - 1] == End_data[1])
            )
        {
          /************************************************************************		
            *name:  
            *describe:RACK1 Shlef1 信息接收
            *data : 2022.3.29 Alex
            *************************************************************************/
          //RACK1
          if (receivedData[2] == ID_RACK1)
          {
            switch (receivedData[3])
            {
              case ID_Shelf1:
                //摘要信息Summary&detail
                if (receivedData[4] == CMD_Summary)
                {
                  Array.ConstrainedCopy(receivedData, 6, Rack1Shelf1Summary, 0, receivedData.Length - 6);
                  RACKShelf_Summary_decribe(RACK1SHLE1, 0x0101);
                  SetLightShow(RACK1SHLE1);
                }
                //温度信息Cell Temperature 
                else if (receivedData[4] == CMD_Cell_Temperature)
                {
                  Array.ConstrainedCopy(receivedData, 6, Rack1Shelf1Temperature, 0, receivedData.Length - 6);
                  RACKShelf_Cell_Temp_decribe(RACK1SHLE1, 0x0101);
                }
                //电压信息 Cell voltage
                else if (receivedData[4] == CMD_Cell_Voltage)
                {
                  Array.ConstrainedCopy(receivedData, 6, Rack1Shelf1Voltage, 0, receivedData.Length - 6);
                  RACKShelf_Cele_Vol_decribe(RACK1SHLE1, 0x0101);  //RACK1Shelf1的显示线程
                }
                //CMD_All_Parameter_Read 设置信息
                else if (receivedData[4] == CMD_All_Parameter_Read)
                {
                  if (receivedData[5] == 0x4E)
                  {
                    Array.ConstrainedCopy(receivedData, 6, Rack1Shelf1ThreshholdValue, 0, receivedData.Length - 6);
                    app_ThreshholdValue_Read(RACK1SHLE1, 0x0101);
                  }
                  else if (receivedData[5] == 0x10)
                  {
                    Array.ConstrainedCopy(receivedData, 6, Rack1Shelf1CalibrationValue, 0, receivedData.Length - 6);
                    app_CalibrationValue_Read(RACK1SHLE1, 0x0101);
                  }
                  else if(receivedData[5] == 0x11)
                  {
                    Array.ConstrainedCopy(receivedData, 6, Rack1Shelf1FactoryValue, 0, receivedData.Length - 6);
                    app_Factory_Setting(RACK1SHLE1, 0x0101);
                  }
                  else
                  {

                  }
                  /*

                  else
                  {

                  }
                  */
                }
                else if (receivedData[4] == ParameterSet)
                {
                  SetRepleFunction(receivedData[7]);
                }


                break;

              case ID_Shelf2:
                if (receivedData[4] == CMD_Summary)
                {
                  Array.ConstrainedCopy(receivedData, 6, Rack1Shelf1Summary, 0, receivedData.Length - 6);
                  RACKShelf_Summary_decribe(RACK1SHLE2, 0x0102);
                }


                //温度信息Cell Temperature 
                else if (receivedData[4] == CMD_Cell_Temperature)
                {
                  Array.ConstrainedCopy(receivedData, 6, Rack1Shelf1Temperature, 0, receivedData.Length - 6);
                  RACKShelf_Cell_Temp_decribe(RACK1SHLE2, 0x0102);
                }
                //电压信息 Cell voltage
                else if (receivedData[4] == CMD_Cell_Voltage)
                {
                  Array.ConstrainedCopy(receivedData, 6, Rack1Shelf1Voltage, 0, receivedData.Length - 6);
                  RACKShelf_Cele_Vol_decribe(RACK1SHLE2, 0x0102);
                }
                //CMD_All_Parameter_Read 设置信息
                else if (receivedData[4] == CMD_All_Parameter_Read)
                {
                  Array.ConstrainedCopy(receivedData, 6, Rack1Shelf1ThreshholdValue, 0, receivedData.Length - 6);
                  //Parameter_Read(RACK1SHLE2, 0x0101);
                }
                break;

              case ID_Shelf3:

                break;
              case ID_Shelf4:

                break;
              case ID_Shelf6:

                break;

              default:
                break;

            }
          }
          else
          {
            textBox1.AppendText("received corrupt data" + "\r\n");
          }

        }
      }
      catch { }

    }
    /************************************************************************		
    *name:  HandleDataRx
    *describe: NAck的报错
    *data : 2022.3.29 Alex
    *************************************************************************/
    public void HandleDataRx(byte[] buff)
    {
      if (count_ACK == 1)
      {
        byte opcode = buff[1];

        switch (opcode)
        {
          case 0xCC:

            break;
          case 0xA3:
            textBox1.AppendText("start transferring data" + "\r\n");
            Form_DownLoad.frm_2.ProgressBa_value(8);
            break;
          case 0x25:
            textBox1.AppendText("Download Failed, please try again!  " + "\r\n");
            Form_DownLoad.frm_2.ProgressBa_value(0);
            break;
          case 0x40:
            textBox1.AppendText("Download Failed, please try again!  " + "\r\n");
            Form_DownLoad.frm_2.ProgressBa_value(0);
            break;
          case 0x41:
            textBox1.AppendText("Download Failed, please try again!  " + "\r\n");
            Form_DownLoad.frm_2.ProgressBa_value(0);
            break;
          case 0x42:
            textBox1.AppendText("Download Failed, please try again!  " + "\r\n");
            Form_DownLoad.frm_2.ProgressBa_value(0);
            break;
          case 0x43:
            textBox1.AppendText("Download Failed, please try again!  " + "\r\n");
            Form_DownLoad.frm_2.ProgressBa_value(0);
            break;
          case 0x44:
            textBox1.AppendText("Download Failed, please try again!  " + "\r\n");
            Form_DownLoad.frm_2.ProgressBa_value(0);
            break;
          case 0x45:
            textBox1.AppendText("Download Failed, please try again!  " + "\r\n");
            Form_DownLoad.frm_2.ProgressBa_value(0);
            break;
          case 0x33:
            textBox1.AppendText("Download Failed, please try again!  " + "\r\n");
            signle = 1;
            break;
          default:
            textBox1.AppendText("Download Failed, please try again!  " + "\r\n");
            break;
        }
      }
    }

    /************************************************************************		
      *name:  Send_Data
      *describe: 更新程序发送函数
      *data : 2022.4.1 Alex
      *************************************************************************/
    public void Send_Data()
    {
      byte COMMAND_SEND_DATA = 0xFF;
      var COMMAND_GET_STATUS = new byte[4] { 0x03, 0x23, 0x23, 0xCC };
      var packetBuffer = new byte[134]; // 128 bytes data + 1 length + 1 opcode + 1 terminator
      var dataBuffer = new byte[128];

      packetBuffer[0] = COMMAND_SEND_DATA;
      packetBuffer[1] = 0;
      int temp_value = 4;

      using (Stream fs = openFileDialog1.OpenFile())
      {
        using (BinaryReader reader = new BinaryReader(fs))
        {
          while (reader.BaseStream.Position < reader.BaseStream.Length)
          {
            dataBuffer = new byte[128];
            temp_value++;
            var bytesRead = reader.BaseStream.Read(dataBuffer, 0, 128);
            UInt32 len = (UInt32)reader.BaseStream.Length;
            Form_DownLoad.frm_2.progressBar1.Maximum = (int)len;
            if (signle == 1)
            {
              temp_value = temp_value - 1;
              signle = 0;
            }
            else
            {
              if (bytesRead == 252)
              {
                //  Form_DownLoad.frm_2.progressBa_value(temp_value);
                Array.ConstrainedCopy(dataBuffer, 0, packetBuffer, 4, 128);
              }
              else // readjust size (this is the last chunk!)
              {
                Array.ConstrainedCopy(dataBuffer, 0, packetBuffer, 4, 128);
                //packetBuffer[0] = (byte)(bytesRead + 3); // readjust size byte
                //  Form_DownLoad.frm_2.progressBa_value(99);
              }
            }
            packetBuffer[0] = 0x01;
            packetBuffer[1] = 0x10;
            packetBuffer[2] = 0x60;
            packetBuffer[3] = 0x06;
            // packetBuffer[1] = getCheckSum(dataBuffer, 252);
            // packetBuffer[2] = 0x24;
            Main.frm1.SendPacketAck(packetBuffer, 134);
            textBox1.AppendText("Downloading..." + "\r\n");
            // Thread.Sleep(1);//休眠时间
            // SendPacketAck(COMMAND_GET_STATUS, 4);
            Form_DownLoad.frm_2.ProgressBa_value((int)reader.BaseStream.Position);
          }
        }
        Thread.Sleep(300);//休眠时间

        var temp_1 = new byte[7] { 0x01, 0x06, 0x60, 0x00, 0x0b, 0x11, 0x22 };
        SendPacketAck(temp_1, 7);
        textBox1.AppendText(" Completed!" + "\r\n");
        MessageBox.Show(" Completed!" + "\r\n");
        // Form_DownLoad.frm_2.progressBa_value(100);
        Form_DownLoad.frm_2.button_OenBin.Enabled = true;

      }
    }

    /************************************************************************		
      *name:  Serial_Data_Transmission
      *describe: 发送数据函数（处理报文长度帧头等协议规则）
      *data : 2022.5.5 Alex
      *************************************************************************/
    public void Serial_Data_Transmission(byte[] buffer, byte len, byte RackID, byte ShelfID, byte CMD)
    {
      if (serialPort1.IsOpen)
      {
        UInt16 temp = calCRC16(buffer, len);

        Byte[] SendData = new Byte[len + 10];
        SendData[0] = 0xAA;
        SendData[1] = 0xBB;
        SendData[2] = RackID;
        SendData[3] = ShelfID;
        SendData[4] = CMD;
        SendData[5] = len;
        SendData[len + 6] = (byte)(temp & 0xFF);
        SendData[len + 7] = (byte)(temp & 0xFF00 >> 8);
        SendData[len + 8] = 0xCC;
        SendData[len + 9] = 0xDD;
        Array.ConstrainedCopy(buffer, 0, SendData, 6, len);
        SendPacketAck(SendData, len + 10);
      }
      else
      {

      }

    }
    public void ShowchilidFromData(int a)
    {

    }

    public void RACKShelf_Cele_Vol_decribe(SHELF TEMP, int ID)
    {
      //结构体转化将
      TEMP.voltage = (Moudle_Voltage)BytesToDataStruct(Rack1Shelf1Voltage, typeof(Moudle_Voltage));
      switch (ID)
      {
        //RACK1Shlef1
        case 0x0101:
          #region

          if (Shelf1.frm_Shelf1.Visible == true)
          {
            for (int j = 0; j < 66; j++)
            {
              TEMP.voltage.Cell_Vol[j] = Reverse(TEMP.voltage.Cell_Vol[j]);
              if (j < 11)
              {
                Shelf1.frm_Shelf1.Textbox_Shelf1[j].Text = TEMP.voltage.Cell_Vol[j].ToString() + "mV";
              }
              else if (j < 22)
              {
                Shelf1.frm_Shelf1.Textbox_Shelf2[j - 11].Text = (TEMP.voltage.Cell_Vol[j]).ToString() + "mV";
              }
              else if (j < 33)
              {
                Shelf1.frm_Shelf1.Textbox_Shelf3[j - 22].Text = (TEMP.voltage.Cell_Vol[j]).ToString() + "mV";
              }
              else if (j < 44)
              {
                Shelf1.frm_Shelf1.Textbox_Shelf4[j - 33].Text = (TEMP.voltage.Cell_Vol[j]).ToString() + "mV";
              }
              else if (j < 55)
              {
                Shelf1.frm_Shelf1.Textbox_Shelf5[j - 44].Text = (TEMP.voltage.Cell_Vol[j]).ToString() + "mV";
              }
              else if (j < 66)
              {
                Shelf1.frm_Shelf1.Textbox_Shelf6[j - 55].Text = (TEMP.voltage.Cell_Vol[j]).ToString() + "mV";
              }
              else
              {
                //None
              }
            }
          }
          #endregion
          break;
        case 0x0102:
          #region

          if (Shelf2.frm_Shelf2.Textbox_Shelf1[0].Visible == true)
          {
            for (int j = 0; j < 66; j++)
            {
              TEMP.voltage.Cell_Vol[j] = Reverse(TEMP.voltage.Cell_Vol[j]);
              if (j < 11)
              {
                Shelf2.frm_Shelf2.Textbox_Shelf1[j].Text = TEMP.voltage.Cell_Vol[j].ToString() + "mV";
              }
              else if (j < 22)
              {
                Shelf2.frm_Shelf2.Textbox_Shelf2[j - 11].Text = (TEMP.voltage.Cell_Vol[j]).ToString() + "mV";
              }
              else if (j < 33)
              {
                Shelf2.frm_Shelf2.Textbox_Shelf3[j - 22].Text = (TEMP.voltage.Cell_Vol[j]).ToString() + "mV";
              }
              else if (j < 44)
              {
                Shelf2.frm_Shelf2.Textbox_Shelf4[j - 33].Text = (TEMP.voltage.Cell_Vol[j]).ToString() + "mV";
              }
              else if (j < 55)
              {
                Shelf2.frm_Shelf2.Textbox_Shelf5[j - 44].Text = (TEMP.voltage.Cell_Vol[j]).ToString() + "mV";
              }
              else if (j < 66)
              {
                Shelf2.frm_Shelf2.Textbox_Shelf6[j - 55].Text = (TEMP.voltage.Cell_Vol[j]).ToString() + "mV";
              }
              else
              {
                //None
              }
            }
          }
          #endregion
          break;
      }
    }
        private void ShowFactory_setting(int a, int b)
    {
      if (RACK_ID_Number_Sent == 1 && RACK_ID_Number_Sent == 1)
      {
        Factory_setting.factory_Setting.Textbox_Read_Calibration[1].Text = RACK1SHLE1.FactorySetting_Read.BMS_SN.ToString();
        Factory_setting.factory_Setting.Textbox_Read_Calibration[2].Text = RACK1SHLE1.FactorySetting_Read.Cell_Type.ToString();
        Factory_setting.factory_Setting.Textbox_Read_Calibration[3].Text = RACK1SHLE1.FactorySetting_Read.Cell_Parallel_Config.ToString();
        Factory_setting.factory_Setting.Textbox_Read_Calibration[4].Text = RACK1SHLE1.FactorySetting_Read.Shelf_Serial_Config.ToString();
        Factory_setting.factory_Setting.Textbox_Read_Calibration[5].Text = RACK1SHLE1.FactorySetting_Read.Cell_Temperature_Config.ToString();
        Factory_setting.factory_Setting.Textbox_Read_Calibration[6].Text = RACK1SHLE1.FactorySetting_Read.Rated_Capacity.ToString();
        Factory_setting.factory_Setting.Textbox_Read_Calibration[7].Text = RACK1SHLE1.FactorySetting_Read.Rated_Voltage.ToString();
        Factory_setting.factory_Setting.Textbox_Read_Calibration[8].Text = RACK1SHLE1.FactorySetting_Read.BMS_SN.ToString();
        Factory_setting.factory_Setting.Textbox_Read_Calibration[9].Text = RACK1SHLE1.FactorySetting_Read.Project_Code.ToString();
      }
    }
    public  void app_Factory_Setting(SHELF TEMP,int ID)
    {
      TEMP.FactorySetting_Read = (FactorySetting)BytesToDataStruct(Rack1Shelf1FactoryValue, typeof(FactorySetting));
      TEMP.FactorySetting_Read.BMS_SN = Reverse(TEMP.FactorySetting_Read.BMS_SN);
      TEMP.FactorySetting_Read.Project_Code = Reverse(TEMP.FactorySetting_Read.Project_Code);
      TEMP.FactorySetting_Read.Rated_Capacity = Reverse(TEMP.FactorySetting_Read.Rated_Capacity);
      TEMP.FactorySetting_Read.Rated_Voltage = Reverse(TEMP.FactorySetting_Read.Rated_Voltage);
      Factory_setting.factory_Setting.Textbox_Read_Calibration[1].Text = TEMP.FactorySetting_Read.BMS_SN.ToString();
      Factory_setting.factory_Setting.Textbox_Read_Calibration[2].Text = TEMP.FactorySetting_Read.Cell_Type.ToString();
      Factory_setting.factory_Setting.Textbox_Read_Calibration[3].Text = TEMP.FactorySetting_Read.Cell_Parallel_Config.ToString();
      Factory_setting.factory_Setting.Textbox_Read_Calibration[4].Text = TEMP.FactorySetting_Read.Shelf_Serial_Config.ToString();
      Factory_setting.factory_Setting.Textbox_Read_Calibration[5].Text = TEMP.FactorySetting_Read.Cell_Temperature_Config.ToString();
      Factory_setting.factory_Setting.Textbox_Read_Calibration[6].Text = TEMP.FactorySetting_Read.Rated_Capacity.ToString();
      Factory_setting.factory_Setting.Textbox_Read_Calibration[7].Text = TEMP.FactorySetting_Read.Rated_Voltage.ToString();
      Factory_setting.factory_Setting.Textbox_Read_Calibration[8].Text = TEMP.FactorySetting_Read.BMS_SN.ToString();
     // Factory_setting.factory_Setting.Textbox_Read_Calibration[9].Text = TEMP.FactorySetting_Read.Project_Code.ToString();
    }
    public void app_CalibrationValue_Read(SHELF TEMP,int ID)
    {
      TEMP.CalibrationValue_Read = (CalibrationValue)BytesToDataStruct(Rack1Shelf1CalibrationValue, typeof(CalibrationValue));
      TEMP.CalibrationValue_Read.TotalVol_1_Slope = Reverse(TEMP.CalibrationValue_Read.TotalVol_1_Slope);
      TEMP.CalibrationValue_Read.TotalVol_1_Offset = Reverse(TEMP.CalibrationValue_Read.TotalVol_1_Offset);

      TEMP.CalibrationValue_Read.TotalVol_2_Slope = Reverse(TEMP.CalibrationValue_Read.TotalVol_2_Slope);
      TEMP.CalibrationValue_Read.TotalVol_2_Offset = Reverse(TEMP.CalibrationValue_Read.TotalVol_2_Offset);

      TEMP.CalibrationValue_Read.TotalVol_3_Slope = Reverse(TEMP.CalibrationValue_Read.TotalVol_3_Slope);
      TEMP.CalibrationValue_Read.TotalVol_3_Offset = Reverse(TEMP.CalibrationValue_Read.TotalVol_3_Offset);

      TEMP.CalibrationValue_Read.Current_Offset = Reverse(TEMP.CalibrationValue_Read.Current_Offset);
      TEMP.CalibrationValue_Read.Current_Slope = Reverse(TEMP.CalibrationValue_Read.Current_Slope);
      switch (ID)
      {
        case 0x0101:
          //校准参数设置
          Set.Set1.Textbox_Read_Calibration[1].Text = TEMP.CalibrationValue_Read.TotalVol_1_Offset.ToString();
          Set.Set1.Textbox_Read_Calibration[0].Text = TEMP.CalibrationValue_Read.TotalVol_1_Slope.ToString();
          Set.Set1.Textbox_Read_Calibration[3].Text = TEMP.CalibrationValue_Read.TotalVol_2_Offset.ToString();
          Set.Set1.Textbox_Read_Calibration[2].Text = TEMP.CalibrationValue_Read.TotalVol_2_Slope.ToString();
          Set.Set1.Textbox_Read_Calibration[5].Text = TEMP.CalibrationValue_Read.TotalVol_3_Offset.ToString();
          Set.Set1.Textbox_Read_Calibration[4].Text = TEMP.CalibrationValue_Read.TotalVol_3_Slope.ToString();
          Set.Set1.Textbox_Read_Calibration[7].Text = TEMP.CalibrationValue_Read.Current_Offset.ToString();
          Set.Set1.Textbox_Read_Calibration[6].Text = TEMP.CalibrationValue_Read.Current_Slope.ToString();
         // Set.Set1.Textbox_Read_Calibration[8].Text = TEMP.CalibrationValue_Read.SOC.ToString();
          /*
          Set.Set1.Textbox_Read_Calibration[9].Text = TEMP.CalibrationValue_Read.RTC_Y.ToString() + TEMP.CalibrationValue_Read.RTC_Mon.ToString() +
          TEMP.CalibrationValue_Read.RTC_D.ToString() + TEMP.CalibrationValue_Read.RTC_H.ToString() + TEMP.CalibrationValue_Read.RTC_Min.ToString() +
             TEMP.CalibrationValue_Read.RTC_S.ToString();
             */
          break;
      }
  }
    public void app_ThreshholdValue_Read(SHELF TEMP, int ID)
    {
      TEMP.ThreshholdValue_Read = (ThreshholdValue)BytesToDataStruct(Rack1Shelf1ThreshholdValue, typeof(ThreshholdValue));

      TEMP.ThreshholdValue_Read.Cell_OV_Warning_Pre = Reverse(TEMP.ThreshholdValue_Read.Cell_OV_Warning_Pre);
      TEMP.ThreshholdValue_Read.Cell_OV_Warning = Reverse(TEMP.ThreshholdValue_Read.Cell_OV_Warning);
      TEMP.ThreshholdValue_Read.Cell_OV_Fault = Reverse(TEMP.ThreshholdValue_Read.Cell_OV_Fault);

      TEMP.ThreshholdValue_Read.Battery_OV_Warning_Pre = Reverse(TEMP.ThreshholdValue_Read.Battery_OV_Warning_Pre);
      TEMP.ThreshholdValue_Read.Battery_OV_Warning = Reverse(TEMP.ThreshholdValue_Read.Battery_OV_Warning);
      TEMP.ThreshholdValue_Read.Battery_OV_Fault = Reverse(TEMP.ThreshholdValue_Read.Battery_OV_Fault);

      TEMP.ThreshholdValue_Read.Chg_OCur_Warning_Pre = Reverse(TEMP.ThreshholdValue_Read.Chg_OCur_Warning_Pre);
      TEMP.ThreshholdValue_Read.Chg_OCur_Warning = Reverse(TEMP.ThreshholdValue_Read.Chg_OCur_Warning);
      TEMP.ThreshholdValue_Read.Chg_OCur_Fault = Reverse(TEMP.ThreshholdValue_Read.Chg_OCur_Fault);

      TEMP.ThreshholdValue_Read.Chg_Utemp_Warning_Pre = Reverse(TEMP.ThreshholdValue_Read.Chg_Utemp_Warning_Pre);
      TEMP.ThreshholdValue_Read.Chg_Utemp_Warning = Reverse(TEMP.ThreshholdValue_Read.Chg_Utemp_Warning);
      TEMP.ThreshholdValue_Read.Chg_Utemp_Fault = Reverse(TEMP.ThreshholdValue_Read.Chg_Utemp_Fault);

      TEMP.ThreshholdValue_Read.Chg_Otemp_Warning_Pre = Reverse(TEMP.ThreshholdValue_Read.Chg_Otemp_Warning_Pre);
      TEMP.ThreshholdValue_Read.Chg_Otemp_Warning = Reverse(TEMP.ThreshholdValue_Read.Chg_Otemp_Warning);
      TEMP.ThreshholdValue_Read.Chg_Otemp_Fault = Reverse(TEMP.ThreshholdValue_Read.Chg_Otemp_Fault);

      TEMP.ThreshholdValue_Read.Cell_UV_Warning_Pre = Reverse(TEMP.ThreshholdValue_Read.Cell_UV_Warning_Pre);
      TEMP.ThreshholdValue_Read.Cell_UV_Warning = Reverse(TEMP.ThreshholdValue_Read.Cell_UV_Warning);
      TEMP.ThreshholdValue_Read.Cell_UV_Fault = Reverse(TEMP.ThreshholdValue_Read.Cell_UV_Fault);

      TEMP.ThreshholdValue_Read.Battery_UV_Warning_Pre = Reverse(TEMP.ThreshholdValue_Read.Battery_UV_Warning_Pre);
      TEMP.ThreshholdValue_Read.Battery_UV_Warning = Reverse(TEMP.ThreshholdValue_Read.Battery_UV_Warning);
      TEMP.ThreshholdValue_Read.Battery_UV_Fault = Reverse(TEMP.ThreshholdValue_Read.Battery_UV_Fault);

      TEMP.ThreshholdValue_Read.DisChg_OCur_Warning_Pre = Reverse(TEMP.ThreshholdValue_Read.DisChg_OCur_Warning_Pre);
      TEMP.ThreshholdValue_Read.DisChg_OCur_Warning = Reverse(TEMP.ThreshholdValue_Read.DisChg_OCur_Warning);
      TEMP.ThreshholdValue_Read.DisChg_OCur_Fault = Reverse(TEMP.ThreshholdValue_Read.DisChg_OCur_Fault);

      TEMP.ThreshholdValue_Read.DisChg_UTemp_Warning_Pre = Reverse(TEMP.ThreshholdValue_Read.DisChg_UTemp_Warning_Pre);
      TEMP.ThreshholdValue_Read.DisChg_UTemp_Warning = Reverse(TEMP.ThreshholdValue_Read.DisChg_UTemp_Warning);
      TEMP.ThreshholdValue_Read.DisChg_UTemp_Fault = Reverse(TEMP.ThreshholdValue_Read.DisChg_UTemp_Fault);

      TEMP.ThreshholdValue_Read.DisChg_Otemp_Warning_Pre = Reverse(TEMP.ThreshholdValue_Read.DisChg_Otemp_Warning_Pre);
      TEMP.ThreshholdValue_Read.DisChg_Otemp_Warning = Reverse(TEMP.ThreshholdValue_Read.DisChg_Otemp_Warning);
      TEMP.ThreshholdValue_Read.DisChg_Otemp_Fault = Reverse(TEMP.ThreshholdValue_Read.DisChg_Otemp_Fault);

      TEMP.ThreshholdValue_Read.SOC_Low_Warning_Pre = Reverse(TEMP.ThreshholdValue_Read.SOC_Low_Warning_Pre);
      TEMP.ThreshholdValue_Read.SOC_Low_Warning = Reverse(TEMP.ThreshholdValue_Read.SOC_Low_Warning);
      TEMP.ThreshholdValue_Read.SOC_Low_Fault = Reverse(TEMP.ThreshholdValue_Read.SOC_Low_Fault);

      TEMP.ThreshholdValue_Read.V_Diff_Warning_Pre = Reverse(TEMP.ThreshholdValue_Read.V_Diff_Warning_Pre);
      TEMP.ThreshholdValue_Read.V_Diff_Warning = Reverse(TEMP.ThreshholdValue_Read.V_Diff_Warning);
      TEMP.ThreshholdValue_Read.V_Diff_Fault = Reverse(TEMP.ThreshholdValue_Read.V_Diff_Fault);

      TEMP.ThreshholdValue_Read.T_Diff_Warning_Pre = Reverse(TEMP.ThreshholdValue_Read.T_Diff_Warning_Pre);
      TEMP.ThreshholdValue_Read.T_Diff_Warning = Reverse(TEMP.ThreshholdValue_Read.T_Diff_Warning);
      TEMP.ThreshholdValue_Read.T_Diff_Fault = Reverse(TEMP.ThreshholdValue_Read.T_Diff_Fault);
      switch (ID)
      {
        case 0x0101:
          Set.Set1.TextboxThreshold_Read[0].Text = TEMP.ThreshholdValue_Read.Cell_OV_Warning_Pre.ToString();
          Set.Set1.TextboxThreshold_Read[1].Text = TEMP.ThreshholdValue_Read.Cell_OV_Warning.ToString();
          Set.Set1.TextboxThreshold_Read[2].Text = TEMP.ThreshholdValue_Read.Cell_OV_Fault.ToString();

          Set.Set1.TextboxThreshold_Read[3].Text = TEMP.ThreshholdValue_Read.Battery_OV_Warning_Pre.ToString();
          Set.Set1.TextboxThreshold_Read[4].Text = TEMP.ThreshholdValue_Read.Battery_OV_Warning.ToString();
          Set.Set1.TextboxThreshold_Read[5].Text = TEMP.ThreshholdValue_Read.Battery_OV_Fault.ToString();

          Set.Set1.TextboxThreshold_Read[6].Text = TEMP.ThreshholdValue_Read.Chg_OCur_Warning_Pre.ToString();
          Set.Set1.TextboxThreshold_Read[7].Text = TEMP.ThreshholdValue_Read.Chg_OCur_Warning.ToString();
          Set.Set1.TextboxThreshold_Read[8].Text = TEMP.ThreshholdValue_Read.Chg_OCur_Fault.ToString();

          Set.Set1.TextboxThreshold_Read[9].Text = TEMP.ThreshholdValue_Read.Chg_Utemp_Warning_Pre.ToString();
          Set.Set1.TextboxThreshold_Read[10].Text = TEMP.ThreshholdValue_Read.Chg_Utemp_Warning.ToString();
          Set.Set1.TextboxThreshold_Read[11].Text = TEMP.ThreshholdValue_Read.Chg_Utemp_Fault.ToString();

          Set.Set1.TextboxThreshold_Read[12].Text = TEMP.ThreshholdValue_Read.Chg_Otemp_Warning_Pre.ToString();
          Set.Set1.TextboxThreshold_Read[13].Text = TEMP.ThreshholdValue_Read.Chg_Otemp_Warning.ToString();
          Set.Set1.TextboxThreshold_Read[14].Text = TEMP.ThreshholdValue_Read.Chg_Otemp_Fault.ToString();

          Set.Set1.TextboxThreshold_Read[15].Text = TEMP.ThreshholdValue_Read.Cell_UV_Warning_Pre.ToString();
          Set.Set1.TextboxThreshold_Read[16].Text = TEMP.ThreshholdValue_Read.Cell_UV_Warning.ToString();
          Set.Set1.TextboxThreshold_Read[17].Text = TEMP.ThreshholdValue_Read.Cell_UV_Fault.ToString();

          Set.Set1.TextboxThreshold_Read[18].Text = TEMP.ThreshholdValue_Read.Battery_UV_Warning_Pre.ToString();
          Set.Set1.TextboxThreshold_Read[19].Text = TEMP.ThreshholdValue_Read.Battery_UV_Warning.ToString();
          Set.Set1.TextboxThreshold_Read[20].Text = TEMP.ThreshholdValue_Read.Battery_UV_Fault.ToString();

          Set.Set1.TextboxThreshold_Read[21].Text = TEMP.ThreshholdValue_Read.DisChg_OCur_Warning_Pre.ToString();
          Set.Set1.TextboxThreshold_Read[22].Text = TEMP.ThreshholdValue_Read.DisChg_OCur_Warning.ToString();
          Set.Set1.TextboxThreshold_Read[23].Text = TEMP.ThreshholdValue_Read.DisChg_OCur_Fault.ToString();

          Set.Set1.TextboxThreshold_Read[24].Text = TEMP.ThreshholdValue_Read.DisChg_UTemp_Warning_Pre.ToString();
          Set.Set1.TextboxThreshold_Read[25].Text = TEMP.ThreshholdValue_Read.DisChg_UTemp_Warning.ToString();
          Set.Set1.TextboxThreshold_Read[26].Text = TEMP.ThreshholdValue_Read.DisChg_UTemp_Fault.ToString();

          Set.Set1.TextboxThreshold_Read[27].Text = TEMP.ThreshholdValue_Read.DisChg_Otemp_Warning_Pre.ToString();
          Set.Set1.TextboxThreshold_Read[28].Text = TEMP.ThreshholdValue_Read.DisChg_Otemp_Warning.ToString();
          Set.Set1.TextboxThreshold_Read[29].Text = TEMP.ThreshholdValue_Read.DisChg_Otemp_Fault.ToString();

          Set.Set1.TextboxThreshold_Read[30].Text = TEMP.ThreshholdValue_Read.SOC_Low_Warning_Pre.ToString();
          Set.Set1.TextboxThreshold_Read[31].Text = TEMP.ThreshholdValue_Read.SOC_Low_Warning.ToString();
          Set.Set1.TextboxThreshold_Read[32].Text = TEMP.ThreshholdValue_Read.SOC_Low_Fault.ToString();

          Set.Set1.TextboxThreshold_Read[33].Text = TEMP.ThreshholdValue_Read.V_Diff_Warning_Pre.ToString();
          Set.Set1.TextboxThreshold_Read[34].Text = TEMP.ThreshholdValue_Read.V_Diff_Warning.ToString();
          Set.Set1.TextboxThreshold_Read[35].Text = TEMP.ThreshholdValue_Read.V_Diff_Fault.ToString();

          Set.Set1.TextboxThreshold_Read[36].Text = TEMP.ThreshholdValue_Read.T_Diff_Warning_Pre.ToString();
          Set.Set1.TextboxThreshold_Read[37].Text = TEMP.ThreshholdValue_Read.T_Diff_Warning.ToString();
          Set.Set1.TextboxThreshold_Read[38].Text = TEMP.ThreshholdValue_Read.T_Diff_Fault.ToString();
          /*

             */

          break;
      }
    }
    public void SetLightShow(SHELF teme)
    {
      teme.summary = (Summary)BytesToDataStruct(Rack1Shelf1Summary, typeof(Summary));
      teme.summary.Drive_Status = Reverse(teme.summary.Drive_Status);   //驱动状态
                                                                        //LED1灯的状态
      if (ShortGetBit(teme.summary.Drive_Status, 3) == 0)
      {
        Set.Set1.Drive_Lights[0].OnColor = Color.Gray;
      }
      else
      {
        Set.Set1.Drive_Lights[0].OnColor = Color.Green;
      }
      //LED2灯的状态
      if (ShortGetBit(teme.summary.Drive_Status, 4) == 0)
      {
        Set.Set1.Drive_Lights[3].OnColor = Color.Gray;
      }
      else
      {
        Set.Set1.Drive_Lights[3].OnColor = Color.Green;
      }
      //LED3灯的状态
      if (ShortGetBit(teme.summary.Drive_Status, 5) == 0)
      {
        Set.Set1.Drive_Lights[6].OnColor = Color.Gray;
      }
      else
      {
        Set.Set1.Drive_Lights[6].OnColor = Color.Green;
      }
      //LED4灯的状态
      if (ShortGetBit(teme.summary.Drive_Status, 6) == 0)
      {
        Set.Set1.Drive_Lights[9].OnColor = Color.Gray;
      }
      else
      {
        Set.Set1.Drive_Lights[9].OnColor = Color.Green;
      }
      //风机1的状态
      if (ShortGetBit(teme.summary.Drive_Status, 7) == 0)
      {
        Set.Set1.Drive_Lights[1].OnColor = Color.Gray;
      }
      else
      {
        Set.Set1.Drive_Lights[1].OnColor = Color.Green;
      }
      //风机2的状态
      if (ShortGetBit(teme.summary.Drive_Status, 8) == 0)
      {
        Set.Set1.Drive_Lights[4].OnColor = Color.Gray;
      }
      else
      {
        Set.Set1.Drive_Lights[4].OnColor = Color.Green;
      }
      //风机3的状态
      if (ShortGetBit(teme.summary.Drive_Status, 9) == 0)
      {
        Set.Set1.Drive_Lights[7].OnColor = Color.Gray;
      }
      else
      {
        Set.Set1.Drive_Lights[7].OnColor = Color.Green;
      }
      //MPC状态
      if (ShortGetBit(teme.summary.Drive_Status, 0) == 0)
      {
        Set.Set1.Drive_Lights[2].OnColor = Color.Gray;
      }
      else
      {
        Set.Set1.Drive_Lights[2].OnColor = Color.Green;
      }
      //MNC状态
      if (ShortGetBit(teme.summary.Drive_Status, 1) == 0)
      {
        Set.Set1.Drive_Lights[5].OnColor = Color.Gray;
      }
      else
      {
        Set.Set1.Drive_Lights[5].OnColor = Color.Green;
      }
      //PCC 状态
      if (ShortGetBit(teme.summary.Drive_Status, 2) == 0)
      {
        Set.Set1.Drive_Lights[8].OnColor = Color.Gray;
      }
      else
      {
        Set.Set1.Drive_Lights[8].OnColor = Color.Green;
      }

    }
    public void RACKShelf_Summary_decribe(SHELF TEMP, int ID)
    {
      //接收到信息赋值给结构体
      TEMP.summary = (Summary)BytesToDataStruct(Rack1Shelf1Summary, typeof(Summary));

      //大小端转化
      TEMP.summary.TotalVol_1 = Reverse(TEMP.summary.TotalVol_1); 	//总压1
      TEMP.summary.TotalVol_2 = Reverse(TEMP.summary.TotalVol_2); 	//电压2
      TEMP.summary.TotalVol_3 = Reverse(TEMP.summary.TotalVol_3);		//电压3
      TEMP.summary.CurCurrent = Reverse(TEMP.summary.CurCurrent);		//电流
      TEMP.summary.MaxVol = Reverse(TEMP.summary.MaxVol);						//最大电压
      TEMP.summary.MinVol = Reverse(TEMP.summary.MinVol);						//最小电压
      TEMP.summary.Drive_Status = Reverse(TEMP.summary.Drive_Status); 	//驱动状态
      TEMP.summary.warning = Reverse(TEMP.summary.warning);					//warning状态
      TEMP.summary.warning_pre = Reverse(TEMP.summary.warning_pre);	//pre_warning 状态
      TEMP.summary.fault = Reverse(TEMP.summary.fault);							//故障状态
      TEMP.summary.BalanceVol = Reverse(TEMP.summary.BalanceVol);		//均衡电压
      TEMP.summary.MBB1_Balance_Stas = Reverse(TEMP.summary.MBB1_Balance_Stas); //MBB1的状态
      TEMP.summary.MBB2_Balance_Stas = Reverse(TEMP.summary.MBB2_Balance_Stas); //MBB2的状态    
      TEMP.summary.MBB3_Balance_Stas = Reverse(TEMP.summary.MBB3_Balance_Stas); //MBB3的状态
      TEMP.summary.MBB4_Balance_Stas = Reverse(TEMP.summary.MBB4_Balance_Stas); //MBB4的状态
      TEMP.summary.MBB5_Balance_Stas = Reverse(TEMP.summary.MBB5_Balance_Stas); //MBB5的状态
      TEMP.summary.MBB6_Balance_Stas = Reverse(TEMP.summary.MBB6_Balance_Stas); //MBB5的状态
      TEMP.summary.FW_version = Reverse(TEMP.summary.FW_version);
      TEMP.summary.HW_version = Reverse(TEMP.summary.HW_version);
      switch (ID)
      {
        //RACK1Shelf1
        #region
        case 0x0101:
          {
            try
            {
              Shelf1.frm_Shelf1.Textbox_Summary[0].Text = ((float)TEMP.summary.TotalVol_1 / 10).ToString("#0.0") + "V";
              Shelf1.frm_Shelf1.Textbox_Summary[1].Text = ((float)TEMP.summary.TotalVol_2 / 10).ToString("#0.0") + "V";
              Shelf1.frm_Shelf1.Textbox_Summary[2].Text = ((float)TEMP.summary.TotalVol_3 / 10).ToString("#0.0") + "V";
              Shelf1.frm_Shelf1.Textbox_Summary[3].Text = ((float)TEMP.summary.CurCurrent / 10 - 1000).ToString("#0.0") + "A";
              Shelf1.frm_Shelf1.Textbox_Summary[4].Text = TEMP.summary.MaxVol.ToString() + "mV";
              Shelf1.frm_Shelf1.Textbox_Summary[5].Text = TEMP.summary.MaxVolID.ToString();
              Shelf1.frm_Shelf1.Textbox_Summary[6].Text = TEMP.summary.MinVol.ToString() + "mV";
              Shelf1.frm_Shelf1.Textbox_Summary[7].Text = TEMP.summary.MinVolID.ToString();
              Shelf1.frm_Shelf1.Textbox_Summary[8].Text = ((float)TEMP.summary.MaxTemp - 40).ToString() + "℃";
              Shelf1.frm_Shelf1.Textbox_Summary[9].Text = TEMP.summary.MaxTempID.ToString();
              Shelf1.frm_Shelf1.Textbox_Summary[10].Text = ((float)TEMP.summary.MinTemp - 40).ToString() + "℃";
              Shelf1.frm_Shelf1.Textbox_Summary[11].Text = TEMP.summary.MinTempID.ToString();
              Shelf1.frm_Shelf1.Textbox_Summary[12].Text = TEMP.summary.SOC.ToString() + "%";
              Shelf1.frm_Shelf1.Textbox_Summary[13].Text = TEMP.summary.SOH.ToString() + "%";
              Shelf1.frm_Shelf1.Textbox_Summary[14].Text = TEMP.summary.BalanceVol.ToString() + "mV";
              //LED1灯的状态
              if (ShortGetBit(TEMP.summary.Drive_Status, 3) == 0)
              {
                Shelf1.frm_Shelf1.lights[0].OnColor = Color.Gray;
              }
              else
              {
                Shelf1.frm_Shelf1.lights[0].OnColor = Color.Green;
              }
              //LED2灯的状态
              if (ShortGetBit(TEMP.summary.Drive_Status, 4) == 0)
              {
                Shelf1.frm_Shelf1.lights[1].OnColor = Color.Gray;
              }
              else
              {
                Shelf1.frm_Shelf1.lights[1].OnColor = Color.Green;
              }
              //LED3灯的状态
              if (ShortGetBit(TEMP.summary.Drive_Status, 5) == 0)
              {
                Shelf1.frm_Shelf1.lights[2].OnColor = Color.Gray;
              }
              else
              {
                Shelf1.frm_Shelf1.lights[2].OnColor = Color.Green;
              }
              //LED4灯的状态
              if (ShortGetBit(TEMP.summary.Drive_Status, 6) == 0)
              {
                Shelf1.frm_Shelf1.lights[3].OnColor = Color.Gray;
              }
              else
              {
                Shelf1.frm_Shelf1.lights[3].OnColor = Color.Green;
              }
              //风机1的状态
              if (ShortGetBit(TEMP.summary.Drive_Status, 7) == 0)
              {
                Shelf1.frm_Shelf1.lights[4].OnColor = Color.Gray;
              }
              else
              {
                Shelf1.frm_Shelf1.lights[4].OnColor = Color.Green;
              }
              //风机2的状态
              if (ShortGetBit(TEMP.summary.Drive_Status, 8) == 0)
              {
                Shelf1.frm_Shelf1.lights[5].OnColor = Color.Gray;
              }
              else
              {
                Shelf1.frm_Shelf1.lights[5].OnColor = Color.Green;
              }
              //风机3的状态
              if (ShortGetBit(TEMP.summary.Drive_Status, 9) == 0)
              {
                Shelf1.frm_Shelf1.lights[6].OnColor = Color.Gray;
              }
              else
              {
                Shelf1.frm_Shelf1.lights[6].OnColor = Color.Green;
              }
              //风机4的状态
              if (ShortGetBit(TEMP.summary.Drive_Status, 0) == 0)
              {
                Shelf1.frm_Shelf1.lights[7].OnColor = Color.Gray;
              }
              else
              {
                Shelf1.frm_Shelf1.lights[7].OnColor = Color.Green;
              }
              //MPC状态
              if (ShortGetBit(TEMP.summary.Drive_Status, 1) == 0)
              {
                Shelf1.frm_Shelf1.lights[8].OnColor = Color.Gray;
              }
              else
              {
                Shelf1.frm_Shelf1.lights[8].OnColor = Color.Green;
              }
              //MNC状态
              if (ShortGetBit(TEMP.summary.Drive_Status, 2) == 0)
              {
                Shelf1.frm_Shelf1.lights[9].OnColor = Color.Gray;
              }
              else
              {
                Shelf1.frm_Shelf1.lights[9].OnColor = Color.Green;
              }
              //PCC 状态
              if (ShortGetBit(TEMP.summary.Input_Status, 0) == 0)
              {
                Shelf1.frm_Shelf1.lights[10].OnColor = Color.Gray;
              }
              else
              {
                Shelf1.frm_Shelf1.lights[10].OnColor = Color.Green;
              }
              //模块现在状态
              switch (TEMP.summary.Bat_Status)
              {
                case 0x00:
                  Shelf1.frm_Shelf1.Textbox_Summary[15].Text = "Stand By";
                  break;
                case 0x01:
                  Shelf1.frm_Shelf1.Textbox_Summary[15].Text = "Charge";
                  break;
                case 0x02:
                  Shelf1.frm_Shelf1.Textbox_Summary[15].Text = "Discharge";
                  break;
                case 0x03:
                  Shelf1.frm_Shelf1.Textbox_Summary[15].Text = "Precharge In";
                  break;
                case 0x04:
                  Shelf1.frm_Shelf1.Textbox_Summary[15].Text = "Precharge Out";
                  break;
                case 0x05:
                  Shelf1.frm_Shelf1.Textbox_Summary[15].Text = "Full charge";
                  break;
              }

              Shelf1.frm_Shelf1.Textbox_Summary[16].Text = Convert.ToString(TEMP.summary.MBB1_Balance_Stas, 2);
              Shelf1.frm_Shelf1.Textbox_Summary[17].Text = Convert.ToString(TEMP.summary.MBB2_Balance_Stas, 2);
              Shelf1.frm_Shelf1.Textbox_Summary[18].Text = Convert.ToString(TEMP.summary.MBB3_Balance_Stas, 2);
              Shelf1.frm_Shelf1.Textbox_Summary[19].Text = Convert.ToString(TEMP.summary.MBB4_Balance_Stas, 2);
              Shelf1.frm_Shelf1.Textbox_Summary[20].Text = Convert.ToString(TEMP.summary.MBB5_Balance_Stas, 2);
              Shelf1.frm_Shelf1.Textbox_Summary[21].Text = Convert.ToString(TEMP.summary.MBB6_Balance_Stas, 2);
              Shelf1.frm_Shelf1.Textbox_Summary[22].Text = (TEMP.summary.FW_version).ToString();
              Shelf1.frm_Shelf1.Textbox_Summary[23].Text = (TEMP.summary.HW_version).ToString();
              Shelf1.frm_Shelf1.Textbox_Summary[24].Text = TEMP.summary.RTC_Y.ToString() + "/" + TEMP.summary.RTC_MON.ToString() + "/" +
                   TEMP.summary.RTC_D.ToString() + " " + TEMP.summary.RTC_H.ToString() + ":" + TEMP.summary.RTC_MIN.ToString() + ":" +
                   TEMP.summary.RTC_S.ToString();
              //故障页的处理
              for (short i = 0; i < 24; i++)
              {
                if (UintGetBit(TEMP.summary.fault, i) == 1)
                {
                  Shelf1.frm_Shelf1.fault_lights[i].OnColor = Color.Red;
                }
                else if (UintGetBit(TEMP.summary.warning, i) == 1)
                {
                  Shelf1.frm_Shelf1.fault_lights[i].OnColor = Color.Orange;
                }
                else if (UintGetBit(TEMP.summary.warning_pre, i) == 1)
                {
                  Shelf1.frm_Shelf1.fault_lights[i].OnColor = Color.Yellow;
                }
                else
                {
                  Shelf1.frm_Shelf1.fault_lights[i].OnColor = Color.Green;
                }
              }
            }
            catch
            {

            }
          }
          break;
        #endregion
        //RACK2Shelf2
        #region
        case 0x0102:
          {
            Shelf2.frm_Shelf2.Textbox_Summary[0].Text = ((float)TEMP.summary.TotalVol_1 / 10).ToString("#0.0") + "V";
            Shelf2.frm_Shelf2.Textbox_Summary[1].Text = ((float)TEMP.summary.TotalVol_2 / 10).ToString("#0.0") + "V";
            Shelf2.frm_Shelf2.Textbox_Summary[2].Text = ((float)TEMP.summary.TotalVol_3 / 10).ToString("#0.0") + "V";
            Shelf2.frm_Shelf2.Textbox_Summary[3].Text = ((float)TEMP.summary.CurCurrent / 10 - 1000).ToString("#0.0") + "A";
            Shelf2.frm_Shelf2.Textbox_Summary[4].Text = TEMP.summary.MaxVol.ToString() + "mV";
            Shelf2.frm_Shelf2.Textbox_Summary[5].Text = TEMP.summary.MaxVolID.ToString();
            Shelf2.frm_Shelf2.Textbox_Summary[6].Text = TEMP.summary.MinVol.ToString() + "mV";
            Shelf2.frm_Shelf2.Textbox_Summary[7].Text = TEMP.summary.MinVolID.ToString();
            Shelf2.frm_Shelf2.Textbox_Summary[8].Text = ((float)TEMP.summary.MaxTemp - 40).ToString() + "℃";
            Shelf2.frm_Shelf2.Textbox_Summary[9].Text = TEMP.summary.MaxTempID.ToString();
            Shelf2.frm_Shelf2.Textbox_Summary[10].Text = ((float)TEMP.summary.MinTemp - 40).ToString() + "℃";
            Shelf2.frm_Shelf2.Textbox_Summary[11].Text = TEMP.summary.MinTempID.ToString();
            Shelf2.frm_Shelf2.Textbox_Summary[12].Text = TEMP.summary.SOC.ToString() + "%";
            Shelf2.frm_Shelf2.Textbox_Summary[13].Text = TEMP.summary.SOH.ToString() + "%";
            Shelf2.frm_Shelf2.Textbox_Summary[14].Text = TEMP.summary.BalanceVol.ToString() + "mV";
            //LED1灯的状态
            if (ShortGetBit(TEMP.summary.Drive_Status, 3) == 0)
            {
              Shelf2.frm_Shelf2.lights[0].OnColor = Color.Gray;
            }
            else
            {
              Shelf2.frm_Shelf2.lights[0].OnColor = Color.Green;
            }
            //LED2灯的状态
            if (ShortGetBit(TEMP.summary.Drive_Status, 4) == 0)
            {
              Shelf2.frm_Shelf2.lights[1].OnColor = Color.Gray;
            }
            else
            {
              Shelf2.frm_Shelf2.lights[1].OnColor = Color.Green;
            }
            //LED3灯的状态
            if (ShortGetBit(TEMP.summary.Drive_Status, 5) == 0)
            {
              Shelf2.frm_Shelf2.lights[2].OnColor = Color.Gray;
            }
            else
            {
              Shelf2.frm_Shelf2.lights[2].OnColor = Color.Green;
            }
            //LED4灯的状态
            if (ShortGetBit(TEMP.summary.Drive_Status, 6) == 0)
            {
              Shelf2.frm_Shelf2.lights[3].OnColor = Color.Gray;
            }
            else
            {
              Shelf2.frm_Shelf2.lights[3].OnColor = Color.Green;
            }
            //风机1的状态
            if (ShortGetBit(TEMP.summary.Drive_Status, 7) == 0)
            {
              Shelf2.frm_Shelf2.lights[4].OnColor = Color.Gray;
            }
            else
            {
              Shelf2.frm_Shelf2.lights[4].OnColor = Color.Green;
            }
            //风机2的状态
            if (ShortGetBit(TEMP.summary.Drive_Status, 8) == 0)
            {
              Shelf2.frm_Shelf2.lights[4].OnColor = Color.Gray;
            }
            else
            {
              Shelf2.frm_Shelf2.lights[4].OnColor = Color.Green;
            }
            //风机3的状态
            if (ShortGetBit(TEMP.summary.Drive_Status, 9) == 0)
            {
              Shelf2.frm_Shelf2.lights[5].OnColor = Color.Gray;
            }
            else
            {
              Shelf2.frm_Shelf2.lights[5].OnColor = Color.Green;
            }
            //风机4的状态
            if (ShortGetBit(TEMP.summary.Drive_Status, 0) == 0)
            {
              Shelf2.frm_Shelf2.lights[6].OnColor = Color.Gray;
            }
            else
            {
              Shelf2.frm_Shelf2.lights[6].OnColor = Color.Green;
            }
            //MPC状态
            if (ShortGetBit(TEMP.summary.Drive_Status, 1) == 0)
            {
              Shelf2.frm_Shelf2.lights[7].OnColor = Color.Gray;
            }
            else
            {
              Shelf2.frm_Shelf2.lights[7].OnColor = Color.Green;
            }
            //MNC状态
            if (ShortGetBit(TEMP.summary.Drive_Status, 2) == 0)
            {
              Shelf2.frm_Shelf2.lights[8].OnColor = Color.Gray;
            }
            else
            {
              Shelf2.frm_Shelf2.lights[8].OnColor = Color.Green;
            }
            //PCC 状态
            if (ShortGetBit(TEMP.summary.Input_Status, 0) == 0)
            {
              Shelf2.frm_Shelf2.lights[9].OnColor = Color.Gray;
            }
            else
            {
              Shelf2.frm_Shelf2.lights[9].OnColor = Color.Green;
            }
            //模块现在状态
            switch (TEMP.summary.Bat_Status)
            {
              case 0x00:
                Shelf2.frm_Shelf2.Textbox_Summary[15].Text = "Stand By";
                break;
              case 0x01:
                Shelf2.frm_Shelf2.Textbox_Summary[15].Text = "Charge";
                break;
              case 0x02:
                Shelf2.frm_Shelf2.Textbox_Summary[15].Text = "Discharge";
                break;
              case 0x03:
                Shelf2.frm_Shelf2.Textbox_Summary[15].Text = "Precharge In";
                break;
              case 0x04:
                Shelf2.frm_Shelf2.Textbox_Summary[15].Text = "Precharge Out";
                break;
              case 0x05:
                Shelf2.frm_Shelf2.Textbox_Summary[15].Text = "Full charge";
                break;
            }

            Shelf2.frm_Shelf2.Textbox_Summary[16].Text = Convert.ToString(TEMP.summary.MBB1_Balance_Stas, 2);
            Shelf2.frm_Shelf2.Textbox_Summary[17].Text = Convert.ToString(TEMP.summary.MBB2_Balance_Stas, 2);
            Shelf1.frm_Shelf1.Textbox_Summary[18].Text = Convert.ToString(TEMP.summary.MBB3_Balance_Stas, 2);
            Shelf1.frm_Shelf1.Textbox_Summary[19].Text = Convert.ToString(TEMP.summary.MBB4_Balance_Stas, 2);
            Shelf1.frm_Shelf1.Textbox_Summary[20].Text = Convert.ToString(TEMP.summary.MBB5_Balance_Stas, 2);
            Shelf1.frm_Shelf1.Textbox_Summary[21].Text = Convert.ToString(TEMP.summary.MBB6_Balance_Stas, 2);
            Shelf1.frm_Shelf1.Textbox_Summary[22].Text = (TEMP.summary.FW_version).ToString();
            Shelf1.frm_Shelf1.Textbox_Summary[23].Text = (TEMP.summary.HW_version).ToString();
            Shelf1.frm_Shelf1.Textbox_Summary[24].Text = TEMP.summary.RTC_Y.ToString() + "." + TEMP.summary.RTC_MON.ToString() + "." + TEMP.summary.RTC_D.ToString() + "." + TEMP.summary.RTC_H.ToString() + ":" + TEMP.summary.RTC_MIN.ToString() + ":" +
                 TEMP.summary.RTC_S.ToString();
            //故障页的处理
            for (short i = 0; i < 24; i++)
            {
              if (UintGetBit(TEMP.summary.warning_pre, i) == 1)
              {
                Shelf1.frm_Shelf1.fault_lights[i].OnColor = Color.Orange;
              }
              else if (UintGetBit(TEMP.summary.warning, i) == 1)
              {
                Shelf1.frm_Shelf1.fault_lights[i].OnColor = Color.Yellow;
              }
              else if (UintGetBit(TEMP.summary.fault, i) == 1)
              {
                Shelf1.frm_Shelf1.fault_lights[i].OnColor = Color.Red;
              }
              else
              {
                Shelf1.frm_Shelf1.fault_lights[i].OnColor = Color.Green;
              }
            }
          }
          break;
          #endregion
      }
    }
    private void RACKShelf_Cell_Temp_decribe(SHELF TEMP, int ID)
    {
      TEMP.moudle_Temp = (Moudle_Temp)BytesToDataStruct(Rack1Shelf1Temperature, typeof(Moudle_Temp));
      #region
      switch (ID)
      {
        //RACK1Shlef1
        case 0x0101:
          {
            if (Shelf1.frm_Shelf1.Visible == true)
              for (int i = 0; i < 7; i++)
              {
                if (i < 4)
                {
                  Shelf1.frm_Shelf1.Textbox_Shelf1[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i]) - 40).ToString() + "°C";
                  Shelf1.frm_Shelf1.Textbox_Shelf2[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 4]) - 40).ToString() + "°C";
                  Shelf1.frm_Shelf1.Textbox_Shelf3[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 8]) - 40).ToString() + "°C";
                  Shelf1.frm_Shelf1.Textbox_Shelf4[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 12]) - 40).ToString() + "°C";
                  Shelf1.frm_Shelf1.Textbox_Shelf5[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 16]) - 40).ToString() + "°C";
                  Shelf1.frm_Shelf1.Textbox_Shelf6[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 20]) - 40).ToString() + "°C";
                }

                else if (i < 5)
                {
                  Shelf1.frm_Shelf1.Textbox_Shelf1[i + 13].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 20]) - 40).ToString() + "°C";
                  Shelf1.frm_Shelf1.Textbox_Shelf2[i + 13].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 21]) - 40).ToString() + "°C";
                  Shelf1.frm_Shelf1.Textbox_Shelf3[i + 13].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 22]) - 40).ToString() + "°C";
                  Shelf1.frm_Shelf1.Textbox_Shelf4[i + 13].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 23]) - 40).ToString() + "°C";
                  Shelf1.frm_Shelf1.Textbox_Shelf5[i + 13].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 24]) - 40).ToString() + "°C";
                  Shelf1.frm_Shelf1.Textbox_Shelf6[i + 13].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 25]) - 40).ToString() + "°C";


                }
                else if (i < 7)
                {
                  Shelf1.frm_Shelf1.Textbox_Shelf1[i + 10].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 25]) - 40).ToString() + "°C";
                  Shelf1.frm_Shelf1.Textbox_Shelf2[i + 10].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 27]) - 40).ToString() + "°C";
                  Shelf1.frm_Shelf1.Textbox_Shelf3[i + 10].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 29]) - 40).ToString() + "°C";
                  Shelf1.frm_Shelf1.Textbox_Shelf4[i + 10].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 31]) - 40).ToString() + "°C";
                  Shelf1.frm_Shelf1.Textbox_Shelf5[i + 10].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 33]) - 40).ToString() + "°C";
                  Shelf1.frm_Shelf1.Textbox_Shelf6[i + 10].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 35]) - 40).ToString() + "°C";
                }
                else
                {

                }
              }
          }
          break;

        case 0x0102:
          {
            if (Shelf2.frm_Shelf2.Visible == true)
              for (int i = 0; i < 7; i++)
              {
                if (i < 4)
                {
                  Shelf2.frm_Shelf2.Textbox_Shelf1[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i]) - 40).ToString() + "°C";
                  Shelf2.frm_Shelf2.Textbox_Shelf2[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 4]) - 40).ToString() + "°C";
                  Shelf2.frm_Shelf2.Textbox_Shelf3[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 8]) - 40).ToString() + "°C";
                  Shelf2.frm_Shelf2.Textbox_Shelf4[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 12]) - 40).ToString() + "°C";
                  Shelf2.frm_Shelf2.Textbox_Shelf5[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 16]) - 40).ToString() + "°C";
                  Shelf2.frm_Shelf2.Textbox_Shelf6[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 20]) - 40).ToString() + "°C";
                }

                else if (i < 5)
                {
                  Shelf2.frm_Shelf2.Textbox_Shelf1[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 20]) - 40).ToString() + "°C";
                  Shelf2.frm_Shelf2.Textbox_Shelf2[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 21]) - 40).ToString() + "°C";
                  Shelf2.frm_Shelf2.Textbox_Shelf3[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 22]) - 40).ToString() + "°C";
                  Shelf2.frm_Shelf2.Textbox_Shelf4[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 23]) - 40).ToString() + "°C";
                  Shelf2.frm_Shelf2.Textbox_Shelf5[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 24]) - 40).ToString() + "°C";
                  Shelf2.frm_Shelf2.Textbox_Shelf6[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 25]) - 40).ToString() + "°C";

                }
                else if (i < 7)
                {
                  Shelf2.frm_Shelf2.Textbox_Shelf1[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 25]) - 40).ToString() + "°C";
                  Shelf2.frm_Shelf2.Textbox_Shelf2[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 27]) - 40).ToString() + "°C";
                  Shelf2.frm_Shelf2.Textbox_Shelf3[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 29]) - 40).ToString() + "°C";
                  Shelf2.frm_Shelf2.Textbox_Shelf4[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 31]) - 40).ToString() + "°C";
                  Shelf2.frm_Shelf2.Textbox_Shelf5[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 33]) - 40).ToString() + "°C";
                  Shelf1.frm_Shelf1.Textbox_Shelf6[i + 11].Text = ((TEMP.moudle_Temp.Cell_Temp[i + 35]) - 40).ToString() + "°C";
                }
                else
                {

                }
              }
          }
          break;
      }
      #endregion
    }
    #endregion
    /************************************************************************		
    *name:  所有的按键操作
    *************************************************************************/
    #region


    private void Butten_SerialPort_Click_1(object sender, EventArgs e)
    {
      //OpenOrCloseSubmenu(Panel_ChildSerialPort);
      panel_RS485.Visible = true;
    }

    private void button_FaltList_Click(object sender, EventArgs e)
    {
      //OpenChildFrom(new Fault());
    }

    private void button_RACK1_Click(object sender, EventArgs e)
    {
      OpenOrCloseSubmenu(panel2_RACK1);
    }

    private void uiSymbolButton2_RACK2_Click(object sender, EventArgs e)
    {
      OpenOrCloseSubmenu(panel3_RACK2);
    }
    private void Button4_RACK3_Click(object sender, EventArgs e)
    {
      OpenOrCloseSubmenu(panel4_RACK3);
    }
    private void Button5_DATA_OUTPUT_Click(object sender, EventArgs e)
    {

    }

    private void Button6_Download_Click(object sender, EventArgs e)
    {
      OpenChildFrom(new Form_DownLoad());
    }
    private void button_Connect_Click(object sender, EventArgs e)
    {
      Button6_Download.Enabled = true;
      try
      {
        //将可能产生异常的代码放置在try块中
        //根据当前串口属性来判断是否打开
        if (serialPort1.IsOpen)
        {
          //串口已经打开的状态
          serialPort1.Close();//关闭串口
          button_Connect.Text = "ON";
          uiLight6.State = Sunny.UI.UILightState.Off;
          button_Connect.BackColor = Color.ForestGreen;
          comboBox_BaudRate.Enabled = true;
          comboBox_CheckDigit.Enabled = true;
          comboBox_DataBits.Enabled = true;
          comboBox_StopBit.Enabled = true;
          comboBox_SerialNumber.Enabled = true;
          //textBox_Receive.Text = "";//清空接受区
        }
        else
        {
          button_Connect.Text = "OFF";
          uiLight6.State = Sunny.UI.UILightState.On;
          //串口处于关闭状态，则设置好串口属性后打开
          comboBox_BaudRate.Enabled = false;
          comboBox_CheckDigit.Enabled = false;
          comboBox_DataBits.Enabled = false;
          comboBox_StopBit.Enabled = false;
          comboBox_SerialNumber.Enabled = false;
          serialPort1.PortName = comboBox_SerialNumber.Text;  //串口号
          serialPort1.BaudRate = Convert.ToInt32(comboBox_BaudRate.Text);  //波特率
          serialPort1.DataBits = Convert.ToInt16(comboBox_DataBits.Text); //数据位
                                                                          //校验位选择
          if (comboBox_CheckDigit.Text.Equals("NONE"))
          {
            serialPort1.Parity = System.IO.Ports.Parity.None;
          }
          if (comboBox_CheckDigit.Text.Equals("ODD"))
          {
            serialPort1.Parity = System.IO.Ports.Parity.Odd;
          }
          if (comboBox_CheckDigit.Text.Equals("EVEN"))
          {
            serialPort1.Parity = System.IO.Ports.Parity.Even;
          }
          if (comboBox_CheckDigit.Text.Equals("MARK"))
          {
            serialPort1.Parity = System.IO.Ports.Parity.Mark;
          }
          if (comboBox_CheckDigit.Text.Equals("SPACE"))
          {
            serialPort1.Parity = System.IO.Ports.Parity.Space;
          }
          //停止位选择
          if (comboBox_StopBit.Text.Equals("1"))
          {
            serialPort1.StopBits = System.IO.Ports.StopBits.One;
          }
          if (comboBox_StopBit.Text.Equals("1.5"))
          {
            serialPort1.StopBits = System.IO.Ports.StopBits.OnePointFive;
          }
          if (comboBox_StopBit.Text.Equals("2"))
          {
            serialPort1.StopBits = System.IO.Ports.StopBits.Two;
          }
          //可以打开串口
          serialPort1.Open();
          button_Connect.BackColor = Color.Firebrick;
        }
      }
      catch (Exception ex)
      {
        //捕获可能发生的异常并进行处理
        //捕获到异常，创建一个新的对象，之前的不可以再用
        serialPort1 = new System.IO.Ports.SerialPort();
        comboBox_SerialNumber.Items.Clear();
        comboBox_SerialNumber.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
        //响铃并显示异常给用户
        System.Media.SystemSounds.Beep.Play();
        button_Connect.Text = "OFF";
        uiLight6.State = Sunny.UI.UILightState.Off;
        button_Connect.BackColor = Color.ForestGreen;
        MessageBox.Show(ex.Message);
        comboBox_BaudRate.Enabled = true;
        comboBox_CheckDigit.Enabled = true;
        comboBox_DataBits.Enabled = true;
        comboBox_StopBit.Enabled = true;
        comboBox_SerialNumber.Enabled = true;
      }
    }

    private void button9_Click(object sender, EventArgs e)
    {

    }

    private void button8_Click(object sender, EventArgs e)
    {
      textBox1.Visible = false;
      panel1.Visible = false;
      panel_main.Text = "RACK1  -  SHELF1";
      OpenChildFrom(new Shelf1());
      RACK_ID_Number_Sent = 0x01;
      Shelf_ID_Number_Sent = 0x01;
      Send_Query_Command(RACK_ID_Number_Sent, Shelf_ID_Number_Sent);
    }

    private void button7_Click(object sender, EventArgs e)
    {
      RACK_ID_Number_Sent = 0x01;
      Shelf_ID_Number_Sent = 0x02;
      OpenChildFrom(new Shelf2());
      panel_main.Text = "RACK1  -  SHELF2";
      Send_Query_Command(RACK_ID_Number_Sent, Shelf_ID_Number_Sent);
    }
    #endregion

    private void button6_Click(object sender, EventArgs e)
    {
      OpenChildFrom(new Shelf3());
      panel_main.Text = "RACK1  -  SHELF3";
    }

    private void uiMarkLabel1_Click(object sender, EventArgs e)
    {

    }

    private void uiMarkLabel2_Click(object sender, EventArgs e)
    {

    }
    private void Send_Query_Command(byte RACK, byte Shelf)
    {
      Serial_Data_Transmission(Null, 8, RACK, Shelf, 0x01);  //读取摘要
      Serial_Data_Transmission(Null, 8, RACK, Shelf, 0x02);  //读取温度
      Serial_Data_Transmission(Null, 8, RACK, Shelf, 0x03);	//读取温度
    }
    private void button_Connect_Click_1(object sender, EventArgs e)
    {
      Button6_Download.Enabled = true;
      try
      {
        //将可能产生异常的代码放置在try块中
        //根据当前串口属性来判断是否打开
        if (serialPort1.IsOpen)
        {
          //串口已经打开的状态
          serialPort1.Close();//关闭串口
          button_Connect.Text = "Connect";
          uiLight6.State = Sunny.UI.UILightState.Off;
          button_Connect.BackColor = Color.ForestGreen;
          comboBox_BaudRate.Enabled = true;
          comboBox_CheckDigit.Enabled = true;
          comboBox_DataBits.Enabled = true;
          comboBox_StopBit.Enabled = true;
          comboBox_SerialNumber.Enabled = true;
          //textBox_Receive.Text = "";//清空接受区
        }
        else
        {
          button_Connect.Text = "DisConnect";
          uiLight6.State = Sunny.UI.UILightState.On;
          //串口处于关闭状态，则设置好串口属性后打开
          comboBox_BaudRate.Enabled = false;
          comboBox_CheckDigit.Enabled = false;
          comboBox_DataBits.Enabled = false;
          comboBox_StopBit.Enabled = false;
          comboBox_SerialNumber.Enabled = false;
          serialPort1.PortName = comboBox_SerialNumber.Text;  //串口号
          serialPort1.BaudRate = Convert.ToInt32(comboBox_BaudRate.Text);  //波特率
          serialPort1.DataBits = Convert.ToInt16(comboBox_DataBits.Text); //数据位
                                                                          //校验位选择
          if (comboBox_CheckDigit.Text.Equals("NONE"))
          {
            serialPort1.Parity = System.IO.Ports.Parity.None;
          }
          if (comboBox_CheckDigit.Text.Equals("ODD"))
          {
            serialPort1.Parity = System.IO.Ports.Parity.Odd;
          }
          if (comboBox_CheckDigit.Text.Equals("EVEN"))
          {
            serialPort1.Parity = System.IO.Ports.Parity.Even;
          }
          if (comboBox_CheckDigit.Text.Equals("MARK"))
          {
            serialPort1.Parity = System.IO.Ports.Parity.Mark;
          }
          if (comboBox_CheckDigit.Text.Equals("SPACE"))
          {
            serialPort1.Parity = System.IO.Ports.Parity.Space;
          }
          //停止位选择
          if (comboBox_StopBit.Text.Equals("1"))
          {
            serialPort1.StopBits = System.IO.Ports.StopBits.One;
          }
          if (comboBox_StopBit.Text.Equals("1.5"))
          {
            serialPort1.StopBits = System.IO.Ports.StopBits.OnePointFive;
          }
          if (comboBox_StopBit.Text.Equals("2"))
          {
            serialPort1.StopBits = System.IO.Ports.StopBits.Two;
          }
          //可以打开串口
          serialPort1.Open();
          button_Connect.BackColor = Color.Firebrick;
        }
      }
      catch (Exception ex)
      {
        //响铃并显示异常给用户
        System.Media.SystemSounds.Beep.Play();
        button_Connect.Text = "DisConnect";
        uiLight6.State = Sunny.UI.UILightState.Off;
        button_Connect.BackColor = Color.ForestGreen;
        MessageBox.Show(ex.Message);
        comboBox_BaudRate.Enabled = true;
        comboBox_CheckDigit.Enabled = true;
        comboBox_DataBits.Enabled = true;
        comboBox_StopBit.Enabled = true;
        comboBox_SerialNumber.Enabled = true;
      }
    }
    private void uiSymbolButton1_Click(object sender, EventArgs e)
    {
      Factory_setting f = new Factory_setting();
      f.ShowDialog();
    }
    private void SetRepleFunction(byte temp)
    {
      switch (temp)

      {
        case 0x01:
          MessageBox.Show("Cell_OV_Warning_Pre:  Success!");
          break;
        case 0x02:
          MessageBox.Show("Cell_OV_Warning:   Success!");
          break;
        case 0x03:
          MessageBox.Show("Cell_OV_Fault:   Success!");
          break;
        case 0x04:
          MessageBox.Show("Battery_OV_Warning_Pre:   Success!");
          break;
        case 0x05:
          MessageBox.Show("Battery_OV_Warning:   Success!");
          break;
        case 0x06:
          MessageBox.Show("Battery_OV_Fault:   Success!");
          break;
        case 0x07:
          MessageBox.Show("Chg_OCur_Warning_Pre:   Success!");
          break;
        case 0x08:
          MessageBox.Show("Chg_OCur_Warning:   Success!");
          break;
        case 0x09:
          MessageBox.Show("Chg_OCur_Fault:   Success!");
          break;
        case 0x0A:
          MessageBox.Show("Chg_Utemp_Warning_Pre:   Success!");
          break;
        case 0x0B:
          MessageBox.Show("Chg_Utemp_Warning:   Success!");
          break;
        case 0x0C:
          MessageBox.Show("Chg_Utemp_Fault:   Success!");
          break;
        case 0x0D:
          MessageBox.Show("Chg_Otemp_Warning_Pre:   Success!");
          break;
        case 0x0E:
          MessageBox.Show("Chg_Otemp_Warning:   Success!");
          break;
        case 0x0F:
          MessageBox.Show("Chg_Otemp_Fault:   Success!");
          break;
        case 0x10:
          MessageBox.Show("Cell_UV_Warning_Pre:   Success!");
          break;
        case 0x11:
          MessageBox.Show("Cell_UV_Warning:   Success!");
          break;
        case 0x12:
          MessageBox.Show("Cell_UV_Fault:   Success!");
          break;
        case 0x13:
          MessageBox.Show("Battery_UV_Warning_Pre:   Success!");
          break;
        case 0x14:
          MessageBox.Show("Battery_UV_Warning:   Success!");
          break;
        case 0x15:
          MessageBox.Show("Battery_UV_Fault:   Success!");
          break;
        case 0x16:
          MessageBox.Show("DisChg_OCur_Warning_Pre:   Success!");
          break;
        case 0x17:
          MessageBox.Show("DisChg_OCur_Warning:   Success!");
          break;
        case 0x18:
          MessageBox.Show("DisChg_OCur_Fault:   Success!");
          break;
        case 0x19:
          MessageBox.Show("DisChg_UTemp_Warning_Pre:   Success!");
          break;
        case 0x1A:
          MessageBox.Show("DisChg_UTemp_Warning:   Success!");
          break;
        case 0x1B:
          MessageBox.Show("DisChg_UTemp_Fault:   Success!");
          break;
        case 0x1C:
          MessageBox.Show("DisChg_Otemp_Warning_Pre:   Success!");
          break;
        case 0x1D:
          MessageBox.Show("DisChg_Otemp_Warning:   Success!");
          break;
        case 0x1E:
          MessageBox.Show("DisChg_Otemp_Fault:   Success!");
          break;
        case 0x1F:
          MessageBox.Show("SOC_Low_Warning_Pre:   Success!");
          break;
        case 0x20:
          MessageBox.Show("SOC_Low_Warning:   Success!");
          break;
        case 0x21:
          MessageBox.Show("SOC_Low_Fault:   Success!");
          break;
        case 0x22:
          MessageBox.Show("V_Diff_Warning_Pre:   Success!");
          break;
        case 0x23:
          MessageBox.Show("V_Diff_Warning:   Success!");
          break;
        case 0x24:
          MessageBox.Show("V_Diff_Fault:   Success!");
          break;
        case 0x25:
          MessageBox.Show("T_Diff_Warning_Pre:   Success!");
          break;
        case 0x26:
          MessageBox.Show("T_Diff_Warning:   Success!");
          break;
        case 0x27:
          MessageBox.Show("T_Diff_Fault:   Success!");
          break;

        case 0xA0:
          MessageBox.Show("TotalVol_1_Slope:   Success!");
          break;
        case 0xA1:
          MessageBox.Show("TotalVol_1_Offset:   Success!");
          break;
        case 0xA2:
          MessageBox.Show("TotalVol_2_Slope:   Success!");
          break;
        case 0xA3:
          MessageBox.Show("TotalVol_2_Offset:   Success!");
          break;
        case 0xA4:
          MessageBox.Show("TotalVol_3_Slope:   Success!");
          break;
        case 0xA5:
          MessageBox.Show("TotalVol_3_Offset:   Success!");
          break;
        case 0xA6:
          MessageBox.Show("Current_Slope:   Success!");
          break;
        case 0xA7:
          MessageBox.Show("Current_Offset:   Success!");
          break;
        case 0xA8:
          MessageBox.Show("SOC_Set:   Success!");
          break;

        case 0xA9:
          MessageBox.Show("RTC_Set:   Success!");
          break;
        case 0xE0:
          MessageBox.Show("Cell_Type:   Success!");
          break;
        case 0xE1:
          MessageBox.Show("Cell_Parallel_Config:   Success!");
          break;
        case 0xE2:
          MessageBox.Show("Shelf_Serial_Config:   Success!");
          break;
        case 0xE3:
          MessageBox.Show("Cell_Temperature_Config:   Success!");
          break;
        case 0xE4:
          MessageBox.Show("Rated_Capacity:   Success!");
          break;
        case 0xE5:
          MessageBox.Show("Rated_Voltage:   Success!");
          break;
        case 0xE6:
          MessageBox.Show("BMS_SN:   Success!");
          break;
        case 0xE7:
          MessageBox.Show("Project_Code:   Success!");
          break;
        case 0xF0:
          MessageBox.Show("Rack_ID:   Success!");
          break;

      }
    }
    /*
      阈值设置事件发生调用
    */

    public void ThreshholdValue_Set_Click()
    {
      byte[] ThreshholdValue = new byte[8];
      ThreshholdValue[1] = CMD_ThreshholdValue;
      //读取阈值参数
      Serial_Data_Transmission(ThreshholdValue, 8, Main.frm1.RACK_ID_Number_Sent, Main.frm1.Shelf_ID_Number_Sent, 0x11);
      Serial_Data_Transmission(ThreshholdValue, 8, Main.frm1.RACK_ID_Number_Sent, Main.frm1.Shelf_ID_Number_Sent, 0x11);
      //因为读设置有的值是通过summary传给上位机的
      Serial_Data_Transmission(ThreshholdValue, 8, Main.frm1.RACK_ID_Number_Sent, Main.frm1.Shelf_ID_Number_Sent, 0x03);
    }
    /*
     * 校准设置事件发生时调用
     */
    public void Read_CalibrationValue()
    {
      byte[] CalibrationValue = new byte[8];
      CalibrationValue[1] = CMD_CalibrationValue;
      Serial_Data_Transmission(CalibrationValue, 8, Main.frm1.RACK_ID_Number_Sent, Main.frm1.Shelf_ID_Number_Sent, 0x11);
      Thread.Sleep(250);//休眠时间
      Serial_Data_Transmission(CalibrationValue, 8, Main.frm1.RACK_ID_Number_Sent, Main.frm1.Shelf_ID_Number_Sent, 0x11);
      Serial_Data_Transmission(CalibrationValue, 8, Main.frm1.RACK_ID_Number_Sent, Main.frm1.Shelf_ID_Number_Sent, 0x03);
    }
    public void Read_FactoryVauleClick()
    {
      byte[] CalibrationValue = new byte[8];
      CalibrationValue[1] = CMD_FactoryVaule;
      Serial_Data_Transmission(CalibrationValue, 8, Main.frm1.RACK_ID_Number_Sent, Main.frm1.Shelf_ID_Number_Sent, 0x11);
      Thread.Sleep(250);//休眠时间
      Serial_Data_Transmission(CalibrationValue, 8, Main.frm1.RACK_ID_Number_Sent, Main.frm1.Shelf_ID_Number_Sent, 0x11);
    }

    public void Poll_Query(object source, System.Timers.ElapsedEventArgs e)
    {
      try
      {
        currentTime = System.DateTime.Now;
      }
      catch
      {

      }
      string[] a = System.IO.Ports.SerialPort.GetPortNames();
      if (SerialNuber_data.Length != a.Length)
      {
        SerialNuber_data = System.IO.Ports.SerialPort.GetPortNames();
        comboBox_SerialNumber.Items.Clear();
        comboBox_SerialNumber.Items.Clear();
        comboBox_SerialNumber.Items.AddRange(SerialNuber_data);
      }

      Timer_Count++;
      if (Timer_Count == 16)
      {
        Timer_Count = 1;
      }
      //chaxunzhiling(Timer_Count);
      Rotation_query(Timer_Count);
    }

    //单shelf
    public void Rotation_query(byte i)
    {
      if( i == 3)
      {
        Serial_Data_Transmission(Null, 8, 0x01, 0x01, 0x03);  //温度
      }
      else if(i == 6)
      {
        Serial_Data_Transmission(Null, 8, 0x01, 0x01, 0x02);  //读取电压
      }
      else if (i == 9){
        Serial_Data_Transmission(Null, 8, 0x01, 0x01, 0x01);  //读取摘要
      }
      else
      {
        
      }
    }
    public void chaxunzhiling(byte i)
    {
      if (i < 8)
      {
        Serial_Data_Transmission(Null, 8, 0x01, i, 0x03);  //温度
        Serial_Data_Transmission(Null, 8, 0x01, i, 0x02);  //读取电压
        Serial_Data_Transmission(Null, 8, 0x01, i, 0x01);  //读取摘要
      }
      else if (i < 24)
      {
        Serial_Data_Transmission(Null, 8, 0x02, (byte)(i - 0x08), 0x01);  //读取摘要
        Serial_Data_Transmission(Null, 8, 0x02, (byte)(i - 0x08), 0x02);  //读取摘要
        Serial_Data_Transmission(Null, 8, 0x02, (byte)(i - 0x08), 0x03);  //读取摘要
      }
      else if (i < 40)
      {
        Serial_Data_Transmission(Null, 8, 0x03, (byte)(i - 0x18), 0x01);  //读取摘要
        Serial_Data_Transmission(Null, 8, 0x03, (byte)(i - 0x18), 0x02);  //读取摘要
        Serial_Data_Transmission(Null, 8, 0x03, (byte)(i - 0x18), 0x03);  //读取摘要
      }
      else
      {
      }
    }
    public void Buttons_RTC_Set()
    {
      byte[] temp = new byte[8];
      temp[1] = 0xF3;
      temp[2] = Convert.ToByte(currentTime.Year - 2000);
      temp[3] = Convert.ToByte(currentTime.Month);
      temp[4] = Convert.ToByte(currentTime.Day);
      temp[5] = Convert.ToByte(currentTime.Hour);
      temp[6] = Convert.ToByte(currentTime.Minute);
      temp[7] = Convert.ToByte(currentTime.Second);
      Main.frm1.Serial_Data_Transmission(temp, 8, RACK_ID_Number_Sent, Shelf_ID_Number_Sent, 0x14);  //设置Divre状态

    }
    //一键使能设置
    public void Button_Eanble_Click()
    {
      byte[] temp = new byte[8];
      for (int i = 0; i < 39; i++)
        if (Set.Set1.TextboxThreshold_Setting[i].Text != string.Empty)
        {
          setStructure.ID = SendSetCMDID[i];
          setStructure.Value = Convert.ToInt32(Set.Set1.TextboxThreshold_Setting[i].Text);

          setStructure.ID = Reverse(setStructure.ID);
          setStructure.Value = Reverse(setStructure.Value);
          temp = Main.frm1.StructToBytes(setStructure);
          Main.frm1.Serial_Data_Transmission(temp, 8, RACK_ID_Number_Sent, Shelf_ID_Number_Sent, 0x12);  //设置Divre状态
        }
        else
        {
          //None
        }
    }
    //一键使能设置
    public void Button_Eanble_Fw_Click()
    {
      byte[] temp = new byte[8];
      for (int i = 0; i < 9; i++)
        if (Set.Set1.Textbox_set_Calibration[i].Text != string.Empty)
        {
          setStructure.ID = SendSetCMDID1[i];
          setStructure.Value = Convert.ToInt32(Set.Set1.Textbox_set_Calibration[i].Text);
          setStructure.ID = Reverse(setStructure.ID);
          setStructure.Value = Reverse(setStructure.Value);
          temp = Main.frm1.StructToBytes(setStructure);
          //协议设置值唯一一个在buff[2]位置与我结构体不符合特殊处理
          if (i == 8)
          {
            temp[1] = 0xF2;
            temp[2] = temp[7];
            temp[7] = 0;
            Main.frm1.Serial_Data_Transmission(temp, 8, RACK_ID_Number_Sent, Shelf_ID_Number_Sent, 0x13);  //设置Divre状态
          }
          else
          {
            Main.frm1.Serial_Data_Transmission(temp, 8, RACK_ID_Number_Sent, Shelf_ID_Number_Sent, 0x12);  //设置Divre状态
          }
        }
        else
        {

        }
    }
  }
}


