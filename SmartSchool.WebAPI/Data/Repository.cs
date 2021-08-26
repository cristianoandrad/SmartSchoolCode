using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Helpers;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Data
{
    public class Repository : IRepository
    {
        private readonly SmartContext _context;

        public Repository(SmartContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
             _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0);
        }

        public async Task<PageList<Aluno>> GetAllAlunosAsync(PageParams pageParams, bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if (includeProfessor)
            {
                query = query.Include(ad => ad.AlunoDisciplinas)
                             .ThenInclude(d => d.Disciplina)
                             .ThenInclude(p => p.Professor);
            }

            query = query.AsNoTracking().OrderBy(a => a.Id);

            if (!string.IsNullOrEmpty(pageParams.Nome))
                query = query.Where(aluno => aluno.Nome
                                                  .ToUpper()
                                                  .Contains(pageParams.Nome.ToUpper()) ||
                                             aluno.Sobrenome
                                                  .ToUpper()
                                                  .Contains(pageParams.Nome.ToUpper()));
            if (pageParams.Matricula > 0)
                query = query.Where(aluno => aluno.Matricula == pageParams.Matricula);

            if (pageParams.Ativo != null)
                query = query.Where(aluno => aluno.Ativo == (pageParams.Ativo != 0));

            //return await query.ToListAsync();
            return await PageList<Aluno>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public Aluno[] GetAllAlunos(bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if(includeProfessor) {
                query = query.Include(ad => ad.AlunoDisciplinas)
                             .ThenInclude(d => d.Disciplina)          
                             .ThenInclude(p => p.Professor);   
            }

            query = query.AsNoTracking().OrderBy(a => a.Id);
            
            return query.ToArray();
        }

        public Aluno[] GetAllAlunosByDisciplina(int disciplinaid, bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if(includeProfessor) {
                query = query.Include(ad => ad.AlunoDisciplinas)
                             .ThenInclude(d => d.Disciplina)          
                             .ThenInclude(p => p.Professor);   
            }

            query = query.AsNoTracking()
                         .OrderBy(a => a.Id)
                         .Where(aluno => aluno.AlunoDisciplinas.Any(ad => ad.DisciplinaId == disciplinaid));
            
            return query.ToArray();
        }

        public Aluno GetAlunoById(int alunoId, bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _context.Alunos;

            if(includeProfessor) {
                query = query.Include(ad => ad.AlunoDisciplinas)
                             .ThenInclude(d => d.Disciplina)          
                             .ThenInclude(p => p.Professor);   
            }

            query = query.AsNoTracking()
                         .OrderBy(a => a.Id)
                         .Where(aluno => aluno.Id == alunoId);
            
            return query.FirstOrDefault();
        }

        public Professor[] GetAllProfessores(bool includeAlunos = false)
        {
            IQueryable<Professor> query = _context.Professores;

            if(includeAlunos) {
                query = query.Include(d => d.Disciplinas)
                             .ThenInclude(ad => ad.AlunoDisciplinas)          
                             .ThenInclude(a => a.Aluno);   
            }

            query = query.AsNoTracking().OrderBy(a => a.Id);
            
            return query.ToArray();
        }

        public Professor[] GetAllProfessoresByDisciplina(int disciplinaid, bool includeAlunos = false)
        {
            IQueryable<Professor> query = _context.Professores;

            if(includeAlunos) {
                query = query.Include(d => d.Disciplinas) 
                             .ThenInclude(ad => ad.AlunoDisciplinas)          
                             .ThenInclude(a => a.Aluno);   
            }

            query = query.AsNoTracking()
                         .OrderBy(aluno => aluno.Id)
                         .Where(aluno => aluno.Disciplinas.Any(
                             d => d.AlunoDisciplinas.Any(ad => ad.DisciplinaId == disciplinaid )));
            
            return query.ToArray();
        }

        public Professor GetProfessorById(int professorId, bool includeProfessor = false)
        {
            IQueryable<Professor> query = _context.Professores;

            if(includeProfessor) {
                query = query.Include(d => d.Disciplinas) 
                             .ThenInclude(ad => ad.AlunoDisciplinas)          
                             .ThenInclude(a => a.Aluno);    
            }

            query = query.AsNoTracking()
                         .OrderBy(a => a.Id)
                         .Where(professor => professor.Id == professorId);
            
            return query.FirstOrDefault();
        }
    }
}