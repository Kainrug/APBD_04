﻿using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (IsFirstNameCorrect(firstName) || IsLastNameCorrect(lastName))
            {
                return false;
            }

            if (IsUserEmailCorrect(email))
            {
                return false;
            }

            var age = CalculateAgeUsingBirthdate(dateOfBirth);

            if (IsAgeCorrect(age))
            {
                return false;
            }

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
                }
            }
            else
            {
                user.HasCreditLimit = true;
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }

        private static int CalculateAgeUsingBirthdate(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;
            return age;
        }

        private static bool IsAgeCorrect(int age)
        {
            return age < 21;
        }

        private static bool IsUserEmailCorrect(string email)
        {
            return !email.Contains("@") && !email.Contains(".");
        }

        private static bool IsLastNameCorrect(string lastName)
        {
            return string.IsNullOrEmpty(lastName);
        }

        private static bool IsFirstNameCorrect(string firstName)
        {
            return string.IsNullOrEmpty(firstName);
        }
    }
}
