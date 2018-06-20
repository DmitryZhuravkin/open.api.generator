using System.Runtime.Serialization;

namespace DZzzz.Swag.Specification.Version12.Model.Declaration
{
    public enum ParamType
    {
        [EnumMember(Value = "path")]
        Path,
        [EnumMember(Value = "query")]
        Query,
        [EnumMember(Value = "body")]
        Body,
        [EnumMember(Value = "header")]
        Header,
        [EnumMember(Value = "form")]
        Form
    }
}