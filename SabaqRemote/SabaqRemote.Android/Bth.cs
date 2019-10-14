
using Android.OS;

using System;
using Android.Bluetooth;
using Java.Util;
using System.Threading.Tasks;
using Java.IO;
//using TestBth.Droid;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using SabaqRemote.Droid;
using Android.App;
using Xamarin.Forms;
using Android.Content;
using System.Diagnostics;

[assembly: Xamarin.Forms.Dependency(typeof(Bth))]
namespace SabaqRemote.Droid
{
    public class Bth : IBth
    {
        private CancellationTokenSource _ct { get; set; }
        public static BluetoothSocket BthSocket = null;
        public static bool bthCONNECTING = false;
        const int RequestResolveError = 1000;
        private bool getSDPath;
        private string baseFolderPath;

        public Bth()
        {
        }

        public void LoadSettings()
        {
            Xamarin.Forms.Forms.Context.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionLocat‌​ionSourceSettings));

        }

        #region IBth implementation

        /// <summary>
        /// Start the "reading" loop 
        /// </summary>
        /// <param name="name">Name of the paired bluetooth device (also a part of the name)</param>
        //public async  void Start(string name, int sleepTime = 200, bool readAsCharArray = false)
        //{
        //    try
        //    {
        //     await   Task.Run(async () => await loop(name, sleepTime, readAsCharArray));
        //    }
        //    catch (System.Exception e)
        //    {
        //    }
        //}
        public void Start(string name, int sleepTime = 200, bool readAsCharArray = false)
        {
            try
            {
                Task.Run(async () => loop(name, sleepTime, readAsCharArray));
            }
            catch (System.Exception e)
            {
            }
        }
        public void setbrightness(float factor)
        {
            var activity = (Activity)Forms.Context;
            var attributes = activity.Window.Attributes;

            attributes.ScreenBrightness = factor;
            activity.Window.Attributes = attributes;
        }
        public void BthCancel()
        {
            bthCONNECTING = false;
        }
        public void BthSend(string data)
        {
            //System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            //    Process.KillProcess(Process.MyPid());
            Stream outStream = null;

            //Extraemos el stream de salida
            try
            {
                outStream = BthSocket.OutputStream;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error al enviar" + e.Message);
            }

            //creamos el string que enviaremos
            Java.Lang.String message = new Java.Lang.String(data);

            //lo convertimos en bytes
            byte[] msgBuffer = message.GetBytes();

            try
            {
                //Escribimos en el buffer el arreglo que acabamos de generar
                outStream.Write(msgBuffer, 0, msgBuffer.Length);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error al enviar" + e.Message);
                Xamarin.Forms.MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "Barcode", "BT DISCONNECTED");
                _ct.Cancel();

            }

        }

        //private async Task loop(string name, int sleepTime, bool readAsCharArray)
        //{

        //    if (bthCONNECTING == true)
        //    { return; }
        //    bthCONNECTING = true;
        //    if (!(DateTime.Now - new TimeSpan(0, 0, 0, 10, 0) > App.BTLastSeen))
        //    {
        //        return;
        //    }
        //    BluetoothDevice device = null;
        //    BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
        //    //BluetoothSocket BthSocket = null;
        //    Stream outStream = null;

        

        //    //Thread.Sleep(1000);
        //    _ct = new CancellationTokenSource();
        //    while (_ct.IsCancellationRequested == false)
        //    {

        //        try
        //        {
        //            Thread.Sleep(sleepTime);

        //            adapter = BluetoothAdapter.DefaultAdapter;
        //            if (adapter == null)
        //                System.Diagnostics.Debug.WriteLine("No Bluetooth adapter found.");
        //            else
        //                System.Diagnostics.Debug.WriteLine("Adapter found!!");

        //            if (!adapter.IsEnabled)
        //                System.Diagnostics.Debug.WriteLine("Bluetooth adapter is not enabled.");
        //            else
        //                System.Diagnostics.Debug.WriteLine("Adapter enabled!");

        //            System.Diagnostics.Debug.WriteLine("Try to connect to " + name);


        //            foreach (var bd in adapter.BondedDevices)
        //            {
        //                System.Diagnostics.Debug.WriteLine("Paired devices found: " + bd.Name.ToUpper());
        //                if (bd.Name.ToUpper().IndexOf(name.ToUpper()) >= 0)
        //                {

        //                    System.Diagnostics.Debug.WriteLine("Found " + bd.Name + ". Try to connect with it!");
        //                    device = bd;
        //                    break;
        //                }
        //            }

        //            if (device == null)
        //            {
        //                System.Diagnostics.Debug.WriteLine("Named device not found.");
        //                if (_ct != null)
        //                {
        //                    System.Diagnostics.Debug.WriteLine("Send a cancel to task!");
        //                    _ct.Cancel();
        //                }
        //            }
        //            else
        //            {
        //                UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
        //                if ((int)Android.OS.Build.VERSION.SdkInt >= 10) // Gingerbread 2.3.3 2.3.4
        //                    BthSocket = device.CreateInsecureRfcommSocketToServiceRecord(uuid);
        //                else
        //                    BthSocket = device.CreateRfcommSocketToServiceRecord(uuid);

        //                if (BthSocket != null)
        //                {



        //                    //Task.Run ((Func<Task>)loop); /*) => {
        //                    await BthSocket.ConnectAsync();


        //                    if (BthSocket.IsConnected)
        //                    {
        //                        /*
        //                        //Extraemos el stream de salida
        //                        try
        //                        {
        //                            outStream = BthSocket.OutputStream;
        //                        }
        //                        catch (System.Exception e)
        //                        {
        //                            System.Console.WriteLine("Error al enviar" + e.Message);
        //                        }

        //                        //creamos el string que enviaremos
        //                        Java.Lang.String message = new Java.Lang.String(name);

        //                        //lo convertimos en bytes
        //                        byte[] msgBuffer = message.GetBytes();

        //                        try
        //                        {
        //                            //Escribimos en el buffer el arreglo que acabamos de generar
        //                            outStream.Write(msgBuffer, 0, msgBuffer.Length);
        //                        }
        //                        catch (System.Exception e)
        //                        {
        //                            System.Console.WriteLine("Error al enviar" + e.Message);
        //                        }
        //                        */
        //                        Xamarin.Forms.MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "Barcode", "BT CONNECTED");

        //                        System.Diagnostics.Debug.WriteLine("Connected!");
        //                        var mReader = new InputStreamReader(BthSocket.InputStream);
        //                        var buffer = new BufferedReader(mReader);
        //                        //buffer.re
        //                        while (_ct.IsCancellationRequested == false)
        //                        {
        //                            if (buffer.Ready())
        //                            {
        //                                //										string barcode =  buffer
        //                                //string barcode = buffer.

        //                                //string barcode = await buffer.ReadLineAsync();
        //                                char[] chr = new char[200];
        //                                //await buffer.ReadAsync(chr);
        //                                string barcode = "";
        //                                if (readAsCharArray)
        //                                {

        //                                    await buffer.ReadAsync(chr);
        //                                    foreach (char c in chr)
        //                                    {

        //                                        if (c == '\0')
        //                                            break;
        //                                        barcode += c;
        //                                    }

        //                                }
        //                                else
        //                                    barcode = await buffer.ReadLineAsync();

        //                                if (barcode.Length > 0)
        //                                {
        //                                    System.Diagnostics.Debug.WriteLine("Letto: " + barcode);
        //                                    Xamarin.Forms.MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "Barcode", barcode);
        //                                }
        //                                else
        //                                    System.Diagnostics.Debug.WriteLine("No data");

        //                            }
        //                            else
        //                            {
        //                                System.Diagnostics.Debug.WriteLine("No data to read");
        //                            }
        //                            // A little stop to the uneverending thread...
        //                            System.Threading.Thread.Sleep(sleepTime);
        //                            Stopwatch timer = Stopwatch.StartNew();
        //                            try
        //                            {
        //                                //   await BthSocket.ConnectAsync();
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                timer.Stop();
        //                                TimeSpan timespan = timer.Elapsed;
        //                                if (timespan.Seconds > 5)
        //                                {
        //                                    if (_ct != null)
        //                                    {
        //                                        Xamarin.Forms.MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "Barcode", "BT DISCONNECTED");
        //                                        //                                                System.Diagnostics.Debug.WriteLine("DEVICE SEEMS OFFLINE Send a cancel to task!");
        //                                        _ct.Cancel();
        //                                    }

        //                                }
        //                                //     System.Diagnostics.Debug.WriteLine(String.Format("{0:00}:{1:00}:{2:00}", timespan.Minutes, timespan.Seconds, timespan.Milliseconds / 10));
        //                            }

        //                            if (!BthSocket.IsConnected)
        //                            {
        //                                System.Diagnostics.Debug.WriteLine("BthSocket.IsConnected = false, Throw exception");
        //                                throw new Exception();
        //                            }
        //                        }

        //                        System.Diagnostics.Debug.WriteLine("Exit the inner loop");

        //                    }
        //                    else
        //                    {
        //                        System.Diagnostics.Debug.WriteLine("NOT AVAILABLE Connected!");
        //                    }
        //                }
        //                else
        //                    System.Diagnostics.Debug.WriteLine("BthSocket = null");
        //                if (_ct != null)
        //                {
        //                    System.Diagnostics.Debug.WriteLine("Send a cancel to task!");
        //                    _ct.Cancel();
        //                }

        //            }


        //        }
        //        catch (Exception ex)
        //        {
        //            System.Diagnostics.Debug.WriteLine("EXCEPTION: " + ex.Message);
        //        }

        //        finally
        //        {
        //            if (BthSocket != null)
        //                BthSocket.Close();
        //            device = null;
        //            adapter = null;
        //            if (_ct != null)
        //            {
        //                System.Diagnostics.Debug.WriteLine("Send a cancel to task!");
        //                _ct.Cancel();
        //            }
        //        }
        //    }
        //    bthCONNECTING = false;
        //    System.Diagnostics.Debug.WriteLine("Exit the external loop");
        //}
        private async Task loop(string name, int sleepTime, bool readAsCharArray)
        {

            if (bthCONNECTING == true)
            { return; }
            bthCONNECTING = true;
            BluetoothDevice device = null;
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            //BluetoothSocket BthSocket = null;

            Stream outStream = null;



            //Thread.Sleep(1000);
            _ct = new CancellationTokenSource();
            while (_ct.IsCancellationRequested == false)
            {

                try
                {
                    Thread.Sleep(sleepTime);

                    adapter = BluetoothAdapter.DefaultAdapter;

                    if (adapter == null)
                        System.Diagnostics.Debug.WriteLine("No Bluetooth adapter found.");
                    else
                        System.Diagnostics.Debug.WriteLine("Adapter found!!");

                    if (!adapter.IsEnabled)
                        System.Diagnostics.Debug.WriteLine("Bluetooth adapter is not enabled.");
                    else
                        System.Diagnostics.Debug.WriteLine("Adapter enabled!");

                    System.Diagnostics.Debug.WriteLine("Try to connect to " + name);

                    foreach (var bd in adapter.BondedDevices)
                    {
                        System.Diagnostics.Debug.WriteLine("Paired devices found: " + bd.Name.ToUpper());
                        if (bd.Name.ToUpper().IndexOf(name.ToUpper()) >= 0)
                        {

                            System.Diagnostics.Debug.WriteLine("Found " + bd.Name + ". Try to connect with it!");
                            device = bd;
                            break;
                        }
                    }

                    if (device == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Named device not found.");
                        if (_ct != null)
                        {
                            System.Diagnostics.Debug.WriteLine("Send a cancel to task!");
                            _ct.Cancel();
                        }
                    }
                    else
                    {
                        UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                        if ((int)Android.OS.Build.VERSION.SdkInt >= 10) // Gingerbread 2.3.3 2.3.4
                            BthSocket = device.CreateInsecureRfcommSocketToServiceRecord(uuid);
                        else
                            BthSocket = device.CreateRfcommSocketToServiceRecord(uuid);

                        if (BthSocket != null)
                        {



                            //Task.Run ((Func<Task>)loop); /*) => {
                            await BthSocket.ConnectAsync();


                            if (BthSocket.IsConnected)
                            {
                                /*
                                //Extraemos el stream de salida
                                try
                                {
                                    outStream = BthSocket.OutputStream;
                                }
                                catch (System.Exception e)
                                {
                                    System.Console.WriteLine("Error al enviar" + e.Message);
                                }

                                //creamos el string que enviaremos
                                Java.Lang.String message = new Java.Lang.String(name);

                                //lo convertimos en bytes
                                byte[] msgBuffer = message.GetBytes();

                                try
                                {
                                    //Escribimos en el buffer el arreglo que acabamos de generar
                                    outStream.Write(msgBuffer, 0, msgBuffer.Length);
                                }
                                catch (System.Exception e)
                                {
                                    System.Console.WriteLine("Error al enviar" + e.Message);
                                }
                                */
                                Xamarin.Forms.MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "Barcode", "BT CONNECTED");

                                System.Diagnostics.Debug.WriteLine("Connected!");
                                var mReader = new InputStreamReader(BthSocket.InputStream);
                                var buffer = new BufferedReader(mReader);
                                //buffer.re
                                while (_ct.IsCancellationRequested == false)
                                {
                                    if (buffer.Ready())
                                    {
                                        //										string barcode =  buffer
                                        //string barcode = buffer.

                                        //string barcode = await buffer.ReadLineAsync();
                                        char[] chr = new char[200];
                                        //await buffer.ReadAsync(chr);
                                        string barcode = "";
                                        if (readAsCharArray)
                                        {

                                            await buffer.ReadAsync(chr);
                                            foreach (char c in chr)
                                            {

                                                if (c == '\0')
                                                    break;
                                                barcode += c;
                                            }

                                        }
                                        else
                                            barcode = await buffer.ReadLineAsync();

                                        if (barcode.Length > 0)
                                        {
                                            System.Diagnostics.Debug.WriteLine("Letto: " + barcode);
                                            Xamarin.Forms.MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "Barcode", barcode);
                                        }
                                        else
                                            System.Diagnostics.Debug.WriteLine("No data");

                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine("No data to read");
                                    }
                                    // A little stop to the uneverending thread...
                                    System.Threading.Thread.Sleep(sleepTime);
                                    Stopwatch timer = Stopwatch.StartNew();
                                    try
                                    {
                                        //   await BthSocket.ConnectAsync();
                                    }
                                    catch (Exception ex)
                                    {
                                        timer.Stop();
                                        TimeSpan timespan = timer.Elapsed;
                                        if (timespan.Seconds > 5)
                                        {
                                            if (_ct != null)
                                            {
                                                Xamarin.Forms.MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "Barcode", "BT DISCONNECTED");
                                                //                                                System.Diagnostics.Debug.WriteLine("DEVICE SEEMS OFFLINE Send a cancel to task!");
                                                _ct.Cancel();
                                            }

                                        }
                                        //     System.Diagnostics.Debug.WriteLine(String.Format("{0:00}:{1:00}:{2:00}", timespan.Minutes, timespan.Seconds, timespan.Milliseconds / 10));
                                    }

                                    if (!BthSocket.IsConnected)
                                    {
                                        System.Diagnostics.Debug.WriteLine("BthSocket.IsConnected = false, Throw exception");
                                        throw new Exception();
                                    }
                                }

                                System.Diagnostics.Debug.WriteLine("Exit the inner loop");

                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("NOT AVAILABLE Connected!");
                            }
                        }
                        else
                            System.Diagnostics.Debug.WriteLine("BthSocket = null");
                        if (_ct != null)
                        {
                            System.Diagnostics.Debug.WriteLine("Send a cancel to task!");
                            _ct.Cancel();
                        }

                    }


                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("EXCEPTION: " + ex.Message);
                }

                finally
                {
                    if (BthSocket != null)
                        BthSocket.Close();
                    device = null;
                    adapter = null;
                    if (_ct != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Send a cancel to task!");
                        _ct.Cancel();
                    }
                }
            }
            bthCONNECTING = false;
            System.Diagnostics.Debug.WriteLine("Exit the external loop");
        }

        /// <summary>
        /// Cancel the Reading loop
        /// </summary>
        /// <returns><c>true</c> if this instance cancel ; otherwise, <c>false</c>.</returns>
        public void Cancel()
        {
            //try
            //{
            //    Context context = Android.App.Application.Context;
            //    Java.IO.File[] dirs = context.GetExternalFilesDirs(null);

            //    foreach (Java.IO.File folder in dirs)
            //    {
            //        bool IsRemovable = Android.OS.Environment.InvokeIsExternalStorageRemovable(folder);
            //        bool IsEmulated = Android.OS.Environment.InvokeIsExternalStorageEmulated(folder);

            //        if (getSDPath ? IsRemovable && !IsEmulated : !IsRemovable && IsEmulated)
            //            baseFolderPath = folder.Path;
            //    }
            //}

            //catch (Exception ex)
            //{
            //    // Console.WriteLine("GetBaseFolderPath caused the follwing exception: {0}", ex);
            //}
            if (_ct != null)
            {
                System.Diagnostics.Debug.WriteLine("Send a cancel to task!");
                _ct.Cancel();
            }
        }

        public ObservableCollection<string> PairedDevices()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            ObservableCollection<string> devices = new ObservableCollection<string>();

            foreach (var bd in adapter.BondedDevices)
                devices.Add(bd.Name);

            return devices;
        }
        public String StoragePaths()
        {
            String DrivePath = "";
            //          List<string> Paths = new List<string>();
            try
            {
                Context context = Android.App.Application.Context;
                Java.IO.File[] dirs = context.GetExternalFilesDirs(null);

                foreach (Java.IO.File folder in dirs)
                {
                    //                    Paths.Add(folder.Path.ToString());
                    bool IsRemovable = Android.OS.Environment.InvokeIsExternalStorageRemovable(folder);
                    if (IsRemovable)
                    {
                        DrivePath = folder.Path;
                    }//bool IsEmulated = Android.OS.Environment.InvokeIsExternalStorageEmulated(folder);

                    //if (getSDPath ? IsRemovable && !IsEmulated : !IsRemovable && IsEmulated)
                    //    baseFolderPath = folder.Path;
                }
                //              var externalPath = Android.OS.Environment.ExternalStorageDirectory;
                //            Paths.Add(externalPath.Path.ToString());
            }


            catch (Exception ex)
            {
                // Console.WriteLine("GetBaseFolderPath caused the follwing exception: {0}", ex);
            }
            //            return Paths;
            return DrivePath;
        }





        #endregion
    }
}

