using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using Microcharts;
using SkiaSharp;
using System.Collections.Generic;
using System.Drawing;

namespace budgetTracking
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public ObservableCollection<Expense> Expenses { get; set; } = new ObservableCollection<Expense>();
        private Expense _selectedExpense;

        public Expense SelectedExpense
        {
            get { return _selectedExpense; }
            set
            {
                _selectedExpense = value;
                OnPropertyChanged(nameof(SelectedExpense));
            }
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            UpdateChartEntries();
        }

        private void OnAdd(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescriptionEntry.Text) || string.IsNullOrWhiteSpace(AmountEntry.Text))
                return;

            if (!decimal.TryParse(AmountEntry.Text, out decimal amount))
            {
                // Inform the user that the input for the amount is not valid
                return;
            }

            Expenses.Add(new Expense
            {
                Description = DescriptionEntry.Text,
                Amount = amount
            });

            DescriptionEntry.Text = string.Empty;
            AmountEntry.Text = string.Empty;
            UpdateChartEntries();
        }

        private void OnDelete(object sender, EventArgs e)
        {
            Expenses.Remove(SelectedExpense);
            SelectedExpense = null;
            UpdateChartEntries();
        }

        private void UpdateChartEntries()
        {
            var entries = new List<ChartEntry>();

            foreach (var expense in Expenses)
            {
                var chartEntry = new ChartEntry((float)expense.Amount)
                {
                    Label = expense.Description,
                    Color = SKColor.Parse("#266489"),
                    ValueLabel = expense.Amount.ToString("C")
                };

                entries.Add(chartEntry);
            }

            var chart = new BarChart
            {
                Entries = entries,
                LabelOrientation = Microcharts.Orientation.Horizontal,
                ValueLabelOrientation = Microcharts.Orientation.Horizontal,
                LabelTextSize = 50 // Increase the label font size
            };

            chartView.Chart = chart;
        }

    }

    public class Expense
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
