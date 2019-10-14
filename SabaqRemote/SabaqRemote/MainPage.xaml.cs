using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SabaqRemote
{

    public partial class MainPage : ContentPage
    {


        public String LastUpdateCommand = "";

        private static String CSV = "";
        private static String CSVTimeStamp = "";
        private bool btnRelease;

        private static string PressureLimit = "";
        private static string PressureLimitBT = "";
        private static bool AccStatus = false;

        public float MX = 0;
        public float MY = 0;
        public float MZ = 0;
        bool UpdateGdataStatus = false;

        public ObservableCollection<string> ListOfDevices { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<BLDataItem> BLDataList { get; set; } = new ObservableCollection<BLDataItem>();
        private static DateTime TimeNow;
        private static TimeSpan TimeDiff;
        private static int TapCount = 0;
        DateTime Starttime = DateTime.Now;
        private static TimeSpan PressDurartionSec = new TimeSpan(0, 0, 0, 2, 0);

        Int32 RCount = 0;
        public MainPage()
        {
            InitializeComponent();
            App.Database.DeleteSensorData();
            BlueToothConnectOnStart();
            ResulList.ItemSelected += (sender, e) => { ((ListView)sender).SelectedItem = null; };

            

         //   PopulateListData();
            //OnStart()            

            MessagingCenter.Subscribe<App, string>(this, "Barcode", async (nothing, arg) =>
            {

                // Add the barcode to a list (first position)
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        var BTdata = arg.ToString();
 
                        if (BTdata.Contains("2688"))
                        {
                            BLDataList.Add(new BLDataItem { TxtParameter ="Student 1", TxtValue =  "Result 1"  });

                        }
                    
                        else if (BTdata.Contains("2689"))
                        {
                            BLDataList.Add(new BLDataItem { TxtParameter = "Student 1", TxtValue = "Result 2" });

                        }
                        else if (BTdata.Contains("2690"))
                        {
                            BLDataList.Add(new BLDataItem { TxtParameter = "Student 1", TxtValue = "Result 3" });

                        }
                        else if (BTdata.Contains("2691"))
                        {
                            BLDataList.Add(new BLDataItem { TxtParameter = "Student 1", TxtValue = "Result 4" });

                        }
                        else if (BTdata.Contains("2704"))
                        {
                            BLDataList.Add(new BLDataItem { TxtParameter = "Student 2", TxtValue = "Result 1" });

                        }
                        else if (BTdata.Contains("2705"))
                        {
                            BLDataList.Add(new BLDataItem { TxtParameter = "Student 2", TxtValue = "Result 2" });

                        }
                        else if (BTdata.Contains("2706"))
                        {
                            BLDataList.Add(new BLDataItem { TxtParameter = "Student 2", TxtValue = "Result 3" });

                        }
                        else if (BTdata.Contains("2707"))
                        {
                            BLDataList.Add(new BLDataItem { TxtParameter = "Student 2", TxtValue = "Result 3" });

                        }



                        BLDataList = new ObservableCollection<BLDataItem>(BLDataList.Distinct().ToList());
                        ResulList.ItemsSource = BLDataList;

                    }
                    catch
                    {

                    }

                });
                // ListOfBarcodes.Insert(0, arg);
            });
        }

        private async void PopulateListData()
        {
            for (int i = 1; i <= 10; i++)
            {
                BLDataList.Add(new BLDataItem { TxtParameter = "Paramete no " + i.ToString(), TxtValue = "Value = " + (i * 31).ToString() });
                await Task.Delay(500);
            }

        }

        private async void SelectedBthdevice(object sender, EventArgs e)
        {
            if (BthPicker.SelectedItem == null) { return; }
            App.BthId = BthPicker.SelectedItem.ToString();
            var savedPrinter = await App.Database.GetBthDeviceList();
            if (savedPrinter != null || savedPrinter.Count() > 0)
            {
                App.Database.DeleteAllDevices();
            }

            App.Database.AddBLDevice(App.BthId, App.BthId);

            BlueToothConnect();
        }
        private async  void BlueToothConnect()
        {
            await Task.Delay(2000);
            DependencyService.Get<IBth>().Start(App.BthId, 250, true);
            await Task.Delay(3000);
        }
        private async void LikeButton_Clicked(object sender, EventArgs e)
        {

        }

        private void LikeButton_Pressed(object sender, EventArgs e)
        {
            if (BthPicker.Items.Count > 0)
            {
                BthPicker.ItemsSource = null;
                BthPicker.Items.Clear();
            }
            try
            {
                //   At startup, I load all paired devices
                ListOfDevices = DependencyService.Get<IBth>().PairedDevices();
                BthPicker.ItemsSource = ListOfDevices;
            }
            catch { }
            btnRelease = false;
            Starttime = DateTime.Now;
            //  CheckTickCount(sender);
            BthPicker.Focus(); return;
        }
        private async void CheckTickCount(object sender = null)
        {
            if ((DateTime.Now - Starttime) > new TimeSpan(0, 0, 0, 0, 500))
            {
                if (BthPicker.Items.Count > 0)
                {
                    BthPicker.ItemsSource = null;
                    BthPicker.Items.Clear();
                }
                try
                {
                    //   At startup, I load all paired devices
                    ListOfDevices = DependencyService.Get<IBth>().PairedDevices();
                    BthPicker.ItemsSource = ListOfDevices;
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Attention", ex.Message, "Ok");
                }

                BthPicker.Focus(); return;
            }
            await Task.Delay(250);
            if (!btnRelease) { CheckTickCount(sender); }

        }
        private async void BlueToothConnectOnStart()
        {
            try
            {
                await Task.Delay(1000);
                DependencyService.Get<IBth>().Start("HC-06", 250, true);
                await Task.Delay(5000);
            }

            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Attention", ex.Message.ToString(), "Ok");

            }

        }


        private async void LikeButton_Released(object sender, EventArgs e)
        {
            btnRelease = true;
            // Starttime = DateTime.Now;
            if ((DateTime.Now - Starttime) < new TimeSpan(0, 0, 0, 0, 500))
            {

            }


        }

        private void ResultList_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }

    public class BLDataItem
    {
        public string TxtParameter { get; set; }
        public string TxtValue { get; set; }
    }
}
