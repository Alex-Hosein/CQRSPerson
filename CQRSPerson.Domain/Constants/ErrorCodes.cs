using System;
using System.Collections.Generic;
using System.Text;

namespace CQRSPerson.Domain.Constants
{
    public static class ErrorCodes
    {
        public const string GetAllPersonsErrorCode = "GET_ALL_PERSONS_FAILED";
        public const string AddPersonErrorCode = "ADD_PERSON_FAILED";
        public const string FirstNameInvalid = "FIRST_NAME_INVALID";
        public const string LastNameInvalid = "LAST_NAME_INVALID";
        public const string InterestsInvalid = "INTERESTS_INVALID";
        public const string ImageInvalid = "IMAGE_INVALID";
        public const string AgeInvalid = "AGE_INVALID";
    }
}
