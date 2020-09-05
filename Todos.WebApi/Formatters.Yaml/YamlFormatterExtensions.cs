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
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.DependencyInjection;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;

    public static class YamlFormatterExtensions
    {
        public static void ConfigureFormatter(this TextOutputFormatter formatter, Encoding[] encodings)
        {
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            if (encodings == null) throw new ArgumentNullException(nameof(encodings));

            formatter.SupportedMediaTypes.Add(YamlHeadersMediaType.Application);
            formatter.SupportedMediaTypes.Add(YamlHeadersMediaType.Text);

            foreach (var encoding in encodings) formatter.SupportedEncodings.Add(encoding);
        }

        public static void ConfigureFormatter(this TextInputFormatter formatter, Encoding[] encodings)
        {
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            if (encodings == null) throw new ArgumentNullException(nameof(encodings));

            formatter.SupportedMediaTypes.Add(YamlHeadersMediaType.Application);
            formatter.SupportedMediaTypes.Add(YamlHeadersMediaType.Text);

            foreach (var encoding in encodings) formatter.SupportedEncodings.Add(encoding);
        }

        public static void AddYamlFormatter(this IServiceCollection services)
        {
            //Allow IO Serialization Sync
            services
                .Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; })
                .Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });

            services.AddMvc(options =>
            {
                options.FormatterMappings.SetMediaTypeMappingForFormat("yaml", YamlHeadersMediaType.Application);

                var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
                var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

                var output = new YamlOutputTextOutputFormatter(serializer);
                var input = new YamlInputTextInputFormatter(deserializer);

                options.OutputFormatters.Add(output);
                options.InputFormatters.Add(input);
            });
        }
    }
}