using KanbApp.Services;

namespace KanbApp
{
    public partial class App : Application
    {
        public static LocalDbService Database { get; private set; }
        public App()
        {
            InitializeComponent();

            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "local_db.db3");

            Database = new LocalDbService(dbPath);

            MainPage = new AppShell();
        }
    }
}
