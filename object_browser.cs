using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace object_browser
{
    public partial class object_browser : Form
    {
        public object_browser()
        {
            InitializeComponent();
        }

        
        Assembly asm;
        MethodInfo[] methods;
        PropertyInfo[] properties;
        FieldInfo[] fields;
        Type t;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            t = asm.GetType(listBox1.SelectedItem.ToString());
            methods = t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (MethodInfo m in methods)
            {
                if (!m.IsSpecialName)
                {
                    listBox2.Items.Add(m.Name);
                }
            }
            properties = t.GetProperties();
            foreach (PropertyInfo p in properties)
            {
                listBox3.Items.Add(p.Name);
            }
            fields = t.GetFields(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo f in fields)
            {
                listBox4.Items.Add(f.Name);
            }
        }
        MethodInfo method;
        ParameterInfo[] parameters;
        List<TextBox> li;
        int y = 120;
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            label1.Text = null;
            method = t.GetMethod(listBox2.SelectedItem.ToString());
            parameters = method.GetParameters();
            foreach (TextBox tx in li)
            {
                this.Controls.Remove(tx);
            }
            li.Clear();
            for (int i = 0; i < parameters.Length; i++)
            {
                TextBox tx = new TextBox();
                tx.Location = new Point(442, y);
                li.Add(tx);
                this.Controls.Add(tx);
                y = y + 30;
            }
            foreach (ParameterInfo p in parameters)
            {
                label1.Text += p.Name + p.ParameterType + Environment.NewLine;
            }
        }
        Object o;
        PropertyInfo p;
        TextBox tx;
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            o = Activator.CreateInstance(t);
            p = t.GetProperty(listBox3.SelectedItem.ToString());
            label1.Text = p.PropertyType.ToString();
            if (p.CanRead)
            {
                tx = new TextBox();
                tx.Location = new Point(442, 130);
                this.Controls.Add(tx);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (p.GetIndexParameters().Length == 0)
            {
                p.SetValue(o, "ducat", null);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(p.GetValue(o).ToString());
        }
        MemberInfo[] members;
        private void button5_Click(object sender, EventArgs e)
        {
            members = t.GetMembers();
            foreach (MemberInfo m in members)
            {
                listBox5.Items.Add(m.Name);
            }
        }



        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            object o = Activator.CreateInstance(t);
            if (parameters.Length == 0)
            {
                method.Invoke(o, null);
            }
            else
            {
                Object[] values = new Object[parameters.Length];
                for (int j = 0; j < values.Length; j++)
                {
                    values[j] = Convert.ChangeType(li[j].Text, parameters[j].ParameterType);
                }
                if (method.ReturnType.Name != "void")
                {
                    MessageBox.Show(method.Invoke(o, values).ToString());
                }
                else
                {
                    method.Invoke(o, values);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "dll files|*.dll|exe file|*.exe";
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
            asm = Assembly.LoadFrom(textBox1.Text);
            label1.Text = asm.FullName;
            Type[] types = asm.GetTypes();
            foreach (Type t in types)
            {
                listBox1.Items.Add(t.FullName);
            }
        }

       private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
   }
}
