// -----------------------------------------------------------------
// <copyright>Copyright (C) 2020, David Ruiz.</copyright>
// Licensed under the Apache License, Version 2.0.
// You may not use this file except in compliance with the License:
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Software is distributed on an "AS IS", WITHOUT WARRANTIES
// OR CONDITIONS OF ANY KIND, either express or implied.
// -----------------------------------------------------------------

namespace Todos.WebApi.Formatters.Yaml
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using YamlDotNet.Serialization;

    public class YamlOutputTextOutputFormatter : TextOutputFormatter
    {
        public YamlOutputTextOutputFormatter(ISerializer serializer) : this(serializer, Encoding.UTF8, Encoding.Unicode)
        {
        }

        public YamlOutputTextOutputFormatter(ISerializer serializer, params Encoding[] encodings)
        {
            this.ConfigureFormatter(encodings);
            this.serializer = serializer;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (selectedEncoding == null) throw new ArgumentNullException(nameof(selectedEncoding));

            var response = context.HttpContext.Response;
            using (var writer = context.WriterFactory(response.Body, selectedEncoding))
            {
                WriteObject(writer, context.Object);
                await writer.FlushAsync().ConfigureAwait(false);
            }
        }

        private void WriteObject(TextWriter writer, object value)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            serializer.Serialize(writer, value);
        }

        private readonly ISerializer serializer;
    }
}