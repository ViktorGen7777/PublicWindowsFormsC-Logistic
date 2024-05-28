using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;
namespace WindowsFormsC_
{
    public partial class Form1 : Form
    {
        public string fileName = string.Empty;
        public Form1()
        {
            InitializeComponent();
            CheckAndCreateTable();
        }
        private void CheckAndCreateTable()
        {
            string connectionString = "Server=DESKTOP-HQFVC8H\\MSSQLSERVER2022;Database=myDataBase;User Id=sa;Password=SQL2024";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string checkTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                        CREATE TABLE Users (
                            Id INT PRIMARY KEY IDENTITY,
                            Username NVARCHAR(50) NOT NULL,
                            Password NVARCHAR(50) NOT NULL
                        );

                       IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Warehouses')
            CREATE TABLE Warehouses (
                WarehouseId INT PRIMARY KEY IDENTITY,
                WarehouseName NVARCHAR(100) NOT NULL,
                Location NVARCHAR(100) NOT NULL
            );

            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products')
            CREATE TABLE Products (
                ProductId INT PRIMARY KEY IDENTITY,
                ProductName NVARCHAR(100) NOT NULL,
                SupplierId INT,
                FOREIGN KEY (SupplierId) REFERENCES Suppliers(SupplierId)
            );

            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Suppliers')
            CREATE TABLE Suppliers (
                SupplierId INT PRIMARY KEY IDENTITY,
                SupplierName NVARCHAR(100) NOT NULL,
                ContactName NVARCHAR(100),
                ContactEmail NVARCHAR(100)
            );

            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customers')
            CREATE TABLE Customers (
                CustomerId INT PRIMARY KEY IDENTITY,
                CustomerName NVARCHAR(100) NOT NULL,
                ContactName NVARCHAR(100),
                ContactEmail NVARCHAR(100)
            );

            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders')
            CREATE TABLE Orders (
                OrderId INT PRIMARY KEY IDENTITY,
                CustomerId INT,
                OrderDate DATETIME NOT NULL,
                FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
            );

            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderDetails')
            CREATE TABLE OrderDetails (
                OrderDetailId INT PRIMARY KEY IDENTITY,
                OrderId INT,
                ProductId INT,
                Quantity INT NOT NULL,
                FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
                FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
            );

            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Shipments')
            CREATE TABLE Shipments (
                ShipmentId INT PRIMARY KEY IDENTITY,
                ShipmentDate DATETIME NOT NULL,
                EmployeeId INT,
                FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId)
            );

            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ShipmentDetails')
            CREATE TABLE ShipmentDetails (
                ShipmentDetailId INT PRIMARY KEY IDENTITY,
                ShipmentId INT,
                ProductId INT,
                Quantity INT NOT NULL,
                FOREIGN KEY (ShipmentId) REFERENCES Shipments(ShipmentId),
                FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
            );

            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Inventory')
            CREATE TABLE Inventory (
                InventoryId INT PRIMARY KEY IDENTITY,
                ProductId INT,
                WarehouseId INT,
                Quantity INT NOT NULL,
                FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
                FOREIGN KEY (WarehouseId) REFERENCES Warehouses(WarehouseId)
            );

            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')
            CREATE TABLE Employees (
                EmployeeId INT PRIMARY KEY IDENTITY,
                EmployeeName NVARCHAR(100) NOT NULL,
                Position NVARCHAR(100),
                HireDate DATETIME
            );
        ";
                    using (SqlCommand command = new SqlCommand(checkTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Таблица Users проверена или создана успешно.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при подключении к базе данных: " + ex.Message);
                }
            }
        }
        private void ButtonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;
            bool userFound = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", login);
                        command.Parameters.AddWithValue("@Password", password);

                        int count = (int)command.ExecuteScalar();
                        userFound = count > 0;
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
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка: " + ex.Message);
                }
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Проверяем, существует ли пользователь с таким логином
                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    using (SqlCommand checkCommand = new SqlCommand(checkUserQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Username", login);
                        int userCount = (int)checkCommand.ExecuteScalar();

                        if (userCount > 0)
                        {
                            MessageBox.Show("Такой пользователь уже существует");
                        }
                        else
                        {
                            // Добавляем нового пользователя
                            string insertQuery = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
                            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@Username", login);
                                insertCommand.Parameters.AddWithValue("@Password", password);
                                insertCommand.ExecuteNonQuery();
                                MessageBox.Show("Пользователь создан");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка: " + ex.Message);
                }
            }
        }
    }
}