using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    /**
     *  Provides support for connecting to the SQL Server database,
     *  and creating SQL commands for manipulating the data in the database.
     */
    internal static class DbUtils
    {
        private static readonly string connectionString = @"Server=DESKTOP-8ANVK2E\SQLEXPRESS;Database=OrganizatorEvenimente;Integrated Security=true;TrustServerCertificate=true";
        private static readonly string sqlSelectParents = @"SELECT * FROM Evenimente";
        private static readonly string sqlSelectChildren = @"SELECT * FROM Inventar";
        private static readonly string sqlUpdateChild = @"UPDATE Inventar SET Produs = @Produs, Cantitate = @Cantitate WHERE Iid = @Iid";
        private static readonly string sqlDeleteChild = @"DELETE Inventar WHERE Iid = @Iid";
        private static readonly string sqlInsertChild = @"INSERT INTO Inventar(Eid, Produs, Cantitate) VALUES (@Eid, @Produs, @Cantitate)";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static SqlCommand GetSelectParentsCommand(SqlConnection sqlConnection)
        {
            SqlCommand selectParentsCommand = new SqlCommand(sqlSelectParents, sqlConnection);
            return selectParentsCommand;
        }

        public static SqlCommand GetSelectChildrenCommand(SqlConnection sqlConnection)
        {
            SqlCommand selectChildrenCommand = new(sqlSelectChildren, sqlConnection);

            return selectChildrenCommand;
        }

        public static SqlCommand GetUpdateChildCommand(SqlConnection sqlConnection, int iid,  string produs, int cantitate)
        {
            SqlCommand updateChildCommand = new(sqlUpdateChild, sqlConnection);
            updateChildCommand.Parameters.AddWithValue("@Iid", iid);
            updateChildCommand.Parameters.AddWithValue("@Produs", produs);
            updateChildCommand.Parameters.AddWithValue("@Cantitate", cantitate);

            return updateChildCommand;
        }
    
        public static SqlCommand GetDeleteChildCommand(SqlConnection sqlConnection, int iid)
        {
            SqlCommand deleteChildCommand = new(sqlDeleteChild, sqlConnection);
            deleteChildCommand.Parameters.AddWithValue("@Iid", iid);

            return deleteChildCommand;
        }

        public static SqlCommand GetInsertChildCommand(SqlConnection sqlConnection, int eid, string produs, int cantitate) {
            SqlCommand insertChildCommand = new(sqlInsertChild, sqlConnection);
            insertChildCommand.Parameters.AddWithValue("@Eid", eid);
            insertChildCommand.Parameters.AddWithValue("@Produs", produs);
            insertChildCommand.Parameters.AddWithValue("@Cantitate", cantitate);

            return insertChildCommand;
        }
    }
}
