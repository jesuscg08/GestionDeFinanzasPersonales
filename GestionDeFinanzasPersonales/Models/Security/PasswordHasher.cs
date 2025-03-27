using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace GestionDeFinanzasPersonales.Models.Security
{
    public static class PasswordHasher
    {
        //Tamaño Salt en bytes
        private const int SaltSize = 16;
        //Tamaño Hash en bytes
        private const int HashSize = 20;
        //Iteraciones
        private const int Iterations = 10000;

        //Método para la creación del hash
        public static string HashClave(string clave) {
            //Salt aleatorio
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            //Crear el hash
            var pbkdf2 = new Rfc2898DeriveBytes(clave, salt, Iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            //Combinación de salt y hash
            var hashBytes= new byte[SaltSize+HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            //Convertir a base64
            var base64Hash= Convert.ToBase64String(hashBytes);

            return base64Hash;

        }

        public static bool VerificarClave(string clave, string hashedClave)
        {
            //Hash base64-> bytes
            var hashBytes= Convert.FromBase64String(hashedClave);

            //Extraer salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            //Crear el hash con el salt extraido
            var pbkdf2 = new Rfc2898DeriveBytes(clave, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Comparar los hashes
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        
        }
    }

    
}