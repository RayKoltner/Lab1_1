using System.ComponentModel.DataAnnotations;

namespace Lab1_1.Share.DTOs
{
    public class DictionaryPostDTO
    {
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "������")]
        public DateTime BeginDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "���������")]
        public DateTime EndDate { get; set; }
        [Display(Name = "���")]
        public int Code { get; set; }
        [Display(Name = "������������")]
        public string Name { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public DictionaryPostDTO() 
        {
            BeginDate = DateTime.MaxValue;
            EndDate = DateTime.MaxValue;
        }
    }
}