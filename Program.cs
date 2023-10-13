using System.Data.SqlClient;
using System;
using System.Net.NetworkInformation;

class Program
{
    static string connectionString = "Data Source=karkro\\SQLEXPRESS;Initial Catalog=Uczelnia;Integrated Security=True;";

    static void Main()
    {
        try
        {
            SqlConnection connection = PolaczZBazaDanych(connectionString);
            if (connection != null )
            {
                Console.WriteLine("Dane z bazy:");
                WyswietlDaneZBazy(connection);
                Console.WriteLine();

                DodajDaneDoBazy(connection, "Paździoch", "Marian");
                WyswietlDaneZBazy(connection);
                Console.WriteLine();

                NadpiszDaneWBazie(connection, "Paździoch", "Wójcik");
                WyswietlDaneZBazy(connection);
                Console.WriteLine();

                UsunDaneZBazy(connection, "Wójcik");
                WyswietlDaneZBazy(connection);
            }
        }

        catch (SqlException ex)
        {
            Console.WriteLine("Błąd SQL: " + ex.Message);
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Ogólny błąd: " + ex.Message);

        }

        finally
        {
            Console.ReadKey();
        }
    }

    static SqlConnection PolaczZBazaDanych(string connectionString)
    {
        SqlConnection connection = null;

        try
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
            int i = 0;
            while (connection.State == System.Data.ConnectionState.Connecting)
            {
                System.Threading.Thread.Sleep(50);
                i++;
                if (i > 2000)
                    break;
            }

            if (connection.State != System.Data.ConnectionState.Open)
                throw new Exception("Nie połączono");
        }
        catch (SqlException ex)
        {
            // Obsługa błędów związanych z bazą danych
            Console.WriteLine("Błąd SQL: " + ex.Message);
            connection = null;
        }
        catch (Exception ex)
        {
            // Obsługa ogólnych błędów
            Console.WriteLine("Wystąpił ogólny błąd: " + ex.Message);
            connection = null;
        }

        return connection;
    }

    static void WyswietlDaneZBazy(SqlConnection connection)
    {
        if (connection != null)
        {
            try
            {
                string queryString = "SELECT Nazwisko, Imie FROM dbo.Test";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nazwisko = reader["Nazwisko"].ToString();
                            string imie = reader["Imie"].ToString();


                            Console.WriteLine($"Nazwisko: {nazwisko}; Imię: {imie}");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {

                Console.WriteLine("Błąd Sql: " + ex.Message);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Ogólny błąd: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Brak połączenia z bazą danych");
        }

    }

    static void DodajDaneDoBazy(SqlConnection connection, string nazwisko, string imie)
    {
        if (connection != null)
        {
            try
            {
                string queryString = "INSERT INTO dbo.Test (Nazwisko, Imie) VALUES (@Nazwisko, @Imie)";
                
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@Nazwisko", nazwisko);
                    command.Parameters.AddWithValue("@Imie", imie);

                    int affectedRow = command.ExecuteNonQuery();
                    if (affectedRow > 0)
                    {
                        Console.WriteLine("Dane zostały dodane do bazy");
                    }
                    else 
                    {
                        Console.WriteLine("Nie udało się dodać danych do bazy");
                    }
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine("Błąd Sql:" + ex.Message);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Ogólny błąd: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Brak połączenia z bazą danych");
        }
    }

    static void UsunDaneZBazy(SqlConnection connection, string nazwisko)
    {
        if (connection != null)
        {
            try
            {
                string queryString = "DELETE FROM dbo.Test WHERE Nazwisko = @Nazwisko";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@Nazwisko", nazwisko);

                    int affectedRow = command.ExecuteNonQuery();
                    if (affectedRow > 0)
                    {
                        Console.WriteLine($"Osoba o nazwisku {nazwisko} została usunięta z bazy");
                    }
                    else
                    {
                        Console.WriteLine($"Nie znaleziono osoby o nazwisku {nazwisko} w bazie");
                    }
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine("Błąd Sql:" + ex.Message);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Ogólny błąd: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Brak połączenia z bazą danych");
        }
    }

    static void NadpiszDaneWBazie(SqlConnection connection, string stareNazwisko, string noweNazwisko)
    {
        if (connection != null)
        {
            try
            {
                string queryString = "Update dbo.Test SET Nazwisko = @NoweNazwisko WHERE Nazwisko = @StareNazwisko";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@StareNazwisko", stareNazwisko);
                    command.Parameters.AddWithValue("@NoweNazwisko", noweNazwisko);

                    int affectedRow = command.ExecuteNonQuery();
                    if (affectedRow > 0)
                    {
                        Console.WriteLine($"Nazwisko osoby {stareNazwisko} zostało zmienione na {noweNazwisko}");
                    }
                    else
                    {
                        Console.WriteLine($"Nie znaleziono osoby o nazwisku {stareNazwisko} w bazie");
                    }
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine("Błąd Sql:" + ex.Message);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Ogólny błąd: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Brak połączenia z bazą danych");
        }
    }
}


//Using System;
//Using System.Data.SqlClient;

//class Program
//{
//    static void Main()
//    {
//        string connectionString = "Data Source = karkro\\SQLEXPRESS; Initial Catalog = Uczelnia; Integrated Security = True";

//        try
//        {
//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                connection.Open();

//                string queryString = "SELECT imie, nazwisko, nr_albumu FROM dbo.Student";

//                using (SqlCommand command = new SqlCommand(queryString, connection))
//                {
//                    using (SqlDataReader reader = command.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            string imie = reader["Imie"].ToString();
//                            string nazwisko = reader["Nazwisko"].ToString();
//                            string nr_albumu = reader["nr_albumu"].ToString();

//                            Console.WriteLine($"Imię: {imie}; Nazwisko: {nazwisko}");
//                        }
//                    }
//                }
//            }
//        }

//        catch (SqlException ex)
//        {
//            Console.WriteLine("Błąd SQL: " + ex.Message);
//        }

//        catch (Exception ex)
//        {
//            Console.WriteLine("Ogólny błąd: " + ex.Message)

//        }

//        finally
//        {
//            Console.ReadKey();
//        }
//    }
//}
