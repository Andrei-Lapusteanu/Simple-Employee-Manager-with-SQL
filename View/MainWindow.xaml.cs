using System;
using System.Collections.Generic;
using System.Linq;
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

using Controller;
using Entities;

using System.Data;
using System.ComponentModel;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Members

        DBManager dbManager = new DBManager();
        public BackgroundWorker bgWorker;
        
        string pathTick = @"C:\Users\Andrei\Documents\Visual Studio 2013\Projects\WpfApplication1\WpfApplication1\Images\tick.png";
        string pathCross = @"C:\Users\Andrei\Documents\Visual Studio 2013\Projects\WpfApplication1\WpfApplication1\Images\cross.png";
        int id;

        int salary;
        string gender;
        string hiredStatus;
        #endregion


        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            
            CheckBoxIsCheckedProp = false;
            ValidatorIconFirstNameVisibilityProp = Visibility.Hidden;
            ValidatorIconLastNameVisibilityProp = Visibility.Hidden;
            ValidatorIconSalaryVisibilityProp = Visibility.Hidden;

            FirstNameValidator();
            LastNameValidator();
            SalaryValidator();
            IsLockToHired();

            ButtonSubmit.IsEnabled = false;
            ButtonSaveEdit.IsEnabled = false;
            ButtonExitEditing.IsEnabled = false;
            ButtonEditEmployeeData.IsEnabled = false;
            ButtonDelete.IsEnabled = false;

            bgWorker = new BackgroundWorker();

            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);

            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
        }
        #endregion

        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() => { CleanThreadingLog(); }));
            this.Dispatcher.Invoke((Action)(() => { TextBoxThreading.AppendText("Task: populate DataGrid \n\n"); }));
            
            List<Employee> employeeList = new List<Employee>();

            bgWorker.ReportProgress(10);

            this.Dispatcher.Invoke((Action)(() => { TextBoxThreading.AppendText("> Loading database into list... \n"); }));
            
            employeeList = dbManager.LoadDatabase();

            bgWorker.ReportProgress(50);

            if (bgWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            
            this.Dispatcher.Invoke((Action)(() => { TextBoxThreading.AppendText("> Database has been loaded \n"); }));
            this.Dispatcher.Invoke((Action)(() => { TextBoxThreading.AppendText("> Loading list into DataGrid... \n"); }));

            System.Threading.Thread.Sleep(300);

            if (bgWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            this.Dispatcher.Invoke((Action)(() => { MyDataGrid.DataContext = employeeList; } ));
            this.Dispatcher.Invoke((Action)(() => { TextBoxThreading.AppendText("> DataGrid loaded! Task finished. \n"); }));

            bgWorker.ReportProgress(100);
        }

        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                TextBoxThreading.AppendText("\n Task has been cancelled.");
            }
            else if (e.Error != null)
            {
                TextBoxThreading.AppendText("Error occured while performing task.");
            }
            ButtonPopulate.IsEnabled = true;
        }

        void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBarThreading.Value = e.ProgressPercentage;
        }

        #region Control Events


            #region Button Events

        private void ButtonSubmit_Click(object sender, RoutedEventArgs e)
        {
            Employee employeeToAdd = new Employee();

            employeeToAdd.FirstName = TextBoxFirstName.Text;
            employeeToAdd.LastName = TextBoxLastName.Text;
            employeeToAdd.Salary = ConverterSalary();
            employeeToAdd.Gender = ConverterGender();
            employeeToAdd.HiredStatus = ConverterHiredStatus();

            dbManager.Insert(employeeToAdd);

            CleanForm();
            
            //bgWorker.RunWorkerAsync();
        }

        private void ButtonSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            Employee employeeToUpdate = new Employee();

            employeeToUpdate.FirstName = TextBoxFirstName.Text;
            employeeToUpdate.LastName = TextBoxLastName.Text;
            employeeToUpdate.Salary = ConverterSalary();
            employeeToUpdate.Gender = ConverterGender();
            employeeToUpdate.HiredStatus = ConverterHiredStatus();

            dbManager.Update(employeeToUpdate);

            DisableEditing();
            bgWorker.RunWorkerAsync(); ;
        }

        // RETURN BUTTON

        private void ButtonPopulate_Click(object sender, RoutedEventArgs e)
        {
            ButtonPopulate.IsEnabled = false;
            bgWorker.RunWorkerAsync();
        }

        private void ButtonEditEmployeeData_Click(object sender, RoutedEventArgs e)
        {
            EnableEditing();
            FillTextBoxes();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            dbManager.DeleteEmployee(id);
            bgWorker.RunWorkerAsync();
        }

        private void ButtonClearDatabase_Click(object sender, RoutedEventArgs e)
        {
            dbManager.DeleteDatabase();
            dbManager.ResetID();

        }

        private void ButtonCleanForm_Click(object sender, RoutedEventArgs e)
        {
            CleanForm();
        }

        private void ButtonAbortTask_Click(object sender, RoutedEventArgs e)
        {
            if (bgWorker.IsBusy)
            {
                bgWorker.CancelAsync();
            }

            bool EventFired =  new bool();
            EventFired = true;

            dbManager.GetAndPassEventFired(EventFired);
        }

        private void ButtonCleanThreadingText_Click(object sender, RoutedEventArgs e)
        {
            CleanThreadingLog();
        }
        #endregion


            #region Textbox Events

        private void TextBoxFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            FirstNameValidator();
            IsSubmitAvailable();
        }

        private void TextBoxFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CheckBoxUpperCase.IsChecked == true)
            {
                TextBoxFirstName.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(TextBoxFirstName.Text);
            }
        }

        private void TextBoxLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            LastNameValidator();
            IsSubmitAvailable();
        }

        private void TextBoxLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CheckBoxUpperCase.IsChecked == true)
            {
                TextBoxLastName.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(TextBoxLastName.Text);
            }
        }

        private void TextBoxSalary_TextChanged(object sender, TextChangedEventArgs e)
        {
            SalaryValidator();
            IsSubmitAvailable();
        }
        #endregion


            #region Combobox Events

        private void ComboBoxGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConverterGender();
            IsSubmitAvailable();
        }

        private void ComboBoxHiredStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConverterHiredStatus();
            IsSubmitAvailable();
        }
        #endregion


            #region Checkbox Events

        private void CheckBoxAllowChar_Click(object sender, RoutedEventArgs e)
        {
            FirstNameValidator();
            LastNameValidator();
            IsSubmitAvailable();
        }

        private void CheckBoxLockHired_Click(object sender, RoutedEventArgs e)
        {
            IsLockToHired();
        }
        #endregion


            #region DataGrid Events
        
        private void MyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Employee selecterEmployee = (Employee)MyDataGrid.SelectedItem;

            try
            {
                id = selecterEmployee.Id;
            }
            catch { }

            if (MyDataGrid.SelectedItems.Count == 1)
            {
                ButtonEditEmployeeData.IsEnabled = true;
                ButtonDelete.IsEnabled = true;
            }
            else
            {
                ButtonEditEmployeeData.IsEnabled = false;
                ButtonDelete.IsEnabled = false;
            }
        }

        //Checkedchanged
        //Salary Slider
        #endregion

        #endregion


        #region Properties

        public bool? CheckBoxIsCheckedProp { get; set; }

        public System.Windows.Visibility ValidatorIconFirstNameVisibilityProp
        {
            get { return ValidatorIconFirstName.Visibility; }

            set { ValidatorIconFirstName.Visibility = value; }
        }

        public System.Windows.Visibility ValidatorIconLastNameVisibilityProp
        {
            get { return ValidatorIconLastName.Visibility; }

            set { ValidatorIconLastName.Visibility = value; }
        }

        public System.Windows.Visibility ValidatorIconSalaryVisibilityProp
        {
            get { return ValidatorIconSalary.Visibility; }

            set { ValidatorIconSalary.Visibility = value; }
        }
        #endregion


        #region Validators

        public void FirstNameValidator()
        {
            if (CheckBoxIsCheckedProp == false)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(TextBoxFirstName.Text, @"^[a-zA-Z]+$"))
                {
                    ChangeBackgroundCross(ref ValidatorIconFirstName);

                    ValidatorIconFirstNameVisibilityProp = Visibility.Visible;
                }
                else
                {
                    ChangeBackgroundTick(ref ValidatorIconFirstName);

                    ValidatorIconFirstNameVisibilityProp = Visibility.Visible;
                }

                if (TextBoxFirstName.Text.Length == 0)
                {
                    ValidatorIconFirstNameVisibilityProp = Visibility.Hidden;
                }
            }
            else
            {
                if (TextBoxFirstName.Text.Length == 0)
                {
                    ValidatorIconFirstNameVisibilityProp = Visibility.Hidden;
                }
                else
                {
                    ChangeBackgroundTick(ref ValidatorIconFirstName);

                    ValidatorIconFirstNameVisibilityProp = Visibility.Visible;
                }
            }
        }

        public void LastNameValidator()
        {
            if (CheckBoxIsCheckedProp == false)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(TextBoxLastName.Text, @"^[a-zA-Z]+$"))
                {
                    ChangeBackgroundCross(ref ValidatorIconLastName);

                    ValidatorIconLastNameVisibilityProp = Visibility.Visible;
                }
                else
                {
                    ChangeBackgroundTick(ref ValidatorIconLastName);

                    ValidatorIconLastNameVisibilityProp = Visibility.Visible;
                }

                if (TextBoxLastName.Text.Length == 0)
                {
                    ValidatorIconLastNameVisibilityProp = Visibility.Hidden;
                }
            }
            else
            {
                if (TextBoxLastName.Text.Length == 0)
                {
                    ValidatorIconLastNameVisibilityProp = Visibility.Hidden;
                }
                else
                {
                    ChangeBackgroundTick(ref ValidatorIconLastName);

                    ValidatorIconLastNameVisibilityProp = Visibility.Visible;
                }
            }
        }


        public void SalaryValidator()
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(TextBoxSalary.Text, @"^\d+$"))
            {
                ChangeBackgroundCross(ref ValidatorIconSalary);

                ValidatorIconSalaryVisibilityProp = Visibility.Visible;
            }
            else
            {
                ChangeBackgroundTick(ref ValidatorIconSalary);

                ValidatorIconSalaryVisibilityProp = Visibility.Visible;
            }

            if (TextBoxSalary.Text.Length == 0)
            {
                ValidatorIconSalaryVisibilityProp = Visibility.Hidden;

            }
        }
        #endregion


        #region Input Converters

        public string ConverterGender()
        {
            if (ComboBoxGender.Text == "Male")
            {
                return gender = "Male";
            }
            else return gender = "Female";
        }

        public string ConverterHiredStatus()
        {
            if (ComboBoxHiredStatus.Text == "Hired")
            {
                return hiredStatus = "Hired";
            }
            else return hiredStatus = "Not hired";
        }

        public int ConverterSalary()
        {
            string salaryText = TextBoxSalary.Text;

            return salary = Convert.ToInt32(salaryText);
        }
        #endregion


        #region Methods called by control events


        #region Validator icon background change

        public Rectangle ChangeBackgroundTick(ref Rectangle rectangle)
        {
            try
            {
                rectangle.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(pathTick)) };
                rectangle.Fill.Opacity = 66;
            }
            catch { }

            return rectangle;
        }

        public Rectangle ChangeBackgroundCross(ref Rectangle rectangle)
        {
            try
            {
                rectangle.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(pathCross)) };
                rectangle.Fill.Opacity = 65;
            }
            catch { }

            return rectangle;
        }
        #endregion


        #region Enable/Disable editing

        public void EnableEditing()
        {
            ButtonSaveEdit.IsEnabled = true;
            ButtonExitEditing.IsEnabled = true;

            ButtonSubmit.IsEnabled = false;
            ButtonPopulate.IsEnabled = false;
            ButtonEditEmployeeData.IsEnabled = false;
            ButtonDelete.IsEnabled = false;
            ButtonClearDatabase.IsEnabled = false;
        }

        public void DisableEditing()
        {
            ButtonSaveEdit.IsEnabled = false;
            ButtonExitEditing.IsEnabled = false;

            ButtonPopulate.IsEnabled = true;
            ButtonClearDatabase.IsEnabled = true;
        }
        #endregion


        #region Verify button enabled status
        
        public void IsSubmitAvailable()
        {
            if (ButtonExitEditing.IsEnabled == false &&
                ValidatorIconFirstName.Fill.Opacity == 66 &&
                ValidatorIconLastName.Fill.Opacity == 66 &&
                ValidatorIconSalary.Fill.Opacity == 66 &&
                (ComboBoxGender.SelectedIndex == 0 || ComboBoxGender.SelectedIndex == 1) &&
                (ComboBoxHiredStatus.SelectedIndex == 0 || ComboBoxHiredStatus.SelectedIndex == 1))
            {
                ButtonSubmit.IsEnabled = true;
                // MessageBox.Show("Enabled.");
            }
            else
            {
                ButtonSubmit.IsEnabled = false;
                // MessageBox.Show("Disabled.");
            }

            if (ButtonExitEditing.IsEnabled == true)
            {
                IsSaveAvailable();
            }
        }

        public void IsSaveAvailable()
        {
            if (ButtonSubmit.IsEnabled == false &&
               ValidatorIconFirstName.Fill.Opacity == 66 &&
               ValidatorIconLastName.Fill.Opacity == 66 &&
               ValidatorIconSalary.Fill.Opacity == 66 &&
               (ComboBoxGender.SelectedIndex == 0 || ComboBoxGender.SelectedIndex == 1) &&
               (ComboBoxHiredStatus.SelectedIndex == 0 || ComboBoxHiredStatus.SelectedIndex == 1))
            {
                ButtonSaveEdit.IsEnabled = true;
            }
            else
            {
                ButtonSaveEdit.IsEnabled = false;
            }
        }
        #endregion


        #region Verify checkbox enabled status

        public void IsLockToHired()
        {
            if (CheckBoxLockHired.IsChecked == true)
            {
                ComboBoxHiredStatus.IsEnabled = false;
                ComboBoxHiredStatus.SelectedIndex = 0;
                ConverterHiredStatus();
            }
            else
            {
                ComboBoxHiredStatus.IsEnabled = true;
                ComboBoxHiredStatus.SelectedIndex = -1;
                ConverterHiredStatus();
            }
        }
        #endregion


        #region Cleaning

        public void CleanForm()
        {
            TextBoxFirstName.Text = "";
            TextBoxSalary.Text = "";
            TextBoxLastName.Text = "";
            TextBoxSearch.Text = "";

            if (ComboBoxHiredStatus.IsEnabled == true)
            {
                ComboBoxHiredStatus.SelectedIndex = -1;
            }

            ComboBoxGender.SelectedIndex = -1;
            ComboBoxSearchChoise.SelectedIndex = -1;
        }

        public void CleanThreadingLog()
        {
            TextBoxThreading.Text = "";
            ProgressBarThreading.Value = 0;
        }
        #endregion
      

        #region Fill textbox control

        public void FillTextBoxes()
        {
            //DataRowView drv = (DataRowView)MyDataGrid.SelectedItem;
            
            Employee employeeToEdit = (Employee)MyDataGrid.SelectedItem;

            // 1 - First name
            TextBoxFirstName.Text = employeeToEdit.FirstName;

            // 2 - Last name
            TextBoxLastName.Text = employeeToEdit.LastName;

            // 3 - Gender
            ComboBoxGender.Text = employeeToEdit.Gender;

            if (ComboBoxGender.Text == "Male")
            {
                ComboBoxGender.SelectedIndex = 0;
            }
            else ComboBoxGender.SelectedIndex = 1;

            // 4 - Salary
            TextBoxSalary.Text = employeeToEdit.Salary.ToString();

            // 5 - Hired status
            ComboBoxHiredStatus.Text = employeeToEdit.HiredStatus;

            if (ComboBoxHiredStatus.Text == "Hired")
            {
                ComboBoxHiredStatus.SelectedIndex = 0;
            }
            else ComboBoxHiredStatus.SelectedIndex = 1;
        }
        #endregion

        #endregion
    }
}
