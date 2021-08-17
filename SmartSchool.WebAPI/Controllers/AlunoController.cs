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
        private readonly SmartContext _context;

        public AlunoController(SmartContext context)
        {
            _context = context;
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
        public IActionResult Get(){
            return Ok(_context.Alunos);
        }

        [HttpGet("byId/{id}")] //exemplo alterando rota
        //[HttpGet("byId")] //exemplo queryString http://localhost:5000/api/aluno/byId?id=1 
        //[HttpGet("{Id:int}")] //exemplo definindo tipo
        public IActionResult GetById(int id){
            var aluno = _context.Alunos.FirstOrDefault(a => a.Id == id);

            if(aluno == null) return BadRequest("O aluno não foi encontrado");

            return Ok(aluno);
        }

        [HttpGet("byName")] // http://localhost:5000/api/aluno/byName?nome=Marta&sobrenome=Kent
        public IActionResult GetByName(string nome, string sobrenome){
            var aluno = _context.Alunos.FirstOrDefault(a => 
            a.Nome.Contains(nome) && a.Sobrenome.Contains(sobrenome));

            if(aluno == null) return BadRequest("O aluno não foi encontrado");

            return Ok(aluno);
        }

        [HttpPost]
        public IActionResult Post(Aluno aluno){
            
            _context.Add(aluno);
            _context.SaveChanges();
            return Ok(aluno);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Aluno aluno){
            
            var alu = _context.Alunos.AsNoTracking().FirstOrDefault(a => a.Id == id);
            
            if(alu == null) return BadRequest("Aluno não encontrado");

            _context.Update(aluno);

            _context.SaveChanges();

            return Ok(aluno);
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Aluno aluno){
            var alu = _context.Alunos.AsNoTracking().FirstOrDefault(a => a.Id == id);
            if(alu == null) return BadRequest("Aluno não encontrado");
            _context.Update(aluno);
            _context.SaveChanges();
            return Ok(aluno);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id){
            var aluno = _context.Alunos.FirstOrDefault(a => a.Id == id);
            if(aluno == null) return BadRequest("Aluno não encontrado");
            _context.Remove(aluno);
            _context.SaveChanges();
            return Ok(aluno);
        }
    }
}