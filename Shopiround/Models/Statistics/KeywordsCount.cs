using System.ComponentModel.DataAnnotations.Schema;

namespace Shopiround.Models.Statistics
{
    public class KeywordsCount
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public int Count { get; set; }
    }
}
