using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataGrid datag;
        List<string> headers;
        List<TextBox> txtBs1;
        List<TextBox> txtBs2;
        NorthwindEntities nw;
        List<StackPanel> stckp;
        List<Button> btns;
        public MainWindow()
        {
            InitializeComponent();

            //Top StackPanel
            StackPanel stckTop = new StackPanel();
            stckTop.Name = "stckTop";
            stckTop.Orientation = Orientation.Horizontal;
            stck.Children.Add(stckTop);

            // Bottom StackPanel 
            StackPanel stckBtm = new StackPanel();
            stckBtm.Margin = new Thickness(5);
            stckBtm.Orientation = Orientation.Horizontal;
            stck.Children.Add(stckBtm);

            //DataGrid
            datag = new DataGrid();
            datag.Height = 300;
            datag.Width = 1000;
            datag.HorizontalAlignment = HorizontalAlignment.Center;
            datag.Margin = new Thickness(10);
            //datag.IsReadOnly = true;
            //datag.Background = new SolidColorBrush(Colors.AntiqueWhite);
            datag.SelectionChanged += Datag_SelectionChanged;
            stckBtm.Children.Add(datag);
            datag.IsReadOnly = true;

            //creating lists to add stackpanels and buttons
            stckp = new List<StackPanel>();
            btns = new List<Button>();


            //CREATING STACKPANELS
            for (int i=0; i < 6; i++)
            {
                StackPanel stck = new StackPanel();
                stck.Margin = new Thickness(5);
                // stck2.HorizontalAlignment = HorizontalAlignment.Left;
                stck.Background = new SolidColorBrush(Colors.Beige);
                stckTop.Children.Add(stck);
                stckp.Add(stck);
            }
            stckp[1].Name = "stck3";
            stckp[4].Name="stck6";
           
            //CREATING BUTTONS
            for(int i = 0; i < 4; i++)
            {
                Button btn= new Button();
                btn.Height = 50;
                btn.Width = 50;
                btn.VerticalAlignment = VerticalAlignment.Center;
                btns.Add(btn);
            }
            btns[0].Content = "Add";
            btns[1].Content = "Update";
            btns[2].Content = "Load";
            btns[3].Content = "Remove";

            stckp[2].Children.Add(btns[0]);
            stckp[5].Children.Add(btns[1]);
            stckBtm.Children.Add(btns[2]);
            stckBtm.Children.Add(btns[3]);

            btns[0].Click += BtnAdd_Click;
            btns[1].Click += BtnUpd_Click;
            btns[2].Click += BtnLoad_Click;
            btns[3].Click += BtnR_Click;


            //DB Part
            NorthwindEntities nw = new NorthwindEntities();
            var stfs = from s in nw.Customers
                       select s;

            foreach (var item in stfs)
            {
                Console.WriteLine(item.ContactTitle);
                Console.WriteLine(item.ContactName);
            }
            Customer x = new Customer();


            //CREATING THE headers LIST FOR HEADERS
            headers = new List<string>();
            
            foreach (var item in x.GetType().GetProperties())
            {
                var y = item.ToString().Split(' ');
                headers.Add(y[1]);
            }
            for (int i = 0; i < headers.Count; i++)
            {
                Console.WriteLine(headers[i]);
            }
            txtBs1 = new List<TextBox>();
            txtBs2 = new List<TextBox>();

            //creating labels and textbox for add part
            for (int i = 0; i < headers.Count; i++)
            {
                //creating labels
                Label lbl = new Label() { Content = headers[i] };
                lbl.Height = 30;
                lbl.Width = 130;
                lbl.FontSize = 15;
                stckp[0].Children.Add((lbl));
                //creating textboxes

                TextBox txtBox = new TextBox();
                txtBox.FontSize = 15;
                txtBox.Tag = headers[i].ToString();
                txtBox.Height = 30;
                txtBox.Width = 250;
                stckp[1].Children.Add(txtBox);
                //txtBox.Name = "tb" + lbl.Content.ToString();
                txtBox.Name = "t"+i;
                txtBs1.Add(txtBox);

            }

            //creating labels and textbox for update part
            for (int i = 0; i < headers.Count; i++)
            {
                //creating labels
                Label lbl = new Label() { Content = headers[i] };
                lbl.Height = 30;
                lbl.Width = 130;
                lbl.FontSize = 15;
                stckp[3].Children.Add((lbl));
                //creating textboxes
                TextBox txtBox2 = new TextBox() { Text = "TextBox" };
                txtBox2.FontSize = 15;
                txtBox2.Tag = headers[i].ToString();
                txtBox2.Height = 30;
                txtBox2.Name = "tbox" + i.ToString();
                txtBox2.Width = 250;
                txtBox2.Text = "";

                stckp[4].Children.Add(txtBox2);
                txtBs2.Add(txtBox2);
            }
        }

        //UPDATE BUTTON EVENT HANDLER
        private void BtnUpd_Click(object sender, RoutedEventArgs e)
        {
            NorthwindEntities db = new NorthwindEntities();
            var r = from cst in db.Customers
                   where cst.CustomerID == this.updatingCustomerID
                    select cst;
            //whatever the result is

            Customer obj = r.SingleOrDefault(); //
            
            foreach (StackPanel cSp in stck.Children.OfType<StackPanel>())
            {//searching for top Stack
                if (cSp.Name == "stckTop")
                    foreach (StackPanel cSp2 in cSp.Children.OfType<StackPanel>())
                    {//in top stack, getting stck3(where add button is)
                        if (cSp2.Name == "stck6")
                            foreach (TextBox cTb in cSp2.Children.OfType<TextBox>())
                            {//textboxes in stck3 which keeps the values for new data

                                PropertyInfo propertyInfo = obj.GetType().GetProperty(cTb.Tag.ToString());
                                propertyInfo.SetValue(obj, Convert.ChangeType(cTb.Text, propertyInfo.PropertyType), null);
                            }
                    }
            }
            db.SaveChanges();
            dgLoad();
            
        }

        //ADD BUTTON EVENT HANDLER
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {//f11 control
            nw = new NorthwindEntities();  
            Customer co = new Customer();
          
            foreach (StackPanel cSp in stck.Children.OfType<StackPanel>())
            {//searching for top Stack
                if (cSp.Name == "stckTop")
                    foreach (StackPanel cSp2 in cSp.Children.OfType<StackPanel>())
                    {//in top stack, getting stck3(where add button is)
                        if (cSp2.Name == "stck3")
                            foreach (TextBox cTb in cSp2.Children.OfType<TextBox>())
                            {//textboxes in stck3
                                PropertyInfo propertyInfo = co.GetType().GetProperty(cTb.Tag.ToString());
                                propertyInfo.SetValue(co, Convert.ChangeType(cTb.Text, propertyInfo.PropertyType), null);
                            }
                    }
            }
            nw.Customers.Add(co);
            try
            {
                nw.SaveChanges();
                dgLoad();
            }
            catch
            {
                MessageBoxResult msgBoxResult = MessageBox.Show("You already have this Customer!",
                "Adding Error",
                MessageBoxButton.OK,
                MessageBoxImage.Warning,
                MessageBoxResult.Cancel
                );
            }
        }

        //LOAD BUTTON EVENT HANDLER
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            dgLoad();
        }

        private void dgLoad()
        {
            nw = new NorthwindEntities();
            datag.ItemsSource = nw.Customers.ToArray<Customer>();
        }

        //DELETE BUTTON EVENT HANDLER
        private void BtnR_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msgBoxResult = MessageBox.Show("Are you sure you want to remove?",
                "Remove Staff",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No
                );
            if (msgBoxResult == MessageBoxResult.Yes)
            {

                NorthwindEntities nw = new NorthwindEntities();
                var r = from cst in nw.Customers
                        where cst.CustomerID ==updatingCustomerID
                        select cst;
                //whatever the result is

                Customer obj = r.SingleOrDefault();

                if (obj != null)
                {
                    nw.Customers.Remove(obj);
                    nw.SaveChanges();
                }
            }
            dgLoad();
        }

        //SELECTION EVENT HANDLER
        private string updatingCustomerID = null;
        private void Datag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (datag.SelectedIndex != -1)
            {
                if (datag.SelectedItems.Count != -1)
                {
                    if (datag.SelectedItems[0].GetType() == typeof(Customer))
                    {
                        //user makes one selection at one time
                        Customer c = (Customer)datag.SelectedItems[0];

                        foreach (StackPanel cSp in stck.Children.OfType<StackPanel>())
                        {//searching for top Stack
                        if (cSp.Name == "stckTop")
                           foreach (StackPanel cSp2 in cSp.Children.OfType<StackPanel>())
                              {//in top stack, getting stck6, where textboxes for update is
                                if (cSp2.Name == "stck6")
                                  foreach (TextBox cTb in cSp2.Children.OfType<TextBox>())
                                    {//textboxes in stck6
                            
                                PropertyInfo propertyInfo = c.GetType().GetProperty(cTb.Tag.ToString());
                                cTb.Text =(string)propertyInfo.GetValue(c, null);
                                   }
                                }
                         }
                        updatingCustomerID = c.CustomerID;

                    }
                }
            }
        }

    }
}

