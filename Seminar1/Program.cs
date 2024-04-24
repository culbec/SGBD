// See https://aka.ms/new-console-template for more information

using Microsoft.Data.SqlClient;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.BackgroundColor = ConsoleColor.Magenta;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Clear();

        Console.WriteLine("Hello, World!");

        try
        {
            // @ - verbose string - toate caracterele sunt escaped si normale
            string connectionString = @"	";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Starea conexiunii inainte de apelul Open(): {0}", connection.State);

                connection.Open(); // deschidem conexiunea la baza de date
                Console.WriteLine("Starea conexiunii dupa apelul Open(): {0}", connection.State);

                string insertString = "INSERT INTO Cadouri(descriere, posesor, pret) VALUES" +
                    "(@desc1, @posesor1, @pret1), (@desc2, @posesor2, @pret2), (@desc3, @posesor3, @pret3)";
                SqlCommand insertCommand = new SqlCommand(insertString, connection);

                // adaugam parametrii in comanda

                insertCommand.Parameters.AddWithValue("@desc1", "telefon");
                insertCommand.Parameters.AddWithValue("@posesor1", "George");
                insertCommand.Parameters.AddWithValue("@pret1", 7.5);

                insertCommand.Parameters.AddWithValue("@desc2", "laptop");
                insertCommand.Parameters.AddWithValue("@posesor2", "Marius");
                insertCommand.Parameters.AddWithValue("@pret2", 1000);

                insertCommand.Parameters.AddWithValue("@desc3", "carte");
                insertCommand.Parameters.AddWithValue("@posesor3", "Andreea");
                insertCommand.Parameters.AddWithValue("@pret3", 50);

                // inserarea valorilor in bd
                int insertRowCount = insertCommand.ExecuteNonQuery();
                Console.WriteLine("Numarul de randuri afectate: {0}", insertRowCount);

                // citirea si afisarea datelor din bd
                SqlCommand selectCommand = new SqlCommand("SELECT descriere, posesor, pret FROM CADOURI", connection);
                
                // obiect pentru citirea datelor din executia unui query
                SqlDataReader reader = selectCommand.ExecuteReader();

                // verificam daca reader-ul are continut
                if (reader.HasRows)
                {
                    Console.WriteLine("Continutul tabelului 'Cadouri'");

                    while(reader.Read())
                    {
                        Console.WriteLine("{0}\t{1}\t{2}", reader.GetString(0), reader.GetString(1), reader.GetFloat(2));
                    }
                } else
                {
                    Console.WriteLine("Instructiunea 'select' nu a returnat inregistrari");
                }

                reader.Close();

                // actualizarea datelor

                SqlCommand updateCommand = new SqlCommand("UPDATE Cadouri SET pret = @pretNou WHERE descriere = @descriere", connection);

                updateCommand.Parameters.AddWithValue("@pretNou", 100);
                updateCommand.Parameters.AddWithValue("@descriere", "telefon");

                int updateRowCount = updateCommand.ExecuteNonQuery();
                Console.WriteLine("Numar de randuri afectate: {0}", updateRowCount);

                // stergerea datelor

                SqlCommand deleteCommand = new SqlCommand("DELETE FROM Cadouri WHERE descriere = @descriere", connection);

                deleteCommand.Parameters.AddWithValue("@descriere", "carte");

                int deleteRowCount = deleteCommand.ExecuteNonQuery();
                Console.WriteLine("Numar de randuri afectate: {0}", deleteRowCount);

                // citirea si afisarea datelor dupa actualizare si stergere

                reader = selectCommand.ExecuteReader();

                // verificam daca reader-ul are continut
                if (reader.HasRows)
                {
                    Console.WriteLine("Continutul tabelului 'Cadouri'");

                    while (reader.Read())
                    {
                        Console.WriteLine("{0}\t{1}\t{2}", reader.GetString(0), reader.GetString(1), reader.GetFloat(2));
                    }
                }
                else
                {
                    Console.WriteLine("Instructiunea 'select' nu a returnat inregistrari");
                }

                reader.Close();
            }


        } catch (Exception ex) {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("Eroare la conexiunea cu baza de date: {0}", ex.Message);
        }
  
    }
}
