using SmartSchool.WebAPI.Helpers;
using SmartSchool.WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSchool.WebAPI.Data
{
    public interface IRepository
    {
         void Add<T>(T entity) where T : class;
         void Update<T>(T entity) where T : class;
         void Delete<T>(T entity) where T : class;
         bool SaveChanges();

        Task<PageList<Aluno>> GetAllAlunosAsync(PageParams pageParams, bool includeProfessor = false);
         public Aluno[] GetAllAlunos(bool includeProfessor = false);
         public Aluno[] GetAllAlunosByDisciplina(int disciplinaid, bool includeProfessor = false);
         public Aluno GetAlunoById(int alunoId, bool includeProfessor = false);
         public Professor[] GetAllProfessores(bool includeAlunos = false);
         public Professor[] GetAllProfessoresByDisciplina(int disciplinaid, bool includeAlunos = false);
         public Professor GetProfessorById(int professorId, bool includeProfessor = false);
         


    }
}