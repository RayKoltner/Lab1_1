using Lab1;
using Lab1_1.Contracts;
using Lab1_1.Data;
using Lab1_1.Data.Model;
using Lab1_1.Repositories;
using Lab1_1.Share.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Lab1_1.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DictionaryController : ControllerBase
    {
        private readonly ILogger<DictionaryController> _logger;
        private readonly IRepository<N018Dictionary> dictionaryRepo;

        public DictionaryController(ILogger<DictionaryController> logger)
        {
            _logger = logger;
            dictionaryRepo = new N018Repository<N018Dictionary>(new ApplicationContext());
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
                //Вот эта вся логика должна быть в репозитории, но я не смог это реализовать
                N018Dictionary addedDict = new N018Dictionary();
                addedDict.Name = recievedData.Name;
                addedDict.Code = recievedData.Code;
                addedDict.BeginDate = recievedData.BeginDate.ToUniversalTime();
                addedDict.EndDate = recievedData.EndDate.ToUniversalTime();
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

        [HttpDelete]
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
            var reader = new DictionaryXMLReader();
            MemoryStream stream = new MemoryStream();
            formFile.CopyTo(stream);
            stream.Position = 0;
            List<DictionaryXMLDTO> newEntries = reader.ReadFromXml(stream);
            foreach (var existing in dictionaryRepo.GetAll())
            {
                dictionaryRepo.Delete(existing); //По какой-то причине не работает виртуальное удаление
            }
            foreach (var newEntry in newEntries)
            {
                N018Dictionary addedDict = new N018Dictionary();
                addedDict.Name = newEntry.Name;
                addedDict.Code = newEntry.Code;
                addedDict.BeginDate = newEntry.BeginDate.ToUniversalTime();
                addedDict.EndDate = newEntry.EndDate.ToUniversalTime();
                dictionaryRepo.Add(addedDict);
            }
            dictionaryRepo.Save();
            return Ok();
        }
    }
}
