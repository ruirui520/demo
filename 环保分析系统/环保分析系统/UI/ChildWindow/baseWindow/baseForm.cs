﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 环保分析系统.except;

namespace 环保分析系统.UI.ChildWindow.baseWindow
{
    public partial class baseForm : Form
    {
        public baseForm()
        {
            InitializeComponent();
        }
        public baseForm(string[] feature, int limit,int rows)
        {
            this.features = feature;
            this.limit = limit;
            this.rows = rows;
            InitializeComponent();
        }
        public void SetFeatures(string[] features)
        {
            this.features = features;
        
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.splitContainer.Panel2Collapsed == false)
            {
                this.Height = this.smallhight;
                this.splitContainer.Panel2Collapsed = true;
                //this.splitContainer1.Panel2.Hide();
                
            }
            else
            {
               // this.splitContainer1.Panel2.Show();
                this.splitContainer.Panel2Collapsed = false;
                this.Height = this.bighight;
            }
           
        }

        public void baseForm_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                if (this.varList != null && this.features != null )
                {
                    SetFeatureToListBox(this.features, this.varList);
                }
                this.bighight = this.Height;
                this.smallhight = this.splitContainer.Panel1.Height + 50;
                this.Height = this.smallhight;
                this.splitContainer.Panel2Collapsed = true;
                SetTrainRow(this.rows);
            }
          
            //this.splitContainer1.Panel2.Hide();
           
        }
        protected virtual void SetTrainRow(int rows)
        {
            string str;
            int row = (int)Math.Floor(0.9*rows);
            str = "1" + "-" + row;
            this.trainrownum.Text = str;
            str = row + "-" + rows;
            this.predictrownum.Text = str;
            
        }
       public static void SetFeatureToListBox( string[] feature, ListBox listbox)
        {
                for (int i = 0; i < feature.Length; ++i)
                {  
                    listbox.Items.Add( feature[i]);
                }
        }


        //用于获取行号
        private static int[] GetRowNum( ref TextBox text)
        {
            string strtmp = text.Text;
            string[] strlist = strtmp.Split('-');
            if (strlist.Length!=2)
            {
                throw new InputException("行号输入格式有误");
            }

            int[] tmp = new int[strlist.Length];
            for (int i = 0; i < strlist.Length; ++i)
            {
                tmp[i] = int.Parse(strlist[i]);
            }

            for (int i = 1; i < strlist.Length; i++)
            {
                if (tmp[i]<tmp[i-1])
                {
                    throw new OrderException("行号范围输入有误");
                }
            }

            
            return tmp;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void reset_Click(object sender, EventArgs e)
        {
            ResetParm();
        }

        private void isAdvence_CheckedChanged(object sender, EventArgs e)
        {
            setAdvancedControlEnabled(this.isAdvence.Checked);
        }

    
        protected void onlyInputNum(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键
            if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) return;   //处理负数
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    MessageBox.Show("非法输入,请输入数字");   //处理非法字符
                    e.KeyChar = (char)0;
                }
            }
        }
        //state 表示checkbox 是否按下的bool值
        protected virtual void setAdvancedControlEnabled(bool state)
        {

            if (state == false)
            {
                MessageBox.Show("未选中");
            }
            else
            {
                MessageBox.Show("选中");
            }
        
        }

        private void timeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            onlyInputNum( sender,  e);
        }

        private void VarToVar( ListBox src, ListBox dst)
        {

            if (src.SelectedIndex>=0)
            {
                dst.Items.Add(src.Items[src.SelectedIndex]); 
                src.Items.Remove(src.Items[src.SelectedIndex]);
            }
           
        }
        private void ResetParm()
        {
            this.varList.Items.Clear();
            SetFeatureToListBox(this.features, this.varList);
            this.devarList.Items.Clear();
            this.indevarList.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            VarToVar(this.varList,  this.indevarList);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            VarToVar( this.indevarList, this.varList);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VarToVar( this.varList,  this.devarList);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            VarToVar(  this.devarList,this.varList);
        }

        protected string GetMethod()
        {
            return this.methodcomboBox.Text;
        }
        public int[] GetPredictRowNum()
        {
            return this.predictnum;
        
        }
        public int[] GetTrainRowNum()
        { 
		    return this.trainnum;
        }
        public int GetTimeLen()
        {
            return this.timelen;
        }
        protected bool IsAdvence()
        {
            return this.isAdvence.Checked;
        }

        private void CheckParm()
        {
            if (this.indevarList.Items.Count!=this.limit)
            {
                throw new IndexOutOfRangeException("因变量个数设置错误");

            }

            if (this.timeTextBox.Text==string.Empty)
            {
                throw new NullReferenceException("请设置时序长度");
            }
            else
            { 
                this.timelen = Convert.ToInt32(this.timeTextBox.Text);
            }
            try
            {
                this.trainnum = GetRowNum(ref this.trainrownum);
                this.predictnum = GetRowNum(ref this.predictrownum);
            }
            catch (Exception )
            {
                throw new IndexOutOfRangeException("训练行号或者预测行号设置错误"); ;
            }

            
           
            if (this.methodcomboBox.SelectedIndex<0&&this.IsAdvence()==false)
            {

                throw new NullReferenceException("请设置时序长度");
            }
            else
            { 
                this.timelen = Convert.ToInt32(this.timeTextBox.Text);

            }
        }
        protected virtual void SetParm(string method)
        {

            //this.trainnum = GetRowNum(ref this.trainrownum);
            //this.predictnum = GetRowNum(ref this.predictrownum);
           
            if (this.methodcomboBox.SelectedIndex<0&&this.IsAdvence()==false)
            {
                throw new NullReferenceException("请设置方案");
            }
        }
        //protected virtual void setParm(string method)
        //{
        //    MessageBox.Show(method);

        //}
        public string[] GetIndvFeature()
        {
            int size = this.indevarList.Items.Count;
            string[] tmpstr = new string[size];
            for (int i = 0; i < size; i++)
            {
                tmpstr[i] = this.indevarList.Items[i].ToString();
            }
            return tmpstr;
            
        }
     

//        public string[] GetIndvFeature()
//        {
////<<<<<<< HEAD
//            int size = this.indevarList.Items.Count;
//            string[] tmpstr = new string[size];
//            for (int i = 0; i < size; i++)
//            {
//                tmpstr[i] = this.indevarList.Items[i].ToString();
//            }
//            return tmpstr;
            
//        }
     


        private void begin_Click(object sender, EventArgs e)
        {

            try
            {
                CheckParm();
                if (this.IsAdvence() == false)
                {
                    string method = GetMethod();

                    SetParm(method);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (IndexOutOfRangeException ioe)
            {
                MessageBox.Show(ioe.Message);
            }
            catch (NullReferenceException nfe)
            {
                MessageBox.Show(nfe.Message);
            }
            catch (OrderException ore)
            {
                MessageBox.Show(ore.Message);
            }
            catch(InputException ie) 
            {
                MessageBox.Show(ie.Message);
            }
//=======
            
//              try 
//            {	        
//                CheckParm();
//                if (this.IsAdvence()==false)
//                {
//                    string method = GetMethod();

//                    setParm(method);
//                }
                
//                this.DialogResult = DialogResult.OK;
//                this.Close();
//            }
//            catch (IndexOutOfRangeException ioe)
//            {
//                MessageBox.Show(ioe.Message);
//            }
//              catch (NullReferenceException nfe)
//            {
//                MessageBox.Show(nfe.Message);
//            }
//>>>>>>> upstream/master
         
        }
       
    }
}
