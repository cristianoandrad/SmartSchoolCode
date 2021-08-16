using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunoController : ControllerBase
    {
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
        };
        public AlunoController(){}

        [HttpGet]
        public IActionResult Get(){
            return Ok(Alunos);
        }

        [HttpGet("byId/{id}")] //exemplo alterando rota
        //[HttpGet("byId")] //exemplo queryString http://localhost:5000/api/aluno/byId?id=1 
        //[HttpGet("{Id:int}")] //exemplo definindo tipo
        public IActionResult GetById(int id){
            var aluno = Alunos.FirstOrDefault(a => a.Id == id);

            if(aluno == null) return BadRequest("O aluno não foi encontrado");

            return Ok(aluno);
        }

        [HttpGet("byName")] // http://localhost:5000/api/aluno/byName?nome=Marta&sobrenome=Kent
        public IActionResult GetByName(string nome, string sobrenome){
            var aluno = Alunos.FirstOrDefault(a => 
            a.Nome.Contains(nome) && a.Sobrenome.Contains(sobrenome));

            if(aluno == null) return BadRequest("O aluno não foi encontrado");

            return Ok(aluno);
        }

        [HttpPost]
        public IActionResult Post(Aluno aluno){
            return Ok(aluno);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Aluno aluno){
            return Ok(aluno);
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Aluno aluno){
            return Ok(aluno);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id){
            return Ok();
        }
    }
}