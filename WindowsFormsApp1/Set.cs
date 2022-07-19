using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
  public partial class Set : Form
  {
    public static Set Set1;
    public Set()
    {
      InitializeComponent();
      Set_Load_Init();
      Set_FW();
      Set_RACK_ShlefID();
      Set1 = this;
    }
    const int Width_Label = 160; //50
    const int Height_Label = 20;
    const int Width_Text = 50;
    const int Height_Text = 20;
    const int Width_Button = 50;
    const int Height_Button = 25;
    const int Width_Panel = 920;
    const int Number_Lines = 11;//每列的行数设置
    const int Number_Line_1 = 3;//每列的行数设置
    const int Number_Line_2 = 4;//每列的行数设置
    const int Row_Spacing = 10; //行间距设置
    const int Top_distance = 45; //顶部距离
    const int Line_size = 3; //粗细
    public Sunny.UI.UILight[] Drive_Lights = new Sunny.UI.UILight[20];
    public TextBox[] TextboxThreshold_Setting = new TextBox[50];  //阈值设置文本框 
    public TextBox[] TextboxThreshold_Read = new TextBox[50];  //阈值读取文本框
    public TextBox[] Textbox_Read_FW = new TextBox[48];  //设备状态读取文本框
    public TextBox[] Textbox_set_FW = new TextBox[60]; //设备设置文本框
    public TextBox[] Textbox_Read_Calibration = new TextBox[48];  //校准参数读取文本框
    public TextBox[] Textbox_set_Calibration = new TextBox[60]; //校准参数设置文本框
    public Label[] label_set = new Label[60]; //阈值标签	
    public Label[] label_set_FW = new Label[60];  //设备状态文本
    public Label[] label_set_ID = new Label[60];  //设备状态文本
    public Sunny.UI.UISwitch[] UISwitch_set_FW = new Sunny.UI.UISwitch[20];
    public Sunny.UI.UISymbolButton Enable = new Sunny.UI.UISymbolButton();
    public Sunny.UI.UISymbolButton Read_Flash = new Sunny.UI.UISymbolButton();
    public Sunny.UI.UILine Line_1 = new Sunny.UI.UILine();
    public Sunny.UI.UILine Line_2 = new Sunny.UI.UILine();
    public Sunny.UI.UILine Line_3 = new Sunny.UI.UILine();
    public Sunny.UI.UILine Line_4 = new Sunny.UI.UILine();
    public Sunny.UI.UILine Line_5 = new Sunny.UI.UILine();
    private  Main.SHELF MyDAta = Main.frm1.RACK1SHLE1;

    //驱动器手动控制
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
		public struct Driver_Manual_control
		{
			public short Diver;
			public byte Acttion;
			public byte RESERVED1;
			public byte RESERVED2;
			public byte RESERVED3;
      public byte RESERVED4;
      public byte RESERVED5;
    };
    //阈值设置发送
    public struct Driver_Manual_Control
    {

    };
    Driver_Manual_control driver_Manual_Control;
    public void Set_Load_Init()
    {
      int i = 0;
      Sunny.UI.UIButton Read = new Sunny.UI.UIButton();
      Sunny.UI.UIButton Eanable = new Sunny.UI.UIButton();
      Read.Name = "Read";
      Read.Text = "Refresh";
      Read.Width = 125;
      Read.Height = 35;
      Read.Font = new Font("微软雅黑 ", 11F);
      Read.Location = new Point(950, 20 + 10 * (Row_Spacing + Height_Label));
      Read.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      Read.Click += delegate
      {
        /*22-07-15更改
         * 取消使用Main.frm1.Button_read_Click()        
         * 协议改变使用0x50命令，不可以使用全部查询命令
         * 更改为调用函数        
         * Main.frm1.ThreshholdValue_Set_Click();
         */

        //Main.frm1.Button_read_Click();

        //阈值设置
        Main.frm1.ThreshholdValue_Set_Click();
      };
      Read.Parent = uiTitlePanel1;


      Line_1.Name = "Read";
      Line_1.Location = new Point(295, 30);
      Line_1.Width = 16;
      Line_1.Height = 345;
      Line_1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      Line_1.Parent = uiTitlePanel1;
      Line_1.Direction = Sunny.UI.UILine.LineDirection.Vertical;
      Line_1.LineSize = Line_size;

      Line_2.Name = "Read";
      Line_2.Location = new Point(590, 30);
      Line_2.Width = 16;
      Line_2.Height = 345;
      Line_2.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      Line_2.Parent = uiTitlePanel1;
      Line_2.Direction = Sunny.UI.UILine.LineDirection.Vertical;
      Line_2.LineSize = Line_size;

      Line_3.Name = "Read";
      Line_3.Location = new Point(900, 30);
      Line_3.Width = 16;
      Line_3.Height = 345;
      Line_3.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      Line_3.Parent = uiTitlePanel1;
      Line_3.Direction = Sunny.UI.UILine.LineDirection.Vertical;
      Line_3.LineSize = Line_size;
      Eanable.Name = "Read";
      Eanable.Name = "Read";
      Eanable.Name = "Read";
      Eanable.Text = "Set";
      Eanable.Width = 125;
      Eanable.Height = 35;
      Eanable.Font = new Font("微软雅黑 ", 11F);
      Eanable.Location = new Point(1100, 20 + 10 * (Row_Spacing + Height_Label));
      Eanable.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      Eanable.Click += delegate
      {
        Main.frm1.Button_Eanble_Click();
			};
      Eanable.Parent = uiTitlePanel1;
      //Shlef2
      for (i = 0; i < 39; i++)
      {
        //创建标签与显示窗体
        TextboxThreshold_Setting[i] = new TextBox();
        TextboxThreshold_Read[i] = new TextBox();
        label_set[i] = new Label();
        label_set[i].Name = "Labe2_Volatage" + i.ToString();
        label_set[i].Width = Width_Label;
        label_set[i].Height = Height_Label;
        label_set[i].Parent = uiTitlePanel1;
        label_set[i].Font = new Font("Leelawadee", 7F);

        TextboxThreshold_Setting[i].Name = "Voltage_Battery" + i.ToString();
        TextboxThreshold_Setting[i].Width = Width_Text;
        TextboxThreshold_Setting[i].Height = Height_Text;
        TextboxThreshold_Setting[i].Parent = uiTitlePanel1;
        TextboxThreshold_Setting[i].Font = new Font("Leelawadee", 9F);
				
        TextboxThreshold_Read[i].Name = "Voltage_Battery" + i.ToString();
        TextboxThreshold_Read[i].Width = Width_Text;
        TextboxThreshold_Read[i].Height = Height_Text;
        TextboxThreshold_Read[i].Parent = uiTitlePanel1;
        TextboxThreshold_Read[i].Font = new Font("Leelawadee", 9F);
        TextboxThreshold_Read[i].ReadOnly = true;

        #region 
        if (i < Number_Lines)
        {
          label_set[i].Location = new Point(10, Top_distance + i * (Row_Spacing + Height_Label));
          TextboxThreshold_Read[i].Location = new Point(180, Top_distance + i * (Row_Spacing + Height_Label));
          TextboxThreshold_Setting[i].Location = new Point(240, Top_distance + i * (Row_Spacing + Height_Label));
        }
        else if (i < (2 * Number_Lines))
        {
          label_set[i].Location = new Point(310, Top_distance + (i - Number_Lines) * (Row_Spacing + Height_Label));
          TextboxThreshold_Read[i].Location = new Point(480, Top_distance + (i - Number_Lines) * (Row_Spacing + Height_Label));
          TextboxThreshold_Setting[i].Location = new Point(540, Top_distance + (i - Number_Lines) * (Row_Spacing + Height_Label));
        }

        else if (i < (3 * Number_Lines))
        {
          label_set[i].Location = new Point(610, Top_distance + (i - 2 * Number_Lines) * (Row_Spacing + Height_Label));
          TextboxThreshold_Read[i].Location = new Point(790, Top_distance + (i - 2 * Number_Lines) * (Row_Spacing + Height_Label));
          TextboxThreshold_Setting[i].Location = new Point(850, Top_distance + (i - 2 * Number_Lines) * (Row_Spacing + Height_Label));
        }
        else if (i < 4 * Number_Lines)
        {
          label_set[i].Location = new Point(930, Top_distance + (i - 3 * Number_Lines) * (Row_Spacing + Height_Label));
          TextboxThreshold_Read[i].Location = new Point(1110, Top_distance + (i - 3 * Number_Lines) * (Row_Spacing + Height_Label));
          TextboxThreshold_Setting[i].Location = new Point(1180, Top_distance + (i - 3 * Number_Lines) * (Row_Spacing + Height_Label));
        }
        else
        {
          //None
        }
        #endregion
        #region      //标签单独描述
        if (i == 0)
        {
          label_set[i].Text = "Cell_OV_Warning_Pre"+ "(mV)";
        }
        else if (i == 1)
        {
          label_set[i].Text = "Cell_OV_Warning" + "（mV）";
        }
        else if (i == 2)
        {
          label_set[i].Text = "Cell_OV_Fault" + "（mV）";
        }
        else if (i == 3)
        {
          label_set[i].Text = "Battery_OV_Warning_Pre" + "（0.1V）";
        }
        else if (i == 4)
        {
          label_set[i].Text = "Battery_OV_Warning" + "（0.1V）";
        }
        else if (i == 5)
        {
          label_set[i].Text = "Battery_OV_Fault" + "（0.1V)";
        }
        else if (i == 6)
        {
          label_set[i].Text = "Chg_OCur_Warning_Pre" + "(0.1A)";
        }
        else if (i == 7)
        {
          label_set[i].Text = "Chg_OCur_Warning" + "(0.1A)";
        }
        else if (i == 8)
        {
          label_set[i].Text = "Chg_OCur_Fault" + "(0.1A)";
        }
        else if (i == 9)
        {
          label_set[i].Text = "Chg_UTemp_Pre" + "(°C)";
        }
        else if (i == 10)
        {
          label_set[i].Text = "Chg_UTemp_Warning" + "(°C)";

        }
        else if (i == 11)
        {
          label_set[i].Text = "Chg_UTemp_Fault" + "(  ℃)";
        }
        else if (i == 12)
        {
          label_set[i].Text = "Chg_OTemp_Pre" + "(℃)";
        }
        else if (i == 13)
        {
          label_set[i].Text = "Chg_OTemp_Warning" + "(℃)";
        }
        else if (i == 14)
        {
          label_set[i].Text = "Chg_OTemp_Fault" + "(℃)";
        }
        else if (i == 15)
        {
          label_set[i].Text = "Cell_UV_Pre" + "(mV)";
        }
        else if (i == 16)
        {
          label_set[i].Text = "Cell_UV_Warning" + "(mV)";
        }
        else if (i == 17)
        {
          label_set[i].Text = "Cell_UV_Fault" +  "(mV)";
        }
        else if (i == 18)
        {
          label_set[i].Text = "Battery_UV_Pre" + "(0.1V)";
        }
        else if (i == 19)
        {
          label_set[i].Text = "Battery_UV_Warning" +"(0.1V)";
        }
        else if (i == 20)
        {
          label_set[i].Text = "Battery_UV_Fault" + "(0.1V)";
        }
        else if (i == 21)
        {
          label_set[i].Text = "DisChg_OCur_Pre"+"(0.1A)";
        }
        else if (i == 22)
        {
          label_set[i].Text = "DisChg_OCur_Warning"+"(0.1A)";
        }
        else if (i == 23)
        {
          label_set[i].Text = "DisChg_OCur_Fault" + "(0.1A)";
        }
        else if (i == 24)
        {
          label_set[i].Text = "DisChg_UTemp_Warning_Pre" + "(℃)";
        }
        else if (i == 25)
        {
          label_set[i].Text = "DisChg_UTemp_Warning"+ "(℃)";
        }

        else if (i == 26)
        {
          label_set[i].Text = "DisChg_UTemp_Fault"+"(℃)"; 
        }
        else if (i == 27)
        {
          label_set[i].Text = "DisChg_Otemp_Warning_Pre"+"(℃)"; 
        }
        else if (i == 28)
        {
          label_set[i].Text = "DisChg_Otemp_Warning"+"(℃)" ;
        }
        else if (i == 29)
        {
          label_set[i].Text = "DisChg_Otemp_Fault"+"(℃)"; 
        }
        else if (i == 30)
        {
          label_set[i].Text = "SOC_Low_Warning_Pre"+"(%)";
        }
        else if (i == 31)
        {
          label_set[i].Text = "SOC_Low_Warning"+"(%)";
        }
        else if (i == 32)
        {
          label_set[i].Text = "SOC_Low_Fault"+"(%)";
        }
        else if (i == 33)
        {
          label_set[i].Text = "V_Diff_Warning_Pre" +"(mV)";
        }
        else if (i == 34)
        {
          label_set[i].Text = "V_Diff_Warning" + "(mV)";
        }
        else if (i == 35)
        {
          label_set[i].Text = "V_Diff_Fault" + "(mV)";
        }
        else if (i == 36)
        {
          label_set[i].Text = "T_Diff_Warning_Pre"+ "(℃)";
        }
        else if (i == 37)
        {
          label_set[i].Text = "T_Diff_Warning"+ "(℃)";
        }
        else if (i == 38)
        {
          label_set[i].Text = "T_Diff_Fault"+ "(℃)";
        }

        #endregion
        this.uiTitlePanel1.Height = 380;
        this.uiTitlePanel1.Text = "Threshold Setting";
      }
    }
    public void Set_FW()
    {
      Sunny.UI.UIButton Read_1 = new Sunny.UI.UIButton();
      Sunny.UI.UIButton Eanable_1 = new Sunny.UI.UIButton();
      Read_1.Name = "Read";
      Read_1.Text = "Refresh";
      Read_1.Width = 125;
      Read_1.Height = 35;

      Read_1.Font = new Font("微软雅黑 ", 11F);
      Read_1.Location = new Point(950, 20 + 3 * (Row_Spacing + Height_Label));
      Read_1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      Read_1.Click += delegate
      {
        /*取消使用全部查询函数*/
        //Main.frm1.Button_read_Click();
        //校准值查询函数
        Main.frm1.Read_CalibrationValue();
      };
      Read_1.Parent = uiTitlePanel2;
      Eanable_1.Name = "Read";
      Eanable_1.Text = "Set";

      Eanable_1.Font = new Font("微软雅黑 ", 11F);
      Eanable_1.Width = 125;
      Eanable_1.Height = 35;
      Eanable_1.Location = new Point(1100, 20 + 3 * (Row_Spacing + Height_Label));
      Eanable_1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
      Eanable_1.Click += delegate
      {
				Main.frm1.Button_Eanble_Fw_Click();
			};
      Eanable_1.Parent = uiTitlePanel2;
      for (int i = 0; i < 10; i++)
      {
        label_set_FW[i] = new Label();
        UISwitch_set_FW[i] = new Sunny.UI.UISwitch();
        Drive_Lights[i] = new Sunny.UI.UILight();

        Drive_Lights[i] = new Sunny.UI.UILight();
        Drive_Lights[i].Name = "LED" + i.ToString();
        Drive_Lights[i].Height = 20;
        Drive_Lights[i].Parent = uiTitlePanel3;
        Drive_Lights[i].OnColor = Color.Gray;

        label_set_FW[i].Name = "Labe2_Volatage" + i.ToString();
        label_set_FW[i].Width = Width_Label;
        label_set_FW[i].Height = Height_Label;
        label_set_FW[i].Parent = uiTitlePanel3;
        label_set_FW[i].Font = new Font("Leelawadee", 9F);

        Drive_Lights[i].Name = "TextboxThreshold_Read" + i.ToString();
        Drive_Lights[i].Width = Width_Text;
        Drive_Lights[i].Height = Height_Text;
        Drive_Lights[i].Parent = uiTitlePanel3;
        Drive_Lights[i].Font = new Font("Leelawadee", 9F);

        UISwitch_set_FW[i].Name = "TextboxThreshold_Setting" + i.ToString();
        UISwitch_set_FW[i].Width = Width_Text;
        UISwitch_set_FW[i].Height = Height_Text;
        UISwitch_set_FW[i].Parent = uiTitlePanel3;
        UISwitch_set_FW[i].Font = new Font("Leelawadee", 9F);
        UISwitch_set_FW[i].ActiveText = "ON";
        UISwitch_set_FW[i].InActiveText = "OFF";

        if (i < Number_Line_1)
        {
          label_set_FW[i].Location = new Point(10, Top_distance + i * (Row_Spacing + Height_Label));
          Drive_Lights[i].Location = new Point(180, Top_distance + i * (Row_Spacing + Height_Label));
          UISwitch_set_FW[i].Location = new Point(240, Top_distance + i * (Row_Spacing + Height_Label));
        }
        else if (i < (2 * Number_Line_1))
        {
          label_set_FW[i].Location = new Point(310, Top_distance + (i - Number_Line_1) * (Row_Spacing + Height_Label));
          Drive_Lights[i].Location = new Point(480, Top_distance + (i - Number_Line_1) * (Row_Spacing + Height_Label));
          UISwitch_set_FW[i].Location = new Point(540, Top_distance + (i - Number_Line_1) * (Row_Spacing + Height_Label));
        }

        else if (i < (3 * Number_Line_1))
        {
          label_set_FW[i].Location = new Point(610, Top_distance + (i - 2 * Number_Line_1) * (Row_Spacing + Height_Label));
          Drive_Lights[i].Location = new Point(790, Top_distance + (i - 2 * Number_Line_1) * (Row_Spacing + Height_Label));
          UISwitch_set_FW[i].Location = new Point(850, Top_distance + (i - 2 * Number_Line_1) * (Row_Spacing + Height_Label));
        }
        else if (i < (4 * Number_Line_1))
        {
          label_set_FW[i].Location = new Point(930, Top_distance + (i - 3 * Number_Line_1) * (Row_Spacing + Height_Label));
          Drive_Lights[i].Location = new Point(1110, Top_distance + (i - 3 * Number_Line_1) * (Row_Spacing + Height_Label));
          UISwitch_set_FW[i].Location = new Point(1180, Top_distance + (i - 3 * Number_Line_1) * (Row_Spacing + Height_Label));
        }
        else
        {
          //None
        }

        if (i == 0)
        {
          label_set_FW[i].Text = "LED1";
        }
        else if (i == 1)
        {
          label_set_FW[i].Text = "FAN1";

        }
        else if (i == 2)
        {
          label_set_FW[i].Text = "Main Positive Contactor";

        }
        else if (i == 3)
        {
          label_set_FW[i].Text = "LED2";
        }
        else if (i == 4)
        {
          label_set_FW[i].Text = "FAN2";

        }
        else if (i == 5)
        {
          label_set_FW[i].Text = "Main Negative Contactor";

        }
        else if (i == 6)
        {
          label_set_FW[i].Text = "LED3";
        }
        else if (i == 7)
        {
          label_set_FW[i].Text = "FAN3";
        }
        else if (i == 8)
        {
          label_set_FW[i].Text = "PreChg Contactor";
        }
        else if (i == 9)
        {
          label_set_FW[i].Text = "LED4";
        }
				else
				{
					//None
				}		
      }
      UISwitch_set_FW[0].Click += delegate
      {
        UISwitch_set_FW0_Click();
      };
			UISwitch_set_FW[1].Click += delegate
      {
        UISwitch_set_FW1_Click();
      };
			UISwitch_set_FW[2].Click += delegate
      {
        UISwitch_set_FW2_Click();
      };
			UISwitch_set_FW[3].Click += delegate
      {
        UISwitch_set_FW3_Click();
      };
			UISwitch_set_FW[4].Click += delegate
      {
        UISwitch_set_FW4_Click();
      };
			UISwitch_set_FW[5].Click += delegate
      {
        UISwitch_set_FW5_Click();
      };
			UISwitch_set_FW[6].Click += delegate
      {
        UISwitch_set_FW6_Click();
      };
			UISwitch_set_FW[7].Click += delegate
      {
        UISwitch_set_FW7_Click();
      };
			UISwitch_set_FW[8].Click += delegate
      {
        UISwitch_set_FW8_Click();
      };
      UISwitch_set_FW[9].Click += delegate
      {
        UISwitch_set_FW9_Click();
      };

      this.uiTitlePanel3.Height = 140;
      this.uiTitlePanel3.Text = "Force Enable";
    }
    public void Set_RACK_ShlefID()
    {
      for (int i = 0; i < 9; i++)
      {
        Textbox_Read_Calibration[i] = new TextBox();
        label_set_ID[i] = new Label();
        Textbox_set_Calibration[i] = new TextBox();

        label_set_ID[i].Name = "Labe2_Volatage" + i.ToString();
        label_set_ID[i].Width = Width_Label;
        label_set_ID[i].Height = Height_Label;
        label_set_ID[i].Parent = uiTitlePanel2;
        label_set_ID[i].Font = new Font("Leelawadee", 9F);

        Line_4.Name = "Read";
        Line_4.Location = new Point(295, 38);
        Line_4.Width = 16;
        Line_4.Height = 160;
        Line_4.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
        Line_4.Parent = uiTitlePanel2;
        Line_4.Direction = Sunny.UI.UILine.LineDirection.Vertical;
        Line_4.LineSize = Line_size;
        Line_5.Name = "Read";
        Line_5.Location = new Point(590, 38);
        Line_5.Width = 16;
        Line_5.Height = 160;
        Line_5.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(170)))), ((int)(((byte)(50)))));
        Line_5.Parent = uiTitlePanel2;
        Line_5.Direction = Sunny.UI.UILine.LineDirection.Vertical;
        Line_5.LineSize = Line_size;
        Textbox_set_Calibration[i].Name = "Textbox_set_Calibration" + i.ToString();
        Textbox_set_Calibration[i].Width = Width_Text;
        Textbox_set_Calibration[i].Height = Height_Text;
        Textbox_set_Calibration[i].Parent = uiTitlePanel2;
        Textbox_set_Calibration[i].Font = new Font("Leelawadee", 9F);

        Textbox_Read_Calibration[i].Name = "Textbox_Read_Calibration" + i.ToString();
        Textbox_Read_Calibration[i].Width = Width_Text;
        Textbox_Read_Calibration[i].Height = Height_Text;
        Textbox_Read_Calibration[i].Parent = uiTitlePanel2;
        Textbox_Read_Calibration[i].Font = new Font("Leelawadee", 9F);
        Textbox_Read_Calibration[i].ReadOnly = true;
        if (i < Number_Line_2)
        {
          label_set_ID[i].Location = new Point(10, Top_distance + i * (Row_Spacing + Height_Label));
          Textbox_Read_Calibration[i].Location = new Point(180, Top_distance + i * (Row_Spacing + Height_Label));
          Textbox_set_Calibration[i].Location = new Point(240, Top_distance + i * (Row_Spacing + Height_Label));
        }
        else if (i < (2 * Number_Line_2))
        {
          label_set_ID[i].Location = new Point(310, Top_distance + (i - Number_Line_2) * (Row_Spacing + Height_Label));
          Textbox_Read_Calibration[i].Location = new Point(480, Top_distance + (i - Number_Line_2) * (Row_Spacing + Height_Label));
          Textbox_set_Calibration[i].Location = new Point(540, Top_distance + (i - Number_Line_2) * (Row_Spacing + Height_Label));
        }

        else if (i < (3 * Number_Line_2))
        {
          label_set_ID[i].Location = new Point(610, Top_distance + (i - 2 * Number_Line_2) * (Row_Spacing + Height_Label));
          Textbox_Read_Calibration[i].Location = new Point(790, Top_distance + (i - 2 * Number_Line_2) * (Row_Spacing + Height_Label));
          Textbox_set_Calibration[i].Location = new Point(850, Top_distance + (i - 2 * Number_Line_2) * (Row_Spacing + Height_Label));
        }
        else if (i < 4 * Number_Line_2)
        {
          label_set_ID[i].Location = new Point(930, Top_distance + (i - 3 * Number_Line_2) * (Row_Spacing + Height_Label));
          Textbox_Read_Calibration[i].Location = new Point(1110, Top_distance + (i - 3 * Number_Line_2) * (Row_Spacing + Height_Label));
          Textbox_set_Calibration[i].Location = new Point(1180, Top_distance + (i - 3 * Number_Line_2) * (Row_Spacing + Height_Label));
        }
        else
        {
          //None
        }
        if (i == 0)
        {
          label_set_ID[i].Text = "TotalVol_1_Slope"+"(0.001)";
        }
        else if (i == 1)
        {
          label_set_ID[i].Text = "TotalVol_1_Offset"+ "(0.1V)";
        }
        else if (i == 2)
        {
          label_set_ID[i].Text = "TotalVol_2_Slope"+"(0.001)";
        }
        else if (i == 3)
        {
          label_set_ID[i].Text = "TotalVol_2_Offset"+"(0.1V)";
        }
        else if (i == 4)
        {
          label_set_ID[i].Text = "TotalVol_3_Slope" + "(0.001)";
        }
        else if (i == 5)
        {
          label_set_ID[i].Text = "TotalVol_3_Offset"+"(0.1V)";
        }
        else if (i == 6)
        {
          label_set_ID[i].Text = "Current_Slope" + "(0.001)";
        }
        else if (i == 7)
        {
          label_set_ID[i].Text = "Current_Offset"+"(0.1A)";
        }
        else if (i == 8)
        {
          label_set_ID[i].Text = "SOC_Set"+"(%)";
        }
        else
        {
          //None
        }

      }
      this.uiTitlePanel2.Height = 170;
      this.uiTitlePanel2.Text = "Calibration Settings";
    }
    

		//一键读取设置
		private void Button_read_Click()
    {
      byte[] Null = new byte[8];
      Main.frm1.Serial_Data_Transmission(Null,8,Main.frm1.RACK_ID_Number_Sent,Main.frm1.Shelf_ID_Number_Sent,0x11);  //读取参数设置所有参数设置
    }
		//LED1设置
    public void UISwitch_set_FW0_Click()
    {
        byte[] temp = new byte[8];
				driver_Manual_Control.Diver = 0;				
        driver_Manual_Control.Diver = SetBit(driver_Manual_Control.Diver, 3);
        if (UISwitch_set_FW[0].Active ==false )
        {
					driver_Manual_Control.Acttion = 0x00;
        }
				else
				{
					driver_Manual_Control.Acttion = 0x01;
				}
      driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
      temp = 	Main.frm1.StructToBytes(driver_Manual_Control);
      Main.frm1.Serial_Data_Transmission(temp,8,Main.frm1.RACK_ID_Number_Sent ,Main.frm1.Shelf_ID_Number_Sent,0x10);  //设置Divre状态
      driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
    }
		//FAN1设置
		public void UISwitch_set_FW1_Click()
    {
        byte[] temp = new byte[8];	
				driver_Manual_Control.Diver = 0;			
				driver_Manual_Control.Diver = SetBit(driver_Manual_Control.Diver, 7);
        if (UISwitch_set_FW[1].Active ==false )
        {
					driver_Manual_Control.Acttion = 0x00;
        }
        else
        {
					driver_Manual_Control.Acttion = 0x01;
        }
      driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
      temp = 	Main.frm1.StructToBytes(driver_Manual_Control);
			Main.frm1.Serial_Data_Transmission(temp,8,Main.frm1.RACK_ID_Number_Sent ,Main.frm1.Shelf_ID_Number_Sent,0x10);
      driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
    }
		//主正接触器设置
			public void UISwitch_set_FW2_Click()
    {
    
			byte[] temp = new byte[8];
      driver_Manual_Control.Diver = 0;
      driver_Manual_Control.Diver = SetBit(driver_Manual_Control.Diver, 0);
      if (UISwitch_set_FW[2].Active ==false )
			{
        driver_Manual_Control.Acttion = 0x00;
			}
			else
			{
        driver_Manual_Control.Acttion = 0x01;
      }
      driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
      temp = 	Main.frm1.StructToBytes(driver_Manual_Control);
      Main.frm1.Serial_Data_Transmission(temp,8,Main.frm1.RACK_ID_Number_Sent ,Main.frm1.Shelf_ID_Number_Sent,0x10);
      driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);

    }
			//LED2设置
    public void UISwitch_set_FW3_Click()
    {
        byte[] temp = new byte[8];
      driver_Manual_Control.Diver = 0;
      driver_Manual_Control.Diver = SetBit(driver_Manual_Control.Diver, 4);
      if (UISwitch_set_FW[3].Active ==false )
				{
        driver_Manual_Control.Acttion = 0x00;
      }
      else
      {
        driver_Manual_Control.Acttion = 0x01;

      }
      driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
      temp = 	Main.frm1.StructToBytes(driver_Manual_Control);
			Main.frm1.Serial_Data_Transmission(temp,8,Main.frm1.RACK_ID_Number_Sent ,Main.frm1.Shelf_ID_Number_Sent,0x10);
      driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
    }
		//FAN2设置
    public void UISwitch_set_FW4_Click()
    {
    
        byte[] temp = new byte[8];
        driver_Manual_Control.Diver = 0;
      driver_Manual_Control.Diver = SetBit(driver_Manual_Control.Diver, 8);

      if (UISwitch_set_FW[4].Active ==false )
				{
        driver_Manual_Control.Acttion = 0x00;
      }
      else
				{
        driver_Manual_Control.Acttion = 0x01;

      }
      driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
			temp = 	Main.frm1.StructToBytes(driver_Manual_Control);
			Main.frm1.Serial_Data_Transmission(temp,8,Main.frm1.RACK_ID_Number_Sent ,Main.frm1.Shelf_ID_Number_Sent,0x10);
			driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
    }
				//主负接触器
		public void UISwitch_set_FW5_Click()
    {
    
      byte[] temp = new byte[8];
      driver_Manual_Control.Diver = 0;
      driver_Manual_Control.Diver = SetBit(driver_Manual_Control.Diver, 1);
      if (UISwitch_set_FW[5].Active ==false )
      {
        driver_Manual_Control.Acttion = 0x00;
      }
      else
      {
        driver_Manual_Control.Acttion = 0x01;
      }
      driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
      temp = Main.frm1.StructToBytes(driver_Manual_Control);
      Main.frm1.Serial_Data_Transmission(temp,8,Main.frm1.RACK_ID_Number_Sent ,Main.frm1.Shelf_ID_Number_Sent,0x10);
      driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
    }
		//LED3设置
    public void UISwitch_set_FW6_Click()
    {
        byte[] temp = new byte[8];
				driver_Manual_Control.Diver = 0;			
				driver_Manual_Control.Diver = SetBit(driver_Manual_Control.Diver, 5);
				if (UISwitch_set_FW[6].Active ==false )
				{					
				driver_Manual_Control.Acttion = 0x00;
				}
				else
				{
				driver_Manual_Control.Acttion = 0x01;
				}
				
				driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
        temp = 	Main.frm1.StructToBytes(driver_Manual_Control);
				Main.frm1.Serial_Data_Transmission(temp,8,Main.frm1.RACK_ID_Number_Sent ,Main.frm1.Shelf_ID_Number_Sent,0x10);
  			driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
    }
		//FAN3
		public void UISwitch_set_FW7_Click()
    {
    
        byte[] temp = new byte[8];
				
				driver_Manual_Control.Diver = 0;	
				driver_Manual_Control.Diver = SetBit(driver_Manual_Control.Diver, 9);
				if (UISwitch_set_FW[7].Active ==false )
				{					
				driver_Manual_Control.Acttion = 0x00;
				}
				else
				{
				driver_Manual_Control.Acttion = 0x01;
				}
				
				driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
        temp = 	Main.frm1.StructToBytes(driver_Manual_Control);
				Main.frm1.Serial_Data_Transmission(temp,8,Main.frm1.RACK_ID_Number_Sent ,Main.frm1.Shelf_ID_Number_Sent,0x10);
        driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
    }
		//预充接触器
		public void UISwitch_set_FW8_Click()
    {
    
        byte[] temp = new byte[8];
				
				driver_Manual_Control.Diver = 0;				
				driver_Manual_Control.Diver = SetBit(driver_Manual_Control.Diver, 2);
				if (UISwitch_set_FW[8].Active ==false )
				{					
				driver_Manual_Control.Acttion = 0x00;
				}
				else
				{
				driver_Manual_Control.Acttion = 0x01;
				}
				driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
        temp = 	Main.frm1.StructToBytes(driver_Manual_Control);
				Main.frm1.Serial_Data_Transmission(temp,8,Main.frm1.RACK_ID_Number_Sent ,Main.frm1.Shelf_ID_Number_Sent,0x10);
        driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
    }
    //LED4
    public void UISwitch_set_FW9_Click()
    {

      byte[] temp = new byte[8];
      driver_Manual_Control.Diver = 0;
      driver_Manual_Control.Diver = SetBit(driver_Manual_Control.Diver, 6);
      if (UISwitch_set_FW[9].Active == false)
      {
        driver_Manual_Control.Acttion = 0x00;
      }
      else
      {
        driver_Manual_Control.Acttion = 0x01;
      }
      driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
      temp = Main.frm1.StructToBytes(driver_Manual_Control);
      Main.frm1.Serial_Data_Transmission(temp, 8, Main.frm1.RACK_ID_Number_Sent, Main.frm1.Shelf_ID_Number_Sent, 0x10);
      driver_Manual_Control.Diver = Reverse(driver_Manual_Control.Diver);
    }
    //一键使能设置
    private void Button_Eanble_Click()
    {
     
    }
		//将第index位设为1
		 public static short SetBit(short b, int index) 
		{ 
			return (short)(b | (1 << index)); 
		}
		 //将index位设置位0
		 public static short ClearBit(short b, int index) 
		{ 
			return (short)(b & (short.MaxValue - (1 << index))); 
		}
    private static short Reverse(short value)
    {
      return (short)((((int)value & 0xFF) << 8) | (int)((value >> 8) & 0xFF));
    }
  }
}
