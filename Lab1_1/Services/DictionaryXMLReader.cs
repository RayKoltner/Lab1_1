using System.Text;
using System.Xml.Linq;
using Lab1_1.Share.DTOs;

namespace Lab1_1.Services
{
    internal class DictionaryXMLReader {  
        public List<DictionaryXMLDTO> ReadFromXml(MemoryStream file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding windows1251 = Encoding.GetEncoding("windows-1251");

            var reader = new StreamReader(file, windows1251);

            XDocument rawData = XDocument.Load(file);
            XElement package = rawData.Element("packet");
            
            List<DictionaryXMLDTO > result = new List<DictionaryXMLDTO>();

            foreach (XElement note in package.Elements("zap")) {
                DictionaryXMLDTO column = new DictionaryXMLDTO();

                column.Code = int.Parse(note.Element("ID_REAS").Value);
                column.Name = note.Element("REAS_NAME").Value;
                column.BeginDate = DateTime.Parse(note.Element("DATEBEG").Value);
                if (!note.Element("DATEEND").Value.Equals(""))
                {
                    column.EndDate = DateTime.Parse(note.Element("DATEEND").Value);
                }
                result.Add(column);
            }

            return result;
        }
    }
}
