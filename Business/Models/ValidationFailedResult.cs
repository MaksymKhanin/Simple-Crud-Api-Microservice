// This code is under Copyright (C) 2021 of Cegid SAS all right reserved

namespace Business.Models
{
    public class ValidationFailedResult
    {
        public string[] ValidationFailures { get; set; }

        public ValidationFailedResult(string[] validationFailures)
        {
            ValidationFailures = validationFailures;
        }
    }
}
