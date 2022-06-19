using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("v1/categorias")]

    public class CategoryController : ControllerBase
    {
        
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        {
            var category = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(category);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context)
        {
            try
            {
                var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                return Ok(category);
            }
            
            catch
            {
                return BadRequest(new { mesage = "Categoria não encontrada em nosso sistema" });
            }

        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Post(
            [FromBody] Category model,
            [FromServices]DataContext context
        )
        { 
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch(Exception)
            {
                return BadRequest(new { message = "Não foi possível criar a catagoria" });
            }
        }

        [HttpPut] 
        [Route("{id}:int")]
        public async Task<ActionResult<List<Category>>>Put(int id,
            [FromBody]Category model,
            [FromServices] DataContext context
            )
        {
            if(id != model.Id)
                return NotFound(new { message = "Categoria não encontrada " });
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch(DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro já está sendo atualizado" });
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível atualizar o categoria" });

            }
        }
        
        [HttpDelete]
        [Route("{id}:int")]
        public async Task<ActionResult<List<Category>>> Delete(int id, [FromServices] DataContext context)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(new { message = "Categoria não encontrada" });

            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(new { mesage="Categoria removida com sucesso"});
            }
            catch(Exception)
            {
                return BadRequest(new { message = "Não foi possível remover a categoria" });
            }
        }
    }
}
