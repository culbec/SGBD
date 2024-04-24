using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    internal static class DbUtils
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;
        private static readonly string sqlSelectParents = ConfigurationManager.AppSettings["selectParents"]!;
        private static readonly string sqlSelectChildren = ConfigurationManager.AppSettings["selectChildren"]!;
        private static readonly string sqlInsertChild = ConfigurationManager.AppSettings["insert"]!;
        private static readonly string sqlUpdateChild = ConfigurationManager.AppSettings["update"]!;
        private static readonly string sqlDeleteChild = ConfigurationManager.AppSettings["delete"]!;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static SqlCommand GetSelectParentsCommand(SqlConnection conn)
        {
            return new SqlCommand(sqlSelectParents, conn);
        }

        public static SqlCommand GetSelectChildrenCommand(SqlConnection conn) {
            return new SqlCommand(sqlSelectChildren, conn);
        }

        public static SqlCommand GetInsertChildCommand(SqlConnection conn, Panel panel)
        {
            var insertCommand = new SqlCommand(sqlInsertChild, conn);
            var insertColumns = new List<string>(ConfigurationManager.AppSettings["insertColumnNames"]!.Split(','));

            foreach(var column in insertColumns)
            {
                TextBox textBox = (TextBox)panel.Controls[column.Split('@')[1]]!;
                insertCommand.Parameters.AddWithValue(column, textBox.Text);
            }
            return insertCommand;
        }

        public static SqlCommand GetUpdateChildCommand(SqlConnection conn, Panel panel)
        {
            var updateCommand = new SqlCommand(sqlUpdateChild, conn);
            var updateColumns = new List<string>(ConfigurationManager.AppSettings["updateColumnNames"]!.Split(','));

            foreach(var column in updateColumns)
            {
                TextBox textBox = (TextBox)panel.Controls[column.Split('@')[1]]!;
                updateCommand.Parameters.AddWithValue(column, textBox.Text);
            }

            return updateCommand;
        }

        public static SqlCommand GetDeleteChildCommand(SqlConnection conn, Panel panel)
        {
            var deleteCommand = new SqlCommand(sqlDeleteChild, conn);
            var deleteColumns = new List<string>(ConfigurationManager.AppSettings["deleteColumnNames"]!.Split(','));

            foreach(var column in deleteColumns)
            {
                TextBox textBox = (TextBox)panel.Controls[column.Split('@')[1]]!;
                deleteCommand.Parameters.AddWithValue(column, textBox.Text);
            }

            return deleteCommand;
        }
    }
}
