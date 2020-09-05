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
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TodoEntity
    {
        public DateTime? End { get; set; }
        public bool Finished { get; set; }

        [Key] public Guid Id { get; set; }
        public DateTime Start { get; set; }
        [Required] public string Title { get; set; }

        public TodoEntity()
        {
            Id = Guid.NewGuid();
            Start = DateTime.Now;
        }
    }
}