using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using static Dienstplan.VMBase;

namespace Dienstplan;
internal class MainViewModel : VMBase
{
    internal const string DataBaseName = "Dienstplan.sqlite";
    private string dataBasePath;

    private Visibility stackPanelVisibility = Visibility.Visible;
    public Visibility StackPanelVisibility
    {
        get { return stackPanelVisibility; }
        set 
        {
            stackPanelVisibility = value;
            OnPropertChanged(nameof(StackPanelVisibility));
        }
    }

    private List<Group> groups;

    public ICommand EditEmployeesCommand => new Command(EditEmployees);
    public ICommand EditGroupsCommand => new Command(EditGroups);

    public EmployeesViewModel EmployeesViewModel { get; init; }
    public GroupsViewModel GroupsViewModel { get; init; }


    public MainViewModel()
    {
        EmployeesViewModel = new EmployeesViewModel();
        GroupsViewModel = new GroupsViewModel();
        GroupsViewModel.SaveAndClose += GroupsViewModel_SaveAndClose;

        InitDataBase();
        
        // init connection
        // checken ob Datenbank da ist 
        // read data     
        // creat Models

    }

    private void GroupsViewModel_SaveAndClose(object? sender, IList<Group> newItems)
    {
        StackPanelVisibility = Visibility.Visible;
        GroupsViewModel.Visibility = Visibility.Collapsed;

        long maxId = groups.Max(x => x.Id) + 1;
        IEnumerable<Group> editItems = groups.Where(x => x.IsEdit); 

        using (SqliteConnection connection = new SqliteConnection(GetConnectionString()))
        {
            connection.Open();

            foreach (Group group in newItems)
            {
                group.Id = maxId;
                InsertGroup(connection, group);

                maxId++;
            }

            foreach (Group group in editItems)
            {
                UpdateGroup(connection, group);
                group.IsEdit = false;
            }

            connection.Close();
        }

        groups.AddRange(newItems);
    }

    private void EditEmployees(object param)
    {
        StackPanelVisibility = Visibility.Collapsed;
        EmployeesViewModel.Visibility = Visibility.Visible;
    }
    private void EditGroups(object param)
    {
        GroupsViewModel.Groups = new ObservableCollection<Group>(groups.Where(x => !x.IsOut));
        StackPanelVisibility = Visibility.Collapsed;
        GroupsViewModel.Visibility = Visibility.Visible;
    }

    private void InitDataBase()
    {
        string path = Assembly.GetExecutingAssembly().Location;
        string directory = Path.GetDirectoryName(path);
        dataBasePath = Path.Combine(directory, DataBaseName);
        if (File.Exists(dataBasePath))
            File.Delete(dataBasePath);
        
        SqliteConnectionStringBuilder sqlBuilder = new SqliteConnectionStringBuilder();
        sqlBuilder.Mode = SqliteOpenMode.ReadWriteCreate;
        sqlBuilder.DataSource = DataBaseName;

        using (SqliteConnection connection = new SqliteConnection(GetConnectionString()))
        {
            connection.Open();

            CreateGroupsTable(connection);

            InsertGroup(connection, new Group() { Id = 1, Name = "Pandas", Type = GroupType.Small });
            InsertGroup(connection, new Group() { Id = 2, Name = "Bienen", Type = GroupType.Small, IsOut = true });
            InsertGroup(connection, new Group() { Id = 3, Name = "Koalas", Type = GroupType.Big });
            InsertGroup(connection, new Group() { Id = 4, Name = "Bambis", Type = GroupType.Small });

            groups = ReadGroups(connection).ToList();

            connection.Close();
        }


        // if exists --> return

        // if not create file and tables
    }

    private string GetConnectionString()
    {
        SqliteConnectionStringBuilder sqlBuilder = new SqliteConnectionStringBuilder();
        sqlBuilder.Mode = SqliteOpenMode.ReadWriteCreate;
        sqlBuilder.DataSource = DataBaseName;
        return sqlBuilder.ConnectionString;
    }

    private void CreateGroupsTable(SqliteConnection connection)
    {
        string command = "CREATE Table Groups (id INTEGER, name TEXT, type INTEGER, isout INTEGER)";
        SqliteCommand cmd = new SqliteCommand(command, connection);
        cmd.ExecuteNonQuery();
    }
    private void CreateEmploeesTable(SqliteConnection connection)
    {
        string command = "CREATE Table Employees (id INTEGER, firstname TEXT, lastname TEXT)";
        SqliteCommand cmd = new SqliteCommand(command, connection);
        cmd.ExecuteNonQuery();
    }

    private void InsertGroup(SqliteConnection connection, Group group)
    {
        string command = "INSERT INTO Groups (id, name, type, isout) VALUES($id, $name, $type, $isout)";

        SqliteCommand cmd = new SqliteCommand(command, connection);
        cmd.Parameters.AddWithValue("$id", group.Id);
        cmd.Parameters.AddWithValue("$name", group.Name);
        cmd.Parameters.AddWithValue("$type", (long)group.Type);
        cmd.Parameters.AddWithValue("$isout", group.IsOut ? 1 : 0);

        cmd.ExecuteNonQuery();
    }

    private void UpdateGroup(SqliteConnection connection, Group group)
    {
        string command = "UPDATE Groups SET name = $name, type = $type, isout = $isout WHERE id = $id";

        SqliteCommand cmd = new SqliteCommand(command, connection);
        cmd.Parameters.AddWithValue("$id", group.Id);
        cmd.Parameters.AddWithValue("$name", group.Name);
        cmd.Parameters.AddWithValue("$type", (long)group.Type);
        cmd.Parameters.AddWithValue("$isout", group.IsOut ? 1 : 0);

        cmd.ExecuteNonQuery();
    }

    private IEnumerable<Group> ReadGroups(SqliteConnection connection)
    {
        string command = "SELECT id, name, type, isout FROM Groups";
        SqliteCommand cmd = new SqliteCommand(command, connection);
        SqliteDataReader reader = cmd.ExecuteReader();

        List<Group> groups = new List<Group>();

        while (reader.Read()) 
        {
            Group group = new Group();

            group.Id = (long)reader["id"];
            group.Name = (string)reader["name"];
            group.Type = (GroupType)((long)reader["type"]);
            group.IsOut = (long)reader["isout"] == 1;

            groups.Add(group);
        }

        return groups;
    }

    private void Old(SqliteConnection connection)
    {


        string command = "INSERT INTO Employees (id, firstname, lastname) VALUES($id, $firstname, $lastname)";
        //command = "INSERT INTO Employees";
        SqliteCommand cmd = new SqliteCommand(command, connection);
        cmd.Parameters.AddWithValue("$id", 1);
        cmd.Parameters.AddWithValue("$firstname", "paul");
        cmd.Parameters.AddWithValue("$lastname", "neider");
        var ret = cmd.ExecuteNonQuery();
    }

    private SqliteConnection CreatConnection()
    {
        SqliteConnection sqlite_conn;
        SqliteConnectionStringBuilder con_sql = new SqliteConnectionStringBuilder();
        
        sqlite_conn = new SqliteConnection($"Data Source={dataBasePath}; Version = 3; New = True; Compress = True; ");
        // Open the connection:
        try
        {
            sqlite_conn.Open();
        }
        catch (Exception) { }

        return sqlite_conn;
    }


}
