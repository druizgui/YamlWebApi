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
    using Microsoft.Net.Http.Headers;

    public static class YamlHeadersMediaType
    {
        private const string ApplicationYaml = "application/x-yaml";
        private const string TextYaml = "text/yaml";

        public static readonly MediaTypeHeaderValue Application = MediaTypeHeaderValue.Parse(ApplicationYaml).CopyAsReadOnly();
        public static readonly MediaTypeHeaderValue Text = MediaTypeHeaderValue.Parse(TextYaml).CopyAsReadOnly();
    }
}