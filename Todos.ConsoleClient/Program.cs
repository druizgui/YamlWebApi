// -----------------------------------------------------------------
// <copyright>Copyright (C) 2020, David Ruiz.</copyright>
// Licensed under the Apache License, Version 2.0.
// You may not use this file except in compliance with the License:
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Software is distributed on an "AS IS", WITHOUT WARRANTIES
// OR CONDITIONS OF ANY KIND, either express or implied.
// -----------------------------------------------------------------

namespace Todos.ConsoleClient
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Bogus;
    using Todos.Apiclient.TodoService;

    class Program
    {
        static void Main(string[] args)
        {
            //Wait for service
            Thread.Sleep(3000);

            var client = new Client("https://localhost:44335", new HttpClient());

            var tasks = new List<Task>();

            var faker = new Faker();
            var ids = new BlockingCollection<Guid>();

            for (var i = 0; i < 10; i++)
            {
                var item = new TodoEntity
                {
                    Title = faker.Random.Words(2),
                    Finished = faker.Random.Bool(),
                    Start = faker.Date.Between(DateTime.Now.AddDays(-20), DateTime.Now)
                };
                Console.WriteLine($"Item {i,3}: {item.Title}");
                tasks.Add(client.ApiTodoPostAsync(item).ContinueWith(r =>
                {
                    if (r.IsCompletedSuccessfully)
                    {
                        ids.Add(r.Result.Id);
                    }
                    else
                    {
                        Console.WriteLine($"Http POST Error: {r.Status}");
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
            tasks.Clear();

            foreach (var id in ids)
            {
                tasks.Add(client.ApiTodoGetAsync(id)
                    .ContinueWith(response =>
                    {
                        var item = response.Result;

                        if (response.IsCompletedSuccessfully)
                        {
                            Console.WriteLine($"[GET] Item {item.Id,35}: {item.Title}");
                            item.Title = item.Title + " updated";

                            tasks.Add(client.ApiTodoPutAsync(item.Id, item).ContinueWith(r =>
                            {
                                if (r.IsCompletedSuccessfully)
                                {
                                    Console.WriteLine($"[PUT] Item {item.Id,35}: {item.Title}");
                                }
                                else
                                {
                                    Console.WriteLine($"Http PUT Error: {r.Status}");
                                }
                            }));
                        }
                        else
                        {
                            Console.WriteLine($"Http GET[id] Error: {response.Status}");
                        }
                    }));
            }

            var items = client.ApiTodoGetAsync().Result;
            Task.WaitAll(tasks.ToArray());
            tasks.Clear();

            Console.WriteLine("List Items:");

            foreach (var item in items)
            {
                Console.WriteLine($"{item.Id,35} {item.Start.Date:d} {item.Finished,-5} {item.Title}");
            }
        }
    }
}