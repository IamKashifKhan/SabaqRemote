using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SabaqRemote
{
    public partial class App : Application
    {
        public static float FontScale { get; set; }

        static SabaqRemoteDB database;
        public static string BthId { get; set; }
        public static DateTime BTLastSeen = DateTime.Now;

        public static SabaqRemoteDB Database
        {
            get
            {
                if (database == null)
                {
                    database = new SabaqRemoteDB(DependencyService.Get<IFileHelper>().GetLocalFilePath("SabaqRemote.db3"));
                }
                return database;
            }
        }
        public static string WebService = "";
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
