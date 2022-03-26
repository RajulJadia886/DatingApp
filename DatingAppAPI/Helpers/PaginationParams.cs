using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Helpers
{
    public class PaginationParams
    {
        
        private const int MaxSize = 50;
        public int PageNumber { get; set;} = 1;
        private int _pageSize = 10;
        public int PageSize {
            get => _pageSize;
            set => _pageSize = (value > MaxSize) ? MaxSize : value;
        }
    }
}