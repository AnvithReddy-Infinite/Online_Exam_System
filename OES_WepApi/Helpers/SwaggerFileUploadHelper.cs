using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace OES_WepApi.Helpers
{
    public class SwaggerFileUploadHelper : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            // ✅ Apply ONLY to methods marked with [SwaggerFileUpload]
            var hasFileUploadAttribute =
                apiDescription.ActionDescriptor
                    .GetCustomAttributes<SwaggerFileUploadAttribute>()
                    .Any();

            if (!hasFileUploadAttribute)
                return;

            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();

            operation.parameters.Clear();

            operation.parameters.Add(new Parameter
            {
                name = "file",
                @in = "formData",
                required = true,
                type = "file"
            });

            operation.parameters.Add(new Parameter
            {
                name = "techId",
                @in = "query",
                required = true,
                type = "integer"
            });

            operation.parameters.Add(new Parameter
            {
                name = "levelId",
                @in = "query",
                required = true,
                type = "integer"
            });

            operation.consumes = new[] { "multipart/form-data" };
        }
    }
}