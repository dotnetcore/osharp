using System;


namespace OSharp.Demo.Common.Dtos
{
    public class ModuleSetFunctionDto
    {
        public int ModuleId { get; set; }

        public Guid[] FunctionIds { get; set; }
    }
}