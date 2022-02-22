// This code is under Copyright (C) 2021 of Cegid SAS all right reserved

namespace Business.Models
{
    public record PayloadData(string Number, DateTime IssuedAt, string Format, NestedObject NestedObject);
    public record NestedObject(string Number, string? Name);


    public class PayloadDataType1
    {
        public NestedObject GetNestedObject() => new NestedObject("number", "name");

        private string someNumberField;

        public string SomeNumber
        {
            get { return someNumberField; }
            set { someNumberField = value; }
        }


    }
}
