namespace TestFromGitToMongo.Services
{
    public class AppData
    {
        //this value could be used in all components but would need to implement the code below so that all subsribbed components get updated 
        //taken from https://wellsb.com/csharp/aspnet/blazor-singleton-pass-data-between-pages
        public bool ShowOldBikesInBikesPage { get; set; }

        private bool _ShowOldBikesInBikesPage;
        //public bool _ShowOldBikesInBikesPage
        //{
        //    get
        //    {
        //        return _ShowOldBikesInBikesPage;
        //    }
        //    set
        //    {
        //        _ShowOldBikesInBikesPage = value;
        //        NotifyDataChanged();
        //    }
        //}
        //public event Action OnChange;

        //private void NotifyDataChanged() => OnChange?.Invoke();
    }
}
