#define Download_Hex   //预编译  使用Hex文件还是bin文件
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Sunny.UI;
using System.Globalization;

namespace WindowsFormsApp1
{
  public partial class Form_DownLoad : Form
  {
    public static Form_DownLoad frm_2;
    public int file_len;//bin文件长度
    public Form_DownLoad()
    {
      InitializeComponent();
      Download_select_Rack();
      Download_select_Shelf();
      Slect_Chip();
      frm_2 = this;
      button_download.Enabled = false;
    }
    /************************************************************************		
    *name: 所有初始化选项
    *************************************************************************/
    #region
    String szHexPath = "";
    private void Download_select_Rack()
    {
      string[] moudle_number = { "1", "2", "3" };

      comboBox1.Items.AddRange(moudle_number);
    }
    private void Download_select_Shelf()
    {
      string[] Shelf_number = { "1", "2", "3", "4", "5", "6", "7", "8" };

      comboBox_Moudlenumber.Items.AddRange(Shelf_number);
    }
    private void Slect_Chip()
    {
      string[] Chip_Name = { "TM4C", "MSP430" };
      uiComboBox1_Chip.Items.AddRange(Chip_Name);
    }
    #endregion
    /************************************************************************		
    *name: 所有按键操作
    *************************************************************************/
    #region

    public void ProgressBa_value(int form1_value)
    {
      progressBar1.Value = form1_value;
    }

    private void button_download_Click_1(object sender, EventArgs e)
    {
      button_OenBin.Enabled = false;
      //芯片选择
      if (uiComboBox1_Chip.Text.Equals("TM4C")) //TM4C 更新程序
      {
        //pingTM4C芯片
        var temp1 = new byte[7] { 0x01, 0x03, 0x60,0x01,0x01,0x11,0x22 };
        Main.frm1.SendPacketAck(temp1, 7);
        //需要下载的地址和文件大小
        var temp_2 = new byte[16] { 0x01, 0x10, 0x60, 0x03, 0x03, 0x06,0x00,0x00, 0x40, 0x00,0x00, 0x00, 0x09, 0x3C,0x11,0x22 };
        temp_2[9] = (byte)(Form_DownLoad.frm_2.file_len >> 24 & 0xFF);
        temp_2[10] = (byte)(Form_DownLoad.frm_2.file_len >> 16 & 0xFF);
        temp_2[11] = (byte)(Form_DownLoad.frm_2.file_len >> 8 & 0xFF);
        temp_2[12] = (byte)(Form_DownLoad.frm_2.file_len & 0xFF);
        Main.frm1.SendPacketAck(temp_2, 16);

        Thread.Sleep(5);
        //确认下载地址
        temp_2 = new byte[7] { 0x01, 0x03, 0x60,0x03,0x03,0x11, 0x22 };
        Main.frm1.SendPacketAck(temp_2, 7);
        Main.frm1.textBox1.AppendText("Packet acknowledgment successful" + "\r\n");
        Thread.Sleep(5);

        new Thread(Main.frm1.Send_Data).Start();
      }
      else if (uiComboBox1_Chip.Text.Equals("MSP430")) //更新MSP430
      {

        //pingMSP芯片
        var temp1 = new byte[7] { 0x01, 0x03, 0x60, 0x01, 0x01, 0x11, 0x22 };
        Main.frm1.SendPacketAck(temp1, 7);
        //需要下载的地址和文件大小
        var temp_2 = new byte[16] { 0x01, 0x10, 0x60, 0x03, 0x03, 0x06, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x09, 0x3C, 0x11, 0x22 };
        temp_2[9] = (byte)(Form_DownLoad.frm_2.file_len >> 24 & 0xFF);
        temp_2[10] = (byte)(Form_DownLoad.frm_2.file_len >> 16 & 0xFF);
        temp_2[11] = (byte)(Form_DownLoad.frm_2.file_len >> 8 & 0xFF);
        temp_2[12] = (byte)(Form_DownLoad.frm_2.file_len & 0xFF);
        Main.frm1.SendPacketAck(temp_2, 16);

        Thread.Sleep(5);
        //确认下载地址
        temp_2 = new byte[7] { 0x01, 0x03, 0x60, 0x03, 0x03, 0x11, 0x22 };
        Main.frm1.SendPacketAck(temp_2, 7);
        Main.frm1.textBox1.AppendText("Packet acknowledgment successful" + "\r\n");
        Thread.Sleep(5);

        new Thread(Main.frm1.Send_Data).Start();
      }
      else
      {
        MessageBox.Show("Please select the chip model to be upgraded" + "\r\n");
      }

    }
    /* 
     * 下载文件更改为使用Hex文件
     */
#if Download_bin

    private void button_OenBin_Click(object sender, EventArgs e)
    {
      Main.frm1.openFileDialog1.Filter = "*.bin|*.BIN";
      if (Main.frm1.openFileDialog1.ShowDialog() == DialogResult.OK)
      {
        textBox_OpenFile.Text = Main.frm1.openFileDialog1.FileName;

        int addr = 0;//地址从0x00000000开始
        int count = 0;//换行显示计数
        byte[] binchar = new byte[] { };

        FileStream Myfile = new FileStream(Main.frm1.openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
        BinaryReader binreader = new BinaryReader(Myfile);

        file_len = (int)Myfile.Length;//获取bin文件长度

        StringBuilder str = new StringBuilder();

        binchar = binreader.ReadBytes(file_len);

        foreach (byte j in binchar)
        {
          if (count % 16 == 0)
          {
            count = 0;
            if (addr > 0)
            {
              str.Append("\r\n");
              str.Append(addr.ToString("x8") + "      ");
              addr++;
            }
          }
          str.Append(j.ToString("X2") + " ");
          if (count == 8)
          {
            str.Append("  ");
            count++;
          }
        }
        textBox_tail.Text = str.ToString();
        binreader.Close();
        button_download.Enabled = true;
        
      }
    }

#elif Download_Hex
    private void button_OenBin_Click(object sender, EventArgs e)
    {
      Main.frm1.openFileDialog1.Filter = "*.hex|*.HEX";
      if (Main.frm1.openFileDialog1.ShowDialog() == DialogResult.OK)
      {
        textBox_OpenFile.Text = Main.frm1.openFileDialog1.FileName;

        try
        {
          Int32 i;
          Int32 j = 0;
          String szLine = "";//Hex文件每行的数据
          String szHex = "";//bin文件的HEX字符串数据
          int startAdr = 0;//起始地址
          int endAdr = 0;   //用于判断hex地址是否连续，不连续补充0xFF
          int lineLenth;//当前行的数据长度
          bool FirstLineFlag;//起始行标志位
          bool FirstAddrFlag;//起始地址标志位
          szHexPath = textBox_OpenFile.Text;//HEX文件路径
    // 读取并处理需要转换的hex文件
          StreamReader HexReader = new StreamReader(szHexPath);
          FirstLineFlag = true;
          FirstAddrFlag = true;
          while (true)
          {
            szLine = HexReader.ReadLine(); //读取Hex中一行
            if (szLine == null) { break; } //读取完毕，退出
            if (szLine.Substring(0, 1) == ":") //判断首字符是”:”
            {
              if (szLine.Substring(7, 2) == "00")//数据记录
              {
                lineLenth = Int32.Parse(szLine.Substring(1, 2), System.Globalization.NumberStyles.HexNumber); // 获取一行的数据个数值
                startAdr = Int32.Parse(szLine.Substring(3, 4), System.Globalization.NumberStyles.HexNumber); // 获取地址值

                if (FirstAddrFlag)
                {
                  endAdr = startAdr;
                  FirstAddrFlag = false;
                }

                if (startAdr == 0x1A90)
                {
                  ;
                }
                for (i = 0; i < startAdr - endAdr; i++) // 补空位置
                {
                  szHex += "FF";
                }
                szHex += szLine.Substring(9, lineLenth * 2); //读取有效字符
                endAdr = startAdr + lineLenth;
              }
              else if (szLine.Substring(7, 2) == "04")//起始标识
              {
                if (FirstLineFlag)
                {
                  FirstLineFlag = false;
                }
              }
              else
              {
              }
            }
          }
          HexReader.Close(); //关闭目标文件
      // 将hex数据转换为字节数据
          Int32 Length = szHex.Length;
          byte[] szBin = new byte[Length / 2];
          int addr = 0;//地址从0x00000000开始
          int count = 0;//换行显示计数
          for (i = 0; i < Length; i += 2) //两字符合并成一个16进制字节
          {
            szBin[j] = (byte)Int16.Parse(szHex.Substring(i, 2), NumberStyles.HexNumber);
            j++;
          }
            StringBuilder str = new StringBuilder();

            foreach (byte temp in szBin)
          {
            if (count % 16 == 0)
          {
            if (addr > 0)
            {
              str.Append("\r\n");
              str.Append(addr.ToString("x8") + "      ");
              addr++;
            }
          }
          str.Append(temp.ToString("X2") + " ");
          if (count == 8)
          {
            str.Append("  ");
            count++;
          }
        }
        textBox_tail.Text = str.ToString();
      button_download.Enabled = true;

        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.ToString());
        }

      }
    }
#else

#endif
    #endregion
    //接口函数
    #region
    /**
    * 和校验
    * */

    public static byte getCheckSum_1(byte[] packBytes, int length)
    {
      UInt64 checkSum = 0x0000;
      for (int i = 0; i < length; i++)
      {
        checkSum += packBytes[i];//计算和校验
      }
      checkSum &= 0x00ff; //取低八位  

      return (byte)checkSum;
    }

    //Hex转Bin

    private StringBuilder DumpHex(string filePath)
    {
      StringBuilder HexStr = new StringBuilder("");
      using (Stream stream = new FileStream(filePath, FileMode.Open))
      {
        int bin;
        // textBox1.Text is Separator, b.ToString("X") is Hex


        while ((bin = stream.ReadByte()) != -1)
          HexStr.Append(textBox_tail.Text + bin.ToString("X"));
      }
      return HexStr;
    }

#endregion

    private void label1_Click(object sender, EventArgs e)
    {

    }
  }
}
