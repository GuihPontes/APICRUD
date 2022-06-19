using API.Data;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("v1/usuario")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        [Authorize(Roles ="menager")]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
        {
            var users = await context.Users.AsNoTracking().ToListAsync();
            return Ok(users); 
        }



        [HttpPost]
        [Route("")]
        [AllowAnonymous]
       // [Authorize(Roles ="menager")]

        public async Task<ActionResult<User>> Post(
            [FromBody]User model,
            [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var user = await context.Users.AsNoTracking().
          Where(x => x.Username == model.Username)
          .FirstOrDefaultAsync();
             
            try
            {
                
                    context.Users.Add(model);
               await context.SaveChangesAsync();
                return Ok(model);

            }
            
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar usuario no banco" });
            }
            
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate(
            [FromBody] User model,
            [FromServices] DataContext context)
        {
            var user = await context.Users.AsNoTracking().
                Where(x => x.Username == model.Username && x.Password == model.Password)
                .FirstOrDefaultAsync();
        
            if (user == null)
                return NotFound(new { message = "Usuario ou senha inválida" });

            var token = TokenService.GenerateToken(user);
            return new
            {
                user = user,
                token = token
            };
        }


        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles ="menager")]
        public async Task<ActionResult<User>> Put([FromBody] User model,int id , [FromServices] DataContext context)
        {
            if (id != model.Id)
                return NotFound(new { mesage = "Usuario não encontrado" });
            if (!ModelState.IsValid)
                return NotFound(ModelState);

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model; 
            }
            catch(Exception)
            {
                return BadRequest(new { mesage = "Não foi possível atualizar o usuario" });
            }
        }

    }
}
