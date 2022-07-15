
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
		const byte CMD_Cell_Voltage= 0x03;
    const byte CMD_All_Parameter_Read = 0x11;
    public byte RACK_ID_Number_Sent; //生成要发送的RACKID
    public byte Shelf_ID_Number_Sent; //生成要发送的Shelf ID
    public byte Timer_Count;//时间定时器

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
    public struct SHELF
    {
     public Moudle_Temp moudle_Temp;  //温度
     public Summary summary;    //概要
     public All_Parameter_Read parameter_Read;//设置信息读取
     public Moudle_Voltage voltage;//电压
    };  
    public string[] SerialNuber_data = System.IO.Ports.SerialPort.GetPortNames();

    public Byte[] Rack1Shelf1Summary = new byte[136];      //摘要信息的创建
    public Byte[] Rack1Shelf1Voltage = new byte[136];   //温度信息接受
    public Byte[] Rack1Shelf1Temperature = new byte[136];//温度信息接受
    public Byte[] Rack1Shelf1Parameter = new byte[136];   //温度信息接受
    public struct SetStructure
    {
      public UInt16 ID;
			public byte reseve;
      public byte reseve1;
      public UInt32 Value;
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
    //阈值信息
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct All_Parameter_Read
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
			//预留3组位置
			public UInt16 reserved1;
			public UInt16 reserved2;
			public UInt16 reserved3;
			public UInt16 reserved4;
			public UInt16 reserved5;
			public UInt16 reserved6;
			public UInt16 reserved7;
			public UInt16 reserved8;
			public UInt16 reserved9;
			//参数校准参数
			public UInt16 TotalVol_1_Slope;
			public UInt16 TotalVol_1_Offset;
			public UInt16 TotalVol_2_Slope;
			public UInt16 TotalVol_2_Offset;
			public UInt16 TotalVol_3_Slope;
			public UInt16 TotalVol_3_Offset;
			public UInt16 Current_Slope;
			public UInt16 Current_Offset;
			
    };
  
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
    public byte[] SendSetCMDID = {0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,
                                0x0D,0x0E,0x0F,0x10,0x11,0x12,0x13,0x14,0x15,0x16,0x17,0x18,0x19,
                                0x2A,0x2B,0x2C,0x2D,0x2E,0x2F,0x30,0x31,0x32,0x33,0x34,0x35,0x36,0x37
};
    public Main()
    {
      InitializeComponent();
      RS485_select();
      frm1 = this;
      LoadInitialization();
			Timer();
      Control.CheckForIllegalCrossThreadCalls = false;
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
      comboBox_SerialNumber.Text = "System.IO.Ports.SerialPort.GetPortNames()";
      comboBox_BaudRate.Text = "115200";
      comboBox_CheckDigit.Text = "NONE";
      comboBox_DataBits.Text = "8";
      comboBox_StopBit.Text = "1";
    }
    private void Timer()
    {
      System.Timers.Timer t = new System.Timers.Timer(100);//实例化Timer类，设置间隔时间为10000毫秒；
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
    *describe: 获得Byte的Bit值
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
    *describe: 转换成浮点小鼠
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
      if (ackRcvd.WaitOne(80))
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
      Thread.Sleep(50); //延时
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
                RACKShelf_Summary_decribe(RACK1SHLE1);
              }
              //温度信息Cell Temperature 
              else if (receivedData[4] == CMD_Cell_Temperature)
              {
                Array.ConstrainedCopy(receivedData, 6, Rack1Shelf1Temperature, 0, receivedData.Length - 6);
                RACKShelf_Cell_Temp_decribe(RACK1SHLE1);
              }
              //电压信息 Cell voltage
              else if (receivedData[4] == CMD_Cell_Voltage)
              {
                Array.ConstrainedCopy(receivedData, 6, Rack1Shelf1Voltage, 0, receivedData.Length - 6);
                RACKShelf_Cele_Vol_decribe(RACK1SHLE1);  //RACK1Shelf1的显示线程
              }
              //CMD_All_Parameter_Read 设置信息
              else if (receivedData[4] == CMD_All_Parameter_Read)
              {
                Array.ConstrainedCopy(receivedData, 6, Rack1Shelf1Parameter, 0, receivedData.Length - 6);
                Parameter_Read(RACK1SHLE1);
              }
              break;

            case ID_Shelf2:
            if (receivedData[4] == CMD_Summary)
            {
                Array.ConstrainedCopy(receivedData, 6, Rack1Shelf1Summary, 0, receivedData.Length - 6);
                RACKShelf_Summary_decribe(RACK1SHLE2);
            }

            
            //温度信息Cell Temperature 
            else if (receivedData[4] == CMD_Cell_Temperature)
            {
              Array.ConstrainedCopy(receivedData, 6, Rack1Shelf1Temperature, 0, receivedData.Length - 6);
              RACKShelf_Cell_Temp_decribe(RACK1SHLE2);
            }
            //电压信息 Cell voltage
            else if (receivedData[4] == CMD_Cell_Voltage)
            {
              Array.ConstrainedCopy(receivedData, 6, Rack1Shelf1Voltage, 0, receivedData.Length - 6);
              RACKShelf_Cele_Vol_decribe(RACK1SHLE2);
            }
            //CMD_All_Parameter_Read 设置信息
            else if (receivedData[4] == CMD_All_Parameter_Read)
            {
              Array.ConstrainedCopy(receivedData, 6, Rack1Shelf1Parameter, 0, receivedData.Length - 6);
              Parameter_Read(RACK1SHLE2);
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
            Form_DownLoad.frm_2.progressBa_value(8);
            break;
          case 0x25:
            textBox1.AppendText("Download Failed, please try again!  " + "\r\n");
            Form_DownLoad.frm_2.progressBa_value(0);
            break;
          case 0x40:
            textBox1.AppendText("Download Failed, please try again!  " + "\r\n");
            Form_DownLoad.frm_2.progressBa_value(0);
            break;
          case 0x41:
            textBox1.AppendText("Download Failed, please try again!  " + "\r\n");
            Form_DownLoad.frm_2.progressBa_value(0);
            break;
          case 0x42:
            textBox1.AppendText("Download Failed, please try again!  " + "\r\n");
            Form_DownLoad.frm_2.progressBa_value(0);
            break;
          case 0x43:
            textBox1.AppendText("Download Failed, please try again!  " + "\r\n");
            Form_DownLoad.frm_2.progressBa_value(0);
            break;
          case 0x44:
            textBox1.AppendText("Download Failed, please try again!  " + "\r\n");
            Form_DownLoad.frm_2.progressBa_value(0);
            break;
          case 0x45:
            textBox1.AppendText("Download Failed, please try again!  " + "\r\n");
            Form_DownLoad.frm_2.progressBa_value(0);
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
      var packetBuffer = new byte[255]; // 128 bytes data + 1 length + 1 opcode + 1 terminator
      var dataBuffer = new byte[252];

      packetBuffer[0] = COMMAND_SEND_DATA;
      packetBuffer[1] = 0;
      int temp_value = 4;

      using (Stream fs = openFileDialog1.OpenFile())
      {
        using (BinaryReader reader = new BinaryReader(fs))
        {
          while (reader.BaseStream.Position < reader.BaseStream.Length)
          {
            dataBuffer = new byte[252];
            temp_value++;
            var bytesRead = reader.BaseStream.Read(dataBuffer, 0, 252);
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
                Array.ConstrainedCopy(dataBuffer, 0, packetBuffer, 3, dataBuffer.Length);
              }
              else // readjust size (this is the last chunk!)
              {
                Array.ConstrainedCopy(dataBuffer, 0, packetBuffer, 3, bytesRead);
                packetBuffer[0] = (byte)(bytesRead + 3); // readjust size byte
                                                         //  Form_DownLoad.frm_2.progressBa_value(99);
              }
            }
            packetBuffer[1] = getCheckSum(dataBuffer, 252);
            packetBuffer[2] = 0x24;
            Main.frm1.SendPacketAck(packetBuffer, bytesRead + 3);
            textBox1.AppendText("Downloading..." + "\r\n");
            // Thread.Sleep(1);//休眠时间
            SendPacketAck(COMMAND_GET_STATUS, 4);
            Form_DownLoad.frm_2.progressBa_value((int)reader.BaseStream.Position);
          }
        }

        Thread.Sleep(300);//休眠时间
        var temp_1 = new byte[3] { 0x03, 0x25, 0x25 };
        SendPacketAck(temp_1, 3);
        // Form_DownLoad.frm_2.progressBa_value(100);
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
		public void Serial_Data_Transmission(byte[] buffer, byte len,byte RackID ,byte ShelfID,byte CMD )
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
    //

    public void RACKShelf_Cele_Vol_decribe(SHELF TEMP)
    {
      //结构体转化将
      TEMP.voltage = (Moudle_Voltage)BytesToDataStruct(Rack1Shelf1Voltage, typeof(Moudle_Voltage));
      for (int j = 0; j < 66; j++)
      {
        TEMP.voltage.Cell_Vol[j] = Reverse(TEMP.voltage.Cell_Vol[j]);
      }
    }

    public void Parameter_Read(SHELF TEMP)
    {
      TEMP.parameter_Read = (All_Parameter_Read)BytesToDataStruct(Rack1Shelf1Parameter, typeof(All_Parameter_Read));
      TEMP.parameter_Read.Cell_OV_Warning_Pre = Reverse(TEMP.parameter_Read.Cell_OV_Warning_Pre);
      TEMP.parameter_Read.Cell_OV_Warning = Reverse(TEMP.parameter_Read.Cell_OV_Warning);
      TEMP.parameter_Read.Cell_OV_Fault = Reverse(TEMP.parameter_Read.Cell_OV_Fault);

      TEMP.parameter_Read.Battery_OV_Warning_Pre = Reverse(TEMP.parameter_Read.Battery_OV_Warning_Pre);
      TEMP.parameter_Read.Battery_OV_Warning = Reverse(TEMP.parameter_Read.Battery_OV_Warning);
      TEMP.parameter_Read.Battery_OV_Fault = Reverse(TEMP.parameter_Read.Battery_OV_Fault);

      TEMP.parameter_Read.Chg_OCur_Warning_Pre = Reverse(TEMP.parameter_Read.Chg_OCur_Warning_Pre);
      TEMP.parameter_Read.Chg_OCur_Warning = Reverse(TEMP.parameter_Read.Chg_OCur_Warning);
      TEMP.parameter_Read.Chg_OCur_Fault = Reverse(TEMP.parameter_Read.Chg_OCur_Fault);

      TEMP.parameter_Read.Chg_Utemp_Warning_Pre = Reverse(TEMP.parameter_Read.Chg_Utemp_Warning_Pre);
      TEMP.parameter_Read.Chg_Utemp_Warning = Reverse(TEMP.parameter_Read.Chg_Utemp_Warning);
      TEMP.parameter_Read.Chg_Utemp_Fault = Reverse(TEMP.parameter_Read.Chg_Utemp_Fault);

      TEMP.parameter_Read.Chg_Otemp_Warning_Pre = Reverse(TEMP.parameter_Read.Chg_Otemp_Warning_Pre);
      TEMP.parameter_Read.Chg_Otemp_Warning = Reverse(TEMP.parameter_Read.Chg_Otemp_Warning);
      TEMP.parameter_Read.Chg_Otemp_Fault = Reverse(TEMP.parameter_Read.Chg_Otemp_Fault);

      TEMP.parameter_Read.Cell_UV_Warning_Pre = Reverse(TEMP.parameter_Read.Cell_UV_Warning_Pre);
      TEMP.parameter_Read.Cell_UV_Warning = Reverse(TEMP.parameter_Read.Cell_UV_Warning);
      TEMP.parameter_Read.Cell_UV_Fault = Reverse(TEMP.parameter_Read.Cell_UV_Fault);

      TEMP.parameter_Read.Battery_UV_Warning_Pre = Reverse(TEMP.parameter_Read.Battery_UV_Warning_Pre);
      TEMP.parameter_Read.Battery_UV_Warning = Reverse(TEMP.parameter_Read.Battery_UV_Warning);
      TEMP.parameter_Read.Battery_UV_Fault = Reverse(TEMP.parameter_Read.Battery_UV_Fault);

      TEMP.parameter_Read.DisChg_OCur_Warning_Pre = Reverse(TEMP.parameter_Read.DisChg_OCur_Warning_Pre);
      TEMP.parameter_Read.DisChg_OCur_Warning = Reverse(TEMP.parameter_Read.DisChg_OCur_Warning);
      TEMP.parameter_Read.DisChg_OCur_Fault = Reverse(TEMP.parameter_Read.DisChg_OCur_Fault);

      TEMP.parameter_Read.DisChg_UTemp_Warning_Pre = Reverse(TEMP.parameter_Read.DisChg_UTemp_Warning_Pre);
      TEMP.parameter_Read.DisChg_UTemp_Warning = Reverse(TEMP.parameter_Read.DisChg_UTemp_Warning);
      TEMP.parameter_Read.DisChg_UTemp_Fault = Reverse(TEMP.parameter_Read.DisChg_UTemp_Fault);

      TEMP.parameter_Read.DisChg_Otemp_Warning_Pre = Reverse(TEMP.parameter_Read.DisChg_Otemp_Warning_Pre);
      TEMP.parameter_Read.DisChg_Otemp_Warning = Reverse(TEMP.parameter_Read.DisChg_Otemp_Warning);
      TEMP.parameter_Read.DisChg_Otemp_Fault = Reverse(TEMP.parameter_Read.DisChg_Otemp_Fault);

      TEMP.parameter_Read.SOC_Low_Warning_Pre = Reverse(TEMP.parameter_Read.SOC_Low_Warning_Pre);
      TEMP.parameter_Read.SOC_Low_Warning = Reverse(TEMP.parameter_Read.SOC_Low_Warning);
      TEMP.parameter_Read.SOC_Low_Fault = Reverse(TEMP.parameter_Read.SOC_Low_Fault);

      TEMP.parameter_Read.V_Diff_Warning_Pre = Reverse(TEMP.parameter_Read.V_Diff_Warning_Pre);
      TEMP.parameter_Read.V_Diff_Warning = Reverse(TEMP.parameter_Read.V_Diff_Warning);
      TEMP.parameter_Read.V_Diff_Fault = Reverse(TEMP.parameter_Read.V_Diff_Fault);

      TEMP.parameter_Read.T_Diff_Warning_Pre = Reverse(TEMP.parameter_Read.T_Diff_Warning_Pre);
      TEMP.parameter_Read.T_Diff_Warning = Reverse(TEMP.parameter_Read.T_Diff_Warning);
      TEMP.parameter_Read.T_Diff_Fault = Reverse(RACK1SHLE1.parameter_Read.T_Diff_Fault);
    }
    public void RACKShelf_Summary_decribe(SHELF TEMP)
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
    }
    private void RACKShelf_Cell_Temp_decribe(SHELF TEMP)
    {
      TEMP.moudle_Temp = (Moudle_Temp)BytesToDataStruct(Rack1Shelf1Temperature , typeof(Moudle_Temp));
      
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
      OpenChildFrom(new Shelf2());
      panel_main.Text = "RACK1  -  SHELF2";
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
    private void Send_Query_Command(byte RACK,byte Shelf)
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
        //捕获可能发生的异常并进行处理
        //捕获到异常，创建一个新的对象，之前的不可以再用
        serialPort1 = new System.IO.Ports.SerialPort();
        comboBox_SerialNumber.Items.Clear();
        comboBox_SerialNumber.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
        //响铃并显示异常给用户
        System.Media.SystemSounds.Beep.Play();
        button_Connect.Text = "DisConnect";
        uiLight6.State = Sunny.UI.UILightState.Off;
        button_Connect.BackColor = Color.ForestGreen;
        MessageBox.Show(ex.Message);
        MessageBox.Show(ex.Message);
        MessageBox.Show(ex.Message);
        MessageBox.Show(ex.Message);
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
    //一键读取设置
    public void Button_read_Click()
    {
			byte[] Null = new byte[8];
			 //读取参数设置所有参数设置
      Main.frm1.Serial_Data_Transmission(Null,8,Main.frm1.RACK_ID_Number_Sent,Main.frm1.Shelf_ID_Number_Sent,0x11); 
    }

      public void Poll_Query(object source, System.Timers.ElapsedEventArgs e)
    {
      string[] a = System.IO.Ports.SerialPort.GetPortNames();
      if (SerialNuber_data.Length  != a.Length)
      {
        SerialNuber_data = System.IO.Ports.SerialPort.GetPortNames();
        comboBox_SerialNumber.Items.Clear();
        comboBox_SerialNumber.Items.Clear();

        comboBox_SerialNumber.Items.AddRange(SerialNuber_data);
      }

      Timer_Count++;
      if (Timer_Count == 40)
      {
        Timer_Count = 0;
      }
      chaxunzhiling(Timer_Count);
    }
  public void chaxunzhiling(byte i)
  {
    if (i < 8)
    {
      Serial_Data_Transmission(Null, 8, 0x01, i, 0x01);  //读取摘要
    }
    else if (i < 24)
    {

      Serial_Data_Transmission(Null, 8, 0x02, (byte)(i - 0x08), 0x01);  //读取摘要
    }
    else if (i < 40)
    {
      Serial_Data_Transmission(Null, 8, 0x03, (byte)(i - 0x28), 0x01);  //读取摘要

    }
    else
    {
    }
  }
    //一键使能设置
    public void Button_Eanble_Click()
    {
      byte[] temp = new byte[8];
      for (int i = 0; i<39;i++)
        if (Set.Set1.TextboxThreshold_Setting[i].Text != string.Empty)
        {
          setStructure.ID = SendSetCMDID[i];
          setStructure.Value = Convert.ToUInt32(Set.Set1.TextboxThreshold_Setting[i].Text);
					
					setStructure.ID=Reverse(setStructure.ID);
					setStructure.Value = Reverse(setStructure.Value);
          temp = Main.frm1.StructToBytes(setStructure);
          Main.frm1.Serial_Data_Transmission(temp, 8, RACK_ID_Number_Sent, Shelf_ID_Number_Sent, 0x12);  //设置Divre状态
        }
        else
        {
          //None
        }
    }
  }
}


