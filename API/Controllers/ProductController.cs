using API.Data;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("v1/Produto")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            var product = await context.Products.Include(x=>x.Category).AsNoTracking().ToListAsync();
           return Ok(product);
        }

        [HttpGet]
        [Route("{id}")]

        public async Task<ActionResult<List<Product>>> GetById(int id, [FromServices] DataContext context)
        {
            try
            {
                var product = await context.Products.Include(x=>x.Category).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                return Ok(product);
            }

            catch
            {
                return BadRequest(new { mesage = "Produto não encontrada em nosso sistema" });
            }
        }
        [HttpGet]
        [Route("{id}/categoria")]
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
        {
            var products = await context.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => x.CategoriaId == id)
                .ToListAsync();
            return Ok(products);
        }


        [HttpPost]
     [Route("")]
         public async Task<ActionResult<List<Product>>>Post([FromBody]Product product, [FromServices]DataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                context.Products.Add(product);
                await context.SaveChangesAsync();
                return Ok(product);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar a Produto" });
            }
        }


        [HttpPut]
        [Route("{id}:int")]
        public async Task<ActionResult<List<Product>>> Put(int id,
            [FromBody] Product model,
            [FromServices] DataContext context
            )
        {
            if (id != model.Id)
                return NotFound(new { message = "Produto não encontrada " });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Product>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro já está sendo atualizado" });
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível atualizar o Produto" });

            }
        }

        [HttpDelete]
        [Route("{id}:int")]

        public async Task<ActionResult<List<Product>>> Delete(int id, [FromServices] DataContext context)
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
                return NotFound(new { message = "Produto não encontrada" });

            try
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return Ok(new { mesage = "Produto removida com sucesso" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível remover a categoria" });
            }
        }
    }
}
