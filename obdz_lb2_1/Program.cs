using System;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace CinemaDatabaseApp
{
    class Program
    {
        static string connectionString = "User Id=system;Password=system;" +
                                         "Data Source=//localhost:1521/XE";

        static void Main(string[] args)
        {
            Console.WriteLine("Select an option from the list below:");
            Console.WriteLine("1 - Display data from tables");
            Console.WriteLine("2 - Insert data into a table");
            Console.WriteLine("3 - Perform JOIN query");
            Console.WriteLine("4 - Perform filtered query");
            Console.WriteLine("5 - Perform aggregate function query");
            int option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 1:
                    DisplayData();
                    break;
                case 2:
                    InsertData();
                    break;
                case 3:
                    PerformJoinQuery();
                    break;
                case 4:
                    PerformFilteredQuery();
                    break;
                case 5:
                    PerformAggregateQuery();
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }

        static void DisplayData()
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                string[] tables = { "THEATER", "SCREEN", "MOVIE", "SHOWTIME", "VIEWER", "TICKET" };
                
                foreach (var table in tables)
                {
                    Console.WriteLine($"\nData from {table}:");
                    OracleCommand command = new OracleCommand($"SELECT * FROM {table}", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader.GetName(i) + "\t");
                    }
                    Console.WriteLine();
                    
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write(reader.GetValue(i) + "\t");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        static void InsertData()
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Enter table name to insert data into:");
                string tableName = Console.ReadLine().ToUpper();

                switch (tableName)
                {
                    case "THEATER":
                        Console.WriteLine("Enter Theater_ID, Name, Address, Phone:");
                        string theaterData = Console.ReadLine();
                        OracleCommand theaterCmd = new OracleCommand($"INSERT INTO THEATER (Theater_ID, Name, Address, Phone) VALUES ({theaterData})", connection);
                        theaterCmd.ExecuteNonQuery();
                        break;
                    case "SCREEN":
                        Console.WriteLine("Enter Screen_ID, Theater_ID, Name, Capacity:");
                        string screenData = Console.ReadLine();
                        OracleCommand screenCmd = new OracleCommand($"INSERT INTO SCREEN (Screen_ID, Theater_ID, Name, Capacity) VALUES ({screenData})", connection);
                        screenCmd.ExecuteNonQuery();
                        break;
                    case "MOVIE":
                        Console.WriteLine("Enter Movie_ID, Title, Duration, Genre, Description:");
                        string movieData = Console.ReadLine();
                        OracleCommand movieCmd = new OracleCommand($"INSERT INTO MOVIE (Movie_ID, Title, Duration, Genre, Description) VALUES ({movieData})", connection);
                        movieCmd.ExecuteNonQuery();
                        break;
                    case "SHOWTIME":
                        Console.WriteLine("Enter Showtime_ID, Screen_ID, Movie_ID, StartTime, EndTime:");
                        string showtimeData = Console.ReadLine();
                        OracleCommand showtimeCmd = new OracleCommand($"INSERT INTO SHOWTIME (Showtime_ID, Screen_ID, Movie_ID, StartTime, EndTime) VALUES ({showtimeData})", connection);
                        showtimeCmd.ExecuteNonQuery();
                        break;
                    case "VIEWER":
                        Console.WriteLine("Enter Viewer_ID, Name, Email, PhoneNumber:");
                        string viewerData = Console.ReadLine();
                        OracleCommand viewerCmd = new OracleCommand($"INSERT INTO VIEWER (Viewer_ID, Name, Email, PhoneNumber) VALUES ({viewerData})", connection);
                        viewerCmd.ExecuteNonQuery();
                        break;
                    case "TICKET":
                        Console.WriteLine("Enter Ticket_ID, Viewer_ID, Showtime_ID:");
                        string ticketData = Console.ReadLine();
                        OracleCommand ticketCmd = new OracleCommand($"INSERT INTO TICKET (Ticket_ID, Viewer_ID, Showtime_ID) VALUES ({ticketData})", connection);
                        ticketCmd.ExecuteNonQuery();
                        break;
                    default:
                        Console.WriteLine("Invalid table name");
                        break;
                }
                Console.WriteLine("Data inserted successfully.");
            }
        }

        static void PerformJoinQuery()
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT t.Name AS TheaterName, s.Name AS ScreenName, m.Title AS MovieTitle, sh.StartTime
                    FROM THEATER t
                    JOIN SCREEN s ON t.Theater_ID = s.Theater_ID
                    JOIN SHOWTIME sh ON s.Screen_ID = sh.Screen_ID
                    JOIN MOVIE m ON sh.Movie_ID = m.Movie_ID";
                
                OracleCommand command = new OracleCommand(query, connection);
                OracleDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["TheaterName"]}\t{reader["ScreenName"]}\t{reader["MovieTitle"]}\t{reader["StartTime"]}");
                }
            }
        }

        static void PerformFilteredQuery()
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM MOVIE WHERE Genre = 'Crime drama'";
                OracleCommand command = new OracleCommand(query, connection);
                OracleDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Movie_ID"]}\t{reader["Title"]}\t{reader["Duration"]}\t{reader["Genre"]}\t{reader["Description"]}");
                }
            }
        }

        static void PerformAggregateQuery()
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Genre, COUNT(*) AS MovieCount FROM MOVIE GROUP BY Genre";
                OracleCommand command = new OracleCommand(query, connection);
                OracleDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Genre"]}\t{reader["MovieCount"]}");
                }
            }
        }
    }
}
