using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using System.Transactions;

namespace DeadlockC_
{
    internal class Program
    {
        static string connString = @"Server=CULBEC\SQLEXPRESS; Database=OrganizatorEvenimente; Integrated Security = true; TrustServerCertificate = true;";
        static readonly int RETRY_COUNT = 5; 

        static void Main(string[] args)
        {
            int retries = 0;
            bool success = false;

            while(!success && retries < RETRY_COUNT)
            {
                Console.WriteLine($"Retries: {retries}");

                // New thread creation.
                Thread t1 = new Thread(() =>
                {
                    Console.WriteLine("Thread 1 running...");
                    // Opening a connection to the database.
                    using var conn = new SqlConnection(connString);
                    conn.Open();

                    // Retrieving the stored procedure from the database.
                    using var deadlock_command = conn.CreateCommand();
                    deadlock_command.CommandText = "SET DEADLOCK_PRIORITY HIGH";
                    deadlock_command.ExecuteNonQuery();

                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            using var command = conn.CreateCommand();

                            command.Transaction = tran;
                            command.CommandText = "update Vehicule set Culoare = 'DEADC' where Vid = 10";
                            command.ExecuteNonQuery();

                            Thread.Sleep(10000);

                            command.CommandText = "update Furnizori set Nume = 'DEADN' where Fid = 5";
                            command.ExecuteNonQuery();

                            // Executing the stored procedure.
                            tran.Commit();
                            Console.WriteLine("Transaction 1 executed successfully!");
                            success = true;
                        }
                        catch (SqlException ex)
                        {
                            // Caught a deadlock.
                            if (ex.Number == 1205)
                            {
                                Console.WriteLine($"Deadlock identified. Retrying... {ex.Message}");
                            }
                            else
                            {
                                Console.WriteLine($"An error occurred: {ex.Message}");
                            }

                            tran.Rollback();
                            retries++;
                        }
                    }                    
                });

                // New thread creation.
                Thread t2 = new Thread(() =>
                {
                    Console.WriteLine("Thread 2 running...");

                    // Opening a connection to the database.
                    using var conn = new SqlConnection(connString);
                    conn.Open();

                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            using var command = conn.CreateCommand();

                            command.Transaction = tran;
                            command.CommandText = "update Furnizori set Nume = 'DEADN' where Fid = 5";
                            command.ExecuteNonQuery();

                            Thread.Sleep(10000);

                            command.CommandText = "update Vehicule set Culoare = 'DEADC' where Vid = 10";
                            command.ExecuteNonQuery();

                            // Executing the stored procedure.
                            tran.Commit();
                            Console.WriteLine("Transaction 2 executed successfully!");
                            success = true;
                        }
                        catch (SqlException ex)
                        {
                            // Caught a deadlock.
                            if (ex.Number == 1205)
                            {
                                Console.WriteLine($"Deadlock identified. Retrying... {ex.Message}");
                            }
                            else
                            {
                                Console.WriteLine($"An error occurred: {ex.Message}");
                            }
                            tran.Rollback();
                            retries++;
                        }
                    }                    
                });

                t1.Start(); t2.Start();
                t1.Join(); t2.Join();
            }

            if (retries >= RETRY_COUNT)
            {
                Console.WriteLine("Maximum number of retries reached!");
            } else
            {
                Console.WriteLine("All operations were executed successfully!");
            }
        }
    }
}
