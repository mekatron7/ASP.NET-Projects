using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormAppPractice
{
    public partial class Form1 : Form
    {
        private int retryCounter;
        private string userFName = "Lian";
        private string userlName = "Xiang";

        class TestClass
        {
            public string Name { get; set; }

            public TestClass()
            {
                Name = "Gabe";
            }
        }

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "dssdds";
            textBox1.Enabled = new bool();
            var button1 = new Button();
            button1.Size = new Size(70, 70);
            button1.Location = new Point(30, 30);
            button1.Text = "Click me nigga";
            Controls.Add(button1);
            button1.Click += button1_Click;
            listBox1.Items.Add("Test");
            listBox1.Items.Add(new TestClass().Name);
            listBox1.Font = new Font(new FontFamily("Comic Sans MS"), 10);
            var a = listBox1.SelectedItem;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = DialogResult.Retry;
            while (result == DialogResult.Retry)
            {
                result = MessageBox.Show($"Aye yo {userFName} {userlName}, youse a bitch ass nigga!", "BAN Warning", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2);
                switch (result)
                {
                    case DialogResult.Abort:
                        MessageBox.Show($"{userFName}, you shoulda been aborted......BITCH!!!!", "Abortion Notice", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        this.Close();
                        break;
                    case DialogResult.Retry:
                        retryCounter++;
                        listBox1.Items.Add($"{userFName} {userlName} is a BITCH times {retryCounter}");
                        if (retryCounter >= 5) MessageBox.Show("Of course your bitchass had to retry again.", $"You retried {retryCounter} times like a dumb bitch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if(retryCounter == 20)
                        {
                            MessageBox.Show($"{userFName} get the fuck outta here stupid BITCH! Wastin my muhfuckin time retrying like a big fat bitch. Bitch get the fuck up on outta here bitch lol.", $"{userFName} {userlName} you a RETARDED BITCH on God.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            result = DialogResult.Abort;
                            this.Close();
                        }
                        break;
                    case DialogResult.Ignore:
                        MessageBox.Show("Bitch!!!", $"Hey {userFName} you're a...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        var result2 = MessageBox.Show("BITCH ._.", "Reminder, you're a", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                        switch (result2)
                        {
                            case DialogResult.Yes:
                                MessageBox.Show("Aight bitch, youse a bitchass bitch.", "Bitch Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                break;
                            case DialogResult.No:
                                MessageBox.Show("Look bitch. A bitch you are, a bitch you be. But bitch, are you too bitchy...aight bitch are you too bitchy...for a bitchass bitch like you bitch???", "Serious Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                MessageBox.Show("Yooooooo bitch you just got GOT bitch!!", "OOOOHHHHHHHHH!!!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                break;
                            case DialogResult.Cancel:
                                MessageBox.Show($"{userFName} you need a nice tall glass of shuttin...the fuck...UP...BITCH!!!!", "STFU Bitch Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                break;
                        }
                        break;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show("fsssdsdd");
            listBox1.SelectionMode = SelectionMode.MultiExtended;
            MessageBox.Show(listBox1.SelectedItems[retryCounter].ToString(), listBox1.SelectedIndex.ToString());
        }
    }
}
