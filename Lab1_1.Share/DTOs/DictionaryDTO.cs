using System.ComponentModel.DataAnnotations;

namespace Lab1_1.Share.DTOs
{
    public class DictionaryDTO : BaseDTO
    {
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Начало")]
        public DateTime BeginDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Окончание")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Код")]
        public int Code { get; set; }
        [Display(Name = "Наименование")]
        public string Name { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public DictionaryDTO() //Это должен быть конструктор? Я хз, что это (Оригинально метод звался DictionaryModel)
        {
            BeginDate = DateTime.MaxValue;
            EndDate = DateTime.MaxValue;
        }
    }
}
