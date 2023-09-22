using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Security.Policy;

namespace Laboratorio_ED2_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Operations operations = new Operations();
            try
            {
                Console.WriteLine("RECOMENDACIONES:\n");
                Console.WriteLine("Al momento de ingresar las personas en el archivo se debe seguir el siguiente formato: \n Operación (INSERT, PATCH, DELETE); {'variable':'valor'}\n" +
                    "No agregar más de una persona con el mismo DPI, ya que no puede repetirse.\n Para asegurar el correcto funcionamiento, al usar la operación INSERT se deben llenar todos los parámetros" +
                    "\n Al usar la operación DELETE, solo es necesario el nombre y principalmente el DPI.\n");
                Console.WriteLine("¿Qué desea hacer? \n 1. CARGAR EL ARBOL \n 2. BUSCAR PERSONA \n 3.MOSTRAR COMPAÑIAS CIFRADAS \n 4.MOSTRAR COMPAÑIAS DESCIFRADAS\n");
                string opcion = Console.ReadLine().Trim();
                while (opcion != "0")
                {
                    switch (opcion)
                    {
                        case "1":
                            Operations.leerArchivo();
                            Console.WriteLine("¿Qué desea hacer? \n 1. CARGAR EL ARBOL \n 2. BUSCAR PERSONA \n 3.MOSTRAR COMPAÑIAS CIFRADAS \n 4.MOSTRAR COMPAÑIAS DESCIFRADAS\n");
                            opcion = Console.ReadLine().Trim();
                            break;
                        case "2":
                            Busqueda();
                            Console.WriteLine("¿Qué desea hacer? \n 1. CARGAR EL ARBOL \n 2. BUSCAR PERSONA \n 3.MOSTRAR COMPAÑIAS CIFRADAS \n 4.MOSTRAR COMPAÑIAS DESCIFRADAS\n");
                            opcion = Console.ReadLine().Trim();
                            break;
                        case "3":
                            BusquedaCompresion();
                            Console.WriteLine("¿Qué desea hacer? \n 1. CARGAR EL ARBOL \n 2. BUSCAR PERSONA \n 3.MOSTRAR COMPAÑIAS CIFRADAS \n 4.MOSTRAR COMPAÑIAS DESCIFRADAS\n");
                            opcion = Console.ReadLine().Trim();
                            break;
                        case "4":
                            BusquedaDescompresion();
                            Console.WriteLine("¿Qué desea hacer? \n 1. CARGAR EL ARBOL \n 2. BUSCAR PERSONA \n 3.MOSTRAR COMPAÑIAS CIFRADAS \n 4.MOSTRAR COMPAÑIAS DESCIFRADAS\n");
                            opcion = Console.ReadLine().Trim();
                            break;
                        default:
                            Console.WriteLine("Ninguna opcion elegida");
                            break;
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            

            

            Console.ReadKey();
        }
        public static void Busqueda()
        {
            try
            {
                Console.WriteLine("Escriba el DPI de la persona que desea buscar: ");
                string dpi_buscar =  Console.ReadLine().Trim();
                long dpi_convertido = Convert.ToInt64(dpi_buscar);

                Console.WriteLine("Se esta haciendo su busqueda...\n");
                Console.WriteLine(Operations.personaBuscada(dpi_convertido));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public static void BusquedaCompresion()
        {
            try
            {
                Console.WriteLine("Escriba el DPI de la persona que desea buscar: "); 
                string dpi_buscar = Console.ReadLine().Trim();
                long dpi_convertido = Convert.ToInt64(dpi_buscar);

                Console.WriteLine("Se esta haciendo su busqueda...\n");
                Console.WriteLine(Operations.MostrarCompresion(dpi_convertido));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public static void BusquedaDescompresion()
        {
            try
            {
                Console.WriteLine("Escriba el DPI de la persona que desea buscar: ");
                string dpi_buscar = Console.ReadLine().Trim();
                long dpi_convertido = Convert.ToInt64(dpi_buscar);

                Console.WriteLine("Se esta haciendo su busqueda...\n");
                Console.WriteLine(Operations.MostrarDescompresion(dpi_convertido));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
