using System.Runtime.Serialization;

namespace DZzzz.Swag.Specification.Version12.Model.Declaration
{
    public enum Method
    {
        [EnumMember(Value = "GET")]
        Get,
        [EnumMember(Value = "HEAD")]
        Head,
        [EnumMember(Value = "POST")]
        Post,
        [EnumMember(Value = "PUT")]
        Put,
        [EnumMember(Value = "PATCH")]
        Patch,
        [EnumMember(Value = "DELETE")]
        Delete,
        [EnumMember(Value = "OPTIONS")]
        Options
    }
}