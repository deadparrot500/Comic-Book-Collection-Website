using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace WebApplication.Web.Providers.Auth
{
    /// <summary>
    /// The hash provider provides functionality to hash a plain text password and validate 
    /// an existing password against its hash.
    /// </summary>
    public class HashProvider
    {
        private const int WorkFactor = 10000;
        
        /// <summary>
        /// Hashes a plain text password.
        /// </summary>
        /// <param name="plainTextPassword"></param>
        /// <returns></returns>
        public HashedPassword HashPassword(string plainTextPassword)
        {
            //Create the hashing provider
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(plainTextPassword, 8, WorkFactor);

            //Get the Hashed Password
            byte[] hash = rfc.GetBytes(20);

            //Set the SaltValue 
            string salt = Convert.ToBase64String(rfc.Salt);

            //Return the Hashed Password
            return new HashedPassword(Convert.ToBase64String(hash), salt);
        }

        /// <summary>
        /// Verifies if an existing hashed password matches a plaintext password (+salt).
        /// </summary>
        /// <param name="existingHashedPassword">The password we are comparing to.</param>
        /// <param name="plainTextPassword">The plaintext password being validated.</param>
        /// <param name="salt">The salt used to get the existing hashed password.</param>
        /// <returns></returns>
        public bool VerifyPasswordMatch(string existingHashedPassword, string plainTextPassword, string salt)
        {
            byte[] saltArray = Convert.FromBase64String(salt);      //gets us the byte[] array representation

            //Create the hashing provider
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(plainTextPassword, saltArray, WorkFactor);

            //Get the hashed password
            byte[] hash = rfc.GetBytes(20);

            //Compare the hashed password values
            string newHashedPassword = Convert.ToBase64String(hash);

            return (existingHashedPassword == newHashedPassword);
        }
    }

    public class HashedPassword
    {
        /// <summary>
        /// Creates a new hashed password.
        /// </summary>
        /// <param name="password">The hashed password</param>
        /// <param name="salt">The salt used to get the hashed password.</param>
        public HashedPassword(string password, string salt)
        {
            this.Password = password;
            this.Salt = salt;
        }

        /// <summary>
        /// The hashed password
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// The salt used to get the hashed password.
        /// </summary>
        public string Salt { get; }
    }

}
