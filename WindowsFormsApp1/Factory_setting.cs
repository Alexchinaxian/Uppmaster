using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Factory_setting : Form
    {
    public static Factory_setting factory_Setting;
    public Factory_setting()
    {
        InitializeComponent();
        Set_RACK_ShlefID();
        factory_Setting = this;
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
    const int Number_Line_2 = 11;//每列的行数设置
    const int Row_Spacing = 10; //行间距设置
    const int Top_distance = 45; //顶部距离
    public TextBox[] Textbox_set_FW = new TextBox[60]; //设备设置文本框
    public TextBox[] Textbox_Read_Calibration = new TextBox[48];  //校准参数读取文本框
    public TextBox[] Textbox_set_Calibration = new TextBox[60]; //校准参数设置文本框
    public Label[] label_set_ID = new Label[60];	//设备状态文本
    public Button[] buttons_set = new Button[60];//设置按钮
    public struct Set_Moudle_Request
    {
      public byte ID;
      public byte RESERVED1;
      public byte RESERVED2;
      public byte RESERVED3;
      public byte RESERVED4;
      public byte RESERVED5;
      public byte RESERVED6;
      public byte RESERVED7;
    };
    Set_Moudle_Request GetSet_Moudle_Request1;
public void Set_RACK_ShlefID()
{
        for (int i = 0; i < 11; i++)
        {
            Textbox_Read_Calibration[i] = new TextBox();
            label_set_ID[i] = new Label();
            Textbox_set_Calibration[i] = new TextBox();
            buttons_set[i] = new Button();

            label_set_ID[i].Name = "Labe2_Volatage" + i.ToString();
            label_set_ID[i].Width = Width_Label;
            label_set_ID[i].Height = Height_Label;
            label_set_ID[i].Parent = panel_Factory;
            label_set_ID[i].Font = new Font("微软雅黑", 9F);

            buttons_set[i].Name = "buttons_set" + i.ToString();
            buttons_set[i].Text = "Set";
            buttons_set[i].Width = Width_Text;
            buttons_set[i].Height = Height_Button;
            buttons_set[i].Parent = panel_Factory;
            buttons_set[i].Font = new Font("微软雅黑", 9F);
								

            Textbox_set_Calibration[i].Name = "Textbox_set_Calibration" + i.ToString();
            Textbox_set_Calibration[i].Width = Width_Text;
            Textbox_set_Calibration[i].Height = Height_Text;
            Textbox_set_Calibration[i].Parent = panel_Factory;
            Textbox_set_Calibration[i].Font = new Font("微软雅黑", 9F);

            Textbox_Read_Calibration[i].Name = "Textbox_Read_Calibration" + i.ToString();
            Textbox_Read_Calibration[i].Width = Width_Text;
            Textbox_Read_Calibration[i].Height = Height_Text;
            Textbox_Read_Calibration[i].Parent = panel_Factory;
            Textbox_Read_Calibration[i].Font = new Font("微软雅黑", 9F);
            Textbox_Read_Calibration[i].ReadOnly = true;
            if (i < Number_Line_2)
            {
                label_set_ID[i].Location = new Point(10, Top_distance + i * (Row_Spacing + Height_Label));
                Textbox_Read_Calibration[i].Location = new Point(190, Top_distance + i * (Row_Spacing + Height_Label));
                Textbox_set_Calibration[i].Location = new Point(250, Top_distance + i * (Row_Spacing + Height_Label));
                buttons_set[i].Location = new Point(320,  Top_distance + i * (Row_Spacing + Height_Label));
            }
            else if (i < (2 * Number_Line_2))
            {
                label_set_ID[i].Location = new Point(310, Top_distance + (i - Number_Line_2) * (Row_Spacing + Height_Label));
                Textbox_Read_Calibration[i].Location = new Point(490, Top_distance + (i - Number_Line_2) * (Row_Spacing + Height_Label));
                Textbox_set_Calibration[i].Location = new Point(550, Top_distance + (i - Number_Line_2) * (Row_Spacing + Height_Label));
            }

            else if (i < (3 * Number_Line_2))
            {
                label_set_ID[i].Location = new Point(610, Top_distance + (i - 2 * Number_Line_2) * (Row_Spacing + Height_Label));
                Textbox_Read_Calibration[i].Location = new Point(790, Top_distance + (i - 2 * Number_Line_2) * (Row_Spacing + Height_Label));
                Textbox_set_Calibration[i].Location = new Point(850, Top_distance + (i - 2 * Number_Line_2) * (Row_Spacing + Height_Label));
            }
            else if (i < 4 * Number_Line_2)
            {
                label_set_ID[i].Location = new Point(910, Top_distance + (i - 3 * Number_Line_2) * (Row_Spacing + Height_Label));
                Textbox_Read_Calibration[i].Location = new Point(1090, Top_distance + (i - 3 * Number_Line_2) * (Row_Spacing + Height_Label));
                Textbox_set_Calibration[i].Location = new Point(1150, Top_distance + (i - 3 * Number_Line_2) * (Row_Spacing + Height_Label));
            }
            else
            {
                //None
            }

            if (i == 0)
            {
              label_set_ID[i].Text = "Rack ID";
            }
            else if (i == 1)
            {
              label_set_ID[i].Text = "BMS SN";
            }
            else if (i == 2)
            {
              label_set_ID[i].Text = "Cell Type";
            }
            else if (i == 3)
            {
              label_set_ID[i].Text = "Parallel Config";
            }
            else if (i == 4)
            {
              label_set_ID[i].Text = "Serial Config";
            }
            else if (i == 5)
            {
              label_set_ID[i].Text = "Temperature Config";
            }
            else if (i == 6)
            {
              label_set_ID[i].Text = "Rated Capacity"+"(0.1Ah)";
            }
            else if (i == 7)
            {
              label_set_ID[i].Text = "Rated Voltage"+"(0.1V)";
            }
            else if (i==8)
            {
              label_set_ID[i].Text = "Project Code";
            }
            else if(i == 9)
            {
                label_set_ID[i].Text = "RTC";
            }
            else if(i == 10)
            {
              label_set_ID[i].Text = "Read All";
              buttons_set[i].Text = "Read";
              Textbox_set_Calibration[i].ReadOnly = true;

            }
            else
            {
               //None
            }
        }
			  buttons_set[0].Click += delegate
			  {
				  Button_0_Set();
			  };
        buttons_set[1].Click += delegate
        {
          Button_1_Set();
        };
        buttons_set[2].Click += delegate
        {
          Button_2_Set();
        };
        buttons_set[1].Click += delegate
        {
          Button_3_Set();
        };
        buttons_set[4].Click += delegate
        {
          Button_4_Set();
        };
        buttons_set[5].Click += delegate
        {
          Button_5_Set();
        };
        buttons_set[6].Click += delegate
        {
          Button_6_Set();
        };
        buttons_set[7].Click += delegate
        {
          Button_7_Set();
        };
        buttons_set[8].Click += delegate
        {
          Button_8_Set();
        };
        buttons_set[9].Click += delegate
        {
          Main.frm1.Buttons_RTC_Set();
        };
      buttons_set[10].Click += delegate
      {
        Main.frm1.Read_FactoryVauleClick();
      };
    this.panel_Factory.Text = "Factory Setting";
    }
		private void Button_0_Set()
		{
			byte[] temp = new byte[8];
      if (Textbox_set_Calibration[0].Text != string.Empty)
      {
        GetSet_Moudle_Request1.ID = Convert.ToByte(Textbox_set_Calibration[0].Text);
        temp = Main.frm1.StructToBytes(GetSet_Moudle_Request1);
        Main.frm1.Serial_Data_Transmission(temp, 8, 0xFF, 0xFF, 0xFF);  //设置Divre状态
      }
      else
      {
        //
      }
    }
    private void Button_1_Set()
		{
		  byte[] temp = new byte[8];

		  GetSet_Moudle_Request1.ID = Convert.ToByte (Textbox_set_Calibration[0].Text);
      temp = 	Main.frm1.StructToBytes(GetSet_Moudle_Request1);
      Main.frm1.Serial_Data_Transmission(temp,8,0x01,0x01,0xF2);  //设置Divre状态
		}
    private void Button_2_Set()
    { }
    private void Button_3_Set()
    { }
    private void Button_4_Set()
    { }
    private void Button_5_Set()
    { }
    private void Button_6_Set()
    { }
    private void Button_7_Set()
    { }
    private void Button_8_Set()
    { }
    private void Button_10_Set()
    {

    }

  }
}
