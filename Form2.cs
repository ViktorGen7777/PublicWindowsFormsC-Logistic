using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;

namespace WindowsFormsC_
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Load += Form2_Load; // Добавление обработчика к событию Form.Load
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            // Очистка таблицы перед загрузкой XML данных
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            // Очистка таблицы перед загрузкой XML данных
            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();

            // Указывается путь к файлу XML
            string defaultPath = "C:\\Users\\V.Kudinov\\source\\repos\\WindowsFormsC#\\bin\\Debug\\";
            string fileName = Path.Combine(defaultPath, "DocumentsAccept.xml"); //используется для объединения пути к файлу

            // Указывается путь к файлу XML
            string defaultPath1 = "C:\\Users\\V.Kudinov\\source\\repos\\WindowsFormsC#\\bin\\Debug\\";
            string fileName1 = Path.Combine(defaultPath, "DocumentsAccept2.xml"); //используется для объединения пути к файлу

            // Создание нового DataSet
            DataSet dataset = new DataSet();
            // Чтение данных из XML файла в DataSet
            dataset.ReadXml(fileName);
            // Проверка наличия пустой строки в конце XML-файла
            if (dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
            {
                DataRow lastRow = dataset.Tables[0].Rows[dataset.Tables[0].Rows.Count - 1];
                bool isLastRowEmpty = true;

                foreach (var item in lastRow.ItemArray)
                {
                    if (item != null && !string.IsNullOrEmpty(item.ToString()))
                    {
                        isLastRowEmpty = false;
                        break;
                        // get out from cycle
                    }
                }

                if (isLastRowEmpty)
                {
                    dataset.Tables[0].Rows.RemoveAt(dataset.Tables[0].Rows.Count - 1);
                }
            }
            // Устанавливается источник данных для DataGridView как первая таблица в DataSet
            dataGridView1.DataSource = dataset.Tables[0];

            // Создание нового DataSet для второго файла XML
            DataSet dataset2 = new DataSet();
            // Чтение данных из второго XML файла в DataSet
            dataset2.ReadXml(fileName1);
            // Проверка наличия пустой строки в конце XML-файла
            if (dataset2.Tables.Count > 0 && dataset2.Tables[0].Rows.Count > 0)
            {
                DataRow lastRow2 = dataset2.Tables[0].Rows[dataset2.Tables[0].Rows.Count - 1];
                bool isLastRowEmpty2 = true;

                foreach (var item in lastRow2.ItemArray)
                {
                    if (item != null && !string.IsNullOrEmpty(item.ToString()))
                    {
                        isLastRowEmpty2 = false;
                        break;
                    }
                }

                if (isLastRowEmpty2)
                {
                    dataset2.Tables[0].Rows.RemoveAt(dataset2.Tables[0].Rows.Count - 1);
                }
            }
            // Устанавливается источник данных для dataGridView2 как первая таблица в DataSet2
            dataGridView2.DataSource = dataset2.Tables[0];
        }



        private DataTable GetDataTableFromDGV(DataGridView dgv)
        {
            // Создание новой DataTable
            var dt = new DataTable();
            // Добавление столбцов в DataTable на основе видимых столбцов в DataGridView
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (column.Visible)
                {
                    dt.Columns.Add();
                }
            }
            // Создание массива для хранения значений ячеек в каждой строке
            object[] cellValues = new object[dgv.Columns.Count];
            // Обход всех строк в DataGridView
            foreach (DataGridViewRow row in dgv.Rows)
            {
                // Заполнение массива cellValues значениями ячеек в текущей строке
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    cellValues[i] = row.Cells[i].Value;
                }
                // Добавление строки с значениями ячеек в DataTable
                dt.Rows.Add(cellValues);
            }
            // Возврат DataTable
            return dt;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Получение DataTable из DataGridView с помощью метода GetDataTableFromDGV
            DataTable dt = GetDataTableFromDGV(dataGridView1);
            DataTable dt2 = GetDataTableFromDGV(dataGridView2);
            // Установка названий колонок в DataTable
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                if (column.Visible)
                {
                    dt.Columns[column.Index].ColumnName = column.HeaderText;
                }
            }
            // Установка названий колонок в DataTable для dataGridView2
            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                if (column.Visible)
                {
                    dt2.Columns[column.Index].ColumnName = column.HeaderText;
                }
            }
            // Создание новых DataSet
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            DataSet ds2 = new DataSet();
                ds2.Tables.Add(dt2);
                // Запись DataSet в XML файл
                string filePath = @"C:\Users\V.Kudinov\source\repos\WindowsFormsC#\bin\Debug\DocumentsAccept.xml";
                try
                {
                    // Создание XmlWriterSettings для настройки кодировки
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Encoding = Encoding.UTF8;

                    using (XmlWriter writer = XmlWriter.Create(filePath, settings))
                    {
                        ds.WriteXml(writer, XmlWriteMode.WriteSchema);
                    }
                    if (File.Exists(filePath))
                    {
                        MessageBox.Show("Данные сохранены в DocumentsAccept1");
                    }
                }
                catch (Exception ex)
                {
                    // Отображение сообщения об ошибке
                    MessageBox.Show("Ошибка сохранения файла: " + ex.Message);
                }
                // Запись DataSet в XML файл для dataGridView2
                string filePath2 = @"C:\Users\V.Kudinov\source\repos\WindowsFormsC#\bin\Debug\DocumentsAccept2.xml";
                try
                {
                    // Создание XmlWriterSettings для настройки кодировки
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Encoding = Encoding.UTF8;

                    using (XmlWriter writer = XmlWriter.Create(filePath2, settings))
                    {
                        ds2.WriteXml(writer, XmlWriteMode.WriteSchema);
                    }

                    if (File.Exists(filePath2))
                    {
                        MessageBox.Show("Данные сохранены в DocumentsAccept2");
                    }
                }
                catch (Exception ex)
                {
                    // Отображение сообщения об ошибке
                    MessageBox.Show("Ошибка сохранения файла для dataGridView2: " + ex.Message);
                }
            }
        }
    }

