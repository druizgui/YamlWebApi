// -----------------------------------------------------------------
// <copyright>Copyright (C) 2020, David Ruiz.</copyright>
// Licensed under the Apache License, Version 2.0.
// You may not use this file except in compliance with the License:
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Software is distributed on an "AS IS", WITHOUT WARRANTIES
// OR CONDITIONS OF ANY KIND, either express or implied.
// -----------------------------------------------------------------

namespace Todos.WebApi
{
    using Microsoft.EntityFrameworkCore;

    public class TodosContext : DbContext
    {
        public DbSet<TodoEntity> TodoEntities { get; set; }

        public TodosContext(DbContextOptions<TodosContext> options)
            : base(options)
        {
        }
    }
}