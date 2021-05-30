using SK.Domain.Enums;
using System;

namespace SK.Application.Common.Helpers
{
    public static class ConvertHelper
    {
        public static string ConvertTypeOfFieldEnumToStringType(TypeOfField typeOfField)
        {
            return typeOfField == TypeOfField.Number ? "int" : "string";
        }

        public static TypeOfField ConvertStringTypeToTypeOfFieldenum(string stringType)
        {
            return stringType == "int" ? TypeOfField.Number : (stringType == "string" ? TypeOfField.Text : throw new ArgumentOutOfRangeException());
        }
    }
}
