using Lab1_1.Data.Model;
using Lab1_1.Share.DTOs;

namespace Lab1_1.Mappers
{
    public static class DictPostDTOToN018DictMapper
    {
        public static N018Dictionary Convert(DictionaryPostDTO DTO)
        { 
            N018Dictionary N018 = new N018Dictionary();
            N018.Name = DTO.Name;
            N018.Code = DTO.Code;
            N018.BeginDate = DTO.BeginDate.ToUniversalTime();
            N018.EndDate = DTO.EndDate.ToUniversalTime();
            N018.Comments = DTO.Comments;
            return N018;
        }
    }
}
