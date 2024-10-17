using Lab1_1.Share.DTOs;
namespace Lab1_1.Contracts
{
    public interface IDictionaryXMLReader
    {
        List<DictionaryXMLDTO> ReadFromXml(MemoryStream file);
    }
}
