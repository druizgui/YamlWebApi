// -----------------------------------------------------------------
// <copyright>Copyright (C) 2020, David Ruiz.</copyright>
// Licensed under the Apache License, Version 2.0.
// You may not use this file except in compliance with the License:
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Software is distributed on an "AS IS", WITHOUT WARRANTIES
// OR CONDITIONS OF ANY KIND, either express or implied.
// -----------------------------------------------------------------

namespace Todos.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        public TodoController(TodosContext context)
        {
            this.context = context;
        }

        // GET: api/TodoEntities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoEntity>>> GetTodoEntities()
        {
            return await context.TodoEntities.ToListAsync();
        }

        // GET: api/TodoEntities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoEntity>> GetTodoEntity(Guid id)
        {
            var todoEntity = await context.TodoEntities.FindAsync(id);

            if (todoEntity == null) return NotFound();

            return todoEntity;
        }

        // PUT: api/TodoEntities/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoEntity(Guid id, TodoEntity todoEntity)
        {
            if (id != todoEntity.Id) return BadRequest();

            context.Entry(todoEntity).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoEntityExists(id)) return NotFound();

                throw;
            }

            return NoContent();
        }

        // POST: api/TodoEntities
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TodoEntity>> PostTodoEntity(TodoEntity todoEntity)
        {
            context.TodoEntities.Add(todoEntity);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetTodoEntity", new { id = todoEntity.Id }, todoEntity);
        }

        // DELETE: api/TodoEntities/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoEntity>> DeleteTodoEntity(Guid id)
        {
            var todoEntity = await context.TodoEntities.FindAsync(id);
            if (todoEntity == null) return NotFound();

            context.TodoEntities.Remove(todoEntity);
            await context.SaveChangesAsync();

            return todoEntity;
        }

        private bool TodoEntityExists(Guid id)
        {
            return context.TodoEntities.Any(e => e.Id == id);
        }

        private readonly TodosContext context;
    }
}