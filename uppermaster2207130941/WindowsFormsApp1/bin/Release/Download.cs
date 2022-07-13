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
      frm_2 = this;
      button_download.Enabled = false;
    }
    /************************************************************************		
    *name: 所有初始化选项
    *************************************************************************/
    #region
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
    #endregion
    /************************************************************************		
    *name: 所有按键操作
    *************************************************************************/
    #region

    public void progressBa_value(int form1_value)
    {
      progressBar1.Value = form1_value;
    }

    private void button_download_Click_1(object sender, EventArgs e)
    {
      button_OenBin.Enabled = false;
      var temp1 = new byte[3] { 0x03, 0x20, 0x20 };
      Main.frm1.SendPacketAck(temp1, 3);
      progressBar1.Value = 0;
      var temp_1 = new byte[11];
      var temp_2 = new byte[9] { 0x21, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x09, 0x3C };
      temp_2[5] = (byte)(Form_DownLoad.frm_2.file_len >> 24 & 0xFF);
      temp_2[6] = (byte)(Form_DownLoad.frm_2.file_len >> 16 & 0xFF);
      temp_2[7] = (byte)(Form_DownLoad.frm_2.file_len >> 8 & 0xFF);
      temp_2[8] = (byte)(Form_DownLoad.frm_2.file_len & 0xFF);

      temp_1[0] = 0x0B;
      temp_1[1] = getCheckSum_1(temp_2, 9);

      Array.ConstrainedCopy(temp_2, 0, temp_1, 2, 9);
      Main.frm1.SendPacketAck(temp_1, 11);
      Main.frm1.textBox1.AppendText("communication establishment" + "\r\n");
      Thread.Sleep(5);

      temp_1 = new byte[4] { 0x03, 0x23, 0x23, 0xCC };
      Main.frm1.SendPacketAck(temp_1, 4);
      Main.frm1.textBox1.AppendText("Packet acknowledgment successful" + "\r\n");
      Thread.Sleep(5);

      new Thread(Main.frm1.Send_Data).Start();
      Main.frm1.textBox1.AppendText("start transferring data" + "\r\n");
      Main.frm1.textBox1.AppendText("data transfer completed " + "\r\n");
      Form_DownLoad.frm_2.progressBa_value(3);
      Thread.Sleep(5);
      Main.frm1.signle = 0;
      Main.frm1.count_ACK = 1;

    }

    private void button_OenBin_Click(object sender, EventArgs e)
    {
      Main.frm1.openFileDialog1.Filter = "*.bin|*.BIN|*.hex|*.HEX";
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

  }
}
