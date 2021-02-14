using System.Collections.Generic;

namespace UfoWeb.Models
{
    public class HomeViewModel
    {
        public HomeViewModel(Dictionary<string, int> data)
        {
            Data = data;
        }

        public Dictionary<string, int> Data { get; }
    }
}