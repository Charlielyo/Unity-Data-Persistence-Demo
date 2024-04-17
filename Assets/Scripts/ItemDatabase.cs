using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using UnityEngine.UI;

public class ItemDatabase : MonoBehaviour
{
    private string connectionString;
    private string filePath;

    public InputField itemNameInput;
    public InputField itemDescriptionInput;
    public InputField itemPriceInput;
    public Button insertButton;
    public Button queryButton;
    public Button clearButton;
    public Text itemListText;

    void Start()
    {
        filePath = Application.persistentDataPath + "/items.sqlite";
        connectionString = "URI=file:" + filePath;

        CreateTables();

        insertButton.onClick.AddListener(InsertItem);
        queryButton.onClick.AddListener(QueryAllItems);
        clearButton.onClick.AddListener(ClearData);
    }

    private void CreateTables()
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            IDbCommand dbCommand = dbConnection.CreateCommand();
            string createTableQuery = "CREATE TABLE IF NOT EXISTS Items (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Description TEXT, Price INTEGER)";
            dbCommand.CommandText = createTableQuery;
            dbCommand.ExecuteNonQuery();
        }
    }

    private void InsertItem()
    {
        string name = itemNameInput.text;
        string description = itemDescriptionInput.text;
        int price;
        if (int.TryParse(itemPriceInput.text, out price))
        {
            using (IDbConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                IDbCommand dbCommand = dbConnection.CreateCommand();
                string insertQuery = "INSERT INTO Items (Name, Description, Price) VALUES (@Name, @Description, @Price)";
                dbCommand.CommandText = insertQuery;
                dbCommand.Parameters.Add(new SqliteParameter("@Name", name));
                dbCommand.Parameters.Add(new SqliteParameter("@Description", description));
                dbCommand.Parameters.Add(new SqliteParameter("@Price", price));
                dbCommand.ExecuteNonQuery();

                Debug.Log("Item inserted successfully!");
            }
        }
        else
        {
            Debug.LogError("Invalid price input!");
        }
    }

    private void QueryAllItems()
    {
        itemListText.text = "";
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            IDbCommand dbCommand = dbConnection.CreateCommand();
            string queryAllQuery = "SELECT * FROM Items";
            dbCommand.CommandText = queryAllQuery;

            IDataReader reader = dbCommand.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string description = reader.GetString(2);
                int price = reader.GetInt32(3);

                itemListText.text += $"Id: {id}, Name: {name}, Description: {description}, Price: {price}\n";
            }
            reader.Close();
        }
    }

    private void ClearData()
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            IDbCommand dbCommand = dbConnection.CreateCommand();
            string clearQuery = "DELETE FROM Items";
            dbCommand.CommandText = clearQuery;
            dbCommand.ExecuteNonQuery();

            itemListText.text = "Data cleared successfully!";
        }
    }
}