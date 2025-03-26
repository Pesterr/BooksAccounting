using BooksAccounting.Model;
using MySqlConnector;
using MySqlStart1125.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BooksAccounting   
{
    internal class BooksDB
    {
        DbConnection connection;

        private BooksDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Books books)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Books` Values (0, @Title, @AuthorID, @YearPublished, @Genre, @IsAvailable);select LAST_INSERT_ID();");

               
                cmd.Parameters.Add(new MySqlParameter("Title", books.Title));
                cmd.Parameters.Add(new MySqlParameter("AuthorID", books.AuthorID));
                cmd.Parameters.Add(new MySqlParameter("YearPublished", books.YearPublished));
                cmd.Parameters.Add(new MySqlParameter("Genre", books.Genre));
                cmd.Parameters.Add(new MySqlParameter("IsAvailable", books.IsAvailable));

                try
                {            
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());
                        books.Id = id;
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

        internal List<Books> SelectAll()
        {
            List<Books> books = new List<Books>();
            if (connection == null)
                return books;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `Id`, `Title`, `AuthorID`, `YearPublished`, `Genre`, `IsAvailable` from `Books` ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();              
                    while (dr.Read())
                    {
                        int Id = dr.GetInt32(0);
                        string title = string.Empty;
                        int AuthorID = dr.GetInt32(1);
                        if (!dr.IsDBNull(1))
                            title = dr.GetString("Title");
                        if (!dr.IsDBNull(2))
                            AuthorID = dr.GetInt32("AuthorID");
                        int YearPublished = dr.GetInt32("YearPublished");
                        string Genre = dr.GetString("Genre");
                        bool IsAvailable = dr.GetBoolean("IsAvailable");
                        books.Add(new Books
                        {
                            Id = Id,
                            Title = title,
                            AuthorID = AuthorID,
                            YearPublished = YearPublished,
                            Genre = Genre,
                            IsAvailable = IsAvailable,
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return books;
        }

        internal bool Update(Books edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Books` set `Title`=@Title, `AuthorID`=@AuthorID, `YearPublished`=@YearPublished, `Genre`=@Genre, `IsAvailable`=@IsAvailable where `id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("Title", edit.Title));
                mc.Parameters.Add(new MySqlParameter("AuthorID", edit.AuthorID));
                mc.Parameters.Add(new MySqlParameter("YearPublished", edit.YearPublished));
                mc.Parameters.Add(new MySqlParameter("Genre", edit.Genre));
                mc.Parameters.Add(new MySqlParameter("IsAvailable", edit.IsAvailable));

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


        internal bool Remove(Books remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Books` where `id` = {remove.Id}");
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

        static BooksDB db;
        public static BooksDB GetDb()
        {
            if (db == null)
                db = new BooksDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}