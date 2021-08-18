using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunoController : ControllerBase
    {
        
        public readonly IRepository _repo;

        public AlunoController(IRepository repo)
        {
            _repo = repo;
        }

        /*
        public List<Aluno> Alunos = new List<Aluno>() {
            new Aluno(){
                Id = 1,
                Nome = "Marcos",
                Sobrenome = "Almeida",
                Telefone = "123456"
            },
            new Aluno(){
                Id = 2,
                Nome = "Marta",
                Sobrenome = "Kente",
                Telefone = "5465456"
            },
            new Aluno(){
                Id = 3,
                Nome = "Laura",
                Sobrenome = "Maria",
                Telefone = "8564546"
            },            
        };*/


        [HttpGet]
        public IActionResult Get()
        {
            var result = _repo.GetAllAlunos(true);
            return Ok(result);
        }

        [HttpGet("{id}")] //exemplo alterando rota
        //[HttpGet("byId")] //exemplo queryString http://localhost:5000/api/aluno/byId?id=1 
        //[HttpGet("{Id:int}")] //exemplo definindo tipo
        public IActionResult GetById(int id)
        {
            var aluno = _repo.GetAlunoById(id, false);

            if (aluno == null) return BadRequest("O aluno não foi encontrado");

            return Ok(aluno);
        }
    /*
        [HttpGet("byName")] // http://localhost:5000/api/aluno/byName?nome=Marta&sobrenome=Kent
        public IActionResult GetByName(string nome, string sobrenome)
        {
            var aluno = _context.Alunos.FirstOrDefault(a =>
            a.Nome.Contains(nome) && a.Sobrenome.Contains(sobrenome));

            if (aluno == null) return BadRequest("O aluno não foi encontrado");

            return Ok(aluno);
        }
        */

        [HttpPost]
        public IActionResult Post(Aluno aluno)
        {
            _repo.Add(aluno);
            if(_repo.SaveChanges()) return Ok(aluno);
            
            return BadRequest("Aluno não cadastrado");
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Aluno aluno)
        {

            var alu = _repo.GetAlunoById(id);

            if (alu == null) return BadRequest("Aluno não encontrado");

            _repo.Update(aluno);
            if(_repo.SaveChanges()) return Ok(aluno);
            
            return BadRequest("Aluno não atualizado");
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Aluno aluno)
        {
            var alu = _repo.GetAlunoById(id);

            if (alu == null) return BadRequest("Aluno não encontrado");
            _repo.Update(aluno);

            if(_repo.SaveChanges()) return Ok(aluno);
            
            return BadRequest("Aluno não atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var alu = _repo.GetAlunoById(id);
            if (alu == null) return BadRequest("Aluno não encontrado");
            
            _repo.Delete(alu);
            if(_repo.SaveChanges()) return Ok("Aluno deletado");
            
            return BadRequest("Aluno não deletado");
        }
    }
}