using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using static System.Collections.Specialized.BitVector32;
using Laboratorio_ED2_1;
using Newtonsoft.Json;

namespace Laboratorio_ED2_1
{
    public class Operations
    {
        public static AVL<Persona> arbol = new AVL<Persona>();
        public static void leerArchivo()
        {
            
           string filePath = "C:/Users/Diego/OneDrive/Documentos/Diego/Universidad/5. Cuarto Ciclo/Estructuras II/Laboratorio_ED2_1/datos.txt"; // Reemplaza con la ruta del archivo
            string filePathJsonL = "C:/Users/Diego/OneDrive/Documentos/Diego/Universidad/5. Cuarto Ciclo/Estructuras II/Laboratorio_ED2_1/datosConvertidos.txt";
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length == 2)
                    {
                        string action = parts[0].Trim();
                        string dataJson = parts[1].Trim();

                        Persona person = Newtonsoft.Json.JsonConvert.DeserializeObject<Persona>(dataJson);
                        commandReader(action, person, arbol);
                        
                    }
                }
                GuardarArbolEnJsonl(arbol, filePathJsonL);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al leer el archivo: " + ex.Message);
            }
        }

        public static void commandReader(string action, Persona  person, AVL<Persona> arbol)
        {
            if (action == "INSERT")
            {
                try
                {
                    arbol.Add(person);
                    //Console.WriteLine("Insert hecho");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("El error es " + ex);
                    throw;
                }


            }
            else if (action == "DELETE")
            {
                try
                {
                    arbol.Remove(person);
                    //Console.WriteLine("Delete hecho");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("El error es " + ex);
                    throw;
                }

            }
            else if (action == "PATCH")
            {
                try
                {
                    arbol.Update(person, person.DPI);
                    //Console.WriteLine("Patch hecho");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("El error es "+ ex);
                    throw;
                }
                
            }
        }

        public static void GuardarArbolEnJsonl(AVL<Persona> arbol, string filePath)
        {
            try
            {
                List<string> jsonLines = new List<string>();

                List<Persona> elementos = arbol.ObtenerLista(); // Cambia esto según el nombre de tu método

                foreach (var persona in elementos)
                {
                    string jsonData = JsonConvert.SerializeObject(persona);
                    jsonLines.Add($"{jsonData}");
                }

                File.WriteAllLines(filePath, jsonLines);
                Console.WriteLine($"\nÁrbol guardado en '{filePath}'\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar el árbol en el archivo JSONL: " + ex.Message);
            }
        }


        public static string personaBuscada(long dpiABuscar)
        {
            Nodo<Persona> nodoEncontrado = arbol.GetByDPI(dpiABuscar);

            if (nodoEncontrado != null)
            {
                Persona personaEncontrada = nodoEncontrado.elvalor;
                return ($"DPI: {personaEncontrada.DPI} \nNombre: {personaEncontrada.name} \nNacimiento: {personaEncontrada.datebirth} \nDireccion: {personaEncontrada.address}\n");
            }
            else
            {
                return ($"No se encontró un nodo con DPI: {dpiABuscar}");
            }
            
        }




    }
}
