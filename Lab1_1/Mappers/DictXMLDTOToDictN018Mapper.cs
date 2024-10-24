using Lab1_1.Share.DTOs;
using Lab1_1.Data.Model;

namespace Lab1_1.Mappers
{
    static class DictXMLDTOToDictN018Mapper
    {
        public static N018Dictionary Convert(DictionaryXMLDTO DTO) 
        {
            N018Dictionary N018 = new N018Dictionary();
            N018.Name = DTO.Name;
            N018.Code = DTO.Code;
            N018.BeginDate = DTO.BeginDate.ToUniversalTime();
            N018.EndDate = DTO.EndDate.ToUniversalTime();
            return N018;
        }
    }
}
