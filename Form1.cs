using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
namespace WindowsFormsC_
{
    public partial class Form1 : Form
    {
        public string fileName = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string defaultPath = "C:\\Users\\V.Kudinov\\source\\repos\\WindowsFormsC#\\bin\\Debug\\";
            string fileName = Path.Combine(defaultPath, "users.xml"); //используется для объединения пути к файлу

            // Загрузка файла только если fileName не пусто
            if (File.Exists(fileName))
            {
                // Файл  найден по пути по умолчанию
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                XmlNodeList nodes = doc.SelectNodes("users/user"); // пример выбора всех узлов 

                string login = textBoxLogin.Text;
                string password = textBoxPassword.Text;
                bool userFound = false;

                foreach (XmlNode node in nodes)
                {
                    string Nodelogin = node.SelectSingleNode("username").InnerText; // получение значения атрибута "name"
                    string Nodepassword = node.SelectSingleNode("password").InnerText;

                    if (login == Nodelogin && password == Nodepassword)
                    {
                        userFound = true;
                        break;
                    }

                }
                if (userFound)
                {
                    try
                    {
                        Form2 form2 = new Form2();
                        form2.Show();
                        this.Hide();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Не верный пользователь или пароль");
                }
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {

            string fileName;
            try
            {
                fileName = "C:\\Users\\V.Kudinov\\source\\repos\\WindowsFormsC#\\bin\\Debug\\users.xml";//Определяем имя файла, в который будут записываться данные.
                if (File.Exists(fileName)) // Проверяем существование файла
                {
                    XmlDocument doc = new XmlDocument();//Создаем объект XmlDocument
                    doc.Load(fileName);//загружаем содержимое файла в память
                    XmlNode root = doc.DocumentElement;//Получаем корневой элемент XML-документа

                    // Проверяем, есть ли уже в файле запись с таким логином (текст из текстового поля textBoxLogin). 
                    XmlNode existingUser = root.SelectSingleNode($"user[username='{textBoxLogin.Text}']");
                    if (existingUser != null)//Если такая запись уже есть выводим сообщение об ошибке и завершаем выполнение метода.
                    {
                        MessageBox.Show("Такой пользователь уже существует");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Пользователь создан");
                    }
                    XmlNode userNode = doc.CreateElement("user");//Создаем новый элемент XML-документа с тегом "user"

                    XmlNode usernameNode = doc.CreateElement("username");// Создаем новый элемент "username" и записываем в него логин пользователя
                    usernameNode.InnerText = textBoxLogin.Text;//Значения этих элементов берутся из соответствующих текстовых полей textBoxLogin

                    XmlNode passwordNode = doc.CreateElement("password");//Создаем новый элемент "password" и записываем в него пароль пользователя
                    passwordNode.InnerText = textBoxPassword.Text;//Значения этих элементов берутся из соответствующих текстовых полей textBoxPassword 

                    userNode.AppendChild(usernameNode);// Добавляем элементы "username" в фаил(элемент) "user"
                    userNode.AppendChild(passwordNode);// Добавляем элементы "password" в фаил(элемент) "user"

                    root.AppendChild(userNode);//Добавляем новый элемент "user" в корневой элемент XML-документа

                    doc.Save(fileName);//сохраняем изменения в файл
                }
                else
                {
                    DialogResult result = MessageBox.Show("Файл users.xml не найден. Создать файл?", "Создание файла", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                        saveFileDialog.FileName = "users";

                        fileName = saveFileDialog.FileName + ".xml";
                        XmlDocument doc = new XmlDocument();
                        XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                        doc.AppendChild(xmlDeclaration);
                        XmlNode root = doc.CreateElement("users");
                        doc.AppendChild(root);
                        doc.Save(fileName);
                        MessageBox.Show("Файл users.xml успешно создан");

                        if (result == DialogResult.Yes)
                        {
                            OpenFileDialog openFileDialog = new OpenFileDialog();
                            openFileDialog.Filter = "XML files (*.xml)|*.xml";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Приложение продолжит работу без создания файла");
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }
    }
}
