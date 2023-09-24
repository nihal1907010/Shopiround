using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopiround.Models.Models
{
    public class Question
    {
        public int Id { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public string Text { get; set; }
        public string Questioner {  get; set; }
        public int SerialNo { get; set; }
        
    }
}
