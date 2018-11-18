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
using System.Xml.Serialization;
using Microsoft.Win32;

namespace Lab_4_C
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            notifyIcon1.Visible = false;
            ShowInTaskbar = false;
            Closing += this.OnWindowClosing;
            Load += this.OnWindowLoad;
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
            textBox.DoDragDrop(textBox.Text, DragDropEffects.Copy);
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = (string)e.Data.GetData(DataFormats.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.DragEnter += new DragEventHandler(textBox1_DragEnter);
            textBox2.MouseDown += new MouseEventHandler(textBox1_MouseDown);
            textBox2.DragDrop += new DragEventHandler(textBox1_DragDrop);

            object[] sc = new object[Enum.GetValues(typeof(Shortcut)).Length];
            Enum.GetValues(typeof(Shortcut)).CopyTo(sc, 0);
//            cmbShortCuts.Items.AddRange(sc);

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                ComboBox combo = new ComboBox();
                TextBox textBox = new TextBox();
                Label label = new Label();
                textBox.Width = 185;
                label.Location = new Point(190, 4);
                label.Text = "PIN 2";

                panel1.Controls.Add(textBox);
                panel1.Controls.Add(label);

                textBox.KeyPress += new KeyPressEventHandler(this.textBox5_KeyPress);

            }
            else
            {
                panel1.Controls.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
                Button btn = new Button();
                Control prev = (Control)this.Controls[this.Controls.Count - 1];
                //Устанавливаем позицию добавляемых кнопок
                int height = prev.Height;
                int width = prev.Width;
                btn.Location = new Point(width - 15,height + 1);
                //Добавляем событие для новой кнопки и обработчик button1_Click
                btn.Click += new EventHandler(button1_Click);
                //Устанавливаем свойство Text кнопки
                btn.Text = "Clone";
                //Добавляем экземпляр в коллекцию
                this.Controls.Add(btn);
                panel3.Controls.Add(btn);
                //Определяем обработчик для события MouseUp экземпляра кнопки btn
                btn.MouseUp += new MouseEventHandler(button1_MouseUp);
        }

        private void button1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //Удаляем данную кнопку
                panel3.Controls.Remove((Control)sender);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Если значение текстового поля пустое, выводим сообщение
            if (txtMenuText.Text == "")
            {
                //Текст сообщения
                MessageBox.Show("Введите текст для пункта меню");
                return;
            }

            this.comboBox1.Items.AddRange(new object[] {txtMenuText.Text});

            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Выход") {
                this.Close();
            }
            if (comboBox1.Text == "Изменить цвет")
            {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.BackColor = colorDialog1.Color;
                }
            }
            else if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.BackColor = colorDialog1.Color;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (char.IsDigit(e.KeyChar))
            {
                errorProvider1.SetError(textBox6, "Must be letter");
                label_err.Text = "Поле Name не может содержать цифры";
            }

            
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                label_err.Text = "Поле Name не может содержать цифры";
            }
        }
        
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                errorProvider1.SetError(textBox5, "Must be number");
                label_err.Text = "Поля PIN и PIN2 не могут содержать буквы";
            }

            /*
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                label_err.Text = "Поле PIN не может содержать буквы";
            }*/
        }
    
        private void textBox5_Validating(object sender, CancelEventArgs e)
        {
            if (textBox5.Text == "")
            {
                e.Cancel = false;
            }
            else
            {
                try
                {
                    double.Parse(textBox5.Text);
                    e.Cancel = false;

                }
                catch
                {
                    e.Cancel = true;
                    label_err.Text = "Поле PIN не может содержать буквы!";
                }
            }
        }

        private void textBox3_Validated(object sender, EventArgs e)
        {
            if (nameValid())
            {
                // Все правильно, удаляем сообщение с надписи
                errorProvider1.SetError(textBox3, "");
            }
            else
            {
                //Поле не заполнено — выводим сообщение
                errorProvider1.SetError(textBox3, "Name is required.");
                lbloutput.Text = "Введите имя!";
            }

        }

        private void numUDAge_Validated(object sender, EventArgs e)
        {
            if (ageLess())
            {
                // Введенное значение  меньше 25
                errorProvider1.SetError(numUDAge, "Age not old enough");
                lbloutput.Text = "Введите значение, большее или равное  25";
            }
            else if (ageMore())
            {
                /// Введенное значение  больше 25
                errorProvider1.SetError(numUDAge, "Age is too old");
                lbloutput.Text = "Введите значение, меньшее или равное  65";
            }
            else
            {
                // Все правильно, удаляем сообщение с надписи
                errorProvider1.SetError(numUDAge, "");
            }

        }


        private bool nameValid()
        {
            // Проверяем заполнение текствого поля
            return (textBox3.Text.Length > 0);
        }

        private bool ageLess()
        {
            //Возраст меньше 25
            return (numUDAge.Value < 25);
        }

        private bool ageMore()
        {
            //Возраст больше 25
            return (numUDAge.Value > 65);
        }

        private void mnuShow_Click(object sender, EventArgs e)
        {
            //Включаем отображения приложения на панели задач при запуске
            notifyIcon1.Visible = true;
            this.ShowInTaskbar = true;
            //Отключаем доступность пункта меню mnuShow
            mnuShow.Enabled = false;
            //Включаем доступность пункта меню mnuHide
            mnuHide.Enabled = true;
            //Переменной Hidden устанавливаем значение false
        }

        private void mnuHide_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            notifyIcon1.Visible = false;
            mnuShow.Enabled = true;
            mnuHide.Enabled = false;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            mnuShow_Click(this, new EventArgs());
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            /*
            FormSize frmSize = new FormSize();
            frmSize.height = this.Height;
            frmSize.width = this.Width;
            //Открываем раздел RegApplication
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey
         ("SOFTWARE\\Microsoft\\RegApplication", true);
            //Если раздел не обнаружен, создаем его
            if (regkey == null)
            {
                RegistryKey newregkey = Registry.CurrentUser.OpenSubKey
                   ("SOFTWARE\\Microsoft", true);
                regkey = newregkey.CreateSubKey("RegApplication");
            }
            //Записываем значения ширины и высоты формы в ключи Height и Width
            regkey.SetValue("Height", frmSize.height);
            regkey.SetValue("Width", frmSize.width);
            */

            //Создаем экземпляр frmSize класса FormSize:
            FormSize frmSize = new FormSize();
            // Присваиваем текущие значения высоты и ширины формы 
            //переменным height и width
            frmSize.height = this.Height;
            frmSize.width = this.Width;
            //Cоздаем экземпляр  xmlser класса XmlSerializer
            XmlSerializer xmlser = new XmlSerializer(typeof(FormSize));
            //Создаем переменную filename, которой присваиваем 
            //название файла applicationSettings.xml в текущей директории
            string filename = System.Environment.CurrentDirectory +
                   "\\applicationSettings.xml";
            //Создаем поток filestream для создания XML-файла
            FileStream filestream = new FileStream(filename, FileMode.Create);
            //Создаем сериализацию для экземпляра frmSize
            xmlser.Serialize(filestream, frmSize);
            //Закрываем поток
            filestream.Close();

        }

        private void OnWindowLoad(object sender, EventArgs e)
        {
            /*
            FormSize frmSizeSetup = new FormSize();
            //Открываем раздел реестра
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\RegApplication");
            //Получаем значения ключей Height и Width
            frmSizeSetup.height = Convert.ToInt32(regkey.GetValue("Height"));
            frmSizeSetup.width = Convert.ToInt32(regkey.GetValue("Width"));
            //Устанавливаем текущие значения ширины и высоты формы
            this.Height = frmSizeSetup.height;
            this.Width = frmSizeSetup.width;
            */

            //Создаем экземпляр frmSizeSetup класса FormSize:
            FormSize frmSizeSetup = new FormSize();
            //Cоздаем экземпляр  xmlser класса XmlSerializer
            XmlSerializer xmlser = new XmlSerializer(typeof(FormSize));
            //Создаем переменную filename, которой присваиваем 
            //название файла applicationSettings.xml в текущей директории
            string filename = System.Environment.CurrentDirectory +
                            "\\applicationSettings.xml";
            //Создаем поток filestream для чтения XML-файла
            FileStream filestream = new FileStream(filename, FileMode.Open);
            //Экземпляру frmSizeSetup передаем данные,
            //полученные в процессе десериализации
            frmSizeSetup = (FormSize)xmlser.Deserialize(filestream);
            //Устанавливаем  текущие высоту и ширину формы
            this.Height = frmSizeSetup.height;
            this.Width = frmSizeSetup.width;
            //Закрываем поток
            filestream.Close();
        }
    }
}




public class FormSize
{
    public int height;
    public int width;
}
