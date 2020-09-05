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
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using YamlDotNet.Serialization;

    public class YamlInputTextInputFormatter : TextInputFormatter
    {
        public YamlInputTextInputFormatter(IDeserializer deserializer) : this(deserializer, Encoding.UTF8, Encoding.Unicode)
        {
        }

        public YamlInputTextInputFormatter(IDeserializer deserializer, params Encoding[] encodings)
        {
            this.ConfigureFormatter(encodings);
            this.deserializer = deserializer;
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            var request = context.HttpContext.Request;

            using (var reader = context.ReaderFactory(request.Body, encoding))
            {
                var type = context.ModelType;

                try
                {
                    var model = deserializer.Deserialize(reader, type);
                    return InputFormatterResult.SuccessAsync(model);
                }
                catch (FormatException)
                {
                    return InputFormatterResult.FailureAsync();
                }
            }
        }

        private readonly IDeserializer deserializer;
    }
}