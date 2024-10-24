using Lab1_1.Contracts;
using Lab1_1.Data;
using Lab1_1.Data.Model;
using Lab1_1.Repositories;
using Lab1_1.Share.DTOs;
using Microsoft.AspNetCore.Mvc;
using Lab1_1.Mappers;
using Lab1_1.Services;

namespace Lab1_1.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DictionaryController : ControllerBase
    {
        private readonly ILogger<DictionaryController> _logger;
        private readonly IRepository<N018Dictionary> dictionaryRepo;

        public DictionaryController(ILogger<DictionaryController> logger, IRepository<N018Dictionary> repo)
        {
            _logger = logger;
            dictionaryRepo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(dictionaryRepo.GetAll());
        }


        [HttpGet("getById/{id}")]
        public IActionResult Get(int id)
        {
            var result = dictionaryRepo.GetByKey(id);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] DictionaryPostDTO recievedData)
        {
            try
            {
                // Теперь с маппером
                var addedDict = DictPostDTOToN018DictMapper.Convert(recievedData);
                dictionaryRepo.Add(addedDict);
                dictionaryRepo.Save();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] DictionaryDTO recievedData) 
        {
            try
            {
                var existing = dictionaryRepo.GetByKey(recievedData.Id);
                if (existing == null) return NotFound();

                // Это я решил оставить без маппера, поскольку здесь идёт замена части полей сущности,
                // а не создание новой
                existing.Name = recievedData.Name;
                existing.Code = recievedData.Code;
                existing.BeginDate = recievedData.BeginDate.ToUniversalTime();
                existing.EndDate = recievedData.EndDate.ToUniversalTime();
                existing.Comments = recievedData.Comments;

                dictionaryRepo.Edit(existing);
                dictionaryRepo.Save();
                return Ok(recievedData.Id);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = dictionaryRepo.GetByKey(id);
            if (existing == null) return NotFound();

            dictionaryRepo.VirtualDelete(existing, 0); //Пока пользователей нет, так что удаляет всех 0
            return Ok();
        }

        [HttpPost("uploadFile")]
        public IActionResult PostFromFile(IFormFile formFile)
        {
            try
            {
                if (formFile.ContentType != "text/xml" && formFile.ContentType != "application/xml")
                {
                    Console.WriteLine(formFile.ContentType);
                    throw new Exception();
                }
                var reader = new DictionaryXMLReader();
                MemoryStream stream = new MemoryStream();
                formFile.CopyTo(stream);
                stream.Position = 0;
                List<DictionaryXMLDTO> newEntries = reader.ReadFromXml(stream);
                if (!newEntries.Any()) //Если прислать xml, но без записей, то он распарсится и вернётся как пустой список
                {
                    throw new Exception();
                }
                foreach (var existing in dictionaryRepo.GetAll())
                {
                    dictionaryRepo.Delete(existing); //По какой-то причине не работает виртуальное удаление
                }
                foreach (var newEntry in newEntries)
                {
                    var addedDict = DictXMLDTOToDictN018Mapper.Convert(newEntry);
                    dictionaryRepo.Add(addedDict);
                }
                dictionaryRepo.Save();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
