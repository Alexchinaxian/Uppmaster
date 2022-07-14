using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Sunny.UI;
using Microsoft.VisualBasic;
namespace WindowsFormsApp1
{
    public partial class Shelf3 : Form
    {
        public static Shelf3 frm_Shelf1;

        public TextBox[] Textbox_Shelf1 = new TextBox[18];
        public TextBox[] Textbox_Shelf2 = new TextBox[18];
        public TextBox[] Textbox_Shelf3 = new TextBox[18];
        public TextBox[] Textbox_Shelf4 = new TextBox[18];
        public TextBox[] Textbox_Shelf5 = new TextBox[18];
        public TextBox[] Textbox_Shelf6 = new TextBox[18];
        public TextBox[] Textbox_Summary = new TextBox[26];
        public TextBox[] Textbox_Set_parameters = new TextBox[24];

        public Label[] labell_Shelf1 = new Label[18];
        public Label[] labell_Shelf2 = new Label[18];
        public Label[] labell_Shelf3 = new Label[18];
        public Label[] labell_Shelf4 = new Label[18];
        public Label[] labell_Shelf5 = new Label[18];
        public Label[] labell_Shelf6 = new Label[18];
        public Label[] labell_Summary = new Label[38];
        public Label[] labell_Set_parameters = new Label[24];
        public Label[] Label_fault = new Label[36];
        public UILight[] lights = new UILight[13];
        public UILight[] fault_lights = new UILight[36];
        public Shelf3()
        {
            InitializeComponent();
            Shelf1_load_init();
            Shelf2_load_init();
            shelf3_load_init();
            shelf4_load_init();
            shelf5_load_init();
            shelf6_load_init();
            Summary_Load_init();
            Fault_list_Init();
            frm_Shelf1 = this;
        }

        #region
        public const int Summary_Label = 80;
        public const int Fault_Label = 100;
        public const int Width_Label = 30;  //标签宽度
        public const int Height_Label = 18; // 标签高度
        public const int Width_Text = 50;   // 文本框宽度
        public const int Height_Text = 18;  //文本框高度
        public const int Width_Panel = 60; //容器宽度
        public const int Light_initial_position_1 = 120;    //灯位置
        public const int Light_initial_position_2 = 250;    //灯   
        /************************************************************************		
        *name:  Shelf1_load_init
        *describe: RACK1_shelf1界面内容的显示初始化
        *data : 2022.4.14 Alex
        *************************************************************************/
        public void Shelf1_load_init()
        {
            int i = 0; //列
            for (i = 0; i < 18; i++)
            {
                //标签的创建
                Textbox_Shelf1[i] = new TextBox();
                labell_Shelf1[i] = new Label();
                labell_Shelf1[i].Name = "Label_Volatage" + i.ToString();
                labell_Shelf1[i].Width = Width_Label;

                labell_Shelf1[i].Height = Height_Label;
                labell_Shelf1[i].Parent = uiGroupBox1;
                labell_Shelf1[i].Location = new Point(10, 32 + i * (15 + Height_Label));
                labell_Shelf1[i].Font = new Font("微软雅黑", Textbox_Shelf1[i].Font.Size, Textbox_Shelf1[i].Font.Style);
                //文本框的创建
                Textbox_Shelf1[i].Name = "Voltage_Battery" + i.ToString();
                Textbox_Shelf1[i].Text = "-";
                Textbox_Shelf1[i].Width = Width_Text;
                Textbox_Shelf1[i].Height = Height_Text;
                Textbox_Shelf1[i].Location = new Point(45, 30 + i * (15 + Height_Label));
                Textbox_Shelf1[i].Parent = uiGroupBox1;
                Textbox_Shelf1[i].ReadOnly = true;
                Textbox_Shelf1[i].Font = new Font("微软雅黑", Textbox_Shelf1[i].Font.Size, Textbox_Shelf1[i].Font.Style);
                //标签文本的特殊处理
                if (i < 11)
                {
                    labell_Shelf1[i].Text = "B" + (i + 1).ToString();
                }
                else if (i > 10 && i < 15)
                {
                    labell_Shelf1[i].Text = "T" + (i - 10).ToString();
                }
                else if (i > 14 && i < 17)
                {
                    labell_Shelf1[i].Text = "DT" + (i - 14).ToString();
                }
                else
                {
                    labell_Shelf1[i].Text = "BT" + (i - 16).ToString();
                }
                this.uiGroupBox1.Width = Width_Panel;
                this.uiGroupBox1.Height = 389;
                this.uiGroupBox1.Text = "Module1";
            }
        }
        /************************************************************************		
        *name:  Shelf2_load_init
        *describe: RACK2_shelf1界面内容的显示初始化
        *data : 2022.4.14 Alex
        *************************************************************************/
        public void Shelf2_load_init()
        {
            int i = 0;
            //Shlef2
            for (i = 0; i < 18; i++)
            {
                //创建标签与显示窗体
                Textbox_Shelf2[i] = new TextBox();
                labell_Shelf2[i] = new Label();
                labell_Shelf2[i].Name = "Labe2_Volatage" + i.ToString();
                labell_Shelf2[i].Width = Width_Label;
                labell_Shelf2[i].Height = Height_Label;
                labell_Shelf2[i].Parent = uiGroupBox2;
                labell_Shelf2[i].Location = new Point(10, 32 + i * (15 + Height_Label));
                Textbox_Shelf2[i].Name = "Voltage_Battery" + i.ToString();
                Textbox_Shelf2[i].Text = "-";
                Textbox_Shelf2[i].Width = Width_Text;
                Textbox_Shelf2[i].Height = Height_Text;
                Textbox_Shelf2[i].Location = new Point(45, 30 + i * (15 + Height_Label));
                Textbox_Shelf2[i].Parent = uiGroupBox2;
                Textbox_Shelf2[i].ReadOnly = true;
                if (i < 11)
                {
                    labell_Shelf2[i].Text = "B" + (i + 1).ToString();
                }
                else if (i > 10 && i < 15)
                {
                    labell_Shelf2[i].Text = "T" + (i - 10).ToString();
                }
                else if (i > 14 && i < 17)
                {
                    labell_Shelf2[i].Text = "DT" + (i - 14).ToString();
                }
                else
                {
                    labell_Shelf2[i].Text = "BT" + (i - 16).ToString();
                }
                this.uiGroupBox2.Width = Width_Panel;
                this.uiGroupBox2.Height = 389;
                this.uiGroupBox2.Text = "Module2";
            }
        }
        /************************************************************************		
        *name:  Shelf3_load_init
        *describe: RACK1_shelf3界面内容的显示初始化
        *data : 2022.4.14 Alex
        *************************************************************************/
        public void shelf3_load_init()
        {
            int i = 0;
            //Shlef2
            for (i = 0; i < 18; i++)
            {
                //创建标签与显示窗体
                Textbox_Shelf3[i] = new TextBox();
                labell_Shelf3[i] = new Label();
                labell_Shelf3[i].Name = "Labe2_Volatage" + i.ToString();

                labell_Shelf3[i].Width = Width_Label;
                labell_Shelf3[i].Height = Height_Label;
                labell_Shelf3[i].Parent = uiGroupBox3;
                labell_Shelf3[i].Location = new Point(10, 32 + i * (15 + Height_Label));

                Textbox_Shelf3[i].Name = "Voltage_Battery" + i.ToString();
                Textbox_Shelf3[i].Text = "-";
                Textbox_Shelf3[i].Width = Width_Text;
                Textbox_Shelf3[i].Height = Height_Text;
                Textbox_Shelf3[i].Location = new Point(45, 30 + i * (15 + Height_Label));
                Textbox_Shelf3[i].ReadOnly = true;
                Textbox_Shelf3[i].Parent = uiGroupBox3;
                if (i < 11)
                {
                    labell_Shelf3[i].Text = "B" + (i + 1).ToString();
                }
                else if (i > 10 && i < 15)
                {
                    labell_Shelf3[i].Text = "T" + (i - 10).ToString();
                }
                else if (i > 14 && i < 17)
                {
                    labell_Shelf3[i].Text = "DT" + (i - 14).ToString();
                }
                else
                {
                    labell_Shelf3[i].Text = "BT" + (i - 16).ToString();
                }
                this.uiGroupBox3.Width = Width_Panel;
                this.uiGroupBox3.Height = 389;
                this.uiGroupBox3.Text = "Module3";

            }
        }
        /************************************************************************		
        *name:  Shelf4_load_init
        *describe: RACK1_shelf4界面内容的显示初始化
        *data : 2022.4.14 Alex
        *************************************************************************/
        public void shelf4_load_init()
        {
            int i = 0;
            //Shlef2
            for (i = 0; i < 18; i++)
            {
                //创建标签与显示窗体
                Textbox_Shelf4[i] = new TextBox();
                labell_Shelf4[i] = new Label();
                labell_Shelf4[i].Name = "Labe2_Volatage" + i.ToString();

                labell_Shelf4[i].Width = Width_Label;
                labell_Shelf4[i].Height = Height_Label;
                labell_Shelf4[i].Parent = uiGroupBox4;
                labell_Shelf4[i].Location = new Point(10, 32 + i * (15 + Height_Label));

                Textbox_Shelf4[i].Name = "Voltage_Battery" + i.ToString();
                Textbox_Shelf4[i].Text = "-";
                Textbox_Shelf4[i].Width = Width_Text;
                Textbox_Shelf4[i].Height = Height_Text;
                Textbox_Shelf4[i].Location = new Point(45, 30 + i * (15 + Height_Label));
                Textbox_Shelf4[i].Parent = uiGroupBox4;
                Textbox_Shelf4[i].ReadOnly = true;
                if (i < 11)
                {
                    labell_Shelf4[i].Text = "B" + (i + 1).ToString();
                }
                else if (i > 10 && i < 15)
                {
                    labell_Shelf4[i].Text = "T" + (i - 10).ToString();
                }
                else if (i > 14 && i < 17)
                {
                    labell_Shelf4[i].Text = "DT" + (i - 14).ToString();
                }
                else
                {
                    labell_Shelf4[i].Text = "BT" + (i - 16).ToString();
                }
                this.uiGroupBox4.Width = Width_Panel;
                this.uiGroupBox4.Height = 389;
                this.uiGroupBox4.Text = "Module4";

            }
        }
        /************************************************************************		
        *name:  Shelf5_load_init
        *describe: RACK1_shelf5界面内容的显示初始化
        *data : 2022.4.14 Alex
        *************************************************************************/
        public void shelf5_load_init()
        {
            int i = 0;
            //Shlef2
            for (i = 0; i < 18; i++)
            {
                //创建标签与显示窗体
                Textbox_Shelf5[i] = new TextBox();
                labell_Shelf5[i] = new Label();
                labell_Shelf5[i].Name = "Labe2_Volatage" + i.ToString();

                labell_Shelf5[i].Width = Width_Label;
                labell_Shelf5[i].Height = Height_Label;
                labell_Shelf5[i].Parent = uiGroupBox5;
                labell_Shelf5[i].Location = new Point(10, 32 + i * (15 + Height_Label));

                Textbox_Shelf5[i].Name = "Voltage_Battery" + i.ToString();
                Textbox_Shelf5[i].Text = "-";
                Textbox_Shelf5[i].Width = Width_Text;
                Textbox_Shelf5[i].Height = Height_Text;
                Textbox_Shelf5[i].Location = new Point(45, 30 + i * (15 + Height_Label));
                Textbox_Shelf5[i].ReadOnly = true;
                Textbox_Shelf5[i].Parent = uiGroupBox5;
                if (i < 11)
                {
                    labell_Shelf5[i].Text = "B" + (i + 1).ToString();
                }
                else if (i > 10 && i < 15)
                {
                    labell_Shelf5[i].Text = "T" + (i - 10).ToString();
                }
                else if (i > 14 && i < 17)
                {
                    labell_Shelf5[i].Text = "DT" + (i - 14).ToString();
                }
                else
                {
                    labell_Shelf5[i].Text = "BT" + (i - 16).ToString();
                }
                this.uiGroupBox5.Width = Width_Panel;
                this.uiGroupBox5.Height = 389;
                this.uiGroupBox5.Text = "Module5";

            }
        }

        /************************************************************************		
        *name:  Shelf6_load_init
        *describe: RACK1_shelf6界面内容的显示初始化
        *data : 2022.4.14 Alex
        *************************************************************************/
        public void shelf6_load_init()
        {
            int i = 0;
            //Shlef2
            for (i = 0; i < 18; i++)
            {
                //创建标签与显示窗体
                Textbox_Shelf6[i] = new TextBox();
                labell_Shelf6[i] = new Label();
                labell_Shelf6[i].Name = "Labe2_Volatage" + i.ToString();

                labell_Shelf6[i].Width = Width_Label;
                labell_Shelf6[i].Height = Height_Label;
                labell_Shelf6[i].Parent = uiGroupBox6;
                labell_Shelf6[i].Location = new Point(10, 32 + i * (15 + Height_Label));

                Textbox_Shelf6[i].Name = "Voltage_Battery" + i.ToString();
                Textbox_Shelf6[i].Text = "-";
                Textbox_Shelf6[i].Width = Width_Text;
                Textbox_Shelf6[i].Height = Height_Text;
                Textbox_Shelf6[i].Location = new Point(40, 30 + i * (15 + Height_Label));
                Textbox_Shelf6[i].ReadOnly = true;
                Textbox_Shelf6[i].Parent = uiGroupBox6;
                if (i < 11)
                {
                    labell_Shelf6[i].Text = "B" + (i + 1).ToString();
                }
                else if (i > 10 && i < 15)
                {
                    labell_Shelf6[i].Text = "T" + (i - 10).ToString();
                }
                else if (i > 14 && i < 17)
                {
                    labell_Shelf6[i].Text = "DT" + (i - 14).ToString();
                }
                else
                {
                    labell_Shelf6[i].Text = "BT" + (i - 16).ToString();
                }
                this.uiGroupBox6.Width = Width_Panel;
                this.uiGroupBox6.Height = 389;
                this.uiGroupBox6.Text = "Module6";
            }
        }
        /************************************************************************		
        *name:  Summary_Load_init
        *describe: 概要信息的的设计框
        *data : 2022.4.14 Alex
        *************************************************************************/
        public void Summary_Load_init()
        {
            int i = 0;//行
            int k = 0;

            //状态灯的初始化
            Button set = new Button();
            for (k = 0; k < 11; k++)
            {
                lights[k] = new Sunny.UI.UILight();
                lights[k].Name = "LED" + i.ToString();
                lights[k].Height = 20;
                lights[k].Parent = uiGroupBox7;
                lights[k].OnColor = Color.Gray;
            }
            // 设置界面的按钮的初始化
            set.Name = "Button_set";
            set.Text = "Parameter setting";
            set.Width = 160;
            set.Height = 50;
            set.Font = new Font("Leelawadee", 12F);
            set.Location = new Point(170, 32 + 16 * (15 + Height_Label));
            set.Click += delegate
            {
                Buttonset_Click();
            };
            set.Parent = uiGroupBox7;

            //概要显示文本的初始化
            for (int j = 0; j < 25; j++)
            {
                Textbox_Summary[j] = new TextBox();
                if (j < 15)
                {
                    Textbox_Summary[j].Location = new Point(100, 30 + j * (13 + Height_Label));
                    Textbox_Summary[j].Width = Width_Text;
                }
                else
                {
                    Textbox_Summary[j].Location = new Point(250, 30 + (j - 8) * (13 + Height_Label));
                    Textbox_Summary[j].Width = 105;
                }
                Textbox_Summary[j].Name = "Voltage_Battery" + i.ToString();
                Textbox_Summary[j].Text = "-";

                Textbox_Summary[j].Height = Height_Text;
                Textbox_Summary[j].Name = "Labe2_Volatage" + i.ToString();
                Textbox_Summary[j].Parent = uiGroupBox7;

            }
            //标签的初始化设置
            for (i = 0; i < 36; i++)
            {
                labell_Summary[i] = new Label();

                if (i < 19)
                {
                    labell_Summary[i].Location = new Point(10, 32 + i * (13 + Height_Label));
                }
                else
                {
                    labell_Summary[i].Location = new Point(160, 32 + (i - 19) * (13 + Height_Label));
                }
                labell_Summary[i].Width = Summary_Label;
                labell_Summary[i].Height = Height_Label;
                labell_Summary[i].Parent = uiGroupBox7;
                //特殊处理名字 以及灯的位置
                if (i < 3)
                {
                    labell_Summary[i].Text = "TotalVol_" + (i + 1).ToString();
                }
                else if (i == 3)
                {
                    labell_Summary[i].Text = "Current";
                }
                else if (i == 4)
                {
                    labell_Summary[i].Text = "MaxVol";
                }
                else if (i == 5)
                {
                    labell_Summary[i].Text = "MaxVolID";
                }
                else if (i == 6)
                {
                    labell_Summary[i].Text = "MinVol";
                }
                else if (i == 7)
                {
                    labell_Summary[i].Text = "MinVolID";
                }
                else if (i == 8)
                {
                    labell_Summary[i].Text = "MaxTemp";
                }
                else if (i == 9)
                {
                    labell_Summary[i].Text = "MaxTempID";
                }
                else if (i == 10)
                {
                    labell_Summary[i].Text = "MinTemp";
                }
                else if (i == 11)
                {
                    labell_Summary[i].Text = "MinTempID";
                }
                else if (i == 12)
                {
                    labell_Summary[i].Text = "SOC";
                }
                else if (i == 13)
                {
                    labell_Summary[i].Text = "SOH";
                }
                else if (i == 14)
                {
                    labell_Summary[i].Text = "BalanceVol";
                }
                else if (i == 15)
                {
                    labell_Summary[i].Text = "LED1_Status";
                    lights[0].Location = new Point(100, 35 + (i) * (13 + Height_Label));
                }
                else if (i == 16)
                {
                    labell_Summary[i].Text = "LED2_Status";
                    lights[1].Location = new Point(100, 35 + (i) * (13 + Height_Label));
                }
                else if (i == 17)
                {
                    labell_Summary[i].Text = "LED3_Status";
                    lights[2].Location = new Point(100, 35 + (i) * (13 + Height_Label));
                }
                else if (i == 18)
                {
                    labell_Summary[i].Text = "LED4_Status";
                    lights[3].Location = new Point(100, 32 + i * (13 + Height_Label));
                }
                else if (i == 19)
                {
                    labell_Summary[i].Text = "FAN1_Status";
                    lights[4].Location = new Point(Light_initial_position_2, 32 + (i - 19) * (13 + Height_Label));
                }
                else if (i == 20)
                {
                    labell_Summary[i].Text = "FAN2_Status";
                    lights[5].Location = new Point(Light_initial_position_2, 32 + (i - 19) * (13 + Height_Label));
                }
                else if (i == 21)
                {
                    labell_Summary[i].Text = "FAN3_Status";
                    lights[6].Location = new Point(Light_initial_position_2, 32 + (i - 19) * (13 + Height_Label));
                }
                else if (i == 22)
                {
                    labell_Summary[i].Text = "MPC_Status";
                    lights[7].Location = new Point(Light_initial_position_2, 32 + (i - 19) * (13 + Height_Label));
                }
                else if (i == 23)
                {
                    labell_Summary[i].Text = "MNC_Status";
                    lights[8].Location = new Point(Light_initial_position_2, 32 + (i - 19) * (13 + Height_Label));
                }
                else if (i == 24)
                {
                    labell_Summary[i].Text = "PCC_Status";
                    lights[9].Location = new Point(Light_initial_position_2, 32 + (i - 19) * (13 + Height_Label));
                }
                else if (i == 25)
                {
                    labell_Summary[i].Text = "Lock_Status";
                    lights[10].Location = new Point(Light_initial_position_2, 32 + (i - 19) * (13 + Height_Label));
                }
                else if (i == 26)
                {
                    labell_Summary[i].Text = "Shelf_Mode";
                }
                else if (i == 27)
                {
                    labell_Summary[i].Text = "M1_Balance";
                }
                else if (i == 28)
                {
                    labell_Summary[i].Text = "M2_Balance";
                }
                else if (i == 29)
                {
                    labell_Summary[i].Text = "M3_Balance";
                }
                else if (i == 30)
                {
                    labell_Summary[i].Text = "M4_Balance";
                }

                else if (i == 31)
                {
                    labell_Summary[i].Text = "M5_Balance";
                }
                else if (i == 32)
                {
                    labell_Summary[i].Text = "M6_Balance";
                }
                else if (i == 33)
                {
                    labell_Summary[i].Text = "FW version";
                }
                else if (i == 34)
                {
                    labell_Summary[i].Text = "HW version";
                }
                else if (i == 35)
                {
                    labell_Summary[i].Text = "System Time";
                }

                {
                    //None
                }
                this.uiGroupBox7.Width = 220;
                this.uiGroupBox7.Height = 389;
                this.uiGroupBox7.Text = "Summary";

            }
        }
        /************************************************************************		
        *name:  Fault_list_Init
        *describe: 故障页的显示
        *data : 2022.4.14 Alex
        *************************************************************************/
        public void Fault_list_Init()
        {
            for (int i = 0; i < 24; i++)
            {
                Label_fault[i] = new Label();
                Label_fault[i].Width = 90;
                Label_fault[i].Height = Height_Label;
                Label_fault[i].Parent = uiGroupBox8;

                fault_lights[i] = new Sunny.UI.UILight();
                fault_lights[i].Name = "LED" + i.ToString();
                fault_lights[i].Height = 20;
                fault_lights[i].Width = 20;
                fault_lights[i].Parent = uiGroupBox8;
                fault_lights[i].OnColor = Color.Gray;
                if (i < 18)
                {
                    Label_fault[i].Location = new Point(5, 32 + i * (15 + Height_Label));
                    fault_lights[i].Location = new Point(100, 32 + i * (15 + Height_Label));
                }
                else
                {
                    Label_fault[i].Location = new Point(130, 32 + (i - 18) * (15 + Height_Label));
                    fault_lights[i].Location = new Point(225, 32 + (i - 18) * (15 + Height_Label));
                }
            }
            //标签文本的特殊做
            Label_fault[0].Text = "Cell_OV";
            Label_fault[1].Text = "Battery_OV";
            Label_fault[2].Text = "Chg_OCur";
            Label_fault[3].Text = "Chg_UTemp";
            Label_fault[4].Text = "Chg_OTemp";
            Label_fault[5].Text = "Cell_UV";
            Label_fault[6].Text = "Battery_UV";
            Label_fault[7].Text = "DisChg_OCur";
            Label_fault[8].Text = "DisChg_UTemp";
            Label_fault[9].Text = "DisChg_OTemp";
            Label_fault[10].Text = "SOC_Low";
            Label_fault[11].Text = "V_Diff";
            Label_fault[12].Text = "T_Diff";
            Label_fault[13].Text = "Inter CAN Fault";
            Label_fault[14].Text = "MPC Stuck Fault";
            Label_fault[15].Text = "MNC Stuck Fault";
            Label_fault[16].Text = "PCC Stuck Fault";
            Label_fault[17].Text = "MPC Open Fault";
            Label_fault[18].Text = "MNC Open Fault";
            Label_fault[19].Text = "PCC Open Fault";
            Label_fault[20].Text = "Precharge Fault";
            Label_fault[21].Text = "FAN Fault";
            Label_fault[22].Text = "TM4C-MSP";
            Label_fault[23].Text = "MSP-BQ";
            this.uiGroupBox8.Width = 150;
            this.uiGroupBox8.Height = 389;
            this.uiGroupBox8.Text = "Fault List";
        }
        #endregion

        #region

        /************************************************************************		
        *name:  RACK1_Shelf1_Data_reception
        *describe: 数据显示
        *data : 2022.4.14 Alex
        *************************************************************************/
        public void RACK1_Shelf1_Moudle1_Data_reception(short buff)
        {
            for (int i = 0; i < 18; i++)
            {
                float ftemp = (float)buff / 10;
                Textbox_Shelf1[i].Text = (ftemp).ToString("#0.0");
            }
        }
        public void RACK1_Shelf1_Moudle2_Data_reception(byte[] buff)
        {
            for (int i = 0; i < 18; i++)
            {
                float ftemp = (float)buff[i] / 10;
                Textbox_Shelf2[i].Text = (ftemp).ToString("#0.0");
            }

        }
        public void RACK1_Shelf1_Moudle3_Data_reception(byte[] buff)
        {
            for (int i = 0; i < 18; i++)
            {
                float ftemp = (float)buff[i] / 10;
                Textbox_Shelf3[i].Text = (ftemp).ToString("#0.0");
            }

        }
        public void RACK1_Shelf1_Moudle4_Data_reception(byte[] buff)
        {
            for (int i = 0; i < 18; i++)
            {
                float ftemp = (float)buff[i] / 10;
                Textbox_Shelf4[i].Text = (ftemp).ToString("#0.0");
            }

        }
        public void RACK1_Shelf1_Moudle5_Data_reception(byte[] buff)
        {
            for (int i = 0; i < 18; i++)
            {
                float ftemp = (float)buff[i] / 10;
                Textbox_Shelf5[i].Text = (ftemp).ToString("#0.0");
            }
        }

        public void RACK1_Shelf1_Moudle6_Data_reception(byte[] buff)
        {
            for (int i = 0; i < 18; i++)
            {
                float ftemp = (float)buff[i] / 10;
                Textbox_Shelf6[i].Text = (ftemp).ToString("#0.0");
            }
        }

        public void RACK1_Shelf1_Summary()
        {

            //  Textbox_Summary[21].Text = (Main.frm1.RACK1_Shelf1.MBB6_Balance_Stas).ToString();
        }
        #endregion


        #region


        /************************************************************************		
        *name:  set_bit
        *describe: 设置位函数
        *data : 2022.4.18 Alex
        * "data"  需要设置的byte数据
        * "index" 要设置的位， 值从低到高为 1-8
        * "flag"  要设置的值 true / false
        *************************************************************************/
        public static byte set_bit(byte data, int index, bool flag)
        {
            if (index > 8 || index < 1)
                throw new System.ArgumentOutOfRangeException();

            int v = index < 2 ? index : (2 << (index - 2));
            return flag ? (byte)(data | v) : (byte)(data & ~v);
        }

        /************************************************************************		
        *name:  Buttonset_Click
        *describe: 设置页面的按键
        *data : 2022.4.18 Alex
        *************************************************************************/
        private void Buttonset_Click()
        {
            string PM121 = Interaction.InputBox("Please enter password", "enter password", "", 100, 100);
            if (PM121 != "123456")
            {
                MessageBox.Show("Please enter the correct password thank you!");
                return;
            }
            else
            {
                Set f = new Set();
                f.ShowDialog();
            }
        }
        #endregion

        private void uiGroupBox8_Click(object sender, System.EventArgs e)
        {

        }

        private void uiGroupBox2_Click(object sender, System.EventArgs e)
        {

        }

        private void uiGroupBox7_Click(object sender, System.EventArgs e)
        {

        }

        private void uiGroupBox6_Click(object sender, System.EventArgs e)
        {

        }

        private void uiGroupBox5_Click(object sender, System.EventArgs e)
        {

        }

        private void uiGroupBox4_Click(object sender, System.EventArgs e)
        {

        }

        private void uiGroupBox1_Click(object sender, System.EventArgs e)
        {

        }

        private void uiGroupBox3_Click(object sender, System.EventArgs e)
        {

        }
    }
}
