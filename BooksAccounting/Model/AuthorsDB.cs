using BooksAccounting.Model;
using MySqlConnector;
using MySqlStart1125.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BooksAccounting
{
    internal class AuthorsDB
    {
        DbConnection connection;

        private AuthorsDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Authors authors)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Authors` Values (0, @FirstName, @Patronymic, @LastName, @Birthday);select LAST_INSERT_ID();");


                cmd.Parameters.Add(new MySqlParameter("FirstName", authors.FirstName));
                cmd.Parameters.Add(new MySqlParameter("Patronymic", authors.Patronymic));
                cmd.Parameters.Add(new MySqlParameter("LastName", authors.LastName));
                cmd.Parameters.Add(new MySqlParameter("Birthday", authors.Birthday));
   

                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());
                        authors.Id = id;
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Запись не добавлена");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        internal List<Authors> SelectAll()
        {
            List<Authors> authors = new List<Authors>();
            if (connection == null)
                return authors;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `Id`, `FirstName`, `Patronymic`, `LastName`, `Birthday` from `Authors` ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int Id = dr.GetInt32(0);
                        string Firstname = dr.GetString("FirstName");
                        string Patronymic = dr.GetString("Patronymic");
                        string LastName = dr.GetString("LastName");
                        DateTime Birthday = dr.GetDateTime("Birthday");
                        authors.Add(new Authors
                        {
                            Id = Id,
                            FirstName = Firstname,
                            Patronymic = Patronymic,
                            LastName = LastName,
                            Birthday = Birthday
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return authors;
        }

        internal bool Update(Authors edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Authors` set `FirstName`=@FirstName, `Patronymic`=@Patronymic, `LastName`=@LastName, `Birthday`=@Birthday where `Id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("FirstName", edit.FirstName));
                mc.Parameters.Add(new MySqlParameter("Patronymic", edit.Patronymic));
                mc.Parameters.Add(new MySqlParameter("LastName", edit.LastName));
                mc.Parameters.Add(new MySqlParameter("Birthday", edit.Birthday));
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }


        internal bool Remove(Authors remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Authors` where `Id` = {remove.Id}");
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        static AuthorsDB db;
        public static AuthorsDB GetDb()
        {
            if (db == null)
                db = new AuthorsDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}