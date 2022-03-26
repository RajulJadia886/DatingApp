namespace DatingAppAPI.Helpers
{
    public class UserParams
    {
        private const int MaxSize = 50;
        public int PageNumber { get; set;} = 1;
        private int _pageSize = 10;
        public int PageSize {
            get => _pageSize;
            set => _pageSize = (value > MaxSize) ? MaxSize : value;
        }
        public string CurrentUsername { get; set; }
        public string Gender { get; set;}
        public int minAge { get; set; } = 18;
        public int maxAge { get; set; } = 150;

        public string OrderBy { get; set; } = "lastActive";
    }
}