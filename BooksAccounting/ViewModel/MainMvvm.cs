using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BooksAccounting.Model;
using BooksAccounting.View;



namespace BooksAccounting.ViewModel
{
    internal class MainMVVM : BaseVM
    {
        private Books selectedBooks;
        private ObservableCollection<Books> books = new();

        public ObservableCollection<Books> Books
        {
            get => books;
            set
            {
                books = value;
                Signal();
            }
        }
        public Books SelectedBooks
        {
            get => selectedBooks;
            set
            {
                selectedBooks = value;
                Signal();
            }
        }
        public CommandMvvm UpdateBook { get; set; }
        public CommandMvvm RemoveBook { get; set; }
        public CommandMvvm AddBook { get; set; }

        public MainMVVM()
        {
            SelectAll();

            UpdateBook = new CommandMvvm(() =>
            {
                if (BooksDB.GetDb().Update(SelectedBooks))
                    MessageBox.Show("Успешно");
            }, () => SelectedBooks != null);

            RemoveBook = new CommandMvvm(() =>
            {
                BooksDB.GetDb().Remove(SelectedBooks);
                SelectAll();
            }, () => SelectedBooks != null);

            AddBook = new CommandMvvm(() =>
            {
                new AddBooks().ShowDialog();
                SelectAll();
            }, () => true);
        }

        private void SelectAll()
        {
            Books = new ObservableCollection<Books>(BooksDB.GetDb().SelectAll());
        }

    }
}
