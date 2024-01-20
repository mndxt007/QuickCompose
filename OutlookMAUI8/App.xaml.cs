namespace OutlookMAUI8
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

			// Check if the device is in dark mode
			

			MainPage = new MainPage();
        }
    }
}
